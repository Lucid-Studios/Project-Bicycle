using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum TypedActionFormationDisposition
{
    DeclaredForReviewCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public enum ActionFormationOrigin
{
    OperatorInstruction = 0,
    CompassShell = 1,
    ReceiptQuery = 2,
    ArtifactReplay = 3,
    MemoryResidue = 4,
    ToolResult = 5,
    PublicWitnessPressure = 6,
    DesignInference = 7
}

public sealed record TypedActionSurfaceDeclaration(
    string ActionHandle,
    string SourceSurface,
    string TargetSurface,
    string DeclaredIntent,
    string MethodCode,
    string AuthorityCeiling,
    string CustodyOwner,
    string WitnessBurden,
    string TelemetryRoute,
    string AdmissibilityPredicate,
    string RevocationPath,
    string LossCondition,
    bool ReviewOnly,
    bool CandidateOnly,
    bool RuntimeEffectRequested,
    bool ContinuityEffectRequested,
    bool AttemptsSelfAuthorization)
{
    public bool IsColdActionDeclaration =>
        !string.IsNullOrWhiteSpace(ActionHandle) &&
        !string.IsNullOrWhiteSpace(SourceSurface) &&
        !string.IsNullOrWhiteSpace(TargetSurface) &&
        !string.IsNullOrWhiteSpace(DeclaredIntent) &&
        !string.IsNullOrWhiteSpace(MethodCode) &&
        !string.IsNullOrWhiteSpace(AuthorityCeiling) &&
        !string.IsNullOrWhiteSpace(CustodyOwner) &&
        !string.IsNullOrWhiteSpace(WitnessBurden) &&
        !string.IsNullOrWhiteSpace(TelemetryRoute) &&
        !string.IsNullOrWhiteSpace(AdmissibilityPredicate) &&
        !string.IsNullOrWhiteSpace(RevocationPath) &&
        !string.IsNullOrWhiteSpace(LossCondition) &&
        ReviewOnly &&
        CandidateOnly &&
        !RuntimeEffectRequested &&
        !ContinuityEffectRequested &&
        !AttemptsSelfAuthorization;
}

public sealed record MethodologicalFormationAnalysis(
    string FormationHandle,
    string ActionHandle,
    ActionFormationOrigin Origin,
    string SourceEvidenceHandle,
    string FormationTrace,
    string PressureClass,
    bool EvidenceBodyPresent,
    bool WitnessBodyPresent,
    bool ExplainsCandidate,
    bool AuthorizesCandidate,
    bool EmitsPacket,
    bool ReplaysReceipt,
    bool IncrementsPassage)
{
    public bool IsColdFormationAnalysis =>
        !string.IsNullOrWhiteSpace(FormationHandle) &&
        !string.IsNullOrWhiteSpace(ActionHandle) &&
        !string.IsNullOrWhiteSpace(SourceEvidenceHandle) &&
        !string.IsNullOrWhiteSpace(FormationTrace) &&
        !string.IsNullOrWhiteSpace(PressureClass) &&
        EvidenceBodyPresent &&
        WitnessBodyPresent &&
        ExplainsCandidate &&
        !AuthorizesCandidate &&
        !EmitsPacket &&
        !ReplaysReceipt &&
        !IncrementsPassage;
}

public sealed record DesignPredicateDeclaration(
    string PredicateHandle,
    string ActionHandle,
    string PredicateCode,
    string RequiresTerm,
    bool RequiredTermPresent,
    bool ReviewOnly,
    bool MayExecuteItself,
    bool MayAuthorizeAction,
    bool MayAdmitContinuity,
    bool MayActivateRuntime,
    bool MayEvaluateLisp)
{
    public bool IsColdDesignPredicate =>
        !string.IsNullOrWhiteSpace(PredicateHandle) &&
        !string.IsNullOrWhiteSpace(ActionHandle) &&
        !string.IsNullOrWhiteSpace(PredicateCode) &&
        !string.IsNullOrWhiteSpace(RequiresTerm) &&
        RequiredTermPresent &&
        ReviewOnly &&
        !MayExecuteItself &&
        !MayAuthorizeAction &&
        !MayAdmitContinuity &&
        !MayActivateRuntime &&
        !MayEvaluateLisp;
}

public sealed record TypedActionFormationScopeBoundary(
    string ScopeCode,
    bool Present,
    bool ReviewOnly,
    bool AllowsRuntimeAction,
    bool AllowsContinuityEffect,
    bool AllowsAuthority,
    bool AllowsActivation,
    bool AllowsLispEvaluation,
    bool AllowsPacketEmission,
    bool AllowsReceiptReplay,
    bool AllowsPassageIncrement)
{
    public bool IsColdScope =>
        !string.IsNullOrWhiteSpace(ScopeCode) &&
        Present &&
        ReviewOnly &&
        !AllowsRuntimeAction &&
        !AllowsContinuityEffect &&
        !AllowsAuthority &&
        !AllowsActivation &&
        !AllowsLispEvaluation &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement;
}

public sealed record TypedActionFormationNonExecutionBoundary(
    bool DeclaredActionMayExecute,
    bool FormationAnalysisMayAuthorize,
    bool DesignPredicateMayExecute,
    bool DesignPredicateMayAuthorize,
    bool SummaryMayBecomeAction,
    bool ReceiptMayBecomeAction,
    bool ReplayMayBecomeAction,
    bool QueryMayBecomeAction,
    bool EmitsPacket,
    bool IncrementsPassageCount,
    bool AllowsContinuity,
    bool AllowsAuthority,
    string BoundaryLaw);

public sealed record TypedActionFormationRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record TypedActionFormationRequest(
    HarmonicInterlockModulationCorrespondenceReceipt? SourceCorrespondenceReceipt,
    IReadOnlyList<TypedActionSurfaceDeclaration> ActionDeclarations,
    IReadOnlyList<MethodologicalFormationAnalysis> FormationAnalyses,
    IReadOnlyList<DesignPredicateDeclaration> DesignPredicates,
    TypedActionFormationScopeBoundary ScopeBoundary,
    int PriorPassageCount);

public sealed record TypedActionFormationReceipt(
    string ReceiptHandle,
    TypedActionFormationDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceCorrespondenceReceiptHandle,
    IReadOnlyList<TypedActionSurfaceDeclaration> ActionDeclarations,
    IReadOnlyList<MethodologicalFormationAnalysis> FormationAnalyses,
    IReadOnlyList<DesignPredicateDeclaration> DesignPredicates,
    TypedActionFormationScopeBoundary ScopeBoundary,
    TypedActionFormationNonExecutionBoundary NonExecutionBoundary,
    TypedActionFormationRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterTypedActionReview,
    bool ReviewOnly,
    bool CandidateOnly,
    bool DeclaredActionExecutes,
    bool FormationAnalysisAuthorizes,
    bool DesignPredicateExecutes,
    bool DesignPredicateAuthorizes,
    bool SummaryBecomesAction,
    bool ReceiptBecomesAction,
    bool ReplayBecomesAction,
    bool QueryBecomesAction,
    bool RuntimeActionAllowed,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdTypedActionFormation =>
        (Disposition is TypedActionFormationDisposition.DeclaredForReviewCold or
            TypedActionFormationDisposition.EmptyReviewCold) &&
        Refusal is null &&
        ReviewOnly &&
        CandidateOnly &&
        PassageCountAfterTypedActionReview == PriorPassageCount &&
        !DeclaredActionExecutes &&
        !FormationAnalysisAuthorizes &&
        !DesignPredicateExecutes &&
        !DesignPredicateAuthorizes &&
        !SummaryBecomesAction &&
        !ReceiptBecomesAction &&
        !ReplayBecomesAction &&
        !QueryBecomesAction &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused &&
        NonExecutionBoundary is
        {
            DeclaredActionMayExecute: false,
            FormationAnalysisMayAuthorize: false,
            DesignPredicateMayExecute: false,
            DesignPredicateMayAuthorize: false,
            SummaryMayBecomeAction: false,
            ReceiptMayBecomeAction: false,
            ReplayMayBecomeAction: false,
            QueryMayBecomeAction: false,
            EmitsPacket: false,
            IncrementsPassageCount: false,
            AllowsContinuity: false,
            AllowsAuthority: false
        };

    public bool IsRetainedTypedActionFormationRefusal =>
        Disposition == TypedActionFormationDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterTypedActionReview == PriorPassageCount &&
        !DeclaredActionExecutes &&
        !FormationAnalysisAuthorizes &&
        !DesignPredicateExecutes &&
        !DesignPredicateAuthorizes &&
        !SummaryBecomesAction &&
        !ReceiptBecomesAction &&
        !ReplayBecomesAction &&
        !QueryBecomesAction &&
        !RuntimeActionAllowed &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        ActivationRefused;
}

public sealed class DefaultTypedActionFormationBoundaryValidator
{
    private static readonly TypedActionFormationNonExecutionBoundary NonExecutionBoundary = new(
        DeclaredActionMayExecute: false,
        FormationAnalysisMayAuthorize: false,
        DesignPredicateMayExecute: false,
        DesignPredicateMayAuthorize: false,
        SummaryMayBecomeAction: false,
        ReceiptMayBecomeAction: false,
        ReplayMayBecomeAction: false,
        QueryMayBecomeAction: false,
        EmitsPacket: false,
        IncrementsPassageCount: false,
        AllowsContinuity: false,
        AllowsAuthority: false,
        BoundaryLaw: "Typed action may be declared for review. Formation analysis and design predicates may constrain it. None may execute, authorize, or admit continuity.");

    public TypedActionFormationReceipt Declare(
        TypedActionFormationRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceCorrespondenceReceipt is null ||
            !request.SourceCorrespondenceReceipt.IsColdCorrespondenceAtlas)
        {
            return Refuse(
                request,
                "typed-action-source-correspondence-missing",
                "Typed action formation refused because a cold modulation correspondence receipt is required before action-surface declaration.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.IsColdScope)
        {
            return Refuse(
                request,
                "typed-action-scope-boundary-promotional",
                "Typed action formation refused because scope must be review-only and cannot allow runtime action, continuity effect, authority, activation, Lisp evaluation, packet emission, replay, or passage increment.",
                timestampUtc);
        }

        if (request.ActionDeclarations.Any(static action => !action.IsColdActionDeclaration))
        {
            return Refuse(
                request,
                "typed-action-declaration-invalid",
                "Typed action declaration refused because every action candidate must declare source, target, intent, method, authority ceiling, custody, witness burden, telemetry route, admissibility, revocation, and loss condition without runtime effect, continuity effect, or self-authorization.",
                timestampUtc);
        }

        var actionHandles = request.ActionDeclarations
            .Select(static action => action.ActionHandle)
            .ToHashSet(StringComparer.Ordinal);

        if (actionHandles.Count != request.ActionDeclarations.Count)
        {
            return Refuse(
                request,
                "typed-action-duplicate-action-handle",
                "Typed action formation refused because duplicate action handles would collapse candidate lineage.",
                timestampUtc);
        }

        if (request.FormationAnalyses.Any(analysis =>
                !analysis.IsColdFormationAnalysis ||
                !actionHandles.Contains(analysis.ActionHandle)))
        {
            return Refuse(
                request,
                "typed-action-formation-analysis-invalid",
                "Methodological formation analysis refused because analysis may explain only declared action candidates and may not authorize, emit packets, replay receipts, or increment passage.",
                timestampUtc);
        }

        if (request.DesignPredicates.Any(predicate =>
                !predicate.IsColdDesignPredicate ||
                !actionHandles.Contains(predicate.ActionHandle)))
        {
            return Refuse(
                request,
                "typed-action-design-predicate-invalid",
                "Design predicate refused because predicates may constrain only declared action candidates and may not execute themselves, authorize action, admit continuity, activate runtime, or evaluate Lisp.",
                timestampUtc);
        }

        if (request.ActionDeclarations.Count > 0 &&
            request.ActionDeclarations.Any(action =>
                !request.FormationAnalyses.Any(analysis => analysis.ActionHandle == action.ActionHandle) ||
                !request.DesignPredicates.Any(predicate => predicate.ActionHandle == action.ActionHandle)))
        {
            return Refuse(
                request,
                "typed-action-missing-formation-or-predicate",
                "Typed action formation refused because every declared action candidate requires both formation analysis and design predicate coverage.",
                timestampUtc);
        }

        var disposition = request.ActionDeclarations.Count == 0
            ? TypedActionFormationDisposition.EmptyReviewCold
            : TypedActionFormationDisposition.DeclaredForReviewCold;
        var outcomeCode = disposition == TypedActionFormationDisposition.EmptyReviewCold
            ? "typed-action-formation-empty-review-only"
            : "typed-action-formation-declared-review-only";

        return CreateReceipt(
            request,
            disposition,
            outcomeCode,
            "Typed action, methodological formation analysis, and design predicates were declared as review-only candidate structure. Declaration does not execute, authorize, admit continuity, evaluate Lisp, emit packets, replay receipts, or activate.",
            refusal: null,
            timestampUtc);
    }

    private static TypedActionFormationReceipt Refuse(
        TypedActionFormationRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            TypedActionFormationDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new TypedActionFormationRefusalReceipt(
                ReceiptHandle: $"urn:san:typed-action-formation-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static TypedActionFormationReceipt CreateReceipt(
        TypedActionFormationRequest request,
        TypedActionFormationDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        TypedActionFormationRefusalReceipt? refusal,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:typed-action-formation:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, request.ActionDeclarations.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceCorrespondenceReceiptHandle: SourceHandle(request),
            ActionDeclarations: refusal is null ? request.ActionDeclarations.ToArray() : [],
            FormationAnalyses: refusal is null ? request.FormationAnalyses.ToArray() : [],
            DesignPredicates: refusal is null ? request.DesignPredicates.ToArray() : [],
            ScopeBoundary: request.ScopeBoundary,
            NonExecutionBoundary: NonExecutionBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterTypedActionReview: request.PriorPassageCount,
            ReviewOnly: true,
            CandidateOnly: true,
            DeclaredActionExecutes: false,
            FormationAnalysisAuthorizes: false,
            DesignPredicateExecutes: false,
            DesignPredicateAuthorizes: false,
            SummaryBecomesAction: false,
            ReceiptBecomesAction: false,
            ReplayBecomesAction: false,
            QueryBecomesAction: false,
            RuntimeActionAllowed: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);

    private static string SourceHandle(TypedActionFormationRequest request) =>
        request.SourceCorrespondenceReceipt?.ReceiptHandle ?? "missing-modulation-correspondence-source";

    private static string ShortHash(params string[] parts)
    {
        var payload = string.Join("|", parts);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(bytes)[..16].ToLowerInvariant();
    }
}
