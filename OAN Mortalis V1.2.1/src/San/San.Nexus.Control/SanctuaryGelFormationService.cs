using System.Security.Cryptography;
using System.Text;
using San.Common;

namespace San.Nexus.Control;

public interface ISanctuaryGelFormationService
{
    SanctuaryGelFormationAssessment EvaluateFormation(SanctuaryGelFormationInput input);
}

public sealed class DefaultSanctuaryGelFormationService : ISanctuaryGelFormationService
{
    public SanctuaryGelFormationAssessment EvaluateFormation(SanctuaryGelFormationInput input)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(input.PredicatePoolAssessment);
        ArgumentNullException.ThrowIfNull(input.SubstrateIdentity);

        var witnessRefs = new List<string>
        {
            input.PredicatePoolAssessment.Receipt.ReceiptHandle,
            input.DerivedPayloadLineage,
            input.SymbolicTransformWitness,
            input.EngrammatizationWitness,
            input.SubstrateIdentity.FormationReceiptHandle
        };

        if (!string.IsNullOrWhiteSpace(input.PredicatePoolAssessment.PredicatePool?.Identity.PoolHandle))
        {
            witnessRefs.Add(input.PredicatePoolAssessment.PredicatePool.Identity.PoolHandle);
            witnessRefs.AddRange(input.PredicatePoolAssessment.PredicatePool.Candidates.Select(static candidate => candidate.CandidateHandle));
        }

        var silent = input.PredicatePoolAssessment.Disposition == SanctuaryGelPredicatePoolDisposition.Silence;
        var refused =
            input.PredicatePoolAssessment.Disposition == SanctuaryGelPredicatePoolDisposition.Refused ||
            input.PredicatePoolAssessment.PredicatePool is null ||
            string.IsNullOrWhiteSpace(input.DerivedPayloadLineage) ||
            string.IsNullOrWhiteSpace(input.SymbolicAnchorSummary) ||
            string.IsNullOrWhiteSpace(input.SymbolicTransformWitness) ||
            string.IsNullOrWhiteSpace(input.EngrammatizationWitness) ||
            string.IsNullOrWhiteSpace(input.SubstrateIdentity.SubstrateHandle) ||
            string.IsNullOrWhiteSpace(input.SubstrateIdentity.EnvironmentHandle) ||
            string.IsNullOrWhiteSpace(input.SubstrateIdentity.FormationReceiptHandle) ||
            input.RawRootAtlasResidencyClaimed ||
            input.LabSideTemplatingAuthorityClaimed ||
            input.PublicProjectionRequested;

        SanctuaryGelSubstrateRecord? substrateRecord = null;
        SanctuaryGelFormationDisposition disposition;
        string outcomeCode;
        string summary;

        if (silent)
        {
            disposition = SanctuaryGelFormationDisposition.Refused;
            outcomeCode = "sanctuary-gel-formation-silence";
            summary = "First Sanctuary.GEL formation remained silent because the upstream formation data pool did not expose a useful listening response surface.";
        }
        else if (refused)
        {
            disposition = SanctuaryGelFormationDisposition.Refused;
            outcomeCode = "sanctuary-gel-formation-refused";
            summary = "First Sanctuary.GEL formation refused because the bounded formation data pool or tripartite witness was incomplete or widened.";
        }
        else
        {
            disposition = SanctuaryGelFormationDisposition.Retained;
            outcomeCode = "sanctuary-gel-formation-retained";
            summary = "First Sanctuary.GEL formation retained one runtime-admitted local substrate record from bounded tripartite witness.";
            substrateRecord = new SanctuaryGelSubstrateRecord(
                Identity: input.SubstrateIdentity,
                State: SanctuaryGelFormationDisposition.Retained,
                DerivedPayloadLineage: input.DerivedPayloadLineage,
                SymbolicAnchorSummary: input.SymbolicAnchorSummary,
                PredicatePoolHandle: input.PredicatePoolAssessment.PredicatePool!.Identity.PoolHandle,
                PredicateFamilies: input.PredicatePoolAssessment.PredicatePool.FamilySets.Select(static familySet => familySet.Family).ToArray(),
                InheritedPredicateKinds: input.PredicatePoolAssessment.PredicatePool.Candidates.Select(static candidate => candidate.Kind).ToArray(),
                Retained: true,
                RestCapable: true,
                WitnessRefs: witnessRefs);
        }

        var receipt = new SanctuaryGelFormationReceipt(
            ReceiptHandle: CreateHandle("sanctuary-gel-formation-receipt://", input.SubstrateIdentity.FormationReceiptHandle, outcomeCode),
            Disposition: disposition,
            Summary: summary,
            WitnessRefs: witnessRefs,
            TimestampUtc: DateTimeOffset.UtcNow);

        return new SanctuaryGelFormationAssessment(
            Input: input,
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            Summary: summary,
            SubstrateRecord: substrateRecord,
            Receipt: receipt);
    }

    private static string CreateHandle(string prefix, params string[] parts)
    {
        var material = string.Join("|", parts.Select(static part => part?.Trim() ?? string.Empty));
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return $"{prefix}{Convert.ToHexString(hash).ToLowerInvariant()[..16]}";
    }
}
