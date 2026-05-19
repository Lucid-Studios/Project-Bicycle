using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum MembraneMorphologyTransitionDisposition
{
    TransitionRetainedCold = 0,
    EmptyReviewCold = 1,
    Refused = 2
}

public enum MembraneMorphologyTransitionClass
{
    ElasticDeformation = 0,
    LawfulMalformation = 1,
    CompostableResidue = 2,
    RepairableTransition = 3,
    StableMorphologyCandidate = 4,
    ReturnToPrimeCooling = 5,
    CorruptionAttempt = 6
}

public sealed record MembraneMorphologyPressureVector(
    decimal ArticulationPressure,
    decimal DeformationPressure,
    decimal MalformationPressure,
    decimal CompostPressure,
    decimal RepairPressure,
    decimal CoolingPressure)
{
    public bool IsBounded =>
        IsUnit(ArticulationPressure) &&
        IsUnit(DeformationPressure) &&
        IsUnit(MalformationPressure) &&
        IsUnit(CompostPressure) &&
        IsUnit(RepairPressure) &&
        IsUnit(CoolingPressure);

    private static bool IsUnit(decimal value) => value is >= 0m and <= 1m;
}

public sealed record MembraneMorphologyTransition(
    string TransitionHandle,
    MembraneMorphologyTransitionClass TransitionClass,
    string SourceHighEnergyCandidateReceiptHandle,
    string SourceCandidateHandle,
    string ZedDeltaChamberReceiptHandle,
    string ZedDeltaOriginHandle,
    string ConditionalOeHandle,
    string ConditionalSelfGelHandle,
    string MembraneHandle,
    string EvidenceHandle,
    string WitnessHandle,
    string CoolingHandle,
    string CustodyOwner,
    MembraneMorphologyPressureVector PressureVector,
    bool ReviewOnly,
    bool TransitionOnly,
    bool MembraneOnly,
    bool MorphologyCandidateOnly,
    bool MembraneMayDeform,
    bool MalformationMayBeWitnessed,
    bool CompostMayBeRetained,
    bool RepairMayBeRouted,
    bool ReturnToPrimeAllowed,
    bool PreservesHighEnergyCandidateLineage,
    bool PreservesChamberLineage,
    bool PreservesConditionalOeLineage,
    bool PreservesConditionalSelfGelLineage,
    bool CorruptionAttempted,
    bool CoreMutated,
    bool IdentityMutated,
    bool SelfGelMutated,
    bool OeMutated,
    bool ModelBindingRequested,
    bool ProviderCallRequested,
    bool HeartbeatActivationRequested,
    bool CmeActualAdmissionRequested,
    bool RuntimeStartRequested,
    bool ActionAuthorizationRequested,
    bool ContinuityAdmissionRequested,
    bool AuthorityRequested,
    bool LispEvaluationRequested,
    bool PacketEmissionRequested,
    bool ReceiptReplayRequested,
    bool PassageIncrementRequested,
    bool ActivationRequested)
{
    public bool IsColdTransition =>
        !string.IsNullOrWhiteSpace(TransitionHandle) &&
        !string.IsNullOrWhiteSpace(SourceHighEnergyCandidateReceiptHandle) &&
        !string.IsNullOrWhiteSpace(SourceCandidateHandle) &&
        !string.IsNullOrWhiteSpace(ZedDeltaChamberReceiptHandle) &&
        !string.IsNullOrWhiteSpace(ZedDeltaOriginHandle) &&
        !string.IsNullOrWhiteSpace(ConditionalOeHandle) &&
        !string.IsNullOrWhiteSpace(ConditionalSelfGelHandle) &&
        !string.IsNullOrWhiteSpace(MembraneHandle) &&
        !string.IsNullOrWhiteSpace(EvidenceHandle) &&
        !string.IsNullOrWhiteSpace(WitnessHandle) &&
        !string.IsNullOrWhiteSpace(CoolingHandle) &&
        !string.IsNullOrWhiteSpace(CustodyOwner) &&
        PressureVector.IsBounded &&
        TransitionClass != MembraneMorphologyTransitionClass.CorruptionAttempt &&
        ReviewOnly &&
        TransitionOnly &&
        MembraneOnly &&
        MorphologyCandidateOnly &&
        MembraneMayDeform &&
        MalformationMayBeWitnessed &&
        CompostMayBeRetained &&
        RepairMayBeRouted &&
        ReturnToPrimeAllowed &&
        PreservesHighEnergyCandidateLineage &&
        PreservesChamberLineage &&
        PreservesConditionalOeLineage &&
        PreservesConditionalSelfGelLineage &&
        !CorruptionAttempted &&
        !CoreMutated &&
        !IdentityMutated &&
        !SelfGelMutated &&
        !OeMutated &&
        !ModelBindingRequested &&
        !ProviderCallRequested &&
        !HeartbeatActivationRequested &&
        !CmeActualAdmissionRequested &&
        !RuntimeStartRequested &&
        !ActionAuthorizationRequested &&
        !ContinuityAdmissionRequested &&
        !AuthorityRequested &&
        !LispEvaluationRequested &&
        !PacketEmissionRequested &&
        !ReceiptReplayRequested &&
        !PassageIncrementRequested &&
        !ActivationRequested;
}

