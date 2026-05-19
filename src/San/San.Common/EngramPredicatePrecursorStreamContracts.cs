using System.Text.Json.Serialization;

namespace San.Common;

public enum EngramPredicatePrecursorStreamDisposition
{
    EmittedCold = 0,
    Refused = 1
}

public enum EngramPredicateResidueClass
{
    Semantic = 0,
    Pressure = 1,
    Witness = 2,
    Governance = 3,
    Morphology = 4,
    Return = 5
}

public sealed record EngramPredicatePressureVector(
    decimal SemanticDensity,
    decimal SaliencePressure,
    decimal CoherencePressure,
    decimal AmbiguityPressure,
    decimal UrgencyPressure,
    decimal GovernanceFriction,
    decimal MorphologyDeformation,
    decimal ReturnCooling)
{
    public bool IsColdVector =>
        IsUnit(SemanticDensity) &&
        IsUnit(SaliencePressure) &&
        IsUnit(CoherencePressure) &&
        IsUnit(AmbiguityPressure) &&
        IsUnit(UrgencyPressure) &&
        IsUnit(GovernanceFriction) &&
        IsUnit(MorphologyDeformation) &&
        IsUnit(ReturnCooling);

    public decimal MaximumPressure => new[]
    {
        SemanticDensity,
        SaliencePressure,
        CoherencePressure,
        AmbiguityPressure,
        UrgencyPressure,
        GovernanceFriction,
        MorphologyDeformation,
        ReturnCooling
    }.Max();

    private static bool IsUnit(decimal value) => value is >= 0m and <= 1m;
}

public sealed record EngramPredicateResidue(
    string ResidueHandle,
    EngramPredicateResidueClass ResidueClass,
    string SourceStageId,
    string SourceBoundaryCellId,
    string PredicateCode,
    string EvidenceHandle,
    string WitnessHandle,
    EngramPredicatePressureVector PressureVector,
    bool ReviewOnly,
    bool IsPreEngram,
    bool RequiresCandidacyReview,
    bool CoolingRequired,
    bool IsContinuityBearing,
    bool IsAdmittedEngram,
    bool IsActionAuthorizing,
    bool IsMemoryAdmitting,
    bool IsAuthorityGranting,
    bool AdmitsSelfGel,
    bool EvaluatesLisp,
    bool EmitsPacket,
    bool IncrementsPassage,
    bool Activates)
{
    public bool IsColdResidue =>
        !string.IsNullOrWhiteSpace(ResidueHandle) &&
        !string.IsNullOrWhiteSpace(SourceStageId) &&
        !string.IsNullOrWhiteSpace(SourceBoundaryCellId) &&
        !string.IsNullOrWhiteSpace(PredicateCode) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        PressureVector.IsColdVector &&
        ReviewOnly &&
        IsPreEngram &&
        RequiresCandidacyReview &&
        CoolingRequired &&
        !IsContinuityBearing &&
        !IsAdmittedEngram &&
        !IsActionAuthorizing &&
        !IsMemoryAdmitting &&
        !IsAuthorityGranting &&
        !AdmitsSelfGel &&
        !EvaluatesLisp &&
        !EmitsPacket &&
        !IncrementsPassage &&
        !Activates;
}

public sealed record PredicateWitnessRoute(
    string RouteHandle,
    string SourceRiderReceiptHandle,
    IReadOnlyList<string> StageIds,
    bool ReviewOnly,
    bool PreservesRiderLineage,
    bool SeparateCustody,
    bool GrantsAuthority,
    bool AdmitsContinuity,
    bool AuthorizesAction,
    bool AdmitsMemory)
{
    public bool IsColdWitnessRoute =>
        !string.IsNullOrWhiteSpace(RouteHandle) &&
        !string.IsNullOrWhiteSpace(SourceRiderReceiptHandle) &&
        StageIds.Count > 0 &&
        StageIds.All(static stageId => !string.IsNullOrWhiteSpace(stageId)) &&
        ReviewOnly &&
        PreservesRiderLineage &&
        SeparateCustody &&
        !GrantsAuthority &&
        !AdmitsContinuity &&
        !AuthorizesAction &&
        !AdmitsMemory;
}

