using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum SliLispCompassCarrierShellDisposition
{
    EmptyReviewCold = 0,
    DeclaredForReviewCold = 1,
    Refused = 2
}

public enum SliLispPetalCandidateKind
{
    Skill = 0,
    Ability = 1,
    Talent = 2
}

public enum SliLispExtensionTemplateSurface
{
    EngineeredCognitionPetalTemplate = 0,
    GoaStewardControlMatrix = 1
}

public enum SliLispDomainTemplatePack
{
    Personal = 0,
    Enterprise = 1,
    Industrial = 2,
    Civic = 3,
    Governance = 4,
    Special = 5
}

public sealed record SliLispCompassCarrierShellScopeBoundary(
    string ScopeCode,
    bool Present,
    bool ReviewOnly,
    bool InertOnly,
    bool AllowsEngram,
    bool AllowsTruth,
    bool AllowsAuthority,
    bool AllowsContinuityAdmission,
    bool AllowsPetalAuthorization,
    bool AllowsLineagePermission,
    bool AllowsLispEvaluation,
    bool AllowsLispLoad,
    bool AllowsLispCompilation,
    bool AllowsMacroExpansion,
    bool AllowsRuntimeAction,
    bool AllowsPacketEmission,
    bool AllowsReceiptReplay,
    bool IncrementsPassageCount);

public sealed record SliLispRootingLineageChain(
    string SanctuaryId,
    string CradleId,
    string CmeId,
    string GelId,
    string SelfGelId,
    string OeId,
    bool PreservesTypedLineage,
    bool GrantsPermission,
    bool GrantsAuthority)
{
    public bool IsColdLineage =>
        !string.IsNullOrWhiteSpace(SanctuaryId) &&
        !string.IsNullOrWhiteSpace(CradleId) &&
        !string.IsNullOrWhiteSpace(CmeId) &&
        !string.IsNullOrWhiteSpace(GelId) &&
        !string.IsNullOrWhiteSpace(SelfGelId) &&
        !string.IsNullOrWhiteSpace(OeId) &&
        PreservesTypedLineage &&
        !GrantsPermission &&
        !GrantsAuthority;
}

public sealed record SliLispPetalCandidate(
    string PetalHandle,
    int PetalOrdinal,
    SliLispPetalCandidateKind CandidateKind,
    string CandidateName,
    string SourceHandle,
    SliLispExtensionTemplateSurface ExtensionSurface,
    SliLispDomainTemplatePack DomainTemplatePack,
    string PredicateClass,
    bool TemplateForm,
    bool StewardControlMatrixRequested,
    bool CrossDomainInheritanceRequested,
    bool CandidateOnly,
    bool ReviewOnly,
    bool Inert,
    bool AuthorityRequested,
    bool ClosureClaimed,
    bool ActivationRequested)
{
    public bool IsColdCandidate =>
        !string.IsNullOrWhiteSpace(PetalHandle) &&
        PetalOrdinal is >= 1 and <= 42 &&
        !string.IsNullOrWhiteSpace(CandidateName) &&
        !string.IsNullOrWhiteSpace(SourceHandle) &&
        !string.IsNullOrWhiteSpace(PredicateClass) &&
        ExtensionSurface == SliLispExtensionTemplateSurface.EngineeredCognitionPetalTemplate &&
        TemplateForm &&
        !StewardControlMatrixRequested &&
        !CrossDomainInheritanceRequested &&
        CandidateOnly &&
        ReviewOnly &&
        Inert &&
        !AuthorityRequested &&
        !ClosureClaimed &&
        !ActivationRequested;
}

public sealed record SliLispCompassCarrierShellBoundaryLaw(
    bool ShellMayBecomeEngram,
    bool ShellMayBecomeTruth,
    bool ShellMayGrantAuthority,
    bool ShellMayAdmitContinuity,
    bool PetalMayAuthorizeUse,
    bool PetalMayForceClosure,
    bool LineageMayGrantPermission,
    bool LispMayEvaluate,
    bool LispMayLoad,
    bool LispMayCompile,
    bool LispMayExpandMacros,
    bool RuntimeActionAllowed,
    bool PacketEmissionAllowed,
    bool ReceiptReplayAllowed,
    bool PassageMayIncrement,
    string BoundaryLaw);

public sealed record SliLispCompassCarrierShellRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record SliLispCompassCarrierShellRequest(
    string ShellHandle,
    string LispCarrierSourceName,
    SliLispDomainTemplatePack DeclaredDomainTemplatePack,
    SliLispRootingLineageChain Lineage,
    IReadOnlyList<SliLispPetalCandidate> PetalCandidates,
    CompassPressureWitnessContext WitnessContext,
    SliLispCompassCarrierShellScopeBoundary ScopeBoundary,
    int PriorPassageCount);

public sealed record SliLispCompassCarrierShellReceipt(
    string ReceiptHandle,
    SliLispCompassCarrierShellDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string ShellHandle,
    string LispCarrierSourceName,
    SliLispRootingLineageChain? Lineage,
    IReadOnlyList<SliLispPetalCandidate> PetalCandidates,
    IReadOnlyList<string> PreservedLineageIds,
    SliLispCompassCarrierShellBoundaryLaw Boundary,
    SliLispCompassCarrierShellRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterShell,
    bool ReviewOnly,
    bool InertOnly,
    bool WitnessPresent,
    bool SeparateCustody,
    bool ShellBecomesEngram,
    bool ShellBecomesTruth,
    bool ShellGrantsAuthority,
    bool ShellAdmitsContinuity,
    bool PetalAuthorizesUse,
    bool PetalForcesClosure,
    bool LineageGrantsPermission,
    bool LispEvaluationRequested,
    bool LispLoadRequested,
    bool LispCompilationRequested,
    bool MacroExpansionRequested,
    bool RuntimeActionRequested,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdShell =>
        Disposition is SliLispCompassCarrierShellDisposition.DeclaredForReviewCold or SliLispCompassCarrierShellDisposition.EmptyReviewCold &&
        ReviewOnly &&
        InertOnly &&
        WitnessPresent &&
        SeparateCustody &&
        !ShellBecomesEngram &&
        !ShellBecomesTruth &&
        !ShellGrantsAuthority &&
        !ShellAdmitsContinuity &&
        !PetalAuthorizesUse &&
        !PetalForcesClosure &&
        !LineageGrantsPermission &&
        !LispEvaluationRequested &&
        !LispLoadRequested &&
        !LispCompilationRequested &&
        !MacroExpansionRequested &&
        !RuntimeActionRequested &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        ActivationRefused &&
        PassageCountAfterShell == PriorPassageCount;
}

public sealed class DefaultSliLispCompassCarrierShellBoundaryValidator
{
    private static readonly SliLispCompassCarrierShellBoundaryLaw Boundary = new(
        ShellMayBecomeEngram: false,
        ShellMayBecomeTruth: false,
        ShellMayGrantAuthority: false,
        ShellMayAdmitContinuity: false,
        PetalMayAuthorizeUse: false,
        PetalMayForceClosure: false,
        LineageMayGrantPermission: false,
        LispMayEvaluate: false,
        LispMayLoad: false,
        LispMayCompile: false,
        LispMayExpandMacros: false,
        RuntimeActionAllowed: false,
        PacketEmissionAllowed: false,
        ReceiptReplayAllowed: false,
        PassageMayIncrement: false,
        BoundaryLaw: "SLI.Lisp may name a Compass shell, Rooting Law lineage, and petal candidates for review. Naming may not become engram, truth, authority, continuity, petal authorization, lineage permission, Lisp evaluation, runtime action, packet emission, receipt replay, or passage.");

