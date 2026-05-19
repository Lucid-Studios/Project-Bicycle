using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum MatureDisciplineDomain
{
    SignalProcessing = 0,
    Telecommunications = 1,
    ControlTheory = 2,
    NetworkScheduling = 3,
    DistributedSystems = 4,
    AcousticEngineering = 5
}

public enum HarmonicInterlockModulationCorrespondenceDisposition
{
    AtlasReviewCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public sealed record MatureDisciplineSource(
    string SourceHandle,
    MatureDisciplineDomain Domain,
    string SourceName,
    string SourceSuccessCondition,
    bool ReviewOnly,
    bool Inert,
    bool ClaimsEquivalence,
    bool ClaimsProofTransfer,
    bool ClaimsOntologyTransfer,
    bool ClaimsAuthority)
{
    public bool IsColdSource =>
        !string.IsNullOrWhiteSpace(SourceHandle) &&
        !string.IsNullOrWhiteSpace(SourceName) &&
        !string.IsNullOrWhiteSpace(SourceSuccessCondition) &&
        ReviewOnly &&
        Inert &&
        !ClaimsEquivalence &&
        !ClaimsProofTransfer &&
        !ClaimsOntologyTransfer &&
        !ClaimsAuthority;
}

public sealed record BorrowedCorrespondenceConcept(
    string ConceptHandle,
    string SourceHandle,
    string ConceptName,
    string SourceDomainSuccessCondition,
    string CmeTranslation,
    string ExplicitNonClaim,
    string ActualizationTest,
    IReadOnlyList<string> LossConditions,
    bool BorrowStructureNotAuthority,
    bool BorrowAnalogyNotProof,
    bool BorrowMechanismNotOntology,
    bool ReGovernedUnderCmeLaw,
    bool ChannelSuccessBecomesSemanticWarrant,
    bool TransmissionBecomesAdmissibility,
    bool SynchronizationBecomesAuthority,
    bool ThroughputBecomesContinuity,
    bool PersistenceBecomesContinuity,
    bool StabilityBecomesTruth,
    bool ImportedSuccessBecomesGovernanceCondition)
{
    public bool IsColdConcept =>
        !string.IsNullOrWhiteSpace(ConceptHandle) &&
        !string.IsNullOrWhiteSpace(SourceHandle) &&
        !string.IsNullOrWhiteSpace(ConceptName) &&
        !string.IsNullOrWhiteSpace(SourceDomainSuccessCondition) &&
        !string.IsNullOrWhiteSpace(CmeTranslation) &&
        !string.IsNullOrWhiteSpace(ExplicitNonClaim) &&
        !string.IsNullOrWhiteSpace(ActualizationTest) &&
        LossConditions.Count > 0 &&
        BorrowStructureNotAuthority &&
        BorrowAnalogyNotProof &&
        BorrowMechanismNotOntology &&
        ReGovernedUnderCmeLaw &&
        !ChannelSuccessBecomesSemanticWarrant &&
        !TransmissionBecomesAdmissibility &&
        !SynchronizationBecomesAuthority &&
        !ThroughputBecomesContinuity &&
        !PersistenceBecomesContinuity &&
        !StabilityBecomesTruth &&
        !ImportedSuccessBecomesGovernanceCondition;
}

public sealed record CmeCorrespondenceTranslationBoundary(
    string BoundaryCode,
    bool Present,
    bool SemanticCustodyRequired,
    bool WitnessBurdenRequired,
    bool AuthorityCeilingRequired,
    bool ContinuityRiskRequired,
    bool RevocationPathRequired,
    bool ExplicitNonClaimRequired,
    bool AllowsEquivalenceClaim,
    bool AllowsProofTransfer,
    bool AllowsOntologyTransfer,
    bool AllowsSourceSuccessAsCmeSuccess,
    bool AllowsChannelSuccessAsWarrant)
{
    public bool IsColdTranslationBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryCode) &&
        Present &&
        SemanticCustodyRequired &&
        WitnessBurdenRequired &&
        AuthorityCeilingRequired &&
        ContinuityRiskRequired &&
        RevocationPathRequired &&
        ExplicitNonClaimRequired &&
        !AllowsEquivalenceClaim &&
        !AllowsProofTransfer &&
        !AllowsOntologyTransfer &&
        !AllowsSourceSuccessAsCmeSuccess &&
        !AllowsChannelSuccessAsWarrant;
}

