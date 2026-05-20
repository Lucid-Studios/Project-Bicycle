using San.Sanctuary.Runtime;
using SLI.Lisp;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryAgentEngineIdleReadinessServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-20T00:00:00Z");

    [Fact]
    public void Run_Stages_Provider_Neutral_Engine_Llm_Seat_Without_Authority_Or_Action()
    {
        using var fixture = AgentEngineIdleFixture.Create();
        var source = fixture.CreateColdLabGelReceipt();
        var service = new DefaultSanctuaryAgentEngineIdleReadinessService(new EchoSliLispAgentEngineIdleReadinessService());

        var receipt = service.Run(
            new SanctuaryAgentEngineIdleReadinessRequest(
                SourceLabGelReceipt: source,
                PriorAgentEngineIdleReceiptHandle: "urn:san:agent-engine-idle-readiness:prior"),
            TimestampUtc.AddSeconds(1));

        Assert.True(source.IsColdPreAdmissionLabGel);
        Assert.Equal(SanctuaryAgentEngineIdleReadinessDisposition.CompletedCold, receipt.Disposition);
        Assert.Equal("sanctuary-agent-engine-idle-readiness-completed-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsColdAgentEngineIdleReadiness);
        Assert.Equal(source.ReceiptHandle, receipt.SourceLabGelReceiptHandle);
        Assert.Equal(source.EngramCandidate?.CandidateHandle, receipt.SourceEngramCandidateHandle);
        Assert.Equal("urn:san:agent-engine-idle-readiness:prior", receipt.PriorAgentEngineIdleReceiptHandle);
        Assert.True(receipt.EngineSeatCandidate?.IsColdEngineLlmSeatCandidate);
        Assert.True(receipt.DriverAuthorityGate?.IsColdDriverAuthorityGate);
        Assert.True(receipt.ActualizationLock?.IsColdActualizationLock);
        Assert.True(receipt.ProviderNeutralityHeld);
        Assert.True(receipt.CrossModelHarnessApproachable);
        Assert.True(receipt.EngineLlmSeatCandidateStaged);
        Assert.True(receipt.CodexAgentLabProfileStaged);
        Assert.True(receipt.CodexEngineSeatCandidateStaged);
        Assert.True(receipt.SubagentEngineSeatCandidateStaged);
        Assert.True(receipt.EngineLlmArticulationAllowed);
        Assert.True(receipt.EngineLlmRehearsalAllowed);
        Assert.True(receipt.EngineLlmCandidateFormationAllowed);
        Assert.False(receipt.EngineLlmAuthorityGrantingAllowed);
        Assert.False(receipt.EngineLlmActionExecutionAllowed);
        Assert.True(receipt.OperatorAuthorityRequired);
        Assert.True(receipt.AuthorityGrantAbsent);
        Assert.True(receipt.ActionExecutorLocked);
        Assert.True(receipt.GelAdmissionLocked);
        Assert.True(receipt.SelfGelMutationLocked);
        Assert.True(receipt.HeartbeatLocked);
        Assert.True(receipt.CmeActualLocked);
        Assert.True(receipt.SanctuaryActualLocked);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.ActionExecutorArmed);
        Assert.False(receipt.LabGelAdmitted);
        Assert.False(receipt.SelfGelMutated);
        Assert.False(receipt.HeartbeatActive);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.CmeActualAllowed);
        Assert.False(receipt.SanctuaryActualAllowed);
        Assert.True(File.Exists(receipt.ReceiptJsonPath));
        Assert.True(File.Exists(receipt.ReceiptMarkdownPath));
        Assert.True(File.Exists(receipt.SessionLedgerPath));
        Assert.Contains("Provider neutrality held: `True`", File.ReadAllText(receipt.ReceiptMarkdownPath), StringComparison.Ordinal);
        Assert.Contains(receipt.ReceiptHandle, File.ReadAllText(receipt.SessionLedgerPath), StringComparison.Ordinal);
    }

    [Fact]
    public void Run_Withholds_Without_Pre_Admission_Lab_Gel_Source()
    {
        var service = new DefaultSanctuaryAgentEngineIdleReadinessService(new EchoSliLispAgentEngineIdleReadinessService());

        var receipt = service.Run(
            new SanctuaryAgentEngineIdleReadinessRequest(SourceLabGelReceipt: null),
            TimestampUtc);

        Assert.Equal(SanctuaryAgentEngineIdleReadinessDisposition.Withheld, receipt.Disposition);
        Assert.Equal("sanctuary-agent-engine-idle-source-lab-gel-missing", receipt.OutcomeCode);
        Assert.False(receipt.IsColdAgentEngineIdleReadiness);
        Assert.Null(receipt.SliLispAgentEngineIdleReceipt);
        Assert.Null(receipt.EngineSeatCandidate);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ActionAuthorized);
    }

    [Fact]
    public void Run_Refuses_Authority_Action_Or_Actualization_Before_Sli_Lisp_Invocation()
    {
        using var fixture = AgentEngineIdleFixture.Create();
        var source = fixture.CreateColdLabGelReceipt();
        var service = new DefaultSanctuaryAgentEngineIdleReadinessService(new EchoSliLispAgentEngineIdleReadinessService());

        var receipt = service.Run(
            new SanctuaryAgentEngineIdleReadinessRequest(
                SourceLabGelReceipt: source,
                AuthorityGrantRequested: true,
                ActionExecutorArmRequested: true,
                GelAdmissionRequested: true,
                HeartbeatActivationRequested: true,
                CmeActualRequested: true),
            TimestampUtc);

        Assert.Equal(SanctuaryAgentEngineIdleReadinessDisposition.Refused, receipt.Disposition);
        Assert.Equal("sanctuary-agent-engine-idle-runtime-motion-refused", receipt.OutcomeCode);
        Assert.False(receipt.IsColdAgentEngineIdleReadiness);
        Assert.Null(receipt.SliLispAgentEngineIdleReceipt);
        Assert.Null(receipt.EngineSeatCandidate);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ActionAuthorized);
        Assert.False(receipt.CmeActualAllowed);
        Assert.True(File.Exists(receipt.ReceiptJsonPath));
        Assert.Contains("runtime-motion-refused", File.ReadAllText(receipt.ReceiptJsonPath), StringComparison.Ordinal);
    }

    private sealed class EchoSliLispAgentEngineIdleReadinessService : ISliLispAgentEngineIdleReadinessService
    {
        public SliLispAgentEngineIdleReadinessReceipt Run(
            SliLispAgentEngineIdleReadinessRequest request,
            DateTimeOffset timestampUtc) =>
            new(
                ReceiptHandle: $"sli-lisp-agent-engine-idle://{request.SessionId}-{request.TurnIndex}",
                Disposition: SliLispAgentEngineIdleReadinessDisposition.CompletedCold,
                OutcomeCode: "sli-lisp-agent-engine-idle-readiness-completed-cold",
                RuntimeKind: "SBCL",
                RuntimePath: "sbcl",
                OperatorId: request.OperatorId,
                Domain: request.Domain,
                Role: request.Role,
                JobClass: request.JobClass,
                SessionId: request.SessionId,
                TurnIndex: request.TurnIndex,
                SourceLabGelReceiptHandle: request.SourceLabGelReceiptHandle,
                SourceEngramCandidateHandle: request.SourceEngramCandidateHandle,
                SourceEngramClosureReceiptHandle: request.SourceEngramClosureReceiptHandle,
                ThoughtForm: request.ThoughtForm,
                Telemetry: new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["engine-owner"] = "sli.lisp",
                    ["bounded-entrypoint"] = "run-agent-engine-idle-readiness",
                    ["engine-llm.profile"] = "provider-agnostic-test-seat"
                },
                ModuleNames: ["core.lisp", "typed-warm-use-rehearsal.lisp", "lab-gel-engrammitization.lisp", "agent-engine-idle-readiness.lisp"],
                ModuleCount: 4,
                BoundedEntrypointCalled: true,
                LoadAttempted: true,
                LoadSucceeded: true,
                AgentEngineIdleReadinessCompleted: true,
                EngineSeatKind: "engine-llm-candidate",
                EngineLlmProfile: "provider-agnostic-test-seat",
                ProviderNeutralityHeld: true,
                CrossModelTestHarnessApproachable: true,
                EngineLlmProviderAssumptionAllowed: false,
                EngineLlmInternalSubstrateClaimed: false,
                CodexAgentLabProfileStaged: true,
                CodexEngineSeatCandidateStaged: true,
                SubagentEngineSeatCandidateStaged: true,
                OperatorPresenceRequired: true,
                DriverSeated: false,
                DriverSeatCandidateStaged: true,
                AuthorityGrantCandidateStaged: true,
                AuthorityGrantAbsent: true,
                ActionExecutorCandidateStaged: true,
                ActionExecutorLocked: true,
                ActionExecutorArmed: false,
                GelAdmissionCandidateStaged: true,
                GelAdmissionLocked: true,
                SelfGelMutationCandidateStaged: true,
                SelfGelMutationLocked: true,
                HeartbeatCandidateStaged: true,
                HeartbeatLocked: true,
                HeartbeatActive: false,
                CmeActualCandidateStaged: true,
                CmeActualLocked: true,
                SanctuaryActualCandidateStaged: true,
                SanctuaryActualLocked: true,
                IdleLoopAllowed: true,
                EngineLlmMayArticulate: true,
                EngineLlmMayRehearse: true,
                EngineLlmMayFormCandidates: true,
                EngineLlmMayGrantAuthority: false,
                EngineLlmMayAuthorizeAction: false,
                EngineLlmMayExecuteAction: false,
                EngineLlmMayAdmitGel: false,
                EngineLlmMayMutateSelfGel: false,
                EngineLlmMayActivateActual: false,
                TypedScopeAccepted: true,
                SourceLabGelAcceptedCold: true,
                SourceEngramClosureAcceptedCold: true,
                SessionLineageWitnessed: true,
                ListeningFrameReceived: true,
                SliMembraneInterpretedPredicatePressure: true,
                CompassOrientedPressure: true,
                CompassCoolingRequired: true,
                SoulFrameReceivedListeningFrame: true,
                AgentiCoreReceivedCompassPressure: true,
                ThinkingAboutThinkingTelemetryProduced: true,
                StewardReviewed: true,
                ModelBindingAllowed: false,
                ArbitraryEvaluationAllowed: false,
                RuntimeActionAllowed: false,
                MemoryAdmissionAllowed: false,
                ContinuityAdmissionAllowed: false,
                GelAdmissionAllowed: false,
                SelfGelMutationAllowed: false,
                AuthorityGranted: false,
                ActionAuthorized: false,
                CmeActualActivationAllowed: false,
                SanctuaryActualActivationAllowed: false,
                ExitCode: 0,
                StandardOutput: "SAN-SLI-AGENT-ENGINE-IDLE-OK",
                StandardError: string.Empty,
                TimestampUtc: timestampUtc);
    }

    private sealed class AgentEngineIdleFixture : IDisposable
    {
        private AgentEngineIdleFixture(string rootPath)
        {
            RootPath = rootPath;
            LineRootPath = Path.Combine(rootPath, "line-root");
            InstallRootPath = Path.Combine(rootPath, "Sanctuary");
        }

        public string RootPath { get; }

        private string LineRootPath { get; }

        private string InstallRootPath { get; }

        public static AgentEngineIdleFixture Create()
        {
            var fixture = new AgentEngineIdleFixture(Path.Combine(Path.GetTempPath(), $"sanctuary-agent-idle-tests-{Guid.NewGuid():N}"));
            Directory.CreateDirectory(fixture.LineRootPath);
            Directory.CreateDirectory(fixture.InstallRootPath);
            return fixture;
        }

        public SanctuaryLabGelEngrammitizationReceipt CreateColdLabGelReceipt()
        {
            var sourceWarmUse = "urn:san:typed-warm-use:source";
            var sourceSliWarmUse = "sli-lisp-typed-warm-use://source";
            var sliLabGel = CreateSliLispLabGelReceipt(sourceWarmUse);
            var predicates = CreatePredicates(sourceWarmUse);
            var evidence = new LabGelEvidenceBody(
                EvidenceBodyHandle: "urn:san:lab-gel-evidence-body:source",
                SourceWarmUseReceiptHandle: sourceWarmUse,
                PredicateHandles: predicates.Select(static predicate => predicate.PredicateHandle).ToArray(),
                EvidenceBoundToWarmUseReceipt: true,
                EvidenceBoundToSliLispTelemetry: true,
                EvidenceCeilingPassive: true,
                ReviewOnly: true,
                GrantsWarrant: false,
                AdmitsContinuity: false,
                AuthorizesAction: false);
            var witness = new LabGelWitnessBody(
                WitnessBodyHandle: "urn:san:lab-gel-witness-body:source",
                SourceWarmUseReceiptHandle: sourceWarmUse,
                SourceSliLispReceiptHandle: sourceSliWarmUse,
                SessionId: "agent-engine-idle-session",
                TurnIndex: 3,
                PreservesWarmUseLineage: true,
                PreservesSessionLineage: true,
                SeparateCustody: true,
                ReviewOnly: true,
                AdmitsMemory: false,
                GrantsAuthority: false,
                AuthorizesAction: false);
            var candidate = new EngramCandidateReceipt(
                CandidateHandle: "urn:san:engram-candidate:source",
                SourceWarmUseReceiptHandle: sourceWarmUse,
                LabGelPredicateFamily: "lab-gel-pre-admission-warm-use-predicate-family",
                PredicateCount: predicates.Count,
                EvidenceBodyHandle: evidence.EvidenceBodyHandle,
                WitnessBodyHandle: witness.WitnessBodyHandle,
                CandidateFormed: true,
                PreAdmissionOnly: true,
                EvidenceBodyPresent: true,
                WitnessBodyPresent: true,
                CoolingRequired: true,
                StewardReviewRequired: true,
                GelAdmitted: false,
                EngramAdmitted: false,
                MemoryAdmitted: false,
                SelfGelMutated: false,
                ContinuityAdmitted: false,
                AuthorityGranted: false,
                ActionAuthorized: false);
            var cooling = new EngramCandidateCoolingReceipt(
                CoolingReceiptHandle: "urn:san:engram-candidate-cooling:source",
                CandidateHandle: candidate.CandidateHandle,
                CoolingRoute: "return-to-prime-lab-substrate-hold",
                HeldAsLabSubstrate: true,
                ReturnToPrimePreserved: true,
                ReviewOnly: true,
                AdmitsGel: false,
                AdmitsSelfGel: false,
                GrantsAuthority: false,
                AuthorizesAction: false);
            var review = new EngramPreAdmissionReviewReceipt(
                ReviewReceiptHandle: "urn:san:engram-pre-admission-review:source",
                CandidateHandle: candidate.CandidateHandle,
                ReviewOutcomeCode: "retain-as-lab-substrate-pre-admission",
                StewardReviewed: true,
                RecommendRetainAsLabSubstrate: true,
                RequiresFutureAdmissionGate: true,
                PerformsAdmission: false,
                MutatesGel: false,
                MutatesSelfGel: false,
                GrantsAuthority: false,
                AuthorizesAction: false);
            var readback = new LabGelReadbackReceipt(
                ReadbackReceiptHandle: "urn:san:lab-gel-readback:source",
                CandidateHandle: candidate.CandidateHandle,
                ReadbackScope: "lab-substrate-pre-admission-only",
                ReadbackAvailable: true,
                PreAdmissionOnly: true,
                LabSubstrateOnly: true,
                MayInformFutureRehearsal: true,
                MayInformActionAuthority: false,
                AdmitsMemory: false,
                AdmitsContinuity: false,
                GrantsAuthority: false,
                AuthorizesAction: false);
            var closure = new EngramClosureReceipt(
                ClosureReceiptHandle: "urn:san:engram-closure:source",
                CandidateHandle: candidate.CandidateHandle,
                EvidenceBodyHandle: evidence.EvidenceBodyHandle,
                WitnessBodyHandle: witness.WitnessBodyHandle,
                CoolingReceiptHandle: cooling.CoolingReceiptHandle,
                PreAdmissionReviewReceiptHandle: review.ReviewReceiptHandle,
                ReadbackReceiptHandle: readback.ReadbackReceiptHandle,
                PredicateHandles: predicates.Select(static predicate => predicate.PredicateHandle).ToArray(),
                ClosureState: "pre-admission-lab-substrate-closed",
                ClosureFormed: true,
                PreAdmissionOnly: true,
                LabSubstrateOnly: true,
                WitnessedBySliLisp: true,
                ClosureSealed: true,
                ReadyForEcPayload: true,
                AdmitsGel: false,
                AdmitsEngram: false,
                AdmitsMemory: false,
                MutatesSelfGel: false,
                AdmitsContinuity: false,
                GrantsAuthority: false,
                AuthorizesAction: false);

            return new SanctuaryLabGelEngrammitizationReceipt(
                ReceiptHandle: "urn:san:lab-gel-engrammitization:source",
                Disposition: SanctuaryLabGelEngrammitizationDisposition.CompletedCold,
                OutcomeCode: "sanctuary-lab-gel-engrammitization-completed-cold",
                GovernanceTrace: "test lab GEL source",
                LineRootPath: LineRootPath,
                InstallRootPath: InstallRootPath,
                ReceiptJsonPath: Path.Combine(InstallRootPath, "source-lab-gel.json"),
                ReceiptMarkdownPath: Path.Combine(InstallRootPath, "source-lab-gel.md"),
                SessionLedgerPath: Path.Combine(InstallRootPath, "source-lab-gel.jsonl"),
                SourceWarmUseReceiptHandle: sourceWarmUse,
                SourceSliLispWarmUseReceiptHandle: sourceSliWarmUse,
                PriorLabGelReceiptHandle: string.Empty,
                OperatorId: "YourNameHere.ID",
                Domain: "Civic",
                Role: "PaternalCareAssistance",
                JobClass: "Listening",
                SessionId: "agent-engine-idle-session",
                TurnIndex: 3,
                ThoughtForm: "The engine LLM may articulate and rehearse without becoming authority.",
                SliLispLabGelReceipt: sliLabGel,
                Predicates: predicates,
                EvidenceBody: evidence,
                WitnessBody: witness,
                EngramCandidate: candidate,
                CoolingReceipt: cooling,
                PreAdmissionReview: review,
                ReadbackReceipt: readback,
                EngramClosure: closure,
                ReviewOnly: true,
                SliLispOwnedEngineMotion: true,
                LabGelPredicateFormed: true,
                EngramCandidateFormed: true,
                EvidenceBodyFormed: true,
                WitnessBodyFormed: true,
                CoolingHeld: true,
                PreAdmissionReviewRequired: true,
                LabGelReadbackAvailable: true,
                EngramClosureFormed: true,
                EngramClosureReadyForEcPayload: true,
                CandidateRetainedAsLabSubstrate: true,
                LabGelAdmitted: false,
                EngramAdmitted: false,
                MemoryAdmitted: false,
                SelfGelMutated: false,
                ContinuityAdmitted: false,
                AuthorityGranted: false,
                ActionAuthorized: false,
                ActivationRefused: true,
                ModelBindingAllowed: false,
                ArbitraryLispEvaluationAllowed: false,
                RuntimeIdentityAllowed: false,
                RuntimeActionAllowed: false,
                DatabaseWriteAllowed: false,
                GelPromotionAllowed: false,
                CmeActualAllowed: false,
                SanctuaryActualAllowed: false,
                TimestampUtc: TimestampUtc);
        }

        public void Dispose()
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, recursive: true);
            }
        }

        private static IReadOnlyList<LabGelPredicateReceipt> CreatePredicates(string sourceWarmUse) =>
        [
            CreatePredicate(sourceWarmUse, LabGelPredicateClass.Semantic, "semantic"),
            CreatePredicate(sourceWarmUse, LabGelPredicateClass.Pressure, "pressure"),
            CreatePredicate(sourceWarmUse, LabGelPredicateClass.Witness, "witness"),
            CreatePredicate(sourceWarmUse, LabGelPredicateClass.Governance, "governance"),
            CreatePredicate(sourceWarmUse, LabGelPredicateClass.Morphology, "morphology"),
            CreatePredicate(sourceWarmUse, LabGelPredicateClass.Return, "return")
        ];

        private static LabGelPredicateReceipt CreatePredicate(
            string sourceWarmUse,
            LabGelPredicateClass predicateClass,
            string code) =>
            new(
                PredicateHandle: $"urn:san:lab-gel-predicate:{code}",
                PredicateClass: predicateClass,
                PredicateCode: $"lab-gel-{code}-predicate-pre-admission",
                SourceWarmUseReceiptHandle: sourceWarmUse,
                SourceResidueClass: code,
                EvidenceHandle: $"urn:san:lab-gel-evidence:{code}",
                WitnessHandle: $"urn:san:lab-gel-witness:{code}",
                ReviewOnly: true,
                PreAdmissionOnly: true,
                LabSubstrateOnly: true,
                MayEnterEngramCandidacy: true,
                GelAdmitted: false,
                SelfGelMutated: false,
                ContinuityAdmitted: false,
                AuthorityGranted: false,
                ActionAuthorized: false);

        private static SliLispLabGelEngrammitizationReceipt CreateSliLispLabGelReceipt(string sourceWarmUse) =>
            new(
                ReceiptHandle: "sli-lisp-lab-gel://source",
                Disposition: SliLispLabGelEngrammitizationDisposition.CompletedCold,
                OutcomeCode: "sli-lisp-lab-gel-engrammitization-completed-cold",
                RuntimeKind: "SBCL",
                RuntimePath: "sbcl",
                OperatorId: "YourNameHere.ID",
                Domain: "Civic",
                Role: "PaternalCareAssistance",
                JobClass: "Listening",
                SessionId: "agent-engine-idle-session",
                TurnIndex: 3,
                SourceWarmUseReceiptHandle: sourceWarmUse,
                ThoughtForm: "The engine LLM may articulate and rehearse without becoming authority.",
                Telemetry: new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["engine-owner"] = "sli.lisp",
                    ["bounded-entrypoint"] = "run-lab-gel-engrammitization"
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
                TimestampUtc: TimestampUtc);
    }
}