public sealed record MembraneTransitionScopeBoundary(
    string BoundaryCode,
    bool Present,
    bool ReviewOnly,
    bool AllowsMembraneDeformation,
    bool AllowsMalformationWitness,
    bool AllowsCompostRetention,
    bool AllowsRepairRouting,
    bool AllowsTransitionEvidence,
    bool AllowsCoreMutation,
    bool AllowsIdentityMutation,
    bool AllowsSelfGelMutation,
    bool AllowsOeMutation,
    bool AllowsModelBinding,
    bool AllowsProviderCall,
    bool AllowsHeartbeatActivation,
    bool AllowsCmeActualAdmission,
    bool AllowsRuntimeStart,
    bool AllowsActionAuthorization,
    bool AllowsContinuityAdmission,
    bool AllowsAuthority,
    bool AllowsLispEvaluation,
    bool AllowsPacketEmission,
    bool AllowsReceiptReplay,
    bool AllowsPassageIncrement,
    bool AllowsActivation)
{
    public bool IsColdScope =>
        !string.IsNullOrWhiteSpace(BoundaryCode) &&
        Present &&
        ReviewOnly &&
        AllowsMembraneDeformation &&
        AllowsMalformationWitness &&
        AllowsCompostRetention &&
        AllowsRepairRouting &&
        AllowsTransitionEvidence &&
        !AllowsCoreMutation &&
        !AllowsIdentityMutation &&
        !AllowsSelfGelMutation &&
        !AllowsOeMutation &&
        !AllowsModelBinding &&
        !AllowsProviderCall &&
        !AllowsHeartbeatActivation &&
        !AllowsCmeActualAdmission &&
        !AllowsRuntimeStart &&
        !AllowsActionAuthorization &&
        !AllowsContinuityAdmission &&
        !AllowsAuthority &&
        !AllowsLispEvaluation &&
        !AllowsPacketEmission &&
        !AllowsReceiptReplay &&
        !AllowsPassageIncrement &&
        !AllowsActivation;
}

public sealed record MembraneCoreNonMutationBoundary(
    string BoundaryLaw,
    bool MembraneMayDeform,
    bool CoreMayMutate,
    bool MalformationMayBecomeFailure,
    bool CompostMayBecomeContinuity,
    bool TransitionEvidenceMayAuthorize,
    bool DeformationMayBindEngine,
    bool TransitionMayActivateHeartbeat,
    bool TransitionMayAdmitCmeActual,
    bool TransitionMayStartRuntime,
    bool TransitionMayAuthorizeAction,
    bool TransitionMayAdmitContinuity,
    bool TransitionMayGrantAuthority,
    bool TransitionMayEvaluateLisp,
    bool TransitionMayEmitPacket,
    bool TransitionMayReplayReceipt,
    bool TransitionMayIncrementPassage,
    bool TransitionMayActivate,
    bool RequiresHighEnergyCandidate,
    bool RequiresWitness,
    bool RequiresCooling,
    bool RequiresAuthorityAbsence)
{
    public bool IsColdNonMutation =>
        !string.IsNullOrWhiteSpace(BoundaryLaw) &&
        MembraneMayDeform &&
        !CoreMayMutate &&
        !MalformationMayBecomeFailure &&
        !CompostMayBecomeContinuity &&
        !TransitionEvidenceMayAuthorize &&
        !DeformationMayBindEngine &&
        !TransitionMayActivateHeartbeat &&
        !TransitionMayAdmitCmeActual &&
        !TransitionMayStartRuntime &&
        !TransitionMayAuthorizeAction &&
        !TransitionMayAdmitContinuity &&
        !TransitionMayGrantAuthority &&
        !TransitionMayEvaluateLisp &&
        !TransitionMayEmitPacket &&
        !TransitionMayReplayReceipt &&
        !TransitionMayIncrementPassage &&
        !TransitionMayActivate &&
        RequiresHighEnergyCandidate &&
        RequiresWitness &&
        RequiresCooling &&
        RequiresAuthorityAbsence;
}

