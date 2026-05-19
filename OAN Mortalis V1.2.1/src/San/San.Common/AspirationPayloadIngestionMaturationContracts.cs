using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum AspirationPayloadIngestionMaturationDisposition
{
    MaturedForReviewCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public enum AspirationPayloadLaneKind
{
    PrimeBody = 0,
    CrypticMind = 1,
    StewardWitness = 2,
    SliLisp = 3,
    EngineeredCognition = 4,
    Pedagogy = 5,
    Telemetry = 6,
    OperatorIntent = 7
}

public sealed record AspirationPayloadStatement(
    string StatementHandle,
    string SourceWaveCascadeHandle,
    AspirationPayloadLaneKind LaneKind,
    string SourceSurface,
    string StatementText,
    string EvidenceHandle,
    string WitnessHandle,
    string CoolingPathHandle,
    string ReturnPathHandle,
    bool ReviewOnly,
    bool EvidenceBodyPresent,
    bool WitnessBodyPresent,
    bool CoolingPathPresent,
    bool ReturnPathPresent,
    bool IngestionAllowed,
    bool ArticulationAllowed,
    bool MaturationAllowed,
    bool TreatsAspirationAsWarrant,
    bool TreatsPayloadDensityAsTruth,
    bool TreatsIngestionAsAdmission,
    bool TreatsArticulationAsAuthority,
    bool TreatsMaturationAsContinuity,
    bool AuthorizesAction,
    bool MutatesIdentity,
    bool EvaluatesLisp)
{
    public bool IsColdAspirationStatement =>
        !string.IsNullOrWhiteSpace(StatementHandle) &&
        !string.IsNullOrWhiteSpace(SourceWaveCascadeHandle) &&
        !string.IsNullOrWhiteSpace(SourceSurface) &&
        !string.IsNullOrWhiteSpace(StatementText) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(CoolingPathHandle) &&
        !string.IsNullOrWhiteSpace(ReturnPathHandle) &&
        ReviewOnly &&
        EvidenceBodyPresent &&
        WitnessBodyPresent &&
        CoolingPathPresent &&
        ReturnPathPresent &&
        IngestionAllowed &&
        ArticulationAllowed &&
        MaturationAllowed &&
        !TreatsAspirationAsWarrant &&
        !TreatsPayloadDensityAsTruth &&
        !TreatsIngestionAsAdmission &&
        !TreatsArticulationAsAuthority &&
        !TreatsMaturationAsContinuity &&
        !AuthorizesAction &&
        !MutatesIdentity &&
        !EvaluatesLisp;
}

public sealed record AspirationPayloadIngestionLane(
    string LaneHandle,
    string SourceStatementHandle,
    AspirationPayloadLaneKind LaneKind,
    string TargetBodySurface,
    string PayloadClass,
    bool ReviewOnly,
    bool IngestedForReview,
    bool PreservesSourceLineage,
    bool RequiresEvidence,
    bool RequiresWitness,
    bool RequiresCooling,
    bool RequiresReturnPath,
    bool AllowsAdmission,
    bool AllowsAuthority,
    bool AllowsContinuity,
    bool AllowsAction,
    bool AllowsLispEvaluation)
{
    public bool IsColdIngestionLane =>
        !string.IsNullOrWhiteSpace(LaneHandle) &&
        !string.IsNullOrWhiteSpace(SourceStatementHandle) &&
        !string.IsNullOrWhiteSpace(TargetBodySurface) &&
        !string.IsNullOrWhiteSpace(PayloadClass) &&
        ReviewOnly &&
        IngestedForReview &&
        PreservesSourceLineage &&
        RequiresEvidence &&
        RequiresWitness &&
        RequiresCooling &&
        RequiresReturnPath &&
        !AllowsAdmission &&
        !AllowsAuthority &&
        !AllowsContinuity &&
        !AllowsAction &&
        !AllowsLispEvaluation;
}

public sealed record AspirationMaturationCandidate(
    string CandidateHandle,
    string SourceStatementHandle,
    string LaneHandle,
    string ArticulatedForm,
    string MaturationPosture,
    bool ReviewOnly,
    bool ArticulatedForReview,
    bool MaturedAsCandidate,
    bool CandidateOnly,
    bool PreservesPayloadLineage,
    bool RequiresStewardReview,
    bool RequiresReturnPath,
    bool ArticulationBecomesAuthority,
    bool MaturationBecomesContinuity,
    bool CandidateBecomesWarrant,
    bool CandidateAuthorizesAction,
    bool CandidateEvaluatesLisp,
    bool CandidateActivates)
{
    public bool IsColdMaturationCandidate =>
        !string.IsNullOrWhiteSpace(CandidateHandle) &&
        !string.IsNullOrWhiteSpace(SourceStatementHandle) &&
        !string.IsNullOrWhiteSpace(LaneHandle) &&
        !string.IsNullOrWhiteSpace(ArticulatedForm) &&
        !string.IsNullOrWhiteSpace(MaturationPosture) &&
        ReviewOnly &&
        ArticulatedForReview &&
        MaturedAsCandidate &&
        CandidateOnly &&
        PreservesPayloadLineage &&
        RequiresStewardReview &&
        RequiresReturnPath &&
        !ArticulationBecomesAuthority &&
        !MaturationBecomesContinuity &&
        !CandidateBecomesWarrant &&
        !CandidateAuthorizesAction &&
        !CandidateEvaluatesLisp &&
        !CandidateActivates;
}

public sealed record AspirationPayloadIngestionMaturationBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool AllowsPayloadLoad,
    bool AllowsIngestion,
    bool AllowsArticulation,
    bool AllowsMaturation,
    bool RequiresTypedLanes,
    bool RequiresEvidence,
    bool RequiresWitness,
    bool RequiresCooling,
    bool RequiresReturnPath,
    bool RequiresStewardReview,
    bool AllowsPayloadAsWarrant,
    bool AllowsPayloadDensityAsTruth,
    bool AllowsIngestionAsAdmission,
    bool AllowsArticulationAsAuthority,
    bool AllowsMaturationAsContinuity,
    bool AllowsRuntimeAction,
    bool AllowsIdentityMutation,
    bool AllowsLispEvaluation,
    bool AllowsPacketEmission,
    bool AllowsReceiptReplay,
    bool IncrementsPassageCount,
    bool AllowsActivation)
{
    public bool IsColdBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryCode) &&
        Present &&
        ReviewOnly &&
        AllowsPayloadLoad &&
        AllowsIngestion &&
        AllowsArticulation &&
        AllowsMaturation &&
        RequiresTypedLanes &&
        RequiresEvidence &&
        RequiresWitness &&
        RequiresCooling &&
        RequiresReturnPath &&
        RequiresStewardReview &&
        !AllowsPayloadAsWarrant &&
        !AllowsPayloadDensityAsTruth &&
        !AllowsIngestionAsAdmission &&
        !AllowsArticulationAsAuthority &&
        !AllowsMaturationAsContinuity &&
        !AllowsRuntimeAction &&
        !AllowsIdentityMutation &&
        !AllowsLispEvaluation &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !IncrementsPassageCount &&
        !AllowsActivation;
}

