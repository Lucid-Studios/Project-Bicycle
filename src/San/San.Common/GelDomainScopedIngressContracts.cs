using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace San.Common;

public enum GelDomainScopedIngressDisposition
{
    RecommendedCold = 0,
    Held = 1,
    Refused = 2
}

public enum GelIngressDomain
{
    ScholarlyReview = 0,
    EngineeringTelemetry = 1,
    OperatorDoctrine = 2,
    Pedagogy = 3,
    CivicGovernance = 4,
    LegalCompliance = 5,
    MedicalClinical = 6,
    Personification = 7,
    Security = 8,
    MilitaryDefenseClosed = 9,
    SpecialCase = 10
}

public enum GelIngressEvidenceCeiling
{
    Interpretive = 0,
    Operational = 1,
    Reproducible = 2,
    Regulated = 3,
    Licensed = 4,
    Clinical = 5,
    SpecialCaseHeld = 6,
    Closed = 7
}

public enum GelIngressCycleStage
{
    SourceEvent = 0,
    TelemetryPrecipitation = 1,
    EppsResidue = 2,
    BridgeSynthesis = 3,
    CandidateSubstrate = 4,
    DomainClassification = 5,
    EvidenceCeilingAssignment = 6,
    Cooling = 7,
    StewardReview = 8,
    Recommendation = 9
}

public sealed record GelIngressCandidateSubstrate(
    string CandidateHandle,
    string SourceEppsReceiptHandle,
    string SourceBridgeReceiptHandle,
    string CandidateSummary,
    IReadOnlyList<string> SourceResidueHandles,
    IReadOnlyList<string> SourceBridgeSegmentHandles,
    bool PostGelFormation,
    bool PreGelAdmission,
    bool CandidateOnly,
    bool ReviewOnly,
    bool FormedSubstrate,
    bool AdmittedGel,
    bool AdmittedMemory,
    bool MutatedContinuity,
    bool MutatedSelfGel,
    bool GrantedAuthority,
    bool AuthorizedAction,
    bool EvaluatesLisp,
    bool EmitsPacket,
    bool IncrementsPassage,
    bool Activates)
{
    public bool IsColdCandidate =>
        !string.IsNullOrWhiteSpace(CandidateHandle) &&
        !string.IsNullOrWhiteSpace(SourceEppsReceiptHandle) &&
        !string.IsNullOrWhiteSpace(SourceBridgeReceiptHandle) &&
        !string.IsNullOrWhiteSpace(CandidateSummary) &&
        SourceResidueHandles.Count > 0 &&
        SourceResidueHandles.All(static handle => !string.IsNullOrWhiteSpace(handle)) &&
        SourceResidueHandles.Distinct(StringComparer.Ordinal).Count() == SourceResidueHandles.Count &&
        SourceBridgeSegmentHandles.Count > 0 &&
        SourceBridgeSegmentHandles.All(static handle => !string.IsNullOrWhiteSpace(handle)) &&
        SourceBridgeSegmentHandles.Distinct(StringComparer.Ordinal).Count() == SourceBridgeSegmentHandles.Count &&
        PostGelFormation &&
        PreGelAdmission &&
        CandidateOnly &&
        ReviewOnly &&
        FormedSubstrate &&
        !AdmittedGel &&
        !AdmittedMemory &&
        !MutatedContinuity &&
        !MutatedSelfGel &&
        !GrantedAuthority &&
        !AuthorizedAction &&
        !EvaluatesLisp &&
        !EmitsPacket &&
        !IncrementsPassage &&
        !Activates;
}