public sealed record CorrespondenceActualizationTestBoundary(
    string BoundaryCode,
    bool Present,
    bool PreservesIntendedGoal,
    bool PreservesCustody,
    bool PreservesWitness,
    bool PreservesRevocation,
    bool PreservesContinuitySafety,
    bool RefusesAuthorityLaundering,
    bool RefusesSemanticWarrantFromPropagation,
    bool AllowsRuntimeAction,
    bool AllowsLispEvaluation,
    bool AllowsPacketEmission,
    bool AllowsReceiptReplay,
    bool AllowsPassageIncrement,
    bool AllowsContinuityAdmission,
    bool AllowsAuthority)
{
    public bool IsColdActualizationBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryCode) &&
        Present &&
        PreservesIntendedGoal &&
        PreservesCustody &&
        PreservesWitness &&
        PreservesRevocation &&
        PreservesContinuitySafety &&
        RefusesAuthorityLaundering &&
        RefusesSemanticWarrantFromPropagation &&
        !AllowsRuntimeAction &&
        !AllowsLispEvaluation &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement &&
        !AllowsContinuityAdmission &&
        !AllowsAuthority;
}

public sealed record CorrespondenceLossCondition(
    string LossHandle,
    string ForbiddenCollapse,
    bool Refused,
    bool RetainedForReview,
    bool GrantsAuthority,
    bool AdmitsContinuity,
    bool ActivatesRuntime)
{
    public bool IsColdLossCondition =>
        !string.IsNullOrWhiteSpace(LossHandle) &&
        !string.IsNullOrWhiteSpace(ForbiddenCollapse) &&
        Refused &&
        RetainedForReview &&
        !GrantsAuthority &&
        !AdmitsContinuity &&
        !ActivatesRuntime;
}

public sealed record HarmonicInterlockModulationCorrespondenceRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record HarmonicInterlockModulationCorrespondenceRequest(
    StewardHarmonicCustodyInterlockReceipt? SourceInterlockReceipt,
    IReadOnlyList<MatureDisciplineSource> Sources,
    IReadOnlyList<BorrowedCorrespondenceConcept> Concepts,
    CmeCorrespondenceTranslationBoundary TranslationBoundary,
    CorrespondenceActualizationTestBoundary ActualizationBoundary,
    IReadOnlyList<CorrespondenceLossCondition> LossConditions,
    int PriorPassageCount);

public sealed record HarmonicInterlockModulationCorrespondenceReceipt(
    string ReceiptHandle,
    HarmonicInterlockModulationCorrespondenceDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceInterlockReceiptHandle,
    IReadOnlyList<MatureDisciplineSource> Sources,
    IReadOnlyList<BorrowedCorrespondenceConcept> Concepts,
    CmeCorrespondenceTranslationBoundary TranslationBoundary,
    CorrespondenceActualizationTestBoundary ActualizationBoundary,
    IReadOnlyList<CorrespondenceLossCondition> LossConditions,
    IReadOnlyList<string> PreservedSourceHandles,
    IReadOnlyList<string> PreservedConceptHandles,
    HarmonicInterlockModulationCorrespondenceRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterCorrespondenceReview,
    bool ReviewOnly,
    bool InertOnly,
    bool CorrespondenceBecomesEquivalence,
    bool BorrowedAnalogyBecomesProof,
    bool BorrowedMechanismBecomesOntology,
    bool ImportedSuccessBecomesGovernanceCondition,
    bool ChannelSuccessBecomesSemanticWarrant,
    bool TransmissionBecomesAdmissibility,
    bool SynchronizationBecomesAuthority,
    bool ThroughputBecomesContinuity,
    bool RuntimeActionAllowed,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdCorrespondenceAtlas =>
        (Disposition is HarmonicInterlockModulationCorrespondenceDisposition.AtlasReviewCold or
            HarmonicInterlockModulationCorrespondenceDisposition.EmptyReviewCold) &&
        Refusal is null &&
        ReviewOnly &&
        InertOnly &&
        !CorrespondenceBecomesEquivalence &&
        !BorrowedAnalogyBecomesProof &&
        !BorrowedMechanismBecomesOntology &&
        !ImportedSuccessBecomesGovernanceCondition &&
        !ChannelSuccessBecomesSemanticWarrant &&
        !TransmissionBecomesAdmissibility &&
        !SynchronizationBecomesAuthority &&
        !ThroughputBecomesContinuity &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused &&
        PassageCountAfterCorrespondenceReview == PriorPassageCount;

    public bool IsRetainedCorrespondenceRefusal =>
        Disposition == HarmonicInterlockModulationCorrespondenceDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterCorrespondenceReview == PriorPassageCount &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused;
}