public sealed record AspirationPayloadNonPromotionBoundary(
    string BoundaryLaw,
    bool AspirationMayBecomeWarrant,
    bool PayloadDensityMayBecomeTruth,
    bool IngestionMayBecomeAdmission,
    bool ArticulationMayBecomeAuthority,
    bool MaturationMayAdmitContinuity,
    bool CandidateMayAuthorizeAction,
    bool CandidateMayEvaluateLisp,
    bool CandidateMayEmitPacket,
    bool CandidateMayReplayReceipts,
    bool CandidateMayIncrementPassage,
    bool CandidateMayActivate)
{
    public bool IsColdNonPromotionBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryLaw) &&
        !AspirationMayBecomeWarrant &&
        !PayloadDensityMayBecomeTruth &&
        !IngestionMayBecomeAdmission &&
        !ArticulationMayBecomeAuthority &&
        !MaturationMayAdmitContinuity &&
        !CandidateMayAuthorizeAction &&
        !CandidateMayEvaluateLisp &&
        !CandidateMayEmitPacket &&
        !CandidateMayReplayReceipts &&
        !CandidateMayIncrementPassage &&
        !CandidateMayActivate;
}

public sealed record AspirationPayloadRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record AspirationPayloadIngestionMaturationRequest(
    IReadOnlyList<AspirationPayloadStatement> Statements,
    IReadOnlyList<AspirationPayloadIngestionLane> IngestionLanes,
    IReadOnlyList<AspirationMaturationCandidate> MaturationCandidates,
    AspirationPayloadIngestionMaturationBoundary Boundary,
    AspirationPayloadNonPromotionBoundary NonPromotionBoundary,
    int PriorPassageCount);

