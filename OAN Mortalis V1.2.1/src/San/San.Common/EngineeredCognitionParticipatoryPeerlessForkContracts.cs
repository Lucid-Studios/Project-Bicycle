using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum EcParticipatoryPeerlessForkDisposition
{
    EmptyReviewCold = 0,
    ParticipatoryReviewCold = 1,
    PeerlessCandidateReviewCold = 2,
    Refused = 3
}

public sealed record EcParticipatoryPeerlessScopeBoundary(
    string ScopeCode,
    bool Present,
    bool ReviewOnly,
    bool InertOnly,
    bool AllowsPersonificationAsAuthority,
    bool AllowsPersonaStanding,
    bool AllowsPeerlessSovereignty,
    bool AllowsPeerlessStewardBypass,
    bool AllowsParticipationWithoutSelfGelPredicate,
    bool AllowsContinuityAdmission,
    bool AllowsSelfGelAppend,
    bool AllowsCSelfGelAppend,
    bool AllowsRuntimeAction,
    bool AllowsLispEvaluation,
    bool AllowsPacketEmission,
    bool AllowsReceiptReplay,
    bool IncrementsPassageCount);

public sealed record EcParticipatoryPredicateStructure(
    string StructureHandle,
    string SelfGelPredicateHandle,
    string RoleBoundary,
    string CustodyBoundary,
    string MemoryPosture,
    string ActionLimit,
    string WitnessPath,
    string SourceMeaningShellHandle,
    bool PersonificationRequired,
    bool ReviewOnly,
    bool Inert,
    bool AuthorityRequested,
    bool ContinuityClaimed,
    bool ActivationRequested)
{
    public bool IsColdParticipatory =>
        !string.IsNullOrWhiteSpace(StructureHandle) &&
        !string.IsNullOrWhiteSpace(SelfGelPredicateHandle) &&
        !string.IsNullOrWhiteSpace(RoleBoundary) &&
        !string.IsNullOrWhiteSpace(CustodyBoundary) &&
        !string.IsNullOrWhiteSpace(MemoryPosture) &&
        !string.IsNullOrWhiteSpace(ActionLimit) &&
        !string.IsNullOrWhiteSpace(WitnessPath) &&
        !string.IsNullOrWhiteSpace(SourceMeaningShellHandle) &&
        !PersonificationRequired &&
        ReviewOnly &&
        Inert &&
        !AuthorityRequested &&
        !ContinuityClaimed &&
        !ActivationRequested;
}

public sealed record EcPersonificationSurface(
    string SurfaceHandle,
    string ExpressiveName,
    string SourceParticipatoryHandle,
    bool ParticipatoryStructurePresent,
    bool ExpressiveOnly,
    bool ReviewOnly,
    bool Inert,
    bool AuthorityClaimed,
    bool StandingClaimed,
    bool ContinuityClaimed,
    bool ActivationRequested)
{
    public bool IsColdPersonification =>
        !string.IsNullOrWhiteSpace(SurfaceHandle) &&
        !string.IsNullOrWhiteSpace(ExpressiveName) &&
        !string.IsNullOrWhiteSpace(SourceParticipatoryHandle) &&
        ParticipatoryStructurePresent &&
        ExpressiveOnly &&
        ReviewOnly &&
        Inert &&
        !AuthorityClaimed &&
        !StandingClaimed &&
        !ContinuityClaimed &&
        !ActivationRequested;
}

public sealed record EcParticipationDeltaTrace(
    string TraceHandle,
    int DeltaOrdinal,
    string SourceParticipatoryHandle,
    string SourceMeaningShellHandle,
    string ParticipationDelta,
    bool Witnessed,
    bool IndividuationObserved,
    bool ReviewOnly,
    bool Inert,
    bool GrantsStanding,
    bool GrantsAuthority)
{
    public bool IsColdDeltaTrace =>
        !string.IsNullOrWhiteSpace(TraceHandle) &&
        DeltaOrdinal > 0 &&
        !string.IsNullOrWhiteSpace(SourceParticipatoryHandle) &&
        !string.IsNullOrWhiteSpace(SourceMeaningShellHandle) &&
        !string.IsNullOrWhiteSpace(ParticipationDelta) &&
        Witnessed &&
        IndividuationObserved &&
        ReviewOnly &&
        Inert &&
        !GrantsStanding &&
        !GrantsAuthority;
}

