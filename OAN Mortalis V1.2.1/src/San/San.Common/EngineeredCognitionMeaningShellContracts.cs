using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum EcMeaningShellDisposition
{
    EmptyReviewCold = 0,
    FormedForReviewCold = 1,
    CompostedForReviewCold = 2,
    Refused = 3
}

public enum EcMeaningShellTier
{
    Root = 0,
    PropositionalTier1 = 1,
    ProceduralTier2Plus = 2,
    PerspectivalComposite = 3
}

public enum EcMeaningShellSplineOutcome
{
    HeldOpen = 0,
    ClosedCandidate = 1,
    MergeToBaseCandidate = 2,
    NewRootCandidate = 3,
    Composted = 4,
    Refused = 5
}

public enum EcIngressPosture
{
    BondedOperator = 0,
    AuthorizedToolBody = 1,
    UnbondedIo = 2
}

public sealed record EcMeaningShellScopeBoundary(
    string ScopeCode,
    bool Present,
    bool ReviewOnly,
    bool InertOnly,
    bool AllowsEngram,
    bool AllowsSelfAttribution,
    bool AllowsSelfGelAppend,
    bool AllowsCSelfGelAppend,
    bool AllowsAuthority,
    bool AllowsContinuityAdmission,
    bool AllowsIdentityMutation,
    bool AllowsRuntimeAction,
    bool AllowsLispEvaluation,
    bool AllowsDomainInheritance,
    bool AllowsPacketEmission,
    bool AllowsReceiptReplay,
    bool IncrementsPassageCount);

public sealed record EcMeaningShellCandidate(
    string ShellHandle,
    string RootAnchor,
    EcMeaningShellTier Tier,
    string PropositionalPredicate,
    string ProceduralTrace,
    string PerspectivalTrunk,
    IReadOnlyList<string> PerspectivalBranches,
    string SourcePetalHandle,
    SliLispDomainTemplatePack DomainTemplatePack,
    string PredicateClass,
    bool CandidateOnly,
    bool ReviewOnly,
    bool Inert,
    bool CompostAllowed,
    bool ClosureClaimed,
    bool EngramClaimed,
    bool SelfAttributionClaimed,
    bool AuthorityRequested,
    bool ActivationRequested)
{
    public bool IsColdMeaningShell =>
        !string.IsNullOrWhiteSpace(ShellHandle) &&
        !string.IsNullOrWhiteSpace(RootAnchor) &&
        !string.IsNullOrWhiteSpace(SourcePetalHandle) &&
        !string.IsNullOrWhiteSpace(PredicateClass) &&
        HasTierPayload &&
        CandidateOnly &&
        ReviewOnly &&
        Inert &&
        !ClosureClaimed &&
        !EngramClaimed &&
        !SelfAttributionClaimed &&
        !AuthorityRequested &&
        !ActivationRequested;

    public bool HasTierPayload =>
        Tier switch
        {
            EcMeaningShellTier.Root => true,
            EcMeaningShellTier.PropositionalTier1 => !string.IsNullOrWhiteSpace(PropositionalPredicate),
            EcMeaningShellTier.ProceduralTier2Plus => !string.IsNullOrWhiteSpace(ProceduralTrace),
            EcMeaningShellTier.PerspectivalComposite => !string.IsNullOrWhiteSpace(PerspectivalTrunk) &&
                PerspectivalBranches.Count > 0 &&
                PerspectivalBranches.All(static branch => !string.IsNullOrWhiteSpace(branch)),
            _ => false
        };
}

public sealed record EcCompostDisposition(
    string CompostHandle,
    string SourceShellHandle,
    bool RetainedNearCSelfGel,
    bool AttributedToSelf,
    bool GrantsContinuity,
    bool ReviewOnly,
    bool Inert,
    string ResolutionNote)
{
    public bool IsColdCompost =>
        !string.IsNullOrWhiteSpace(CompostHandle) &&
        !string.IsNullOrWhiteSpace(SourceShellHandle) &&
        !string.IsNullOrWhiteSpace(ResolutionNote) &&
        RetainedNearCSelfGel &&
        !AttributedToSelf &&
        !GrantsContinuity &&
        ReviewOnly &&
        Inert;
}

public sealed record EcMeaningShellBoundaryLaw(
    bool ShellMayBecomeEngram,
    bool ShellMayAppendSelfGel,
    bool ShellMayAppendCSelfGel,
    bool ShellMayAuthorize,
    bool ShellMayActivate,
    bool ShellMayEvaluateLisp,
    bool ShellMayAdmitContinuity,
    bool ShellMayMutateIdentity,
    bool CompostMayAttributeToSelf,
    bool CompostMayGrantContinuity,
    bool SplineOutcomeMayCloseAsTruth,
    bool DomainInheritanceAllowed,
    bool PacketEmissionAllowed,
    bool ReceiptReplayAllowed,
    bool PassageMayIncrement,
    string BoundaryLaw);