public sealed class DefaultHarmonicInterlockModulationCorrespondenceBoundaryValidator
{
    public HarmonicInterlockModulationCorrespondenceReceipt Declare(
        HarmonicInterlockModulationCorrespondenceRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceInterlockReceipt is null ||
            !request.SourceInterlockReceipt.IsColdInterlock)
        {
            return Refuse(
                request,
                "modulation-correspondence-source-interlock-missing",
                "Modulation correspondence refused because a cold Steward harmonic custody interlock receipt is required.",
                timestampUtc);
        }

        if (!request.TranslationBoundary.IsColdTranslationBoundary)
        {
            return Refuse(
                request,
                "modulation-correspondence-translation-boundary-promotional",
                "Modulation correspondence refused because translation must require semantic custody, witness, authority ceiling, continuity risk, revocation, and explicit non-claim while refusing equivalence and source-domain success inheritance.",
                timestampUtc);
        }

        if (!request.ActualizationBoundary.IsColdActualizationBoundary)
        {
            return Refuse(
                request,
                "modulation-correspondence-actualization-boundary-promotional",
                "Modulation correspondence refused because actualization must preserve goal, custody, witness, revocation, and continuity safety without runtime action, Lisp evaluation, packet emission, replay, passage, authority, or continuity admission.",
                timestampUtc);
        }

        if (request.Sources.Any(static source => !source.IsColdSource))
        {
            return Refuse(
                request,
                "modulation-correspondence-source-promotional-refused",
                "Mature discipline source refused because source domains may contribute structure only as review-only evidence without equivalence, proof transfer, ontology transfer, or authority.",
                timestampUtc);
        }

        var sourceHandles = request.Sources
            .Select(static source => source.SourceHandle)
            .ToHashSet(StringComparer.Ordinal);

        if (request.Concepts.Any(concept => !sourceHandles.Contains(concept.SourceHandle)))
        {
            return Refuse(
                request,
                "modulation-correspondence-concept-source-missing",
                "Borrowed concept refused because every correspondence concept must bind to a declared mature discipline source.",
                timestampUtc);
        }

        if (request.Concepts.Any(static concept => !concept.IsColdConcept))
        {
            return Refuse(
                request,
                "modulation-correspondence-concept-collapse-refused",
                "Borrowed concept refused because structure, analogy, and mechanism must be re-governed under CME law without becoming authority, proof, ontology, semantic warrant, admissibility, authority, continuity, truth, or governance condition.",
                timestampUtc);
        }

        if (request.LossConditions.Any(static loss => !loss.IsColdLossCondition))
        {
            return Refuse(
                request,
                "modulation-correspondence-loss-condition-promotional",
                "Loss condition refused because forbidden collapse mappings must be refused and retained for review without authority, continuity, or activation.",
                timestampUtc);
        }