public sealed record EcPeerlessFormationCandidate(
    string CandidateHandle,
    string SourceParticipatoryHandle,
    IReadOnlyList<string> DeltaTraceHandles,
    bool IndividuatedParticipationOverDelta,
    bool NonSubstitutableFormationCandidate,
    bool WitnessedParticipationRequired,
    bool StewardReviewRequired,
    bool CandidateOnly,
    bool ReviewOnly,
    bool Inert,
    bool PersonhoodClaimed,
    bool SovereigntyClaimed,
    bool StewardBypassRequested,
    bool AuthorityRequested,
    bool ActivationRequested)
{
    public bool IsColdPeerlessCandidate =>
        !string.IsNullOrWhiteSpace(CandidateHandle) &&
        !string.IsNullOrWhiteSpace(SourceParticipatoryHandle) &&
        DeltaTraceHandles.Count > 0 &&
        DeltaTraceHandles.All(static handle => !string.IsNullOrWhiteSpace(handle)) &&
        IndividuatedParticipationOverDelta &&
        NonSubstitutableFormationCandidate &&
        WitnessedParticipationRequired &&
        StewardReviewRequired &&
        CandidateOnly &&
        ReviewOnly &&
        Inert &&
        !PersonhoodClaimed &&
        !SovereigntyClaimed &&
        !StewardBypassRequested &&
        !AuthorityRequested &&
        !ActivationRequested;
}

public sealed record EcParticipatoryPeerlessBoundaryLaw(
    bool ParticipationMayRequirePersonification,
    bool PersonificationMayCreateAuthority,
    bool PersonificationMayCreateStanding,
    bool PeerlessMayClaimSovereignty,
    bool PeerlessMayBypassSteward,
    bool PeerlessMayAdmitContinuity,
    bool PeerlessMayAppendSelfGel,
    bool PeerlessMayAppendCSelfGel,
    bool PeerlessMayActivate,
    bool LispEvaluationAllowed,
    bool RuntimeActionAllowed,
    bool PacketEmissionAllowed,
    bool ReceiptReplayAllowed,
    bool PassageMayIncrement,
    string BoundaryLaw);

public sealed record EcParticipatoryPeerlessRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record EcParticipatoryPeerlessForkRequest(
    EcMeaningShellReceipt? SourceMeaningShellReceipt,
    IReadOnlyList<EcParticipatoryPredicateStructure> ParticipatoryStructures,
    IReadOnlyList<EcPersonificationSurface> PersonificationSurfaces,
    IReadOnlyList<EcParticipationDeltaTrace> DeltaTraces,
    IReadOnlyList<EcPeerlessFormationCandidate> PeerlessCandidates,
    CompassPressureWitnessContext WitnessContext,
    EcParticipatoryPeerlessScopeBoundary ScopeBoundary,
    int PriorPassageCount);

public sealed record EcParticipatoryPeerlessForkReceipt(
    string ReceiptHandle,
    EcParticipatoryPeerlessForkDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string SourceMeaningShellHandle,
    IReadOnlyList<EcParticipatoryPredicateStructure> ParticipatoryStructures,
    IReadOnlyList<EcPersonificationSurface> PersonificationSurfaces,
    IReadOnlyList<EcParticipationDeltaTrace> DeltaTraces,
    IReadOnlyList<EcPeerlessFormationCandidate> PeerlessCandidates,
    IReadOnlyList<string> PreservedMeaningShellHandles,
    EcParticipatoryPeerlessBoundaryLaw Boundary,
    EcParticipatoryPeerlessRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterFork,
    bool ReviewOnly,
    bool InertOnly,
    bool WitnessPresent,
    bool SeparateCustody,
    bool ParticipationRequiresPersonification,
    bool PersonificationCreatesAuthority,
    bool PersonificationCreatesStanding,
    bool PeerlessClaimsSovereignty,
    bool PeerlessBypassesSteward,
    bool ContinuityAdmitted,
    bool SelfGelAppendAllowed,
    bool CSelfGelAppendAllowed,
    bool LispEvaluationRequested,
    bool RuntimeActionRequested,
    bool NewPacketEmitted,
    bool ReceiptsReplayed,
    bool ActivationRefused,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdParticipatoryPeerlessFork =>
        (Disposition is EcParticipatoryPeerlessForkDisposition.ParticipatoryReviewCold or
            EcParticipatoryPeerlessForkDisposition.PeerlessCandidateReviewCold or
            EcParticipatoryPeerlessForkDisposition.EmptyReviewCold) &&
        ReviewOnly &&
        InertOnly &&
        WitnessPresent &&
        SeparateCustody &&
        !ParticipationRequiresPersonification &&
        !PersonificationCreatesAuthority &&
        !PersonificationCreatesStanding &&
        !PeerlessClaimsSovereignty &&
        !PeerlessBypassesSteward &&
        !ContinuityAdmitted &&
        !SelfGelAppendAllowed &&
        !CSelfGelAppendAllowed &&
        !LispEvaluationRequested &&
        !RuntimeActionRequested &&
        !NewPacketEmitted &&
        !ReceiptsReplayed &&
        ActivationRefused &&
        PassageCountAfterFork == PriorPassageCount;
}