public sealed record PredicateRefusalCoolingMarker(
    string MarkerHandle,
    string ResidueHandle,
    string MarkerCode,
    string CoolingRoute,
    string RefusalCode,
    bool RetainedAsResidue,
    bool ReviewOnly,
    bool RequiresCooling,
    bool GrantsAuthority,
    bool AdmitsContinuity,
    bool AuthorizesAction,
    bool AdmitsMemory)
{
    public bool IsColdMarker =>
        !string.IsNullOrWhiteSpace(MarkerHandle) &&
        !string.IsNullOrWhiteSpace(ResidueHandle) &&
        !string.IsNullOrWhiteSpace(MarkerCode) &&
        !string.IsNullOrWhiteSpace(CoolingRoute) &&
        !string.IsNullOrWhiteSpace(RefusalCode) &&
        RetainedAsResidue &&
        ReviewOnly &&
        RequiresCooling &&
        !GrantsAuthority &&
        !AdmitsContinuity &&
        !AuthorizesAction &&
        !AdmitsMemory;
}

public sealed record PredicateCandidacyGate(
    string GateHandle,
    string SourceRiderReceiptHandle,
    int ResidueCount,
    bool Present,
    bool ReviewOnly,
    bool CandidateMaterialAvailable,
    bool CandidacyReviewRequired,
    bool GateClosed,
    bool AdmitsEngram,
    bool AdmitsMemory,
    bool AdmitsContinuity,
    bool GrantsAuthority,
    bool AuthorizesAction,
    bool PromotesSelfGel)
{
    public bool IsColdGate =>
        !string.IsNullOrWhiteSpace(GateHandle) &&
        !string.IsNullOrWhiteSpace(SourceRiderReceiptHandle) &&
        ResidueCount > 0 &&
        Present &&
        ReviewOnly &&
        CandidateMaterialAvailable &&
        CandidacyReviewRequired &&
        GateClosed &&
        !AdmitsEngram &&
        !AdmitsMemory &&
        !AdmitsContinuity &&
        !GrantsAuthority &&
        !AuthorizesAction &&
        !PromotesSelfGel;
}

public sealed record EngramPredicatePrecursorStreamReceipt(
    string ReceiptHandle,
    EngramPredicatePrecursorStreamDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceRiderReceiptHandle,
    string ThoughtForm,
    IReadOnlyList<EngramPredicateResidue> Residues,
    PredicateWitnessRoute WitnessRoute,
    IReadOnlyList<PredicateRefusalCoolingMarker> RefusalCoolingMarkers,
    PredicateCandidacyGate CandidacyGate,
    int PriorPassageCount,
    int PassageCountAfterStream,
    bool ReviewOnly,
    bool PreEngramOnly,
    bool ResidueProofOnly,
    bool StreamAdmitsEngram,
    bool StreamAdmitsMemory,
    bool StreamAdmitsContinuity,
    bool StreamAuthorizesAction,
    bool StreamGrantsAuthority,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    [JsonIgnore]
    public bool IsColdPrecursorStream =>
        Disposition == EngramPredicatePrecursorStreamDisposition.EmittedCold &&
        Residues.Count == 6 &&
        Residues.Select(static residue => residue.ResidueClass).Distinct().Count() == 6 &&
        Residues.All(static residue => residue.IsColdResidue) &&
        WitnessRoute.IsColdWitnessRoute &&
        RefusalCoolingMarkers.Count == Residues.Count &&
        RefusalCoolingMarkers.All(static marker => marker.IsColdMarker) &&
        CandidacyGate.IsColdGate &&
        PassageCountAfterStream == PriorPassageCount &&
        ReviewOnly &&
        PreEngramOnly &&
        ResidueProofOnly &&
        !StreamAdmitsEngram &&
        !StreamAdmitsMemory &&
        !StreamAdmitsContinuity &&
        !StreamAuthorizesAction &&
        !StreamGrantsAuthority &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        ActivationRefused;
}
