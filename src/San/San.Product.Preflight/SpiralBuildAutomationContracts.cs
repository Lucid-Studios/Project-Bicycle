using System.Text.Json.Serialization;

namespace San.Product.Preflight;

public enum SpiralBuildAutomationDisposition
{
    Withheld = 0,
    ReadyCold = 1,
    Refused = 2,
    Complete = 3
}

public enum SpiralBuildPhase
{
    FullBodyPass = 0,
    CellularStructure = 1,
    Membrane = 2,
    InstrumentBodyHardening = 3,
    WiringTelemetryHardening = 4,
    PacketMembraneContractValidation = 5,
    PacketMembraneReceiptRouting = 6,
    PacketMembraneReceiptReplayBoundary = 7,
    PacketMembraneReceiptQueryBoundary = 8,
    PacketMembraneReceiptSelectionBoundary = 9,
    WitnessSummaryBoundary = 10,
    CompassPreEngramPressureBoundary = 11,
    CompassShellStabilizationBoundary = 12,
    CleavingDiscernmentBoundary = 13,
    IterativeEvaluationBoundary = 14,
    RecursiveContemplationBoundary = 15,
    StewardHandoffReadinessBoundary = 16,
    TypedDuplexIterationMap = 17,
    TenByTenBodyOptimizationSchedule = 18,
    TenByTenGroupAOptimizationRun = 19,
    TenByTenGroupBOptimizationRun = 20,
    TenByTenGroupCOptimizationRun = 21,
    TenByTenGroupDOptimizationRun = 22,
    WholeBodySynthesisColdComparison = 23,
    NinefoldColdReviewTelemetryContract = 24,
    EngramCandidatePreconditionBoundary = 25,
    SwarmCustodyBraidOrchestrationBoundary = 26,
    PersistentWitnessStoreCustodyBoundary = 27,
    SliLispPostureManifestBoundary = 28,
    SliLispCompassCarrierShellBoundary = 29,
    EngineeredCognitionMeaningShellBoundary = 30,
    EngineeredCognitionParticipatoryPeerlessForkBoundary = 31,
    CmeLispThreadFretboardStringingBoundary = 32,
    CmeLispListeningFrameResonanceHeartbeatBoundary = 33,
    StewardHarmonicCustodyInterlockBoundary = 34,
    HarmonicInterlockModulationCorrespondenceBoundary = 35,
    TypedActionFormationBoundary = 36,
    ActionMethodReadinessBoundary = 37,
    StewardActionAdmissibilityBoundary = 38,
    AntiCaptureMotivatedConcernBoundary = 39,
    PersonificationPredicateHookBoundary = 40,
    PersonificationModalityHumilityBoundary = 41,
    DialogosDiscernmentBoundary = 42,
    WaveCondensationSharedRealityBoundary = 43,
    WaveCascadeRunBoundary = 44,
    AspirationPayloadIngestionMaturationBoundary = 45,
    AspirationCandidateSelectionClosureBoundary = 46,
    ScopedWorkPacketFormationBoundary = 47,
    EnactmentBoundaryReadinessBoundary = 48,
    EnactmentDryRunRehearsalBoundary = 49,
    EcPrecipitationWitnessBoundary = 50,
    RehearsalDistinctionPressureBoundary = 51,
    PersonificationActualizationSurfaceBoundary = 52,
    SelectiveLawfulActionSurfaceBoundary = 53,
    ZedDeltaChamberFormationBoundary = 54,
    HighEnergyArticulationCandidateBoundary = 55,
    MembraneMorphologyTransitionBoundary = 56,
    EngramPredicatePrecursorStreamBoundary = 57,
    PeerReviewPredicateBridgeBoundary = 58,
    GelDomainScopedIngressBoundary = 59,
    SharedPrimeRealityPressureEcologyBoundary = 60,
    GapCrossingArticulationBoundary = 61,
    PreDiagnosticRiskSurfaceEngramStewardshipBoundary = 62
}

public enum SpiralBuildCellStatus
{
    VerifiedCold = 0,
    Candidate = 1,
    Planned = 2,
    Blocked = 3
}

public sealed record SpiralBuildAutomationRequest(
    string LineRootPath,
    string InstallRootPath,
    bool ActivationRequested = false,
    bool ModelBindingRequested = false,
    bool LispEvaluationRequested = false,
    bool RuntimeIdentityRequested = false,
    bool RuntimeActionRequested = false,
    bool DatabaseWriteRequested = false,
    bool GelPromotionRequested = false,
    bool CmeActualRequested = false,
    bool SanctuaryActualRequested = false)
{
    public bool RequestsRuntimeMotion =>
        ActivationRequested ||
        ModelBindingRequested ||
        LispEvaluationRequested ||
        RuntimeIdentityRequested ||
        RuntimeActionRequested ||
        DatabaseWriteRequested ||
        GelPromotionRequested ||
        CmeActualRequested ||
        SanctuaryActualRequested;
}

public sealed record SpiralBuildCellRecord(
    string CellId,
    SpiralBuildPhase Phase,
    string Layer,
    string CellName,
    SpiralBuildCellStatus Status,
    IReadOnlyList<string> AdjacentTo,
    IReadOnlyList<string> RequiredArtifacts,
    IReadOnlyList<string> StopConditions,
    string NextAction,
    bool HitlRequired);

public sealed record SpiralBuildAutomationReceipt(
    string ReceiptHandle,
    SpiralBuildAutomationDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string LineRootPath,
    string InstallRootPath,
    string BuildLaw,
    IReadOnlyList<string> Phases,
    IReadOnlyList<SpiralBuildCellRecord> Cells,
    SpiralBuildCellRecord? NextCell,
    IReadOnlyList<string> AutomationStopConditions,
    bool AutomationMayContinue,
    bool HitlRequired,
    bool ActivationRefused,
    bool ModelBindingAllowed,
    bool LispEvaluationAllowed,
    bool RuntimeIdentityAllowed,
    bool RuntimeActionAllowed,
    bool DatabaseWriteAllowed,
    bool GelPromotionAllowed,
    bool CmeActualAllowed,
    bool SanctuaryActualAllowed,
    DateTimeOffset TimestampUtc)
{
    [JsonIgnore]
    public bool IsColdAutomationReady =>
        Disposition == SpiralBuildAutomationDisposition.ReadyCold &&
        AutomationMayContinue &&
        !HitlRequired &&
        NextCell is not null &&
        NextCell.Status == SpiralBuildCellStatus.Candidate &&
        ActivationRefused &&
        !ModelBindingAllowed &&
        !LispEvaluationAllowed &&
        !RuntimeIdentityAllowed &&
        !RuntimeActionAllowed &&
        !DatabaseWriteAllowed &&
        !GelPromotionAllowed &&
        !CmeActualAllowed &&
        !SanctuaryActualAllowed;
}