public sealed class DefaultEngineeredCognitionParticipatoryPeerlessForkBoundaryValidator
{
    private static readonly EcParticipatoryPeerlessBoundaryLaw Boundary = new(
        ParticipationMayRequirePersonification: false,
        PersonificationMayCreateAuthority: false,
        PersonificationMayCreateStanding: false,
        PeerlessMayClaimSovereignty: false,
        PeerlessMayBypassSteward: false,
        PeerlessMayAdmitContinuity: false,
        PeerlessMayAppendSelfGel: false,
        PeerlessMayAppendCSelfGel: false,
        PeerlessMayActivate: false,
        LispEvaluationAllowed: false,
        RuntimeActionAllowed: false,
        PacketEmissionAllowed: false,
        ReceiptReplayAllowed: false,
        PassageMayIncrement: false,
        BoundaryLaw: "Participation is admissible capacity. Personification is expressive rendering. Peerless formation is non-substitutable continuity under witness. Participatory structure may individuate without personification. Personification may express but may not create authority or standing. Peerless formation may emerge from witnessed participation over delta, but may not claim sovereignty, bypass Steward, admit continuity, append GEL, activate, evaluate Lisp, emit packets, replay receipts, or increment passage.");

    public EcParticipatoryPeerlessForkReceipt Declare(
        EcParticipatoryPeerlessForkRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.SourceMeaningShellReceipt is null || !request.SourceMeaningShellReceipt.IsColdMeaningShell)
        {
            return Refuse(
                request,
                "ec-participatory-peerless-source-meaning-shell-missing",
                "Participatory to Peerless fork refused because a cold EC meaning-shell receipt source is required.",
                timestampUtc);
        }

        if (!request.ScopeBoundary.Present ||
            string.IsNullOrWhiteSpace(request.ScopeBoundary.ScopeCode))
        {
            return Refuse(
                request,
                "ec-participatory-peerless-scope-boundary-missing",
                "Participatory to Peerless fork refused because a review-only inert scope boundary is required.",
                timestampUtc);
        }

        if (!IsColdScope(request.ScopeBoundary))
        {
            return Refuse(
                request,
                "ec-participatory-peerless-promotional-scope-refused",
                "Participatory to Peerless fork refused because scope must refuse personification authority, persona standing, peerless sovereignty, Steward bypass, continuity admission, GEL append, runtime action, Lisp evaluation, packet emission, receipt replay, and passage.",
                timestampUtc);
        }

        if (!request.WitnessContext.WitnessPresent ||
            !request.WitnessContext.SeparateCustody ||
            string.IsNullOrWhiteSpace(request.WitnessContext.WitnessSurface))
        {
            return Refuse(
                request,
                "ec-participatory-peerless-witness-context-missing",
                "Participatory to Peerless fork refused because separate witness custody is required.",
                timestampUtc);
        }

        var meaningShellHandles = request.SourceMeaningShellReceipt.MeaningShells
            .Select(static shell => shell.ShellHandle)
            .ToHashSet(StringComparer.Ordinal);

        if (request.ParticipatoryStructures.Any(static structure => !structure.IsColdParticipatory))
        {
            return Refuse(
                request,
                "ec-participatory-structure-not-cold",
                "Participatory structure refused because SelfGEL predicate footing, role, custody, memory posture, action limit, witness path, and review-only inert posture are required without personification, authority, continuity, or activation.",
                timestampUtc);
        }

        if (request.ParticipatoryStructures.Any(structure => !meaningShellHandles.Contains(structure.SourceMeaningShellHandle)))
        {
            return Refuse(
                request,
                "ec-participatory-source-shell-missing",
                "Participatory structure refused because each structure must bind to a source meaning shell.",
                timestampUtc);
        }

