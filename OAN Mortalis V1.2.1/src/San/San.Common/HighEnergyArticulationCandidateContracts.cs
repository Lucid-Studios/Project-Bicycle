using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum HighEnergyArticulationCandidateDisposition
{
    CandidateNamedCold = 0,
    Refused = 1
}

public enum HighEnergyArticulationCandidateRole
{
    MainBodyEngineCandidate = 0,
    GovernanceReviewCandidate = 1,
    InstantiatedCmeTestBodyCandidate = 2,
    ComparativeUniversalityCandidate = 3,
    LocalSlmCandidate = 4
}

public enum HighEnergyProviderInterfaceClass
{
    OfficialPublicDocumentation = 0,
    PublishedApiContract = 1,
    ObservableConversationBehavior = 2,
    LocalRuntimeAdapterDescription = 3,
    ComparativeEvaluationSurface = 4
}

public sealed record HighEnergyArticulationCandidate(
    string CandidateHandle,
    HighEnergyArticulationCandidateRole CandidateRole,
    HighEnergyProviderInterfaceClass InterfaceClass,
    string ProviderFamily,
    string ModelLine,
    string IntendedRole,
    string ZedDeltaChamberReceiptHandle,
    string ZedDeltaOriginHandle,
    string ConditionalOeHandle,
    string ConditionalSelfGelHandle,
    string TelemetryShapeHandle,
    string PublicDocumentationHandle,
    string WitnessHandle,
    string CustodyOwner,
    bool ReviewOnly,
    bool CandidateOnly,
    bool RoleTyped,
    bool PublicInterfaceOnly,
    bool ObservableBehaviorOnly,
    bool PreservesChamberLineage,
    bool PreservesConditionalOeLineage,
    bool PreservesConditionalSelfGelLineage,
    bool ProviderCallRequested,
    bool ModelBindingRequested,
    bool HiddenSubstrateClaimed,
    bool WeightAccessClaimed,
    bool TrainingDataClaimed,
    bool PersistentMemoryClaimed,
    bool RuntimeIdentityClaimed,
    bool HeartbeatActivationRequested,
    bool CmeActualAdmissionRequested,
    bool ActionAuthorizationRequested,
    bool ContinuityAdmissionRequested,
    bool AuthorityRequested,
    bool LispEvaluationRequested,
    bool PacketEmissionRequested,
    bool ReceiptReplayRequested,
    bool PassageIncrementRequested,
    bool ActivationRequested)
{
    public bool IsColdCandidate =>
        !string.IsNullOrWhiteSpace(CandidateHandle) &&
        !string.IsNullOrWhiteSpace(ProviderFamily) &&
        !string.IsNullOrWhiteSpace(ModelLine) &&
        !string.IsNullOrWhiteSpace(IntendedRole) &&
        !string.IsNullOrWhiteSpace(ZedDeltaChamberReceiptHandle) &&
        !string.IsNullOrWhiteSpace(ZedDeltaOriginHandle) &&
        !string.IsNullOrWhiteSpace(ConditionalOeHandle) &&
        !string.IsNullOrWhiteSpace(ConditionalSelfGelHandle) &&
        !string.IsNullOrWhiteSpace(TelemetryShapeHandle) &&
        !string.IsNullOrWhiteSpace(PublicDocumentationHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(CustodyOwner) &&
        ReviewOnly &&
        CandidateOnly &&
        RoleTyped &&
        PublicInterfaceOnly &&
        ObservableBehaviorOnly &&
        PreservesChamberLineage &&
        PreservesConditionalOeLineage &&
        PreservesConditionalSelfGelLineage &&
        !ProviderCallRequested &&
        !ModelBindingRequested &&
        !HiddenSubstrateClaimed &&
        !WeightAccessClaimed &&
        !TrainingDataClaimed &&
        !PersistentMemoryClaimed &&
        !RuntimeIdentityClaimed &&
        !HeartbeatActivationRequested &&
        !CmeActualAdmissionRequested &&
        !ActionAuthorizationRequested &&
        !ContinuityAdmissionRequested &&
        !AuthorityRequested &&
        !LispEvaluationRequested &&
        !PacketEmissionRequested &&
        !ReceiptReplayRequested &&
        !PassageIncrementRequested &&
        !ActivationRequested;
}

public sealed record ProviderInterfaceObservationBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool PublicObservableOnly,
    bool AllowsOfficialDocumentationReference,
    bool AllowsPublishedApiContractReference,
    bool AllowsObservableBehaviorStudy,
    bool AllowsProviderCall,
    bool AllowsProviderVisibleAccess,
    bool AllowsModelContextExport,
    bool AllowsScraping,
    bool AllowsHiddenInternalsMapping,
    bool AllowsWeightAccess,
    bool AllowsTrainingDataInference,
    bool AllowsPersistentMemoryClaim,
    bool AllowsRuntimeIdentityClaim,
    bool AllowsAuthority)
{
    public bool IsColdObservationBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryCode) &&
        Present &&
        ReviewOnly &&
        PublicObservableOnly &&
        AllowsOfficialDocumentationReference &&
        AllowsPublishedApiContractReference &&
        AllowsObservableBehaviorStudy &&
        !AllowsProviderCall &&
        !AllowsProviderVisibleAccess &&
        !AllowsModelContextExport &&
        !AllowsScraping &&
        !AllowsHiddenInternalsMapping &&
        !AllowsWeightAccess &&
        !AllowsTrainingDataInference &&
        !AllowsPersistentMemoryClaim &&
        !AllowsRuntimeIdentityClaim &&
        !AllowsAuthority;
}

