using System.Text;
using System.Text.Json;

namespace San.Sanctuary.Runtime;

public static class SanctuaryLabGelEngrammitizationReportWriter
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    public static string ToJson(SanctuaryLabGelEngrammitizationReceipt receipt) =>
        JsonSerializer.Serialize(receipt, JsonOptions);

    public static string ToMarkdown(SanctuaryLabGelEngrammitizationReceipt receipt)
    {
        var builder = new StringBuilder();
        builder.AppendLine("# Sanctuary Lab GEL Engrammitization");
        builder.AppendLine();
        builder.AppendLine($"- Disposition: `{receipt.Disposition}`");
        builder.AppendLine($"- Outcome: `{receipt.OutcomeCode}`");
        builder.AppendLine($"- Receipt: `{receipt.ReceiptHandle}`");
        builder.AppendLine($"- Source warm-use receipt: `{receipt.SourceWarmUseReceiptHandle}`");
        builder.AppendLine($"- Source SLI.Lisp warm-use receipt: `{receipt.SourceSliLispWarmUseReceiptHandle}`");
        builder.AppendLine($"- Prior lab GEL receipt: `{(string.IsNullOrWhiteSpace(receipt.PriorLabGelReceiptHandle) ? "none" : receipt.PriorLabGelReceiptHandle)}`");
        builder.AppendLine($"- Engine owner: `{receipt.SliLispLabGelReceipt?.Telemetry.GetValueOrDefault("engine-owner") ?? "none"}`");
        builder.AppendLine($"- Bounded entrypoint: `{receipt.SliLispLabGelReceipt?.Telemetry.GetValueOrDefault("bounded-entrypoint") ?? "none"}`");
        builder.AppendLine($"- Governance trace: {receipt.GovernanceTrace}");
        builder.AppendLine();
        builder.AppendLine("## Scope");
        builder.AppendLine();
        builder.AppendLine($"- Operator ID: `{receipt.OperatorId}`");
        builder.AppendLine($"- Domain: `{receipt.Domain}`");
        builder.AppendLine($"- Role: `{receipt.Role}`");
        builder.AppendLine($"- Job class: `{receipt.JobClass}`");
        builder.AppendLine($"- Session ID: `{receipt.SessionId}`");
        builder.AppendLine($"- Turn index: `{receipt.TurnIndex}`");
        builder.AppendLine();
        builder.AppendLine("## Predicate Formation");
        builder.AppendLine();
        builder.AppendLine($"- SLI.Lisp owned engine motion: `{receipt.SliLispOwnedEngineMotion}`");
        builder.AppendLine($"- Lab GEL predicate formed: `{receipt.LabGelPredicateFormed}`");
        builder.AppendLine($"- Predicate count: `{receipt.Predicates.Count}`");
        builder.AppendLine($"- Engram candidate formed: `{receipt.EngramCandidateFormed}`");
        builder.AppendLine($"- Evidence body formed: `{receipt.EvidenceBodyFormed}`");
        builder.AppendLine($"- Witness body formed: `{receipt.WitnessBodyFormed}`");
        builder.AppendLine($"- Cooling held: `{receipt.CoolingHeld}`");
        builder.AppendLine($"- Pre-admission review required: `{receipt.PreAdmissionReviewRequired}`");
        builder.AppendLine($"- Lab GEL readback available: `{receipt.LabGelReadbackAvailable}`");
        builder.AppendLine($"- Engram closure formed: `{receipt.EngramClosureFormed}`");
        builder.AppendLine($"- Engram closure ready for EC payload: `{receipt.EngramClosureReadyForEcPayload}`");
        builder.AppendLine($"- Candidate retained as lab substrate: `{receipt.CandidateRetainedAsLabSubstrate}`");
        builder.AppendLine();
        builder.AppendLine("## Predicates");
        builder.AppendLine();
        foreach (var predicate in receipt.Predicates)
        {
            builder.AppendLine($"- `{predicate.PredicateClass}`: `{predicate.PredicateCode}` -> `{predicate.PredicateHandle}`");
        }

        builder.AppendLine();
        builder.AppendLine("## Boundary");
        builder.AppendLine();
        builder.AppendLine($"- Lab GEL admitted: `{receipt.LabGelAdmitted}`");
        builder.AppendLine($"- Engram admitted: `{receipt.EngramAdmitted}`");
        builder.AppendLine($"- Memory admitted: `{receipt.MemoryAdmitted}`");
        builder.AppendLine($"- SelfGEL mutated: `{receipt.SelfGelMutated}`");
        builder.AppendLine($"- Continuity admitted: `{receipt.ContinuityAdmitted}`");
        builder.AppendLine($"- Authority granted: `{receipt.AuthorityGranted}`");
        builder.AppendLine($"- Action authorized: `{receipt.ActionAuthorized}`");
        builder.AppendLine($"- Activation refused: `{receipt.ActivationRefused}`");
        builder.AppendLine($"- Model binding allowed: `{receipt.ModelBindingAllowed}`");
        builder.AppendLine($"- Arbitrary Lisp evaluation allowed: `{receipt.ArbitraryLispEvaluationAllowed}`");
        builder.AppendLine($"- Runtime action allowed: `{receipt.RuntimeActionAllowed}`");
        builder.AppendLine($"- GEL promotion allowed: `{receipt.GelPromotionAllowed}`");
        builder.AppendLine($"- CME.Actual allowed: `{receipt.CmeActualAllowed}`");
        builder.AppendLine($"- Sanctuary.Actual allowed: `{receipt.SanctuaryActualAllowed}`");
        return builder.ToString();
    }
}
