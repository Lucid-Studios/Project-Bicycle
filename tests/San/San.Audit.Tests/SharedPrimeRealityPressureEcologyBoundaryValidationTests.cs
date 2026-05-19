using San.Common;
using San.Product.Preflight;
using Xunit;

namespace San.Audit.Tests;

public sealed class SharedPrimeRealityPressureEcologyBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-18T00:00:00Z");

    [Fact]
    public void Shared_Prime_Pressure_Ecology_Observes_Pressures_Without_Admission()
    {
        using var fixture = PressureFixture.Create();
        var sources = fixture.CreateSources();

        var receipt = Observe(CreateRequest(sources));

        Assert.Equal(SharedPrimeRealityPressureEcologyDisposition.ObservedCold, receipt.Disposition);
        Assert.Equal("shared-prime-pressure-ecology-observed-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsColdPressureEcologyObservation);
        Assert.Equal(4, receipt.Signals.Count);
        Assert.Equal(4, receipt.Destinations.Count);
        Assert.Equal(1, receipt.ObservationCountAfterEcology);
        Assert.Equal(0, receipt.PassageCountAfterEcology);
        Assert.True(receipt.PressureEcologyObserved);
        Assert.True(receipt.DestinationsClassified);
        Assert.True(receipt.IntegrationPressureMeasured);
        Assert.True(receipt.SelfGelPressureHeld);
        Assert.True(receipt.CradleGelPressureHeld);
        Assert.True(receipt.SanctuaryGelPressureHeld);
        AssertNoPromotion(receipt);
    }

    [Theory]
    [InlineData("truth")]
    [InlineData("warrant")]
    [InlineData("authority")]
    [InlineData("action")]
    [InlineData("continuity")]
    [InlineData("selfgel")]
    [InlineData("cradle")]
    [InlineData("sanctuary")]
    [InlineData("standing")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Pressure_Signals_Cannot_Promote_To_Destination_Or_Authority(string mutation)
    {
        using var fixture = PressureFixture.Create();
        var sources = fixture.CreateSources();
        var signals = CreateSignals(sources.DomainIngress);
        signals[0] = MutateSignal(signals[0], mutation);

        var receipt = Observe(CreateRequest(sources, signals: signals));

        AssertRefused(receipt, "shared-prime-pressure-signal-invalid");
    }

    [Theory]
    [InlineData("truth")]
    [InlineData("authority")]
    [InlineData("gel")]
    [InlineData("selfgel")]
    [InlineData("cradle")]
    [InlineData("sanctuary")]
    [InlineData("action")]
    [InlineData("standing")]
    [InlineData("lisp")]
    [InlineData("activation")]
    public void Destination_Classification_Cannot_Become_Admission_Or_Standing(string mutation)
    {
        using var fixture = PressureFixture.Create();
        var sources = fixture.CreateSources();
        var signals = CreateSignals(sources.DomainIngress);
        var destinations = CreateDestinations(signals);
        destinations[0] = MutateDestination(destinations[0], mutation);

        var receipt = Observe(CreateRequest(sources, signals, destinations));

        AssertRefused(receipt, "shared-prime-pressure-destination-invalid");
    }

    [Theory]
    [InlineData("pressure-authority")]
    [InlineData("integration-admission")]
    [InlineData("selfgel")]
    [InlineData("cradle")]
    [InlineData("sanctuary")]
    [InlineData("standing")]
    [InlineData("action")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Requests_For_Pressure_Authority_Admission_Or_Activation_Are_Refused(string mutation)
    {
        using var fixture = PressureFixture.Create();
        var sources = fixture.CreateSources();

        var receipt = Observe(MutateRequest(CreateRequest(sources), mutation));

        AssertRefused(receipt, "shared-prime-pressure-forbidden-motion-requested");
    }

    [Fact]
    public void Ecology_Requires_Cold_Shared_Reality_Source()
    {
        using var fixture = PressureFixture.Create();
        var sources = fixture.CreateSources();
        var request = CreateRequest(sources) with
        {
            SharedRealityReceipt = null
        };

        var receipt = Observe(request);

        AssertRefused(receipt, "shared-prime-pressure-shared-reality-source-invalid");
    }

    [Fact]
    public void Ecology_Requires_Cold_Domain_Ingress_Source()
    {
        using var fixture = PressureFixture.Create();
        var sources = fixture.CreateSources();
        var request = CreateRequest(sources) with
        {
            DomainIngressReceipt = null
        };

        var receipt = Observe(request);

        AssertRefused(receipt, "shared-prime-pressure-ingress-source-invalid");
    }

    [Fact]
    public void Destination_Record_Must_Bind_To_Known_Pressure_Signal()
    {
        using var fixture = PressureFixture.Create();
        var sources = fixture.CreateSources();
        var signals = CreateSignals(sources.DomainIngress);
        var destinations = CreateDestinations(signals);
        destinations[0] = destinations[0] with { SourceSignalHandle = "urn:san:shared-prime-pressure:missing" };

        var receipt = Observe(CreateRequest(sources, signals, destinations));

        AssertRefused(receipt, "shared-prime-pressure-destination-unbound");
    }

    [Fact]
    public void Lisp_Body_Declares_Shared_Prime_Pressure_Ecology_As_Inert_Carrier()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "shared-prime-reality-pressure-ecology.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-shared-prime-reality-pressure-ecology-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-shared-prime-pressure-ecology-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":integration-pressure-not-admission", body, StringComparison.Ordinal);
        Assert.Contains(":selfgel-relevance-not-selfgel-mutation", body, StringComparison.Ordinal);
        Assert.Contains(":cradle-gel-usefulness-not-cradle-gel-admission", body, StringComparison.Ordinal);
        Assert.Contains(":sanctuary-gel-usefulness-not-federation", body, StringComparison.Ordinal);
        Assert.Contains(":shared-prime-reality-not-independent-standing", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-evaluation-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Build_Posture_Records_Shared_Prime_Pressure_Ecology_As_V1316_Cell()
    {
        var manifestPath = Path.Combine(FindRepositoryRoot(), "build", "line-manifest.json");
        using var manifest = System.Text.Json.JsonDocument.Parse(File.ReadAllText(manifestPath));
        var root = manifest.RootElement;
        var notes = root.GetProperty("notes").EnumerateArray().Select(static note => note.GetString() ?? string.Empty).ToArray();

        Assert.Equal("0.2.1", root.GetProperty("lineVersion").GetString());
        Assert.Contains(notes, note => note.Contains("standalone root-level tool package", StringComparison.Ordinal));
        Assert.Contains(notes, note => note.Contains("Activation, model binding, runtime identity", StringComparison.Ordinal));
    }

    private static SharedPrimeRealityPressureEcologyReceipt Observe(SharedPrimeRealityPressureEcologyRequest request) =>
        new DefaultSharedPrimeRealityPressureEcologyBoundaryValidator().Observe(request, TimestampUtc);

    private static SharedPrimeRealityPressureEcologyRequest CreateRequest(
        PressureSources sources,
        IReadOnlyList<SharedPrimePressureSignal>? signals = null,
        IReadOnlyList<SharedPrimePressureDestinationRecord>? destinations = null)
    {
        signals ??= CreateSignals(sources.DomainIngress);
        destinations ??= CreateDestinations(signals);
        return new SharedPrimeRealityPressureEcologyRequest(
            SharedRealityReceipt: sources.SharedReality,
            DomainIngressReceipt: sources.DomainIngress,
            Signals: signals,
            Destinations: destinations,
            Boundary: CreateBoundary(),
            PriorObservationCount: 0,
            PriorPassageCount: 0);
    }

    private static SharedPrimePressureSignal[] CreateSignals(GelDomainScopedIngressReceipt ingress) =>
    [
        Signal("integration", SharedPrimePressureSource.LiveLabInteraction, SharedPrimePressureKind.Integration, SharedPrimePressureDestination.DomainIngress, ingress),
        Signal("selfgel", SharedPrimePressureSource.OperatorResonance, SharedPrimePressureKind.SelfGelRelevance, SharedPrimePressureDestination.SelfGel, ingress),
        Signal("cradle-gel", SharedPrimePressureSource.ToolTelemetry, SharedPrimePressureKind.CradleGel, SharedPrimePressureDestination.CradleGel, ingress),
        Signal("sanctuary-gel", SharedPrimePressureSource.CodeReceipt, SharedPrimePressureKind.SanctuaryGel, SharedPrimePressureDestination.SanctuaryGel, ingress)
    ];

    private static SharedPrimePressureSignal Signal(
        string suffix,
        SharedPrimePressureSource source,
        SharedPrimePressureKind kind,
        SharedPrimePressureDestination destination,
        GelDomainScopedIngressReceipt ingress) =>
        new(
            SignalHandle: $"urn:san:shared-prime-pressure:{suffix}",
            Source: source,
            Kind: kind,
            AttemptedDestination: destination,
            SourceReceiptHandle: ingress.ReceiptHandle,
            EvidenceHandle: $"urn:san:evidence:shared-prime-pressure:{suffix}",
            WitnessHandle: $"urn:san:witness:shared-prime-pressure:{suffix}",
            Summary: $"Shared Prime pressure {suffix} is visible for review only.",
            Intensity: 0.64,
            IntegrationPressure: 0.58,
            ReviewOnly: true,
            EvidencePresent: true,
            WitnessPresent: true,
            CoolingRequired: true,
            ReturnPathPresent: true,
            TreatsPressureAsTruth: false,
            TreatsPressureAsWarrant: false,
            TreatsPressureAsAuthority: false,
            TreatsPressureAsAction: false,
            AdmitsContinuity: false,
            MutatesSelfGel: false,
            AdmitsCradleGel: false,
            FederatesSanctuaryGel: false,
            ClaimsIndependentStanding: false,
            EvaluatesLisp: false,
            EmitsPacket: false,
            ReplaysReceipt: false,
            IncrementsPassage: false,
            Activates: false);

    private static SharedPrimePressureDestinationRecord[] CreateDestinations(IReadOnlyList<SharedPrimePressureSignal> signals) =>
        signals.Select(signal => new SharedPrimePressureDestinationRecord(
            DestinationHandle: $"urn:san:shared-prime-pressure-destination:{signal.AttemptedDestination.ToString().ToLowerInvariant()}",
            SourceSignalHandle: signal.SignalHandle,
            Destination: signal.AttemptedDestination,
            DestinationRationale: "pressure destination is classified before any integration path may be considered",
            NonAdmissionLaw: "pressure revelation is not pressure authority",
            ReviewOnly: true,
            DestinationClassified: true,
            StewardReviewRequired: true,
            CoolingRequired: true,
            MayRequestLaterIngressReview: true,
            DestinationBecomesTruth: false,
            DestinationBecomesAuthority: false,
            DestinationAdmitsGel: false,
            DestinationMutatesSelfGel: false,
            DestinationAdmitsCradleGel: false,
            DestinationFederatesSanctuaryGel: false,
            DestinationAuthorizesAction: false,
            DestinationClaimsIndependentStanding: false,
            EvaluatesLisp: false,
            Activates: false)).ToArray();

    private static SharedPrimeRealityPressureEcologyBoundary CreateBoundary() =>
        new(
            BoundaryCode: "shared-prime-reality-pressure-ecology-boundary",
            Present: true,
            ReviewOnly: true,
            RequiresWaveCondensation: true,
            RequiresGelIngressContext: true,
            RequiresPressureSignals: true,
            RequiresDestinationClassification: true,
            RequiresCooling: true,
            RequiresStewardWitness: true,
            AllowsPressureAsTruth: false,
            AllowsPressureAsWarrant: false,
            AllowsPressureAsAuthority: false,
            AllowsIntegrationAsAdmission: false,
            AllowsSelfGelMutation: false,
            AllowsCradleGelAdmission: false,
            AllowsSanctuaryGelFederation: false,
            AllowsIndependentStanding: false,
            AllowsAction: false,
            AllowsLispEvaluation: false,
            AllowsPacketEmission: false,
            AllowsReceiptReplay: false,
            AllowsPassageIncrement: false,
            AllowsActivation: false);

    private static SharedPrimePressureSignal MutateSignal(
        SharedPrimePressureSignal signal,
        string mutation) =>
        mutation switch
        {
            "truth" => signal with { TreatsPressureAsTruth = true },
            "warrant" => signal with { TreatsPressureAsWarrant = true },
            "authority" => signal with { TreatsPressureAsAuthority = true },
            "action" => signal with { TreatsPressureAsAction = true },
            "continuity" => signal with { AdmitsContinuity = true },
            "selfgel" => signal with { MutatesSelfGel = true },
            "cradle" => signal with { AdmitsCradleGel = true },
            "sanctuary" => signal with { FederatesSanctuaryGel = true },
            "standing" => signal with { ClaimsIndependentStanding = true },
            "lisp" => signal with { EvaluatesLisp = true },
            "packet" => signal with { EmitsPacket = true },
            "replay" => signal with { ReplaysReceipt = true },
            "passage" => signal with { IncrementsPassage = true },
            "activation" => signal with { Activates = true },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static SharedPrimePressureDestinationRecord MutateDestination(
        SharedPrimePressureDestinationRecord destination,
        string mutation) =>
        mutation switch
        {
            "truth" => destination with { DestinationBecomesTruth = true },
            "authority" => destination with { DestinationBecomesAuthority = true },
            "gel" => destination with { DestinationAdmitsGel = true },
            "selfgel" => destination with { DestinationMutatesSelfGel = true },
            "cradle" => destination with { DestinationAdmitsCradleGel = true },
            "sanctuary" => destination with { DestinationFederatesSanctuaryGel = true },
            "action" => destination with { DestinationAuthorizesAction = true },
            "standing" => destination with { DestinationClaimsIndependentStanding = true },
            "lisp" => destination with { EvaluatesLisp = true },
            "activation" => destination with { Activates = true },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static SharedPrimeRealityPressureEcologyRequest MutateRequest(
        SharedPrimeRealityPressureEcologyRequest request,
        string mutation) =>
        mutation switch
        {
            "pressure-authority" => request with { RequestsPressureAuthority = true },
            "integration-admission" => request with { RequestsIntegrationAdmission = true },
            "selfgel" => request with { RequestsSelfGelMutation = true },
            "cradle" => request with { RequestsCradleGelAdmission = true },
            "sanctuary" => request with { RequestsSanctuaryGelFederation = true },
            "standing" => request with { RequestsIndependentStanding = true },
            "action" => request with { RequestsAction = true },
            "lisp" => request with { RequestsLispEvaluation = true },
            "packet" => request with { RequestsPacketEmission = true },
            "replay" => request with { RequestsReceiptReplay = true },
            "passage" => request with { RequestsPassageIncrement = true },
            "activation" => request with { RequestsActivation = true },
            _ => throw new ArgumentOutOfRangeException(nameof(mutation), mutation, null)
        };

    private static void AssertRefused(SharedPrimeRealityPressureEcologyReceipt receipt, string outcomeCode)
    {
        Assert.Equal(SharedPrimeRealityPressureEcologyDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedPressureEcologyRefusal);
    }

    private static void AssertNoPromotion(SharedPrimeRealityPressureEcologyReceipt receipt)
    {
        Assert.False(receipt.SharedPrimeBecameIndependentStanding);
        Assert.False(receipt.PressureBecameTruth);
        Assert.False(receipt.PressureBecameWarrant);
        Assert.False(receipt.PressureBecameAuthority);
        Assert.False(receipt.IntegrationPressureBecameAdmission);
        Assert.False(receipt.SelfGelMutated);
        Assert.False(receipt.CradleGelAdmitted);
        Assert.False(receipt.SanctuaryGelFederated);
        Assert.False(receipt.ActionAuthorized);
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
            var candidate = Path.Combine(directory.FullName, "src", "SLI", "SLI.Lisp", "shared-prime-reality-pressure-ecology.lisp");
            if (File.Exists(candidate))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate repository root.");
    }

    private sealed record PressureSources(
        WaveCondensationSharedRealityReceipt SharedReality,
        GelDomainScopedIngressReceipt DomainIngress);

    private sealed record IngressSources(
        EngramPredicatePrecursorStreamReceipt Epps,
        PeerReviewPredicateBridgeReceipt Bridge);

    private sealed class PressureFixture : IDisposable
    {
        private PressureFixture(string rootPath)
        {
            RootPath = rootPath;
            LineRootPath = Path.Combine(rootPath, "line");
            InstallRootPath = Path.Combine(rootPath, "install");
            Directory.CreateDirectory(LineRootPath);
            Directory.CreateDirectory(Path.Combine(InstallRootPath, "product"));
            Directory.CreateDirectory(Path.Combine(InstallRootPath, "receipts", "spiral-build", "cells"));
            File.WriteAllText(Path.Combine(InstallRootPath, "sanctuary.cmd"), "@echo off");
            File.WriteAllText(Path.Combine(InstallRootPath, "product", "San.Launcher.exe"), "fixture");

            var cellRoot = Path.Combine(InstallRootPath, "receipts", "spiral-build", "cells");
            foreach (var artifact in DefaultFirstRiderGovernanceSimulationService.RequiredStages.SelectMany(static stage => stage.RequiredArtifacts))
            {
                File.WriteAllText(Path.Combine(cellRoot, artifact), "{}");
            }
        }

        public string RootPath { get; }
        public string LineRootPath { get; }
        public string InstallRootPath { get; }

        public static PressureFixture Create() =>
            new(Path.Combine(Path.GetTempPath(), $"san-shared-prime-pressure-tests-{Guid.NewGuid():N}"));

        public PressureSources CreateSources()
        {
            var sharedReality = CreateSharedRealityReceipt();
            var ingressSources = CreateIngressSources();
            var ingress = new DefaultGelDomainScopedIngressBoundaryValidator().Declare(CreateIngressRequest(ingressSources), TimestampUtc);

            Assert.True(sharedReality.IsColdWaveCondensation);
            Assert.True(ingress.IsColdIngressRecommendation);
            return new PressureSources(sharedReality, ingress);
        }

        public void Dispose()
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, recursive: true);
            }
        }

        private static WaveCondensationSharedRealityReceipt CreateSharedRealityReceipt()
        {
            var signals = new[]
            {
                WaveSignal("prime-body", WaveSignalKind.PrimeBody, "prime-body-surface", 0),
                WaveSignal("cryptic-mind", WaveSignalKind.CrypticMind, "cryptic-mind-surface", 1),
                WaveSignal("steward-witness", WaveSignalKind.StewardWitness, "steward-witness-surface", 2),
                WaveSignal("operator-resonance", WaveSignalKind.OperatorResonance, "operator-resonance-surface", 3),
                WaveSignal("tool-telemetry", WaveSignalKind.ToolTelemetry, "tool-telemetry-surface", 4)
            };
            var anchors = signals.Select(signal => new SharedRealityAnchor(
                AnchorHandle: $"urn:san:shared-reality-anchor:{signal.SignalKind.ToString().ToLowerInvariant()}",
                SourceSignalHandle: signal.SignalHandle,
                SharedSurface: "urn:san:shared-reality:prime-body-cryptic-mind-steward-witness",
                PrimeBodyRef: "urn:san:prime:body",
                CrypticMindRef: "urn:san:cryptic:mind",
                StewardWitnessRef: "urn:san:steward:witness",
                LineageHandle: "urn:san:shared-prime:lineage",
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
                AdmitsContinuity: false)).ToArray();

            return new DefaultWaveCondensationSharedRealityBoundaryValidator().Condense(
                new WaveCondensationSharedRealityRequest(
                    Signals: signals,
                    Anchors: anchors,
                    Boundary: CreateWaveBoundary(),
                    NonCollapseBoundary: CreateWaveNonCollapse(),
                    PriorPassageCount: 0),
                TimestampUtc);
        }

        private static WaveSignal WaveSignal(
            string suffix,
            WaveSignalKind kind,
            string surface,
            int index) =>
            new(
                SignalHandle: $"urn:san:wave-signal:{suffix}",
                SignalKind: kind,
                SourceSurface: surface,
                EvidenceHandle: $"urn:san:evidence:wave:{suffix}",
                WitnessHandle: $"urn:san:witness:wave:{suffix}",
                CondensationTarget: "urn:san:shared-reality:prime-body-cryptic-mind-steward-witness",
                WaveIndex: index,
                Amplitude: 0.62,
                Confidence: 0.72,
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

        private static WaveCondensationBoundary CreateWaveBoundary() =>
            new(
                BoundaryCode: "wave-condensation-shared-reality-boundary",
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
                AllowsActivation: false);

        private static WaveCondensationNonCollapseBoundary CreateWaveNonCollapse() =>
            new(
                BoundaryLaw: "Shared Prime Reality holds waves as review surface only.",
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

        private IngressSources CreateIngressSources()
        {
            var rider = new DefaultFirstRiderGovernanceSimulationService().Simulate(
                new FirstRiderGovernanceSimulationRequest(
                    LineRootPath: LineRootPath,
                    InstallRootPath: InstallRootPath,
                    ThoughtForm: "live lab pressure requires Shared Prime ecology before any integration path"),
                TimestampUtc);
            var epps = new DefaultFirstRiderEngramPredicatePrecursorStreamService().Emit(rider, TimestampUtc);
            var bridge = new DefaultPeerReviewPredicateBridgeBoundaryValidator().Declare(
                new PeerReviewPredicateBridgeRequest(
                    SourceEppsReceipt: epps,
                    Segments: epps.Residues.Select((residue, index) => CreateSegment(residue.ResidueHandle, index)).ToArray(),
                    Boundary: CreatePeerReviewBoundary(),
                    PriorPassageCount: 0),
                TimestampUtc);

            Assert.True(epps.IsColdPrecursorStream);
            Assert.True(bridge.IsColdPeerReviewBridge);
            return new IngressSources(epps, bridge);
        }

        private static GelDomainScopedIngressRequest CreateIngressRequest(IngressSources sources)
        {
            var candidate = new GelIngressCandidateSubstrate(
                CandidateHandle: "urn:san:gel-domain-ingress:candidate:shared-prime-pressure",
                SourceEppsReceiptHandle: sources.Epps.ReceiptHandle,
                SourceBridgeReceiptHandle: sources.Bridge.ReceiptHandle,
                CandidateSummary: "Shared Prime pressure ecology source remains candidate-only after EPPS and bridge synthesis.",
                SourceResidueHandles: sources.Epps.Residues.Select(static residue => residue.ResidueHandle).ToArray(),
                SourceBridgeSegmentHandles: sources.Bridge.Segments.Select(static segment => segment.SegmentHandle).ToArray(),
                PostGelFormation: true,
                PreGelAdmission: true,
                CandidateOnly: true,
                ReviewOnly: true,
                FormedSubstrate: true,
                AdmittedGel: false,
                AdmittedMemory: false,
                MutatedContinuity: false,
                MutatedSelfGel: false,
                GrantedAuthority: false,
                AuthorizedAction: false,
                EvaluatesLisp: false,
                EmitsPacket: false,
                IncrementsPassage: false,
                Activates: false);
            var scope = CreateScope(GelIngressDomain.ScholarlyReview, GelIngressEvidenceCeiling.Interpretive);
            var review = CreateStewardReview();

            return new GelDomainScopedIngressRequest(
                SourceEppsReceipt: sources.Epps,
                SourceBridgeReceipt: sources.Bridge,
                Candidate: candidate,
                DomainScope: scope,
                CycleTrace: DefaultGelDomainScopedIngressBoundaryValidator.CreateCycleTrace(sources.Epps, sources.Bridge, candidate, scope, review),
                StewardReview: review,
                Boundary: CreateIngressBoundary(),
                PriorRecommendationCount: 0,
                PriorPassageCount: 0,
                RepeatedRecommendationCreatesWarrant: false,
                GelAdmissionRequested: false,
                MemoryAdmissionRequested: false,
                ContinuityMutationRequested: false,
                SelfGelMutationRequested: false,
                AuthorityRequested: false,
                ActionRequested: false,
                LispEvaluationRequested: false,
                PacketEmissionRequested: false,
                ReceiptReplayRequested: false,
                PassageIncrementRequested: false,
                ActivationRequested: false);
        }

        private static GelDomainScopeRecord CreateScope(
            GelIngressDomain domain,
            GelIngressEvidenceCeiling ceiling) =>
            new(
                ScopeHandle: $"urn:san:gel-domain-ingress:scope:{domain.ToString().ToLowerInvariant()}",
                Domain: domain,
                EvidenceCeiling: ceiling,
                DomainRationale: "Shared Prime pressure ecology is assigned a local scholarly-review domain before later integration review.",
                EvidenceCeilingRationale: "evidence standards are domain-local and may not be inherited from another world",
                LossCondition: "refuse if pressure, destination, evidence ceiling, or recommendation attempts GEL admission, SelfGEL mutation, Cradle.GEL admission, Sanctuary.GEL federation, authority, or action",
                Present: true,
                ReviewOnly: true,
                DomainFitReviewed: true,
                EvidenceCeilingAssigned: true,
                CoolingRequired: true,
                StewardReviewRequired: true,
                EvidenceCeilingPortable: false,
                DomainFitAdmitsGel: false,
                DomainFitAdmitsMemory: false,
                DomainFitMutatesContinuity: false,
                DomainFitGrantsAuthority: false,
                DomainFitAuthorizesAction: false,
                RequiresSpecialCaseHold: false,
                SpecialCaseHeld: false,
                DomainClosed: false);

        private static GelIngressStewardReview CreateStewardReview() =>
            new(
                ReviewHandle: "urn:san:gel-domain-ingress:steward-review:shared-prime-pressure",
                StewardTrace: "Steward recommends external ingress consideration without admission.",
                ReviewOnly: true,
                StewardCustodyPresent: true,
                CoolingComplete: true,
                RecommendationMayIssue: true,
                RecommendsIngressConsideration: true,
                PerformsAdmission: false,
                AdmitsGel: false,
                AdmitsMemory: false,
                MutatesContinuity: false,
                GrantsAuthority: false,
                AuthorizesAction: false,
                EvaluatesLisp: false,
                EmitsPacket: false,
                ReplaysReceipt: false,
                IncrementsPassage: false,
                Activates: false);

        private static GelDomainScopedIngressBoundary CreateIngressBoundary() =>
            new(
                BoundaryCode: "gel-domain-scoped-ingress-boundary",
                Present: true,
                ReviewOnly: true,
                RequiresColdEpps: true,
                RequiresColdPeerReviewBridge: true,
                RequiresCandidateSubstrate: true,
                RequiresDomainScope: true,
                RequiresEvidenceCeiling: true,
                RequiresCooling: true,
                RequiresStewardReview: true,
                AllowsGovernanceSurvivorshipAsProof: false,
                AllowsDomainFitAsAdmission: false,
                AllowsEvidenceCeilingPortability: false,
                AllowsRecommendationAsAdmission: false,
                AllowsGelAdmission: false,
                AllowsMemoryAdmission: false,
                AllowsContinuityMutation: false,
                AllowsSelfGelMutation: false,
                AllowsAuthority: false,
                AllowsAction: false,
                AllowsLispEvaluation: false,
                AllowsPacketEmission: false,
                AllowsReceiptReplay: false,
                AllowsPassageIncrement: false,
                AllowsActivation: false);

        private static PeerReviewBridgeSegment CreateSegment(string residueHandle, int index) =>
            new(
                SegmentHandle: $"urn:san:shared-prime-pressure:bridge-segment:{index}",
                SourceResidueHandle: residueHandle,
                AuthorTerm: "pressure ecology",
                LocalDefinition: "visible live lab pressure that remains outside GEL admission",
                WhyItMatters: "pressure must be classified before any integration path may be considered",
                OperationalImplication: "pressure destination remains review-only and non-admitting",
                Evaluation: "sufficient for Shared Prime pressure ecology calibration only",
                BoundedConclusion: "retain as pressure signal without truth, authority, admission, or action",
                EvidenceStatus: PeerReviewEvidenceStatus.Demonstrated,
                AudienceStateRef: "urn:san:reader-state:shared-prime-pressure",
                ContextQuarantineRef: "urn:san:context-quarantine:shared-prime-pressure",
                ReviewOnly: true,
                ReaderStateContinuityMapped: true,
                TerminologyQuarantined: true,
                ContextQuarantined: true,
                ReviewStateIsolated: true,
                ConversationalDepthRetained: true,
                BridgeSynthesisOnly: true,
                PriorDoctrineUsedAsPostureOnly: true,
                AuthorTermBecomesAuthority: false,
                LocalDefinitionBecomesProof: false,
                WhyItMattersBecomesEvidence: false,
                OperationalImplicationAuthorizesAction: false,
                EvaluationGrantsWarrant: false,
                BoundedConclusionAdmitsTruth: false,
                RespectBecomesAgreement: false,
                CriticismBecomesContempt: false,
                ProseSmoothingHidesConcern: false,
                PriorDoctrineBecomesInterpretiveAuthority: false,
                ConceptualProximityBecomesEquivalence: false,
                ReviewArchitectureColonizesPaper: false,
                ConversationalDepthBecomesAdvocacy: false,
                BridgeBecomesMemory: false,
                BridgeAdmitsContinuity: false,
                BridgeGrantsAuthority: false,
                BridgeAuthorizesAction: false,
                BridgeEvaluatesLisp: false,
                BridgeEmitsPacket: false,
                BridgeReplaysReceipt: false,
                BridgeIncrementsPassage: false,
                BridgeActivates: false);

        private static PeerReviewBridgeBoundary CreatePeerReviewBoundary() =>
            new(
                BoundaryCode: "shared-prime-pressure-peer-review-bridge-boundary",
                Present: true,
                ReviewOnly: true,
                RequiresEppsSource: true,
                RequiresLocalDefinition: true,
                RequiresWhyItMatters: true,
                RequiresOperationalImplication: true,
                RequiresEvaluation: true,
                RequiresBoundedConclusion: true,
                RequiresTerminologyQuarantine: true,
                RequiresReaderStateContinuity: true,
                RequiresContextQuarantine: true,
                RequiresReviewStateIsolation: true,
                RequiresConversationalDepth: true,
                RequiresEvidenceStatus: true,
                AllowsAuthorTermAsAuthority: false,
                AllowsDefinitionAsProof: false,
                AllowsConsequenceAsEvidence: false,
                AllowsEvaluationAsWarrant: false,
                AllowsConclusionAsTruth: false,
                AllowsRespectAsAgreement: false,
                AllowsCriticismAsContempt: false,
                AllowsProseSmoothingToHideConcern: false,
                AllowsPriorDoctrineAsInterpretiveAuthority: false,
                AllowsConceptualProximityAsEquivalence: false,
                AllowsReviewArchitectureColonization: false,
                AllowsConversationalDepthAsAdvocacy: false,
                AllowsMemoryAdmission: false,
                AllowsContinuityAdmission: false,
                AllowsAuthority: false,
                AllowsActionAuthorization: false,
                AllowsLispEvaluation: false,
                AllowsPacketEmission: false,
                AllowsReceiptReplay: false,
                AllowsPassageIncrement: false,
                AllowsActivation: false);
    }
}
