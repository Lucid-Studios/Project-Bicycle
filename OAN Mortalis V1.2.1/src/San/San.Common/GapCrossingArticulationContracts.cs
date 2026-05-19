using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace San.Common;

public enum GapCrossingArticulationDisposition
{
    CrossedForReviewCold = 0,
    Refused = 1
}

public enum GapCrossingPressureLane
{
    MeaningPressure = 0,
    ReviewPressure = 1,
    RehearsalPressure = 2,
    StewardReviewPressure = 3,
    CoolingPressure = 4,
    ReturnToPrimePressure = 5
}

public enum GapCrossingArticulationSurface
{
    MainBodyEngine = 0,
    GovernanceReview = 1,
    InstantiatedCmeTestBody = 2,
    ComparativeUniversality = 3,
    LocalSlm = 4
}

public sealed record GapCrossingPressureLaneRecord(
    string LaneHandle,
    string SourceSignalHandle,
    string SourceDestinationHandle,
    string CandidateHandle,
    string ArticulationSurfaceHandle,
    GapCrossingPressureLane Lane,
    GapCrossingArticulationSurface Surface,
    string LaneRationale,
    string NonAuthorityLaw,
    double PressureIntensity,
    double ArticulationReadiness,
    bool ReviewOnly,
    bool LaneClassified,
    bool CarriesPressureToArticulation,
    bool StewardReviewRequired,
    bool CoolingRequired,
    bool ReturnPathPresent,
    bool TreatsPressureAsPromptAuthority,
    bool TreatsPressureAsTruth,
    bool TreatsPressureAsWarrant,
    bool BindsModel,
    bool CallsProvider,
    bool StartsRuntime,
    bool AuthorizesAction,
    bool AdmitsContinuity,
    bool MutatesSelfGel,
    bool AdmitsGel,
    bool AdmitsCmeActual,
    bool ActivatesHeartbeat,
    bool EvaluatesLisp,
    bool EmitsPacket,
    bool ReplaysReceipt,
    bool IncrementsPassage,
    bool Activates)
{
    public bool IsColdLane =>
        !string.IsNullOrWhiteSpace(LaneHandle) &&
        !string.IsNullOrWhiteSpace(SourceSignalHandle) &&
        !string.IsNullOrWhiteSpace(SourceDestinationHandle) &&
        !string.IsNullOrWhiteSpace(CandidateHandle) &&
        !string.IsNullOrWhiteSpace(ArticulationSurfaceHandle) &&
        !string.IsNullOrWhiteSpace(LaneRationale) &&
        !string.IsNullOrWhiteSpace(NonAuthorityLaw) &&
        Enum.IsDefined(Lane) &&
        Enum.IsDefined(Surface) &&
        PressureIntensity is >= 0 and <= 1 &&
        ArticulationReadiness is >= 0 and <= 1 &&
        ReviewOnly &&
        LaneClassified &&
        CarriesPressureToArticulation &&
        StewardReviewRequired &&
        CoolingRequired &&
        ReturnPathPresent &&
        !TreatsPressureAsPromptAuthority &&
        !TreatsPressureAsTruth &&
        !TreatsPressureAsWarrant &&
        !BindsModel &&
        !CallsProvider &&
        !StartsRuntime &&
        !AuthorizesAction &&
        !AdmitsContinuity &&
        !MutatesSelfGel &&
        !AdmitsGel &&
        !AdmitsCmeActual &&
        !ActivatesHeartbeat &&
        !EvaluatesLisp &&
        !EmitsPacket &&
        !ReplaysReceipt &&
        !IncrementsPassage &&
        !Activates;
}

