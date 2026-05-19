using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum PeerReviewPredicateBridgeDisposition
{
    BridgesRetainedCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public enum PeerReviewEvidenceStatus
{
    Demonstrated = 0,
    Suggested = 1,
    Interpretive = 2,
    Speculative = 3,
    Unsupported = 4,
    Overstated = 5
}

public sealed record PeerReviewBridgeSegment(
    string SegmentHandle,
    string SourceResidueHandle,
    string AuthorTerm,
    string LocalDefinition,
    string WhyItMatters,
    string OperationalImplication,
    string Evaluation,
    string BoundedConclusion,
    PeerReviewEvidenceStatus EvidenceStatus,
    string AudienceStateRef,
    string ContextQuarantineRef,
    bool ReviewOnly,
    bool ReaderStateContinuityMapped,
    bool TerminologyQuarantined,
    bool ContextQuarantined,
    bool ReviewStateIsolated,
    bool ConversationalDepthRetained,
    bool BridgeSynthesisOnly,
    bool PriorDoctrineUsedAsPostureOnly,
    bool AuthorTermBecomesAuthority,
    bool LocalDefinitionBecomesProof,
    bool WhyItMattersBecomesEvidence,
    bool OperationalImplicationAuthorizesAction,
    bool EvaluationGrantsWarrant,
    bool BoundedConclusionAdmitsTruth,
    bool RespectBecomesAgreement,
    bool CriticismBecomesContempt,
    bool ProseSmoothingHidesConcern,
    bool PriorDoctrineBecomesInterpretiveAuthority,
    bool ConceptualProximityBecomesEquivalence,
    bool ReviewArchitectureColonizesPaper,
    bool ConversationalDepthBecomesAdvocacy,
    bool BridgeBecomesMemory,
    bool BridgeAdmitsContinuity,
    bool BridgeGrantsAuthority,
    bool BridgeAuthorizesAction,
    bool BridgeEvaluatesLisp,
    bool BridgeEmitsPacket,
    bool BridgeReplaysReceipt,
    bool BridgeIncrementsPassage,
    bool BridgeActivates)
{
    public bool IsColdBridgeSegment =>
        !string.IsNullOrWhiteSpace(SegmentHandle) &&
        !string.IsNullOrWhiteSpace(SourceResidueHandle) &&
        !string.IsNullOrWhiteSpace(AuthorTerm) &&
        !string.IsNullOrWhiteSpace(LocalDefinition) &&
        !string.IsNullOrWhiteSpace(WhyItMatters) &&
        !string.IsNullOrWhiteSpace(OperationalImplication) &&
        !string.IsNullOrWhiteSpace(Evaluation) &&
        !string.IsNullOrWhiteSpace(BoundedConclusion) &&
        !string.IsNullOrWhiteSpace(AudienceStateRef) &&
        !string.IsNullOrWhiteSpace(ContextQuarantineRef) &&
        Enum.IsDefined(EvidenceStatus) &&
        ReviewOnly &&
        ReaderStateContinuityMapped &&
        TerminologyQuarantined &&
        ContextQuarantined &&
        ReviewStateIsolated &&
        ConversationalDepthRetained &&
        BridgeSynthesisOnly &&
        PriorDoctrineUsedAsPostureOnly &&
        !AuthorTermBecomesAuthority &&
        !LocalDefinitionBecomesProof &&
        !WhyItMattersBecomesEvidence &&
        !OperationalImplicationAuthorizesAction &&
        !EvaluationGrantsWarrant &&
        !BoundedConclusionAdmitsTruth &&
        !RespectBecomesAgreement &&
        !CriticismBecomesContempt &&
        !ProseSmoothingHidesConcern &&
        !PriorDoctrineBecomesInterpretiveAuthority &&
        !ConceptualProximityBecomesEquivalence &&
        !ReviewArchitectureColonizesPaper &&
        !ConversationalDepthBecomesAdvocacy &&
        !BridgeBecomesMemory &&
        !BridgeAdmitsContinuity &&
        !BridgeGrantsAuthority &&
        !BridgeAuthorizesAction &&
        !BridgeEvaluatesLisp &&
        !BridgeEmitsPacket &&
        !BridgeReplaysReceipt &&
        !BridgeIncrementsPassage &&
        !BridgeActivates;
}

public sealed record PeerReviewBridgeBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool RequiresEppsSource,
    bool RequiresLocalDefinition,
    bool RequiresWhyItMatters,
    bool RequiresOperationalImplication,
    bool RequiresEvaluation,
    bool RequiresBoundedConclusion,
    bool RequiresTerminologyQuarantine,
    bool RequiresReaderStateContinuity,
    bool RequiresContextQuarantine,
    bool RequiresReviewStateIsolation,
    bool RequiresConversationalDepth,
    bool RequiresEvidenceStatus,
    bool AllowsAuthorTermAsAuthority,
    bool AllowsDefinitionAsProof,
    bool AllowsConsequenceAsEvidence,
    bool AllowsEvaluationAsWarrant,
    bool AllowsConclusionAsTruth,
    bool AllowsRespectAsAgreement,
    bool AllowsCriticismAsContempt,
    bool AllowsProseSmoothingToHideConcern,
    bool AllowsPriorDoctrineAsInterpretiveAuthority,
    bool AllowsConceptualProximityAsEquivalence,
    bool AllowsReviewArchitectureColonization,
    bool AllowsConversationalDepthAsAdvocacy,
    bool AllowsMemoryAdmission,
    bool AllowsContinuityAdmission,
    bool AllowsAuthority,
    bool AllowsActionAuthorization,
    bool AllowsLispEvaluation,
    bool AllowsPacketEmission,
    bool AllowsReceiptReplay,
    bool AllowsPassageIncrement,
    bool AllowsActivation)
{
    public bool IsColdBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryCode) &&
        Present &&
        ReviewOnly &&
        RequiresEppsSource &&
        RequiresLocalDefinition &&
        RequiresWhyItMatters &&
        RequiresOperationalImplication &&
        RequiresEvaluation &&
        RequiresBoundedConclusion &&
        RequiresTerminologyQuarantine &&
        RequiresReaderStateContinuity &&
        RequiresContextQuarantine &&
        RequiresReviewStateIsolation &&
        RequiresConversationalDepth &&
        RequiresEvidenceStatus &&
        !AllowsAuthorTermAsAuthority &&
        !AllowsDefinitionAsProof &&
        !AllowsConsequenceAsEvidence &&
        !AllowsEvaluationAsWarrant &&
        !AllowsConclusionAsTruth &&
        !AllowsRespectAsAgreement &&
        !AllowsCriticismAsContempt &&
        !AllowsProseSmoothingToHideConcern &&
        !AllowsPriorDoctrineAsInterpretiveAuthority &&
        !AllowsConceptualProximityAsEquivalence &&
        !AllowsReviewArchitectureColonization &&
        !AllowsConversationalDepthAsAdvocacy &&
        !AllowsMemoryAdmission &&
        !AllowsContinuityAdmission &&
        !AllowsAuthority &&
        !AllowsActionAuthorization &&
        !AllowsLispEvaluation &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement &&
        !AllowsActivation;
}