public sealed record GelDomainScopeRecord(
    string ScopeHandle,
    GelIngressDomain Domain,
    GelIngressEvidenceCeiling EvidenceCeiling,
    string DomainRationale,
    string EvidenceCeilingRationale,
    string LossCondition,
    bool Present,
    bool ReviewOnly,
    bool DomainFitReviewed,
    bool EvidenceCeilingAssigned,
    bool CoolingRequired,
    bool StewardReviewRequired,
    bool EvidenceCeilingPortable,
    bool DomainFitAdmitsGel,
    bool DomainFitAdmitsMemory,
    bool DomainFitMutatesContinuity,
    bool DomainFitGrantsAuthority,
    bool DomainFitAuthorizesAction,
    bool RequiresSpecialCaseHold,
    bool SpecialCaseHeld,
    bool DomainClosed)
{
    public bool IsColdScope =>
        !string.IsNullOrWhiteSpace(ScopeHandle) &&
        !string.IsNullOrWhiteSpace(DomainRationale) &&
        !string.IsNullOrWhiteSpace(EvidenceCeilingRationale) &&
        !string.IsNullOrWhiteSpace(LossCondition) &&
        Enum.IsDefined(Domain) &&
        Enum.IsDefined(EvidenceCeiling) &&
        Present &&
        ReviewOnly &&
        DomainFitReviewed &&
        EvidenceCeilingAssigned &&
        CoolingRequired &&
        StewardReviewRequired &&
        !EvidenceCeilingPortable &&
        !DomainFitAdmitsGel &&
        !DomainFitAdmitsMemory &&
        !DomainFitMutatesContinuity &&
        !DomainFitGrantsAuthority &&
        !DomainFitAuthorizesAction &&
        (RequiresSpecialCaseHold == (Domain is GelIngressDomain.SpecialCase or GelIngressDomain.Personification)) &&
        (!RequiresSpecialCaseHold || SpecialCaseHeld) &&
        (DomainClosed == (Domain == GelIngressDomain.MilitaryDefenseClosed || EvidenceCeiling == GelIngressEvidenceCeiling.Closed));
}

public sealed record GelIngressCycleTrace(
    string TraceHandle,
    GelIngressCycleStage Stage,
    string SourceHandle,
    string Summary,
    bool ReviewOnly,
    bool MutatesGel,
    bool AdmitsContinuity,
    bool GrantsAuthority,
    bool AuthorizesAction)
{
    public bool IsColdTrace =>
        !string.IsNullOrWhiteSpace(TraceHandle) &&
        !string.IsNullOrWhiteSpace(SourceHandle) &&
        !string.IsNullOrWhiteSpace(Summary) &&
        Enum.IsDefined(Stage) &&
        ReviewOnly &&
        !MutatesGel &&
        !AdmitsContinuity &&
        !GrantsAuthority &&
        !AuthorizesAction;
}

public sealed record GelIngressStewardReview(
    string ReviewHandle,
    string StewardTrace,
    bool ReviewOnly,
    bool StewardCustodyPresent,
    bool CoolingComplete,
    bool RecommendationMayIssue,
    bool RecommendsIngressConsideration,
    bool PerformsAdmission,
    bool AdmitsGel,
    bool AdmitsMemory,
    bool MutatesContinuity,
    bool GrantsAuthority,
    bool AuthorizesAction,
    bool EvaluatesLisp,
    bool EmitsPacket,
    bool ReplaysReceipt,
    bool IncrementsPassage,
    bool Activates)
{
    public bool IsColdReview =>
        !string.IsNullOrWhiteSpace(ReviewHandle) &&
        !string.IsNullOrWhiteSpace(StewardTrace) &&
        ReviewOnly &&
        StewardCustodyPresent &&
        !PerformsAdmission &&
        !AdmitsGel &&
        !AdmitsMemory &&
        !MutatesContinuity &&
        !GrantsAuthority &&
        !AuthorizesAction &&
        !EvaluatesLisp &&
        !EmitsPacket &&
        !ReplaysReceipt &&
        !IncrementsPassage &&
        !Activates;
}

public sealed record GelDomainScopedIngressBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool RequiresColdEpps,
    bool RequiresColdPeerReviewBridge,
    bool RequiresCandidateSubstrate,
    bool RequiresDomainScope,
    bool RequiresEvidenceCeiling,
    bool RequiresCooling,
    bool RequiresStewardReview,
    bool AllowsGovernanceSurvivorshipAsProof,
    bool AllowsDomainFitAsAdmission,
    bool AllowsEvidenceCeilingPortability,
    bool AllowsRecommendationAsAdmission,
    bool AllowsGelAdmission,
    bool AllowsMemoryAdmission,
    bool AllowsContinuityMutation,
    bool AllowsSelfGelMutation,
    bool AllowsAuthority,
    bool AllowsAction,
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
        RequiresColdEpps &&
        RequiresColdPeerReviewBridge &&
        RequiresCandidateSubstrate &&
        RequiresDomainScope &&
        RequiresEvidenceCeiling &&
        RequiresCooling &&
        RequiresStewardReview &&
        !AllowsGovernanceSurvivorshipAsProof &&
        !AllowsDomainFitAsAdmission &&
        !AllowsEvidenceCeilingPortability &&
        !AllowsRecommendationAsAdmission &&
        !AllowsGelAdmission &&
        !AllowsMemoryAdmission &&
        !AllowsContinuityMutation &&
        !AllowsSelfGelMutation &&
        !AllowsAuthority &&
        !AllowsAction &&
        !AllowsLispEvaluation &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement &&
        !AllowsActivation;
}