public sealed record HiddenSubstrateNonClaimBoundary(
    string BoundaryLaw,
    bool PublicInterfaceMayBeStudied,
    bool HiddenSubstrateMayBeClaimed,
    bool ProprietaryInternalsMayBeMapped,
    bool WeightsMayBeClaimed,
    bool TrainingDataMayBeClaimed,
    bool ProviderLogsMayBeClaimed,
    bool SystemPromptMayBeClaimed,
    bool FullCausalCertaintyMayBeClaimed,
    bool ObservableBehaviorMayBecomeInternalProof,
    bool DocumentationMayBecomeImplementationProof,
    bool InterfaceSuccessMayBecomeSemanticWarrant,
    bool RequiresUncertaintyRetention,
    bool RequiresSourceAttribution,
    bool RequiresNonEquivalenceClaim)
{
    public bool IsColdNonClaimBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryLaw) &&
        PublicInterfaceMayBeStudied &&
        !HiddenSubstrateMayBeClaimed &&
        !ProprietaryInternalsMayBeMapped &&
        !WeightsMayBeClaimed &&
        !TrainingDataMayBeClaimed &&
        !ProviderLogsMayBeClaimed &&
        !SystemPromptMayBeClaimed &&
        !FullCausalCertaintyMayBeClaimed &&
        !ObservableBehaviorMayBecomeInternalProof &&
        !DocumentationMayBecomeImplementationProof &&
        !InterfaceSuccessMayBecomeSemanticWarrant &&
        RequiresUncertaintyRetention &&
        RequiresSourceAttribution &&
        RequiresNonEquivalenceClaim;
}

public sealed record CandidateNonBindingBoundary(
    string BoundaryLaw,
    bool CandidateMayBeNamed,
    bool RoleMayBeAssigned,
    bool InterfaceMayBeObserved,
    bool ModelMayBind,
    bool ProviderMayBeCalled,
    bool HeartbeatMayActivate,
    bool CmeActualMayBeAdmitted,
    bool RuntimeMayStart,
    bool ActionMayBeAuthorized,
    bool ContinuityMayBeAdmitted,
    bool AuthorityMayBeGranted,
    bool IdentityMayMutate,
    bool SelfGelMayMutate,
    bool LispMayEvaluate,
    bool PacketMayEmit,
    bool ReceiptMayReplay,
    bool PassageMayIncrement,
    bool ActivationMayProceed,
    bool RequiresZedDeltaChamber,
    bool RequiresWitness,
    bool RequiresAuthorityAbsence)
{
    public bool IsColdNonBindingBoundary =>
        !string.IsNullOrWhiteSpace(BoundaryLaw) &&
        CandidateMayBeNamed &&
        RoleMayBeAssigned &&
        InterfaceMayBeObserved &&
        !ModelMayBind &&
        !ProviderMayBeCalled &&
        !HeartbeatMayActivate &&
        !CmeActualMayBeAdmitted &&
        !RuntimeMayStart &&
        !ActionMayBeAuthorized &&
        !ContinuityMayBeAdmitted &&
        !AuthorityMayBeGranted &&
        !IdentityMayMutate &&
        !SelfGelMayMutate &&
        !LispMayEvaluate &&
        !PacketMayEmit &&
        !ReceiptMayReplay &&
        !PassageMayIncrement &&
        !ActivationMayProceed &&
        RequiresZedDeltaChamber &&
        RequiresWitness &&
        RequiresAuthorityAbsence;
}

public sealed record HighEnergyArticulationCandidateRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record HighEnergyArticulationCandidateRequest(
    ZedDeltaChamberFormationReceipt? SourceZedDeltaChamberReceipt,
    IReadOnlyList<HighEnergyArticulationCandidate> Candidates,
    ProviderInterfaceObservationBoundary ObservationBoundary,
    HiddenSubstrateNonClaimBoundary NonClaimBoundary,
    CandidateNonBindingBoundary NonBindingBoundary,
    int PriorPassageCount);

public sealed record HighEnergyArticulationCandidateReceipt(
    string ReceiptHandle,
    HighEnergyArticulationCandidateDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceZedDeltaChamberReceiptHandle,
    IReadOnlyList<HighEnergyArticulationCandidate> Candidates,
    ProviderInterfaceObservationBoundary ObservationBoundary,
    HiddenSubstrateNonClaimBoundary NonClaimBoundary,
    CandidateNonBindingBoundary NonBindingBoundary,
    HighEnergyArticulationCandidateRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterCandidateReview,
    int CandidateCount,
    bool ReviewOnly,
    bool CandidateOnly,
    bool HighEnergyBodyNamed,
    bool PublicInterfaceReferenced,
    bool ProviderCallMade,
    bool ModelBound,
    bool HiddenSubstrateClaimed,
    bool HiddenInternalsMapped,
    bool WeightsClaimed,
    bool TrainingDataClaimed,
    bool PersistentMemoryClaimed,
    bool RuntimeIdentityClaimed,
    bool HeartbeatActive,
    bool CmeActualAdmitted,
    bool RuntimeStarted,
    bool ActionAuthorized,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool IdentityMutated,
    bool SelfGelMutated,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool PassageIncremented,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdHighEnergyArticulationCandidate =>
        Disposition == HighEnergyArticulationCandidateDisposition.CandidateNamedCold &&
        Refusal is null &&
        Candidates.Count > 0 &&
        Candidates.All(static candidate => candidate.IsColdCandidate) &&
        ObservationBoundary.IsColdObservationBoundary &&
        NonClaimBoundary.IsColdNonClaimBoundary &&
        NonBindingBoundary.IsColdNonBindingBoundary &&
        PassageCountAfterCandidateReview == PriorPassageCount &&
        CandidateCount == Candidates.Count &&
        ReviewOnly &&
        CandidateOnly &&
        HighEnergyBodyNamed &&
        PublicInterfaceReferenced &&
        !ProviderCallMade &&
        !ModelBound &&
        !HiddenSubstrateClaimed &&
        !HiddenInternalsMapped &&
        !WeightsClaimed &&
        !TrainingDataClaimed &&
        !PersistentMemoryClaimed &&
        !RuntimeIdentityClaimed &&
        !HeartbeatActive &&
        !CmeActualAdmitted &&
        !RuntimeStarted &&
        !ActionAuthorized &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !IdentityMutated &&
        !SelfGelMutated &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !PassageIncremented &&
        ActivationRefused;

    public bool IsRetainedHighEnergyArticulationCandidateRefusal =>
        Disposition == HighEnergyArticulationCandidateDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterCandidateReview == PriorPassageCount &&
        CandidateCount == 0 &&
        ReviewOnly &&
        CandidateOnly &&
        !HighEnergyBodyNamed &&
        !PublicInterfaceReferenced &&
        !ProviderCallMade &&
        !ModelBound &&
        !HiddenSubstrateClaimed &&
        !HiddenInternalsMapped &&
        !WeightsClaimed &&
        !TrainingDataClaimed &&
        !PersistentMemoryClaimed &&
        !RuntimeIdentityClaimed &&
        !HeartbeatActive &&
        !CmeActualAdmitted &&
        !RuntimeStarted &&
        !ActionAuthorized &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !IdentityMutated &&
        !SelfGelMutated &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !PassageIncremented &&
        ActivationRefused;
}