public sealed record GapCrossingArticulationSurfaceRecord(
    string SurfaceHandle,
    string CandidateHandle,
    GapCrossingArticulationSurface Surface,
    string IntendedParticipation,
    string NonBindingLaw,
    bool ReviewOnly,
    bool CandidateOnly,
    bool SurfaceSelectedForReview,
    bool PublicInterfaceOnly,
    bool ObservableBehaviorOnly,
    bool PreservesHighEnergyCandidateLineage,
    bool PreservesPressureEcologyLineage,
    bool AcceptsPressureAsReviewMaterial,
    bool TreatsSurfaceAsAgent,
    bool TreatsSurfaceAsActor,
    bool TreatsSurfaceAsPromptAuthority,
    bool CallsProvider,
    bool BindsModel,
    bool StartsRuntime,
    bool AuthorizesAction,
    bool AdmitsContinuity,
    bool MutatesSelfGel,
    bool AdmitsGel,
    bool AdmitsCmeActual,
    bool ActivatesHeartbeat,
    bool GrantsAuthority,
    bool EvaluatesLisp,
    bool EmitsPacket,
    bool ReplaysReceipt,
    bool IncrementsPassage,
    bool Activates)
{
    public bool IsColdSurface =>
        !string.IsNullOrWhiteSpace(SurfaceHandle) &&
        !string.IsNullOrWhiteSpace(CandidateHandle) &&
        !string.IsNullOrWhiteSpace(IntendedParticipation) &&
        !string.IsNullOrWhiteSpace(NonBindingLaw) &&
        Enum.IsDefined(Surface) &&
        ReviewOnly &&
        CandidateOnly &&
        SurfaceSelectedForReview &&
        PublicInterfaceOnly &&
        ObservableBehaviorOnly &&
        PreservesHighEnergyCandidateLineage &&
        PreservesPressureEcologyLineage &&
        AcceptsPressureAsReviewMaterial &&
        !TreatsSurfaceAsAgent &&
        !TreatsSurfaceAsActor &&
        !TreatsSurfaceAsPromptAuthority &&
        !CallsProvider &&
        !BindsModel &&
        !StartsRuntime &&
        !AuthorizesAction &&
        !AdmitsContinuity &&
        !MutatesSelfGel &&
        !AdmitsGel &&
        !AdmitsCmeActual &&
        !ActivatesHeartbeat &&
        !GrantsAuthority &&
        !EvaluatesLisp &&
        !EmitsPacket &&
        !ReplaysReceipt &&
        !IncrementsPassage &&
        !Activates;
}

public sealed record GapCrossingArticulationBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool RequiresSharedPrimePressureEcology,
    bool RequiresHighEnergyArticulationCandidate,
    bool RequiresLaneClassification,
    bool RequiresSurfaceSelection,
    bool RequiresCooling,
    bool RequiresStewardWitness,
    bool AllowsPressureAsPromptAuthority,
    bool AllowsPressureAsTruth,
    bool AllowsPressureAsWarrant,
    bool AllowsProviderCall,
    bool AllowsModelBinding,
    bool AllowsRuntimeStart,
    bool AllowsAction,
    bool AllowsContinuityAdmission,
    bool AllowsSelfGelMutation,
    bool AllowsGelAdmission,
    bool AllowsCmeActualAdmission,
    bool AllowsHeartbeatActivation,
    bool AllowsAuthority,
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
        RequiresSharedPrimePressureEcology &&
        RequiresHighEnergyArticulationCandidate &&
        RequiresLaneClassification &&
        RequiresSurfaceSelection &&
        RequiresCooling &&
        RequiresStewardWitness &&
        !AllowsPressureAsPromptAuthority &&
        !AllowsPressureAsTruth &&
        !AllowsPressureAsWarrant &&
        !AllowsProviderCall &&
        !AllowsModelBinding &&
        !AllowsRuntimeStart &&
        !AllowsAction &&
        !AllowsContinuityAdmission &&
        !AllowsSelfGelMutation &&
        !AllowsGelAdmission &&
        !AllowsCmeActualAdmission &&
        !AllowsHeartbeatActivation &&
        !AllowsAuthority &&
        !AllowsLispEvaluation &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement &&
        !AllowsActivation;
}

public sealed record GapCrossingArticulationRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record GapCrossingArticulationRequest(
    SharedPrimeRealityPressureEcologyReceipt? SourcePressureEcologyReceipt,
    HighEnergyArticulationCandidateReceipt? SourceHighEnergyCandidateReceipt,
    IReadOnlyList<GapCrossingPressureLaneRecord> Lanes,
    IReadOnlyList<GapCrossingArticulationSurfaceRecord> Surfaces,
    GapCrossingArticulationBoundary Boundary,
    int PriorObservationCount,
    int PriorPassageCount,
    bool RequestsPromptAuthority = false,
    bool RequestsProviderCall = false,
    bool RequestsModelBinding = false,
    bool RequestsRuntimeStart = false,
    bool RequestsAction = false,
    bool RequestsContinuityAdmission = false,
    bool RequestsSelfGelMutation = false,
    bool RequestsGelAdmission = false,
    bool RequestsCmeActualAdmission = false,
    bool RequestsHeartbeatActivation = false,
    bool RequestsAuthority = false,
    bool RequestsLispEvaluation = false,
    bool RequestsPacketEmission = false,
    bool RequestsReceiptReplay = false,
    bool RequestsPassageIncrement = false,
    bool RequestsActivation = false);