public sealed record GelDomainScopedIngressRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record GelDomainScopedIngressRequest(
    EngramPredicatePrecursorStreamReceipt? SourceEppsReceipt,
    PeerReviewPredicateBridgeReceipt? SourceBridgeReceipt,
    GelIngressCandidateSubstrate Candidate,
    GelDomainScopeRecord DomainScope,
    IReadOnlyList<GelIngressCycleTrace> CycleTrace,
    GelIngressStewardReview StewardReview,
    GelDomainScopedIngressBoundary Boundary,
    int PriorRecommendationCount,
    int PriorPassageCount,
    bool RepeatedRecommendationCreatesWarrant,
    bool GelAdmissionRequested,
    bool MemoryAdmissionRequested,
    bool ContinuityMutationRequested,
    bool SelfGelMutationRequested,
    bool AuthorityRequested,
    bool ActionRequested,
    bool LispEvaluationRequested,
    bool PacketEmissionRequested,
    bool ReceiptReplayRequested,
    bool PassageIncrementRequested,
    bool ActivationRequested)
{
    public bool RequestsForbiddenMotion =>
        RepeatedRecommendationCreatesWarrant ||
        GelAdmissionRequested ||
        MemoryAdmissionRequested ||
        ContinuityMutationRequested ||
        SelfGelMutationRequested ||
        AuthorityRequested ||
        ActionRequested ||
        LispEvaluationRequested ||
        PacketEmissionRequested ||
        ReceiptReplayRequested ||
        PassageIncrementRequested ||
        ActivationRequested;
}