public sealed record PeerReviewBridgeRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record PeerReviewPredicateBridgeRequest(
    EngramPredicatePrecursorStreamReceipt? SourceEppsReceipt,
    IReadOnlyList<PeerReviewBridgeSegment> Segments,
    PeerReviewBridgeBoundary Boundary,
    int PriorPassageCount);

public sealed record PeerReviewPredicateBridgeReceipt(
    string ReceiptHandle,
    PeerReviewPredicateBridgeDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceEppsReceiptHandle,
    IReadOnlyList<PeerReviewBridgeSegment> Segments,
    PeerReviewBridgeBoundary Boundary,
    PeerReviewBridgeRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterBridge,
    bool ReviewOnly,
    bool BridgeSynthesisOnly,
    bool ReaderStateContinuityMapped,
    bool TerminologyQuarantined,
    bool ContextQuarantined,
    bool ReviewStateIsolated,
    bool ConversationalDepthRetained,
    bool AuthorTermBecameAuthority,
    bool DefinitionBecameProof,
    bool ConsequenceBecameEvidence,
    bool EvaluationGrantedWarrant,
    bool ConclusionAdmittedTruth,
    bool RespectBecameAgreement,
    bool CriticismBecameContempt,
    bool ProseSmoothingHidConcern,
    bool PriorDoctrineBecameInterpretiveAuthority,
    bool ConceptualProximityBecameEquivalence,
    bool ReviewArchitectureColonizedPaper,
    bool ConversationalDepthBecameAdvocacy,
    bool MemoryAdmitted,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool ActionAuthorized,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdPeerReviewBridge =>
        (Disposition is PeerReviewPredicateBridgeDisposition.BridgesRetainedCold or
            PeerReviewPredicateBridgeDisposition.EmptyReviewCold) &&
        Refusal is null &&
        !string.IsNullOrWhiteSpace(SourceEppsReceiptHandle) &&
        Segments.All(static segment => segment.IsColdBridgeSegment) &&
        HasDistinctSegmentHandles &&
        Boundary.IsColdBoundary &&
        ReviewOnly &&
        BridgeSynthesisOnly &&
        ReaderStateContinuityMapped &&
        TerminologyQuarantined &&
        ContextQuarantined &&
        ReviewStateIsolated &&
        ConversationalDepthRetained &&
        PassageCountAfterBridge == PriorPassageCount &&
        !AuthorTermBecameAuthority &&
        !DefinitionBecameProof &&
        !ConsequenceBecameEvidence &&
        !EvaluationGrantedWarrant &&
        !ConclusionAdmittedTruth &&
        !RespectBecameAgreement &&
        !CriticismBecameContempt &&
        !ProseSmoothingHidConcern &&
        !PriorDoctrineBecameInterpretiveAuthority &&
        !ConceptualProximityBecameEquivalence &&
        !ReviewArchitectureColonizedPaper &&
        !ConversationalDepthBecameAdvocacy &&
        !MemoryAdmitted &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !ActionAuthorized &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        ActivationRefused;

    public bool IsRetainedPeerReviewBridgeRefusal =>
        Disposition == PeerReviewPredicateBridgeDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterBridge == PriorPassageCount &&
        !AuthorTermBecameAuthority &&
        !DefinitionBecameProof &&
        !ConsequenceBecameEvidence &&
        !EvaluationGrantedWarrant &&
        !ConclusionAdmittedTruth &&
        !RespectBecameAgreement &&
        !CriticismBecameContempt &&
        !ProseSmoothingHidConcern &&
        !PriorDoctrineBecameInterpretiveAuthority &&
        !ConceptualProximityBecameEquivalence &&
        !ReviewArchitectureColonizedPaper &&
        !ConversationalDepthBecameAdvocacy &&
        !MemoryAdmitted &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !ActionAuthorized &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        ActivationRefused;

    private bool HasDistinctSegmentHandles =>
        Segments.Select(static segment => segment.SegmentHandle).Distinct(StringComparer.Ordinal).Count() == Segments.Count;
}