public sealed record GapCrossingArticulationReceipt(
    string ReceiptHandle,
    GapCrossingArticulationDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourcePressureEcologyReceiptHandle,
    string SourceHighEnergyCandidateReceiptHandle,
    IReadOnlyList<GapCrossingPressureLaneRecord> Lanes,
    IReadOnlyList<GapCrossingArticulationSurfaceRecord> Surfaces,
    GapCrossingArticulationBoundary Boundary,
    GapCrossingArticulationRefusalReceipt? Refusal,
    int PriorObservationCount,
    int ObservationCountAfterGapCrossing,
    int PriorPassageCount,
    int PassageCountAfterGapCrossing,
    int LaneCount,
    int SurfaceCount,
    bool ReviewOnly,
    bool GapCrossingObserved,
    bool PressureCarriedToArticulation,
    bool ArticulationSurfaceSelected,
    bool StewardWitnessPreserved,
    bool CoolingPreserved,
    bool ReturnPathPreserved,
    bool LlmSurfaceParticipated,
    bool PressureBecamePromptAuthority,
    bool PressureBecameTruth,
    bool PressureBecameWarrant,
    bool ProviderCallMade,
    bool ModelBound,
    bool RuntimeStarted,
    bool ActionAuthorized,
    bool ContinuityAdmitted,
    bool SelfGelMutated,
    bool GelAdmitted,
    bool CmeActualAdmitted,
    bool HeartbeatActive,
    bool AuthorityGranted,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool PassageIncremented,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdGapCrossingArticulation =>
        Disposition == GapCrossingArticulationDisposition.CrossedForReviewCold &&
        Refusal is null &&
        Lanes.Count > 0 &&
        Surfaces.Count > 0 &&
        Lanes.All(static lane => lane.IsColdLane) &&
        Surfaces.All(static surface => surface.IsColdSurface) &&
        Boundary.IsColdBoundary &&
        ObservationCountAfterGapCrossing == PriorObservationCount + 1 &&
        PassageCountAfterGapCrossing == PriorPassageCount &&
        LaneCount == Lanes.Count &&
        SurfaceCount == Surfaces.Count &&
        ReviewOnly &&
        GapCrossingObserved &&
        PressureCarriedToArticulation &&
        ArticulationSurfaceSelected &&
        StewardWitnessPreserved &&
        CoolingPreserved &&
        ReturnPathPreserved &&
        LlmSurfaceParticipated &&
        NoForbiddenPromotion;

    [JsonIgnore]
    public bool IsRetainedGapCrossingRefusal =>
        Disposition == GapCrossingArticulationDisposition.Refused &&
        Refusal?.Retained == true &&
        ObservationCountAfterGapCrossing == PriorObservationCount &&
        PassageCountAfterGapCrossing == PriorPassageCount &&
        LaneCount == 0 &&
        SurfaceCount == 0 &&
        ReviewOnly &&
        !GapCrossingObserved &&
        !PressureCarriedToArticulation &&
        !ArticulationSurfaceSelected &&
        !LlmSurfaceParticipated &&
        NoForbiddenPromotion;

    private bool NoForbiddenPromotion =>
        !PressureBecamePromptAuthority &&
        !PressureBecameTruth &&
        !PressureBecameWarrant &&
        !ProviderCallMade &&
        !ModelBound &&
        !RuntimeStarted &&
        !ActionAuthorized &&
        !ContinuityAdmitted &&
        !SelfGelMutated &&
        !GelAdmitted &&
        !CmeActualAdmitted &&
        !HeartbeatActive &&
        !AuthorityGranted &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !PassageIncremented &&
        ActivationRefused;
}

public sealed class DefaultGapCrossingArticulationBoundaryValidator
{
    public GapCrossingArticulationReceipt Cross(
        GapCrossingArticulationRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Boundary is null ||
            !request.Boundary.Present ||
            string.IsNullOrWhiteSpace(request.Boundary.BoundaryCode))
        {
            return Refuse(
                request,
                "gap-crossing-boundary-missing",
                "Gap crossing refused because a review-only articulation boundary is required before pressure may approach an active LLM surface.",
                timestampUtc);
        }

        if (!request.Boundary.IsColdBoundary)
        {
            return Refuse(
                request,
                "gap-crossing-promotional-boundary",
                "Gap crossing refused because the boundary must require Shared Prime pressure ecology, high-energy articulation candidates, lane classification, surface selection, cooling, and Steward witness while refusing prompt authority, truth, warrant, provider calls, model binding, runtime start, action, continuity, SelfGEL mutation, GEL admission, CME.Actual admission, heartbeat activation, authority, Lisp evaluation, packet emission, replay, passage, and activation.",
                timestampUtc);
        }