public sealed record MorphologicalCompostBoundary(
    string BoundaryLaw,
    bool MalformationMayBeWitnessed,
    bool MalformationMayBeRetainedAsCompost,
    bool CompostMayRouteRepair,
    bool CompostMayReturnToPrime,
    bool CorruptionMayBeNormalized,
    bool CorruptionMayMutateCore,
    bool CompostMayEraseLineage,
    bool CompostMayGrantAuthority,
    bool RepairMaySkipWitness,
    bool CoolingMayBeSkipped,
    bool CompostMayBindModel,
    bool CompostMayActivateHeartbeat,
    bool CompostMayAdmitCmeActual,
    bool CompostMayStartRuntime,
    bool CompostMayAuthorizeAction,
    bool CompostMayAdmitContinuity,
    bool CompostMayEvaluateLisp,
    bool CompostMayEmitPacket,
    bool CompostMayReplayReceipt,
    bool CompostMayIncrementPassage,
    bool CompostMayActivate)
{
    public bool IsColdCompost =>
        !string.IsNullOrWhiteSpace(BoundaryLaw) &&
        MalformationMayBeWitnessed &&
        MalformationMayBeRetainedAsCompost &&
        CompostMayRouteRepair &&
        CompostMayReturnToPrime &&
        !CorruptionMayBeNormalized &&
        !CorruptionMayMutateCore &&
        !CompostMayEraseLineage &&
        !CompostMayGrantAuthority &&
        !RepairMaySkipWitness &&
        !CoolingMayBeSkipped &&
        !CompostMayBindModel &&
        !CompostMayActivateHeartbeat &&
        !CompostMayAdmitCmeActual &&
        !CompostMayStartRuntime &&
        !CompostMayAuthorizeAction &&
        !CompostMayAdmitContinuity &&
        !CompostMayEvaluateLisp &&
        !CompostMayEmitPacket &&
        !CompostMayReplayReceipt &&
        !CompostMayIncrementPassage &&
        !CompostMayActivate;
}

public sealed record MembraneMorphologyTransitionRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record MembraneMorphologyTransitionRequest(
    HighEnergyArticulationCandidateReceipt? SourceHighEnergyCandidateReceipt,
    IReadOnlyList<MembraneMorphologyTransition> Transitions,
    MembraneTransitionScopeBoundary ScopeBoundary,
    MembraneCoreNonMutationBoundary NonMutationBoundary,
    MorphologicalCompostBoundary CompostBoundary,
    int PriorPassageCount);

