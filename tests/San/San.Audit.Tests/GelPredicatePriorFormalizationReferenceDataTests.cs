using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class GelPredicatePriorFormalizationReferenceDataTests
{
    [Fact]
    public void Passive_Contracts_Expose_Predicate_Prior_Formalization_Types()
    {
        Assert.Contains(GelPredicatePriorFormalizationDisposition.Ready, Enum.GetValues<GelPredicatePriorFormalizationDisposition>());
        Assert.Contains(GelPredicatePriorFormalizationDisposition.Held, Enum.GetValues<GelPredicatePriorFormalizationDisposition>());
        Assert.Contains(GelPredicatePriorFormalizationDisposition.Refused, Enum.GetValues<GelPredicatePriorFormalizationDisposition>());

        Assert.Contains(GelPredicatePriorAdmissionCeiling.CandidateOnly, Enum.GetValues<GelPredicatePriorAdmissionCeiling>());
        Assert.Contains(GelPredicatePriorMutationPolicyPosture.InvariantPreservingOnly, Enum.GetValues<GelPredicatePriorMutationPolicyPosture>());
        Assert.Contains(GelPredicatePriorTransportReceiptPosture.FutureReceiptEligible, Enum.GetValues<GelPredicatePriorTransportReceiptPosture>());

        Assert.Contains(GelPredicatePriorFormalizationRefusalReason.MissingUtf8Witness, Enum.GetValues<GelPredicatePriorFormalizationRefusalReason>());
        Assert.Contains(GelPredicatePriorFormalizationRefusalReason.MissingRootPredicate, Enum.GetValues<GelPredicatePriorFormalizationRefusalReason>());
        Assert.Contains(GelPredicatePriorFormalizationRefusalReason.MissingSliConstructor, Enum.GetValues<GelPredicatePriorFormalizationRefusalReason>());
        Assert.Contains(GelPredicatePriorFormalizationRefusalReason.EcMutationOrTransportOverclaimed, Enum.GetValues<GelPredicatePriorFormalizationRefusalReason>());
        Assert.Contains(GelPredicatePriorFormalizationRefusalReason.SliLispActivationOverclaimed, Enum.GetValues<GelPredicatePriorFormalizationRefusalReason>());
    }

    [Fact]
    public void Canonical_Reference_Data_Includes_Ready_Held_And_Refused_Records()
    {
        Assert.Equal(GelPredicatePriorFormalizationDisposition.Ready, GelPredicatePriorFormalizationReferenceData.ReadyPredicatePrior.Disposition);
        Assert.Equal(GelPredicatePriorFormalizationDisposition.Held, GelPredicatePriorFormalizationReferenceData.HeldForAmbiguityOrCarrierReview.Disposition);
        Assert.Contains(GelPredicatePriorFormalizationReferenceData.CanonicalRecords, record => record.Disposition == GelPredicatePriorFormalizationDisposition.Refused);
    }

    [Fact]
    public void Ready_Predicate_Prior_Preserves_Witness_Root_Constructor_Invariants_And_Candidate_Only_Ceiling()
    {
        var record = GelPredicatePriorFormalizationReferenceData.ReadyPredicatePrior;

        Assert.Equal("utf-8-preserved", record.Utf8Witness.EncodingState);
        Assert.Contains("utf8-witness-ref://accept-source", record.Utf8Witness.WitnessRefs);
        Assert.Equal("rt.accept", record.RootPredicate.RootCarrier);
        Assert.Contains("does not create local Atlas authority", record.RootPredicate.LineageSummary, StringComparison.Ordinal);
        Assert.Equal("a", record.Constructor.PrefixSuper);
        Assert.Equal("complement", record.Constructor.PrefixSub);
        Assert.Equal("delta-body", record.Constructor.Body);
        Assert.Equal("K", record.Constructor.SuffixSuper);
        Assert.Equal("107", record.Constructor.SuffixSub);
        Assert.Contains("flat rendering is display-only", record.Constructor.StructuralSummary, StringComparison.Ordinal);
        Assert.Contains("root-body-preserved", record.InvariantSummaries);
        Assert.Equal(GelPredicatePriorAdmissionCeiling.CandidateOnly, record.AdmissionCeiling);
        Assert.Contains("does not admit final Sanctuary.GEL survivor standing, grant first use, authorize EC mutation, activate SLI.Lisp, emit transport, or create runtime authority", record.NonAdmissionSummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Held_Predicate_Prior_Withholds_Mutation_And_Transport()
    {
        var record = GelPredicatePriorFormalizationReferenceData.HeldForAmbiguityOrCarrierReview;

        Assert.Equal(GelPredicatePriorFormalizationDisposition.Held, record.Disposition);
        Assert.Equal(GelPredicatePriorMutationPolicyPosture.Withheld, record.MutationPolicyPosture);
        Assert.Equal(GelPredicatePriorTransportReceiptPosture.NotEmitted, record.TransportReceiptPosture);
        Assert.Equal(GelPredicatePriorAdmissionCeiling.CandidateOnly, record.AdmissionCeiling);
        Assert.Contains("non-operative", record.NonAdmissionSummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Missing_Witness_Root_Constructor_And_Predicate_Surface_Refuse_Formalization()
    {
        var missingWitness = GelPredicatePriorFormalizationReferenceData.RefusedMissingUtf8Witness;
        var missingRoot = GelPredicatePriorFormalizationReferenceData.RefusedMissingRootPredicate;
        var missingConstructor = GelPredicatePriorFormalizationReferenceData.RefusedMissingSliConstructor;
        var missingSurface = GelPredicatePriorFormalizationReferenceData.RefusedMissingEngrammitizationOrPredicateSurface;

        Assert.Contains(GelPredicatePriorFormalizationRefusalReason.MissingUtf8Witness, missingWitness.RefusalReasons);
        Assert.Equal("missing", missingWitness.Utf8Witness.EncodingState);
        Assert.Contains(GelPredicatePriorFormalizationRefusalReason.MissingRootPredicate, missingRoot.RefusalReasons);
        Assert.Equal("missing", missingRoot.RootPredicate.RootCarrier);
        Assert.Contains(GelPredicatePriorFormalizationRefusalReason.MissingSliConstructor, missingConstructor.RefusalReasons);
        Assert.Equal("missing", missingConstructor.Constructor.Body);
        Assert.Contains(GelPredicatePriorFormalizationRefusalReason.MissingEngrammitizationFacingPosture, missingSurface.RefusalReasons);
        Assert.Contains(GelPredicatePriorFormalizationRefusalReason.MissingPredicateSurfaceReadiness, missingSurface.RefusalReasons);
    }

    [Fact]
    public void Admission_Mutation_Lisp_Runtime_And_Governance_Overclaims_Refuse_Formalization()
    {
        var record = GelPredicatePriorFormalizationReferenceData.RefusedAdmissionMutationLispRuntimeOrGovernanceOverclaim;

        Assert.Equal(GelPredicatePriorFormalizationDisposition.Refused, record.Disposition);
        Assert.Contains(GelPredicatePriorFormalizationRefusalReason.SurvivorAdmissionOverclaimed, record.RefusalReasons);
        Assert.Contains(GelPredicatePriorFormalizationRefusalReason.FirstUseOverclaimed, record.RefusalReasons);
        Assert.Contains(GelPredicatePriorFormalizationRefusalReason.EcMutationOrTransportOverclaimed, record.RefusalReasons);
        Assert.Contains(GelPredicatePriorFormalizationRefusalReason.SliLispActivationOverclaimed, record.RefusalReasons);
        Assert.Contains(GelPredicatePriorFormalizationRefusalReason.RuntimeOrGovernanceOverclaimed, record.RefusalReasons);
        Assert.Equal(GelPredicatePriorMutationPolicyPosture.Withheld, record.MutationPolicyPosture);
        Assert.Equal(GelPredicatePriorTransportReceiptPosture.RefusedAsActiveTransport, record.TransportReceiptPosture);
        Assert.Contains("No final Sanctuary.GEL survivor admission, first use, EC mutation, SLI.Lisp activation, transport emission, governing CME, RTME, or runtime authority is granted.", record.NonAdmissionSummary, StringComparison.Ordinal);
    }

    [Fact]
    public void Predicate_Prior_Formalization_Does_Not_Introduce_Service_Evaluator_Runtime_Lisp_Mutation_Or_Admission_Owner()
    {
        var repoRoot = GetRepoRoot();
        var srcRoot = Path.Combine(repoRoot, "src");
        var sliLispPath = Path.Combine(srcRoot, "SLI", "SLI.Lisp");

        var sourceFiles = Directory
            .EnumerateFiles(srcRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path =>
                !path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase) &&
                !path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .ToArray();
        var lispFiles = Directory.Exists(sliLispPath)
            ? Directory.EnumerateFiles(sliLispPath, "*.lisp", SearchOption.AllDirectories).ToArray()
            : Array.Empty<string>();

        Assert.DoesNotContain(sourceFiles, path =>
            path.Contains($"{Path.DirectorySeparatorChar}San.Nexus.Control{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase) &&
            Path.GetFileName(path).Contains("GelPredicatePriorFormalization", StringComparison.OrdinalIgnoreCase));

        Assert.DoesNotContain(sourceFiles, path =>
            Path.GetFileNameWithoutExtension(path).Contains("GelPredicatePriorFormalization", StringComparison.OrdinalIgnoreCase) &&
            (Path.GetFileNameWithoutExtension(path).Contains("Service", StringComparison.OrdinalIgnoreCase) ||
             Path.GetFileNameWithoutExtension(path).Contains("Evaluator", StringComparison.OrdinalIgnoreCase) ||
             Path.GetFileNameWithoutExtension(path).Contains("Runtime", StringComparison.OrdinalIgnoreCase) ||
             Path.GetFileNameWithoutExtension(path).Contains("Owner", StringComparison.OrdinalIgnoreCase) ||
             Path.GetFileNameWithoutExtension(path).Contains("Admission", StringComparison.OrdinalIgnoreCase)));

        Assert.DoesNotContain(sourceFiles, path =>
            Path.GetFileNameWithoutExtension(path).Contains("Mutation", StringComparison.OrdinalIgnoreCase) ||
            Path.GetFileNameWithoutExtension(path).Contains("TransportEmitter", StringComparison.OrdinalIgnoreCase) ||
            Path.GetFileNameWithoutExtension(path).Contains("SliLispActivation", StringComparison.OrdinalIgnoreCase));

        Assert.DoesNotContain(lispFiles, path => Path.GetFileName(path).Contains("predicate-prior", StringComparison.OrdinalIgnoreCase));
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