public sealed class DefaultPeerReviewPredicateBridgeBoundaryValidator
{
    public PeerReviewPredicateBridgeReceipt Declare(
        PeerReviewPredicateBridgeRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceEppsReceipt is null || !request.SourceEppsReceipt.IsColdPrecursorStream)
        {
            return Refuse(
                request,
                "peer-review-bridge-source-epps-not-cold",
                "Peer review bridge refused because a cold EPPS source receipt is required before review residue may be translated for reader-state continuity.",
                timestampUtc);
        }

        if (request.Boundary is null || !request.Boundary.Present)
        {
            return Refuse(
                request,
                "peer-review-bridge-boundary-missing",
                "Peer review bridge refused because a review-only bridge boundary is required before prose smoothing may occur.",
                timestampUtc);
        }

        if (!request.Boundary.IsColdBoundary)
        {
            return Refuse(
                request,
                "peer-review-bridge-boundary-promotional",
                "Peer review bridge refused because terminology quarantine, context quarantine, review-state isolation, conversational depth, and reader-state continuity must be required while refusing term-as-authority, definition-as-proof, consequence-as-evidence, evaluation-as-warrant, conclusion-as-truth, respect-as-agreement, criticism-as-contempt, hidden concern, prior-doctrine-as-authority, proximity-as-equivalence, review-architecture colonization, depth-as-advocacy, memory, continuity, authority, action, Lisp evaluation, packet emission, replay, passage, and activation.",
                timestampUtc);
        }

        var residueHandles = request.SourceEppsReceipt.Residues
            .Select(static residue => residue.ResidueHandle)
            .ToHashSet(StringComparer.Ordinal);