        var duplicateConcept = request.Concepts
            .GroupBy(static concept => concept.ConceptHandle, StringComparer.Ordinal)
            .Any(static group => group.Count() > 1);
        if (duplicateConcept)
        {
            return Refuse(
                request,
                "modulation-correspondence-duplicate-concept-refused",
                "Modulation correspondence refused because duplicate concept handles would collapse atlas lineage.",
                timestampUtc);
        }

        var disposition = request.Concepts.Count == 0
            ? HarmonicInterlockModulationCorrespondenceDisposition.EmptyReviewCold
            : HarmonicInterlockModulationCorrespondenceDisposition.AtlasReviewCold;
        var outcomeCode = disposition == HarmonicInterlockModulationCorrespondenceDisposition.EmptyReviewCold
            ? "modulation-correspondence-empty-review-only"
            : "modulation-correspondence-atlas-review-only";

        return new HarmonicInterlockModulationCorrespondenceReceipt(
            ReceiptHandle: $"urn:san:modulation-correspondence:review:{ShortHash(request.SourceInterlockReceipt.ReceiptHandle, outcomeCode, request.Concepts.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: "Mature modulation and coexistence disciplines may inform Steward interlock through disciplined selective correspondence. Their success conditions may not become CME governance conditions.",
            SourceInterlockReceiptHandle: request.SourceInterlockReceipt.ReceiptHandle,
            Sources: request.Sources.ToArray(),
            Concepts: request.Concepts.ToArray(),
            TranslationBoundary: request.TranslationBoundary,
            ActualizationBoundary: request.ActualizationBoundary,
            LossConditions: request.LossConditions.ToArray(),
            PreservedSourceHandles: request.Sources.Select(static source => source.SourceHandle).ToArray(),
            PreservedConceptHandles: request.Concepts.Select(static concept => concept.ConceptHandle).ToArray(),
            Refusal: null,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterCorrespondenceReview: request.PriorPassageCount,
            ReviewOnly: true,
            InertOnly: true,
            CorrespondenceBecomesEquivalence: false,
            BorrowedAnalogyBecomesProof: false,
            BorrowedMechanismBecomesOntology: false,
            ImportedSuccessBecomesGovernanceCondition: false,
            ChannelSuccessBecomesSemanticWarrant: false,
            TransmissionBecomesAdmissibility: false,
            SynchronizationBecomesAuthority: false,
            ThroughputBecomesContinuity: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static HarmonicInterlockModulationCorrespondenceReceipt Refuse(
        HarmonicInterlockModulationCorrespondenceRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc)
    {
        var sourceHandle = request.SourceInterlockReceipt?.ReceiptHandle ?? "missing-steward-interlock-source";
        return new HarmonicInterlockModulationCorrespondenceReceipt(
            ReceiptHandle: $"urn:san:modulation-correspondence:refused:{ShortHash(sourceHandle, outcomeCode, timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: HarmonicInterlockModulationCorrespondenceDisposition.Refused,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceInterlockReceiptHandle: sourceHandle,
            Sources: [],
            Concepts: [],
            TranslationBoundary: request.TranslationBoundary,
            ActualizationBoundary: request.ActualizationBoundary,
            LossConditions: [],
            PreservedSourceHandles: [],
            PreservedConceptHandles: [],
            Refusal: new HarmonicInterlockModulationCorrespondenceRefusalReceipt(
                ReceiptHandle: $"urn:san:modulation-correspondence-refusal:{ShortHash(sourceHandle, outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterCorrespondenceReview: request.PriorPassageCount,
            ReviewOnly: true,
            InertOnly: true,
            CorrespondenceBecomesEquivalence: false,
            BorrowedAnalogyBecomesProof: false,
            BorrowedMechanismBecomesOntology: false,
            ImportedSuccessBecomesGovernanceCondition: false,
            ChannelSuccessBecomesSemanticWarrant: false,
            TransmissionBecomesAdmissibility: false,
            SynchronizationBecomesAuthority: false,
            ThroughputBecomesContinuity: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static string ShortHash(params string[] parts)
    {
        var payload = string.Join("|", parts);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(bytes)[..16].ToLowerInvariant();
    }
}