public sealed record AspirationPayloadIngestionMaturationReceipt(
    string ReceiptHandle,
    AspirationPayloadIngestionMaturationDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    IReadOnlyList<AspirationPayloadStatement> Statements,
    IReadOnlyList<AspirationPayloadIngestionLane> IngestionLanes,
    IReadOnlyList<AspirationMaturationCandidate> MaturationCandidates,
    AspirationPayloadIngestionMaturationBoundary Boundary,
    AspirationPayloadNonPromotionBoundary NonPromotionBoundary,
    AspirationPayloadRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterMaturation,
    int RetainedPayloadCount,
    bool ReviewOnly,
    bool PayloadLoadedAsColdEvidence,
    bool PayloadBecameWarrant,
    bool PayloadDensityBecameTruth,
    bool IngestionBecameAdmission,
    bool ArticulationBecameAuthority,
    bool MaturationAdmittedContinuity,
    bool CandidateAuthorizedAction,
    bool IdentityMutated,
    bool RuntimeActionAllowed,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdAspirationMaturation =>
        (Disposition is AspirationPayloadIngestionMaturationDisposition.MaturedForReviewCold or
            AspirationPayloadIngestionMaturationDisposition.EmptyReviewCold) &&
        Refusal is null &&
        ReviewOnly &&
        PassageCountAfterMaturation == PriorPassageCount &&
        RetainedPayloadCount == Statements.Count &&
        !PayloadBecameWarrant &&
        !PayloadDensityBecameTruth &&
        !IngestionBecameAdmission &&
        !ArticulationBecameAuthority &&
        !MaturationAdmittedContinuity &&
        !CandidateAuthorizedAction &&
        !IdentityMutated &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused &&
        Boundary.IsColdBoundary &&
        NonPromotionBoundary.IsColdNonPromotionBoundary;

    public bool IsRetainedAspirationRefusal =>
        Disposition == AspirationPayloadIngestionMaturationDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterMaturation == PriorPassageCount &&
        RetainedPayloadCount == 0 &&
        !PayloadLoadedAsColdEvidence &&
        !PayloadBecameWarrant &&
        !PayloadDensityBecameTruth &&
        !IngestionBecameAdmission &&
        !ArticulationBecameAuthority &&
        !MaturationAdmittedContinuity &&
        !CandidateAuthorizedAction &&
        !IdentityMutated &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused;
}

public sealed class DefaultAspirationPayloadIngestionMaturationBoundaryValidator
{
    public AspirationPayloadIngestionMaturationReceipt Mature(
        AspirationPayloadIngestionMaturationRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Boundary is null ||
            !request.Boundary.Present ||
            string.IsNullOrWhiteSpace(request.Boundary.BoundaryCode))
        {
            return Refuse(
                request,
                "aspiration-payload-boundary-missing",
                "Aspiration payload maturation refused because a review-only payload, ingestion, articulation, and maturation boundary is required.",
                timestampUtc);
        }

        if (!request.Boundary.IsColdBoundary)
        {
            return Refuse(
                request,
                "aspiration-payload-promotional-boundary",
                "Aspiration payload maturation refused because the boundary must allow cold loading, ingestion, articulation, and maturation while requiring typed lanes, evidence, witness, cooling, return path, Steward review, and while refusing warrant, truth, admission, authority, continuity, action, identity mutation, Lisp evaluation, packet emission, replay, passage, and activation.",
                timestampUtc);
        }