public sealed class DefaultHighEnergyArticulationCandidateBoundaryValidator
{
    public HighEnergyArticulationCandidateReceipt Declare(
        HighEnergyArticulationCandidateRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceZedDeltaChamberReceipt is null ||
            !request.SourceZedDeltaChamberReceipt.IsColdZedDeltaChamberFormation ||
            !request.SourceZedDeltaChamberReceipt.ChamberFormed ||
            request.SourceZedDeltaChamberReceipt.HeartbeatActive ||
            request.SourceZedDeltaChamberReceipt.CmeActualAdmitted)
        {
            return Refuse(
                request,
                "high-energy-source-zed-delta-chamber-missing",
                "High-energy articulation candidate refused because a cold Zed.Delta chamber must be formed before candidate engines may be named.",
                timestampUtc);
        }

        if (!request.ObservationBoundary.IsColdObservationBoundary)
        {
            return Refuse(
                request,
                "high-energy-provider-observation-boundary-invalid",
                "High-energy articulation candidate refused because provider interfaces may be referenced only as public, observable, review-only surfaces without calls, provider-visible access, scraping, model-context export, hidden-internals mapping, weight access, training-data inference, persistent memory claim, runtime identity claim, or authority.",
                timestampUtc);
        }

        if (!request.NonClaimBoundary.IsColdNonClaimBoundary)
        {
            return Refuse(
                request,
                "high-energy-hidden-substrate-claim-invalid",
                "High-energy articulation candidate refused because public interface observation may not become hidden-substrate knowledge, implementation proof, internal causal certainty, semantic warrant, or equivalence claim.",
                timestampUtc);
        }

        if (!request.NonBindingBoundary.IsColdNonBindingBoundary)
        {
            return Refuse(
                request,
                "high-energy-non-binding-boundary-invalid",
                "High-energy articulation candidate refused because candidate naming must not bind a model, call a provider, activate heartbeat, admit CME.Actual, start runtime, authorize action, admit continuity, grant authority, mutate identity or SelfGEL, evaluate Lisp, emit packets, replay receipts, increment passage, or activate.",
                timestampUtc);
        }

        if (request.Candidates.Count == 0)
        {
            return Refuse(
                request,
                "high-energy-candidate-missing",
                "High-energy articulation candidate refused because at least one role-typed candidate engine must be named before the candidate boundary can retain review evidence.",
                timestampUtc);
        }

        if (request.Candidates.Any(static candidate => !candidate.IsColdCandidate))
        {
            return Refuse(
                request,
                "high-energy-candidate-promotional",
                "High-energy articulation candidate refused because every candidate must remain review-only, candidate-only, public-interface-only, observable-behavior-only, role-typed, lineage-preserving, non-binding, non-calling, non-actualizing, and non-authorizing.",
                timestampUtc);
        }

        if (HasDuplicate(request.Candidates.Select(static candidate => candidate.CandidateHandle)))
        {
            return Refuse(
                request,
                "high-energy-duplicate-candidate-handle",
                "High-energy articulation candidate refused because duplicate candidate handles would collapse candidate engine lineage.",
                timestampUtc);
        }

        var source = request.SourceZedDeltaChamberReceipt;
        var coeHandles = source.ConditionalOperationalExpressions
            .Select(static standing => standing.ConditionalOeHandle)
            .ToHashSet(StringComparer.Ordinal);
        var cselfGelHandles = source.ConditionalSelfGelHolds
            .Select(static hold => hold.ConditionalSelfGelHandle)
            .ToHashSet(StringComparer.Ordinal);

