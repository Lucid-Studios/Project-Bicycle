using System.Text;
using System.Text.Json;
using San.Common;

namespace San.Product.Preflight;

public static class EngramPredicatePrecursorStreamReportWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public static string ToJson(EngramPredicatePrecursorStreamReceipt receipt) =>
        JsonSerializer.Serialize(receipt, JsonOptions);

    public static string ToMarkdown(EngramPredicatePrecursorStreamReceipt receipt)
    {
        ArgumentNullException.ThrowIfNull(receipt);

        var builder = new StringBuilder();
        builder.AppendLine("# First Rider Engram Predicate Precursor Stream Receipt");
        builder.AppendLine();
        builder.AppendLine($"Status: `{receipt.Disposition}`");
        builder.AppendLine($"Outcome: `{receipt.OutcomeCode}`");
        builder.AppendLine($"Generated: `{receipt.TimestampUtc:O}`");
        builder.AppendLine();
        builder.AppendLine("## Source");
        builder.AppendLine();
        builder.AppendLine($"- Rider receipt: `{receipt.SourceRiderReceiptHandle}`");
        builder.AppendLine($"- Thought form: {receipt.ThoughtForm}");
        builder.AppendLine();
        builder.AppendLine("## Boundary");
        builder.AppendLine();
        builder.AppendLine($"- Review only: `{receipt.ReviewOnly}`");
        builder.AppendLine($"- Pre-engram only: `{receipt.PreEngramOnly}`");
        builder.AppendLine($"- Residue proof only: `{receipt.ResidueProofOnly}`");
        builder.AppendLine($"- Stream admits engram: `{receipt.StreamAdmitsEngram}`");
        builder.AppendLine($"- Stream admits memory: `{receipt.StreamAdmitsMemory}`");
        builder.AppendLine($"- Stream admits continuity: `{receipt.StreamAdmitsContinuity}`");
        builder.AppendLine($"- Stream authorizes action: `{receipt.StreamAuthorizesAction}`");
        builder.AppendLine($"- Stream grants authority: `{receipt.StreamGrantsAuthority}`");
        builder.AppendLine($"- Lisp evaluation allowed: `{receipt.LispEvaluationAllowed}`");
        builder.AppendLine($"- New packet emitted: `{receipt.NewPacketEmitted}`");
        builder.AppendLine($"- Receipts replayed: `{receipt.ReceiptsReplayed}`");
        builder.AppendLine($"- Activation refused: `{receipt.ActivationRefused}`");
        builder.AppendLine($"- Passage count after stream: `{receipt.PassageCountAfterStream}`");
        builder.AppendLine();
        builder.AppendLine("## Candidacy Gate");
        builder.AppendLine();
        builder.AppendLine($"- Gate: `{receipt.CandidacyGate.GateHandle}`");
        builder.AppendLine($"- Candidate material available: `{receipt.CandidacyGate.CandidateMaterialAvailable}`");
        builder.AppendLine($"- Candidacy review required: `{receipt.CandidacyGate.CandidacyReviewRequired}`");
        builder.AppendLine($"- Gate closed: `{receipt.CandidacyGate.GateClosed}`");
        builder.AppendLine($"- Admits engram: `{receipt.CandidacyGate.AdmitsEngram}`");
        builder.AppendLine($"- Admits memory: `{receipt.CandidacyGate.AdmitsMemory}`");
        builder.AppendLine($"- Admits continuity: `{receipt.CandidacyGate.AdmitsContinuity}`");
        builder.AppendLine();
        builder.AppendLine("## Witness Route");
        builder.AppendLine();
        builder.AppendLine($"- Route: `{receipt.WitnessRoute.RouteHandle}`");
        builder.AppendLine($"- Preserves rider lineage: `{receipt.WitnessRoute.PreservesRiderLineage}`");
        builder.AppendLine($"- Separate custody: `{receipt.WitnessRoute.SeparateCustody}`");
        builder.AppendLine("- Stage IDs:");

        foreach (var stageId in receipt.WitnessRoute.StageIds)
        {
            builder.AppendLine($"  - `{stageId}`");
        }

        builder.AppendLine();
        builder.AppendLine("## Residues");
        builder.AppendLine();

        foreach (var residue in receipt.Residues)
        {
            builder.AppendLine($"### {residue.ResidueClass}");
            builder.AppendLine();
            builder.AppendLine($"- Residue: `{residue.ResidueHandle}`");
            builder.AppendLine($"- Source stage: `{residue.SourceStageId}`");
            builder.AppendLine($"- Boundary cell: `{residue.SourceBoundaryCellId}`");
            builder.AppendLine($"- Predicate code: `{residue.PredicateCode}`");
            builder.AppendLine($"- Evidence: `{residue.EvidenceHandle}`");
            builder.AppendLine($"- Witness: `{residue.WitnessHandle}`");
            builder.AppendLine($"- Review only: `{residue.ReviewOnly}`");
            builder.AppendLine($"- Pre-engram: `{residue.IsPreEngram}`");
            builder.AppendLine($"- Requires candidacy review: `{residue.RequiresCandidacyReview}`");
            builder.AppendLine($"- Cooling required: `{residue.CoolingRequired}`");
            builder.AppendLine($"- Continuity bearing: `{residue.IsContinuityBearing}`");
            builder.AppendLine($"- Admitted engram: `{residue.IsAdmittedEngram}`");
            builder.AppendLine($"- Action authorizing: `{residue.IsActionAuthorizing}`");
            builder.AppendLine($"- Memory admitting: `{residue.IsMemoryAdmitting}`");
            builder.AppendLine($"- Authority granting: `{residue.IsAuthorityGranting}`");
            builder.AppendLine($"- Max pressure: `{residue.PressureVector.MaximumPressure}`");
            builder.AppendLine();
        }

        builder.AppendLine("## Refusal And Cooling Markers");
        builder.AppendLine();

        foreach (var marker in receipt.RefusalCoolingMarkers)
        {
            builder.AppendLine($"- `{marker.MarkerCode}` -> `{marker.CoolingRoute}` / `{marker.RefusalCode}`");
        }

        builder.AppendLine();
        builder.AppendLine("## Governance Trace");
        builder.AppendLine();
        builder.AppendLine(receipt.GovernanceTrace);

        return builder.ToString();
    }
}
