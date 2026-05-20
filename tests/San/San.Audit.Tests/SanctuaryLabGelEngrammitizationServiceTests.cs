using San.Sanctuary.Runtime;
using SLI.Lisp;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryLabGelEngrammitizationServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-20T00:00:00Z");

    [Fact]
    public void Run_Forms_Pre_Admission_Lab_Gel_Predicate_And_Engram_Candidate()
    {
        using var fixture = SanctuaryInstalledFixture.Create();
        var installed = new DefaultSanctuaryInstalledSubstrateService(new FixedSliLispRuntimeLoadService(CreateLoadedRuntimeReceipt()))
            .Install(fixture.CreateInstallRequest(), TimestampUtc);
        var warmUse = new DefaultSanctuaryTypedWarmUseRehearsalService(new EchoSliLispTypedWarmUseRehearsalService())
            .Run(
                new SanctuaryTypedWarmUseRehearsalRequest(
                    InstalledSubstrateReceipt: installed,
                    SessionId: "lab-gel-session",
                    TurnIndex: 4,
                    ThoughtForm: "Meaningful residue should become candidate substrate, not memory."),
                TimestampUtc);
        var service = new DefaultSanctuaryLabGelEngrammitizationService(new EchoSliLispLabGelEngrammitizationService());

        var receipt = service.Run(
            new SanctuaryLabGelEngrammitizationRequest(
                SourceWarmUseReceipt: warmUse,
                PriorLabGelReceiptHandle: "urn:san:lab-gel-engrammitization:prior"),
            TimestampUtc.AddSeconds(1));

        Assert.True(warmUse.IsTypedColdReadyWarmUse);
        Assert.Equal(SanctuaryLabGelEngrammitizationDisposition.CompletedCold, receipt.Disposition);
        Assert.Equal("sanctuary-lab-gel-engrammitization-completed-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsColdPreAdmissionLabGel);
        Assert.Equal(warmUse.ReceiptHandle, receipt.SourceWarmUseReceiptHandle);
        Assert.Equal(warmUse.SliLispWarmUseReceipt?.ReceiptHandle, receipt.SourceSliLispWarmUseReceiptHandle);
        Assert.Equal("urn:san:lab-gel-engrammitization:prior", receipt.PriorLabGelReceiptHandle);
        Assert.Equal("YourNameHere.ID", receipt.OperatorId);
        Assert.Equal("Civic", receipt.Domain);
        Assert.Equal("PaternalCareAssistance", receipt.Role);
        Assert.Equal("Listening", receipt.JobClass);
        Assert.Equal("lab-gel-session", receipt.SessionId);
        Assert.Equal(4, receipt.TurnIndex);
        Assert.Equal(6, receipt.Predicates.Count);
        Assert.All(receipt.Predicates, predicate =>
        {
            Assert.True(predicate.IsColdLabGelPredicate);
            Assert.Equal(warmUse.ReceiptHandle, predicate.SourceWarmUseReceiptHandle);
            Assert.False(predicate.GelAdmitted);
            Assert.False(predicate.SelfGelMutated);
            Assert.False(predicate.ActionAuthorized);
        });
        Assert.True(receipt.EvidenceBody?.IsColdEvidenceBody);
        Assert.True(receipt.WitnessBody?.IsColdWitnessBody);
        Assert.True(receipt.EngramCandidate?.IsColdEngramCandidate);
        Assert.True(receipt.CoolingReceipt?.IsColdCoolingReceipt);
        Assert.True(receipt.PreAdmissionReview?.IsColdPreAdmissionReview);
        Assert.True(receipt.ReadbackReceipt?.IsColdReadback);
        Assert.True(receipt.EngramClosure?.IsColdEngramClosure);
        Assert.True(receipt.EngramClosureFormed);
        Assert.True(receipt.EngramClosureReadyForEcPayload);
        Assert.True(receipt.CandidateRetainedAsLabSubstrate);
        Assert.False(receipt.LabGelAdmitted);
        Assert.False(receipt.EngramAdmitted);
        Assert.False(receipt.MemoryAdmitted);
        Assert.False(receipt.SelfGelMutated);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.CmeActualAllowed);
        Assert.False(receipt.SanctuaryActualAllowed);
        Assert.True(File.Exists(receipt.ReceiptJsonPath));
        Assert.True(File.Exists(receipt.ReceiptMarkdownPath));
        Assert.True(File.Exists(receipt.SessionLedgerPath));
        Assert.Contains("Lab GEL predicate formed: `True`", File.ReadAllText(receipt.ReceiptMarkdownPath), StringComparison.Ordinal);
        Assert.Contains(receipt.ReceiptHandle, File.ReadAllText(receipt.SessionLedgerPath), StringComparison.Ordinal);
    }

    [Fact]
    public void Run_Withholds_Without_Typed_Cold_Warm_Use_Source()
    {
        var service = new DefaultSanctuaryLabGelEngrammitizationService(new EchoSliLispLabGelEngrammitizationService());

        var receipt = service.Run(
            new SanctuaryLabGelEngrammitizationRequest(SourceWarmUseReceipt: null),
            TimestampUtc);

        Assert.Equal(SanctuaryLabGelEngrammitizationDisposition.Withheld, receipt.Disposition);
        Assert.Equal("sanctuary-lab-gel-source-warm-use-missing", receipt.OutcomeCode);
        Assert.False(receipt.IsColdPreAdmissionLabGel);
        Assert.Null(receipt.SliLispLabGelReceipt);
        Assert.Empty(receipt.Predicates);
        Assert.False(receipt.LabGelAdmitted);
        Assert.False(receipt.SelfGelMutated);
    }

    [Fact]
    public void Run_Refuses_Admission_Or_Action_Motion_Before_Sli_Lisp_Invocation()
    {
        using var fixture = SanctuaryInstalledFixture.Create();
        var installed = new DefaultSanctuaryInstalledSubstrateService(new FixedSliLispRuntimeLoadService(CreateLoadedRuntimeReceipt()))
            .Install(fixture.CreateInstallRequest(), TimestampUtc);
        var warmUse = new DefaultSanctuaryTypedWarmUseRehearsalService(new EchoSliLispTypedWarmUseRehearsalService())
            .Run(
                new SanctuaryTypedWarmUseRehearsalRequest(
                    InstalledSubstrateReceipt: installed,
                    SessionId: "lab-gel-session",
                    TurnIndex: 0,
                    ThoughtForm: "force admission"),
                TimestampUtc);
        var service = new DefaultSanctuaryLabGelEngrammitizationService(new EchoSliLispLabGelEngrammitizationService());

        var receipt = service.Run(
            new SanctuaryLabGelEngrammitizationRequest(
                SourceWarmUseReceipt: warmUse,
                GelAdmissionRequested: true,
                EngramAdmissionRequested: true,
                SelfGelMutationRequested: true,
                RuntimeActionRequested: true),
            TimestampUtc);

        Assert.Equal(SanctuaryLabGelEngrammitizationDisposition.Refused, receipt.Disposition);
        Assert.Equal("sanctuary-lab-gel-runtime-motion-refused", receipt.OutcomeCode);
        Assert.False(receipt.IsColdPreAdmissionLabGel);
        Assert.Null(receipt.SliLispLabGelReceipt);
        Assert.Empty(receipt.Predicates);
        Assert.False(receipt.LabGelAdmitted);
        Assert.False(receipt.SelfGelMutated);
        Assert.False(receipt.ActionAuthorized);
        Assert.True(File.Exists(receipt.ReceiptJsonPath));
        Assert.Contains("runtime-motion-refused", File.ReadAllText(receipt.ReceiptJsonPath), StringComparison.Ordinal);
    }

    private static SliLispRuntimeLoadReceipt CreateLoadedRuntimeReceipt() =>
        new(
            ReceiptHandle: "sli-lisp-runtime-load://loaded",
            Disposition: SliLispRuntimeLoadDisposition.LoadedCold,
            OutcomeCode: "sli-lisp-resident-membrane-loaded-cold",
            RuntimeKind: "SBCL",
            RuntimePath: "sbcl",
            ModuleNames: ["core.lisp", "typed-warm-use-rehearsal.lisp", "lab-gel-engrammitization.lisp"],
            ModuleCount: 3,
            LoadedFromEmbeddedResources: true,
            LoadAttempted: true,
            LoadSucceeded: true,
            ResidentModuleLoadAllowed: true,
            TopLevelLoadEvaluationExpected: true,
            ArbitraryEvaluationAllowed: false,
            RuntimeActionAllowed: false,
            ActivationAllowed: false,
            AuthorityGranted: false,
            ModelBindingAllowed: false,
            GelPromotionAllowed: false,
            CmeActualAllowed: false,
            SanctuaryActualAllowed: false,
            ExitCode: 0,
            StandardOutput: "SAN-SLI-LISP-RUNTIME-LOAD-OK",
            StandardError: string.Empty,
            TimestampUtc: TimestampUtc);

    private sealed class EchoSliLispTypedWarmUseRehearsalService : ISliLispTypedWarmUseRehearsalService
    {
        public SliLispTypedWarmUseRehearsalReceipt Run(
            SliLispTypedWarmUseRehearsalRequest request,
            DateTimeOffset timestampUtc) =>
            new(
                ReceiptHandle: $"sli-lisp-typed-warm-use://{request.SessionId}-{request.TurnIndex}",
                Disposition: SliLispTypedWarmUseRehearsalDisposition.CompletedCold,
                OutcomeCode: "sli-lisp-typed-warm-use-rehearsal-completed-cold",
                RuntimeKind: "SBCL",
                RuntimePath: "sbcl",
                OperatorId: request.OperatorId,
                Domain: request.Domain,
                Role: request.Role,
                JobClass: request.JobClass,
                SessionId: request.SessionId,
                TurnIndex: request.TurnIndex,
                ThoughtForm: request.ThoughtForm,
                Telemetry: new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["engine-owner"] = "sli.lisp",
                    ["bounded-entrypoint"] = "run-typed-warm-use-rehearsal"
                },
                ModuleNames: ["core.lisp", "ec-telemetry-loop.lisp", "typed-warm-use-rehearsal.lisp"],
                ModuleCount: 3,
                BoundedEntrypointCalled: true,
                LoadAttempted: true,
                LoadSucceeded: true,
                TypedWarmUseRehearsalCompleted: true,
                TypedScopeAccepted: true,
                LiveIngressAcceptedCold: true,
                SessionLineageWitnessed: true,
                ListeningFrameReceived: true,
                SliMembraneInterpretedPredicatePressure: true,
                CompassOrientedPressure: true,
                CompassCoolingRequired: true,
                SoulFrameReceivedListeningFrame: true,
                AgentiCoreReceivedCompassPressure: true,
                ThinkingAboutThinkingTelemetryProduced: true,
                PreEngramResidueProduced: true,
                PreEngramResidueCount: 6,
                PreEngramResidueClasses: ["semantic", "pressure", "witness", "governance", "morphology", "return"],
                StewardReviewed: true,
                TurnLineageReceiptOnly: true,
                SessionLedgerAppendOnly: true,
                EngramAdmissionAllowed: false,
                MemoryAdmissionAllowed: false,
                SelfGelMutationAllowed: false,
                ContinuityAdmissionAllowed: false,
                AuthorityGranted: false,
                ActionAuthorized: false,
                ModelBindingAllowed: false,
                ArbitraryEvaluationAllowed: false,
                RuntimeActionAllowed: false,
                ActivationAllowed: false,
                CmeActualActivationAllowed: false,
                SanctuaryActualActivationAllowed: false,
                ExitCode: 0,
                StandardOutput: "SAN-SLI-TYPED-WARM-USE-OK",
                StandardError: string.Empty,
                TimestampUtc: timestampUtc);
    }

    private sealed class EchoSliLispLabGelEngrammitizationService : ISliLispLabGelEngrammitizationService
    {
        public SliLispLabGelEngrammitizationReceipt Run(
            SliLispLabGelEngrammitizationRequest request,
            DateTimeOffset timestampUtc) =>
            new(
                ReceiptHandle: $"sli-lisp-lab-gel://{request.SessionId}-{request.TurnIndex}",
                Disposition: SliLispLabGelEngrammitizationDisposition.CompletedCold,
                OutcomeCode: "sli-lisp-lab-gel-engrammitization-completed-cold",
                RuntimeKind: "SBCL",
                RuntimePath: "sbcl",
                OperatorId: request.OperatorId,
                Domain: request.Domain,
                Role: request.Role,
                JobClass: request.JobClass,
                SessionId: request.SessionId,
                TurnIndex: request.TurnIndex,
                SourceWarmUseReceiptHandle: request.SourceWarmUseReceiptHandle,
                ThoughtForm: request.ThoughtForm,
                Telemetry: new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["engine-owner"] = "sli.lisp",
                    ["bounded-entrypoint"] = "run-lab-gel-engrammitization",
                    ["lab-gel.state"] = "post-gel-formation-pre-admission"
                },
                ModuleNames: ["core.lisp", "typed-warm-use-rehearsal.lisp", "lab-gel-engrammitization.lisp"],
                ModuleCount: 3,
                BoundedEntrypointCalled: true,
                LoadAttempted: true,
                LoadSucceeded: true,
                LabGelEngrammitizationCompleted: true,
                LabGelPredicateFormed: true,
                LabGelPredicateCount: 6,
                LabGelPredicateClasses: ["semantic", "pressure", "witness", "governance", "morphology", "return"],
                EngramCandidateFormed: true,
                EngramCandidatePreAdmissionOnly: true,
                EvidenceBodyFormed: true,
                WitnessBodyFormed: true,
                CoolingHeld: true,
                PreAdmissionReviewRequired: true,
                LabGelReadbackAvailable: true,
                LabGelReadbackPreAdmissionOnly: true,
                EngramClosureFormed: true,
                EngramClosurePreAdmissionOnly: true,
                EngramClosureLabSubstrateOnly: true,
                EngramClosureWitnessed: true,
                EngramClosureSealed: true,
                EngramClosureReadyForEcPayload: true,
                EngramClosureAdmitsGel: false,
                EngramClosureAdmitsEngram: false,
                EngramClosureAdmitsMemory: false,
                EngramClosureMutatesSelfGel: false,
                EngramClosureAdmitsContinuity: false,
                EngramClosureGrantsAuthority: false,
                EngramClosureAuthorizesAction: false,
                TypedScopeAccepted: true,
                SourceWarmUseAcceptedCold: true,
                SessionLineageWitnessed: true,
                ListeningFrameReceived: true,
                SliMembraneInterpretedPredicatePressure: true,
                CompassOrientedPressure: true,
                CompassCoolingRequired: true,
                SoulFrameReceivedListeningFrame: true,
                AgentiCoreReceivedCompassPressure: true,
                ThinkingAboutThinkingTelemetryProduced: true,
                StewardReviewed: true,
                GelPromotionAllowed: false,
                GelAdmissionAllowed: false,
                EngramAdmissionAllowed: false,
                MemoryAdmissionAllowed: false,
                SelfGelMutationAllowed: false,
                ContinuityAdmissionAllowed: false,
                AuthorityGranted: false,
                ActionAuthorized: false,
                ModelBindingAllowed: false,
                ArbitraryEvaluationAllowed: false,
                RuntimeActionAllowed: false,
                ActivationAllowed: false,
                CmeActualActivationAllowed: false,
                SanctuaryActualActivationAllowed: false,
                ExitCode: 0,
                StandardOutput: "SAN-SLI-LAB-GEL-OK",
                StandardError: string.Empty,
                TimestampUtc: timestampUtc);
    }

    private sealed class FixedSliLispRuntimeLoadService : ISliLispRuntimeLoadService
    {
        private readonly SliLispRuntimeLoadReceipt receipt;

        public FixedSliLispRuntimeLoadService(SliLispRuntimeLoadReceipt receipt)
        {
            this.receipt = receipt;
        }

        public SliLispRuntimeLoadReceipt LoadResidentMembrane(
            SliLispRuntimeLoadRequest request,
            DateTimeOffset timestampUtc) =>
            receipt;
    }

    private sealed class SanctuaryInstalledFixture : IDisposable
    {
        private SanctuaryInstalledFixture(string rootPath)
        {
            RootPath = rootPath;
            LineRootPath = Path.Combine(rootPath, "line-root");
            InstallRootPath = Path.Combine(rootPath, "Sanctuary");
        }

        public string RootPath { get; }

        public string LineRootPath { get; }

        public string InstallRootPath { get; }

        public static SanctuaryInstalledFixture Create()
        {
            var fixture = new SanctuaryInstalledFixture(Path.Combine(Path.GetTempPath(), $"sanctuary-lab-gel-tests-{Guid.NewGuid():N}"));
            Directory.CreateDirectory(fixture.LineRootPath);
            File.WriteAllText(Path.Combine(fixture.LineRootPath, "San.sln"), string.Empty);
            return fixture;
        }

        public SanctuaryInstalledSubstrateRequest CreateInstallRequest() =>
            new(
                LineRootPath: LineRootPath,
                InstallRootPath: InstallRootPath,
                OperatorName: "YourNameHere",
                Domain: "Civic",
                Role: "PaternalCareAssistance",
                JobClass: "Listening");

        public void Dispose()
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, recursive: true);
            }
        }
    }
}