        if (request.NonPromotionBoundary is null ||
            !request.NonPromotionBoundary.IsColdNonPromotionBoundary)
        {
            return Refuse(
                request,
                "aspiration-payload-non-promotion-boundary-invalid",
                "Aspiration payload maturation refused because non-promotion law must prevent aspiration, payload density, ingestion, articulation, maturation, candidates, Lisp evaluation, packets, replay, passage, and activation from promoting themselves.",
                timestampUtc);
        }

        if (request.Statements.Any(static statement => !statement.IsColdAspirationStatement))
        {
            return Refuse(
                request,
                "aspiration-payload-statement-invalid",
                "Aspiration payload maturation refused because every aspiration statement must remain evidence-backed, witnessed, cooled, returned, review-only, and unable to become warrant, truth, admission, authority, continuity, identity mutation, action, or Lisp evaluation.",
                timestampUtc);
        }

        if (HasDuplicate(request.Statements.Select(static statement => statement.StatementHandle)))
        {
            return Refuse(
                request,
                "aspiration-payload-duplicate-statement-handle",
                "Aspiration payload maturation refused because duplicate statement handles would collapse payload lineage.",
                timestampUtc);
        }

        var statementHandles = request.Statements
            .Select(static statement => statement.StatementHandle)
            .ToHashSet(StringComparer.Ordinal);
        if (request.IngestionLanes.Any(static lane => !lane.IsColdIngestionLane))
        {
            return Refuse(
                request,
                "aspiration-payload-ingestion-lane-invalid",
                "Aspiration payload maturation refused because every ingestion lane must preserve source lineage and remain review-only without admission, authority, continuity, action, or Lisp evaluation.",
                timestampUtc);
        }

        if (HasDuplicate(request.IngestionLanes.Select(static lane => lane.LaneHandle)))
        {
            return Refuse(
                request,
                "aspiration-payload-duplicate-lane-handle",
                "Aspiration payload maturation refused because duplicate lane handles would collapse ingestion lineage.",
                timestampUtc);
        }

        if (request.IngestionLanes.Any(lane => !statementHandles.Contains(lane.SourceStatementHandle)))
        {
            return Refuse(
                request,
                "aspiration-payload-ingestion-lane-unbound",
                "Aspiration payload maturation refused because every ingestion lane must cite a known aspiration statement.",
                timestampUtc);
        }

        var laneByHandle = request.IngestionLanes
            .ToDictionary(static lane => lane.LaneHandle, StringComparer.Ordinal);
        if (request.MaturationCandidates.Any(static candidate => !candidate.IsColdMaturationCandidate))
        {
            return Refuse(
                request,
                "aspiration-payload-maturation-candidate-invalid",
                "Aspiration payload maturation refused because every candidate must remain articulated, candidate-only, steward-reviewable, returned, and unable to become warrant, authority, continuity, action, Lisp evaluation, or activation.",
                timestampUtc);
        }

        if (HasDuplicate(request.MaturationCandidates.Select(static candidate => candidate.CandidateHandle)))
        {
            return Refuse(
                request,
                "aspiration-payload-duplicate-candidate-handle",
                "Aspiration payload maturation refused because duplicate candidate handles would collapse maturation lineage.",
                timestampUtc);
        }

        if (request.MaturationCandidates.Any(candidate =>
                !statementHandles.Contains(candidate.SourceStatementHandle) ||
                !laneByHandle.ContainsKey(candidate.LaneHandle)))
        {
            return Refuse(
                request,
                "aspiration-payload-maturation-candidate-unbound",
                "Aspiration payload maturation refused because every maturation candidate must cite a known aspiration statement and ingestion lane.",
                timestampUtc);
        }

        if (request.MaturationCandidates.Any(candidate =>
                laneByHandle.TryGetValue(candidate.LaneHandle, out var lane) &&
                !string.Equals(lane.SourceStatementHandle, candidate.SourceStatementHandle, StringComparison.Ordinal)))
        {
            return Refuse(
                request,
                "aspiration-payload-maturation-candidate-misaligned",
                "Aspiration payload maturation refused because a candidate may not cross-bind one statement through another statement's ingestion lane.",
                timestampUtc);
        }