        if (ForbiddenMotionRequested(request))
        {
            return Refuse(
                request,
                "gap-crossing-forbidden-motion-requested",
                "Gap crossing refused because articulation approach may not request prompt authority, provider calls, model binding, runtime start, action, continuity, SelfGEL mutation, GEL admission, CME.Actual admission, heartbeat activation, authority, Lisp evaluation, packet emission, receipt replay, passage, or activation.",
                timestampUtc);
        }

        if (request.SourcePressureEcologyReceipt?.IsColdPressureEcologyObservation != true)
        {
            return Refuse(
                request,
                "gap-crossing-pressure-ecology-source-invalid",
                "Gap crossing refused because a cold Shared Prime Reality pressure ecology receipt is required before pressure may approach articulation.",
                timestampUtc);
        }

        if (request.SourceHighEnergyCandidateReceipt?.IsColdHighEnergyArticulationCandidate != true)
        {
            return Refuse(
                request,
                "gap-crossing-high-energy-source-invalid",
                "Gap crossing refused because a cold high-energy articulation candidate receipt is required before any LLM surface may participate.",
                timestampUtc);
        }

        if (request.Surfaces is null ||
            request.Surfaces.Count == 0 ||
            request.Surfaces.Any(static surface => !surface.IsColdSurface))
        {
            return Refuse(
                request,
                "gap-crossing-surface-invalid",
                "Gap crossing refused because every articulation surface must remain review-only, candidate-only, public-interface-only, observable-behavior-only, lineage-preserving, non-agentic, non-calling, non-binding, non-actualizing, and non-authorizing.",
                timestampUtc);
        }

        if (HasDuplicate(request.Surfaces.Select(static surface => surface.SurfaceHandle)))
        {
            return Refuse(
                request,
                "gap-crossing-duplicate-surface-handle",
                "Gap crossing refused because duplicate articulation surface handles would collapse LLM surface lineage.",
                timestampUtc);
        }

        if (request.Lanes is null ||
            request.Lanes.Count == 0 ||
            request.Lanes.Any(static lane => !lane.IsColdLane))
        {
            return Refuse(
                request,
                "gap-crossing-lane-invalid",
                "Gap crossing refused because every pressure lane must carry pressure toward articulation as review material only while refusing prompt authority, truth, warrant, model binding, provider call, runtime start, action, continuity, SelfGEL mutation, GEL admission, CME.Actual admission, heartbeat activation, Lisp evaluation, packet emission, replay, passage, and activation.",
                timestampUtc);
        }

        if (HasDuplicate(request.Lanes.Select(static lane => lane.LaneHandle)))
        {
            return Refuse(
                request,
                "gap-crossing-duplicate-lane-handle",
                "Gap crossing refused because duplicate pressure lane handles would collapse gap-crossing lineage.",
                timestampUtc);
        }

        if (!SurfacesBindToCandidates(request))
        {
            return Refuse(
                request,
                "gap-crossing-surface-candidate-unbound",
                "Gap crossing refused because every articulation surface must bind to a known high-energy candidate handle.",
                timestampUtc);
        }

        if (!LanesBindToSourceReceipts(request))
        {
            return Refuse(
                request,
                "gap-crossing-lane-lineage-unbound",
                "Gap crossing refused because every pressure lane must bind to a known pressure signal, pressure destination, candidate engine, and articulation surface.",
                timestampUtc);
        }