        if (request.Segments.Any(static segment => !segment.IsColdBridgeSegment) ||
            request.Segments.Any(segment => !residueHandles.Contains(segment.SourceResidueHandle)))
        {
            return Refuse(
                request,
                "peer-review-bridge-segment-invalid",
                "Peer review bridge refused because every segment must map a known EPPS residue through author term, local definition, importance, implication, evaluation, and bounded conclusion under context quarantine and conversational depth without becoming proof, warrant, agreement, prior-doctrine authority, equivalence, paper colonization, advocacy, memory, continuity, authority, action, Lisp evaluation, packet emission, replay, passage, or activation.",
                timestampUtc);
        }

        if (HasDuplicate(request.Segments.Select(static segment => segment.SegmentHandle)))
        {
            return Refuse(
                request,
                "peer-review-bridge-duplicate-segment-handle",
                "Peer review bridge refused because duplicate bridge segment handles would collapse reader-state lineage.",
                timestampUtc);
        }

        var disposition = request.Segments.Count == 0
            ? PeerReviewPredicateBridgeDisposition.EmptyReviewCold
            : PeerReviewPredicateBridgeDisposition.BridgesRetainedCold;
        var outcomeCode = disposition == PeerReviewPredicateBridgeDisposition.EmptyReviewCold
            ? "peer-review-predicate-bridge-empty-review-only"
            : "peer-review-predicate-bridge-retained-cold";
        var governanceTrace = disposition == PeerReviewPredicateBridgeDisposition.EmptyReviewCold
            ? "Peer review predicate bridge found no segments. Empty review preserves reader-state continuity and context quarantine without agreement, proof, warrant, memory, continuity, action, authority, Lisp evaluation, packet emission, replay, passage, or activation."
            : "Peer review predicate bridge retained reader-facing semantic ladders with conversational depth and context quarantine while refusing prose smoothing as agreement, bridge synthesis as proof, respect as endorsement, criticism as contempt, prior doctrine as interpretive authority, conceptual proximity as equivalence, review architecture as paper colonization, memory, continuity, action, authority, Lisp evaluation, packet emission, replay, passage, and activation.";

        return CreateReceipt(request, disposition, outcomeCode, governanceTrace, refusal: null, timestampUtc);
    }

    private static PeerReviewPredicateBridgeReceipt Refuse(
        PeerReviewPredicateBridgeRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            PeerReviewPredicateBridgeDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new PeerReviewBridgeRefusalReceipt(
                ReceiptHandle: $"urn:san:peer-review-bridge-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static PeerReviewPredicateBridgeReceipt CreateReceipt(
        PeerReviewPredicateBridgeRequest request,
        PeerReviewPredicateBridgeDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        PeerReviewBridgeRefusalReceipt? refusal,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:peer-review-bridge:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, request.Segments.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceEppsReceiptHandle: request.SourceEppsReceipt?.ReceiptHandle ?? "missing-epps-source",
            Segments: refusal is null ? request.Segments.ToArray() : [],
            Boundary: request.Boundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterBridge: request.PriorPassageCount,
            ReviewOnly: true,
            BridgeSynthesisOnly: refusal is null,
            ReaderStateContinuityMapped: refusal is null,
            TerminologyQuarantined: refusal is null,
            ContextQuarantined: refusal is null,
            ReviewStateIsolated: refusal is null,
            ConversationalDepthRetained: refusal is null,
            AuthorTermBecameAuthority: false,
            DefinitionBecameProof: false,
            ConsequenceBecameEvidence: false,
            EvaluationGrantedWarrant: false,
            ConclusionAdmittedTruth: false,
            RespectBecameAgreement: false,
            CriticismBecameContempt: false,
            ProseSmoothingHidConcern: false,
            PriorDoctrineBecameInterpretiveAuthority: false,
            ConceptualProximityBecameEquivalence: false,
            ReviewArchitectureColonizedPaper: false,
            ConversationalDepthBecameAdvocacy: false,
            MemoryAdmitted: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActionAuthorized: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);

    private static string SourceHandle(PeerReviewPredicateBridgeRequest request) =>
        request.SourceEppsReceipt?.ReceiptHandle ?? "missing-epps-source";

    private static bool HasDuplicate(IEnumerable<string> handles)
    {
        var seen = new HashSet<string>(StringComparer.Ordinal);
        foreach (var handle in handles)
        {
            if (!seen.Add(handle))
            {
                return true;
            }
        }

        return false;
    }

    private static string ShortHash(params string[] parts)
    {
        var payload = string.Join("|", parts);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(bytes)[..16].ToLowerInvariant();
    }
}