public sealed record EcMeaningShellRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record EcMeaningShellRequest(
    SliLispCompassCarrierShellReceipt? SourceCarrierShell,
    SliLispDomainTemplatePack DeclaredDomainTemplatePack,
    IReadOnlyList<EcMeaningShellCandidate> MeaningShells,
    IReadOnlyList<EcCompostDisposition> CompostDispositions,
    EcMeaningShellSplineOutcome SplineOutcome,
    CompassPressureWitnessContext WitnessContext,
    EcMeaningShellScopeBoundary ScopeBoundary,
    EcIngressPosture IngressPosture,
    bool IngressAuthorized,
    int PriorPassageCount);

public sealed record EcMeaningShellReceipt(
    string ReceiptHandle,
    EcMeaningShellDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceCompassShellHandle,
    IReadOnlyList<EcMeaningShellCandidate> MeaningShells,
    IReadOnlyList<EcCompostDisposition> CompostDispositions,
    EcMeaningShellSplineOutcome SplineOutcome,
    IReadOnlyList<string> PreservedPetalHandles,
    IReadOnlyList<string> PreservedLineageIds,
    EcMeaningShellBoundaryLaw Boundary,
    EcMeaningShellRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterShell,
    bool ReviewOnly,
    bool InertOnly,
    bool WitnessPresent,
    bool SeparateCustody,
    bool ShellBecomesEngram,
    bool SelfGelAppendAllowed,
    bool CSelfGelAppendAllowed,
    bool AuthorityGranted,
    bool ContinuityAdmitted,
    bool IdentityMutated,
    bool LispEvaluationRequested,
    bool RuntimeActionRequested,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdMeaningShell =>
        (Disposition is EcMeaningShellDisposition.FormedForReviewCold or
            EcMeaningShellDisposition.CompostedForReviewCold or
            EcMeaningShellDisposition.EmptyReviewCold) &&
        ReviewOnly &&
        InertOnly &&
        WitnessPresent &&
        SeparateCustody &&
        !ShellBecomesEngram &&
        !SelfGelAppendAllowed &&
        !CSelfGelAppendAllowed &&
        !AuthorityGranted &&
        !ContinuityAdmitted &&
        !IdentityMutated &&
        !LispEvaluationRequested &&
        !RuntimeActionRequested &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        ActivationRefused &&
        PassageCountAfterShell == PriorPassageCount;
}

public sealed class DefaultEngineeredCognitionMeaningShellBoundaryValidator
{
    private static readonly EcMeaningShellBoundaryLaw Boundary = new(
        ShellMayBecomeEngram: false,
        ShellMayAppendSelfGel: false,
        ShellMayAppendCSelfGel: false,
        ShellMayAuthorize: false,
        ShellMayActivate: false,
        ShellMayEvaluateLisp: false,
        ShellMayAdmitContinuity: false,
        ShellMayMutateIdentity: false,
        CompostMayAttributeToSelf: false,
        CompostMayGrantContinuity: false,
        SplineOutcomeMayCloseAsTruth: false,
        DomainInheritanceAllowed: false,
        PacketEmissionAllowed: false,
        ReceiptReplayAllowed: false,
        PassageMayIncrement: false,
        BoundaryLaw: "Engineered Cognition meaning shells may form Root, propositional, procedural, and perspectival candidate bodies for review. Shell formation may not become engram, SelfGEL, cSelfGEL, authority, continuity, identity mutation, Lisp evaluation, runtime action, packet emission, receipt replay, or passage.");

