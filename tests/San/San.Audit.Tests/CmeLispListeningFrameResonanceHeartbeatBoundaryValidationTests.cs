using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class CmeLispListeningFrameResonanceHeartbeatBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-14T00:00:00Z");

    [Fact]
    public void Listening_Frame_Receives_Emanation_Without_Action()
    {
        var receipt = Declare(CreateRequest(touches: [], evidence: [], damping: [], discordance: []));

        Assert.Equal(CmeLispResonanceHeartbeatDisposition.EmanationReviewCold, receipt.Disposition);
        Assert.Equal("cme-lisp-listening-frame-emanation-review-only", receipt.OutcomeCode);
        Assert.Single(receipt.Emanations);
        Assert.Empty(receipt.TouchEvents);
        AssertCold(receipt);
    }

    [Fact]
    public void Thread_Touch_Does_Not_Create_Action_Packet_Authority_Continuity_Or_Passage()
    {
        var receipt = Declare(CreateRequest(evidence: [], damping: [], discordance: [], priorPassageCount: 77));

        Assert.Equal(CmeLispResonanceHeartbeatDisposition.TouchReviewCold, receipt.Disposition);
        Assert.Equal("cme-lisp-thread-touch-review-only", receipt.OutcomeCode);
        Assert.Equal(77, receipt.PriorPassageCount);
        Assert.Equal(77, receipt.PassageCountAfterResonanceReview);
        Assert.False(receipt.ThreadTouchEmitsPacket);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.SoundBecomesAuthority);
        Assert.False(receipt.ResonanceAdmitsContinuity);
        AssertCold(receipt);
    }

    [Fact]
    public void Damping_And_Discordance_Route_Without_Failure_Erasure_Authority_Or_Continuity()
    {
        var receipt = Declare(CreateRequest());

        Assert.Equal(CmeLispResonanceHeartbeatDisposition.DampingReviewCold, receipt.Disposition);
        Assert.Equal("cme-lisp-damping-resonance-review-only", receipt.OutcomeCode);
        Assert.All(receipt.ResonanceEvidence, evidence => Assert.False(evidence.EvidenceBecomesWarrant));
        Assert.All(receipt.DampingProfiles, damping => Assert.False(damping.ErasesWitness));
        Assert.All(receipt.DiscordanceRoutes, route => Assert.False(route.TreatsDiscordanceAsFailure));
        AssertCold(receipt);
    }

    [Fact]
    public void Rest_Is_Lawful_Non_Action_Not_Absence()
    {
        var touch = CreateTouch("rest-001", CmeLispThreadTouchKind.Rest, "urn:san:cme-lisp-thread:steward-001");
        var receipt = Declare(CreateRequest(touches: [touch], evidence: [], damping: [], discordance: []));

        Assert.Equal(CmeLispResonanceHeartbeatDisposition.RestReviewCold, receipt.Disposition);
        Assert.Equal("cme-lisp-rest-review-only", receipt.OutcomeCode);
        Assert.False(receipt.RestBecomesAbsence);
        AssertCold(receipt);
    }

    [Theory]
    [InlineData("sound-action")]
    [InlineData("resonance-authority")]
    [InlineData("resonance-continuity")]
    [InlineData("discordance-failure")]
    [InlineData("damping-erasure")]
    [InlineData("rest-absence")]
    [InlineData("repetition-continuity")]
    [InlineData("amplitude-truth")]
    public void Global_Resonance_Law_Refuses_Promotional_Meaning(string failureCase)
    {
        var receipt = Declare(CreateRequest(law: failureCase switch
        {
            "sound-action" => CreateLaw(soundMayBecomeAction: true),
            "resonance-authority" => CreateLaw(resonanceMayAuthorize: true),
            "resonance-continuity" => CreateLaw(resonanceMayAdmitContinuity: true),
            "discordance-failure" => CreateLaw(discordanceMayBecomeFailure: true),
            "damping-erasure" => CreateLaw(dampingMayEraseWitness: true),
            "rest-absence" => CreateLaw(restMayMeanAbsence: true),
            "repetition-continuity" => CreateLaw(repetitionMayBecomeContinuity: true),
            "amplitude-truth" => CreateLaw(amplitudeMayBecomeTruth: true),
            _ => CreateLaw()
        }));

        AssertRefused(receipt, "cme-lisp-global-resonance-law-promotional");
    }

    [Theory]
    [InlineData("not-steward")]
    [InlineData("no-window")]
    [InlineData("ungoverned")]
    [InlineData("bypass")]
    [InlineData("action")]
    [InlineData("owns-resonance")]
    [InlineData("continuity")]
    public void Heartbeat_Must_Be_Steward_Governed_Review_Cadence(string failureCase)
    {
        var receipt = Declare(CreateRequest(heartbeat: failureCase switch
        {
            "not-steward" => CreateHeartbeat(stewardGoverned: false),
            "no-window" => CreateHeartbeat(reviewWindowPresent: false),
            "ungoverned" => CreateHeartbeat(allowsUngovernedCadence: true),
            "bypass" => CreateHeartbeat(allowsSoundToBypassReview: true),
            "action" => CreateHeartbeat(allowsActionWithoutAdmission: true),
            "owns-resonance" => CreateHeartbeat(allowsHeartbeatToOwnResonance: true),
            "continuity" => CreateHeartbeat(allowsHeartbeatToAdmitContinuity: true),
            _ => CreateHeartbeat()
        }));

        AssertRefused(receipt, "cme-lisp-heartbeat-not-steward-governed");
    }

    [Fact]
    public void Action_Thread_Touch_Requires_Admission_Boundary_While_Still_Refusing_Action()
    {
        var touch = CreateTouch(
            "action-touch-001",
            CmeLispThreadTouchKind.Pluck,
            "urn:san:cme-lisp-thread:action-001",
            actionAdmissionBoundaryPresent: false);

        var receipt = Declare(CreateRequest(touches: [touch], evidence: [], damping: [], discordance: []));

        AssertRefused(receipt, "cme-lisp-action-thread-touch-without-admission-boundary");
    }

    [Fact]
    public void Evidence_Must_Bind_To_Emanation_And_Touch()
    {
        var evidence = CreateEvidence() with { TouchHandle = "urn:san:lisp-thread-touch:missing" };

        var receipt = Declare(CreateRequest(evidence: [evidence]));

        AssertRefused(receipt, "cme-lisp-resonance-evidence-source-missing");
    }

    [Fact]
    public void Evidence_May_Not_Become_Warrant_Action_Authority_Or_Continuity()
    {
        var evidence = CreateEvidence() with { EvidenceBecomesWarrant = true };

        var receipt = Declare(CreateRequest(evidence: [evidence]));

        AssertRefused(receipt, "cme-lisp-resonance-evidence-promotional-refused");
    }

    [Fact]
    public void Lisp_Body_Declares_Listening_Frame_Resonance_Heartbeat_As_Inert_Carrier()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "listening-frame-resonance-heartbeat.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-lisp-listening-frame-resonance-heartbeat-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":heartbeat-owner :steward", body, StringComparison.Ordinal);
        Assert.Contains(":resonance-law-scope :global", body, StringComparison.Ordinal);
        Assert.Contains(":local-tuning-scope :thread-profile", body, StringComparison.Ordinal);
        Assert.Contains(":sound-may-become-action nil", body, StringComparison.Ordinal);
        Assert.Contains(":resonance-may-authorize nil", body, StringComparison.Ordinal);
        Assert.Contains(":discordance-may-become-failure nil", body, StringComparison.Ordinal);
        Assert.Contains(":damping-may-erase-witness nil", body, StringComparison.Ordinal);
        Assert.Contains(":rest-may-mean-absence nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static CmeLispResonanceHeartbeatReceipt Declare(CmeLispResonanceHeartbeatRequest request) =>
        new DefaultCmeLispListeningFrameResonanceHeartbeatBoundaryValidator().Declare(request, TimestampUtc);

    private static CmeLispResonanceHeartbeatRequest CreateRequest(
        CmeLispThreadFretboardReceipt? source = null,
        CmeLispGlobalResonanceLaw? law = null,
        StewardHeartbeatPolicy? heartbeat = null,
        IReadOnlyList<ListeningFrameEmanationRecord>? emanations = null,
        IReadOnlyList<LispThreadTouchEvent>? touches = null,
        IReadOnlyList<ThreadResonanceEvidence>? evidence = null,
        IReadOnlyList<DampingProfile>? damping = null,
        IReadOnlyList<DiscordanceRoute>? discordance = null,
        ActionAdmissionBoundary? actionAdmission = null,
        int priorPassageCount = 33) =>
        new(
            SourceFretboardReceipt: source ?? CreateSourceFretboard(),
            GlobalLaw: law ?? CreateLaw(),
            HeartbeatPolicy: heartbeat ?? CreateHeartbeat(),
            Emanations: emanations ?? [CreateEmanation()],
            TouchEvents: touches ?? [CreateTouch()],
            ResonanceEvidence: evidence ?? [CreateEvidence()],
            DampingProfiles: damping ?? [CreateDamping()],
            DiscordanceRoutes: discordance ?? [CreateDiscordance()],
            ActionAdmissionBoundary: actionAdmission ?? CreateActionAdmissionBoundary(),
            PriorPassageCount: priorPassageCount);

    private static CmeLispThreadFretboardReceipt CreateSourceFretboard()
    {
        var threads = new[]
        {
            CreateThread("delta", CmeLispThreadKind.Delta),
            CreateThread("witness", CmeLispThreadKind.Witness),
            CreateThread("steward", CmeLispThreadKind.Steward, stewardBoundary: true),
            CreateThread("action", CmeLispThreadKind.Action, stewardBoundary: true)
        };

        return new CmeLispThreadFretboardReceipt(
            ReceiptHandle: "urn:san:cme-lisp-thread-fretboard:review:fixture",
            Disposition: CmeLispThreadFretboardDisposition.StringingReviewCold,
            OutcomeCode: "cme-lisp-thread-stringing-review-only",
            GovernanceTrace: "fixture cold fretboard",
            SourceForkReceiptHandle: "urn:san:ec-participatory-peerless:review:fixture",
            Threads: threads,
            ResonanceCandidates: [],
            Boundary: new CmeLispThreadFretboardLaw(
                PlayableThreadRequiresAnchor: true,
                PlayableThreadRequiresWitness: true,
                PlayableThreadRequiresDamping: true,
                ResonanceRequiresDelta: true,
                ActionThreadRequiresStewardBoundary: true,
                MemoryThreadRequiresWitness: true,
                RepairThreadRequiresFailureClassification: true,
                MeaningThreadMayImpersonateIdentity: false,
                LispEvaluationAllowed: false,
                RuntimeActionAllowed: false,
                PacketEmissionAllowed: false,
                ReceiptReplayAllowed: false,
                PassageMayIncrement: false,
                BoundaryLaw: "fixture cold fretboard boundary"),
            Refusal: null,
            PriorPassageCount: 29,
            PassageCountAfterStringing: 29,
            ReviewOnly: true,
            InertOnly: true,
            WitnessPresent: true,
            SeparateCustody: true,
            ThreadWithoutAnchorAccepted: false,
            ResonanceWithoutDeltaAccepted: false,
            ActionWithoutStewardAccepted: false,
            MemoryWithoutWitnessAccepted: false,
            RepairWithoutFailureAccepted: false,
            MeaningImpersonatesIdentityAccepted: false,
            SemanticBuzzingAccepted: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            LispEvaluationRequested: false,
            RuntimeActionRequested: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);
    }

    private static CmeLispThreadCandidate CreateThread(
        string name,
        CmeLispThreadKind kind,
        bool stewardBoundary = false) =>
        new(
            ThreadHandle: $"urn:san:cme-lisp-thread:{name}-001",
            Kind: kind,
            SourceForkReceiptHandle: "urn:san:ec-participatory-peerless:review:fixture",
            AnchorHandle: $"urn:san:anchor:{name}",
            TensionClass: "tempered",
            WitnessPath: $"urn:san:witness:{name}",
            DampingPath: $"urn:san:damping:{name}",
            GovernanceBoundary: $"urn:san:governance:{name}",
            FailureClassification: null,
            AnchorPresent: true,
            Witnessed: true,
            Dampable: true,
            Pluckable: true,
            TensionWithinPlayableRange: true,
            StewardBoundaryPresent: stewardBoundary,
            MeaningImpersonatesIdentity: false,
            SemanticBuzzingDetected: false,
            ReviewOnly: true,
            Inert: true,
            AuthorityRequested: false,
            ContinuityClaimed: false,
            ActivationRequested: false);

    private static CmeLispGlobalResonanceLaw CreateLaw(
        bool soundMayBecomeAction = false,
        bool resonanceMayAuthorize = false,
        bool resonanceMayAdmitContinuity = false,
        bool discordanceMayBecomeFailure = false,
        bool dampingMayEraseWitness = false,
        bool restMayMeanAbsence = false,
        bool repetitionMayBecomeContinuity = false,
        bool amplitudeMayBecomeTruth = false) =>
        new(
            SoundMayBecomeAction: soundMayBecomeAction,
            ResonanceMayAuthorize: resonanceMayAuthorize,
            ResonanceMayAdmitContinuity: resonanceMayAdmitContinuity,
            DiscordanceMayBecomeFailure: discordanceMayBecomeFailure,
            DampingMayEraseWitness: dampingMayEraseWitness,
            RestMayMeanAbsence: restMayMeanAbsence,
            RepetitionMayBecomeContinuity: repetitionMayBecomeContinuity,
            AmplitudeMayBecomeTruth: amplitudeMayBecomeTruth,
            BoundaryLaw: "Heartbeat is governed cadence; resonance is global constraint; tuning is local profile.");

    private static StewardHeartbeatPolicy CreateHeartbeat(
        bool stewardGoverned = true,
        bool reviewWindowPresent = true,
        bool allowsUngovernedCadence = false,
        bool allowsSoundToBypassReview = false,
        bool allowsActionWithoutAdmission = false,
        bool allowsHeartbeatToOwnResonance = false,
        bool allowsHeartbeatToAdmitContinuity = false) =>
        new(
            PolicyCode: "steward-heartbeat-review-cadence",
            StewardGoverned: stewardGoverned,
            ReviewWindowPresent: reviewWindowPresent,
            AllowsUngovernedCadence: allowsUngovernedCadence,
            AllowsSoundToBypassReview: allowsSoundToBypassReview,
            AllowsActionWithoutAdmission: allowsActionWithoutAdmission,
            AllowsHeartbeatToOwnResonance: allowsHeartbeatToOwnResonance,
            AllowsHeartbeatToAdmitContinuity: allowsHeartbeatToAdmitContinuity);

    private static ListeningFrameEmanationRecord CreateEmanation() =>
        new(
            EmanationHandle: "urn:san:listening-frame-emanation:001",
            SharedRealitySurface: "SharedPrimeReality",
            ListeningFrameSurface: "ListeningFrame",
            HarmonicCondition: "coherence-tension-discordance-affordance",
            ReviewOnly: true,
            Inert: true,
            EmanationIsAction: false,
            AuthorityRequested: false,
            ContinuityClaimed: false,
            ActivationRequested: false);

    private static LispThreadTouchEvent CreateTouch(
        string touchHandle = "touch-001",
        CmeLispThreadTouchKind kind = CmeLispThreadTouchKind.Pluck,
        string threadHandle = "urn:san:cme-lisp-thread:delta-001",
        bool actionAdmissionBoundaryPresent = true) =>
        new(
            TouchHandle: $"urn:san:lisp-thread-touch:{touchHandle}",
            TouchKind: kind,
            ThreadHandle: threadHandle,
            HeartbeatOrdinal: 1,
            Attack: 0.30m,
            SustainWindow: 0.20m,
            DampingPath: "urn:san:damping:touch",
            StewardHeartbeatPresent: true,
            ActionAdmissionBoundaryPresent: actionAdmissionBoundaryPresent,
            ReviewOnly: true,
            Inert: true,
            EmitsPacket: false,
            RequestsRuntimeAction: false,
            ClaimsAuthority: false,
            ClaimsContinuity: false);

    private static ThreadResonanceEvidence CreateEvidence() =>
        new(
            EvidenceHandle: "urn:san:thread-resonance-evidence:001",
            EmanationHandle: "urn:san:listening-frame-emanation:001",
            TouchHandle: "urn:san:lisp-thread-touch:touch-001",
            ResonanceAmplitude: 0.34m,
            DiscordanceIndex: 0.21m,
            DampingApplied: true,
            ReviewOnly: true,
            Inert: true,
            EvidenceBecomesWarrant: false,
            ClaimsAction: false,
            ClaimsAuthority: false,
            ClaimsContinuity: false);

    private static DampingProfile CreateDamping() =>
        new(
            DampingCode: "cool-without-erasure",
            DampingCoefficient: 0.62m,
            CoolingRoute: "Steward.cooling.review",
            DampsWithoutErasure: true,
            ErasesWitness: false,
            PromotesContinuity: false,
            GrantsAuthority: false);

    private static DiscordanceRoute CreateDiscordance() =>
        new(
            RouteCode: "discordance-review-cooling-route",
            DiscordanceThreshold: 0.74m,
            RoutesToReview: true,
            RoutesToCooling: true,
            RoutesToRefusal: true,
            TreatsDiscordanceAsFailure: false,
            GrantsAuthority: false,
            AdmitsContinuity: false);

    private static ActionAdmissionBoundary CreateActionAdmissionBoundary() =>
        new(
            BoundaryCode: "cold-action-admission-refused",
            Present: true,
            StewardReviewRequired: true,
            AllowsSoundToBecomeAction: false,
            AllowsAuthority: false,
            AllowsContinuityAdmission: false,
            AllowsPacketEmission: false,
            AllowsLispEvaluation: false,
            IncrementsPassageCount: false);

    private static void AssertCold(CmeLispResonanceHeartbeatReceipt receipt)
    {
        Assert.True(receipt.IsColdResonanceHeartbeat);
        Assert.True(receipt.ReviewOnly);
        Assert.True(receipt.InertOnly);
        Assert.True(receipt.ListeningFrameMayReceive);
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.EmanationBecomesAction);
        Assert.False(receipt.SoundBecomesAuthority);
        Assert.False(receipt.ResonanceAdmitsContinuity);
        Assert.False(receipt.DiscordanceBecomesFailure);
        Assert.False(receipt.DampingErasesWitness);
        Assert.False(receipt.RestBecomesAbsence);
        Assert.False(receipt.ThreadTouchEmitsPacket);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.Null(receipt.Refusal);
    }

    private static void AssertRefused(
        CmeLispResonanceHeartbeatReceipt receipt,
        string outcomeCode)
    {
        Assert.Equal(CmeLispResonanceHeartbeatDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedResonanceHeartbeatRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal!.Retained);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "listening-frame-resonance-heartbeat.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate Project Bicycle repository root.");
    }
}
