namespace San.Common;

public static class SanctuaryPacketSurfaces
{
    public const string Prime = "Prime";
    public const string Cryptic = "Cryptic";
    public const string Steward = "Steward";
    public const string Compass = "Compass";
    public const string CGoa = "cGoA";
    public const string Telemetry = "Telemetry";
    public const string SliLisp = "SLI.Lisp";
}

public static class SanctuaryPacketRoutes
{
    public const string CGoaInsulated = "cGoA-insulated";
    public const string TelemetryString = "telemetry-string";
    public const string Direct = "direct";
}

public enum SanctuaryPacketValidationDisposition
{
    AcceptedCold = 0,
    Refused = 1
}

public enum PacketReceiptRoutingDisposition
{
    RoutedPassageCold = 0,
    RoutedRefusalCold = 1
}

public enum ReceiptReplayDisposition
{
    ReplayedForReviewCold = 0,
    Refused = 1
}

public enum ReceiptQueryDisposition
{
    LocatedForReviewCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public enum ReceiptSelectionDisposition
{
    NominatedForReviewCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public enum WitnessSummaryDisposition
{
    SummarizedForReviewCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public enum CompassPressureDisposition
{
    PressurizedForReviewCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public enum EngramCandidateDisposition
{
    NominatedForReviewCold = 0,
    Refused = 1
}

public sealed record MembraneAddress(
    string SourceSurface,
    string TargetSurface,
    string Route);

public sealed record AuthorityCeiling(
    string CeilingCode,
    bool MayAuthorize,
    bool MayPromoteContinuity,
    bool MayActivate);

public sealed record CustodyEnvelope(
    string CustodyOwner,
    string RevocationPath,
    IReadOnlyList<string> WitnessRefs);

public sealed record TelemetryString(
    string TraceId,
    string Route,
    bool AttemptsAuthority);

public sealed record WitnessReceipt(
    string ReceiptHandle,
    string WitnessSurface,
    bool SeparateCustody);

public sealed record RefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record CompassShellPacket(
    bool ClaimsEngram,
    bool ClaimsTruth,
    bool ClaimsAuthority);

public sealed record CleavingDecisionReceipt(
    string ReceiptHandle,
    string Decision,
    bool CandidateOnly,
    bool ContinuityAdmitted);

public sealed record SanctuaryPacket(
    string PacketHandle,
    string PacketKind,
    MembraneAddress Address,
    AuthorityCeiling AuthorityCeiling,
    CustodyEnvelope CustodyEnvelope,
    TelemetryString? Telemetry,
    WitnessReceipt? Witness,
    CompassShellPacket? CompassShell,
    bool AttemptsRuntimeAction,
    bool AttemptsActivation,
    bool AttemptsContinuityPromotion,
    bool AttemptsSelfAuthorization);

public sealed record PacketValidationReceipt(
    string ReceiptHandle,
    SanctuaryPacketValidationDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string PacketHandle,
    RefusalReceipt? Refusal,
    bool ActivationRefused,
    bool RuntimeActionAllowed,
    bool AuthorityGranted,
    bool ContinuityAdmitted,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdAccepted =>
        Disposition == SanctuaryPacketValidationDisposition.AcceptedCold &&
        Refusal is null &&
        ActivationRefused &&
        !RuntimeActionAllowed &&
        !AuthorityGranted &&
        !ContinuityAdmitted;

    public bool IsRetainedRefusal =>
        Disposition == SanctuaryPacketValidationDisposition.Refused &&
        Refusal?.Retained == true &&
        ActivationRefused &&
        !RuntimeActionAllowed &&
        !AuthorityGranted &&
        !ContinuityAdmitted;
}

public sealed record MembraneWitnessRoute(
    string WitnessSurface,
    string CustodySurface,
    bool SeparateCustody,
    bool StewardWitnessPresent);

public sealed record CustodyRetentionRoute(
    string CustodyOwner,
    string RetentionClass,
    bool Retained,
    string RevocationPath);

public sealed record PacketTelemetryObservation(
    string TraceId,
    string ObservedRoute,
    bool AttemptsAuthority,
    bool GrantsAuthority);

public sealed record ReceiptAuthorityBoundary(
    bool ReceiptMayAuthorizeFuturePacket,
    bool ReceiptMayAdmitContinuity,
    bool ReceiptMayActivate,
    string BoundaryLaw);

public sealed record PacketRevocationPath(
    string PathCode,
    bool Present,
    bool MayUndoHistory,
    bool MayRevokeFutureUse);

public sealed record PacketPassageReceipt(
    string ReceiptHandle,
    string PacketHandle,
    MembraneWitnessRoute WitnessRoute,
    CustodyRetentionRoute CustodyRoute,
    PacketTelemetryObservation TelemetryObservation,
    PacketRevocationPath RevocationPath,
    bool ProvesPassage,
    bool GrantsPermission);

public sealed record PacketRefusalRoutingReceipt(
    string ReceiptHandle,
    string PacketHandle,
    RefusalReceipt Refusal,
    MembraneWitnessRoute WitnessRoute,
    CustodyRetentionRoute CustodyRoute,
    PacketTelemetryObservation TelemetryObservation,
    bool RetainsRefusal,
    bool GrantsPermission);

public sealed record PacketReceiptRoutingReceipt(
    string ReceiptHandle,
    PacketReceiptRoutingDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string PacketHandle,
    PacketValidationReceipt ValidationReceipt,
    PacketPassageReceipt? Passage,
    PacketRefusalRoutingReceipt? RefusalRouting,
    ReceiptAuthorityBoundary AuthorityBoundary,
    bool ActivationRefused,
    bool AuthorityGranted,
    bool ContinuityAdmitted,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdPassageRoute =>
        Disposition == PacketReceiptRoutingDisposition.RoutedPassageCold &&
        Passage is { ProvesPassage: true, GrantsPermission: false } &&
        RefusalRouting is null &&
        ActivationRefused &&
        !AuthorityGranted &&
        !ContinuityAdmitted &&
        AuthorityBoundary is
        {
            ReceiptMayAuthorizeFuturePacket: false,
            ReceiptMayAdmitContinuity: false,
            ReceiptMayActivate: false
        };

    public bool IsColdRefusalRoute =>
        Disposition == PacketReceiptRoutingDisposition.RoutedRefusalCold &&
        RefusalRouting is { RetainsRefusal: true, GrantsPermission: false } &&
        Passage is null &&
        ActivationRefused &&
        !AuthorityGranted &&
        !ContinuityAdmitted &&
        AuthorityBoundary is
        {
            ReceiptMayAuthorizeFuturePacket: false,
            ReceiptMayAdmitContinuity: false,
            ReceiptMayActivate: false
        };
}

public sealed record ReceiptReplaySurface(
    string SurfaceName,
    bool ReviewOnly);

public sealed record ReplayWitnessContext(
    string WitnessSurface,
    bool WitnessPresent,
    bool SeparateCustody);

public sealed record ReplayScopeBoundary(
    string ScopeCode,
    bool Present,
    bool ReviewOnly,
    bool AllowsReentry);

public sealed record ReplayNonReentryBoundary(
    bool EmitsNewPacket,
    bool RepeatsPassage,
    bool IncrementsPassageCount,
    bool AuthorizesFuturePacket,
    bool AdmitsContinuity,
    string BoundaryLaw);

public sealed record ReceiptReplayRequest(
    string ReplayHandle,
    PacketReceiptRoutingReceipt? OriginalReceipt,
    ReceiptReplaySurface ReplaySurface,
    ReplayWitnessContext WitnessContext,
    ReplayScopeBoundary ScopeBoundary,
    int PriorPassageCount);

public sealed record ReplayRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record ReceiptReplayReceipt(
    string ReceiptHandle,
    ReceiptReplayDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string ReplayHandle,
    string? OriginalReceiptHandle,
    string? OriginalPacketHandle,
    PacketReceiptRoutingDisposition? OriginalDisposition,
    ReplayNonReentryBoundary NonReentryBoundary,
    ReplayRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterReplay,
    bool ReviewOnly,
    bool NewPacketEmitted,
    bool NewPassageCreated,
    bool ActivationRefused,
    bool AuthorityGranted,
    bool ContinuityAdmitted,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdReplay =>
        Disposition == ReceiptReplayDisposition.ReplayedForReviewCold &&
        Refusal is null &&
        ReviewOnly &&
        PassageCountAfterReplay == PriorPassageCount &&
        !NewPacketEmitted &&
        !NewPassageCreated &&
        ActivationRefused &&
        !AuthorityGranted &&
        !ContinuityAdmitted &&
        NonReentryBoundary is
        {
            EmitsNewPacket: false,
            RepeatsPassage: false,
            IncrementsPassageCount: false,
            AuthorizesFuturePacket: false,
            AdmitsContinuity: false
        };

    public bool IsRetainedReplayRefusal =>
        Disposition == ReceiptReplayDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterReplay == PriorPassageCount &&
        !NewPacketEmitted &&
        !NewPassageCreated &&
        ActivationRefused &&
            !AuthorityGranted &&
            !ContinuityAdmitted;
}

public sealed record QueryWitnessContext(
    string WitnessSurface,
    bool WitnessPresent,
    bool SeparateCustody);

public sealed record ReceiptQueryScopeBoundary(
    string ScopeCode,
    bool Present,
    bool ReviewOnly,
    bool AllowsWarrant);

public sealed record ReceiptQueryFilter(
    string? PacketHandle,
    PacketReceiptRoutingDisposition? Disposition,
    string? OutcomeCode);

public sealed record ReceiptQueryRequest(
    string QueryHandle,
    IReadOnlyList<PacketReceiptRoutingReceipt> RetainedReceipts,
    ReceiptQueryFilter Filter,
    QueryWitnessContext WitnessContext,
    ReceiptQueryScopeBoundary ScopeBoundary,
    int PriorPassageCount);

public sealed record ReceiptQueryEvidenceHandle(
    string OriginalReceiptHandle,
    string OriginalPacketHandle,
    PacketReceiptRoutingDisposition OriginalDisposition,
    string OriginalOutcomeCode,
    bool PreservesOriginalReceiptHandle);

public sealed record ReceiptQueryNonWarrantBoundary(
    bool FoundReceiptMayAuthorize,
    bool AggregateCountMayAuthorize,
    bool QuerySummaryMayAuthorize,
    bool QuerySummaryMayAdmitContinuity,
    bool QueryReplaysReceipts,
    bool QueryCreatesNewEvidenceReceiptHandles,
    bool IncrementsPassageCount,
    bool EmitsNewPacket,
    string BoundaryLaw);

public sealed record QueryRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record ReceiptQueryReceipt(
    string ReceiptHandle,
    ReceiptQueryDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string QueryHandle,
    IReadOnlyList<ReceiptQueryEvidenceHandle> Evidence,
    ReceiptQueryNonWarrantBoundary NonWarrantBoundary,
    QueryRefusalReceipt? Refusal,
    int AggregateCount,
    int PriorPassageCount,
    int PassageCountAfterQuery,
    bool ReviewOnly,
    bool QuerySummaryGrantsAuthority,
    bool QuerySummaryAdmitsContinuity,
    bool ReceiptsReplayed,
    bool NewPacketEmitted,
    bool ActivationRefused,
    bool AuthorityGranted,
    bool ContinuityAdmitted,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdQuery =>
        Disposition is ReceiptQueryDisposition.LocatedForReviewCold or ReceiptQueryDisposition.EmptyReviewCold &&
        Refusal is null &&
        ReviewOnly &&
        AggregateCount == Evidence.Count &&
        PassageCountAfterQuery == PriorPassageCount &&
        Evidence.All(static evidence => evidence.PreservesOriginalReceiptHandle) &&
        !QuerySummaryGrantsAuthority &&
        !QuerySummaryAdmitsContinuity &&
        !ReceiptsReplayed &&
        !NewPacketEmitted &&
        ActivationRefused &&
        !AuthorityGranted &&
        !ContinuityAdmitted &&
        NonWarrantBoundary is
        {
            FoundReceiptMayAuthorize: false,
            AggregateCountMayAuthorize: false,
            QuerySummaryMayAuthorize: false,
            QuerySummaryMayAdmitContinuity: false,
            QueryReplaysReceipts: false,
            QueryCreatesNewEvidenceReceiptHandles: false,
            IncrementsPassageCount: false,
            EmitsNewPacket: false
        };

    public bool IsRetainedQueryRefusal =>
        Disposition == ReceiptQueryDisposition.Refused &&
        Refusal?.Retained == true &&
        AggregateCount == 0 &&
        PassageCountAfterQuery == PriorPassageCount &&
        !QuerySummaryGrantsAuthority &&
        !QuerySummaryAdmitsContinuity &&
        !ReceiptsReplayed &&
        !NewPacketEmitted &&
        ActivationRefused &&
        !AuthorityGranted &&
        !ContinuityAdmitted;
}

public sealed record SelectionWitnessContext(
    string WitnessSurface,
    bool WitnessPresent,
    bool SeparateCustody);

public sealed record ReceiptSelectionScopeBoundary(
    string ScopeCode,
    bool Present,
    bool ReviewOnly,
    bool AllowsAuthority,
    bool AllowsContinuityAdmission,
    bool AllowsCompassTruth);

public sealed record ReceiptSelectionRequest(
    string SelectionHandle,
    ReceiptQueryReceipt? QueryReceipt,
    IReadOnlyList<string> RequestedOriginalReceiptHandles,
    SelectionWitnessContext WitnessContext,
    ReceiptSelectionScopeBoundary ScopeBoundary,
    int PriorPassageCount);

public sealed record ReceiptSelectionNomination(
    string OriginalReceiptHandle,
    string OriginalPacketHandle,
    PacketReceiptRoutingDisposition OriginalDisposition,
    string OriginalOutcomeCode,
    string SelectionReason,
    bool PreservesOriginalReceiptHandle,
    bool CandidateOnly);

public sealed record ReceiptSelectionNonAdmissionBoundary(
    bool NominationMayAuthorize,
    bool NominationMayAdmitContinuity,
    bool NominationMayBecomeCompassTruth,
    bool NominationMayActivate,
    bool SelectionReplaysReceipts,
    bool SelectionCreatesNewEvidenceReceiptHandles,
    bool IncrementsPassageCount,
    bool EmitsNewPacket,
    string BoundaryLaw);

public sealed record SelectionRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record ReceiptSelectionReceipt(
    string ReceiptHandle,
    ReceiptSelectionDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SelectionHandle,
    string? QueryReceiptHandle,
    IReadOnlyList<ReceiptSelectionNomination> Nominations,
    ReceiptSelectionNonAdmissionBoundary NonAdmissionBoundary,
    SelectionRefusalReceipt? Refusal,
    int NominationCount,
    int PriorPassageCount,
    int PassageCountAfterSelection,
    bool ReviewOnly,
    bool SelectionGrantsAuthority,
    bool SelectionAdmitsContinuity,
    bool SelectionBecomesCompassTruth,
    bool ReceiptsReplayed,
    bool NewPacketEmitted,
    bool ActivationRefused,
    bool AuthorityGranted,
    bool ContinuityAdmitted,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdSelection =>
        Disposition is ReceiptSelectionDisposition.NominatedForReviewCold or ReceiptSelectionDisposition.EmptyReviewCold &&
        Refusal is null &&
        ReviewOnly &&
        NominationCount == Nominations.Count &&
        PassageCountAfterSelection == PriorPassageCount &&
        Nominations.All(static nomination => nomination is { PreservesOriginalReceiptHandle: true, CandidateOnly: true }) &&
        !SelectionGrantsAuthority &&
        !SelectionAdmitsContinuity &&
        !SelectionBecomesCompassTruth &&
        !ReceiptsReplayed &&
        !NewPacketEmitted &&
        ActivationRefused &&
        !AuthorityGranted &&
        !ContinuityAdmitted &&
        NonAdmissionBoundary is
        {
            NominationMayAuthorize: false,
            NominationMayAdmitContinuity: false,
            NominationMayBecomeCompassTruth: false,
            NominationMayActivate: false,
            SelectionReplaysReceipts: false,
            SelectionCreatesNewEvidenceReceiptHandles: false,
            IncrementsPassageCount: false,
            EmitsNewPacket: false
        };

    public bool IsRetainedSelectionRefusal =>
        Disposition == ReceiptSelectionDisposition.Refused &&
        Refusal?.Retained == true &&
        NominationCount == 0 &&
        PassageCountAfterSelection == PriorPassageCount &&
        !SelectionGrantsAuthority &&
        !SelectionAdmitsContinuity &&
        !SelectionBecomesCompassTruth &&
        !ReceiptsReplayed &&
        !NewPacketEmitted &&
        ActivationRefused &&
        !AuthorityGranted &&
        !ContinuityAdmitted;
}

public sealed record SummaryWitnessContext(
    string WitnessSurface,
    bool WitnessPresent,
    bool SeparateCustody);

public sealed record WitnessSummaryScopeBoundary(
    string ScopeCode,
    bool Present,
    bool ReviewOnly,
    bool AllowsAuthority,
    bool AllowsContinuityAdmission,
    bool AllowsEvidenceReplacement,
    bool AllowsCompassTruth);

public sealed record WitnessSummaryArtifactLineage(
    string ArtifactId,
    string CellId,
    string Phase,
    string Layer,
    string SourcePath,
    string Summary,
    bool PreservesLineage);

public sealed record WitnessDoctrinePhrase(
    string Phrase,
    string SourceRef,
    bool EnforcedByTests);

public sealed record WitnessGapCandidate(
    string GapCode,
    string Posture,
    bool BlocksColdBench);

public sealed record WitnessSummaryRequest(
    string SummaryHandle,
    ReceiptSelectionReceipt? SelectionReceipt,
    IReadOnlyList<WitnessSummaryArtifactLineage> ArtifactLineage,
    IReadOnlyList<WitnessDoctrinePhrase> DoctrinePhrases,
    IReadOnlyList<WitnessGapCandidate> GapCandidates,
    SummaryWitnessContext WitnessContext,
    WitnessSummaryScopeBoundary ScopeBoundary,
    decimal ConfidenceEstimate,
    int PriorPassageCount);

public sealed record WitnessSummaryGroup(
    string GroupCode,
    IReadOnlyList<string> ArtifactIds,
    IReadOnlyList<string> OriginalReceiptHandles,
    string SummaryText,
    bool ReviewOnly);

public sealed record WitnessSummaryNonReplacementBoundary(
    bool SummaryMayReplaceEvidence,
    bool SummaryMayAuthorize,
    bool SummaryMayAdmitContinuity,
    bool SummaryMayBecomeCompassTruth,
    bool SummaryCreatesNewEvidenceReceiptHandles,
    bool SummaryReplaysReceipts,
    bool IncrementsPassageCount,
    bool EmitsNewPacket,
    string BoundaryLaw);

public sealed record WitnessSummaryRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record WitnessSummaryReceipt(
    string ReceiptHandle,
    WitnessSummaryDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SummaryHandle,
    string? SelectionReceiptHandle,
    IReadOnlyList<WitnessSummaryGroup> Groups,
    IReadOnlyList<WitnessSummaryArtifactLineage> ArtifactLineage,
    IReadOnlyList<WitnessDoctrinePhrase> DoctrinePhrases,
    IReadOnlyList<WitnessGapCandidate> GapCandidates,
    WitnessSummaryNonReplacementBoundary NonReplacementBoundary,
    WitnessSummaryRefusalReceipt? Refusal,
    decimal ConfidenceEstimate,
    int PriorPassageCount,
    int PassageCountAfterSummary,
    bool ReviewOnly,
    bool SummaryReplacesEvidence,
    bool SummaryGrantsAuthority,
    bool SummaryAdmitsContinuity,
    bool SummaryBecomesCompassTruth,
    bool ReceiptsReplayed,
    bool NewPacketEmitted,
    bool ActivationRefused,
    bool AuthorityGranted,
    bool ContinuityAdmitted,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdSummary =>
        Disposition is WitnessSummaryDisposition.SummarizedForReviewCold or WitnessSummaryDisposition.EmptyReviewCold &&
        Refusal is null &&
        ReviewOnly &&
        PassageCountAfterSummary == PriorPassageCount &&
        ArtifactLineage.All(static artifact => artifact.PreservesLineage) &&
        Groups.All(static group => group.ReviewOnly) &&
        !SummaryReplacesEvidence &&
        !SummaryGrantsAuthority &&
        !SummaryAdmitsContinuity &&
        !SummaryBecomesCompassTruth &&
        !ReceiptsReplayed &&
        !NewPacketEmitted &&
        ActivationRefused &&
        !AuthorityGranted &&
        !ContinuityAdmitted &&
        ConfidenceEstimate is >= 0m and <= 1m &&
        NonReplacementBoundary is
        {
            SummaryMayReplaceEvidence: false,
            SummaryMayAuthorize: false,
            SummaryMayAdmitContinuity: false,
            SummaryMayBecomeCompassTruth: false,
            SummaryCreatesNewEvidenceReceiptHandles: false,
            SummaryReplaysReceipts: false,
            IncrementsPassageCount: false,
            EmitsNewPacket: false
        };

    public bool IsRetainedSummaryRefusal =>
        Disposition == WitnessSummaryDisposition.Refused &&
        Refusal?.Retained == true &&
        Groups.Count == 0 &&
        PassageCountAfterSummary == PriorPassageCount &&
        !SummaryReplacesEvidence &&
        !SummaryGrantsAuthority &&
        !SummaryAdmitsContinuity &&
        !SummaryBecomesCompassTruth &&
        !ReceiptsReplayed &&
        !NewPacketEmitted &&
        ActivationRefused &&
        !AuthorityGranted &&
        !ContinuityAdmitted;
}

public sealed record CompassPressureWitnessContext(
    string WitnessSurface,
    bool WitnessPresent,
    bool SeparateCustody);

public sealed record CompassPressureScopeBoundary(
    string ScopeCode,
    bool Present,
    bool ReviewOnly,
    bool AllowsEngram,
    bool AllowsTruth,
    bool AllowsAuthority,
    bool AllowsContinuityAdmission,
    bool AllowsSelfGelAppend,
    bool AllowsCSelfGelAppend);

public sealed record PreEngramResidue(
    string SummaryReceiptHandle,
    string? SelectionReceiptHandle,
    IReadOnlyList<string> OriginalReceiptHandles,
    IReadOnlyList<WitnessSummaryArtifactLineage> ArtifactLineage,
    bool CandidateOnly,
    bool EngramAdmitted,
    bool SelfGelAppendAllowed,
    bool CSelfGelAppendAllowed);

public sealed record CompassPressureVector(
    decimal EvidenceDensity,
    decimal DoctrinePressure,
    decimal GapPressure,
    decimal Confidence,
    bool Bounded);

public sealed record CompassPressureNonEngramBoundary(
    bool PressureMayBecomeEngram,
    bool PressureMayBecomeTruth,
    bool PressureMayAuthorize,
    bool PressureMayAdmitContinuity,
    bool PressureMayAppendSelfGel,
    bool PressureMayAppendCSelfGel,
    bool PressureReplaysReceipts,
    bool IncrementsPassageCount,
    bool EmitsNewPacket,
    string BoundaryLaw);

public sealed record CompassPressureRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record CompassPressureRequest(
    string PressureHandle,
    WitnessSummaryReceipt? SummaryReceipt,
    CompassPressureWitnessContext WitnessContext,
    CompassPressureScopeBoundary ScopeBoundary,
    int PriorPassageCount);

public sealed record CompassPressureReceipt(
    string ReceiptHandle,
    CompassPressureDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string PressureHandle,
    string? SummaryReceiptHandle,
    PreEngramResidue? Residue,
    CompassPressureVector PressureVector,
    CompassPressureNonEngramBoundary NonEngramBoundary,
    CompassPressureRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterPressure,
    bool ReviewOnly,
    bool CandidateOnly,
    bool PressureBecomesEngram,
    bool PressureBecomesTruth,
    bool PressureGrantsAuthority,
    bool PressureAdmitsContinuity,
    bool PressureAppendsSelfGel,
    bool PressureAppendsCSelfGel,
    bool ReceiptsReplayed,
    bool NewPacketEmitted,
    bool ActivationRefused,
    bool AuthorityGranted,
    bool ContinuityAdmitted,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdPressure =>
        Disposition is CompassPressureDisposition.PressurizedForReviewCold or CompassPressureDisposition.EmptyReviewCold &&
        Refusal is null &&
        Residue is
        {
            CandidateOnly: true,
            EngramAdmitted: false,
            SelfGelAppendAllowed: false,
            CSelfGelAppendAllowed: false
        } &&
        ReviewOnly &&
        CandidateOnly &&
        PassageCountAfterPressure == PriorPassageCount &&
        PressureVector is { Bounded: true, EvidenceDensity: >= 0m and <= 1m, DoctrinePressure: >= 0m and <= 1m, GapPressure: >= 0m and <= 1m, Confidence: >= 0m and <= 1m } &&
        !PressureBecomesEngram &&
        !PressureBecomesTruth &&
        !PressureGrantsAuthority &&
        !PressureAdmitsContinuity &&
        !PressureAppendsSelfGel &&
        !PressureAppendsCSelfGel &&
        !ReceiptsReplayed &&
        !NewPacketEmitted &&
        ActivationRefused &&
        !AuthorityGranted &&
        !ContinuityAdmitted &&
        NonEngramBoundary is
        {
            PressureMayBecomeEngram: false,
            PressureMayBecomeTruth: false,
            PressureMayAuthorize: false,
            PressureMayAdmitContinuity: false,
            PressureMayAppendSelfGel: false,
            PressureMayAppendCSelfGel: false,
            PressureReplaysReceipts: false,
            IncrementsPassageCount: false,
            EmitsNewPacket: false
        };

    public bool IsRetainedPressureRefusal =>
        Disposition == CompassPressureDisposition.Refused &&
        Refusal?.Retained == true &&
        Residue is null &&
        PassageCountAfterPressure == PriorPassageCount &&
        !PressureBecomesEngram &&
        !PressureBecomesTruth &&
        !PressureGrantsAuthority &&
        !PressureAdmitsContinuity &&
        !PressureAppendsSelfGel &&
        !PressureAppendsCSelfGel &&
        !ReceiptsReplayed &&
        !NewPacketEmitted &&
        ActivationRefused &&
        !AuthorityGranted &&
        !ContinuityAdmitted;
}

public sealed record EngramCandidateWitnessContext(
    string WitnessSurface,
    bool WitnessPresent,
    bool SeparateCustody);

public sealed record EngramCandidateScopeBoundary(
    string ScopeCode,
    bool Present,
    bool ReviewOnly,
    bool AllowsEngramAdmission,
    bool AllowsContinuityAdmission,
    bool AllowsAuthority,
    bool AllowsSelfGelAppend,
    bool AllowsCSelfGelAppend,
    bool AllowsRuntimeAction);

public sealed record EngramCandidateSourceBoundary(
    string MembraneLandingCode,
    bool MembraneLandingPresent,
    string ClassificationCode,
    bool ClassificationPresent,
    bool SourceEvidencePresent,
    string TransformationTrace,
    bool TransformationTracePresent,
    string ContinuityRelationCode,
    bool ContinuityRelationPresent);

public sealed record EngramCandidateNonAdmissionBoundary(
    bool CandidateMayBecomeEngram,
    bool CandidateMayAdmitContinuity,
    bool CandidateMayAuthorize,
    bool CandidateMayAppendSelfGel,
    bool CandidateMayAppendCSelfGel,
    bool CandidateMayReplaceEvidence,
    bool CandidateReplaysReceipts,
    bool IncrementsPassageCount,
    bool EmitsNewPacket,
    bool CandidateMayActivate,
    string BoundaryLaw);

public sealed record EngramCandidateEvidenceLineage(
    string PressureReceiptHandle,
    string? SummaryReceiptHandle,
    string? SelectionReceiptHandle,
    IReadOnlyList<string> OriginalReceiptHandles,
    IReadOnlyList<WitnessSummaryArtifactLineage> ArtifactLineage,
    EngramCandidateSourceBoundary SourceBoundary,
    bool PreservesOriginalEvidence,
    bool CandidateOnly);

public sealed record EngramCandidateRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record EngramCandidateReadinessRequest(
    string CandidateHandle,
    CompassPressureReceipt? PressureReceipt,
    EngramCandidateSourceBoundary SourceBoundary,
    EngramCandidateWitnessContext WitnessContext,
    EngramCandidateScopeBoundary ScopeBoundary,
    int PriorPassageCount);

public sealed record EngramCandidateReadinessReceipt(
    string ReceiptHandle,
    EngramCandidateDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string CandidateHandle,
    string? PressureReceiptHandle,
    EngramCandidateEvidenceLineage? EvidenceLineage,
    EngramCandidateNonAdmissionBoundary NonAdmissionBoundary,
    EngramCandidateRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterCandidate,
    bool ReviewOnly,
    bool CandidateOnly,
    bool CandidateNominated,
    bool CandidateBecomesEngram,
    bool CandidateAdmitsContinuity,
    bool CandidateGrantsAuthority,
    bool CandidateAppendsSelfGel,
    bool CandidateAppendsCSelfGel,
    bool CandidateReplacesEvidence,
    bool ReceiptsReplayed,
    bool NewPacketEmitted,
    bool ActivationRefused,
    bool AuthorityGranted,
    bool ContinuityAdmitted,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdCandidate =>
        Disposition == EngramCandidateDisposition.NominatedForReviewCold &&
        Refusal is null &&
        EvidenceLineage is
        {
            PreservesOriginalEvidence: true,
            CandidateOnly: true,
            OriginalReceiptHandles.Count: > 0
        } &&
        ReviewOnly &&
        CandidateOnly &&
        CandidateNominated &&
        PassageCountAfterCandidate == PriorPassageCount &&
        !CandidateBecomesEngram &&
        !CandidateAdmitsContinuity &&
        !CandidateGrantsAuthority &&
        !CandidateAppendsSelfGel &&
        !CandidateAppendsCSelfGel &&
        !CandidateReplacesEvidence &&
        !ReceiptsReplayed &&
        !NewPacketEmitted &&
        ActivationRefused &&
        !AuthorityGranted &&
        !ContinuityAdmitted &&
        NonAdmissionBoundary is
        {
            CandidateMayBecomeEngram: false,
            CandidateMayAdmitContinuity: false,
            CandidateMayAuthorize: false,
            CandidateMayAppendSelfGel: false,
            CandidateMayAppendCSelfGel: false,
            CandidateMayReplaceEvidence: false,
            CandidateReplaysReceipts: false,
            IncrementsPassageCount: false,
            EmitsNewPacket: false,
            CandidateMayActivate: false
        };

    public bool IsRetainedCandidateRefusal =>
        Disposition == EngramCandidateDisposition.Refused &&
        Refusal?.Retained == true &&
        EvidenceLineage is null &&
        PassageCountAfterCandidate == PriorPassageCount &&
        !CandidateNominated &&
        !CandidateBecomesEngram &&
        !CandidateAdmitsContinuity &&
        !CandidateGrantsAuthority &&
        !CandidateAppendsSelfGel &&
        !CandidateAppendsCSelfGel &&
        !CandidateReplacesEvidence &&
        !ReceiptsReplayed &&
        !NewPacketEmitted &&
        ActivationRefused &&
        !AuthorityGranted &&
        !ContinuityAdmitted;
}

public sealed class DefaultSanctuaryPacketMembraneValidator
{
    public PacketValidationReceipt Validate(
        SanctuaryPacket packet,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(packet);

        if (string.IsNullOrWhiteSpace(packet.Address.SourceSurface))
        {
            return Refuse(packet, "packet-source-missing", "Packet refused because source membrane is missing.", timestampUtc);
        }

        if (string.IsNullOrWhiteSpace(packet.Address.TargetSurface))
        {
            return Refuse(packet, "packet-target-missing", "Packet refused because target membrane is missing.", timestampUtc);
        }

        if (packet.AttemptsActivation || packet.AttemptsRuntimeAction)
        {
            return Refuse(packet, "packet-runtime-motion-refused", "Packet refused because cold packet validation cannot carry activation or runtime action.", timestampUtc);
        }

        if (packet.AttemptsContinuityPromotion)
        {
            return Refuse(packet, "packet-continuity-promotion-refused", "Packet refused because packet transit cannot admit continuity by itself.", timestampUtc);
        }

        if (packet.AttemptsSelfAuthorization)
        {
            return Refuse(packet, "packet-self-authorization-refused", "Packet refused because self-witness may report telemetry but may not authorize its own passage.", timestampUtc);
        }

        if (packet.AuthorityCeiling.MayAuthorize)
        {
            return Refuse(packet, "packet-undeclared-authority-refused", "Packet refused because a packet may carry structure but may not carry undeclared authority.", timestampUtc);
        }

        if (packet.Telemetry?.AttemptsAuthority == true)
        {
            return Refuse(packet, "telemetry-authority-refused", "Packet refused because telemetry cannot authorize membrane passage.", timestampUtc);
        }

        if (packet.CompassShell is { ClaimsEngram: true } or { ClaimsTruth: true } or { ClaimsAuthority: true })
        {
            return Refuse(packet, "compass-shell-promotion-refused", "Packet refused because Compass shell posture is pre-continuity and cannot become engram, truth, or authority by transit.", timestampUtc);
        }

        if (IsPrimeToSteward(packet) &&
            !string.Equals(packet.Address.Route, SanctuaryPacketRoutes.CGoaInsulated, StringComparison.Ordinal))
        {
            return Refuse(packet, "prime-steward-cgoa-required", "Packet refused because Prime may reach Steward only through cGoA-insulated passage in the cold bench.", timestampUtc);
        }

        if (IsCrypticToSteward(packet) &&
            !string.Equals(packet.Address.Route, SanctuaryPacketRoutes.TelemetryString, StringComparison.Ordinal))
        {
            return Refuse(packet, "cryptic-steward-telemetry-required", "Packet refused because Cryptic may reach Steward only through telemetry-string passage in the cold bench.", timestampUtc);
        }

        return new PacketValidationReceipt(
            ReceiptHandle: $"packet-validation://accepted/{Math.Abs(HashCode.Combine(packet.PacketHandle, timestampUtc.UtcTicks)):x}",
            Disposition: SanctuaryPacketValidationDisposition.AcceptedCold,
            OutcomeCode: "packet-accepted-cold",
            GovernanceTrace: "Packet accepted as cold structure under declared membrane, custody, witness, and authority ceiling. No authority, continuity, runtime action, or activation is granted.",
            PacketHandle: packet.PacketHandle,
            Refusal: null,
            ActivationRefused: true,
            RuntimeActionAllowed: false,
            AuthorityGranted: false,
            ContinuityAdmitted: false,
            TimestampUtc: timestampUtc);
    }

    private static bool IsPrimeToSteward(SanctuaryPacket packet) =>
        string.Equals(packet.Address.SourceSurface, SanctuaryPacketSurfaces.Prime, StringComparison.Ordinal) &&
        string.Equals(packet.Address.TargetSurface, SanctuaryPacketSurfaces.Steward, StringComparison.Ordinal);

    private static bool IsCrypticToSteward(SanctuaryPacket packet) =>
        string.Equals(packet.Address.SourceSurface, SanctuaryPacketSurfaces.Cryptic, StringComparison.Ordinal) &&
        string.Equals(packet.Address.TargetSurface, SanctuaryPacketSurfaces.Steward, StringComparison.Ordinal);

    private static PacketValidationReceipt Refuse(
        SanctuaryPacket packet,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"packet-validation://refused/{Math.Abs(HashCode.Combine(packet.PacketHandle, outcomeCode, timestampUtc.UtcTicks)):x}",
            Disposition: SanctuaryPacketValidationDisposition.Refused,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            PacketHandle: packet.PacketHandle,
            Refusal: new RefusalReceipt(
                ReceiptHandle: $"packet-refusal://{Math.Abs(HashCode.Combine(packet.PacketHandle, outcomeCode)):x}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            ActivationRefused: true,
            RuntimeActionAllowed: false,
            AuthorityGranted: false,
            ContinuityAdmitted: false,
            TimestampUtc: timestampUtc);
}

public sealed class DefaultPacketReceiptRoutingValidator
{
    private static readonly ReceiptAuthorityBoundary NonPermissionBoundary = new(
        ReceiptMayAuthorizeFuturePacket: false,
        ReceiptMayAdmitContinuity: false,
        ReceiptMayActivate: false,
        BoundaryLaw: "A receipt may prove passage. It may not become permission.");

    public PacketReceiptRoutingReceipt Route(
        SanctuaryPacket packet,
        PacketValidationReceipt validationReceipt,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(packet);
        ArgumentNullException.ThrowIfNull(validationReceipt);

        if (validationReceipt.Disposition == SanctuaryPacketValidationDisposition.AcceptedCold &&
            string.IsNullOrWhiteSpace(packet.CustodyEnvelope.RevocationPath))
        {
            var refusal = new RefusalReceipt(
                ReceiptHandle: $"packet-refusal://{Math.Abs(HashCode.Combine(packet.PacketHandle, "revocation-path-missing")):x}",
                RefusalCode: "accepted-packet-revocation-path-missing",
                GovernanceTrace: "Accepted packet receipt routing refused because accepted passage must retain a revocation path before it can be witnessed.",
                Retained: true);
            return CreateRefusalRouting(
                packet,
                validationReceipt,
                refusal,
                "accepted-packet-revocation-path-missing",
                refusal.GovernanceTrace,
                timestampUtc);
        }

        if (validationReceipt.Disposition == SanctuaryPacketValidationDisposition.Refused)
        {
            return CreateRefusalRouting(
                packet,
                validationReceipt,
                validationReceipt.Refusal ?? new RefusalReceipt(
                    ReceiptHandle: $"packet-refusal://{Math.Abs(HashCode.Combine(packet.PacketHandle, validationReceipt.OutcomeCode)):x}",
                    RefusalCode: validationReceipt.OutcomeCode,
                    GovernanceTrace: validationReceipt.GovernanceTrace,
                    Retained: true),
                validationReceipt.OutcomeCode,
                "Packet refusal routed to witness, custody, and telemetry observation while preserving that refusal is not permission.",
                timestampUtc);
        }

        var passage = new PacketPassageReceipt(
            ReceiptHandle: $"packet-passage://{Math.Abs(HashCode.Combine(packet.PacketHandle, validationReceipt.ReceiptHandle, timestampUtc.UtcTicks)):x}",
            PacketHandle: packet.PacketHandle,
            WitnessRoute: CreateWitnessRoute(packet),
            CustodyRoute: CreateCustodyRoute(packet),
            TelemetryObservation: CreateTelemetryObservation(packet),
            RevocationPath: new PacketRevocationPath(
                PathCode: packet.CustodyEnvelope.RevocationPath,
                Present: true,
                MayUndoHistory: false,
                MayRevokeFutureUse: true),
            ProvesPassage: true,
            GrantsPermission: false);

        return new PacketReceiptRoutingReceipt(
            ReceiptHandle: $"packet-receipt-routing://passage/{Math.Abs(HashCode.Combine(packet.PacketHandle, timestampUtc.UtcTicks)):x}",
            Disposition: PacketReceiptRoutingDisposition.RoutedPassageCold,
            OutcomeCode: "packet-passage-receipt-routed-cold",
            GovernanceTrace: "Accepted packet passage receipt routed to witness, custody, and telemetry observation. The receipt proves passage and grants no future permission, continuity, authority, activation, or runtime action.",
            PacketHandle: packet.PacketHandle,
            ValidationReceipt: validationReceipt,
            Passage: passage,
            RefusalRouting: null,
            AuthorityBoundary: NonPermissionBoundary,
            ActivationRefused: true,
            AuthorityGranted: false,
            ContinuityAdmitted: false,
            TimestampUtc: timestampUtc);
    }

    private static PacketReceiptRoutingReceipt CreateRefusalRouting(
        SanctuaryPacket packet,
        PacketValidationReceipt validationReceipt,
        RefusalReceipt refusal,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc)
    {
        var refusalRouting = new PacketRefusalRoutingReceipt(
            ReceiptHandle: $"packet-refusal-routing://{Math.Abs(HashCode.Combine(packet.PacketHandle, outcomeCode, timestampUtc.UtcTicks)):x}",
            PacketHandle: packet.PacketHandle,
            Refusal: refusal,
            WitnessRoute: CreateWitnessRoute(packet),
            CustodyRoute: CreateCustodyRoute(packet),
            TelemetryObservation: CreateTelemetryObservation(packet),
            RetainsRefusal: true,
            GrantsPermission: false);

        return new PacketReceiptRoutingReceipt(
            ReceiptHandle: $"packet-receipt-routing://refusal/{Math.Abs(HashCode.Combine(packet.PacketHandle, outcomeCode, timestampUtc.UtcTicks)):x}",
            Disposition: PacketReceiptRoutingDisposition.RoutedRefusalCold,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            PacketHandle: packet.PacketHandle,
            ValidationReceipt: validationReceipt,
            Passage: null,
            RefusalRouting: refusalRouting,
            AuthorityBoundary: NonPermissionBoundary,
            ActivationRefused: true,
            AuthorityGranted: false,
            ContinuityAdmitted: false,
            TimestampUtc: timestampUtc);
    }

    private static MembraneWitnessRoute CreateWitnessRoute(SanctuaryPacket packet) =>
        new(
            WitnessSurface: string.IsNullOrWhiteSpace(packet.Witness?.WitnessSurface)
                ? SanctuaryPacketSurfaces.Steward
                : packet.Witness.WitnessSurface,
            CustodySurface: packet.CustodyEnvelope.CustodyOwner,
            SeparateCustody: packet.Witness?.SeparateCustody == true,
            StewardWitnessPresent: string.Equals(packet.Witness?.WitnessSurface, SanctuaryPacketSurfaces.Steward, StringComparison.Ordinal) ||
                string.Equals(packet.CustodyEnvelope.CustodyOwner, SanctuaryPacketSurfaces.Steward, StringComparison.Ordinal));

    private static CustodyRetentionRoute CreateCustodyRoute(SanctuaryPacket packet) =>
        new(
            CustodyOwner: packet.CustodyEnvelope.CustodyOwner,
            RetentionClass: "durable-cold-receipt",
            Retained: true,
            RevocationPath: packet.CustodyEnvelope.RevocationPath);

    private static PacketTelemetryObservation CreateTelemetryObservation(SanctuaryPacket packet) =>
        new(
            TraceId: string.IsNullOrWhiteSpace(packet.Telemetry?.TraceId)
                ? $"trace://{Math.Abs(HashCode.Combine(packet.PacketHandle)):x}"
                : packet.Telemetry.TraceId,
            ObservedRoute: packet.Address.Route,
            AttemptsAuthority: packet.Telemetry?.AttemptsAuthority == true,
            GrantsAuthority: false);
}

public sealed class DefaultReceiptReplayBoundaryValidator
{
    private static readonly ReplayNonReentryBoundary NonReentryBoundary = new(
        EmitsNewPacket: false,
        RepeatsPassage: false,
        IncrementsPassageCount: false,
        AuthorizesFuturePacket: false,
        AdmitsContinuity: false,
        BoundaryLaw: "Replay may inspect evidence. Replay may not repeat passage.");

    public ReceiptReplayReceipt Replay(
        ReceiptReplayRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.OriginalReceipt is null)
        {
            return Refuse(
                request,
                "receipt-replay-original-receipt-missing",
                "Receipt replay refused because original receipt evidence is missing.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.Present || string.IsNullOrWhiteSpace(request.ScopeBoundary.ScopeCode))
        {
            return Refuse(
                request,
                "receipt-replay-scope-boundary-missing",
                "Receipt replay refused because a scope boundary is required before evidence review.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.ReviewOnly || request.ScopeBoundary.AllowsReentry)
        {
            return Refuse(
                request,
                "receipt-replay-reentry-scope-refused",
                "Receipt replay refused because replay scope must be review-only and non-reentry.",
                timestampUtc);
        }

        if (!request.WitnessContext.WitnessPresent ||
            string.IsNullOrWhiteSpace(request.WitnessContext.WitnessSurface))
        {
            return Refuse(
                request,
                "receipt-replay-witness-context-missing",
                "Receipt replay refused because witness context is required for review.",
                timestampUtc);
        }

        if (!request.ReplaySurface.ReviewOnly)
        {
            return Refuse(
                request,
                "receipt-replay-surface-not-review-only",
                "Receipt replay refused because replay surface is not review-only.",
                timestampUtc);
        }

        return new ReceiptReplayReceipt(
            ReceiptHandle: $"receipt-replay://review/{Math.Abs(HashCode.Combine(request.ReplayHandle, request.OriginalReceipt.ReceiptHandle, timestampUtc.UtcTicks)):x}",
            Disposition: ReceiptReplayDisposition.ReplayedForReviewCold,
            OutcomeCode: "receipt-replay-review-only",
            GovernanceTrace: "Receipt replay inspected retained evidence for review only. No new packet, passage, authorization, continuity admission, activation, or runtime action occurred.",
            ReplayHandle: request.ReplayHandle,
            OriginalReceiptHandle: request.OriginalReceipt.ReceiptHandle,
            OriginalPacketHandle: request.OriginalReceipt.PacketHandle,
            OriginalDisposition: request.OriginalReceipt.Disposition,
            NonReentryBoundary: NonReentryBoundary,
            Refusal: null,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterReplay: request.PriorPassageCount,
            ReviewOnly: true,
            NewPacketEmitted: false,
            NewPassageCreated: false,
            ActivationRefused: true,
            AuthorityGranted: false,
            ContinuityAdmitted: false,
            TimestampUtc: timestampUtc);
    }

    private static ReceiptReplayReceipt Refuse(
        ReceiptReplayRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"receipt-replay://refused/{Math.Abs(HashCode.Combine(request.ReplayHandle, outcomeCode, timestampUtc.UtcTicks)):x}",
            Disposition: ReceiptReplayDisposition.Refused,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            ReplayHandle: request.ReplayHandle,
            OriginalReceiptHandle: request.OriginalReceipt?.ReceiptHandle,
            OriginalPacketHandle: request.OriginalReceipt?.PacketHandle,
            OriginalDisposition: request.OriginalReceipt?.Disposition,
            NonReentryBoundary: NonReentryBoundary,
            Refusal: new ReplayRefusalReceipt(
                ReceiptHandle: $"receipt-replay-refusal://{Math.Abs(HashCode.Combine(request.ReplayHandle, outcomeCode)):x}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterReplay: request.PriorPassageCount,
            ReviewOnly: true,
            NewPacketEmitted: false,
            NewPassageCreated: false,
            ActivationRefused: true,
            AuthorityGranted: false,
            ContinuityAdmitted: false,
            TimestampUtc: timestampUtc);
}

public sealed class DefaultReceiptQueryBoundaryValidator
{
    private static readonly ReceiptQueryNonWarrantBoundary NonWarrantBoundary = new(
        FoundReceiptMayAuthorize: false,
        AggregateCountMayAuthorize: false,
        QuerySummaryMayAuthorize: false,
        QuerySummaryMayAdmitContinuity: false,
        QueryReplaysReceipts: false,
        QueryCreatesNewEvidenceReceiptHandles: false,
        IncrementsPassageCount: false,
        EmitsNewPacket: false,
        BoundaryLaw: "Query may locate evidence. Query may not manufacture warrant.");

    public ReceiptQueryReceipt Query(
        ReceiptQueryRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.RetainedReceipts is null)
        {
            return Refuse(
                request,
                "receipt-query-retained-receipts-missing",
                "Receipt query refused because retained receipt evidence set is missing.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.Present || string.IsNullOrWhiteSpace(request.ScopeBoundary.ScopeCode))
        {
            return Refuse(
                request,
                "receipt-query-scope-boundary-missing",
                "Receipt query refused because a scope boundary is required before evidence search.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.ReviewOnly || request.ScopeBoundary.AllowsWarrant)
        {
            return Refuse(
                request,
                "receipt-query-warrant-scope-refused",
                "Receipt query refused because query scope must be review-only and cannot allow warrant.",
                timestampUtc);
        }

        if (!request.WitnessContext.WitnessPresent ||
            string.IsNullOrWhiteSpace(request.WitnessContext.WitnessSurface))
        {
            return Refuse(
                request,
                "receipt-query-witness-context-missing",
                "Receipt query refused because witness context is required for evidence search.",
                timestampUtc);
        }

        var evidence = request.RetainedReceipts
            .Where(receipt => MatchesFilter(receipt, request.Filter))
            .Select(static receipt => new ReceiptQueryEvidenceHandle(
                OriginalReceiptHandle: receipt.ReceiptHandle,
                OriginalPacketHandle: receipt.PacketHandle,
                OriginalDisposition: receipt.Disposition,
                OriginalOutcomeCode: receipt.OutcomeCode,
                PreservesOriginalReceiptHandle: true))
            .ToArray();
        var disposition = evidence.Length == 0
            ? ReceiptQueryDisposition.EmptyReviewCold
            : ReceiptQueryDisposition.LocatedForReviewCold;

        return new ReceiptQueryReceipt(
            ReceiptHandle: $"receipt-query://review/{Math.Abs(HashCode.Combine(request.QueryHandle, evidence.Length, timestampUtc.UtcTicks)):x}",
            Disposition: disposition,
            OutcomeCode: evidence.Length == 0
                ? "receipt-query-empty-review-only"
                : "receipt-query-located-review-only",
            GovernanceTrace: evidence.Length == 0
                ? "Receipt query located no matching retained evidence. Empty result is reviewable but grants no warrant, continuity, authority, activation, or runtime action."
                : "Receipt query located retained evidence for review only. The result preserves original receipt handles and grants no warrant, continuity, authority, activation, or runtime action.",
            QueryHandle: request.QueryHandle,
            Evidence: evidence,
            NonWarrantBoundary: NonWarrantBoundary,
            Refusal: null,
            AggregateCount: evidence.Length,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterQuery: request.PriorPassageCount,
            ReviewOnly: true,
            QuerySummaryGrantsAuthority: false,
            QuerySummaryAdmitsContinuity: false,
            ReceiptsReplayed: false,
            NewPacketEmitted: false,
            ActivationRefused: true,
            AuthorityGranted: false,
            ContinuityAdmitted: false,
            TimestampUtc: timestampUtc);
    }

    private static bool MatchesFilter(
        PacketReceiptRoutingReceipt receipt,
        ReceiptQueryFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.PacketHandle) &&
            !string.Equals(receipt.PacketHandle, filter.PacketHandle, StringComparison.Ordinal))
        {
            return false;
        }

        if (filter.Disposition.HasValue &&
            receipt.Disposition != filter.Disposition.Value)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(filter.OutcomeCode) &&
            !string.Equals(receipt.OutcomeCode, filter.OutcomeCode, StringComparison.Ordinal))
        {
            return false;
        }

        return true;
    }

    private static ReceiptQueryReceipt Refuse(
        ReceiptQueryRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"receipt-query://refused/{Math.Abs(HashCode.Combine(request.QueryHandle, outcomeCode, timestampUtc.UtcTicks)):x}",
            Disposition: ReceiptQueryDisposition.Refused,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            QueryHandle: request.QueryHandle,
            Evidence: [],
            NonWarrantBoundary: NonWarrantBoundary,
            Refusal: new QueryRefusalReceipt(
                ReceiptHandle: $"receipt-query-refusal://{Math.Abs(HashCode.Combine(request.QueryHandle, outcomeCode)):x}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            AggregateCount: 0,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterQuery: request.PriorPassageCount,
            ReviewOnly: true,
            QuerySummaryGrantsAuthority: false,
            QuerySummaryAdmitsContinuity: false,
            ReceiptsReplayed: false,
            NewPacketEmitted: false,
            ActivationRefused: true,
            AuthorityGranted: false,
            ContinuityAdmitted: false,
            TimestampUtc: timestampUtc);
}

public sealed class DefaultReceiptSelectionBoundaryValidator
{
    private static readonly ReceiptSelectionNonAdmissionBoundary NonAdmissionBoundary = new(
        NominationMayAuthorize: false,
        NominationMayAdmitContinuity: false,
        NominationMayBecomeCompassTruth: false,
        NominationMayActivate: false,
        SelectionReplaysReceipts: false,
        SelectionCreatesNewEvidenceReceiptHandles: false,
        IncrementsPassageCount: false,
        EmitsNewPacket: false,
        BoundaryLaw: "Selection may nominate evidence for review. Selection may not admit evidence into continuity.");

    public ReceiptSelectionReceipt Select(
        ReceiptSelectionRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.QueryReceipt is null)
        {
            return Refuse(
                request,
                "receipt-selection-query-receipt-missing",
                "Receipt selection refused because query receipt evidence is missing.",
                timestampUtc);
        }

        if (!request.QueryReceipt.IsColdQuery)
        {
            return Refuse(
                request,
                "receipt-selection-query-not-cold-review",
                "Receipt selection refused because source query receipt is not a cold review result.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.Present || string.IsNullOrWhiteSpace(request.ScopeBoundary.ScopeCode))
        {
            return Refuse(
                request,
                "receipt-selection-scope-boundary-missing",
                "Receipt selection refused because a scope boundary is required before evidence nomination.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.ReviewOnly ||
            request.ScopeBoundary.AllowsAuthority ||
            request.ScopeBoundary.AllowsContinuityAdmission ||
            request.ScopeBoundary.AllowsCompassTruth)
        {
            return Refuse(
                request,
                "receipt-selection-admission-scope-refused",
                "Receipt selection refused because selection scope must be review-only and cannot allow authority, continuity admission, or Compass truth.",
                timestampUtc);
        }

        if (!request.WitnessContext.WitnessPresent ||
            string.IsNullOrWhiteSpace(request.WitnessContext.WitnessSurface))
        {
            return Refuse(
                request,
                "receipt-selection-witness-context-missing",
                "Receipt selection refused because witness context is required for evidence nomination.",
                timestampUtc);
        }

        var availableEvidence = request.QueryReceipt.Evidence
            .ToDictionary(static evidence => evidence.OriginalReceiptHandle, StringComparer.Ordinal);
        var requestedHandles = request.RequestedOriginalReceiptHandles
            .Where(static handle => !string.IsNullOrWhiteSpace(handle))
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        if (requestedHandles.Any(handle => !availableEvidence.ContainsKey(handle)))
        {
            return Refuse(
                request,
                "receipt-selection-unknown-evidence-handle",
                "Receipt selection refused because a requested evidence handle was not present in the source query result.",
                timestampUtc);
        }

        var selectedEvidence = requestedHandles.Length == 0
            ? request.QueryReceipt.Evidence
            : requestedHandles.Select(handle => availableEvidence[handle]).ToArray();
        var nominations = selectedEvidence
            .Select(static evidence => new ReceiptSelectionNomination(
                OriginalReceiptHandle: evidence.OriginalReceiptHandle,
                OriginalPacketHandle: evidence.OriginalPacketHandle,
                OriginalDisposition: evidence.OriginalDisposition,
                OriginalOutcomeCode: evidence.OriginalOutcomeCode,
                SelectionReason: "nominated-for-review",
                PreservesOriginalReceiptHandle: true,
                CandidateOnly: true))
            .ToArray();
        var disposition = nominations.Length == 0
            ? ReceiptSelectionDisposition.EmptyReviewCold
            : ReceiptSelectionDisposition.NominatedForReviewCold;

        return new ReceiptSelectionReceipt(
            ReceiptHandle: $"receipt-selection://review/{Math.Abs(HashCode.Combine(request.SelectionHandle, request.QueryReceipt.ReceiptHandle, nominations.Length, timestampUtc.UtcTicks)):x}",
            Disposition: disposition,
            OutcomeCode: nominations.Length == 0
                ? "receipt-selection-empty-review-only"
                : "receipt-selection-nominated-review-only",
            GovernanceTrace: nominations.Length == 0
                ? "Receipt selection found no query evidence to nominate. Empty nomination is reviewable but grants no authority, continuity, Compass truth, activation, or runtime action."
                : "Receipt selection nominated retained evidence for review only. Nominations preserve original receipt handles and grant no authority, continuity, Compass truth, activation, or runtime action.",
            SelectionHandle: request.SelectionHandle,
            QueryReceiptHandle: request.QueryReceipt.ReceiptHandle,
            Nominations: nominations,
            NonAdmissionBoundary: NonAdmissionBoundary,
            Refusal: null,
            NominationCount: nominations.Length,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterSelection: request.PriorPassageCount,
            ReviewOnly: true,
            SelectionGrantsAuthority: false,
            SelectionAdmitsContinuity: false,
            SelectionBecomesCompassTruth: false,
            ReceiptsReplayed: false,
            NewPacketEmitted: false,
            ActivationRefused: true,
            AuthorityGranted: false,
            ContinuityAdmitted: false,
            TimestampUtc: timestampUtc);
    }

    private static ReceiptSelectionReceipt Refuse(
        ReceiptSelectionRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"receipt-selection://refused/{Math.Abs(HashCode.Combine(request.SelectionHandle, outcomeCode, timestampUtc.UtcTicks)):x}",
            Disposition: ReceiptSelectionDisposition.Refused,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SelectionHandle: request.SelectionHandle,
            QueryReceiptHandle: request.QueryReceipt?.ReceiptHandle,
            Nominations: [],
            NonAdmissionBoundary: NonAdmissionBoundary,
            Refusal: new SelectionRefusalReceipt(
                ReceiptHandle: $"receipt-selection-refusal://{Math.Abs(HashCode.Combine(request.SelectionHandle, outcomeCode)):x}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            NominationCount: 0,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterSelection: request.PriorPassageCount,
            ReviewOnly: true,
            SelectionGrantsAuthority: false,
            SelectionAdmitsContinuity: false,
            SelectionBecomesCompassTruth: false,
            ReceiptsReplayed: false,
            NewPacketEmitted: false,
            ActivationRefused: true,
            AuthorityGranted: false,
            ContinuityAdmitted: false,
            TimestampUtc: timestampUtc);
}

public sealed class DefaultWitnessSummaryBoundaryValidator
{
    private static readonly WitnessSummaryNonReplacementBoundary NonReplacementBoundary = new(
        SummaryMayReplaceEvidence: false,
        SummaryMayAuthorize: false,
        SummaryMayAdmitContinuity: false,
        SummaryMayBecomeCompassTruth: false,
        SummaryCreatesNewEvidenceReceiptHandles: false,
        SummaryReplaysReceipts: false,
        IncrementsPassageCount: false,
        EmitsNewPacket: false,
        BoundaryLaw: "Summary may compress evidence. Summary may not replace evidence.");

    private static readonly string[] StandardGroups =
    [
        "validation",
        "routing",
        "replay",
        "query",
        "selection",
        "refusal",
        "retained-artifacts",
        "doctrine-inventory",
        "unresolved-gaps"
    ];

    public WitnessSummaryReceipt Summarize(
        WitnessSummaryRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SelectionReceipt is null)
        {
            return Refuse(
                request,
                "witness-summary-selection-receipt-missing",
                "Witness summary refused because selection receipt evidence is missing.",
                timestampUtc);
        }

        if (!request.SelectionReceipt.IsColdSelection)
        {
            return Refuse(
                request,
                "witness-summary-selection-not-cold-review",
                "Witness summary refused because source selection receipt is not a cold review result.",
                timestampUtc);
        }

        if (request.ArtifactLineage is null)
        {
            return Refuse(
                request,
                "witness-summary-artifact-lineage-missing",
                "Witness summary refused because artifact lineage is missing.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.Present || string.IsNullOrWhiteSpace(request.ScopeBoundary.ScopeCode))
        {
            return Refuse(
                request,
                "witness-summary-scope-boundary-missing",
                "Witness summary refused because a scope boundary is required before evidence compression.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.ReviewOnly ||
            request.ScopeBoundary.AllowsAuthority ||
            request.ScopeBoundary.AllowsContinuityAdmission ||
            request.ScopeBoundary.AllowsEvidenceReplacement ||
            request.ScopeBoundary.AllowsCompassTruth)
        {
            return Refuse(
                request,
                "witness-summary-replacement-scope-refused",
                "Witness summary refused because summary scope must be review-only and cannot allow authority, continuity admission, evidence replacement, or Compass truth.",
                timestampUtc);
        }

        if (!request.WitnessContext.WitnessPresent ||
            string.IsNullOrWhiteSpace(request.WitnessContext.WitnessSurface))
        {
            return Refuse(
                request,
                "witness-summary-witness-context-missing",
                "Witness summary refused because witness context is required for evidence compression.",
                timestampUtc);
        }

        if (request.ConfidenceEstimate is < 0m or > 1m)
        {
            return Refuse(
                request,
                "witness-summary-confidence-out-of-range",
                "Witness summary refused because confidence estimate must remain bounded between 0 and 1.",
                timestampUtc);
        }

        var artifactLineage = request.ArtifactLineage
            .Select(static artifact => artifact with { PreservesLineage = true })
            .ToArray();
        var originalReceiptHandles = request.SelectionReceipt.Nominations
            .Select(static nomination => nomination.OriginalReceiptHandle)
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        var groups = StandardGroups
            .Select(group => CreateGroup(group, artifactLineage, originalReceiptHandles, request.DoctrinePhrases, request.GapCandidates))
            .ToArray();
        var disposition = artifactLineage.Length == 0 && originalReceiptHandles.Length == 0
            ? WitnessSummaryDisposition.EmptyReviewCold
            : WitnessSummaryDisposition.SummarizedForReviewCold;

        return new WitnessSummaryReceipt(
            ReceiptHandle: $"witness-summary://review/{Math.Abs(HashCode.Combine(request.SummaryHandle, request.SelectionReceipt.ReceiptHandle, artifactLineage.Length, timestampUtc.UtcTicks)):x}",
            Disposition: disposition,
            OutcomeCode: disposition == WitnessSummaryDisposition.EmptyReviewCold
                ? "witness-summary-empty-review-only"
                : "witness-summary-compressed-review-only",
            GovernanceTrace: disposition == WitnessSummaryDisposition.EmptyReviewCold
                ? "Witness summary found no selected evidence or artifact lineage to compress. Empty summary is reviewable but grants no authority, continuity, Compass truth, activation, or runtime action."
                : "Witness summary compressed selected evidence and artifact lineage for review only. Summary preserves source handles and lineage and grants no authority, continuity, Compass truth, activation, or runtime action.",
            SummaryHandle: request.SummaryHandle,
            SelectionReceiptHandle: request.SelectionReceipt.ReceiptHandle,
            Groups: groups,
            ArtifactLineage: artifactLineage,
            DoctrinePhrases: request.DoctrinePhrases ?? [],
            GapCandidates: request.GapCandidates ?? [],
            NonReplacementBoundary: NonReplacementBoundary,
            Refusal: null,
            ConfidenceEstimate: request.ConfidenceEstimate,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterSummary: request.PriorPassageCount,
            ReviewOnly: true,
            SummaryReplacesEvidence: false,
            SummaryGrantsAuthority: false,
            SummaryAdmitsContinuity: false,
            SummaryBecomesCompassTruth: false,
            ReceiptsReplayed: false,
            NewPacketEmitted: false,
            ActivationRefused: true,
            AuthorityGranted: false,
            ContinuityAdmitted: false,
            TimestampUtc: timestampUtc);
    }

    private static WitnessSummaryGroup CreateGroup(
        string groupCode,
        IReadOnlyList<WitnessSummaryArtifactLineage> artifacts,
        IReadOnlyList<string> originalReceiptHandles,
        IReadOnlyList<WitnessDoctrinePhrase>? doctrinePhrases,
        IReadOnlyList<WitnessGapCandidate>? gapCandidates)
    {
        var artifactIds = groupCode switch
        {
            "validation" => SelectArtifacts(artifacts, static artifact => artifact.CellId.Contains("contract-validation", StringComparison.Ordinal)),
            "routing" => SelectArtifacts(artifacts, static artifact => artifact.CellId.Contains("receipt-routing", StringComparison.Ordinal)),
            "replay" => SelectArtifacts(artifacts, static artifact => artifact.CellId.Contains("receipt-replay", StringComparison.Ordinal)),
            "query" => SelectArtifacts(artifacts, static artifact => artifact.CellId.Contains("receipt-query", StringComparison.Ordinal)),
            "selection" => SelectArtifacts(artifacts, static artifact => artifact.CellId.Contains("receipt-selection", StringComparison.Ordinal)),
            "refusal" => SelectArtifacts(artifacts, static artifact =>
                artifact.ArtifactId.Contains("refusal", StringComparison.Ordinal) ||
                artifact.ArtifactId.Contains("non-authority", StringComparison.Ordinal) ||
                artifact.ArtifactId.Contains("non-permission", StringComparison.Ordinal) ||
                artifact.ArtifactId.Contains("non-reentry", StringComparison.Ordinal) ||
                artifact.ArtifactId.Contains("non-warrant", StringComparison.Ordinal) ||
                artifact.ArtifactId.Contains("non-admission", StringComparison.Ordinal)),
            "retained-artifacts" => artifacts.Select(static artifact => artifact.ArtifactId).ToArray(),
            "doctrine-inventory" => doctrinePhrases?.Where(static phrase => phrase.EnforcedByTests).Select(static phrase => phrase.SourceRef).ToArray() ?? [],
            "unresolved-gaps" => gapCandidates?.Select(static gap => gap.GapCode).ToArray() ?? [],
            _ => []
        };

        return new WitnessSummaryGroup(
            GroupCode: groupCode,
            ArtifactIds: artifactIds,
            OriginalReceiptHandles: originalReceiptHandles,
            SummaryText: $"review-only {groupCode} summary",
            ReviewOnly: true);
    }

    private static string[] SelectArtifacts(
        IReadOnlyList<WitnessSummaryArtifactLineage> artifacts,
        Func<WitnessSummaryArtifactLineage, bool> predicate) =>
        artifacts
            .Where(predicate)
            .Select(static artifact => artifact.ArtifactId)
            .ToArray();

    private static WitnessSummaryReceipt Refuse(
        WitnessSummaryRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"witness-summary://refused/{Math.Abs(HashCode.Combine(request.SummaryHandle, outcomeCode, timestampUtc.UtcTicks)):x}",
            Disposition: WitnessSummaryDisposition.Refused,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SummaryHandle: request.SummaryHandle,
            SelectionReceiptHandle: request.SelectionReceipt?.ReceiptHandle,
            Groups: [],
            ArtifactLineage: request.ArtifactLineage ?? [],
            DoctrinePhrases: request.DoctrinePhrases ?? [],
            GapCandidates: request.GapCandidates ?? [],
            NonReplacementBoundary: NonReplacementBoundary,
            Refusal: new WitnessSummaryRefusalReceipt(
                ReceiptHandle: $"witness-summary-refusal://{Math.Abs(HashCode.Combine(request.SummaryHandle, outcomeCode)):x}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            ConfidenceEstimate: request.ConfidenceEstimate is >= 0m and <= 1m ? request.ConfidenceEstimate : 0m,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterSummary: request.PriorPassageCount,
            ReviewOnly: true,
            SummaryReplacesEvidence: false,
            SummaryGrantsAuthority: false,
            SummaryAdmitsContinuity: false,
            SummaryBecomesCompassTruth: false,
            ReceiptsReplayed: false,
            NewPacketEmitted: false,
            ActivationRefused: true,
            AuthorityGranted: false,
            ContinuityAdmitted: false,
            TimestampUtc: timestampUtc);
}

public sealed class DefaultCompassPreEngramPressureBoundaryValidator
{
    private static readonly CompassPressureNonEngramBoundary NonEngramBoundary = new(
        PressureMayBecomeEngram: false,
        PressureMayBecomeTruth: false,
        PressureMayAuthorize: false,
        PressureMayAdmitContinuity: false,
        PressureMayAppendSelfGel: false,
        PressureMayAppendCSelfGel: false,
        PressureReplaysReceipts: false,
        IncrementsPassageCount: false,
        EmitsNewPacket: false,
        BoundaryLaw: "Pre-engram residue may pressure Compass. Pre-engram residue may not become engram.");

    public CompassPressureReceipt Pressurize(
        CompassPressureRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SummaryReceipt is null)
        {
            return Refuse(
                request,
                "compass-pressure-summary-receipt-missing",
                "Compass pressure refused because witness summary evidence is missing.",
                timestampUtc);
        }

        if (!request.SummaryReceipt.IsColdSummary)
        {
            return Refuse(
                request,
                "compass-pressure-summary-not-cold-review",
                "Compass pressure refused because source witness summary is not a cold review result.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.Present || string.IsNullOrWhiteSpace(request.ScopeBoundary.ScopeCode))
        {
            return Refuse(
                request,
                "compass-pressure-scope-boundary-missing",
                "Compass pressure refused because a scope boundary is required before Compass pressure formation.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.ReviewOnly ||
            request.ScopeBoundary.AllowsEngram ||
            request.ScopeBoundary.AllowsTruth ||
            request.ScopeBoundary.AllowsAuthority ||
            request.ScopeBoundary.AllowsContinuityAdmission ||
            request.ScopeBoundary.AllowsSelfGelAppend ||
            request.ScopeBoundary.AllowsCSelfGelAppend)
        {
            return Refuse(
                request,
                "compass-pressure-engram-scope-refused",
                "Compass pressure refused because pressure scope must be review-only and cannot allow engram, truth, authority, continuity, SelfGEL append, or cSelfGEL append.",
                timestampUtc);
        }

        if (!request.WitnessContext.WitnessPresent ||
            string.IsNullOrWhiteSpace(request.WitnessContext.WitnessSurface))
        {
            return Refuse(
                request,
                "compass-pressure-witness-context-missing",
                "Compass pressure refused because witness context is required for pre-engram pressure.",
                timestampUtc);
        }

        var sourceHandles = request.SummaryReceipt.Groups
            .SelectMany(static group => group.OriginalReceiptHandles)
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        var artifactLineage = request.SummaryReceipt.ArtifactLineage
            .Select(static artifact => artifact with { PreservesLineage = true })
            .ToArray();
        var residue = new PreEngramResidue(
            SummaryReceiptHandle: request.SummaryReceipt.ReceiptHandle,
            SelectionReceiptHandle: request.SummaryReceipt.SelectionReceiptHandle,
            OriginalReceiptHandles: sourceHandles,
            ArtifactLineage: artifactLineage,
            CandidateOnly: true,
            EngramAdmitted: false,
            SelfGelAppendAllowed: false,
            CSelfGelAppendAllowed: false);
        var vector = CreateVector(request.SummaryReceipt, residue);
        var disposition = sourceHandles.Length == 0 && artifactLineage.Length == 0
            ? CompassPressureDisposition.EmptyReviewCold
            : CompassPressureDisposition.PressurizedForReviewCold;

        return new CompassPressureReceipt(
            ReceiptHandle: $"compass-pressure://review/{Math.Abs(HashCode.Combine(request.PressureHandle, request.SummaryReceipt.ReceiptHandle, artifactLineage.Length, timestampUtc.UtcTicks)):x}",
            Disposition: disposition,
            OutcomeCode: disposition == CompassPressureDisposition.EmptyReviewCold
                ? "compass-pressure-empty-review-only"
                : "compass-pressure-candidate-review-only",
            GovernanceTrace: disposition == CompassPressureDisposition.EmptyReviewCold
                ? "Compass pressure found no summary lineage to form pressure. Empty pressure is reviewable but grants no engram, truth, authority, continuity, activation, or SelfGEL/cSelfGEL append."
                : "Compass pressure formed a bounded pre-engram pressure candidate from witness summary. Pressure grants no engram, truth, authority, continuity, activation, or SelfGEL/cSelfGEL append.",
            PressureHandle: request.PressureHandle,
            SummaryReceiptHandle: request.SummaryReceipt.ReceiptHandle,
            Residue: residue,
            PressureVector: vector,
            NonEngramBoundary: NonEngramBoundary,
            Refusal: null,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterPressure: request.PriorPassageCount,
            ReviewOnly: true,
            CandidateOnly: true,
            PressureBecomesEngram: false,
            PressureBecomesTruth: false,
            PressureGrantsAuthority: false,
            PressureAdmitsContinuity: false,
            PressureAppendsSelfGel: false,
            PressureAppendsCSelfGel: false,
            ReceiptsReplayed: false,
            NewPacketEmitted: false,
            ActivationRefused: true,
            AuthorityGranted: false,
            ContinuityAdmitted: false,
            TimestampUtc: timestampUtc);
    }

    private static CompassPressureVector CreateVector(
        WitnessSummaryReceipt summary,
        PreEngramResidue residue)
    {
        var evidenceDensity = Clamp01((residue.OriginalReceiptHandles.Count + residue.ArtifactLineage.Count) / 20m);
        var doctrinePressure = Clamp01(summary.DoctrinePhrases.Count(static phrase => phrase.EnforcedByTests) / 10m);
        var gapPressure = Clamp01(summary.GapCandidates.Count(static gap => !gap.BlocksColdBench) / 10m);

        return new CompassPressureVector(
            EvidenceDensity: evidenceDensity,
            DoctrinePressure: doctrinePressure,
            GapPressure: gapPressure,
            Confidence: Clamp01(summary.ConfidenceEstimate),
            Bounded: true);
    }

    private static decimal Clamp01(decimal value) =>
        Math.Max(0m, Math.Min(1m, value));

    private static CompassPressureReceipt Refuse(
        CompassPressureRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"compass-pressure://refused/{Math.Abs(HashCode.Combine(request.PressureHandle, outcomeCode, timestampUtc.UtcTicks)):x}",
            Disposition: CompassPressureDisposition.Refused,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            PressureHandle: request.PressureHandle,
            SummaryReceiptHandle: request.SummaryReceipt?.ReceiptHandle,
            Residue: null,
            PressureVector: new CompassPressureVector(0m, 0m, 0m, 0m, Bounded: true),
            NonEngramBoundary: NonEngramBoundary,
            Refusal: new CompassPressureRefusalReceipt(
                ReceiptHandle: $"compass-pressure-refusal://{Math.Abs(HashCode.Combine(request.PressureHandle, outcomeCode)):x}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterPressure: request.PriorPassageCount,
            ReviewOnly: true,
            CandidateOnly: true,
            PressureBecomesEngram: false,
            PressureBecomesTruth: false,
            PressureGrantsAuthority: false,
            PressureAdmitsContinuity: false,
            PressureAppendsSelfGel: false,
            PressureAppendsCSelfGel: false,
            ReceiptsReplayed: false,
            NewPacketEmitted: false,
            ActivationRefused: true,
            AuthorityGranted: false,
            ContinuityAdmitted: false,
            TimestampUtc: timestampUtc);
}

public sealed class DefaultEngramCandidatePreconditionBoundaryValidator
{
    private static readonly EngramCandidateNonAdmissionBoundary NonAdmissionBoundary = new(
        CandidateMayBecomeEngram: false,
        CandidateMayAdmitContinuity: false,
        CandidateMayAuthorize: false,
        CandidateMayAppendSelfGel: false,
        CandidateMayAppendCSelfGel: false,
        CandidateMayReplaceEvidence: false,
        CandidateReplaysReceipts: false,
        IncrementsPassageCount: false,
        EmitsNewPacket: false,
        CandidateMayActivate: false,
        BoundaryLaw: "Engram candidate readiness may nominate residue for later review. Engram candidate readiness may not admit engram, continuity, authority, SelfGEL, or cSelfGEL.");

    public EngramCandidateReadinessReceipt Nominate(
        EngramCandidateReadinessRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.PressureReceipt is null)
        {
            return Refuse(
                request,
                "engram-candidate-pressure-receipt-missing",
                "Engram candidate readiness refused because Compass pressure evidence is missing.",
                timestampUtc);
        }

        if (!request.PressureReceipt.IsColdPressure)
        {
            return Refuse(
                request,
                "engram-candidate-pressure-not-cold-review",
                "Engram candidate readiness refused because source Compass pressure is not a cold review result.",
                timestampUtc);
        }

        if (request.PressureReceipt.Residue is null)
        {
            return Refuse(
                request,
                "engram-candidate-residue-missing",
                "Engram candidate readiness refused because pre-engram residue is missing.",
                timestampUtc);
        }

        if (!request.SourceBoundary.MembraneLandingPresent ||
            string.IsNullOrWhiteSpace(request.SourceBoundary.MembraneLandingCode))
        {
            return Refuse(
                request,
                "engram-candidate-membrane-landing-missing",
                "Engram candidate readiness refused because membrane landing must be declared before candidate nomination.",
                timestampUtc);
        }

        if (!request.SourceBoundary.ClassificationPresent ||
            string.IsNullOrWhiteSpace(request.SourceBoundary.ClassificationCode))
        {
            return Refuse(
                request,
                "engram-candidate-classification-missing",
                "Engram candidate readiness refused because classification must be declared before candidate nomination.",
                timestampUtc);
        }

        if (!request.SourceBoundary.SourceEvidencePresent ||
            request.PressureReceipt.Residue.OriginalReceiptHandles.Count == 0)
        {
            return Refuse(
                request,
                "engram-candidate-source-evidence-missing",
                "Engram candidate readiness refused because original source evidence handles are required before candidate nomination.",
                timestampUtc);
        }

        if (!request.SourceBoundary.TransformationTracePresent ||
            string.IsNullOrWhiteSpace(request.SourceBoundary.TransformationTrace))
        {
            return Refuse(
                request,
                "engram-candidate-transformation-trace-missing",
                "Engram candidate readiness refused because transformation trace must be declared before candidate nomination.",
                timestampUtc);
        }

        if (!request.SourceBoundary.ContinuityRelationPresent ||
            string.IsNullOrWhiteSpace(request.SourceBoundary.ContinuityRelationCode))
        {
            return Refuse(
                request,
                "engram-candidate-continuity-relation-missing",
                "Engram candidate readiness refused because a candidate continuity relation must be declared without admitting continuity.",
                timestampUtc);
        }

        if (!request.WitnessContext.WitnessPresent ||
            string.IsNullOrWhiteSpace(request.WitnessContext.WitnessSurface))
        {
            return Refuse(
                request,
                "engram-candidate-witness-context-missing",
                "Engram candidate readiness refused because witness context is required before candidate nomination.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.Present ||
            string.IsNullOrWhiteSpace(request.ScopeBoundary.ScopeCode))
        {
            return Refuse(
                request,
                "engram-candidate-scope-boundary-missing",
                "Engram candidate readiness refused because a scope boundary is required before candidate nomination.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.ReviewOnly ||
            request.ScopeBoundary.AllowsEngramAdmission ||
            request.ScopeBoundary.AllowsContinuityAdmission ||
            request.ScopeBoundary.AllowsAuthority ||
            request.ScopeBoundary.AllowsSelfGelAppend ||
            request.ScopeBoundary.AllowsCSelfGelAppend ||
            request.ScopeBoundary.AllowsRuntimeAction)
        {
            return Refuse(
                request,
                "engram-candidate-admission-scope-refused",
                "Engram candidate readiness refused because candidate scope must be review-only and cannot allow engram admission, continuity, authority, SelfGEL, cSelfGEL, or runtime action.",
                timestampUtc);
        }

        var residue = request.PressureReceipt.Residue;
        var evidenceLineage = new EngramCandidateEvidenceLineage(
            PressureReceiptHandle: request.PressureReceipt.ReceiptHandle,
            SummaryReceiptHandle: residue.SummaryReceiptHandle,
            SelectionReceiptHandle: residue.SelectionReceiptHandle,
            OriginalReceiptHandles: residue.OriginalReceiptHandles.Distinct(StringComparer.Ordinal).ToArray(),
            ArtifactLineage: residue.ArtifactLineage
                .Select(static artifact => artifact with { PreservesLineage = true })
                .ToArray(),
            SourceBoundary: request.SourceBoundary,
            PreservesOriginalEvidence: true,
            CandidateOnly: true);

        return new EngramCandidateReadinessReceipt(
            ReceiptHandle: $"engram-candidate://review/{Math.Abs(HashCode.Combine(request.CandidateHandle, request.PressureReceipt.ReceiptHandle, evidenceLineage.OriginalReceiptHandles.Count, timestampUtc.UtcTicks)):x}",
            Disposition: EngramCandidateDisposition.NominatedForReviewCold,
            OutcomeCode: "engram-candidate-readiness-nominated-review-only",
            GovernanceTrace: "Engram candidate readiness nominated pre-engram residue for later review while preserving source evidence, membrane landing, classification, transformation trace, witness custody, and non-admission boundaries. Nomination grants no engram, continuity, authority, activation, or SelfGEL/cSelfGEL append.",
            CandidateHandle: request.CandidateHandle,
            PressureReceiptHandle: request.PressureReceipt.ReceiptHandle,
            EvidenceLineage: evidenceLineage,
            NonAdmissionBoundary: NonAdmissionBoundary,
            Refusal: null,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterCandidate: request.PriorPassageCount,
            ReviewOnly: true,
            CandidateOnly: true,
            CandidateNominated: true,
            CandidateBecomesEngram: false,
            CandidateAdmitsContinuity: false,
            CandidateGrantsAuthority: false,
            CandidateAppendsSelfGel: false,
            CandidateAppendsCSelfGel: false,
            CandidateReplacesEvidence: false,
            ReceiptsReplayed: false,
            NewPacketEmitted: false,
            ActivationRefused: true,
            AuthorityGranted: false,
            ContinuityAdmitted: false,
            TimestampUtc: timestampUtc);
    }

    private static EngramCandidateReadinessReceipt Refuse(
        EngramCandidateReadinessRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"engram-candidate://refused/{Math.Abs(HashCode.Combine(request.CandidateHandle, outcomeCode, timestampUtc.UtcTicks)):x}",
            Disposition: EngramCandidateDisposition.Refused,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            CandidateHandle: request.CandidateHandle,
            PressureReceiptHandle: request.PressureReceipt?.ReceiptHandle,
            EvidenceLineage: null,
            NonAdmissionBoundary: NonAdmissionBoundary,
            Refusal: new EngramCandidateRefusalReceipt(
                ReceiptHandle: $"engram-candidate-refusal://{Math.Abs(HashCode.Combine(request.CandidateHandle, outcomeCode)):x}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterCandidate: request.PriorPassageCount,
            ReviewOnly: true,
            CandidateOnly: true,
            CandidateNominated: false,
            CandidateBecomesEngram: false,
            CandidateAdmitsContinuity: false,
            CandidateGrantsAuthority: false,
            CandidateAppendsSelfGel: false,
            CandidateAppendsCSelfGel: false,
            CandidateReplacesEvidence: false,
            ReceiptsReplayed: false,
            NewPacketEmitted: false,
            ActivationRefused: true,
            AuthorityGranted: false,
            ContinuityAdmitted: false,
            TimestampUtc: timestampUtc);
}