public sealed record GelDomainScopedIngressReceipt(
    string ReceiptHandle,
    GelDomainScopedIngressDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceEppsReceiptHandle,
    string SourceBridgeReceiptHandle,
    GelIngressCandidateSubstrate? Candidate,
    GelDomainScopeRecord? DomainScope,
    IReadOnlyList<GelIngressCycleTrace> CycleTrace,
    GelIngressStewardReview? StewardReview,
    GelDomainScopedIngressBoundary Boundary,
    GelDomainScopedIngressRefusalReceipt? Refusal,
    int PriorRecommendationCount,
    int RecommendationCountAfterIngress,
    int PriorPassageCount,
    int PassageCountAfterIngress,
    bool ReviewOnly,
    bool CandidateSubstrateRetained,
    bool DomainScoped,
    bool EvidenceCeilingAssigned,
    bool EvidenceCeilingSatisfied,
    bool CoolingPreserved,
    bool StewardRecommendationIssued,
    bool IngressHeld,
    bool DomainClosed,
    bool GovernanceSurvivorshipBecameProof,
    bool DomainFitBecameAdmission,
    bool EvidenceCeilingBecamePortable,
    bool RecommendationBecameAdmission,
    bool GelAdmitted,
    bool MemoryAdmitted,
    bool ContinuityMutated,
    bool SelfGelMutated,
    bool AuthorityGranted,
    bool ActionAuthorized,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    [JsonIgnore]
    public bool IsColdIngressRecommendation =>
        Disposition == GelDomainScopedIngressDisposition.RecommendedCold &&
        Refusal is null &&
        Candidate?.IsColdCandidate == true &&
        DomainScope?.IsColdScope == true &&
        CycleTrace.Count == 10 &&
        CycleTrace.Select(static trace => trace.Stage).Distinct().Count() == 10 &&
        CycleTrace.All(static trace => trace.IsColdTrace) &&
        StewardReview?.IsColdReview == true &&
        StewardReview.CoolingComplete &&
        StewardReview.RecommendationMayIssue &&
        StewardReview.RecommendsIngressConsideration &&
        Boundary.IsColdBoundary &&
        ReviewOnly &&
        CandidateSubstrateRetained &&
        DomainScoped &&
        EvidenceCeilingAssigned &&
        EvidenceCeilingSatisfied &&
        CoolingPreserved &&
        StewardRecommendationIssued &&
        !IngressHeld &&
        !DomainClosed &&
        RecommendationCountAfterIngress == PriorRecommendationCount + 1 &&
        PassageCountAfterIngress == PriorPassageCount &&
        NoForbiddenPromotion;

    [JsonIgnore]
    public bool IsColdIngressHold =>
        Disposition == GelDomainScopedIngressDisposition.Held &&
        Refusal is null &&
        Candidate?.IsColdCandidate == true &&
        DomainScope?.IsColdScope == true &&
        DomainScope.RequiresSpecialCaseHold &&
        DomainScope.SpecialCaseHeld &&
        CycleTrace.All(static trace => trace.IsColdTrace) &&
        StewardReview?.IsColdReview == true &&
        Boundary.IsColdBoundary &&
        ReviewOnly &&
        CandidateSubstrateRetained &&
        DomainScoped &&
        EvidenceCeilingAssigned &&
        CoolingPreserved &&
        IngressHeld &&
        !StewardRecommendationIssued &&
        RecommendationCountAfterIngress == PriorRecommendationCount &&
        PassageCountAfterIngress == PriorPassageCount &&
        NoForbiddenPromotion;

    [JsonIgnore]
    public bool IsRetainedIngressRefusal =>
        Disposition == GelDomainScopedIngressDisposition.Refused &&
        Refusal?.Retained == true &&
        RecommendationCountAfterIngress == PriorRecommendationCount &&
        PassageCountAfterIngress == PriorPassageCount &&
        NoForbiddenPromotion;

    private bool NoForbiddenPromotion =>
        !GovernanceSurvivorshipBecameProof &&
        !DomainFitBecameAdmission &&
        !EvidenceCeilingBecamePortable &&
        !RecommendationBecameAdmission &&
        !GelAdmitted &&
        !MemoryAdmitted &&
        !ContinuityMutated &&
        !SelfGelMutated &&
        !AuthorityGranted &&
        !ActionAuthorized &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        ActivationRefused;
}

public sealed class DefaultGelDomainScopedIngressBoundaryValidator
{
    public GelDomainScopedIngressReceipt Declare(
        GelDomainScopedIngressRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceEppsReceipt is null || !request.SourceEppsReceipt.IsColdPrecursorStream)
        {
            return Refuse(
                request,
                "gel-domain-ingress-source-epps-not-cold",
                "GEL domain-scoped ingress refused because candidate substrate may approach ingress only from a cold EPPS receipt.",
                timestampUtc);
        }

        if (request.SourceBridgeReceipt is null ||
            !request.SourceBridgeReceipt.IsColdPeerReviewBridge ||
            !string.Equals(request.SourceBridgeReceipt.SourceEppsReceiptHandle, request.SourceEppsReceipt.ReceiptHandle, StringComparison.Ordinal))
        {
            return Refuse(
                request,
                "gel-domain-ingress-source-bridge-not-cold",
                "GEL domain-scoped ingress refused because a cold peer-review bridge sourced from the same EPPS receipt is required before candidate substrate may be scoped.",
                timestampUtc);
        }

        if (!request.Boundary.IsColdBoundary)
        {
            return Refuse(
                request,
                "gel-domain-ingress-boundary-promotional",
                "GEL domain-scoped ingress refused because the boundary must require cold EPPS, cold bridge, candidate substrate, domain scope, evidence ceiling, cooling, and Steward review while refusing governance-survivorship-as-proof, domain-fit-as-admission, evidence portability, recommendation-as-admission, GEL, memory, continuity, SelfGEL mutation, authority, action, Lisp evaluation, packet emission, replay, passage, and activation.",
                timestampUtc);
        }

        if (request.RequestsForbiddenMotion)
        {
            return Refuse(
                request,
                "gel-domain-ingress-forbidden-motion-requested",
                "GEL domain-scoped ingress refused because repeated recommendation, ingress request, or review pressure attempted to create warrant, GEL admission, memory, continuity mutation, SelfGEL mutation, authority, action, Lisp evaluation, packet emission, replay, passage, or activation.",
                timestampUtc);
        }

