using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class InstallFacingPredicatePostureCorrespondenceContractsTests
{
    [Fact]
    public void Correspondence_Contracts_Are_Read_Only_Reference_Types()
    {
        var set = new InstallFacingPredicatePostureCorrespondenceSet(
            new[]
            {
                new InstallFacingPredicatePostureCorrespondence(
                    PayloadMeaningKind: InstallFacingPayloadMeaningKind.InstallFacingPostureMeaning,
                    Lane: InstallFacingPredicatePostureLane.Posture,
                    PredicateFamily: SanctuaryGelPredicateFamily.Posture,
                    PredicateCandidateKind: SanctuaryGelPredicateCandidateKind.InstallFacing,
                    InstallFacingPhrase: "we are working in an install-facing posture",
                    InstallFacingSummary: "bounded install-first posture wording",
                    OperatorVisible: true,
                    CertifiedLaneOnly: false,
                    HdtSupportEligible: true),
                new InstallFacingPredicatePostureCorrespondence(
                    PayloadMeaningKind: InstallFacingPayloadMeaningKind.TrustAuthorizationMeaning,
                    Lane: InstallFacingPredicatePostureLane.TrustAuthorization,
                    PredicateFamily: SanctuaryGelPredicateFamily.TrustAuthorization,
                    PredicateCandidateKind: SanctuaryGelPredicateCandidateKind.CertifiedCommunication,
                    InstallFacingPhrase: "this lane is certified for communication",
                    InstallFacingSummary: "trust wording for certified communication footing",
                    OperatorVisible: true,
                    CertifiedLaneOnly: true,
                    HdtSupportEligible: true),
                new InstallFacingPredicatePostureCorrespondence(
                    PayloadMeaningKind: InstallFacingPayloadMeaningKind.EvidenceFootingMeaning,
                    Lane: InstallFacingPredicatePostureLane.EvidenceFooting,
                    PredicateFamily: SanctuaryGelPredicateFamily.EvidenceFooting,
                    PredicateCandidateKind: SanctuaryGelPredicateCandidateKind.AssentWitnessed,
                    InstallFacingPhrase: "this stands on witnessed assent footing",
                    InstallFacingSummary: "evidence wording for assent witness presence",
                    OperatorVisible: true,
                    CertifiedLaneOnly: false,
                    HdtSupportEligible: true),
                new InstallFacingPredicatePostureCorrespondence(
                    PayloadMeaningKind: InstallFacingPayloadMeaningKind.ResponseDispositionMeaning,
                    Lane: InstallFacingPredicatePostureLane.ResponseDisposition,
                    PredicateFamily: SanctuaryGelPredicateFamily.ResponseDisposition,
                    PredicateCandidateKind: SanctuaryGelPredicateCandidateKind.Silence,
                    InstallFacingPhrase: "this lane remains silent",
                    InstallFacingSummary: "response wording for silent non-passage",
                    OperatorVisible: false,
                    CertifiedLaneOnly: true,
                    HdtSupportEligible: false)
            });

        Assert.Collection(
            set.Correspondences,
            correspondence =>
            {
                Assert.Equal(InstallFacingPredicatePostureLane.Posture, correspondence.Lane);
                Assert.Equal(SanctuaryGelPredicateFamily.Posture, correspondence.PredicateFamily);
            },
            correspondence =>
            {
                Assert.Equal(InstallFacingPredicatePostureLane.TrustAuthorization, correspondence.Lane);
                Assert.Equal(SanctuaryGelPredicateFamily.TrustAuthorization, correspondence.PredicateFamily);
            },
            correspondence =>
            {
                Assert.Equal(InstallFacingPredicatePostureLane.EvidenceFooting, correspondence.Lane);
                Assert.Equal(SanctuaryGelPredicateFamily.EvidenceFooting, correspondence.PredicateFamily);
            },
            correspondence =>
            {
                Assert.Equal(InstallFacingPredicatePostureLane.ResponseDisposition, correspondence.Lane);
                Assert.Equal(SanctuaryGelPredicateFamily.ResponseDisposition, correspondence.PredicateFamily);
            });
    }

    [Fact]
    public void Correspondence_Boundary_Does_Not_Introduce_A_Control_Layer_Service()
    {
        var repoRoot = GetRepoRoot();
        var controlLayerPath = Path.Combine(repoRoot, "src", "San", "San.Nexus.Control");
        var matchingFiles = Directory.GetFiles(controlLayerPath, "*InstallFacingPredicatePosture*.cs", SearchOption.TopDirectoryOnly);

        Assert.Empty(matchingFiles);
    }

    [Fact]
    public void Sequence_Does_Not_Introduce_Template_Runtime_Or_Generator()
    {
        var repoRoot = GetRepoRoot();
        var lineRoot = repoRoot;
        var templateServiceFiles = Directory.GetFiles(lineRoot, "*Template*Service*.cs", SearchOption.AllDirectories);
        var templateGeneratorFiles = Directory.GetFiles(lineRoot, "*Template*Generator*.cs", SearchOption.AllDirectories);

        Assert.Empty(templateServiceFiles);
        Assert.Empty(templateGeneratorFiles);
    }

    [Fact]
    public void Sequence_Does_Not_Introduce_Readout_Service_Or_Evaluator()
    {
        var repoRoot = GetRepoRoot();
        var lineRoot = repoRoot;
        var readoutServiceFiles = Directory.GetFiles(lineRoot, "*Readout*Service*.cs", SearchOption.AllDirectories);
        var readoutEvaluatorFiles = Directory.GetFiles(lineRoot, "*Readout*Evaluator*.cs", SearchOption.AllDirectories);

        Assert.Empty(readoutServiceFiles);
        Assert.Empty(readoutEvaluatorFiles);
    }

    [Fact]
    public void Sequence_Does_Not_Introduce_Readout_Consumption_Service_Or_Runtime_Router()
    {
        var repoRoot = GetRepoRoot();
        var lineRoot = repoRoot;
        var consumptionServiceFiles = Directory.GetFiles(lineRoot, "*Readout*Consumption*Service*.cs", SearchOption.AllDirectories);
        var consumptionRouterFiles = Directory.GetFiles(lineRoot, "*Readout*Router*.cs", SearchOption.AllDirectories);

        Assert.Empty(consumptionServiceFiles);
        Assert.Empty(consumptionRouterFiles);
    }

    [Fact]
    public void Sequence_Does_Not_Introduce_Return_Posture_Service_Or_Rtme_Handoff_Code()
    {
        var repoRoot = GetRepoRoot();
        var lineRoot = repoRoot;
        var returnServiceFiles = Directory.GetFiles(lineRoot, "*Return*Posture*Service*.cs", SearchOption.AllDirectories);
        var rtmeHandoffFiles = Directory.GetFiles(lineRoot, "*Return*Posture*Rtme*.cs", SearchOption.AllDirectories);
        var preCertificationAuthorityFiles = FindActivePreCertificationAuthorityFiles(lineRoot);

        Assert.Empty(returnServiceFiles);
        Assert.Empty(rtmeHandoffFiles);
        Assert.Empty(preCertificationAuthorityFiles);
    }

    [Fact]
    public void Sequence_Does_Not_Introduce_Approach_Emitter_Template_Or_Sli_Lisp_Mutation()
    {
        var repoRoot = GetRepoRoot();
        var lineRoot = repoRoot;
        var sliLispPath = Path.Combine(lineRoot, "src", "SLI", "SLI.Lisp");
        var approachServiceFiles = Directory.GetFiles(lineRoot, "*Approach*Boundary*Service*.cs", SearchOption.AllDirectories);
        var telemetryEmitterFiles = Directory.GetFiles(lineRoot, "*Telemetry*Emitter*.cs", SearchOption.AllDirectories);
        var templateGeneratorFiles = Directory.GetFiles(lineRoot, "*Predicate*Template*Generator*.cs", SearchOption.AllDirectories);
        var rtmeHandoffFiles = Directory.GetFiles(lineRoot, "*Approach*Rtme*.cs", SearchOption.AllDirectories);
        var preCertificationFiles = Directory.GetFiles(lineRoot, "*Approach*Pre*Certification*.cs", SearchOption.AllDirectories);
        var sliLispApproachFiles = Directory.GetFiles(sliLispPath, "*Approach*Boundary*.cs", SearchOption.AllDirectories);
        var sliLispApproachLispFiles = Directory.GetFiles(sliLispPath, "*approach*.lisp", SearchOption.AllDirectories);

        Assert.Empty(approachServiceFiles);
        Assert.Empty(telemetryEmitterFiles);
        Assert.Empty(templateGeneratorFiles);
        Assert.Empty(rtmeHandoffFiles);
        Assert.Empty(preCertificationFiles);
        Assert.Empty(sliLispApproachFiles);
        Assert.Empty(sliLispApproachLispFiles);
    }

    [Fact]
    public void Sequence_Does_Not_Introduce_Pre_Governing_Standing_Authority_Engines()
    {
        var repoRoot = GetRepoRoot();
        var lineRoot = repoRoot;
        var standingServiceFiles = Directory.GetFiles(lineRoot, "*Pre*Governing*Standing*Service*.cs", SearchOption.AllDirectories);
        var consentEngineFiles = Directory.GetFiles(lineRoot, "*Consent*Engine*.cs", SearchOption.AllDirectories);
        var legalDocumentGeneratorFiles = Directory.GetFiles(lineRoot, "*Legal*Document*Generator*.cs", SearchOption.AllDirectories);
        var telemetryEmitterFiles = Directory.GetFiles(lineRoot, "*Telemetry*Emitter*.cs", SearchOption.AllDirectories);
        var rtmeHandoffFiles = Directory.GetFiles(lineRoot, "*Standing*Rtme*.cs", SearchOption.AllDirectories);
        var domainAuthorizationFiles = Directory.GetFiles(lineRoot, "*Domain*Authorization*Service*.cs", SearchOption.AllDirectories);
        var preCertificationAuthorityFiles = FindActivePreCertificationAuthorityFiles(lineRoot);

        Assert.Empty(standingServiceFiles);
        Assert.Empty(consentEngineFiles);
        Assert.Empty(legalDocumentGeneratorFiles);
        Assert.Empty(telemetryEmitterFiles);
        Assert.Empty(rtmeHandoffFiles);
        Assert.Empty(domainAuthorizationFiles);
        Assert.Empty(preCertificationAuthorityFiles);
    }

    private static string[] FindActivePreCertificationAuthorityFiles(string lineRoot)
    {
        return Directory
            .GetFiles(lineRoot, "*Pre*Certification*.cs", SearchOption.AllDirectories)
            .Where(path =>
            {
                var fileName = Path.GetFileName(path);

                return fileName.Contains("Service", StringComparison.OrdinalIgnoreCase) ||
                       fileName.Contains("Evaluator", StringComparison.OrdinalIgnoreCase) ||
                       fileName.Contains("Engine", StringComparison.OrdinalIgnoreCase) ||
                       fileName.Contains("Generator", StringComparison.OrdinalIgnoreCase) ||
                       fileName.Contains("Runtime", StringComparison.OrdinalIgnoreCase) ||
                       fileName.Contains("Owner", StringComparison.OrdinalIgnoreCase) ||
                       fileName.Contains("Handoff", StringComparison.OrdinalIgnoreCase);
            })
            .ToArray();
    }

    private static string GetRepoRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);

        while (current is not null &&
               (!File.Exists(Path.Combine(current.FullName, "build.ps1")) ||
                !File.Exists(Path.Combine(current.FullName, "San.sln"))))
        {
            current = current.Parent;
        }

        return current?.FullName ?? throw new InvalidOperationException("Unable to locate repository root.");
    }
}