public sealed record MembraneMorphologyTransitionReceipt(
    string ReceiptHandle,
    MembraneMorphologyTransitionDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceHighEnergyCandidateReceiptHandle,
    IReadOnlyList<MembraneMorphologyTransition> Transitions,
    MembraneTransitionScopeBoundary ScopeBoundary,
    MembraneCoreNonMutationBoundary NonMutationBoundary,
    MorphologicalCompostBoundary CompostBoundary,
    MembraneMorphologyTransitionRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterTransitionReview,
    int TransitionCount,
    decimal MaximumObservedDeformationPressure,
    bool ReviewOnly,
    bool TransitionOnly,
    bool MembraneDeformed,
    bool MalformationWitnessed,
    bool CompostRetained,
    bool TransitionEvidenceRetained,
    bool HighEnergyPressureReferenced,
    bool CoreMutated,
    bool IdentityMutated,
    bool SelfGelMutated,
    bool OeMutated,
    bool ModelBound,
    bool ProviderCallMade,
    bool HeartbeatActive,
    bool CmeActualAdmitted,
    bool RuntimeStarted,
    bool ActionAuthorized,
    bool ContinuityAdmitted,
    bool AuthorityGranted,
    bool LispEvaluationAllowed,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool PassageIncremented,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdMembraneMorphologyTransition =>
        Disposition == MembraneMorphologyTransitionDisposition.TransitionRetainedCold &&
        Refusal is null &&
        Transitions.Count > 0 &&
        Transitions.All(static transition => transition.IsColdTransition) &&
        ScopeBoundary.IsColdScope &&
        NonMutationBoundary.IsColdNonMutation &&
        CompostBoundary.IsColdCompost &&
        PassageCountAfterTransitionReview == PriorPassageCount &&
        TransitionCount == Transitions.Count &&
        MaximumObservedDeformationPressure == Transitions.Max(static transition => transition.PressureVector.DeformationPressure) &&
        ReviewOnly &&
        TransitionOnly &&
        MembraneDeformed &&
        MalformationWitnessed &&
        CompostRetained &&
        TransitionEvidenceRetained &&
        HighEnergyPressureReferenced &&
        !CoreMutated &&
        !IdentityMutated &&
        !SelfGelMutated &&
        !OeMutated &&
        !ModelBound &&
        !ProviderCallMade &&
        !HeartbeatActive &&
        !CmeActualAdmitted &&
        !RuntimeStarted &&
        !ActionAuthorized &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !PassageIncremented &&
        ActivationRefused;

    public bool IsColdEmptyMembraneMorphologyTransition =>
        Disposition == MembraneMorphologyTransitionDisposition.EmptyReviewCold &&
        Refusal is null &&
        Transitions.Count == 0 &&
        PassageCountAfterTransitionReview == PriorPassageCount &&
        TransitionCount == 0 &&
        MaximumObservedDeformationPressure == 0m &&
        ReviewOnly &&
        TransitionOnly &&
        !MembraneDeformed &&
        !MalformationWitnessed &&
        !CompostRetained &&
        !TransitionEvidenceRetained &&
        !HighEnergyPressureReferenced &&
        !CoreMutated &&
        !IdentityMutated &&
        !SelfGelMutated &&
        !OeMutated &&
        !ModelBound &&
        !ProviderCallMade &&
        !HeartbeatActive &&
        !CmeActualAdmitted &&
        !RuntimeStarted &&
        !ActionAuthorized &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !PassageIncremented &&
        ActivationRefused;

    public bool IsRetainedMembraneMorphologyTransitionRefusal =>
        Disposition == MembraneMorphologyTransitionDisposition.Refused &&
        Refusal?.Retained == true &&
        PassageCountAfterTransitionReview == PriorPassageCount &&
        TransitionCount == 0 &&
        MaximumObservedDeformationPressure == 0m &&
        ReviewOnly &&
        TransitionOnly &&
        !MembraneDeformed &&
        !MalformationWitnessed &&
        !CompostRetained &&
        !TransitionEvidenceRetained &&
        !HighEnergyPressureReferenced &&
        !CoreMutated &&
        !IdentityMutated &&
        !SelfGelMutated &&
        !OeMutated &&
        !ModelBound &&
        !ProviderCallMade &&
        !HeartbeatActive &&
        !CmeActualAdmitted &&
        !RuntimeStarted &&
        !ActionAuthorized &&
        !ContinuityAdmitted &&
        !AuthorityGranted &&
        !LispEvaluationAllowed &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        !PassageIncremented &&
        ActivationRefused;
}

