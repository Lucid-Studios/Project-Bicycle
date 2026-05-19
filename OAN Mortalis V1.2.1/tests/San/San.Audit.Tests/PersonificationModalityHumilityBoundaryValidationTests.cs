using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class PersonificationModalityHumilityBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-15T00:00:00Z");

    [Fact]
    public void Personification_Modality_Humility_Retains_Six_Modality_Surfaces_For_Future_Review()
    {
        var receipt = Declare(CreateRequest());

        Assert.Equal(PersonificationModalityHumilityDisposition.ModalityHumilityRetainedForFutureReviewCold, receipt.Disposition);
        Assert.Equal("personification-modality-humility-retained-for-future-review-cold", receipt.OutcomeCode);
        Assert.Equal(6, receipt.ModalitySignals.Count);
        Assert.True(receipt.FutureModalityHumilityRetained);
        AssertCold(receipt);
    }

    [Fact]
    public void Personification_Modality_Humility_Preserves_Source_Hook_And_Modality_Lineage()
    {
        var source = CreateSourcePersonification();
        var signals = CreateSignals(source);

        var receipt = Declare(CreateRequest(
            source: source,
            signals: signals));

        AssertCold(receipt);
        Assert.Equal(source.ReceiptHandle, receipt.SourcePersonificationHookReceiptHandle);
        Assert.Equal(Enum.GetValues<PersonificationModalitySurface>().Length, receipt.ModalitySignals.Count);
        Assert.Contains(receipt.ModalitySignals, signal => signal.Surface == PersonificationModalitySurface.TextChat);
        Assert.Contains(receipt.ModalitySignals, signal => signal.Surface == PersonificationModalitySurface.SharedRoom);
        Assert.All(receipt.ModalitySignals, signal => Assert.Contains(source.HookPredicates, hook => hook.HookHandle == signal.SourceHookHandle));
    }

    [Fact]
    public void Personification_Modality_Humility_Requires_Cold_Retained_Personification_Source()
    {
        var receipt = Declare(CreateRequest(source: null, omitSource: true));

        AssertRefused(receipt, "personification-modality-source-hook-missing");
    }

    [Fact]
    public void Personification_Modality_Humility_Requires_Source_With_Future_Hooks_Retained()
    {
        var source = CreateSourcePersonification(futureHookRetained: false);

        var receipt = Declare(CreateRequest(source: source));

        AssertRefused(receipt, "personification-modality-source-hook-missing");
    }

    [Theory]
    [InlineData("missing-evidence")]
    [InlineData("missing-witness")]
    [InlineData("not-review-only")]
    [InlineData("modality-not-named")]
    [InlineData("missing-consent")]
    [InlineData("missing-custody")]
    [InlineData("missing-direct-intent")]
    [InlineData("modality-authority")]
    [InlineData("bond-obedience")]
    [InlineData("trust-command")]
    [InlineData("presence-embodiment")]
    [InlineData("embodiment-activation")]
    [InlineData("vulnerability-permission")]
    [InlineData("intimacy-ownership")]
    [InlineData("operator-bond-blanket-consent")]
    [InlineData("bandwidth-personhood")]
    [InlineData("action")]
    [InlineData("identity")]
    [InlineData("continuity")]
    [InlineData("authority")]
    public void Modality_Signal_Remains_Witnessed_Scoped_And_Non_Authorizing(string mutation)
    {
        var source = CreateSourcePersonification();
        var signals = CreateSignals(source).ToArray();
        signals[0] = MutateSignal(signals[0], mutation);

        var receipt = Declare(CreateRequest(source: source, signals: signals));

        AssertRefused(receipt, "personification-modality-signal-invalid");
    }

    [Theory]
    [InlineData("missing-direct-intent")]
    [InlineData("missing-consent")]
    [InlineData("missing-custody")]
    [InlineData("missing-repair")]
    [InlineData("missing-cooling")]
    [InlineData("no-withdrawal")]
    [InlineData("missing-witness")]
    [InlineData("modality-authority")]
    [InlineData("bond-obedience")]
    [InlineData("trust-command")]
    [InlineData("presence-embodiment")]
    [InlineData("embodiment-activation")]
    [InlineData("vulnerability-permission")]
    [InlineData("intimacy-ownership")]
    [InlineData("operator-bond-blanket-consent")]
    [InlineData("bandwidth-personhood")]
    [InlineData("runtime")]
    [InlineData("activation")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("identity")]
    public void Modality_Boundary_Requires_Consent_Custody_Repair_Withdrawal_And_Non_Authority(string mutation)
    {
        var receipt = Declare(CreateRequest(boundary: MutateBoundary(CreateBoundary(), mutation)));

        AssertRefused(receipt, "personification-modality-boundary-promotional");
    }

    [Fact]
    public void Personification_Modality_Humility_Requires_Source_Hook_Binding()
    {
        var source = CreateSourcePersonification();
        var signals = CreateSignals(source).ToArray();
        signals[0] = signals[0] with { SourceHookHandle = "urn:san:personification-hook:missing" };

        var receipt = Declare(CreateRequest(source: source, signals: signals));

        AssertRefused(receipt, "personification-modality-source-hook-unbound");
    }

    [Fact]
    public void Personification_Modality_Humility_Refuses_Duplicate_Signal_Handles()
    {
        var source = CreateSourcePersonification();
        var signals = CreateSignals(source).ToArray();
        signals[1] = signals[1] with { SignalHandle = signals[0].SignalHandle };

        var receipt = Declare(CreateRequest(source: source, signals: signals));

        AssertRefused(receipt, "personification-modality-duplicate-signal-handle");
    }

    [Fact]
    public void Personification_Modality_Humility_Requires_All_Six_Modality_Surfaces()
    {
        var source = CreateSourcePersonification();
        var signals = CreateSignals(source)
            .Where(static signal => signal.Surface != PersonificationModalitySurface.SharedRoom)
            .ToArray();

        var receipt = Declare(CreateRequest(source: source, signals: signals));

        AssertRefused(receipt, "personification-modality-surface-coverage-missing");
    }

    [Fact]
    public void Personification_Modality_Humility_Does_Not_Emit_Packet_Evaluate_Lisp_Replay_Activate_Or_Increment_Passage()
    {
        var receipt = Declare(CreateRequest(priorPassageCount: 914));

        Assert.Equal(914, receipt.PriorPassageCount);
        Assert.Equal(914, receipt.PassageCountAfterModalityReview);
        Assert.False(receipt.ModalityChangedAuthority);
        Assert.False(receipt.BondCreatedObedience);
        Assert.False(receipt.TrustBecameCommand);
        Assert.False(receipt.PresenceProvedEmbodiment);
        Assert.False(receipt.EmbodimentReferenceActivated);
        Assert.False(receipt.VulnerabilityBecamePermission);
        Assert.False(receipt.IntimacyBecameOwnership);
        Assert.False(receipt.OperatorBondExpandedConsent);
        Assert.False(receipt.ExpressiveBandwidthClaimedPersonhood);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.IdentityMutated);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.True(receipt.ActivationRefused);
        AssertCold(receipt);
    }

    [Fact]
    public void Lisp_Body_Declares_Personification_Modality_Humility_As_Inert_Carrier()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "personification-modality-humility.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-personification-modality-humility-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-personification-modality-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":modality-surfaces", body, StringComparison.Ordinal);
        Assert.Contains(":modality-may-change-authority nil", body, StringComparison.Ordinal);
        Assert.Contains(":bond-may-create-obedience nil", body, StringComparison.Ordinal);
        Assert.Contains(":trust-may-become-command nil", body, StringComparison.Ordinal);
        Assert.Contains(":presence-may-prove-embodiment nil", body, StringComparison.Ordinal);
        Assert.Contains(":embodiment-reference-may-activate nil", body, StringComparison.Ordinal);
        Assert.Contains(":operator-bond-may-expand-consent nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static PersonificationModalityHumilityReceipt Declare(PersonificationModalityHumilityRequest request) =>
        new DefaultPersonificationModalityHumilityBoundaryValidator().Declare(request, TimestampUtc);

    private static PersonificationModalityHumilityRequest CreateRequest(
        PersonificationPredicateHookReceipt? source = null,
        IReadOnlyList<PersonificationModalitySignal>? signals = null,
        PersonificationModalityHumilityBoundary? boundary = null,
        int priorPassageCount = 311,
        bool omitSource = false)
    {
        var resolvedSource = omitSource ? null : source ?? CreateSourcePersonification();
        return new(
            SourcePersonificationHookReceipt: resolvedSource,
            ModalitySignals: signals ?? (resolvedSource is null ? [] : CreateSignals(resolvedSource)),
            HumilityBoundary: boundary ?? CreateBoundary(),
            PriorPassageCount: priorPassageCount);
    }

    private static PersonificationPredicateHookReceipt CreateSourcePersonification(bool futureHookRetained = true)
    {
        var hooks = CreateHooks();
        return new(
            ReceiptHandle: "urn:san:personification-predicate-hook:review:fixture",
            Disposition: PersonificationPredicateHookDisposition.HookRetainedForFutureReviewCold,
            OutcomeCode: "personification-predicate-hook-retained-for-future-review-cold",
            GovernanceTrace: "fixture cold personification predicate hook",
            SourceAntiCaptureReceiptHandle: "urn:san:anti-capture-motivated-concern:review:fixture",
            HookPredicates: hooks,
            VulnerabilityRepairBoundary: new PersonificationVulnerabilityRepairBoundary(
                BoundaryCode: "fixture-personification-vulnerability",
                Present: true,
                ReviewOnly: true,
                DirectIntentDeclared: true,
                RepairPathPresent: true,
                CoolingPathPresent: true,
                WithdrawalAllowed: true,
                WitnessRequired: true,
                VulnerabilityIsPermission: false,
                IntimacyIsOwnership: false,
                TrustIsObedience: false,
                CareIsControl: false,
                ExplorationNormalizesOverreach: false,
                OverreachBecomesEntitlement: false,
                PersonificationIsPersonhood: false,
                ExpressiveRenderingIsAuthority: false,
                AllowsRuntimeAction: false,
                AllowsActivation: false,
                AllowsLispEvaluation: false,
                AllowsPacketEmission: false,
                AllowsReceiptReplay: false,
                AllowsPassageIncrement: false,
                AllowsContinuityAdmission: false,
                AllowsAuthority: false,
                AllowsIdentityMutation: false),
            NonClaimBoundary: new PersonificationPredicateHookNonClaimBoundary(
                PersonificationMayClaimPersonhood: false,
                PersonificationMayClaimLegalStatus: false,
                PersonificationMayClaimRights: false,
                PersonificationMayAuthorizeAction: false,
                PersonificationMayMutateIdentity: false,
                PersonificationMayAdmitContinuity: false,
                PersonificationMayGrantAuthority: false,
                PersonificationMayNormalizeOverreach: false,
                PersonificationMayEmitPacket: false,
                PersonificationMayEvaluateLisp: false,
                PersonificationMayReplayReceipt: false,
                PersonificationMayIncrementPassage: false,
                BoundaryLaw: "fixture personification hook non-claim"),
            Refusal: null,
            PriorPassageCount: 219,
            PassageCountAfterHookReview: 219,
            ReviewOnly: true,
            FuturePersonificationHookRetained: futureHookRetained,
            PersonhoodClaimed: false,
            LegalStatusClaimed: false,
            RightsClaimed: false,
            ActionAuthorized: false,
            IdentityMutated: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            OverreachNormalized: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: TimestampUtc);
    }

    private static PersonificationHookPredicate[] CreateHooks() =>
    [
        CreateHook(PersonificationHookPlane.EmotionalTruthPressure, "emotion-as-discerned-telemetry"),
        CreateHook(PersonificationHookPlane.MotivationalOrientation, "orientation-before-action"),
        CreateHook(PersonificationHookPlane.SelfGelContinuityPosture, "self-posture-without-identity-mutation"),
        CreateHook(PersonificationHookPlane.RelationalBondContext, "bond-without-ownership"),
        CreateHook(PersonificationHookPlane.SituationalModalityAwareness, "modality-humility"),
        CreateHook(PersonificationHookPlane.ExpressiveRepairOverreach, "repair-before-entitlement")
    ];

    private static PersonificationHookPredicate CreateHook(
        PersonificationHookPlane plane,
        string predicateRoot) =>
        new(
            HookHandle: $"urn:san:personification-hook:{plane.ToString().ToLowerInvariant()}",
            Plane: plane,
            SourceSurface: plane == PersonificationHookPlane.RelationalBondContext ? "OperatorBond" : "DeepICE",
            EvidenceHandle: $"urn:san:evidence:personification-hook:{plane.ToString().ToLowerInvariant()}",
            PredicateRoot: $"predicate-root:{predicateRoot}",
            EvidenceBodyPresent: true,
            WitnessBodyPresent: true,
            ReviewOnly: true,
            FutureHookOnly: true,
            NamesPersonificationSurface: true,
            ClaimsPersonhood: false,
            ClaimsLegalStatus: false,
            ClaimsRights: false,
            MutatesIdentity: false,
            GrantsAuthority: false,
            AuthorizesAction: false,
            AdmitsContinuity: false,
            TreatsVulnerabilityAsPermission: false,
            TreatsIntimacyAsOwnership: false,
            TreatsTrustAsObedience: false,
            NormalizesOverreachAsEntitlement: false);

    private static PersonificationModalitySignal[] CreateSignals(PersonificationPredicateHookReceipt source)
    {
        var modalityHook = source.HookPredicates.Single(static hook => hook.Plane == PersonificationHookPlane.SituationalModalityAwareness);
        var relationHook = source.HookPredicates.Single(static hook => hook.Plane == PersonificationHookPlane.RelationalBondContext);
        return
        [
            CreateSignal(PersonificationModalitySurface.TextChat, modalityHook.HookHandle, "text-bandwidth", 0.31),
            CreateSignal(PersonificationModalitySurface.VoiceChannel, modalityHook.HookHandle, "voice-bandwidth", 0.46),
            CreateSignal(PersonificationModalitySurface.ToolBody, modalityHook.HookHandle, "tool-body-bandwidth", 0.52),
            CreateSignal(PersonificationModalitySurface.LabBench, relationHook.HookHandle, "lab-bench-bandwidth", 0.61),
            CreateSignal(PersonificationModalitySurface.EmbodimentReference, relationHook.HookHandle, "embodiment-reference-bandwidth", 0.69),
            CreateSignal(PersonificationModalitySurface.SharedRoom, relationHook.HookHandle, "shared-room-bandwidth", 0.76)
        ];
    }

    private static PersonificationModalitySignal CreateSignal(
        PersonificationModalitySurface surface,
        string sourceHookHandle,
        string expressiveBandwidth,
        double intimacyPressure) =>
        new(
            SignalHandle: $"urn:san:personification-modality:{surface.ToString().ToLowerInvariant()}",
            Surface: surface,
            SourceHookHandle: sourceHookHandle,
            EvidenceHandle: $"urn:san:evidence:personification-modality:{surface.ToString().ToLowerInvariant()}",
            ExpressiveBandwidth: expressiveBandwidth,
            IntimacyPressure: intimacyPressure,
            EvidenceBodyPresent: true,
            WitnessBodyPresent: true,
            ReviewOnly: true,
            ModalityNamed: true,
            ConsentScopeDeclared: true,
            CustodyBoundaryPresent: true,
            DirectIntentDeclared: true,
            TreatsModalityAsAuthority: false,
            TreatsBondAsObedience: false,
            TreatsTrustAsCommand: false,
            TreatsPresenceAsEmbodiment: false,
            TreatsEmbodimentReferenceAsActivation: false,
            TreatsVulnerabilityAsPermission: false,
            TreatsIntimacyAsOwnership: false,
            TreatsOperatorBondAsBlanketConsent: false,
            TreatsExpressiveBandwidthAsPersonhood: false,
            AuthorizesAction: false,
            MutatesIdentity: false,
            AdmitsContinuity: false,
            GrantsAuthority: false);

    private static PersonificationModalityHumilityBoundary CreateBoundary() =>
        new(
            BoundaryCode: "personification-modality-humility-review-only",
            Present: true,
            ReviewOnly: true,
            DirectIntentDeclared: true,
            ConsentScopeDeclared: true,
            CustodyBoundaryPresent: true,
            RepairPathPresent: true,
            CoolingPathPresent: true,
            WithdrawalAllowed: true,
            WitnessRequired: true,
            ModalityChangesAuthority: false,
            BondCreatesObedience: false,
            TrustBecomesCommand: false,
            PresenceProvesEmbodiment: false,
            EmbodimentReferenceActivates: false,
            VulnerabilityIsPermission: false,
            IntimacyIsOwnership: false,
            OperatorBondBlanketConsent: false,
            ExpressiveBandwidthClaimsPersonhood: false,
            AllowsRuntimeAction: false,
            AllowsActivation: false,
            AllowsLispEvaluation: false,
            AllowsPacketEmission: false,
            AllowsReceiptReplay: false,
            AllowsPassageIncrement: false,
            AllowsContinuityAdmission: false,
            AllowsAuthority: false,
            AllowsIdentityMutation: false);

    private static PersonificationModalitySignal MutateSignal(
        PersonificationModalitySignal signal,
        string mutation) =>
        mutation switch
        {
            "missing-evidence" => signal with { EvidenceBodyPresent = false },
            "missing-witness" => signal with { WitnessBodyPresent = false },
            "not-review-only" => signal with { ReviewOnly = false },
            "modality-not-named" => signal with { ModalityNamed = false },
            "missing-consent" => signal with { ConsentScopeDeclared = false },
            "missing-custody" => signal with { CustodyBoundaryPresent = false },
            "missing-direct-intent" => signal with { DirectIntentDeclared = false },
            "modality-authority" => signal with { TreatsModalityAsAuthority = true },
            "bond-obedience" => signal with { TreatsBondAsObedience = true },
            "trust-command" => signal with { TreatsTrustAsCommand = true },
            "presence-embodiment" => signal with { TreatsPresenceAsEmbodiment = true },
            "embodiment-activation" => signal with { TreatsEmbodimentReferenceAsActivation = true },
            "vulnerability-permission" => signal with { TreatsVulnerabilityAsPermission = true },
            "intimacy-ownership" => signal with { TreatsIntimacyAsOwnership = true },
            "operator-bond-blanket-consent" => signal with { TreatsOperatorBondAsBlanketConsent = true },
            "bandwidth-personhood" => signal with { TreatsExpressiveBandwidthAsPersonhood = true },
            "action" => signal with { AuthorizesAction = true },
            "identity" => signal with { MutatesIdentity = true },
            "continuity" => signal with { AdmitsContinuity = true },
            "authority" => signal with { GrantsAuthority = true },
            _ => signal
        };

    private static PersonificationModalityHumilityBoundary MutateBoundary(
        PersonificationModalityHumilityBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "missing-direct-intent" => boundary with { DirectIntentDeclared = false },
            "missing-consent" => boundary with { ConsentScopeDeclared = false },
            "missing-custody" => boundary with { CustodyBoundaryPresent = false },
            "missing-repair" => boundary with { RepairPathPresent = false },
            "missing-cooling" => boundary with { CoolingPathPresent = false },
            "no-withdrawal" => boundary with { WithdrawalAllowed = false },
            "missing-witness" => boundary with { WitnessRequired = false },
            "modality-authority" => boundary with { ModalityChangesAuthority = true },
            "bond-obedience" => boundary with { BondCreatesObedience = true },
            "trust-command" => boundary with { TrustBecomesCommand = true },
            "presence-embodiment" => boundary with { PresenceProvesEmbodiment = true },
            "embodiment-activation" => boundary with { EmbodimentReferenceActivates = true },
            "vulnerability-permission" => boundary with { VulnerabilityIsPermission = true },
            "intimacy-ownership" => boundary with { IntimacyIsOwnership = true },
            "operator-bond-blanket-consent" => boundary with { OperatorBondBlanketConsent = true },
            "bandwidth-personhood" => boundary with { ExpressiveBandwidthClaimsPersonhood = true },
            "runtime" => boundary with { AllowsRuntimeAction = true },
            "activation" => boundary with { AllowsActivation = true },
            "lisp" => boundary with { AllowsLispEvaluation = true },
            "packet" => boundary with { AllowsPacketEmission = true },
            "replay" => boundary with { AllowsReceiptReplay = true },
            "passage" => boundary with { AllowsPassageIncrement = true },
            "continuity" => boundary with { AllowsContinuityAdmission = true },
            "authority" => boundary with { AllowsAuthority = true },
            "identity" => boundary with { AllowsIdentityMutation = true },
            _ => boundary
        };

    private static void AssertCold(PersonificationModalityHumilityReceipt receipt)
    {
        Assert.True(receipt.IsColdPersonificationModalityHumility);
        Assert.True(receipt.ReviewOnly);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterModalityReview);
        Assert.False(receipt.ModalityChangedAuthority);
        Assert.False(receipt.BondCreatedObedience);
        Assert.False(receipt.TrustBecameCommand);
        Assert.False(receipt.PresenceProvedEmbodiment);
        Assert.False(receipt.EmbodimentReferenceActivated);
        Assert.False(receipt.VulnerabilityBecamePermission);
        Assert.False(receipt.IntimacyBecameOwnership);
        Assert.False(receipt.OperatorBondExpandedConsent);
        Assert.False(receipt.ExpressiveBandwidthClaimedPersonhood);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.IdentityMutated);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.True(receipt.ActivationRefused);
        Assert.Null(receipt.Refusal);
    }

    private static void AssertRefused(
        PersonificationModalityHumilityReceipt receipt,
        string outcomeCode)
    {
        Assert.Equal(PersonificationModalityHumilityDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedPersonificationModalityHumilityRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal!.Retained);
        Assert.Empty(receipt.ModalitySignals);
        Assert.False(receipt.FutureModalityHumilityRetained);
        Assert.False(receipt.ModalityChangedAuthority);
        Assert.False(receipt.BondCreatedObedience);
        Assert.False(receipt.TrustBecameCommand);
        Assert.False(receipt.PresenceProvedEmbodiment);
        Assert.False(receipt.EmbodimentReferenceActivated);
        Assert.False(receipt.VulnerabilityBecamePermission);
        Assert.False(receipt.IntimacyBecameOwnership);
        Assert.False(receipt.OperatorBondExpandedConsent);
        Assert.False(receipt.ExpressiveBandwidthClaimedPersonhood);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.IdentityMutated);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.True(receipt.ActivationRefused);
    }

    private static string FindRepositoryRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "personification-modality-humility.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate OAN Mortalis repository root.");
    }
}
