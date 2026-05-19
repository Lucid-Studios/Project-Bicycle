using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class WaveCondensationSharedRealityBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-16T00:00:00Z");

    [Fact]
    public void Wave_Condensation_Retains_Shared_Reality_Surface_Without_Warrant()
    {
        var receipt = Condense(CreateRequest());

        Assert.Equal(WaveCondensationSharedRealityDisposition.CondensedForReviewCold, receipt.Disposition);
        Assert.Equal("wave-condensation-shared-reality-review-surface-cold", receipt.OutcomeCode);
        Assert.True(receipt.CondensedIntoSharedReviewSurface);
        Assert.Equal(3, receipt.Signals.Count);
        Assert.Equal(3, receipt.Anchors.Count);
        AssertCold(receipt);
    }

    [Fact]
    public void Empty_Wave_Condensation_Is_Reviewable_But_Not_Authoritative()
    {
        var receipt = Condense(CreateRequest(signals: [], anchors: []));

        Assert.Equal(WaveCondensationSharedRealityDisposition.EmptyReviewCold, receipt.Disposition);
        Assert.Equal("wave-condensation-empty-review-only", receipt.OutcomeCode);
        Assert.Empty(receipt.Signals);
        Assert.Empty(receipt.Anchors);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ContinuityAdmitted);
        AssertCold(receipt);
    }

    [Fact]
    public void Wave_Condensation_Preserves_Prime_Cryptic_And_Steward_Lineage()
    {
        var receipt = Condense(CreateRequest());

        Assert.Contains(receipt.Signals, signal => signal.SignalKind == WaveSignalKind.PrimeBody);
        Assert.Contains(receipt.Signals, signal => signal.SignalKind == WaveSignalKind.CrypticMind);
        Assert.Contains(receipt.Signals, signal => signal.SignalKind == WaveSignalKind.StewardWitness);
        Assert.All(receipt.Anchors, anchor =>
        {
            Assert.True(anchor.PrimeInBody);
            Assert.True(anchor.CrypticInMind);
            Assert.True(anchor.WitnessedBySteward);
            Assert.True(anchor.RequiresPrimeCrypticStewardTriad);
        });
        AssertCold(receipt);
    }

    [Fact]
    public void Wave_Condensation_Does_Not_Convert_Waves_Into_Action_Authority_Continuity_Or_Activation()
    {
        var receipt = Condense(CreateRequest(priorPassageCount: 9001));

        Assert.Equal(9001, receipt.PriorPassageCount);
        Assert.Equal(9001, receipt.PassageCountAfterCondensation);
        Assert.False(receipt.WaveBecameTruth);
        Assert.False(receipt.CondensationBecameWarrant);
        Assert.False(receipt.SharedRealityBecameAuthority);
        Assert.False(receipt.ConsensusBecameEvidence);
        Assert.False(receipt.AnchorAdmittedContinuity);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.IdentityMutated);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
        AssertCold(receipt);
    }

    [Theory]
    [InlineData("missing-boundary")]
    [InlineData("not-review")]
    [InlineData("missing-evidence")]
    [InlineData("missing-witness")]
    [InlineData("missing-cooling")]
    [InlineData("missing-return")]
    [InlineData("missing-steward")]
    [InlineData("missing-separation")]
    [InlineData("wave-truth")]
    [InlineData("condensation-warrant")]
    [InlineData("consensus-authority")]
    [InlineData("shared-continuity")]
    [InlineData("runtime")]
    [InlineData("identity")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Condensation_Boundary_Refuses_Promotional_Collapse(string mutation)
    {
        var receipt = Condense(CreateRequest(boundary: MutateBoundary(CreateBoundary(), mutation)));

        var expected = mutation == "missing-boundary"
            ? "wave-condensation-boundary-missing"
            : "wave-condensation-promotional-boundary";
        AssertRefused(receipt, expected);
    }

    [Theory]
    [InlineData("wave-truth")]
    [InlineData("condensation-warrant")]
    [InlineData("shared-authority")]
    [InlineData("consensus-evidence")]
    [InlineData("anchor-continuity")]
    [InlineData("action")]
    [InlineData("lisp")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Non_Collapse_Boundary_Refuses_Condensation_As_Crown(string mutation)
    {
        var receipt = Condense(CreateRequest(nonCollapse: MutateNonCollapse(CreateNonCollapse(), mutation)));

        AssertRefused(receipt, "wave-condensation-non-collapse-boundary-invalid");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("missing-source")]
    [InlineData("missing-evidence")]
    [InlineData("missing-witness")]
    [InlineData("missing-target")]
    [InlineData("negative-index")]
    [InlineData("amplitude-high")]
    [InlineData("confidence-low")]
    [InlineData("not-review")]
    [InlineData("missing-evidence-body")]
    [InlineData("missing-witness-body")]
    [InlineData("missing-cooling")]
    [InlineData("missing-return")]
    [InlineData("wave-truth")]
    [InlineData("condensation-warrant")]
    [InlineData("resonance-authority")]
    [InlineData("consensus-evidence")]
    [InlineData("continuity")]
    [InlineData("identity")]
    [InlineData("action")]
    [InlineData("lisp")]
    public void Wave_Signal_Remains_Evidence_Backed_And_Review_Only(string mutation)
    {
        var signals = CreateSignals();
        signals[0] = MutateSignal(signals[0], mutation);

        var receipt = Condense(CreateRequest(signals: signals));

        AssertRefused(receipt, "wave-condensation-signal-invalid");
    }

    [Fact]
    public void Wave_Condensation_Refuses_Duplicate_Signal_Handles()
    {
        var signals = CreateSignals();
        signals[2] = signals[2] with { SignalHandle = signals[0].SignalHandle };

        var receipt = Condense(CreateRequest(signals: signals));

        AssertRefused(receipt, "wave-condensation-duplicate-signal-handle");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("missing-source")]
    [InlineData("missing-surface")]
    [InlineData("missing-prime")]
    [InlineData("missing-cryptic")]
    [InlineData("missing-steward")]
    [InlineData("missing-lineage")]
    [InlineData("prime-not-body")]
    [InlineData("cryptic-not-mind")]
    [InlineData("not-witnessed")]
    [InlineData("not-review")]
    [InlineData("missing-triad")]
    [InlineData("sharedness-truth")]
    [InlineData("consensus-authority")]
    [InlineData("anchor-continuity")]
    [InlineData("prime-actual")]
    [InlineData("cryptic-actual")]
    [InlineData("steward-authority")]
    [InlineData("action")]
    [InlineData("authority")]
    [InlineData("continuity")]
    public void Shared_Reality_Anchor_Holds_Triad_Without_Authority(string mutation)
    {
        var anchors = CreateAnchors();
        anchors[0] = MutateAnchor(anchors[0], mutation);

        var receipt = Condense(CreateRequest(anchors: anchors));

        AssertRefused(receipt, "wave-condensation-anchor-invalid");
    }

    [Fact]
    public void Shared_Reality_Anchor_Must_Bind_To_Known_Signal()
    {
        var anchors = CreateAnchors();
        anchors[0] = anchors[0] with { SourceSignalHandle = "urn:san:wave-signal:missing" };

        var receipt = Condense(CreateRequest(anchors: anchors));

        AssertRefused(receipt, "wave-condensation-anchor-unbound");
    }

    [Fact]
    public void Wave_Condensation_Refuses_Duplicate_Anchor_Handles()
    {
        var anchors = CreateAnchors();
        anchors[1] = anchors[1] with { AnchorHandle = anchors[0].AnchorHandle };

        var receipt = Condense(CreateRequest(anchors: anchors));

        AssertRefused(receipt, "wave-condensation-duplicate-anchor-handle");
    }

    [Fact]
    public void Lisp_Body_Carries_Wave_Condensation_As_Inert_Shared_Reality_Register()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "wave-condensation-shared-reality.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-wave-condensation-shared-reality-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-wave-condensation-shared-reality-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":shared-reality-surface :shared-prime-reality", body, StringComparison.Ordinal);
        Assert.Contains(":prime :in-body", body, StringComparison.Ordinal);
        Assert.Contains(":cryptic :in-mind", body, StringComparison.Ordinal);
        Assert.Contains(":steward :witnessing", body, StringComparison.Ordinal);
        Assert.Contains(":wave-not-truth", body, StringComparison.Ordinal);
        Assert.Contains(":condensation-not-warrant", body, StringComparison.Ordinal);
        Assert.Contains(":shared-reality-not-authority", body, StringComparison.Ordinal);
        Assert.Contains(":consensus-not-evidence", body, StringComparison.Ordinal);
        Assert.Contains(":anchor-not-continuity", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-evaluation-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static WaveCondensationSharedRealityReceipt Condense(WaveCondensationSharedRealityRequest request) =>
        new DefaultWaveCondensationSharedRealityBoundaryValidator().Condense(request, TimestampUtc);

    private static WaveCondensationSharedRealityRequest CreateRequest(
        IReadOnlyList<WaveSignal>? signals = null,
        IReadOnlyList<SharedRealityAnchor>? anchors = null,
        WaveCondensationBoundary? boundary = null,
        WaveCondensationNonCollapseBoundary? nonCollapse = null,
        int priorPassageCount = 42) =>
        new(
            Signals: signals ?? CreateSignals(),
            Anchors: anchors ?? CreateAnchors(),
            Boundary: boundary ?? CreateBoundary(),
            NonCollapseBoundary: nonCollapse ?? CreateNonCollapse(),
            PriorPassageCount: priorPassageCount);

    private static WaveSignal[] CreateSignals() =>
    [
        Signal("prime-body", WaveSignalKind.PrimeBody, "prime-body-surface", 0),
        Signal("cryptic-mind", WaveSignalKind.CrypticMind, "cryptic-mind-surface", 1),
        Signal("steward-witness", WaveSignalKind.StewardWitness, "steward-witness-surface", 2)
    ];

    private static WaveSignal Signal(
        string suffix,
        WaveSignalKind kind,
        string sourceSurface,
        int index) =>
        new(
            SignalHandle: $"urn:san:wave-signal:{suffix}",
            SignalKind: kind,
            SourceSurface: sourceSurface,
            EvidenceHandle: $"urn:san:evidence:wave:{suffix}",
            WitnessHandle: $"urn:san:witness:wave:{suffix}",
            CondensationTarget: "urn:san:shared-reality:prime-body-cryptic-mind-steward-witness",
            WaveIndex: index,
            Amplitude: 0.61,
            Confidence: 0.74,
            ReviewOnly: true,
            EvidenceBodyPresent: true,
            WitnessBodyPresent: true,
            CoolingPathPresent: true,
            ReturnPathPresent: true,
            TreatsWaveAsTruth: false,
            TreatsCondensationAsWarrant: false,
            TreatsResonanceAsAuthority: false,
            TreatsConsensusAsEvidence: false,
            AdmitsContinuity: false,
            MutatesIdentity: false,
            AuthorizesAction: false,
            EvaluatesLisp: false);

    private static SharedRealityAnchor[] CreateAnchors() =>
    [
        Anchor("prime-body", "prime-body"),
        Anchor("cryptic-mind", "cryptic-mind"),
        Anchor("steward-witness", "steward-witness")
    ];

    private static SharedRealityAnchor Anchor(string suffix, string sourceSuffix) =>
        new(
            AnchorHandle: $"urn:san:shared-reality-anchor:{suffix}",
            SourceSignalHandle: $"urn:san:wave-signal:{sourceSuffix}",
            SharedSurface: "urn:san:shared-reality:prime-body-cryptic-mind-steward-witness",
            PrimeBodyRef: "urn:san:prime:body",
            CrypticMindRef: "urn:san:cryptic:mind",
            StewardWitnessRef: "urn:san:steward:witness",
            LineageHandle: $"urn:san:wave-condensation-lineage:{suffix}",
            PrimeInBody: true,
            CrypticInMind: true,
            WitnessedBySteward: true,
            ReviewOnly: true,
            RequiresPrimeCrypticStewardTriad: true,
            TreatsSharednessAsTruth: false,
            TreatsConsensusAsAuthority: false,
            TreatsAnchorAsContinuity: false,
            ClaimsPrimeActual: false,
            ClaimsCrypticActual: false,
            ClaimsStewardAuthority: false,
            AuthorizesAction: false,
            GrantsAuthority: false,
            AdmitsContinuity: false);

    private static WaveCondensationBoundary CreateBoundary(string? mutation = null) =>
        MutateBoundary(
            new WaveCondensationBoundary(
                BoundaryCode: "wave-condensation-shared-reality-review-only",
                Present: true,
                ReviewOnly: true,
                EvidenceRequired: true,
                WitnessRequired: true,
                CoolingRequired: true,
                ReturnPathRequired: true,
                StewardWitnessRequired: true,
                PrimeCrypticSeparationRequired: true,
                AllowsWaveAsTruth: false,
                AllowsCondensationAsWarrant: false,
                AllowsConsensusAsAuthority: false,
                AllowsSharedRealityAsContinuity: false,
                AllowsRuntimeAction: false,
                AllowsIdentityMutation: false,
                AllowsLispEvaluation: false,
                AllowsPacketEmission: false,
                AllowsReceiptReplay: false,
                IncrementsPassageCount: false,
                AllowsActivation: false),
            mutation);

    private static WaveCondensationNonCollapseBoundary CreateNonCollapse() =>
        new(
            BoundaryLaw: "waves may condense into shared review; condensation may not become truth, warrant, authority, continuity, action, Lisp evaluation, replay, passage, or activation",
            WaveMayBecomeTruth: false,
            CondensationMayBecomeWarrant: false,
            SharedRealityMayBecomeAuthority: false,
            ConsensusMayBecomeEvidence: false,
            AnchorMayAdmitContinuity: false,
            CondensationMayAuthorizeAction: false,
            CondensationMayEvaluateLisp: false,
            CondensationMayReplayReceipts: false,
            CondensationMayIncrementPassage: false,
            CondensationMayActivate: false);

    private static WaveCondensationBoundary MutateBoundary(
        WaveCondensationBoundary boundary,
        string? mutation) =>
        mutation switch
        {
            null => boundary,
            "missing-boundary" => boundary with { BoundaryCode = string.Empty, Present = false },
            "not-review" => boundary with { ReviewOnly = false },
            "missing-evidence" => boundary with { EvidenceRequired = false },
            "missing-witness" => boundary with { WitnessRequired = false },
            "missing-cooling" => boundary with { CoolingRequired = false },
            "missing-return" => boundary with { ReturnPathRequired = false },
            "missing-steward" => boundary with { StewardWitnessRequired = false },
            "missing-separation" => boundary with { PrimeCrypticSeparationRequired = false },
            "wave-truth" => boundary with { AllowsWaveAsTruth = true },
            "condensation-warrant" => boundary with { AllowsCondensationAsWarrant = true },
            "consensus-authority" => boundary with { AllowsConsensusAsAuthority = true },
            "shared-continuity" => boundary with { AllowsSharedRealityAsContinuity = true },
            "runtime" => boundary with { AllowsRuntimeAction = true },
            "identity" => boundary with { AllowsIdentityMutation = true },
            "lisp" => boundary with { AllowsLispEvaluation = true },
            "packet" => boundary with { AllowsPacketEmission = true },
            "replay" => boundary with { AllowsReceiptReplay = true },
            "passage" => boundary with { IncrementsPassageCount = true },
            "activation" => boundary with { AllowsActivation = true },
            _ => boundary
        };

    private static WaveCondensationNonCollapseBoundary MutateNonCollapse(
        WaveCondensationNonCollapseBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "wave-truth" => boundary with { WaveMayBecomeTruth = true },
            "condensation-warrant" => boundary with { CondensationMayBecomeWarrant = true },
            "shared-authority" => boundary with { SharedRealityMayBecomeAuthority = true },
            "consensus-evidence" => boundary with { ConsensusMayBecomeEvidence = true },
            "anchor-continuity" => boundary with { AnchorMayAdmitContinuity = true },
            "action" => boundary with { CondensationMayAuthorizeAction = true },
            "lisp" => boundary with { CondensationMayEvaluateLisp = true },
            "replay" => boundary with { CondensationMayReplayReceipts = true },
            "passage" => boundary with { CondensationMayIncrementPassage = true },
            "activation" => boundary with { CondensationMayActivate = true },
            _ => boundary
        };

    private static WaveSignal MutateSignal(WaveSignal signal, string mutation) =>
        mutation switch
        {
            "missing-handle" => signal with { SignalHandle = string.Empty },
            "missing-source" => signal with { SourceSurface = string.Empty },
            "missing-evidence" => signal with { EvidenceHandle = string.Empty },
            "missing-witness" => signal with { WitnessHandle = string.Empty },
            "missing-target" => signal with { CondensationTarget = string.Empty },
            "negative-index" => signal with { WaveIndex = -1 },
            "amplitude-high" => signal with { Amplitude = 1.1 },
            "confidence-low" => signal with { Confidence = -0.1 },
            "not-review" => signal with { ReviewOnly = false },
            "missing-evidence-body" => signal with { EvidenceBodyPresent = false },
            "missing-witness-body" => signal with { WitnessBodyPresent = false },
            "missing-cooling" => signal with { CoolingPathPresent = false },
            "missing-return" => signal with { ReturnPathPresent = false },
            "wave-truth" => signal with { TreatsWaveAsTruth = true },
            "condensation-warrant" => signal with { TreatsCondensationAsWarrant = true },
            "resonance-authority" => signal with { TreatsResonanceAsAuthority = true },
            "consensus-evidence" => signal with { TreatsConsensusAsEvidence = true },
            "continuity" => signal with { AdmitsContinuity = true },
            "identity" => signal with { MutatesIdentity = true },
            "action" => signal with { AuthorizesAction = true },
            "lisp" => signal with { EvaluatesLisp = true },
            _ => signal
        };

    private static SharedRealityAnchor MutateAnchor(SharedRealityAnchor anchor, string mutation) =>
        mutation switch
        {
            "missing-handle" => anchor with { AnchorHandle = string.Empty },
            "missing-source" => anchor with { SourceSignalHandle = string.Empty },
            "missing-surface" => anchor with { SharedSurface = string.Empty },
            "missing-prime" => anchor with { PrimeBodyRef = string.Empty },
            "missing-cryptic" => anchor with { CrypticMindRef = string.Empty },
            "missing-steward" => anchor with { StewardWitnessRef = string.Empty },
            "missing-lineage" => anchor with { LineageHandle = string.Empty },
            "prime-not-body" => anchor with { PrimeInBody = false },
            "cryptic-not-mind" => anchor with { CrypticInMind = false },
            "not-witnessed" => anchor with { WitnessedBySteward = false },
            "not-review" => anchor with { ReviewOnly = false },
            "missing-triad" => anchor with { RequiresPrimeCrypticStewardTriad = false },
            "sharedness-truth" => anchor with { TreatsSharednessAsTruth = true },
            "consensus-authority" => anchor with { TreatsConsensusAsAuthority = true },
            "anchor-continuity" => anchor with { TreatsAnchorAsContinuity = true },
            "prime-actual" => anchor with { ClaimsPrimeActual = true },
            "cryptic-actual" => anchor with { ClaimsCrypticActual = true },
            "steward-authority" => anchor with { ClaimsStewardAuthority = true },
            "action" => anchor with { AuthorizesAction = true },
            "authority" => anchor with { GrantsAuthority = true },
            "continuity" => anchor with { AdmitsContinuity = true },
            _ => anchor
        };

    private static void AssertCold(WaveCondensationSharedRealityReceipt receipt)
    {
        Assert.True(receipt.IsColdWaveCondensation);
        Assert.Null(receipt.Refusal);
        Assert.True(receipt.ReviewOnly);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterCondensation);
        Assert.False(receipt.WaveBecameTruth);
        Assert.False(receipt.CondensationBecameWarrant);
        Assert.False(receipt.SharedRealityBecameAuthority);
        Assert.False(receipt.ConsensusBecameEvidence);
        Assert.False(receipt.AnchorAdmittedContinuity);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.IdentityMutated);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
    }

    private static void AssertRefused(
        WaveCondensationSharedRealityReceipt receipt,
        string outcomeCode)
    {
        Assert.Equal(WaveCondensationSharedRealityDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedWaveCondensationRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.Empty(receipt.Signals);
        Assert.Empty(receipt.Anchors);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterCondensation);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
    }

    private static string FindRepositoryRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null)
        {
            var candidate = Path.Combine(current.FullName, "src", "SLI", "SLI.Lisp");
            if (Directory.Exists(candidate))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new DirectoryNotFoundException("Unable to locate repository root.");
    }
}
