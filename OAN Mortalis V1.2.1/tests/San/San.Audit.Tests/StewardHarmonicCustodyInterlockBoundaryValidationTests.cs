using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class StewardHarmonicCustodyInterlockBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-14T00:00:00Z");

    [Fact]
    public void Two_Lawful_Signals_May_Still_Require_Steward_Interlock()
    {
        var receipt = Declare(CreateRequest(HarmonicInterlockOutcome.Sequence));

        Assert.Equal(StewardHarmonicCustodyInterlockDisposition.SequenceReviewCold, receipt.Disposition);
        Assert.Equal("steward-harmonic-interlock-sequence-review-only", receipt.OutcomeCode);
        Assert.Equal(2, receipt.Signals.Count);
        Assert.True(receipt.ContentionReceipt?.IsColdContentionReceipt);
        Assert.False(receipt.LocalLawfulnessBecomesSharedComposability);
        AssertCold(receipt);
    }

    [Fact]
    public void Shared_Surface_Interlock_Requires_Steward()
    {
        var receipt = Declare(CreateRequest(
            surface: CreateSharedSurface(custodyOwner: "Prime")));

        AssertRefused(receipt, "steward-interlock-shared-surface-not-steward-governed");
    }

    [Fact]
    public void Align_Does_Not_Authorize_Admit_Or_Act()
    {
        var receipt = Declare(CreateRequest(HarmonicInterlockOutcome.Align));

        Assert.Equal(StewardHarmonicCustodyInterlockDisposition.AlignReviewCold, receipt.Disposition);
        Assert.False(receipt.AlignmentAdmits);
        Assert.False(receipt.InterlockGrantsAuthority);
        Assert.False(receipt.RuntimeActionAllowed);
        AssertCold(receipt);
    }

    [Fact]
    public void Sequence_Does_Not_Punish_Or_Admit()
    {
        var receipt = Declare(CreateRequest(HarmonicInterlockOutcome.Sequence));

        Assert.Equal(StewardHarmonicCustodyInterlockDisposition.SequenceReviewCold, receipt.Disposition);
        Assert.False(receipt.SequencePunishes);
        Assert.False(receipt.ContinuityAdmitted);
        AssertCold(receipt);
    }

    [Fact]
    public void Damp_Does_Not_Erase_Witness()
    {
        var receipt = Declare(CreateRequest(HarmonicInterlockOutcome.Damp));

        Assert.Equal(StewardHarmonicCustodyInterlockDisposition.DampReviewCold, receipt.Disposition);
        Assert.False(receipt.DampingErasesWitness);
        Assert.False(receipt.DampingPolicy.AllowsWitnessErasure);
        AssertCold(receipt);
    }

    [Fact]
    public void Split_Preserves_Custody()
    {
        var receipt = Declare(CreateRequest(HarmonicInterlockOutcome.Split));

        Assert.Equal(StewardHarmonicCustodyInterlockDisposition.SplitReviewCold, receipt.Disposition);
        Assert.False(receipt.SplitFragmentsCustody);
        Assert.True(receipt.SplitRoute.PreservesCustody);
        Assert.True(receipt.SplitRoute.PreservesOriginalSignalHandles);
        AssertCold(receipt);
    }

    [Fact]
    public void Cool_Does_Not_Mean_Failure()
    {
        var receipt = Declare(CreateRequest(HarmonicInterlockOutcome.Cool));

        Assert.Equal(StewardHarmonicCustodyInterlockDisposition.CoolReviewCold, receipt.Disposition);
        Assert.False(receipt.CoolingMeansFailure);
        AssertCold(receipt);
    }

    [Fact]
    public void Refuse_Retains_Contention_Evidence_Without_Permission()
    {
        var receipt = Declare(CreateRequest(HarmonicInterlockOutcome.Refuse));

        Assert.Equal(StewardHarmonicCustodyInterlockDisposition.RefusalReviewCold, receipt.Disposition);
        Assert.Equal("steward-harmonic-interlock-contention-refused-review-only", receipt.OutcomeCode);
        Assert.NotNull(receipt.ContentionReceipt);
        Assert.True(receipt.ContentionReceipt!.Retained);
        Assert.False(receipt.ContentionReceipt.GrantsPermission);
        Assert.False(receipt.ReceiptBecomesPermission);
        AssertCold(receipt);
    }

    [Fact]
    public void Interlock_Does_Not_Emit_Packet_Evaluate_Lisp_Activate_Or_Increment_Passage()
    {
        var receipt = Declare(CreateRequest(
            HarmonicInterlockOutcome.Align,
            priorPassageCount: 144));

        Assert.Equal(144, receipt.PriorPassageCount);
        Assert.Equal(144, receipt.PassageCountAfterInterlockReview);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.True(receipt.ActivationRefused);
        AssertCold(receipt);
    }

    [Fact]
    public void Local_Lawfulness_Cannot_Imply_Shared_Surface_Composability()
    {
        var receipt = Declare(CreateRequest(
            boundary: CreateBoundary(localLawfulnessMayImplySharedComposability: true)));

        AssertRefused(receipt, "steward-interlock-non-authority-boundary-promotional");
    }

    [Fact]
    public void Lisp_Body_Declares_Steward_Harmonic_Interlock_As_Inert_Carrier()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "steward-harmonic-custody-interlock.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-steward-harmonic-custody-interlock-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":steward-role :harmonic-custody-interlock-surface", body, StringComparison.Ordinal);
        Assert.Contains(":interlock-outcomes (:align :sequence :damp :split :cool :refuse)", body, StringComparison.Ordinal);
        Assert.Contains(":local-lawfulness-may-imply-shared-composability nil", body, StringComparison.Ordinal);
        Assert.Contains(":interlock-may-authorize nil", body, StringComparison.Ordinal);
        Assert.Contains(":alignment-may-admit nil", body, StringComparison.Ordinal);
        Assert.Contains(":sequence-may-punish nil", body, StringComparison.Ordinal);
        Assert.Contains(":damping-may-erase-witness nil", body, StringComparison.Ordinal);
        Assert.Contains(":split-may-fragment-custody nil", body, StringComparison.Ordinal);
        Assert.Contains(":cooling-may-mean-failure nil", body, StringComparison.Ordinal);
        Assert.Contains(":contention-may-activate nil", body, StringComparison.Ordinal);
        Assert.Contains(":receipt-may-permit nil", body, StringComparison.Ordinal);
        Assert.Contains(":steward-may-own-meaning nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static StewardHarmonicCustodyInterlockReceipt Declare(
        StewardHarmonicCustodyInterlockRequest request) =>
        new DefaultStewardHarmonicCustodyInterlockBoundaryValidator().Declare(request, TimestampUtc);

    private static StewardHarmonicCustodyInterlockRequest CreateRequest(
        HarmonicInterlockOutcome outcome = HarmonicInterlockOutcome.Align,
        CmeLispResonanceHeartbeatReceipt? source = null,
        IReadOnlyList<LawfulSignalCandidate>? signals = null,
        SharedSymbolicSurface? surface = null,
        StewardInterlockHeartbeatWindow? heartbeat = null,
        CadenceAlignmentPolicy? cadence = null,
        DampingBackoffPolicy? damping = null,
        WitnessSurfaceSplitRoute? split = null,
        StewardInterlockNonAuthorityBoundary? boundary = null,
        int priorPassageCount = 55)
    {
        var sourceReceipt = source ?? CreateSourceResonance();
        var sharedSurface = surface ?? CreateSharedSurface();
        return new StewardHarmonicCustodyInterlockRequest(
            SourceResonanceReceipt: sourceReceipt,
            Signals: signals ?? CreateSignals(sourceReceipt.ReceiptHandle, sharedSurface.SurfaceHandle),
            SharedSurface: sharedSurface,
            HeartbeatWindow: heartbeat ?? CreateHeartbeat(),
            RequestedOutcome: outcome,
            CadencePolicy: cadence ?? CreateCadence(),
            DampingPolicy: damping ?? CreateDamping(),
            SplitRoute: split ?? CreateSplit(),
            Boundary: boundary ?? CreateBoundary(),
            PriorPassageCount: priorPassageCount);
    }

    private static CmeLispResonanceHeartbeatReceipt CreateSourceResonance() =>
        new(
            ReceiptHandle: "urn:san:cme-lisp-resonance-heartbeat:review:fixture",
            Disposition: CmeLispResonanceHeartbeatDisposition.DampingReviewCold,
            OutcomeCode: "cme-lisp-damping-resonance-review-only",
            GovernanceTrace: "fixture cold resonance heartbeat",
            SourceFretboardReceiptHandle: "urn:san:cme-lisp-thread-fretboard:review:fixture",
            GlobalLaw: new CmeLispGlobalResonanceLaw(
                SoundMayBecomeAction: false,
                ResonanceMayAuthorize: false,
                ResonanceMayAdmitContinuity: false,
                DiscordanceMayBecomeFailure: false,
                DampingMayEraseWitness: false,
                RestMayMeanAbsence: false,
                RepetitionMayBecomeContinuity: false,
                AmplitudeMayBecomeTruth: false,
                BoundaryLaw: "fixture global resonance law"),
            HeartbeatPolicy: new StewardHeartbeatPolicy(
                PolicyCode: "fixture-steward-heartbeat",
                StewardGoverned: true,
                ReviewWindowPresent: true,
                AllowsUngovernedCadence: false,
                AllowsSoundToBypassReview: false,
                AllowsActionWithoutAdmission: false,
                AllowsHeartbeatToOwnResonance: false,
                AllowsHeartbeatToAdmitContinuity: false),
            Emanations: [],
            TouchEvents: [],
            ResonanceEvidence: [],
            DampingProfiles: [],
            DiscordanceRoutes: [],
            ActionAdmissionBoundary: new ActionAdmissionBoundary(
                BoundaryCode: "fixture-action-admission-refused",
                Present: true,
                StewardReviewRequired: true,
                AllowsSoundToBecomeAction: false,
                AllowsAuthority: false,
                AllowsContinuityAdmission: false,
                AllowsPacketEmission: false,
                AllowsLispEvaluation: false,
                IncrementsPassageCount: false),
            Refusal: null,
            PriorPassageCount: 41,
            PassageCountAfterResonanceReview: 41,
            ReviewOnly: true,
            InertOnly: true,
            ListeningFrameMayReceive: true,
            EmanationBecomesAction: false,
            SoundBecomesAuthority: false,
            ResonanceAdmitsContinuity: false,
            DiscordanceBecomesFailure: false,
            DampingErasesWitness: false,
            RestBecomesAbsence: false,
            ThreadTouchEmitsPacket: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);

    private static LawfulSignalCandidate[] CreateSignals(
        string sourceHandle,
        string sharedSurface) =>
    [
        CreateSignal("prime", sourceHandle, "urn:san:cme-lisp-thread:prime-001", sharedSurface, cadence: 1m),
        CreateSignal("cryptic", sourceHandle, "urn:san:cme-lisp-thread:cryptic-001", sharedSurface, cadence: 1.25m)
    ];

    private static LawfulSignalCandidate CreateSignal(
        string name,
        string sourceHandle,
        string threadHandle,
        string sharedSurface,
        decimal cadence) =>
        new(
            SignalHandle: $"urn:san:lawful-signal:{name}",
            SourceReceiptHandle: sourceHandle,
            ThreadHandle: threadHandle,
            SharedSurfaceHandle: sharedSurface,
            CadenceOrdinal: cadence,
            ResonanceAmplitude: 0.42m,
            SharedSurfacePressure: 0.58m,
            LocallyLawful: true,
            ReviewOnly: true,
            Inert: true,
            RequestsSharedSurface: true,
            EmitsPacket: false,
            RequestsRuntimeAction: false,
            ClaimsAuthority: false,
            ClaimsContinuity: false,
            RequestsActivation: false);

    private static SharedSymbolicSurface CreateSharedSurface(string custodyOwner = "Steward") =>
        new(
            SurfaceHandle: "urn:san:shared-symbolic-surface:compass-worktable",
            SurfaceName: "CompassWorktable",
            CustodyOwner: custodyOwner,
            Shared: true,
            WitnessSurfacePresent: true,
            StewardInterlockRequired: true,
            DirectWriteAdmissionAllowed: false,
            GrantsAuthority: false,
            AdmitsContinuity: false,
            AllowsRuntimeAction: false,
            AllowsActivation: false);

    private static StewardInterlockHeartbeatWindow CreateHeartbeat() =>
        new(
            WindowHandle: "urn:san:steward-heartbeat-window:interlock-001",
            StartOrdinal: 10,
            EndOrdinal: 11,
            StewardGoverned: true,
            Bounded: true,
            AllowsUngovernedCoexistence: false,
            AllowsBypass: false,
            AllowsPassageIncrement: false);

    private static CadenceAlignmentPolicy CreateCadence() =>
        new(
            PolicyCode: "compatible-cadence-review-only",
            Present: true,
            CompatibleCadenceRequired: true,
            AllowsAlignmentToAdmit: false,
            AllowsAlignmentToAuthorize: false,
            AllowsUnwitnessedCoexistence: false);

    private static DampingBackoffPolicy CreateDamping() =>
        new(
            PolicyCode: "damp-without-erasure",
            Present: true,
            DampingCoefficient: 0.48m,
            DampsWithoutErasure: true,
            AllowsWitnessErasure: false,
            AllowsAuthority: false,
            AllowsContinuity: false);

    private static WitnessSurfaceSplitRoute CreateSplit() =>
        new(
            RouteCode: "preserve-custody-split-route",
            Present: true,
            PreservesCustody: true,
            PreservesOriginalSignalHandles: true,
            CreatesNewAuthoritySurface: false,
            FragmentsCustody: false,
            EmitsPackets: false);

    private static StewardInterlockNonAuthorityBoundary CreateBoundary(
        bool localLawfulnessMayImplySharedComposability = false) =>
        new(
            BoundaryCode: "steward-interlock-non-authority",
            LocalLawfulnessMayImplySharedComposability: localLawfulnessMayImplySharedComposability,
            InterlockMayAuthorize: false,
            AlignmentMayAdmit: false,
            SequenceMayPunish: false,
            DampingMayEraseWitness: false,
            SplitMayFragmentCustody: false,
            CoolingMayMeanFailure: false,
            ContentionMayActivate: false,
            ReceiptMayPermit: false,
            StewardMayOwnMeaning: false,
            AllowsLispEvaluation: false,
            AllowsRuntimeAction: false,
            AllowsPacketEmission: false,
            AllowsReceiptReplay: false,
            AllowsPassageIncrement: false,
            AllowsContinuity: false,
            AllowsAuthority: false);

    private static void AssertCold(StewardHarmonicCustodyInterlockReceipt receipt)
    {
        Assert.True(receipt.IsColdInterlock);
        Assert.True(receipt.ReviewOnly);
        Assert.True(receipt.InertOnly);
        Assert.True(receipt.StewardInterlockPresent);
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.InterlockGrantsAuthority);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.Null(receipt.Refusal);
    }

    private static void AssertRefused(
        StewardHarmonicCustodyInterlockReceipt receipt,
        string outcomeCode)
    {
        Assert.Equal(StewardHarmonicCustodyInterlockDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedInterlockRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal!.Retained);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "steward-harmonic-custody-interlock.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate OAN Mortalis repository root.");
    }
}