public sealed class DefaultMembraneMorphologyTransitionBoundaryValidator
{
    public MembraneMorphologyTransitionReceipt Declare(
        MembraneMorphologyTransitionRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceHighEnergyCandidateReceipt is null ||
            !request.SourceHighEnergyCandidateReceipt.IsColdHighEnergyArticulationCandidate ||
            !request.SourceHighEnergyCandidateReceipt.HighEnergyBodyNamed ||
            request.SourceHighEnergyCandidateReceipt.ModelBound ||
            request.SourceHighEnergyCandidateReceipt.ProviderCallMade ||
            request.SourceHighEnergyCandidateReceipt.HeartbeatActive ||
            request.SourceHighEnergyCandidateReceipt.CmeActualAdmitted)
        {
            return Refuse(
                request,
                "membrane-morphology-source-high-energy-candidate-missing",
                "Membrane morphology transition refused because a cold high-energy articulation candidate receipt is required before membrane transition may be witnessed.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.IsColdScope)
        {
            return Refuse(
                request,
                "membrane-morphology-scope-boundary-invalid",
                "Membrane morphology transition refused because the membrane may deform, witness malformation, retain compost, route repair, and retain transition evidence only as review, not core mutation, identity mutation, model binding, provider call, heartbeat activation, CME.Actual, runtime, action, continuity, authority, Lisp evaluation, packet emission, replay, passage, or activation.",
                timestampUtc);
        }

        if (!request.NonMutationBoundary.IsColdNonMutation)
        {
            return Refuse(
                request,
                "membrane-morphology-core-non-mutation-invalid",
                "Membrane morphology transition refused because membrane deformation must not mutate the core, convert malformation to failure, convert compost to continuity, authorize transition evidence, bind an engine, activate heartbeat, admit CME.Actual, start runtime, authorize action, admit continuity, grant authority, evaluate Lisp, emit packets, replay receipts, increment passage, or activate.",
                timestampUtc);
        }

        if (!request.CompostBoundary.IsColdCompost)
        {
            return Refuse(
                request,
                "membrane-morphology-compost-boundary-invalid",
                "Membrane morphology transition refused because malformation may be retained as compost only with lineage, witness, cooling, repair routing, and return-to-Prime posture; corruption may not be normalized or used as authority, continuity, model binding, heartbeat, CME.Actual, runtime, action, Lisp evaluation, packet emission, replay, passage, or activation.",
                timestampUtc);
        }

        if (request.Transitions.Count == 0)
        {
            return CreateReceipt(
                request,
                MembraneMorphologyTransitionDisposition.EmptyReviewCold,
                "membrane-morphology-transition-empty-review-only",
                "Membrane morphology transition reviewed an empty transition set. The membrane remains available for review-only deformation, but no transition evidence was retained.",
                refusal: null,
                timestampUtc);
        }

        if (request.Transitions.Any(static transition => !transition.IsColdTransition))
        {
            return Refuse(
                request,
                "membrane-morphology-transition-invalid",
                "Membrane morphology transition refused because every transition must remain review-only, membrane-only, candidate-only, lineage-preserving, witnessed, cooled, compostable, repair-routable, non-corrupting, non-mutating, non-binding, non-calling, non-activating, and non-authorizing.",
                timestampUtc);
        }

        if (HasDuplicate(request.Transitions.Select(static transition => transition.TransitionHandle)))
        {
            return Refuse(
                request,
                "membrane-morphology-duplicate-transition-handle",
                "Membrane morphology transition refused because duplicate transition handles would collapse membrane morphology lineage.",
                timestampUtc);
        }

        var source = request.SourceHighEnergyCandidateReceipt;
        var candidates = source.Candidates.ToDictionary(static candidate => candidate.CandidateHandle, StringComparer.Ordinal);
        if (request.Transitions.Any(transition =>
                !string.Equals(transition.SourceHighEnergyCandidateReceiptHandle, source.ReceiptHandle, StringComparison.Ordinal) ||
                !candidates.TryGetValue(transition.SourceCandidateHandle, out var candidate) ||
                !string.Equals(transition.ZedDeltaChamberReceiptHandle, candidate.ZedDeltaChamberReceiptHandle, StringComparison.Ordinal) ||
                !string.Equals(transition.ZedDeltaOriginHandle, candidate.ZedDeltaOriginHandle, StringComparison.Ordinal) ||
                !string.Equals(transition.ConditionalOeHandle, candidate.ConditionalOeHandle, StringComparison.Ordinal) ||
                !string.Equals(transition.ConditionalSelfGelHandle, candidate.ConditionalSelfGelHandle, StringComparison.Ordinal)))
        {
            return Refuse(
                request,
                "membrane-morphology-transition-lineage-invalid",
                "Membrane morphology transition refused because every transition must preserve high-energy candidate receipt, candidate, Zed.Delta chamber, origin, cOE, and cSelfGEL lineage.",
                timestampUtc);
        }

        var requiredClasses = Enum.GetValues<MembraneMorphologyTransitionClass>()
            .Where(static transitionClass => transitionClass != MembraneMorphologyTransitionClass.CorruptionAttempt)
            .ToArray();
        var classes = request.Transitions.Select(static transition => transition.TransitionClass).ToHashSet();
        if (requiredClasses.Any(transitionClass => !classes.Contains(transitionClass)))
        {
            return Refuse(
                request,
                "membrane-morphology-transition-class-coverage-missing",
                "Membrane morphology transition refused because elastic deformation, lawful malformation, compostable residue, repairable transition, stable morphology candidate, and return-to-Prime cooling classes must all be represented before retained transition status.",
                timestampUtc);
        }

        return CreateReceipt(
            request,
            MembraneMorphologyTransitionDisposition.TransitionRetainedCold,
            "membrane-morphology-transition-retained-review-only",
            "Membrane morphology transitions were retained for review only. High-energy articulation pressure may deform the SLI.Lisp membrane, witness malformation, retain compost, route repair, and return toward Prime while refusing core mutation, identity mutation, SelfGEL mutation, OE mutation, model binding, provider call, heartbeat activation, CME.Actual admission, runtime start, action, continuity, authority, Lisp evaluation, packet emission, replay, passage, or activation.",
            refusal: null,
            timestampUtc);
    }

    private static MembraneMorphologyTransitionReceipt Refuse(
        MembraneMorphologyTransitionRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        CreateReceipt(
            request,
            MembraneMorphologyTransitionDisposition.Refused,
            outcomeCode,
            governanceTrace,
            new MembraneMorphologyTransitionRefusalReceipt(
                ReceiptHandle: $"urn:san:membrane-morphology-transition-refusal:{ShortHash(SourceHandle(request), outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            timestampUtc);

    private static MembraneMorphologyTransitionReceipt CreateReceipt(
        MembraneMorphologyTransitionRequest request,
        MembraneMorphologyTransitionDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        MembraneMorphologyTransitionRefusalReceipt? refusal,
        DateTimeOffset timestampUtc)
    {
        var admitted = refusal is null && request.Transitions.Count > 0;
        var empty = refusal is null && request.Transitions.Count == 0;
        var transitions = admitted ? request.Transitions.ToArray() : [];
        return new(
            ReceiptHandle: $"urn:san:membrane-morphology-transition:{(refusal is null ? "review" : "refused")}:{ShortHash(SourceHandle(request), outcomeCode, request.Transitions.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceHighEnergyCandidateReceiptHandle: SourceHandle(request),
            Transitions: transitions,
            ScopeBoundary: request.ScopeBoundary,
            NonMutationBoundary: request.NonMutationBoundary,
            CompostBoundary: request.CompostBoundary,
            Refusal: refusal,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterTransitionReview: request.PriorPassageCount,
            TransitionCount: admitted ? request.Transitions.Count : 0,
            MaximumObservedDeformationPressure: admitted ? request.Transitions.Max(static transition => transition.PressureVector.DeformationPressure) : 0m,
            ReviewOnly: true,
            TransitionOnly: true,
            MembraneDeformed: admitted,
            MalformationWitnessed: admitted,
            CompostRetained: admitted,
            TransitionEvidenceRetained: admitted,
            HighEnergyPressureReferenced: admitted,
            CoreMutated: false,
            IdentityMutated: false,
            SelfGelMutated: false,
            OeMutated: false,
            ModelBound: false,
            ProviderCallMade: false,
            HeartbeatActive: false,
            CmeActualAdmitted: false,
            RuntimeStarted: false,
            ActionAuthorized: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            LispEvaluationAllowed: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            PassageIncremented: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static string SourceHandle(MembraneMorphologyTransitionRequest request) =>
        request.SourceHighEnergyCandidateReceipt?.ReceiptHandle ?? "missing-high-energy-candidate-source";

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