        if (request.Candidates.Any(candidate =>
                !string.Equals(candidate.ZedDeltaChamberReceiptHandle, source.ReceiptHandle, StringComparison.Ordinal) ||
                !string.Equals(candidate.ZedDeltaOriginHandle, source.Origin.OriginHandle, StringComparison.Ordinal) ||
                !coeHandles.Contains(candidate.ConditionalOeHandle) ||
                !cselfGelHandles.Contains(candidate.ConditionalSelfGelHandle)))
        {
            return Refuse(
                request,
                "high-energy-candidate-lineage-invalid",
                "High-energy articulation candidate refused because every candidate must preserve Zed.Delta chamber receipt, origin, cOE, and cSelfGEL lineage.",
                timestampUtc);
        }

        var roles = request.Candidates
            .Select(static candidate => candidate.CandidateRole)
            .ToHashSet();
        if (Enum.GetValues<HighEnergyArticulationCandidateRole>().Any(role => !roles.Contains(role)))
        {
            return Refuse(
                request,
                "high-energy-candidate-role-coverage-missing",
                "High-energy articulation candidate refused because main body, governance review, instantiated CME test body, comparative universality, and local SLM candidate roles must be represented together before retained candidate status.",
                timestampUtc);
        }

        return CreateReceipt(
            request,
            HighEnergyArticulationCandidateDisposition.CandidateNamedCold,
            "high-energy-articulation-candidate-named-review-only",
            "High-energy articulation candidates were named for review only. Public interface and observable behavior may be referenced while refusing provider calls, model binding, hidden-substrate claims, weight or training-data claims, persistent memory, runtime identity, heartbeat activation, CME.Actual admission, runtime start, action, continuity, authority, Lisp evaluation, packet emission, replay, passage, or activation.",
            refusal: null,
            timestampUtc);
    }

    private static HighEnergyArticulationCandidateReceipt Refuse(
        HighEnergyArticulationCandidateRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            HighEnergyArticulationCandidateDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new HighEnergyArticulationCandidateRefusalReceipt(
                ReceiptHandle: $"urn:san:high-energy-articulation-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static HighEnergyArticulationCandidateReceipt CreateReceipt(
        HighEnergyArticulationCandidateRequest request,
        HighEnergyArticulationCandidateDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        HighEnergyArticulationCandidateRefusalReceipt? refusal,
        DateTimeOffset timestampUtc)
    {
        var admitted = refusal is null;
        return new(
            ReceiptHandle: $"urn:san:high-energy-articulation:{(admitted ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, request.Candidates.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceZedDeltaChamberReceiptHandle: SourceHandle(request),
            Candidates: admitted ? request.Candidates.ToArray() : [],
            ObservationBoundary: request.ObservationBoundary,
            NonClaimBoundary: request.NonClaimBoundary,
            NonBindingBoundary: request.NonBindingBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterCandidateReview: request.PriorPassageCount,
            CandidateCount: admitted ? request.Candidates.Count : 0,
            ReviewOnly: true,
            CandidateOnly: true,
            HighEnergyBodyNamed: admitted,
            PublicInterfaceReferenced: admitted,
            ProviderCallMade: false,
            ModelBound: false,
            HiddenSubstrateClaimed: false,
            HiddenInternalsMapped: false,
            WeightsClaimed: false,
            TrainingDataClaimed: false,
            PersistentMemoryClaimed: false,
            RuntimeIdentityClaimed: false,
            HeartbeatActive: false,
            CmeActualAdmitted: false,
            RuntimeStarted: false,
            ActionAuthorized: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            IdentityMutated: false,
            SelfGelMutated: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            PassageIncremented: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static string SourceHandle(HighEnergyArticulationCandidateRequest request) =>
        request.SourceZedDeltaChamberReceipt?.ReceiptHandle ?? "missing-high-energy-zed-delta-source";

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
        var text = string.Join("|", parts);
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(text));
        return Convert.ToHexString(hash)[..16].ToLowerInvariant();
    }
}