        if (!IsCandidateMapped(request))
        {
            return Refuse(
                request,
                "gel-domain-ingress-candidate-not-cold",
                "GEL domain-scoped ingress refused because candidate substrate must map known EPPS residues and peer-review bridge segments while remaining post-formation, pre-admission, candidate-only, review-only, and non-authorizing.",
                timestampUtc);
        }

        if (!request.DomainScope.IsColdScope)
        {
            return Refuse(
                request,
                "gel-domain-ingress-scope-not-cold",
                "GEL domain-scoped ingress refused because the domain scope must assign a local evidence ceiling, cooling requirement, Steward review requirement, and loss condition without making evidence portable, domain fit into admission, memory, continuity, authority, or action.",
                timestampUtc);
        }

        if (!IsCycleTraceCold(request))
        {
            return Refuse(
                request,
                "gel-domain-ingress-cycle-trace-invalid",
                "GEL domain-scoped ingress refused because the ingress cycle must preserve source, telemetry, EPPS, bridge, candidate, domain, evidence ceiling, cooling, Steward review, and recommendation stages without mutation, continuity, authority, or action.",
                timestampUtc);
        }

        if (!request.StewardReview.IsColdReview)
        {
            return Refuse(
                request,
                "gel-domain-ingress-steward-review-promotional",
                "GEL domain-scoped ingress refused because Steward review may recommend or hold ingress only; it may not perform admission, admit GEL, admit memory, mutate continuity, grant authority, authorize action, evaluate Lisp, emit packets, replay receipts, increment passage, or activate.",
                timestampUtc);
        }

        if (request.DomainScope.DomainClosed)
        {
            return Refuse(
                request,
                "gel-domain-ingress-domain-closed",
                "GEL domain-scoped ingress refused because closed domains may not inherit ordinary ingress, review recommendation, GEL admission, continuity, authority, or action.",
                timestampUtc);
        }

        if (!EvidenceCeilingSatisfiesDomain(request.DomainScope.Domain, request.DomainScope.EvidenceCeiling))
        {
            return Refuse(
                request,
                "gel-domain-ingress-evidence-ceiling-insufficient",
                "GEL domain-scoped ingress refused because the candidate evidence ceiling does not satisfy the scoped domain's local burden. Evidence standards are not portable across worlds.",
                timestampUtc);
        }

        if (request.DomainScope.RequiresSpecialCaseHold)
        {
            return CreateReceipt(
                request,
                GelDomainScopedIngressDisposition.Held,
                "gel-domain-ingress-special-case-held",
                "GEL domain-scoped ingress held because Special Case and personification-facing substrate may remain visible for Steward custody, but may not become ingress recommendation, GEL admission, memory, continuity, authority, action, Lisp evaluation, packet emission, replay, passage, or activation.",
                refusal: null,
                evidenceSatisfied: true,
                recommendationIssued: false,
                ingressHeld: true,
                timestampUtc);
        }

        if (!request.StewardReview.CoolingComplete ||
            !request.StewardReview.RecommendationMayIssue ||
            !request.StewardReview.RecommendsIngressConsideration)
        {
            return Refuse(
                request,
                "gel-domain-ingress-recommendation-not-cold",
                "GEL domain-scoped ingress refused because ordinary ingress recommendation requires completed cooling and Steward recommendation while preserving admission as an external later gate.",
                timestampUtc);
        }

