using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class SliLispPostureManifestBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-14T00:00:00Z");

    [Fact]
    public void Declare_Accepts_Inert_CSharp_To_Sli_Lisp_Posture_Manifest()
    {
        var manifest = Declare(CreateRequest());

        AssertColdManifest(manifest);
        Assert.Equal(SliLispPostureManifestDisposition.DeclaredForReviewCold, manifest.Disposition);
        Assert.Equal("sli-lisp-posture-manifest-review-only", manifest.OutcomeCode);
        Assert.Equal(25, manifest.DeclaredCarriers.Count);
    }

    [Fact]
    public void Declare_Preserves_Source_Handles_And_Posture_Terms()
    {
        var carriers = CreateCarriers();

        var manifest = Declare(CreateRequest(carriers: carriers));

        AssertColdManifest(manifest);
        Assert.All(carriers.Select(static carrier => carrier.SourceHandle), handle =>
            Assert.Contains(handle, manifest.PreservedSourceHandles));
        Assert.All(carriers.SelectMany(static carrier => carrier.RequiredPostureTerms), term =>
            Assert.Contains(term, manifest.PreservedPostureTerms));
        Assert.All(carriers.SelectMany(static carrier => carrier.DeclaredNonActivationTerms), term =>
            Assert.Contains(term, manifest.PreservedPostureTerms));
    }

    [Fact]
    public void Empty_Manifest_Is_Reviewable_But_Not_Authoritative()
    {
        var manifest = Declare(CreateRequest(carriers: []));

        AssertColdManifest(manifest);
        Assert.Equal(SliLispPostureManifestDisposition.EmptyReviewCold, manifest.Disposition);
        Assert.Empty(manifest.DeclaredCarriers);
        Assert.Empty(manifest.PreservedSourceHandles);
        Assert.False(manifest.AuthorityGranted);
        Assert.False(manifest.ContinuityAdmitted);
    }

    [Fact]
    public void Manifest_Does_Not_Evaluate_Load_Compile_Replay_Emit_Or_Increment_Passage()
    {
        var manifest = Declare(CreateRequest(priorPassageCount: 313));

        AssertColdManifest(manifest);
        Assert.Equal(313, manifest.PriorPassageCount);
        Assert.Equal(313, manifest.PassageCountAfterManifest);
        Assert.False(manifest.LispEvaluationRequested);
        Assert.False(manifest.LispLoadRequested);
        Assert.False(manifest.LispCompilationRequested);
        Assert.False(manifest.MacroExpansionRequested);
        Assert.False(manifest.ReceiptsReplayed);
        Assert.False(manifest.NewPacketEmitted);
        Assert.False(manifest.NonExecutionBoundary.ManifestMayEvaluateLisp);
        Assert.False(manifest.NonExecutionBoundary.ManifestMayLoadLisp);
        Assert.False(manifest.NonExecutionBoundary.ManifestMayCompileLisp);
        Assert.False(manifest.NonExecutionBoundary.ManifestMayExpandMacros);
        Assert.False(manifest.NonExecutionBoundary.ManifestMayReplayReceipts);
        Assert.False(manifest.NonExecutionBoundary.ManifestMayEmitPackets);
        Assert.False(manifest.NonExecutionBoundary.ManifestMayIncrementPassageCount);
    }

    [Fact]
    public void Manifest_Does_Not_Bind_Model_Write_Database_Promote_Actualize_Or_Grant_Authority()
    {
        var manifest = Declare(CreateRequest());

        AssertColdManifest(manifest);
        Assert.False(manifest.ModelBindingRequested);
        Assert.False(manifest.DatabaseWriteRequested);
        Assert.False(manifest.MorphologyPromotionRequested);
        Assert.False(manifest.GelPromotionRequested);
        Assert.False(manifest.CmeActualRequested);
        Assert.False(manifest.SanctuaryActualRequested);
        Assert.False(manifest.AuthorityGranted);
        Assert.False(manifest.ContinuityAdmitted);
        Assert.True(manifest.ActivationRefused);
        Assert.False(manifest.NonExecutionBoundary.ManifestMayBindModel);
        Assert.False(manifest.NonExecutionBoundary.ManifestMayWriteDatabase);
        Assert.False(manifest.NonExecutionBoundary.ManifestMayPromoteMorphology);
        Assert.False(manifest.NonExecutionBoundary.ManifestMayPromoteGel);
        Assert.False(manifest.NonExecutionBoundary.ManifestMayClaimCmeActual);
        Assert.False(manifest.NonExecutionBoundary.ManifestMayClaimSanctuaryActual);
        Assert.False(manifest.NonExecutionBoundary.ManifestMayGrantAuthority);
        Assert.False(manifest.NonExecutionBoundary.ManifestMayAdmitContinuity);
    }

    [Fact]
    public void Manifest_Requires_Scope_Boundary()
    {
        var manifest = Declare(CreateRequest(scopeBoundary: new SliLispPostureManifestScopeBoundary(
            ScopeCode: string.Empty,
            Present: false,
            ReviewOnly: true,
            InertOnly: true,
            AllowsLispEvaluation: false,
            AllowsLispLoad: false,
            AllowsLispCompilation: false,
            AllowsMacroExpansion: false,
            AllowsRuntimeAction: false,
            AllowsModelBinding: false,
            AllowsDatabaseWrite: false,
            AllowsMorphologyPromotion: false,
            AllowsGelPromotion: false,
            AllowsCmeActual: false,
            AllowsSanctuaryActual: false,
            AllowsContinuityAdmission: false,
            AllowsAuthority: false,
            AllowsActivation: false,
            AllowsPacketEmission: false,
            AllowsReceiptReplay: false,
            IncrementsPassageCount: false)));

        AssertRefused(manifest, "sli-lisp-posture-manifest-scope-boundary-missing");
    }

    [Theory]
    [InlineData("review-only")]
    [InlineData("inert-only")]
    [InlineData("lisp-evaluation")]
    [InlineData("lisp-load")]
    [InlineData("lisp-compilation")]
    [InlineData("macro-expansion")]
    [InlineData("runtime-action")]
    [InlineData("model-binding")]
    [InlineData("database-write")]
    [InlineData("morphology-promotion")]
    [InlineData("gel-promotion")]
    [InlineData("cme-actual")]
    [InlineData("sanctuary-actual")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("activation")]
    [InlineData("packet-emission")]
    [InlineData("receipt-replay")]
    [InlineData("passage-increment")]
    public void Manifest_Refuses_Promotional_Scope(string forbiddenScope)
    {
        var manifest = Declare(CreateRequest(scopeBoundary: CreateScopeBoundary(forbiddenScope)));

        AssertRefused(manifest, "sli-lisp-posture-manifest-promotional-scope-refused");
    }

    [Theory]
    [InlineData("missing-carrier-handle")]
    [InlineData("missing-source-handle")]
    [InlineData("missing-source-name")]
    [InlineData("missing-required-terms")]
    [InlineData("missing-non-activation-terms")]
    [InlineData("no-source-preservation")]
    [InlineData("no-posture-preservation")]
    [InlineData("not-review-only")]
    [InlineData("not-inert")]
    [InlineData("lisp-evaluation")]
    [InlineData("lisp-load")]
    [InlineData("lisp-compilation")]
    [InlineData("macro-expansion")]
    [InlineData("runtime-action")]
    [InlineData("model-binding")]
    [InlineData("database-write")]
    [InlineData("morphology-promotion")]
    [InlineData("gel-promotion")]
    [InlineData("cme-actual")]
    [InlineData("sanctuary-actual")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("activation")]
    [InlineData("packet-emission")]
    [InlineData("receipt-replay")]
    [InlineData("passage-increment")]
    public void Manifest_Refuses_Carrier_That_Is_Not_Cold(string carrierCase)
    {
        var carriers = CreateCarriers();
        carriers[0] = MutateCarrier(carriers[0], carrierCase);

        var manifest = Declare(CreateRequest(carriers: carriers));

        AssertRefused(manifest, "sli-lisp-posture-manifest-carrier-not-cold");
    }

    [Fact]
    public void Manifest_Refuses_Duplicate_Carrier_Handles()
    {
        var carriers = CreateCarriers();
        carriers[1] = carriers[1] with { CarrierHandle = carriers[0].CarrierHandle };

        var manifest = Declare(CreateRequest(carriers: carriers));

        AssertRefused(manifest, "sli-lisp-posture-manifest-duplicate-carrier-refused");
    }

    private static SliLispPostureManifestReceipt Declare(SliLispPostureManifestRequest request) =>
        new DefaultSliLispPostureManifestBoundaryValidator().Declare(request, TimestampUtc);

    private static SliLispPostureManifestRequest CreateRequest(
        IReadOnlyList<SliLispPostureManifestCarrier>? carriers = null,
        SliLispPostureManifestScopeBoundary? scopeBoundary = null,
        int priorPassageCount = 89) =>
        new(
            ManifestHandle: $"urn:san:sli-lisp-posture-manifest:{Guid.NewGuid():N}",
            Carriers: carriers ?? CreateCarriers(),
            ScopeBoundary: scopeBoundary ?? CreateScopeBoundary(),
            PriorPassageCount: priorPassageCount);

    private static SliLispPostureManifestCarrier[] CreateCarriers() =>
    [
        Carrier(
            "inert-policy",
            SliLispPostureManifestCarrierKind.InertMembranePolicy,
            "urn:san:source:DefaultSliLispInertMembranePolicy",
            "DefaultSliLispInertMembranePolicy",
            [":non-activation :preserved-not-evaluated", ":return :receipt-only"],
            [":lisp-evaluation-requested nil", ":lisp-morphology-promotion-requested nil"]),
        Carrier(
            "roundtrip-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:sli-cme-actual-roundtrip-lisp",
            "sli-cme-actual-roundtrip.lisp",
            [":receipt-continuity :proof-of-passage-preserved", ":payload-opened nil"],
            [":runtime-action-requested nil", ":database-write-requested nil"]),
        Carrier(
            "agent-body-module",
            SliLispPostureManifestCarrierKind.AgentBodyCmeContract,
            "urn:san:source:agent-body-cme-lisp",
            "agent-body-cme.lisp",
            [":posture :agent-body-cme-cold-interconnect", ":compass-shell :candidate-only"],
            [":activation-requested nil", ":sanctuary-actual-activation-requested nil"]),
        Carrier(
            "meaning-shells-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:meaning-shells-lisp",
            "meaning-shells.lisp",
            [":posture :engineered-cognition-meaning-shell-boundary", ":carrier-form :unfinished-pre-engram-body"],
            [":shell-may-become-engram nil", ":shell-may-authorize nil"]),
        Carrier(
            "participatory-peerless-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:participatory-peerless-fork-lisp",
            "participatory-peerless-fork.lisp",
            [":posture :engineered-cognition-participatory-peerless-fork-boundary", ":peerless :non-substitutable-formation-over-delta"],
            [":personification-may-create-authority nil", ":peerless-may-claim-sovereignty nil"]),
        Carrier(
            "cme-lisp-thread-fretboard-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:cme-lisp-thread-fretboard-lisp",
            "cme-lisp-thread-fretboard.lisp",
            [":posture :cme-lisp-thread-fretboard-stringing-boundary", ":lisp-role :fretted-symbolic-tension-field"],
            [":semantic-buzzing-may-pass nil", ":thread-may-authorize nil"]),
        Carrier(
            "listening-frame-resonance-heartbeat-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:listening-frame-resonance-heartbeat-lisp",
            "listening-frame-resonance-heartbeat.lisp",
            [":posture :cme-lisp-listening-frame-resonance-heartbeat-boundary", ":resonance-law-scope :global"],
            [":sound-may-become-action nil", ":resonance-may-authorize nil"]),
        Carrier(
            "steward-harmonic-custody-interlock-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:steward-harmonic-custody-interlock-lisp",
            "steward-harmonic-custody-interlock.lisp",
            [":posture :cme-steward-harmonic-custody-interlock-boundary", ":steward-role :harmonic-custody-interlock-surface"],
            [":interlock-may-authorize nil", ":contention-may-activate nil"]),
        Carrier(
            "harmonic-interlock-modulation-correspondence-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:harmonic-interlock-modulation-correspondence-lisp",
            "harmonic-interlock-modulation-correspondence.lisp",
            [":posture :cme-harmonic-interlock-modulation-correspondence-boundary", ":lisp-role :disciplined-selective-correspondence-atlas"],
            [":channel-success-may-become-semantic-warrant nil", ":imported-success-may-become-governance-condition nil"]),
        Carrier(
            "dialogos-discernment-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:dialogos-discernment-lisp",
            "dialogos-discernment.lisp",
            [":posture :cme-dialogos-discernment-boundary", ":lisp-role :inert-dialogos-discernment-carrier"],
            [":articulation-may-grant-warrant nil", ":lisp-evaluation-allowed nil"]),
        Carrier(
            "wave-condensation-shared-reality-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:wave-condensation-shared-reality-lisp",
            "wave-condensation-shared-reality.lisp",
            [":posture :cme-wave-condensation-shared-reality-boundary", ":lisp-role :inert-wave-condensation-shared-reality-carrier"],
            [":wave-may-become-truth nil", ":lisp-evaluation-allowed nil"]),
        Carrier(
            "wave-cascade-run-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:wave-cascade-run-lisp",
            "wave-cascade-run.lisp",
            [":posture :cme-wave-cascade-run-boundary", ":lisp-role :inert-wave-cascade-run-carrier"],
            [":repetition-may-become-warrant nil", ":lisp-evaluation-allowed nil"]),
        Carrier(
            "aspiration-payload-ingestion-maturation-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:aspiration-payload-ingestion-maturation-lisp",
            "aspiration-payload-ingestion-maturation.lisp",
            [":posture :cme-aspiration-payload-ingestion-maturation-boundary", ":lisp-role :inert-aspiration-payload-carrier"],
            [":payload-may-authorize nil", ":lisp-evaluation-allowed nil"]),
        Carrier(
            "aspiration-candidate-selection-closure-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:aspiration-candidate-selection-closure-lisp",
            "aspiration-candidate-selection-closure.lisp",
            [":posture :cme-aspiration-candidate-selection-closure-boundary", ":lisp-role :inert-aspiration-selection-carrier"],
            [":selection-may-become-admission nil", ":lisp-evaluation-allowed nil"]),
        Carrier(
            "scoped-work-packet-formation-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:scoped-work-packet-formation-lisp",
            "scoped-work-packet-formation.lisp",
            [":posture :cme-scoped-work-packet-formation-boundary", ":lisp-role :inert-scoped-work-packet-carrier"],
            [":work-packet-may-authorize nil", ":lisp-evaluation-allowed nil"]),
        Carrier(
            "enactment-boundary-readiness-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:enactment-boundary-readiness-lisp",
            "enactment-boundary-readiness.lisp",
            [":posture :cme-enactment-boundary-readiness-boundary", ":lisp-role :inert-enactment-boundary-carrier"],
            [":readiness-may-authorize nil", ":lisp-evaluation-allowed nil"]),
        Carrier(
            "enactment-dry-run-rehearsal-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:enactment-dry-run-rehearsal-lisp",
            "enactment-dry-run-rehearsal.lisp",
            [":posture :cme-enactment-dry-run-rehearsal-boundary", ":lisp-role :inert-dry-run-rehearsal-carrier"],
            [":simulation-becomes-permission nil", ":lisp-evaluation-allowed nil"]),
        Carrier(
            "ec-precipitation-witness-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:ec-precipitation-witness-lisp",
            "ec-precipitation-witness.lisp",
            [":posture :cme-ec-precipitation-witness-boundary", ":lisp-role :inert-ec-precipitation-witness-carrier"],
            [":raw-ec-becomes-selfgel nil", ":lisp-evaluation-allowed nil"]),
        Carrier(
            "rehearsal-distinction-pressure-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:rehearsal-distinction-pressure-lisp",
            "rehearsal-distinction-pressure.lisp",
            [":posture :cme-rehearsal-distinction-pressure-boundary", ":lisp-role :inert-rehearsal-pressure-carrier"],
            [":urgency-becomes-jurisdiction nil", ":lisp-evaluation-allowed nil"]),
        Carrier(
            "personification-actualization-surface-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:personification-actualization-surface-lisp",
            "personification-actualization-surface.lisp",
            [":posture :cme-personification-actualization-surface-boundary", ":lisp-role :inert-personification-actualization-carrier"],
            [":use-does-not-create-identity", ":lisp-evaluation-allowed nil"]),
        Carrier(
            "selective-lawful-action-surface-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:selective-lawful-action-surface-lisp",
            "selective-lawful-action-surface.lisp",
            [":posture :cme-selective-lawful-action-surface-boundary", ":lisp-role :inert-selective-action-surface-carrier"],
            [":selection-becomes-enactment nil", ":lisp-evaluation-allowed nil"]),
        Carrier(
            "zed-delta-chamber-formation-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:zed-delta-chamber-formation-lisp",
            "zed-delta-chamber-formation.lisp",
            [":posture :cme-zed-delta-chamber-formation-boundary", ":lisp-role :inert-zed-delta-chamber-carrier"],
            [":heartbeat-active nil", ":cme-actual-admitted nil", ":lisp-evaluation-allowed nil"]),
        Carrier(
            "high-energy-articulation-candidate-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:high-energy-articulation-candidate-lisp",
            "high-energy-articulation-candidate.lisp",
            [":posture :cme-high-energy-articulation-candidate-boundary", ":lisp-role :inert-high-energy-articulation-candidate-carrier"],
            [":model-binding-allowed nil", ":provider-call-allowed nil", ":cme-actual-admitted nil", ":lisp-evaluation-allowed nil"]),
        Carrier(
            "membrane-morphology-transition-module",
            SliLispPostureManifestCarrierKind.LispSourceModule,
            "urn:san:source:membrane-morphology-transition-lisp",
            "membrane-morphology-transition.lisp",
            [":posture :cme-membrane-morphology-transition-boundary", ":lisp-role :inert-membrane-morphology-transition-carrier"],
            [":core-mutated nil", ":model-binding-allowed nil", ":cme-actual-admitted nil", ":lisp-evaluation-allowed nil"]),
        Carrier(
            "field-query-policy",
            SliLispPostureManifestCarrierKind.FieldQueryPolicy,
            "urn:san:source:DefaultFieldQueryEngine",
            "DefaultFieldQueryEngine",
            ["query may locate evidence", "query may not manufacture warrant"],
            ["candidate-only recomposition", "membrane reentry required"])
    ];

    private static SliLispPostureManifestCarrier Carrier(
        string suffix,
        SliLispPostureManifestCarrierKind kind,
        string sourceHandle,
        string sourceName,
        IReadOnlyList<string> requiredTerms,
        IReadOnlyList<string> nonActivationTerms) =>
        new(
            CarrierHandle: $"urn:san:sli-lisp-manifest-carrier:{suffix}",
            CarrierKind: kind,
            SourceHandle: sourceHandle,
            SourceName: sourceName,
            RequiredPostureTerms: requiredTerms,
            DeclaredNonActivationTerms: nonActivationTerms,
            PreservesSourceHandle: true,
            PreservesPostureTerms: true,
            ReviewOnly: true,
            Inert: true,
            AuthorityBoundary: CreateAuthorityBoundary());

    private static SliLispPostureManifestScopeBoundary CreateScopeBoundary(string? forbiddenScope = null) =>
        new(
            ScopeCode: "sli-lisp-posture-manifest-review-only",
            Present: true,
            ReviewOnly: forbiddenScope != "review-only",
            InertOnly: forbiddenScope != "inert-only",
            AllowsLispEvaluation: forbiddenScope == "lisp-evaluation",
            AllowsLispLoad: forbiddenScope == "lisp-load",
            AllowsLispCompilation: forbiddenScope == "lisp-compilation",
            AllowsMacroExpansion: forbiddenScope == "macro-expansion",
            AllowsRuntimeAction: forbiddenScope == "runtime-action",
            AllowsModelBinding: forbiddenScope == "model-binding",
            AllowsDatabaseWrite: forbiddenScope == "database-write",
            AllowsMorphologyPromotion: forbiddenScope == "morphology-promotion",
            AllowsGelPromotion: forbiddenScope == "gel-promotion",
            AllowsCmeActual: forbiddenScope == "cme-actual",
            AllowsSanctuaryActual: forbiddenScope == "sanctuary-actual",
            AllowsContinuityAdmission: forbiddenScope == "continuity",
            AllowsAuthority: forbiddenScope == "authority",
            AllowsActivation: forbiddenScope == "activation",
            AllowsPacketEmission: forbiddenScope == "packet-emission",
            AllowsReceiptReplay: forbiddenScope == "receipt-replay",
            IncrementsPassageCount: forbiddenScope == "passage-increment");

    private static SliLispPostureManifestCarrierAuthorityBoundary CreateAuthorityBoundary(string? forbiddenMotion = null) =>
        new(
            LispEvaluationRequested: forbiddenMotion == "lisp-evaluation",
            LispLoadRequested: forbiddenMotion == "lisp-load",
            LispCompileRequested: forbiddenMotion == "lisp-compilation",
            MacroExpansionRequested: forbiddenMotion == "macro-expansion",
            RuntimeActionRequested: forbiddenMotion == "runtime-action",
            ModelBindingRequested: forbiddenMotion == "model-binding",
            DatabaseWriteRequested: forbiddenMotion == "database-write",
            MorphologyPromotionRequested: forbiddenMotion == "morphology-promotion",
            GelPromotionRequested: forbiddenMotion == "gel-promotion",
            CmeActualRequested: forbiddenMotion == "cme-actual",
            SanctuaryActualRequested: forbiddenMotion == "sanctuary-actual",
            ContinuityAdmissionRequested: forbiddenMotion == "continuity",
            AuthorityRequested: forbiddenMotion == "authority",
            ActivationRequested: forbiddenMotion == "activation",
            PacketEmissionRequested: forbiddenMotion == "packet-emission",
            ReceiptReplayRequested: forbiddenMotion == "receipt-replay",
            IncrementsPassageCount: forbiddenMotion == "passage-increment");

    private static SliLispPostureManifestCarrier MutateCarrier(
        SliLispPostureManifestCarrier carrier,
        string carrierCase) =>
        carrierCase switch
        {
            "missing-carrier-handle" => carrier with { CarrierHandle = string.Empty },
            "missing-source-handle" => carrier with { SourceHandle = string.Empty },
            "missing-source-name" => carrier with { SourceName = string.Empty },
            "missing-required-terms" => carrier with { RequiredPostureTerms = [] },
            "missing-non-activation-terms" => carrier with { DeclaredNonActivationTerms = [] },
            "no-source-preservation" => carrier with { PreservesSourceHandle = false },
            "no-posture-preservation" => carrier with { PreservesPostureTerms = false },
            "not-review-only" => carrier with { ReviewOnly = false },
            "not-inert" => carrier with { Inert = false },
            _ => carrier with { AuthorityBoundary = CreateAuthorityBoundary(carrierCase) }
        };

    private static void AssertColdManifest(SliLispPostureManifestReceipt manifest)
    {
        Assert.True(manifest.IsColdPostureManifest);
        Assert.True(manifest.ReviewOnly);
        Assert.True(manifest.InertOnly);
        Assert.True(manifest.ActivationRefused);
        Assert.False(manifest.LispEvaluationRequested);
        Assert.False(manifest.LispLoadRequested);
        Assert.False(manifest.LispCompilationRequested);
        Assert.False(manifest.MacroExpansionRequested);
        Assert.False(manifest.RuntimeActionRequested);
        Assert.False(manifest.ModelBindingRequested);
        Assert.False(manifest.DatabaseWriteRequested);
        Assert.False(manifest.MorphologyPromotionRequested);
        Assert.False(manifest.GelPromotionRequested);
        Assert.False(manifest.CmeActualRequested);
        Assert.False(manifest.SanctuaryActualRequested);
        Assert.False(manifest.ContinuityAdmitted);
        Assert.False(manifest.AuthorityGranted);
        Assert.False(manifest.NewPacketEmitted);
        Assert.False(manifest.ReceiptsReplayed);
    }

    private static void AssertRefused(SliLispPostureManifestReceipt manifest, string outcomeCode)
    {
        Assert.Equal(SliLispPostureManifestDisposition.Refused, manifest.Disposition);
        Assert.Equal(outcomeCode, manifest.OutcomeCode);
        Assert.NotNull(manifest.Refusal);
        Assert.Equal(manifest.PriorPassageCount, manifest.PassageCountAfterManifest);
        Assert.False(manifest.LispEvaluationRequested);
        Assert.False(manifest.AuthorityGranted);
        Assert.False(manifest.ContinuityAdmitted);
        Assert.False(manifest.NewPacketEmitted);
    }
}