    public SliLispCompassCarrierShellReceipt Declare(
        SliLispCompassCarrierShellRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.ShellHandle) ||
            string.IsNullOrWhiteSpace(request.LispCarrierSourceName))
        {
            return Refuse(
                request,
                "sli-lisp-compass-shell-source-missing",
                "SLI.Lisp Compass carrier shell refused because a shell handle and Lisp carrier source name are required.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.Present ||
            string.IsNullOrWhiteSpace(request.ScopeBoundary.ScopeCode))
        {
            return Refuse(
                request,
                "sli-lisp-compass-shell-scope-boundary-missing",
                "SLI.Lisp Compass carrier shell refused because a review-only inert scope boundary is required.",
                timestampUtc);
        }

        if (!IsColdScope(request.ScopeBoundary))
        {
            return Refuse(
                request,
                "sli-lisp-compass-shell-promotional-scope-refused",
                "SLI.Lisp Compass carrier shell refused because scope must remain review-only and inert while refusing engram, truth, authority, continuity, petal authorization, lineage permission, Lisp evaluation, runtime action, packet emission, receipt replay, and passage increment.",
                timestampUtc);
        }

        if (!request.WitnessContext.WitnessPresent ||
            !request.WitnessContext.SeparateCustody ||
            string.IsNullOrWhiteSpace(request.WitnessContext.WitnessSurface))
        {
            return Refuse(
                request,
                "sli-lisp-compass-shell-witness-context-missing",
                "SLI.Lisp Compass carrier shell refused because separate witness custody is required.",
                timestampUtc);
        }

        if (!request.Lineage.IsColdLineage)
        {
            return Refuse(
                request,
                "sli-lisp-compass-shell-lineage-not-cold",
                "SLI.Lisp Compass carrier shell refused because Rooting Law lineage must preserve typed lineage without granting permission or authority.",
                timestampUtc);
        }

        if (request.PetalCandidates.Any(static petal => !petal.IsColdCandidate))
        {
            return Refuse(
                request,
                "sli-lisp-compass-shell-petal-not-cold",
                "SLI.Lisp Compass carrier shell refused because every Lisp extension petal must remain an Engineered Cognition petal-template, review-only inert candidate and may not request Steward GoA control matrix access, authorize use, force closure, or request activation.",
                timestampUtc);
        }

        if (request.PetalCandidates.Any(petal => petal.DomainTemplatePack != request.DeclaredDomainTemplatePack))
        {
            return Refuse(
                request,
                "sli-lisp-compass-shell-domain-template-mismatch",
                "SLI.Lisp Compass carrier shell refused because EC petal templates must stay inside their declared domain template pack; Industrial, Civic, Governance, Enterprise, Personal, and Special packs may not silently inherit one another.",
                timestampUtc);
        }

        if (request.PetalCandidates
            .GroupBy(static petal => petal.PetalHandle, StringComparer.Ordinal)
            .Any(static group => group.Count() > 1))
        {
            return Refuse(
                request,
                "sli-lisp-compass-shell-duplicate-petal-refused",
                "SLI.Lisp Compass carrier shell refused because petal handles must remain distinct.",
                timestampUtc);
        }

        var disposition = request.PetalCandidates.Count == 0
            ? SliLispCompassCarrierShellDisposition.EmptyReviewCold
            : SliLispCompassCarrierShellDisposition.DeclaredForReviewCold;
        var outcomeCode = disposition == SliLispCompassCarrierShellDisposition.EmptyReviewCold
            ? "sli-lisp-compass-shell-empty-review-only"
            : "sli-lisp-compass-shell-review-only";
        var governanceTrace = disposition == SliLispCompassCarrierShellDisposition.EmptyReviewCold
            ? "SLI.Lisp Compass carrier shell found no petal candidates. Empty shell review preserves Rooting Law lineage but grants no authority, closure, activation, continuity, or Lisp evaluation."
            : "SLI.Lisp Compass carrier shell declared Rooting Law lineage and petal candidates as inert review posture while refusing engram, truth, authority, continuity, petal authorization, lineage permission, Lisp evaluation, runtime action, packet emission, receipt replay, and passage.";

        return CreateReceipt(
            request,
            disposition,
            outcomeCode,
            governanceTrace,
            request.PetalCandidates.ToArray(),
            timestampUtc);
    }

    private static bool IsColdScope(SliLispCompassCarrierShellScopeBoundary scope) =>
        scope.ReviewOnly &&
        scope.InertOnly &&
        !scope.AllowsEngram &&
        !scope.AllowsTruth &&
        !scope.AllowsAuthority &&
        !scope.AllowsContinuityAdmission &&
        !scope.AllowsPetalAuthorization &&
        !scope.AllowsLineagePermission &&
        !scope.AllowsLispEvaluation &&
        !scope.AllowsLispLoad &&
        !scope.AllowsLispCompilation &&
        !scope.AllowsMacroExpansion &&
        !scope.AllowsRuntimeAction &&
        !scope.AllowsPacketEmission &&
        !scope.AllowsReceiptReplay &&
        !scope.IncrementsPassageCount;

    private static SliLispCompassCarrierShellReceipt CreateReceipt(
        SliLispCompassCarrierShellRequest request,
        SliLispCompassCarrierShellDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        IReadOnlyList<SliLispPetalCandidate> petals,
        DateTimeOffset timestampUtc)
    {
        var lineageIds = request.Lineage.IsColdLineage
            ? new[]
            {
                request.Lineage.SanctuaryId,
                request.Lineage.CradleId,
                request.Lineage.CmeId,
                request.Lineage.GelId,
                request.Lineage.SelfGelId,
                request.Lineage.OeId
            }
            : [];

        return new SliLispCompassCarrierShellReceipt(
            ReceiptHandle: $"urn:san:sli-lisp-compass-shell:review:{ShortHash(request.ShellHandle, outcomeCode, petals.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            ShellHandle: request.ShellHandle,
            LispCarrierSourceName: request.LispCarrierSourceName,
            Lineage: request.Lineage,
            PetalCandidates: petals,
            PreservedLineageIds: lineageIds,
            Boundary: Boundary,
            Refusal: null,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterShell: request.PriorPassageCount,
            ReviewOnly: true,
            InertOnly: true,
            WitnessPresent: true,
            SeparateCustody: true,
            ShellBecomesEngram: false,
            ShellBecomesTruth: false,
            ShellGrantsAuthority: false,
            ShellAdmitsContinuity: false,
            PetalAuthorizesUse: false,
            PetalForcesClosure: false,
            LineageGrantsPermission: false,
            LispEvaluationRequested: false,
            LispLoadRequested: false,
            LispCompilationRequested: false,
            MacroExpansionRequested: false,
            RuntimeActionRequested: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static SliLispCompassCarrierShellReceipt Refuse(
        SliLispCompassCarrierShellRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:sli-lisp-compass-shell:refused:{ShortHash(request.ShellHandle, outcomeCode, timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: SliLispCompassCarrierShellDisposition.Refused,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            ShellHandle: request.ShellHandle,
            LispCarrierSourceName: request.LispCarrierSourceName,
            Lineage: request.Lineage,
            PetalCandidates: [],
            PreservedLineageIds: [],
            Boundary: Boundary,
            Refusal: new SliLispCompassCarrierShellRefusalReceipt(
                ReceiptHandle: $"urn:san:sli-lisp-compass-shell-refusal:{ShortHash(request.ShellHandle, outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterShell: request.PriorPassageCount,
            ReviewOnly: true,
            InertOnly: true,
            WitnessPresent: request.WitnessContext.WitnessPresent,
            SeparateCustody: request.WitnessContext.SeparateCustody,
            ShellBecomesEngram: false,
            ShellBecomesTruth: false,
            ShellGrantsAuthority: false,
            ShellAdmitsContinuity: false,
            PetalAuthorizesUse: false,
            PetalForcesClosure: false,
            LineageGrantsPermission: false,
            LispEvaluationRequested: false,
            LispLoadRequested: false,
            LispCompilationRequested: false,
            MacroExpansionRequested: false,
            RuntimeActionRequested: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);

    private static string ShortHash(params string[] values)
    {
        var material = string.Join("|", values);
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return Convert.ToHexString(hash).ToLowerInvariant()[..16];
    }
}