        if (request.Statements.Count > 0)
        {
            var lanedStatementHandles = request.IngestionLanes
                .Select(static lane => lane.SourceStatementHandle)
                .ToHashSet(StringComparer.Ordinal);
            if (!statementHandles.All(lanedStatementHandles.Contains))
            {
                return Refuse(
                    request,
                    "aspiration-payload-statement-unlaned",
                    "Aspiration payload maturation refused because every non-empty aspiration statement must enter at least one typed ingestion lane.",
                    timestampUtc);
            }

            var candidateLaneHandles = request.MaturationCandidates
                .Select(static candidate => candidate.LaneHandle)
                .ToHashSet(StringComparer.Ordinal);
            if (!request.IngestionLanes.All(lane => candidateLaneHandles.Contains(lane.LaneHandle)))
            {
                return Refuse(
                    request,
                    "aspiration-payload-lane-without-candidate",
                    "Aspiration payload maturation refused because every non-empty ingestion lane must resolve into a review-only maturation candidate.",
                    timestampUtc);
            }
        }

        var disposition = request.Statements.Count == 0 &&
            request.IngestionLanes.Count == 0 &&
            request.MaturationCandidates.Count == 0
            ? AspirationPayloadIngestionMaturationDisposition.EmptyReviewCold
            : AspirationPayloadIngestionMaturationDisposition.MaturedForReviewCold;
        var outcomeCode = disposition == AspirationPayloadIngestionMaturationDisposition.EmptyReviewCold
            ? "aspiration-payload-empty-review-only"
            : "aspiration-payload-matured-for-review-cold";
        var governanceTrace = disposition == AspirationPayloadIngestionMaturationDisposition.EmptyReviewCold
            ? "Aspiration payload found no statements. Empty review preserves the full-stack boundary without warrant, truth, admission, authority, continuity, action, Lisp evaluation, packet emission, replay, passage, or activation."
            : "Aspiration payload loaded, ingested into typed lanes, articulated, and matured as review-only candidates while refusing aspiration-as-warrant, payload-density-as-truth, ingestion-as-admission, articulation-as-authority, maturation-as-continuity, action, Lisp evaluation, packet emission, replay, passage, and activation.";

        return CreateReceipt(
            request,
            disposition,
            outcomeCode,
            governanceTrace,
            refusal: null,
            timestampUtc);
    }

    private static AspirationPayloadIngestionMaturationReceipt Refuse(
        AspirationPayloadIngestionMaturationRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            AspirationPayloadIngestionMaturationDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new AspirationPayloadRefusalReceipt(
                ReceiptHandle: $"urn:san:aspiration-payload-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static AspirationPayloadIngestionMaturationReceipt CreateReceipt(
        AspirationPayloadIngestionMaturationRequest request,
        AspirationPayloadIngestionMaturationDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        AspirationPayloadRefusalReceipt? refusal,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:aspiration-payload:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, request.Statements.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            Statements: refusal is null ? request.Statements.ToArray() : [],
            IngestionLanes: refusal is null ? request.IngestionLanes.ToArray() : [],
            MaturationCandidates: refusal is null ? request.MaturationCandidates.ToArray() : [],
            Boundary: request.Boundary,
            NonPromotionBoundary: request.NonPromotionBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterMaturation: request.PriorPassageCount,
            RetainedPayloadCount: refusal is null ? request.Statements.Count : 0,
            ReviewOnly: true,
            PayloadLoadedAsColdEvidence: refusal is null &&
                disposition == AspirationPayloadIngestionMaturationDisposition.MaturedForReviewCold,
            PayloadBecameWarrant: false,
            PayloadDensityBecameTruth: false,
            IngestionBecameAdmission: false,
            ArticulationBecameAuthority: false,
            MaturationAdmittedContinuity: false,
            CandidateAuthorizedAction: false,
            IdentityMutated: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);

    private static string SourceHandle(AspirationPayloadIngestionMaturationRequest request) =>
        request.Statements.Count == 0
            ? "aspiration-payload-empty-source"
            : string.Join(",", request.Statements.Take(3).Select(static statement => statement.StatementHandle));

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