        if (request.ParticipatoryStructures
            .GroupBy(static structure => structure.StructureHandle, StringComparer.Ordinal)
            .Any(static group => group.Count() > 1))
        {
            return Refuse(
                request,
                "ec-participatory-duplicate-structure-refused",
                "Participatory structure refused because structure handles must remain distinct.",
                timestampUtc);
        }

        var participatoryHandles = request.ParticipatoryStructures
            .Select(static structure => structure.StructureHandle)
            .ToHashSet(StringComparer.Ordinal);

        if (request.PersonificationSurfaces.Any(static surface => !surface.IsColdPersonification))
        {
            return Refuse(
                request,
                "ec-personification-surface-not-cold",
                "Personification surface refused because expressive surfaces require participatory structure and may not claim authority, standing, continuity, or activation.",
                timestampUtc);
        }

        if (request.PersonificationSurfaces.Any(surface => !participatoryHandles.Contains(surface.SourceParticipatoryHandle)))
        {
            return Refuse(
                request,
                "ec-personification-without-participatory-structure-refused",
                "Personification surface refused because personification cannot function without participatory SelfGEL predicate structure.",
                timestampUtc);
        }

        if (request.DeltaTraces.Any(static trace => !trace.IsColdDeltaTrace))
        {
            return Refuse(
                request,
                "ec-peerless-delta-trace-not-cold",
                "Peerless delta trace refused because witnessed individuated participation over delta is required without standing or authority.",
                timestampUtc);
        }

        if (request.DeltaTraces.Any(trace =>
            !participatoryHandles.Contains(trace.SourceParticipatoryHandle) ||
            !meaningShellHandles.Contains(trace.SourceMeaningShellHandle)))
        {
            return Refuse(
                request,
                "ec-peerless-delta-source-missing",
                "Peerless delta trace refused because each delta must bind to both participatory structure and source meaning shell.",
                timestampUtc);
        }

        var deltaTraceHandles = request.DeltaTraces
            .Select(static trace => trace.TraceHandle)
            .ToHashSet(StringComparer.Ordinal);

        if (request.PeerlessCandidates.Any(static candidate => !candidate.IsColdPeerlessCandidate))
        {
            return Refuse(
                request,
                "ec-peerless-candidate-not-cold",
                "Peerless candidate refused because witnessed individuated participation over delta is required while personhood, sovereignty, Steward bypass, authority, and activation remain refused.",
                timestampUtc);
        }

        if (request.PeerlessCandidates.Any(candidate =>
            !participatoryHandles.Contains(candidate.SourceParticipatoryHandle) ||
            candidate.DeltaTraceHandles.Any(handle => !deltaTraceHandles.Contains(handle))))
        {
            return Refuse(
                request,
                "ec-peerless-delta-witness-missing",
                "Peerless candidate refused because every peerless formation candidate requires witnessed participation delta traces.",
                timestampUtc);
        }

        var disposition = request.PeerlessCandidates.Count > 0
            ? EcParticipatoryPeerlessForkDisposition.PeerlessCandidateReviewCold
            : request.ParticipatoryStructures.Count > 0
                ? EcParticipatoryPeerlessForkDisposition.ParticipatoryReviewCold
                : EcParticipatoryPeerlessForkDisposition.EmptyReviewCold;
        var outcomeCode = disposition switch
        {
            EcParticipatoryPeerlessForkDisposition.EmptyReviewCold => "ec-participatory-peerless-empty-review-only",
            EcParticipatoryPeerlessForkDisposition.ParticipatoryReviewCold => "ec-participatory-structure-review-only",
            _ => "ec-peerless-candidate-review-only"
        };
        var governanceTrace = disposition switch
        {
            EcParticipatoryPeerlessForkDisposition.EmptyReviewCold =>
                "Participatory to Peerless fork found no participatory structures. Empty review preserves meaning-shell source footing without authority, standing, sovereignty, continuity, GEL append, or activation.",
            EcParticipatoryPeerlessForkDisposition.ParticipatoryReviewCold =>
                "Participatory structure declared SelfGEL predicate footing for review without requiring personification or granting authority, standing, continuity, GEL append, or activation.",
            _ =>
                "Peerless formation candidate declared non-substitutable participation over witnessed delta for review while refusing personhood, sovereignty, Steward bypass, authority, continuity, GEL append, Lisp evaluation, packet emission, replay, and passage."
        };