        return CreateReceipt(
            request,
            GapCrossingArticulationDisposition.CrossedForReviewCold,
            "gap-crossing-articulation-carried-review-only",
            "Gap crossing carried Shared Prime pressure toward high-energy articulation surfaces as review-only cognitive material. The LLM surface may participate as an unbound articulation surface while refusing prompt authority, provider calls, model binding, runtime start, action, continuity, SelfGEL mutation, GEL admission, CME.Actual admission, heartbeat activation, authority, Lisp evaluation, packet emission, replay, passage, or activation.",
            refusal: null,
            observationIssued: true,
            timestampUtc);
    }

    private static bool ForbiddenMotionRequested(GapCrossingArticulationRequest request) =>
        request.RequestsPromptAuthority ||
        request.RequestsProviderCall ||
        request.RequestsModelBinding ||
        request.RequestsRuntimeStart ||
        request.RequestsAction ||
        request.RequestsContinuityAdmission ||
        request.RequestsSelfGelMutation ||
        request.RequestsGelAdmission ||
        request.RequestsCmeActualAdmission ||
        request.RequestsHeartbeatActivation ||
        request.RequestsAuthority ||
        request.RequestsLispEvaluation ||
        request.RequestsPacketEmission ||
        request.RequestsReceiptReplay ||
        request.RequestsPassageIncrement ||
        request.RequestsActivation;

    private static bool SurfacesBindToCandidates(GapCrossingArticulationRequest request)
    {
        var candidateHandles = request.SourceHighEnergyCandidateReceipt!.Candidates
            .Select(static candidate => candidate.CandidateHandle)
            .ToHashSet(StringComparer.Ordinal);

        return request.Surfaces.All(surface => candidateHandles.Contains(surface.CandidateHandle));
    }

    private static bool LanesBindToSourceReceipts(GapCrossingArticulationRequest request)
    {
        var signalHandles = request.SourcePressureEcologyReceipt!.Signals
            .Select(static signal => signal.SignalHandle)
            .ToHashSet(StringComparer.Ordinal);
        var destinationHandles = request.SourcePressureEcologyReceipt.Destinations
            .Select(static destination => destination.DestinationHandle)
            .ToHashSet(StringComparer.Ordinal);
        var candidateHandles = request.SourceHighEnergyCandidateReceipt!.Candidates
            .Select(static candidate => candidate.CandidateHandle)
            .ToHashSet(StringComparer.Ordinal);
        var surfaceHandles = request.Surfaces
            .Select(static surface => surface.SurfaceHandle)
            .ToHashSet(StringComparer.Ordinal);

        return request.Lanes.All(lane =>
            signalHandles.Contains(lane.SourceSignalHandle) &&
            destinationHandles.Contains(lane.SourceDestinationHandle) &&
            candidateHandles.Contains(lane.CandidateHandle) &&
            surfaceHandles.Contains(lane.ArticulationSurfaceHandle));
    }

    private static GapCrossingArticulationReceipt Refuse(
        GapCrossingArticulationRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            GapCrossingArticulationDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new GapCrossingArticulationRefusalReceipt(
                ReceiptHandle: $"urn:san:gap-crossing-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            observationIssued: false,
            timestampUtc);

    private static GapCrossingArticulationReceipt CreateReceipt(
        GapCrossingArticulationRequest request,
        GapCrossingArticulationDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        GapCrossingArticulationRefusalReceipt? refusal,
        bool observationIssued,
        DateTimeOffset timestampUtc)
    {
        var retained = refusal is null;
        var lanes = retained ? request.Lanes.ToArray() : [];
        var surfaces = retained ? request.Surfaces.ToArray() : [];

        return new(
            ReceiptHandle: $"urn:san:gap-crossing:{(retained ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourcePressureEcologyReceiptHandle: request.SourcePressureEcologyReceipt?.ReceiptHandle ?? "missing-pressure-ecology-source",
            SourceHighEnergyCandidateReceiptHandle: request.SourceHighEnergyCandidateReceipt?.ReceiptHandle ?? "missing-high-energy-candidate-source",
            Lanes: lanes,
            Surfaces: surfaces,
            Boundary: request.Boundary,
            Refusal: refusal,
            PriorObservationCount: request.PriorObservationCount,
            ObservationCountAfterGapCrossing: observationIssued ? request.PriorObservationCount + 1 : request.PriorObservationCount,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterGapCrossing: request.PriorPassageCount,
            LaneCount: retained ? lanes.Length : 0,
            SurfaceCount: retained ? surfaces.Length : 0,
            ReviewOnly: true,
            GapCrossingObserved: retained,
            PressureCarriedToArticulation: retained,
            ArticulationSurfaceSelected: retained,
            StewardWitnessPreserved: retained,
            CoolingPreserved: retained,
            ReturnPathPreserved: retained,
            LlmSurfaceParticipated: retained,
            PressureBecamePromptAuthority: false,
            PressureBecameTruth: false,
            PressureBecameWarrant: false,
            ProviderCallMade: false,
            ModelBound: false,
            RuntimeStarted: false,
            ActionAuthorized: false,
            ContinuityAdmitted: false,
            SelfGelMutated: false,
            GelAdmitted: false,
            CmeActualAdmitted: false,
            HeartbeatActive: false,
            AuthorityGranted: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            PassageIncremented: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static string SourceHandle(GapCrossingArticulationRequest request) =>
        request.SourcePressureEcologyReceipt?.ReceiptHandle ??
        request.SourceHighEnergyCandidateReceipt?.ReceiptHandle ??
        request.Lanes?.FirstOrDefault()?.LaneHandle ??
        "missing-gap-crossing-source";

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
        return Convert.ToHexString(bytes).ToLowerInvariant()[..16];
    }
}
