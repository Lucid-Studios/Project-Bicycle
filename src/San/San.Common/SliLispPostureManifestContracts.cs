using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum SliLispPostureManifestDisposition
{
    EmptyReviewCold = 0,
    DeclaredForReviewCold = 1,
    Refused = 2
}

public enum SliLispPostureManifestCarrierKind
{
    InertMembranePolicy = 0,
    LispSourceModule = 1,
    CSharpHostContract = 2,
    FieldQueryPolicy = 3,
    ReceiptContinuityReference = 4,
    NonActivationReadiness = 5,
    TranslationBoundary = 6,
    AgentBodyCmeContract = 7
}

public sealed record SliLispPostureManifestScopeBoundary(
    string ScopeCode,
    bool Present,
    bool ReviewOnly,
    bool InertOnly,
    bool AllowsLispEvaluation,
    bool AllowsLispLoad,
    bool AllowsLispCompilation,
    bool AllowsMacroExpansion,
    bool AllowsRuntimeAction,
    bool AllowsModelBinding,
    bool AllowsDatabaseWrite,
    bool AllowsMorphologyPromotion,
    bool AllowsGelPromotion,
    bool AllowsCmeActual,
    bool AllowsSanctuaryActual,
    bool AllowsContinuityAdmission,
    bool AllowsAuthority,
    bool AllowsActivation,
    bool AllowsPacketEmission,
    bool AllowsReceiptReplay,
    bool IncrementsPassageCount);

public sealed record SliLispPostureManifestCarrierAuthorityBoundary(
    bool LispEvaluationRequested,
    bool LispLoadRequested,
    bool LispCompileRequested,
    bool MacroExpansionRequested,
    bool RuntimeActionRequested,
    bool ModelBindingRequested,
    bool DatabaseWriteRequested,
    bool MorphologyPromotionRequested,
    bool GelPromotionRequested,
    bool CmeActualRequested,
    bool SanctuaryActualRequested,
    bool ContinuityAdmissionRequested,
    bool AuthorityRequested,
    bool ActivationRequested,
    bool PacketEmissionRequested,
    bool ReceiptReplayRequested,
    bool IncrementsPassageCount)
{
    public bool RequestsForbiddenMotion =>
        LispEvaluationRequested ||
        LispLoadRequested ||
        LispCompileRequested ||
        MacroExpansionRequested ||
        RuntimeActionRequested ||
        ModelBindingRequested ||
        DatabaseWriteRequested ||
        MorphologyPromotionRequested ||
        GelPromotionRequested ||
        CmeActualRequested ||
        SanctuaryActualRequested ||
        ContinuityAdmissionRequested ||
        AuthorityRequested ||
        ActivationRequested ||
        PacketEmissionRequested ||
        ReceiptReplayRequested ||
        IncrementsPassageCount;
}

public sealed record SliLispPostureManifestCarrier(
    string CarrierHandle,
    SliLispPostureManifestCarrierKind CarrierKind,
    string SourceHandle,
    string SourceName,
    IReadOnlyList<string> RequiredPostureTerms,
    IReadOnlyList<string> DeclaredNonActivationTerms,
    bool PreservesSourceHandle,
    bool PreservesPostureTerms,
    bool ReviewOnly,
    bool Inert,
    SliLispPostureManifestCarrierAuthorityBoundary AuthorityBoundary)
{
    public bool IsColdDeclaration =>
        !string.IsNullOrWhiteSpace(CarrierHandle) &&
        !string.IsNullOrWhiteSpace(SourceHandle) &&
        !string.IsNullOrWhiteSpace(SourceName) &&
        RequiredPostureTerms.Count > 0 &&
        RequiredPostureTerms.All(static term => !string.IsNullOrWhiteSpace(term)) &&
        DeclaredNonActivationTerms.Count > 0 &&
        DeclaredNonActivationTerms.All(static term => !string.IsNullOrWhiteSpace(term)) &&
        PreservesSourceHandle &&
        PreservesPostureTerms &&
        ReviewOnly &&
        Inert &&
        !AuthorityBoundary.RequestsForbiddenMotion;
}

public sealed record SliLispPostureManifestNonExecutionBoundary(
    bool ManifestMayEvaluateLisp,
    bool ManifestMayLoadLisp,
    bool ManifestMayCompileLisp,
    bool ManifestMayExpandMacros,
    bool ManifestMayBindModel,
    bool ManifestMayWriteDatabase,
    bool ManifestMayPromoteMorphology,
    bool ManifestMayPromoteGel,
    bool ManifestMayClaimCmeActual,
    bool ManifestMayClaimSanctuaryActual,
    bool ManifestMayAdmitContinuity,
    bool ManifestMayGrantAuthority,
    bool ManifestMayActivate,
    bool ManifestMayEmitPackets,
    bool ManifestMayReplayReceipts,
    bool ManifestMayIncrementPassageCount,
    string BoundaryLaw);

public sealed record SliLispPostureManifestRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record SliLispPostureManifestRequest(
    string ManifestHandle,
    IReadOnlyList<SliLispPostureManifestCarrier> Carriers,
    SliLispPostureManifestScopeBoundary ScopeBoundary,
    int PriorPassageCount);