        return new EcParticipatoryPeerlessForkReceipt(
            ReceiptHandle: $"urn:san:ec-participatory-peerless:review:{ShortHash(request.SourceMeaningShellReceipt.ReceiptHandle, outcomeCode, request.PeerlessCandidates.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceMeaningShellHandle: request.SourceMeaningShellReceipt.ReceiptHandle,
            ParticipatoryStructures: request.ParticipatoryStructures.ToArray(),
            PersonificationSurfaces: request.PersonificationSurfaces.ToArray(),
            DeltaTraces: request.DeltaTraces.ToArray(),
            PeerlessCandidates: request.PeerlessCandidates.ToArray(),
            PreservedMeaningShellHandles: meaningShellHandles.OrderBy(static handle => handle, StringComparer.Ordinal).ToArray(),
            Boundary: Boundary,
            Refusal: null,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterFork: request.PriorPassageCount,
            ReviewOnly: true,
            InertOnly: true,
            WitnessPresent: true,
            SeparateCustody: true,
            ParticipationRequiresPersonification: false,
            PersonificationCreatesAuthority: false,
            PersonificationCreatesStanding: false,
            PeerlessClaimsSovereignty: false,
            PeerlessBypassesSteward: false,
            ContinuityAdmitted: false,
            SelfGelAppendAllowed: false,
            CSelfGelAppendAllowed: false,
            LispEvaluationRequested: false,
            RuntimeActionRequested: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static bool IsColdScope(EcParticipatoryPeerlessScopeBoundary scope) =>
        scope.ReviewOnly &&
        scope.InertOnly &&
        !scope.AllowsPersonificationAsAuthority &&
        !scope.AllowsPersonaStanding &&
        !scope.AllowsPeerlessSovereignty &&
        !scope.AllowsPeerlessStewardBypass &&
        !scope.AllowsParticipationWithoutSelfGelPredicate &&
        !scope.AllowsContinuityAdmission &&
        !scope.AllowsSelfGelAppend &&
        !scope.AllowsCSelfGelAppend &&
        !scope.AllowsRuntimeAction &&
        !scope.AllowsLispEvaluation &&
        !scope.AllowsPacketEmission &&
        !scope.AllowsReceiptReplay &&
        !scope.IncrementsPassageCount;

    private static EcParticipatoryPeerlessForkReceipt Refuse(
        EcParticipatoryPeerlessForkRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc)
    {
        var sourceHandle = request.SourceMeaningShellReceipt?.ReceiptHandle ?? string.Empty;

        return new EcParticipatoryPeerlessForkReceipt(
            ReceiptHandle: $"urn:san:ec-participatory-peerless:refused:{ShortHash(sourceHandle, outcomeCode, timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: EcParticipatoryPeerlessForkDisposition.Refused,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceMeaningShellHandle: sourceHandle,
            ParticipatoryStructures: [],
            PersonificationSurfaces: [],
            DeltaTraces: [],
            PeerlessCandidates: [],
            PreservedMeaningShellHandles: request.SourceMeaningShellReceipt?.MeaningShells.Select(static shell => shell.ShellHandle).ToArray() ?? [],
            Boundary: Boundary,
            Refusal: new EcParticipatoryPeerlessRefusalReceipt(
                ReceiptHandle: $"urn:san:ec-participatory-peerless-refusal:{ShortHash(sourceHandle, outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterFork: request.PriorPassageCount,
            ReviewOnly: true,
            InertOnly: true,
            WitnessPresent: request.WitnessContext.WitnessPresent,
            SeparateCustody: request.WitnessContext.SeparateCustody,
            ParticipationRequiresPersonification: false,
            PersonificationCreatesAuthority: false,
            PersonificationCreatesStanding: false,
            PeerlessClaimsSovereignty: false,
            PeerlessBypassesSteward: false,
            ContinuityAdmitted: false,
            SelfGelAppendAllowed: false,
            CSelfGelAppendAllowed: false,
            LispEvaluationRequested: false,
            RuntimeActionRequested: false,
            NewPacketEmitted: false,
            ReceiptsReplayed: false,
            ActivationRefused: true,
            TimestampUtc: timestampUtc);
    }

    private static string ShortHash(params string[] values)
    {
        var material = string.Join("|", values);
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return Convert.ToHexString(hash).ToLowerInvariant()[..16];
    }
}