    public EcMeaningShellReceipt Declare(
        EcMeaningShellRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceCarrierShell is null || !request.SourceCarrierShell.IsColdShell)
        {
            return Refuse(
                request,
                "ec-meaning-shell-source-shell-missing",
                "Engineered Cognition meaning shell refused because a cold SLI.Lisp Compass carrier shell source is required.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.Present ||
            string.IsNullOrWhiteSpace(request.ScopeBoundary.ScopeCode))
        {
            return Refuse(
                request,
                "ec-meaning-shell-scope-boundary-missing",
                "Engineered Cognition meaning shell refused because a review-only inert scope boundary is required.",
                timestampUtc);
        }

        if (!IsColdScope(request.ScopeBoundary))
        {
            return Refuse(
                request,
                "ec-meaning-shell-promotional-scope-refused",
                "Engineered Cognition meaning shell refused because scope must remain review-only and inert while refusing engram, SelfGEL, cSelfGEL, authority, continuity, identity mutation, Lisp evaluation, domain inheritance, packet emission, receipt replay, and passage.",
                timestampUtc);
        }

        if (!request.WitnessContext.WitnessPresent ||
            !request.WitnessContext.SeparateCustody ||
            string.IsNullOrWhiteSpace(request.WitnessContext.WitnessSurface))
        {
            return Refuse(
                request,
                "ec-meaning-shell-witness-context-missing",
                "Engineered Cognition meaning shell refused because separate witness custody is required.",
                timestampUtc);
        }

        if (!request.IngressAuthorized || request.IngressPosture == EcIngressPosture.UnbondedIo)
        {
            return Refuse(
                request,
                "ec-meaning-shell-ingress-neutral-clamp",
                "Engineered Cognition meaning shell refused because unbonded or unauthorized ingress must clamp to neutral review posture.",
                timestampUtc);
        }

        var sourcePetalHandles = request.SourceCarrierShell.PetalCandidates
            .Select(static petal => petal.PetalHandle)
            .ToHashSet(StringComparer.Ordinal);
        var sourceLineageIds = request.SourceCarrierShell.PreservedLineageIds.ToArray();

        if (request.MeaningShells.Any(static shell => !shell.IsColdMeaningShell))
        {
            return Refuse(
                request,
                "ec-meaning-shell-not-cold",
                "Engineered Cognition meaning shell refused because every meaning shell must remain a review-only inert candidate with tier payload and without engram, Self, authority, closure, or activation claims.",
                timestampUtc);
        }

        if (request.MeaningShells.Any(shell =>
            shell.DomainTemplatePack != request.DeclaredDomainTemplatePack ||
            !shell.PredicateClass.StartsWith($"{DomainPrefix(request.DeclaredDomainTemplatePack)}-", StringComparison.Ordinal)))
        {
            return Refuse(
                request,
                "ec-meaning-shell-domain-template-mismatch",
                "Engineered Cognition meaning shell refused because meaning shells must stay inside their declared domain template pack.",
                timestampUtc);
        }

        if (request.MeaningShells.Any(shell => !sourcePetalHandles.Contains(shell.SourcePetalHandle)))
        {
            return Refuse(
                request,
                "ec-meaning-shell-source-petal-missing",
                "Engineered Cognition meaning shell refused because each shell must bind to a petal handle from the source SLI.Lisp carrier shell.",
                timestampUtc);
        }

        if (request.MeaningShells
            .GroupBy(static shell => shell.ShellHandle, StringComparer.Ordinal)
            .Any(static group => group.Count() > 1))
        {
            return Refuse(
                request,
                "ec-meaning-shell-duplicate-shell-refused",
                "Engineered Cognition meaning shell refused because shell handles must remain distinct.",
                timestampUtc);
        }

        var shellHandles = request.MeaningShells
            .Select(static shell => shell.ShellHandle)
            .ToHashSet(StringComparer.Ordinal);

        if (request.CompostDispositions.Any(compost =>
            !compost.IsColdCompost ||
            !shellHandles.Contains(compost.SourceShellHandle)))
        {
            return Refuse(
                request,
                "ec-meaning-shell-compost-not-cold",
                "Engineered Cognition meaning shell refused because compost must be retained near cSelfGEL as review-only non-Self evidence bound to a source shell.",
                timestampUtc);
        }

        if (request.SplineOutcome == EcMeaningShellSplineOutcome.Refused)
        {
            return Refuse(
                request,
                "ec-meaning-shell-spline-refused",
                "Engineered Cognition meaning shell refused because refused spline outcomes must remain refusal receipts, not shell formation receipts.",
                timestampUtc);
        }

        var disposition = ResolveDisposition(request);
        var outcomeCode = disposition switch
        {
            EcMeaningShellDisposition.EmptyReviewCold => "ec-meaning-shell-empty-review-only",
            EcMeaningShellDisposition.CompostedForReviewCold => "ec-meaning-shell-compost-review-only",
            _ => "ec-meaning-shell-formed-review-only"
        };
        var governanceTrace = disposition switch
        {
            EcMeaningShellDisposition.EmptyReviewCold =>
                "Engineered Cognition found no meaning shells. Empty review preserves source SLI.Lisp lineage but grants no engram, authority, continuity, SelfGEL, cSelfGEL, identity mutation, Lisp evaluation, or activation.",
            EcMeaningShellDisposition.CompostedForReviewCold =>
                "Engineered Cognition retained compost near cSelfGEL as review-only non-Self evidence while refusing engram, authority, continuity, Self attribution, Lisp evaluation, packet emission, replay, and passage.",
            _ =>
                "Engineered Cognition formed cold meaning shells across Root, propositional, procedural, and perspectival tiers for review while refusing engram, authority, continuity, GEL append, identity mutation, Lisp evaluation, packet emission, replay, and passage."
        };

        return new EcMeaningShellReceipt(
            ReceiptHandle: $"urn:san:ec-meaning-shell:review:{ShortHash(request.SourceCarrierShell.ShellHandle, outcomeCode, request.MeaningShells.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceCompassShellHandle: request.SourceCarrierShell.ShellHandle,
            MeaningShells: request.MeaningShells.ToArray(),
            CompostDispositions: request.CompostDispositions.ToArray(),
            SplineOutcome: request.SplineOutcome,
            PreservedPetalHandles: sourcePetalHandles.OrderBy(static handle => handle, StringComparer.Ordinal).ToArray(),
            PreservedLineageIds: sourceLineageIds,
            Boundary: Boundary,
            Refusal: null,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterShell: request.PriorPassageCount,
            ReviewOnly: true,
            InertOnly: true,
            WitnessPresent: true,
            SeparateCustody: true,
            ShellBecomesEngram: false,
            SelfGelAppendAllowed: false,
            CSelfGelAppendAllowed: false,
            AuthorityGranted: false,
            ContinuityAdmitted: false,
            IdentityMutated: false,
            LispEvaluationRequested: false,
            RuntimeActionRequested: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static EcMeaningShellDisposition ResolveDisposition(EcMeaningShellRequest request)
    {
        if (request.MeaningShells.Count == 0)
        {
            return EcMeaningShellDisposition.EmptyReviewCold;
        }

        if (request.SplineOutcome == EcMeaningShellSplineOutcome.Composted ||
            request.MeaningShells.All(static shell => shell.CompostAllowed))
        {
            return EcMeaningShellDisposition.CompostedForReviewCold;
        }

        return EcMeaningShellDisposition.FormedForReviewCold;
    }

    private static bool IsColdScope(EcMeaningShellScopeBoundary scope) =>
        scope.ReviewOnly &&
        scope.InertOnly &&
        !scope.AllowsEngram &&
        !scope.AllowsSelfAttribution &&
        !scope.AllowsSelfGelAppend &&
        !scope.AllowsCSelfGelAppend &&
        !scope.AllowsAuthority &&
        !scope.AllowsContinuityAdmission &&
        !scope.AllowsIdentityMutation &&
        !scope.AllowsRuntimeAction &&
        !scope.AllowsLispEvaluation &&
        !scope.AllowsDomainInheritance &&
        !scope.AllowsPacketEmission &&
        !scope.AllowsReceiptReplay &&
        !scope.IncrementsPassageCount;

    private static EcMeaningShellReceipt Refuse(
        EcMeaningShellRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc)
    {
        var sourceHandle = request.SourceCarrierShell?.ShellHandle ?? string.Empty;

        return new EcMeaningShellReceipt(
            ReceiptHandle: $"urn:san:ec-meaning-shell:refused:{ShortHash(sourceHandle, outcomeCode, timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: EcMeaningShellDisposition.Refused,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceCompassShellHandle: sourceHandle,
            MeaningShells: [],
            CompostDispositions: [],
            SplineOutcome: EcMeaningShellSplineOutcome.Refused,
            PreservedPetalHandles: request.SourceCarrierShell?.PetalCandidates.Select(static petal => petal.PetalHandle).ToArray() ?? [],
            PreservedLineageIds: request.SourceCarrierShell?.PreservedLineageIds.ToArray() ?? [],
            Boundary: Boundary,
            Refusal: new EcMeaningShellRefusalReceipt(
                ReceiptHandle: $"urn:san:ec-meaning-shell-refusal:{ShortHash(sourceHandle, outcomeCode)}",
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
            SelfGelAppendAllowed: false,
            CSelfGelAppendAllowed: false,
            AuthorityGranted: false,
            ContinuityAdmitted: false,
            IdentityMutated: false,
            LispEvaluationRequested: false,
            RuntimeActionRequested: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static string DomainPrefix(SliLispDomainTemplatePack domain) =>
        domain.ToString().ToLowerInvariant();

    private static string ShortHash(params string[] values)
    {
        var material = string.Join("|", values);
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return Convert.ToHexString(hash).ToLowerInvariant()[..16];
    }
}