public sealed record SliLispPostureManifestReceipt(
    string ReceiptHandle,
    SliLispPostureManifestDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string ManifestHandle,
    IReadOnlyList<SliLispPostureManifestCarrier> DeclaredCarriers,
    IReadOnlyList<string> PreservedSourceHandles,
    IReadOnlyList<string> PreservedPostureTerms,
    SliLispPostureManifestNonExecutionBoundary NonExecutionBoundary,
    SliLispPostureManifestRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterManifest,
    bool ReviewOnly,
    bool InertOnly,
    bool LispEvaluationRequested,
    bool LispLoadRequested,
    bool LispCompilationRequested,
    bool MacroExpansionRequested,
    bool RuntimeActionRequested,
    bool ModelBindingRequested,
    bool DatabaseWriteRequested,
    bool MorphologyPromotionRequested,
    bool GelPromotionRequested,
    bool CmeActualRequested,
    bool SanctuaryActualRequested,
    bool ContinuityAdmitted,
    bool ActivationRefused,
    bool AuthorityGranted,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdPostureManifest =>
        (Disposition is SliLispPostureManifestDisposition.DeclaredForReviewCold or SliLispPostureManifestDisposition.EmptyReviewCold) &&
        ReviewOnly &&
        InertOnly &&
        !LispEvaluationRequested &&
        !LispLoadRequested &&
        !LispCompilationRequested &&
        !MacroExpansionRequested &&
        !RuntimeActionRequested &&
        !ModelBindingRequested &&
        !DatabaseWriteRequested &&
        !MorphologyPromotionRequested &&
        !GelPromotionRequested &&
        !CmeActualRequested &&
        !SanctuaryActualRequested &&
        !ContinuityAdmitted &&
        ActivationRefused &&
        !AuthorityGranted &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        PassageCountAfterManifest == PriorPassageCount;
}

public sealed class DefaultSliLispPostureManifestBoundaryValidator
{
    private static readonly SliLispPostureManifestNonExecutionBoundary NonExecutionBoundary = new(
        ManifestMayEvaluateLisp: false,
        ManifestMayLoadLisp: false,
        ManifestMayCompileLisp: false,
        ManifestMayExpandMacros: false,
        ManifestMayBindModel: false,
        ManifestMayWriteDatabase: false,
        ManifestMayPromoteMorphology: false,
        ManifestMayPromoteGel: false,
        ManifestMayClaimCmeActual: false,
        ManifestMayClaimSanctuaryActual: false,
        ManifestMayAdmitContinuity: false,
        ManifestMayGrantAuthority: false,
        ManifestMayActivate: false,
        ManifestMayEmitPackets: false,
        ManifestMayReplayReceipts: false,
        ManifestMayIncrementPassageCount: false,
        BoundaryLaw: "SLI.Lisp posture may declare symbolic readiness for review. A manifest may not evaluate Lisp, load Lisp, compile Lisp, expand macros, bind models, mint morphology, promote GEL, admit continuity, activate runtime, emit packets, replay receipts, or become authority.");

    public SliLispPostureManifestReceipt Declare(
        SliLispPostureManifestRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!request.ScopeBoundary.Present ||
            string.IsNullOrWhiteSpace(request.ScopeBoundary.ScopeCode))
        {
            return Refuse(
                request,
                "sli-lisp-posture-manifest-scope-boundary-missing",
                "SLI.Lisp posture manifest refused because a review-only inert scope boundary is required.",
                timestampUtc);
        }

        if (!IsColdScope(request.ScopeBoundary))
        {
            return Refuse(
                request,
                "sli-lisp-posture-manifest-promotional-scope-refused",
                "SLI.Lisp posture manifest refused because scope must remain review-only and inert while refusing evaluation, load, compilation, macro expansion, runtime action, model binding, database write, morphology promotion, GEL promotion, CME.Actual, Sanctuary.Actual, continuity, authority, activation, packet emission, receipt replay, and passage increment.",
                timestampUtc);
        }

        if (request.Carriers.Count == 0)
        {
            return CreateReceipt(
                request,
                SliLispPostureManifestDisposition.EmptyReviewCold,
                "sli-lisp-posture-manifest-empty-review-only",
                "SLI.Lisp posture manifest found no carrier declarations. Empty manifest review grants no authority, activation, continuity, Lisp evaluation, packet emission, replay, or passage increment.",
                carriers: [],
                timestampUtc);
        }

        if (request.Carriers.Any(static carrier => !carrier.IsColdDeclaration))
        {
            return Refuse(
                request,
                "sli-lisp-posture-manifest-carrier-not-cold",
                "SLI.Lisp posture manifest refused because each carrier must preserve source handle, source name, posture terms, non-activation terms, review-only posture, inert posture, and non-execution authority boundary.",
                timestampUtc);
        }

