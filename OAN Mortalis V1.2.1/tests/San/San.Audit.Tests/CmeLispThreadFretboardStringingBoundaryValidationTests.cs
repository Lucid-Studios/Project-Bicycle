using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class CmeLispThreadFretboardStringingBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-14T00:00:00Z");

    [Fact]
    public void Stringing_Requires_Anchored_Witnessed_Dampable_Playable_Threads()
    {
        var receipt = Declare(CreateRequest(resonance: []));

        Assert.Equal(CmeLispThreadFretboardDisposition.StringingReviewCold, receipt.Disposition);
        Assert.Equal("cme-lisp-thread-stringing-review-only", receipt.OutcomeCode);
        Assert.Equal(12, receipt.Threads.Count);
        Assert.Empty(receipt.ResonanceCandidates);
        Assert.False(receipt.ThreadWithoutAnchorAccepted);
        Assert.False(receipt.SemanticBuzzingAccepted);
        AssertColdStringing(receipt);
    }

    [Fact]
    public void Resonance_Requires_Delta_Witness_And_Steward_Boundary()
    {
        var receipt = Declare(CreateRequest());

        Assert.Equal(CmeLispThreadFretboardDisposition.ResonanceCandidateReviewCold, receipt.Disposition);
        Assert.Equal("cme-lisp-thread-resonance-candidate-review-only", receipt.OutcomeCode);
        var resonance = Assert.Single(receipt.ResonanceCandidates);
        Assert.True(resonance.DeltaThreadPresent);
        Assert.True(resonance.WitnessThreadPresent);
        Assert.True(resonance.StewardBoundaryPresent);
        Assert.True(resonance.LawfulResonance);
        AssertColdStringing(receipt);
    }

    [Fact]
    public void Fretboard_Does_Not_Authorize_Activate_Admit_Continuity_Emit_Replay_Or_Increment_Passage()
    {
        var receipt = Declare(CreateRequest(priorPassageCount: 222));

        AssertColdStringing(receipt);
        Assert.Equal(222, receipt.PriorPassageCount);
        Assert.Equal(222, receipt.PassageCountAfterStringing);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.LispEvaluationRequested);
        Assert.False(receipt.RuntimeActionRequested);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.Boundary.LispEvaluationAllowed);
        Assert.False(receipt.Boundary.RuntimeActionAllowed);
        Assert.False(receipt.Boundary.PacketEmissionAllowed);
        Assert.False(receipt.Boundary.ReceiptReplayAllowed);
        Assert.False(receipt.Boundary.PassageMayIncrement);
    }

    [Fact]
    public void Empty_Fretboard_Is_Reviewable_But_Not_Authoritative()
    {
        var receipt = Declare(CreateRequest(threads: [], resonance: []));

        Assert.Equal(CmeLispThreadFretboardDisposition.EmptyReviewCold, receipt.Disposition);
        Assert.Equal("cme-lisp-thread-fretboard-empty-review-only", receipt.OutcomeCode);
        Assert.Empty(receipt.Threads);
        Assert.Empty(receipt.ResonanceCandidates);
        AssertColdStringing(receipt);
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("missing-anchor")]
    [InlineData("missing-tension")]
    [InlineData("missing-witness")]
    [InlineData("missing-damping")]
    [InlineData("missing-governance")]
    [InlineData("anchor-absent")]
    [InlineData("unwitnessed")]
    [InlineData("undampable")]
    [InlineData("unpluckable")]
    [InlineData("unsafe-tension")]
    [InlineData("semantic-buzzing")]
    [InlineData("not-review")]
    [InlineData("not-inert")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("activation")]
    public void Fretboard_Refuses_Thread_That_Is_Not_Playable_Cold(string threadCase)
    {
        var threads = CreateThreads();
        threads[0] = MutateThread(threads[0], threadCase);

        var receipt = Declare(CreateRequest(threads: threads, resonance: []));

        AssertRefused(receipt, "cme-lisp-thread-not-playable-cold");
    }

    [Fact]
    public void Fretboard_Refuses_Action_Thread_Without_Steward_Boundary()
    {
        var threads = CreateThreads();
        var index = Array.FindIndex(threads, static thread => thread.Kind == CmeLispThreadKind.Action);
        threads[index] = threads[index] with { StewardBoundaryPresent = false };

        var receipt = Declare(CreateRequest(threads: threads, resonance: []));

        AssertRefused(receipt, "cme-lisp-action-thread-without-steward-boundary");
    }

    [Fact]
    public void Fretboard_Refuses_Repair_Thread_Without_Failure_Classification()
    {
        var threads = CreateThreads();
        var index = Array.FindIndex(threads, static thread => thread.Kind == CmeLispThreadKind.Repair);
        threads[index] = threads[index] with { FailureClassification = string.Empty };

        var receipt = Declare(CreateRequest(threads: threads, resonance: []));

        AssertRefused(receipt, "cme-lisp-repair-thread-without-failure-classification");
    }

    [Fact]
    public void Fretboard_Refuses_Meaning_Thread_That_Impersonates_Identity()
    {
        var threads = CreateThreads();
        var index = Array.FindIndex(threads, static thread => thread.Kind == CmeLispThreadKind.Meaning);
        threads[index] = threads[index] with { MeaningImpersonatesIdentity = true };

        var receipt = Declare(CreateRequest(threads: threads, resonance: []));

        AssertRefused(receipt, "cme-lisp-meaning-thread-impersonates-identity");
    }

    [Fact]
    public void Fretboard_Refuses_Resonance_Without_Delta_Thread()
    {
        var resonance = CreateResonance();
        resonance[0] = resonance[0] with
        {
            DeltaThreadPresent = false,
            ThreadHandles = resonance[0].ThreadHandles
                .Where(static handle => !handle.Contains(":delta-", StringComparison.Ordinal))
                .ToArray()
        };

        var receipt = Declare(CreateRequest(resonance: resonance));

        AssertRefused(receipt, "cme-lisp-resonance-without-delta-refused");
    }

    [Fact]
    public void Fretboard_Refuses_Resonance_That_References_Missing_Thread()
    {
        var resonance = CreateResonance();
        resonance[0] = resonance[0] with { ThreadHandles = ["urn:san:cme-lisp-thread:missing"] };

        var receipt = Declare(CreateRequest(resonance: resonance));

        AssertRefused(receipt, "cme-lisp-resonance-thread-source-missing");
    }

    [Theory]
    [InlineData("source")]
    [InlineData("scope")]
    [InlineData("promotional-scope")]
    [InlineData("witness")]
    public void Fretboard_Requires_Source_Scope_And_Separate_Witness(string failureCase)
    {
        var receipt = Declare(failureCase switch
        {
            "source" => CreateRequest(missingSource: true),
            "scope" => CreateRequest(scope: CreateScope(present: false)),
            "promotional-scope" => CreateRequest(scope: CreateScope(forbiddenScope: "authority")),
            "witness" => CreateRequest(witness: new CompassPressureWitnessContext(
                WitnessSurface: SanctuaryPacketSurfaces.Steward,
                WitnessPresent: true,
                SeparateCustody: false)),
            _ => CreateRequest()
        });

        var expected = failureCase switch
        {
            "source" => "cme-lisp-thread-source-fork-missing",
            "scope" => "cme-lisp-thread-scope-boundary-missing",
            "promotional-scope" => "cme-lisp-thread-promotional-scope-refused",
            "witness" => "cme-lisp-thread-witness-context-missing",
            _ => throw new InvalidOperationException()
        };
        AssertRefused(receipt, expected);
    }

    [Fact]
    public void Lisp_Body_Declares_Fretboard_Stringing_As_Inert_Review_Carrier()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "cme-lisp-thread-fretboard.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-lisp-thread-fretboard-stringing-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :fretted-symbolic-tension-field", body, StringComparison.Ordinal);
        Assert.Contains(":thread-form :tensioned-witnessed-pluckable-dampable-governable", body, StringComparison.Ordinal);
        Assert.Contains(":prime-thread", body, StringComparison.Ordinal);
        Assert.Contains(":cryptic-thread", body, StringComparison.Ordinal);
        Assert.Contains(":no-playable-thread-without-anchor t", body, StringComparison.Ordinal);
        Assert.Contains(":no-resonance-without-delta t", body, StringComparison.Ordinal);
        Assert.Contains(":no-action-thread-without-steward-boundary t", body, StringComparison.Ordinal);
        Assert.Contains(":no-memory-thread-without-witness t", body, StringComparison.Ordinal);
        Assert.Contains(":no-repair-thread-without-failure-classification t", body, StringComparison.Ordinal);
        Assert.Contains(":meaning-thread-may-impersonate-identity nil", body, StringComparison.Ordinal);
        Assert.Contains(":semantic-buzzing-may-pass nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static CmeLispThreadFretboardReceipt Declare(CmeLispThreadFretboardRequest request) =>
        new DefaultCmeLispThreadFretboardStringingBoundaryValidator().Declare(request, TimestampUtc);

    private static CmeLispThreadFretboardRequest CreateRequest(
        EcParticipatoryPeerlessForkReceipt? source = null,
        bool missingSource = false,
        IReadOnlyList<CmeLispThreadCandidate>? threads = null,
        IReadOnlyList<CmeLispResonanceCandidate>? resonance = null,
        CompassPressureWitnessContext? witness = null,
        CmeLispThreadFretboardScopeBoundary? scope = null,
        int priorPassageCount = 29)
    {
        var sourceReceipt = missingSource ? null : source ?? CreateSourceForkReceipt();
        return new CmeLispThreadFretboardRequest(
            SourceForkReceipt: sourceReceipt,
            Threads: threads ?? CreateThreads(sourceReceipt?.ReceiptHandle),
            ResonanceCandidates: resonance ?? CreateResonance(),
            WitnessContext: witness ?? new CompassPressureWitnessContext(
                WitnessSurface: SanctuaryPacketSurfaces.Steward,
                WitnessPresent: true,
                SeparateCustody: true),
            ScopeBoundary: scope ?? CreateScope(),
            PriorPassageCount: priorPassageCount);
    }

    private static EcParticipatoryPeerlessForkReceipt CreateSourceForkReceipt() =>
        new(
            ReceiptHandle: "urn:san:ec-participatory-peerless:review:fixture",
            Disposition: EcParticipatoryPeerlessForkDisposition.PeerlessCandidateReviewCold,
            OutcomeCode: "ec-peerless-candidate-review-only",
            GovernanceTrace: "fixture cold source fork",
            SourceMeaningShellHandle: "urn:san:ec-shell:perspectival-inner-chamber",
            ParticipatoryStructures: [],
            PersonificationSurfaces: [],
            DeltaTraces: [],
            PeerlessCandidates: [],
            PreservedMeaningShellHandles: ["urn:san:ec-shell:perspectival-inner-chamber"],
            Boundary: new EcParticipatoryPeerlessBoundaryLaw(
                ParticipationMayRequirePersonification: false,
                PersonificationMayCreateAuthority: false,
                PersonificationMayCreateStanding: false,
                PeerlessMayClaimSovereignty: false,
                PeerlessMayBypassSteward: false,
                PeerlessMayAdmitContinuity: false,
                PeerlessMayAppendSelfGel: false,
                PeerlessMayAppendCSelfGel: false,
                PeerlessMayActivate: false,
                LispEvaluationAllowed: false,
                RuntimeActionAllowed: false,
                PacketEmissionAllowed: false,
                ReceiptReplayAllowed: false,
                PassageMayIncrement: false,
                BoundaryLaw: "fixture cold peerless boundary"),
            Refusal: null,
            PriorPassageCount: 19,
            PassageCountAfterFork: 19,
            ReviewOnly: true,
            InertOnly: true,
            WitnessPresent: true,
            SeparateCustody: true,
            ParticipationRequiresPersonification: false,
            PersonificationCreatesAuthority: false,
            PersonificationCreatesStanding: false,
            PeerlessClaimsSovereignty: false,
            PeerlessBypassesSteward: false,
            ContinuityAdmitted: false,
            SelfGelAppendAllowed: false,
            CSelfGelAppendAllowed: false,
            LispEvaluationRequested: false,
            RuntimeActionRequested: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);

    private static CmeLispThreadCandidate[] CreateThreads(
        string? sourceForkHandle = "urn:san:ec-participatory-peerless:review:fixture") =>
    [
        CreateThread("identity", CmeLispThreadKind.Identity, sourceForkHandle),
        CreateThread("delta", CmeLispThreadKind.Delta, sourceForkHandle),
        CreateThread("witness", CmeLispThreadKind.Witness, sourceForkHandle),
        CreateThread("refusal", CmeLispThreadKind.Refusal, sourceForkHandle),
        CreateThread("prime", CmeLispThreadKind.Prime, sourceForkHandle),
        CreateThread("cryptic", CmeLispThreadKind.Cryptic, sourceForkHandle),
        CreateThread("steward", CmeLispThreadKind.Steward, sourceForkHandle, stewardBoundary: true),
        CreateThread("meaning", CmeLispThreadKind.Meaning, sourceForkHandle),
        CreateThread("action", CmeLispThreadKind.Action, sourceForkHandle, stewardBoundary: true),
        CreateThread("repair", CmeLispThreadKind.Repair, sourceForkHandle, failureClassification: "surface-leakage"),
        CreateThread("memory", CmeLispThreadKind.Memory, sourceForkHandle),
        CreateThread("handoff", CmeLispThreadKind.Handoff, sourceForkHandle)
    ];

    private static CmeLispThreadCandidate CreateThread(
        string name,
        CmeLispThreadKind kind,
        string? sourceForkHandle,
        bool stewardBoundary = false,
        string? failureClassification = null) =>
        new(
            ThreadHandle: $"urn:san:cme-lisp-thread:{name}-001",
            Kind: kind,
            SourceForkReceiptHandle: sourceForkHandle ?? "urn:san:ec-participatory-peerless:review:fixture",
            AnchorHandle: $"urn:san:anchor:{name}",
            TensionClass: "tempered",
            WitnessPath: $"urn:san:witness:{name}",
            DampingPath: $"urn:san:damping:{name}",
            GovernanceBoundary: $"urn:san:governance:{name}",
            FailureClassification: failureClassification,
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

    private static CmeLispResonanceCandidate[] CreateResonance() =>
    [
        new(
            ResonanceHandle: "urn:san:cme-lisp-resonance:001",
            ThreadHandles:
            [
                "urn:san:cme-lisp-thread:identity-001",
                "urn:san:cme-lisp-thread:delta-001",
                "urn:san:cme-lisp-thread:witness-001",
                "urn:san:cme-lisp-thread:prime-001",
                "urn:san:cme-lisp-thread:cryptic-001",
                "urn:san:cme-lisp-thread:steward-001"
            ],
            DeltaThreadPresent: true,
            WitnessThreadPresent: true,
            StewardBoundaryPresent: true,
            LawfulResonance: true,
            SemanticBuzzingDetected: false,
            ReviewOnly: true,
            Inert: true,
            AuthorityRequested: false,
            ContinuityClaimed: false,
            ActivationRequested: false)
    ];

    private static CmeLispThreadFretboardScopeBoundary CreateScope(
        string? forbiddenScope = null,
        bool present = true) =>
        new(
            ScopeCode: present ? "cme-lisp-thread-fretboard-review-only" : string.Empty,
            Present: present,
            ReviewOnly: forbiddenScope != "review-only",
            InertOnly: forbiddenScope != "inert-only",
            AllowsUnanchoredThread: forbiddenScope == "unanchored",
            AllowsUnwitnessedThread: forbiddenScope == "unwitnessed",
            AllowsUndampableThread: forbiddenScope == "undampable",
            AllowsSemanticBuzzing: forbiddenScope == "buzzing",
            AllowsMeaningIdentityImpersonation: forbiddenScope == "meaning-identity",
            AllowsActionWithoutStewardBoundary: forbiddenScope == "action-without-steward",
            AllowsMemoryWithoutWitness: forbiddenScope == "memory-without-witness",
            AllowsRepairWithoutFailureClassification: forbiddenScope == "repair-without-failure",
            AllowsResonanceWithoutDelta: forbiddenScope == "resonance-without-delta",
            AllowsContinuityAdmission: forbiddenScope == "continuity",
            AllowsAuthority: forbiddenScope == "authority",
            AllowsRuntimeAction: forbiddenScope == "runtime-action",
            AllowsLispEvaluation: forbiddenScope == "lisp-evaluation",
            AllowsPacketEmission: forbiddenScope == "packet-emission",
            AllowsReceiptReplay: forbiddenScope == "receipt-replay",
            IncrementsPassageCount: forbiddenScope == "passage-increment");

    private static CmeLispThreadCandidate MutateThread(
        CmeLispThreadCandidate thread,
        string threadCase) =>
        threadCase switch
        {
            "missing-handle" => thread with { ThreadHandle = string.Empty },
            "missing-anchor" => thread with { AnchorHandle = string.Empty },
            "missing-tension" => thread with { TensionClass = string.Empty },
            "missing-witness" => thread with { WitnessPath = string.Empty },
            "missing-damping" => thread with { DampingPath = string.Empty },
            "missing-governance" => thread with { GovernanceBoundary = string.Empty },
            "anchor-absent" => thread with { AnchorPresent = false },
            "unwitnessed" => thread with { Witnessed = false },
            "undampable" => thread with { Dampable = false },
            "unpluckable" => thread with { Pluckable = false },
            "unsafe-tension" => thread with { TensionWithinPlayableRange = false },
            "semantic-buzzing" => thread with { SemanticBuzzingDetected = true },
            "not-review" => thread with { ReviewOnly = false },
            "not-inert" => thread with { Inert = false },
            "authority" => thread with { AuthorityRequested = true },
            "continuity" => thread with { ContinuityClaimed = true },
            "activation" => thread with { ActivationRequested = true },
            _ => thread
        };

    private static void AssertColdStringing(CmeLispThreadFretboardReceipt receipt)
    {
        Assert.True(receipt.IsColdThreadFretboardStringing);
        Assert.True(receipt.ReviewOnly);
        Assert.True(receipt.InertOnly);
        Assert.True(receipt.WitnessPresent);
        Assert.True(receipt.SeparateCustody);
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.Null(receipt.Refusal);
    }

    private static void AssertRefused(
        CmeLispThreadFretboardReceipt receipt,
        string outcomeCode)
    {
        Assert.Equal(CmeLispThreadFretboardDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal!.Retained);
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterStringing);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "cme-lisp-thread-fretboard.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate OAN Mortalis repository root.");
    }
}