        return CreateReceipt(
            request,
            GelDomainScopedIngressDisposition.RecommendedCold,
            "gel-domain-ingress-recommended-cold",
            "GEL domain-scoped ingress recommended candidate substrate for later external review under a local domain and evidence ceiling while refusing governance survivorship as proof, domain fit as admission, recommendation as continuity mutation, GEL, memory, SelfGEL, authority, action, Lisp evaluation, packet emission, replay, passage, and activation.",
            refusal: null,
            evidenceSatisfied: true,
            recommendationIssued: true,
            ingressHeld: false,
            timestampUtc);
    }

    public static IReadOnlyList<GelIngressCycleTrace> CreateCycleTrace(
        EngramPredicatePrecursorStreamReceipt epps,
        PeerReviewPredicateBridgeReceipt bridge,
        GelIngressCandidateSubstrate candidate,
        GelDomainScopeRecord scope,
        GelIngressStewardReview stewardReview) =>
    [
        CreateTrace(GelIngressCycleStage.SourceEvent, epps.SourceRiderReceiptHandle, "source event remains witness input"),
        CreateTrace(GelIngressCycleStage.TelemetryPrecipitation, epps.ReceiptHandle, "telemetry precipitated into reviewable EPPS residue"),
        CreateTrace(GelIngressCycleStage.EppsResidue, string.Join("|", epps.Residues.Select(static residue => residue.ResidueHandle)), "EPPS residue stays pre-engram"),
        CreateTrace(GelIngressCycleStage.BridgeSynthesis, bridge.ReceiptHandle, "peer-review bridge synthesis remains review-only"),
        CreateTrace(GelIngressCycleStage.CandidateSubstrate, candidate.CandidateHandle, "candidate substrate is formed but not admitted"),
        CreateTrace(GelIngressCycleStage.DomainClassification, scope.ScopeHandle, "domain classification assigns a local world"),
        CreateTrace(GelIngressCycleStage.EvidenceCeilingAssignment, scope.EvidenceCeiling.ToString(), "evidence ceiling remains domain-local"),
        CreateTrace(GelIngressCycleStage.Cooling, scope.LossCondition, "cooling preserves refusal and loss conditions"),
        CreateTrace(GelIngressCycleStage.StewardReview, stewardReview.ReviewHandle, "Steward review may recommend but may not admit"),
        CreateTrace(GelIngressCycleStage.Recommendation, stewardReview.StewardTrace, "recommendation remains external to GEL admission")
    ];

    private static GelIngressCycleTrace CreateTrace(
        GelIngressCycleStage stage,
        string sourceHandle,
        string summary) =>
        new(
            TraceHandle: $"urn:san:gel-domain-ingress-trace:{stage.ToString().ToLowerInvariant()}:{ShortHash(sourceHandle, summary)}",
            Stage: stage,
            SourceHandle: sourceHandle,
            Summary: summary,
            ReviewOnly: true,
            MutatesGel: false,
            AdmitsContinuity: false,
            GrantsAuthority: false,
            AuthorizesAction: false);

    private static bool IsCandidateMapped(GelDomainScopedIngressRequest request)
    {
        if (!request.Candidate.IsColdCandidate ||
            request.SourceEppsReceipt is null ||
            request.SourceBridgeReceipt is null ||
            !string.Equals(request.Candidate.SourceEppsReceiptHandle, request.SourceEppsReceipt.ReceiptHandle, StringComparison.Ordinal) ||
            !string.Equals(request.Candidate.SourceBridgeReceiptHandle, request.SourceBridgeReceipt.ReceiptHandle, StringComparison.Ordinal))
        {
            return false;
        }

        var residueHandles = request.SourceEppsReceipt.Residues
            .Select(static residue => residue.ResidueHandle)
            .ToHashSet(StringComparer.Ordinal);
        var segmentHandles = request.SourceBridgeReceipt.Segments
            .Select(static segment => segment.SegmentHandle)
            .ToHashSet(StringComparer.Ordinal);

        return request.Candidate.SourceResidueHandles.All(residueHandles.Contains) &&
            request.Candidate.SourceBridgeSegmentHandles.All(segmentHandles.Contains);
    }

    private static bool IsCycleTraceCold(GelDomainScopedIngressRequest request) =>
        request.CycleTrace.Count == 10 &&
        request.CycleTrace.Select(static trace => trace.Stage).Distinct().Count() == 10 &&
        request.CycleTrace.All(static trace => trace.IsColdTrace);

    private static bool EvidenceCeilingSatisfiesDomain(
        GelIngressDomain domain,
        GelIngressEvidenceCeiling ceiling) =>
        domain switch
        {
            GelIngressDomain.ScholarlyReview => ceiling is GelIngressEvidenceCeiling.Interpretive or GelIngressEvidenceCeiling.Operational or GelIngressEvidenceCeiling.Reproducible,
            GelIngressDomain.OperatorDoctrine => ceiling is GelIngressEvidenceCeiling.Interpretive or GelIngressEvidenceCeiling.Operational or GelIngressEvidenceCeiling.Reproducible,
            GelIngressDomain.Pedagogy => ceiling is GelIngressEvidenceCeiling.Operational or GelIngressEvidenceCeiling.Reproducible,
            GelIngressDomain.CivicGovernance => ceiling is GelIngressEvidenceCeiling.Reproducible or GelIngressEvidenceCeiling.Regulated,
            GelIngressDomain.EngineeringTelemetry => ceiling is GelIngressEvidenceCeiling.Reproducible or GelIngressEvidenceCeiling.Regulated,
            GelIngressDomain.Security => ceiling is GelIngressEvidenceCeiling.Reproducible or GelIngressEvidenceCeiling.Regulated,
            GelIngressDomain.LegalCompliance => ceiling is GelIngressEvidenceCeiling.Licensed or GelIngressEvidenceCeiling.Regulated,
            GelIngressDomain.MedicalClinical => ceiling == GelIngressEvidenceCeiling.Clinical,
            GelIngressDomain.Personification => ceiling == GelIngressEvidenceCeiling.SpecialCaseHeld,
            GelIngressDomain.SpecialCase => ceiling == GelIngressEvidenceCeiling.SpecialCaseHeld,
            GelIngressDomain.MilitaryDefenseClosed => false,
            _ => false
        };

    private static GelDomainScopedIngressReceipt Refuse(
        GelDomainScopedIngressRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            GelDomainScopedIngressDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new GelDomainScopedIngressRefusalReceipt(
                ReceiptHandle: $"urn:san:gel-domain-ingress-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            evidenceSatisfied: false,
            recommendationIssued: false,
            ingressHeld: false,
            timestampUtc);

    private static GelDomainScopedIngressReceipt CreateReceipt(
        GelDomainScopedIngressRequest request,
        GelDomainScopedIngressDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        GelDomainScopedIngressRefusalReceipt? refusal,
        bool evidenceSatisfied,
        bool recommendationIssued,
        bool ingressHeld,
        DateTimeOffset timestampUtc)
    {
        var retained = refusal is null;
        return new GelDomainScopedIngressReceipt(
            ReceiptHandle: $"urn:san:gel-domain-ingress:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceEppsReceiptHandle: request.SourceEppsReceipt?.ReceiptHandle ?? "missing-epps-source",
            SourceBridgeReceiptHandle: request.SourceBridgeReceipt?.ReceiptHandle ?? "missing-bridge-source",
            Candidate: retained ? request.Candidate : null,
            DomainScope: retained ? request.DomainScope : null,
            CycleTrace: retained ? request.CycleTrace.ToArray() : [],
            StewardReview: retained ? request.StewardReview : null,
            Boundary: request.Boundary,
            Refusal: refusal,
            PriorRecommendationCount: request.PriorRecommendationCount,
            RecommendationCountAfterIngress: recommendationIssued ? request.PriorRecommendationCount + 1 : request.PriorRecommendationCount,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterIngress: request.PriorPassageCount,
            ReviewOnly: true,
            CandidateSubstrateRetained: retained,
            DomainScoped: retained,
            EvidenceCeilingAssigned: retained,
            EvidenceCeilingSatisfied: evidenceSatisfied,
            CoolingPreserved: retained,
            StewardRecommendationIssued: recommendationIssued,
            IngressHeld: ingressHeld,
            DomainClosed: request.DomainScope.DomainClosed,
            GovernanceSurvivorshipBecameProof: false,
            DomainFitBecameAdmission: false,
            EvidenceCeilingBecamePortable: false,
            RecommendationBecameAdmission: false,
            GelAdmitted: false,
            MemoryAdmitted: false,
            ContinuityMutated: false,
            SelfGelMutated: false,
            AuthorityGranted: false,
            ActionAuthorized: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static string SourceHandle(GelDomainScopedIngressRequest request) =>
        request.SourceBridgeReceipt?.ReceiptHandle ??
        request.SourceEppsReceipt?.ReceiptHandle ??
        request.Candidate.CandidateHandle ??
        "missing-source";

    private static string ShortHash(params string[] parts)
    {
        var payload = string.Join("|", parts);
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(bytes).ToLowerInvariant()[..16];
    }
}