        if (request.Carriers
            .GroupBy(static carrier => carrier.CarrierHandle, StringComparer.Ordinal)
            .Any(static group => group.Count() > 1))
        {
            return Refuse(
                request,
                "sli-lisp-posture-manifest-duplicate-carrier-refused",
                "SLI.Lisp posture manifest refused because each carrier declaration must have a distinct carrier handle.",
                timestampUtc);
        }

        return CreateReceipt(
            request,
            SliLispPostureManifestDisposition.DeclaredForReviewCold,
            "sli-lisp-posture-manifest-review-only",
            "SLI.Lisp posture manifest declared inert C# to SLI.Lisp readiness for review while preserving source handles and posture terms, and refusing evaluation, load, compilation, macro expansion, model binding, runtime action, database write, morphology promotion, GEL promotion, CME.Actual, Sanctuary.Actual, continuity, authority, activation, packet emission, replay, and passage increment.",
            request.Carriers.ToArray(),
            timestampUtc);
    }

    private static bool IsColdScope(SliLispPostureManifestScopeBoundary scope) =>
        scope.ReviewOnly &&
        scope.InertOnly &&
        !scope.AllowsLispEvaluation &&
        !scope.AllowsLispLoad &&
        !scope.AllowsLispCompilation &&
        !scope.AllowsMacroExpansion &&
        !scope.AllowsRuntimeAction &&
        !scope.AllowsModelBinding &&
        !scope.AllowsDatabaseWrite &&
        !scope.AllowsMorphologyPromotion &&
        !scope.AllowsGelPromotion &&
        !scope.AllowsCmeActual &&
        !scope.AllowsSanctuaryActual &&
        !scope.AllowsContinuityAdmission &&
        !scope.AllowsAuthority &&
        !scope.AllowsActivation &&
        !scope.AllowsPacketEmission &&
        !scope.AllowsReceiptReplay &&
        !scope.IncrementsPassageCount;

    private static SliLispPostureManifestReceipt CreateReceipt(
        SliLispPostureManifestRequest request,
        SliLispPostureManifestDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        IReadOnlyList<SliLispPostureManifestCarrier> carriers,
        DateTimeOffset timestampUtc)
    {
        var sourceHandles = carriers
            .Select(static carrier => carrier.SourceHandle)
            .Where(static handle => !string.IsNullOrWhiteSpace(handle))
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        var postureTerms = carriers
            .SelectMany(static carrier => carrier.RequiredPostureTerms.Concat(carrier.DeclaredNonActivationTerms))
            .Where(static term => !string.IsNullOrWhiteSpace(term))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        return new SliLispPostureManifestReceipt(
            ReceiptHandle: $"urn:san:sli-lisp-posture-manifest:review:{ShortHash(request.ManifestHandle, outcomeCode, carriers.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            ManifestHandle: request.ManifestHandle,
            DeclaredCarriers: carriers,
            PreservedSourceHandles: sourceHandles,
            PreservedPostureTerms: postureTerms,
            NonExecutionBoundary: NonExecutionBoundary,
            Refusal: null,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterManifest: request.PriorPassageCount,
            ReviewOnly: true,
            InertOnly: true,
            LispEvaluationRequested: false,
            LispLoadRequested: false,
            LispCompilationRequested: false,
            MacroExpansionRequested: false,
            RuntimeActionRequested: false,
            ModelBindingRequested: false,
            DatabaseWriteRequested: false,
            MorphologyPromotionRequested: false,
            GelPromotionRequested: false,
            CmeActualRequested: false,
            SanctuaryActualRequested: false,
            ContinuityAdmitted: false,
            ActivationRefused: true,
            AuthorityGranted: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            TimestampUtc: timestampUtc);
    }

    private static SliLispPostureManifestReceipt Refuse(
        SliLispPostureManifestRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:sli-lisp-posture-manifest:refused:{ShortHash(request.ManifestHandle, outcomeCode, timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: SliLispPostureManifestDisposition.Refused,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            ManifestHandle: request.ManifestHandle,
            DeclaredCarriers: [],
            PreservedSourceHandles: [],
            PreservedPostureTerms: [],
            NonExecutionBoundary: NonExecutionBoundary,
            Refusal: new SliLispPostureManifestRefusalReceipt(
                ReceiptHandle: $"urn:san:sli-lisp-posture-manifest-refusal:{ShortHash(request.ManifestHandle, outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterManifest: request.PriorPassageCount,
            ReviewOnly: true,
            InertOnly: true,
            LispEvaluationRequested: false,
            LispLoadRequested: false,
            LispCompilationRequested: false,
            MacroExpansionRequested: false,
            RuntimeActionRequested: false,
            ModelBindingRequested: false,
            DatabaseWriteRequested: false,
            MorphologyPromotionRequested: false,
            GelPromotionRequested: false,
            CmeActualRequested: false,
            SanctuaryActualRequested: false,
            ContinuityAdmitted: false,
            ActivationRefused: true,
            AuthorityGranted: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            TimestampUtc: timestampUtc);

    private static string ShortHash(params string[] values)
    {
        var material = string.Join("|", values);
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return Convert.ToHexString(hash).ToLowerInvariant()[..16];
    }
}
