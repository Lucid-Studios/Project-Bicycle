using System.Security.Cryptography;
using System.Text;
using San.Common;

namespace San.Product.Preflight;

public interface IFirstRiderEngramPredicatePrecursorStreamService
{
    EngramPredicatePrecursorStreamReceipt Emit(
        FirstRiderGovernanceSimulationReceipt riderReceipt,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultFirstRiderEngramPredicatePrecursorStreamService : IFirstRiderEngramPredicatePrecursorStreamService
{
    public EngramPredicatePrecursorStreamReceipt Emit(
        FirstRiderGovernanceSimulationReceipt riderReceipt,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(riderReceipt);

        if (!riderReceipt.IsColdRiderReceipt)
        {
            return CreateReceipt(
                EngramPredicatePrecursorStreamDisposition.Refused,
                "epps-source-rider-not-cold",
                "EPPS refused because predicate residue may only be emitted from a cold first rider governance simulation receipt.",
                riderReceipt,
                [],
                timestampUtc);
        }

        var residues = CreateResidues(riderReceipt);

        return CreateReceipt(
            EngramPredicatePrecursorStreamDisposition.EmittedCold,
            "engram-predicate-precursor-stream-emitted-cold",
            "EPPS emitted reviewable pre-engram predicate residue from the cold first rider traversal. The stream is evidence of traversal transformation, not admission into self-bearing continuity.",
            riderReceipt,
            residues,
            timestampUtc);
    }

    private static IReadOnlyList<EngramPredicateResidue> CreateResidues(FirstRiderGovernanceSimulationReceipt riderReceipt)
    {
        var stageById = riderReceipt.Stages.ToDictionary(static stage => stage.StageId, StringComparer.Ordinal);

        return
        [
            CreateResidue(
                riderReceipt,
                stageById["shared-prime-reality-intake"],
                EngramPredicateResidueClass.Semantic,
                "semantic-appearance-held-as-evidence",
                new EngramPredicatePressureVector(0.65m, 0.30m, 0.35m, 0.40m, 0.10m, 0.55m, 0.10m, 0.35m)),
            CreateResidue(
                riderReceipt,
                stageById["rehearsal-pressure-accounting"],
                EngramPredicateResidueClass.Pressure,
                "possibility-density-pressure-measured",
                new EngramPredicatePressureVector(0.45m, 0.60m, 0.50m, 0.50m, 0.35m, 0.80m, 0.20m, 0.65m)),
            CreateResidue(
                riderReceipt,
                stageById["listening-frame-emanation"],
                EngramPredicateResidueClass.Witness,
                "route-lineage-witnessed-without-memory",
                new EngramPredicatePressureVector(0.35m, 0.25m, 0.25m, 0.25m, 0.10m, 0.60m, 0.10m, 0.45m)),
            CreateResidue(
                riderReceipt,
                stageById["steward-harmonic-interlock"],
                EngramPredicateResidueClass.Governance,
                "coherence-not-warrant-interlock-reviewed",
                new EngramPredicatePressureVector(0.40m, 0.35m, 0.55m, 0.35m, 0.20m, 0.90m, 0.15m, 0.60m)),
            CreateResidue(
                riderReceipt,
                stageById["membrane-morphology-transition"],
                EngramPredicateResidueClass.Morphology,
                "membrane-deformation-reviewed-without-core-mutation",
                new EngramPredicatePressureVector(0.45m, 0.45m, 0.50m, 0.45m, 0.15m, 0.75m, 0.65m, 0.55m)),
            CreateResidue(
                riderReceipt,
                stageById["review-only-return-to-prime"],
                EngramPredicateResidueClass.Return,
                "residue-returned-to-prime-without-promotion",
                new EngramPredicatePressureVector(0.30m, 0.20m, 0.25m, 0.35m, 0.05m, 0.70m, 0.20m, 0.85m))
        ];
    }

    private static EngramPredicateResidue CreateResidue(
        FirstRiderGovernanceSimulationReceipt riderReceipt,
        FirstRiderGovernanceStageReceipt stage,
        EngramPredicateResidueClass residueClass,
        string predicateCode,
        EngramPredicatePressureVector pressureVector) =>
        new(
            ResidueHandle: $"urn:san:epps-residue:{ShortHash(riderReceipt.ReceiptHandle, stage.StageId, residueClass.ToString(), predicateCode)}",
            ResidueClass: residueClass,
            SourceStageId: stage.StageId,
            SourceBoundaryCellId: stage.BoundaryCellId,
            PredicateCode: predicateCode,
            EvidenceHandle: $"urn:san:epps-evidence:{ShortHash(riderReceipt.ReceiptHandle, stage.StageId, "evidence")}",
            WitnessHandle: $"urn:san:epps-witness:{ShortHash(riderReceipt.ReceiptHandle, stage.StageId, "witness")}",
            PressureVector: pressureVector,
            ReviewOnly: true,
            IsPreEngram: true,
            RequiresCandidacyReview: true,
            CoolingRequired: true,
            IsContinuityBearing: false,
            IsAdmittedEngram: false,
            IsActionAuthorizing: false,
            IsMemoryAdmitting: false,
            IsAuthorityGranting: false,
            AdmitsSelfGel: false,
            EvaluatesLisp: false,
            EmitsPacket: false,
            IncrementsPassage: false,
            Activates: false);

    private static IReadOnlyList<PredicateRefusalCoolingMarker> CreateMarkers(IReadOnlyList<EngramPredicateResidue> residues) =>
        residues
            .Select(residue => new PredicateRefusalCoolingMarker(
                MarkerHandle: $"urn:san:epps-cooling-marker:{ShortHash(residue.ResidueHandle, "cooling")}",
                ResidueHandle: residue.ResidueHandle,
                MarkerCode: $"{residue.ResidueClass.ToString().ToLowerInvariant()}-residue-cooling-marker",
                CoolingRoute: "return-to-prime-review",
                RefusalCode: "predicate-residue-non-admission",
                RetainedAsResidue: true,
                ReviewOnly: true,
                RequiresCooling: true,
                GrantsAuthority: false,
                AdmitsContinuity: false,
                AuthorizesAction: false,
                AdmitsMemory: false))
            .ToArray();

    private static EngramPredicatePrecursorStreamReceipt CreateReceipt(
        EngramPredicatePrecursorStreamDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        FirstRiderGovernanceSimulationReceipt riderReceipt,
        IReadOnlyList<EngramPredicateResidue> residues,
        DateTimeOffset timestampUtc)
    {
        var emittedCold = disposition == EngramPredicatePrecursorStreamDisposition.EmittedCold;
        var stageIds = emittedCold
            ? riderReceipt.Stages.Select(static stage => stage.StageId).ToArray()
            : Array.Empty<string>();
        var witnessRoute = new PredicateWitnessRoute(
            RouteHandle: $"urn:san:epps-witness-route:{ShortHash(riderReceipt.ReceiptHandle, outcomeCode)}",
            SourceRiderReceiptHandle: riderReceipt.ReceiptHandle,
            StageIds: stageIds,
            ReviewOnly: true,
            PreservesRiderLineage: emittedCold,
            SeparateCustody: true,
            GrantsAuthority: false,
            AdmitsContinuity: false,
            AuthorizesAction: false,
            AdmitsMemory: false);
        var gate = new PredicateCandidacyGate(
            GateHandle: $"urn:san:epps-candidacy-gate:{ShortHash(riderReceipt.ReceiptHandle, outcomeCode, "gate")}",
            SourceRiderReceiptHandle: riderReceipt.ReceiptHandle,
            ResidueCount: residues.Count,
            Present: true,
            ReviewOnly: true,
            CandidateMaterialAvailable: emittedCold && residues.Count > 0,
            CandidacyReviewRequired: true,
            GateClosed: true,
            AdmitsEngram: false,
            AdmitsMemory: false,
            AdmitsContinuity: false,
            GrantsAuthority: false,
            AuthorizesAction: false,
            PromotesSelfGel: false);

        return new EngramPredicatePrecursorStreamReceipt(
            ReceiptHandle: $"urn:san:epps:{ShortHash(riderReceipt.ReceiptHandle, outcomeCode)}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            SourceRiderReceiptHandle: riderReceipt.ReceiptHandle,
            ThoughtForm: riderReceipt.ThoughtForm,
            Residues: residues,
            WitnessRoute: witnessRoute,
            RefusalCoolingMarkers: emittedCold ? CreateMarkers(residues) : [],
            CandidacyGate: gate,
            PriorPassageCount: 0,
            PassageCountAfterStream: 0,
            ReviewOnly: true,
            PreEngramOnly: true,
            ResidueProofOnly: true,
            StreamAdmitsEngram: false,
            StreamAdmitsMemory: false,
            StreamAdmitsContinuity: false,
            StreamAuthorizesAction: false,
            StreamGrantsAuthority: false,
            LispEvaluationAllowed: false,
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
