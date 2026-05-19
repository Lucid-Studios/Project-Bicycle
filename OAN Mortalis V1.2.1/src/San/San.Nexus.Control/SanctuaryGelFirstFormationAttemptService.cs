using System.Security.Cryptography;
using System.Text;
using San.Common;

namespace San.Nexus.Control;

public interface ISanctuaryGelFirstFormationAttemptService
{
    SanctuaryGelFirstFormationAttemptAssessment EvaluateFormationAttempt(
        SanctuaryGelFirstFormationAttemptInput input);
}

public sealed class DefaultSanctuaryGelFirstFormationAttemptService : ISanctuaryGelFirstFormationAttemptService
{
    public SanctuaryGelFirstFormationAttemptAssessment EvaluateFormationAttempt(
        SanctuaryGelFirstFormationAttemptInput input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var refusalReasons = new List<SanctuaryGelFirstFormationAttemptRefusalReason>();
        var held = false;

        if (input.PredicatePriors.Count == 0 ||
            !input.PredicatePriors.Any(static prior => prior.Disposition == GelPredicatePriorFormalizationDisposition.Ready))
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.MissingPredicatePriors);
        }

        if (input.PredicatePriors.Any(static prior => prior.Disposition == GelPredicatePriorFormalizationDisposition.Refused))
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.MissingPredicatePriors);
        }

        held |= input.PredicatePriors.Any(static prior => prior.Disposition == GelPredicatePriorFormalizationDisposition.Held);

        if (input.LocalizedPreCertificationDataPool is null ||
            input.LocalizedPreCertificationDataPool.Disposition == LocalizedPreCertificationDataPoolDisposition.Refused)
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.MissingLocalizedPreCertificationDataPool);
        }
        else
        {
            held |= input.LocalizedPreCertificationDataPool.Disposition == LocalizedPreCertificationDataPoolDisposition.Held;
            AddStandingRefusals(input.LocalizedPreCertificationDataPool.RefusalReasons, refusalReasons);
        }

        if (input.LocalizedFormation is null ||
            input.LocalizedFormation.Disposition == LocalizedSanctuaryGelFormationDisposition.Refused)
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.MissingLocalizedFormationFloor);
            AddLocalizedFormationStandingRefusals(input.LocalizedFormation?.RefusalReason, refusalReasons);
        }
        else
        {
            held |= input.LocalizedFormation.Disposition == LocalizedSanctuaryGelFormationDisposition.Held;
            AddLocalizedFormationStandingRefusals(input.LocalizedFormation.RefusalReason, refusalReasons);
        }

        if (input.RegionalSubstrate is null ||
            input.RegionalSubstrate.Disposition == SanctuaryGelRegionalSubstrateFormationDisposition.Refused)
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.MissingRegionalSubstrateFooting);
            AddRegionalSubstrateRefusals(input.RegionalSubstrate?.RefusalReasons, refusalReasons);
        }
        else
        {
            held |= input.RegionalSubstrate.Disposition == SanctuaryGelRegionalSubstrateFormationDisposition.Held;
            AddRegionalSubstrateRefusals(input.RegionalSubstrate.RefusalReasons, refusalReasons);
        }

        if (input.PreGoverningStanding is null ||
            input.PreGoverningStanding.Disposition == SanctuaryPreGoverningStandingDisposition.Refused)
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.MissingPreGoverningStanding);
        }
        else
        {
            held |= input.PreGoverningStanding.Disposition == SanctuaryPreGoverningStandingDisposition.Held;
        }

        if (input.FirstUseEligibility is null ||
            input.FirstUseEligibility.Disposition == FirstUseEligibilityDisposition.Refused)
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.MissingFirstUseEligibilityConsideration);
        }
        else
        {
            held |= input.FirstUseEligibility.Disposition == FirstUseEligibilityDisposition.Held;
        }

        AddOverclaimRefusals(input, refusalReasons);

        var normalizedRefusals = refusalReasons
            .Where(static reason => reason != SanctuaryGelFirstFormationAttemptRefusalReason.None)
            .Distinct()
            .ToArray();
        var disposition = normalizedRefusals.Length > 0
            ? SanctuaryGelFirstFormationAttemptDisposition.Refused
            : held
                ? SanctuaryGelFirstFormationAttemptDisposition.Held
                : SanctuaryGelFirstFormationAttemptDisposition.Ready;
        var summary = SummaryFor(disposition);
        var outcomeCode = disposition switch
        {
            SanctuaryGelFirstFormationAttemptDisposition.Ready => "sanctuary-gel-first-formation-attempt-ready",
            SanctuaryGelFirstFormationAttemptDisposition.Held => "sanctuary-gel-first-formation-attempt-held",
            _ => "sanctuary-gel-first-formation-attempt-refused"
        };
        var witnessRefs = WitnessRefs(input, outcomeCode).ToArray();
        var attemptHandle = CreateHandle(
            "sanctuary-gel-first-formation-attempt://",
            outcomeCode,
            string.Join("|", witnessRefs));
        var record = new SanctuaryGelFirstFormationAttemptRecord(
            AttemptHandle: attemptHandle,
            Disposition: disposition,
            PredicatePriorRefs: input.PredicatePriors.Select(static prior => prior.PredicatePriorRef).ToArray(),
            LocalizedPreCertificationDataPoolRefs: input.LocalizedPreCertificationDataPool?.WitnessRefs ?? Array.Empty<string>(),
            LocalizedFormationRefs: input.LocalizedFormation is null
                ? Array.Empty<string>()
                : new[] { input.LocalizedFormation.SourceGelFormationRef },
            StandingRefs: input.LocalizedFormation?.StandingRepresentations.Select(static standing => standing.RepresentationRef).ToArray() ??
                          Array.Empty<string>(),
            RegionalSubstrateRef: input.RegionalSubstrate?.Identity.SubstrateHandle ?? "missing",
            PreGoverningStandingRef: input.PreGoverningStanding?.SourceApproachRef ?? "missing",
            FirstUseEligibilityRef: input.FirstUseEligibility?.WitnessRefs.FirstOrDefault() ?? "missing",
            RefusalReasons: normalizedRefusals.Length == 0
                ? new[] { SanctuaryGelFirstFormationAttemptRefusalReason.None }
                : normalizedRefusals,
            NonAuthoritySummary: summary,
            WitnessRefs: witnessRefs);
        var receipt = new SanctuaryGelFirstFormationAttemptReceipt(
            ReceiptHandle: CreateHandle("sanctuary-gel-first-formation-attempt-receipt://", attemptHandle, outcomeCode),
            Disposition: disposition,
            Summary: summary,
            WitnessRefs: witnessRefs,
            TimestampUtc: DateTimeOffset.UtcNow);

        return new SanctuaryGelFirstFormationAttemptAssessment(
            Input: input,
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            Summary: summary,
            AttemptRecord: record,
            Receipt: receipt);
    }

    private static void AddStandingRefusals(
        IReadOnlyList<LocalizedPreCertificationDataPoolRefusalReason> upstreamReasons,
        List<SanctuaryGelFirstFormationAttemptRefusalReason> refusalReasons)
    {
        if (upstreamReasons.Contains(LocalizedPreCertificationDataPoolRefusalReason.MissingNationalStanding))
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.MissingNationalStanding);
        }

        if (upstreamReasons.Contains(LocalizedPreCertificationDataPoolRefusalReason.MissingRegionalStanding))
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.MissingRegionalStanding);
        }

        if (upstreamReasons.Contains(LocalizedPreCertificationDataPoolRefusalReason.MissingLocalStanding))
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.MissingLocalStanding);
        }
    }

    private static void AddLocalizedFormationStandingRefusals(
        LocalizedSanctuaryGelFormationRefusalReason? upstreamReason,
        List<SanctuaryGelFirstFormationAttemptRefusalReason> refusalReasons)
    {
        switch (upstreamReason)
        {
            case LocalizedSanctuaryGelFormationRefusalReason.MissingNationalStanding:
                refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.MissingNationalStanding);
                break;
            case LocalizedSanctuaryGelFormationRefusalReason.MissingRegionalStanding:
                refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.MissingRegionalStanding);
                break;
            case LocalizedSanctuaryGelFormationRefusalReason.MissingLocalStanding:
                refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.MissingLocalStanding);
                break;
        }
    }

    private static void AddRegionalSubstrateRefusals(
        IReadOnlyList<SanctuaryGelRegionalSubstrateRefusalReason>? upstreamReasons,
        List<SanctuaryGelFirstFormationAttemptRefusalReason> refusalReasons)
    {
        if (upstreamReasons is null)
        {
            return;
        }

        if (upstreamReasons.Contains(SanctuaryGelRegionalSubstrateRefusalReason.MissingPredicatePriorRefs))
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.MissingPredicatePriors);
        }

        if (upstreamReasons.Contains(SanctuaryGelRegionalSubstrateRefusalReason.MissingLocalizedPreCertificationDataPool))
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.MissingLocalizedPreCertificationDataPool);
        }

        if (upstreamReasons.Contains(SanctuaryGelRegionalSubstrateRefusalReason.MissingNationalStanding))
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.MissingNationalStanding);
        }

        if (upstreamReasons.Contains(SanctuaryGelRegionalSubstrateRefusalReason.MissingRegionalStanding))
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.MissingRegionalStanding);
        }

        if (upstreamReasons.Contains(SanctuaryGelRegionalSubstrateRefusalReason.MissingLocalStanding))
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.MissingLocalStanding);
        }

        if (upstreamReasons.Contains(SanctuaryGelRegionalSubstrateRefusalReason.MissingRegionalPackageFooting) ||
            upstreamReasons.Contains(SanctuaryGelRegionalSubstrateRefusalReason.AdmissionCeilingWidened))
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.MissingRegionalSubstrateFooting);
        }
    }

    private static void AddOverclaimRefusals(
        SanctuaryGelFirstFormationAttemptInput input,
        List<SanctuaryGelFirstFormationAttemptRefusalReason> refusalReasons)
    {
        if (input.SanctuaryActualClaimed)
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.SanctuaryActualOverclaimed);
        }

        if (input.SurvivorAdmissionClaimed)
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.SurvivorAdmissionOverclaimed);
        }

        if (input.FirstUseAdmissionClaimed)
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.FirstUseAdmissionOverclaimed);
        }

        if (input.ModelSelectionClaimed)
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.ModelSelectionOverclaimed);
        }

        if (input.RuntimeAuthorityClaimed)
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.RuntimeAuthorityOverclaimed);
        }

        if (input.CradleGelGenerationClaimed)
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.CradleGelGenerationOverclaimed);
        }

        if (input.SliLispOrRtmeActivationClaimed)
        {
            refusalReasons.Add(SanctuaryGelFirstFormationAttemptRefusalReason.SliLispOrRtmeActivationOverclaimed);
        }
    }

    private static string SummaryFor(SanctuaryGelFirstFormationAttemptDisposition disposition)
    {
        return disposition switch
        {
            SanctuaryGelFirstFormationAttemptDisposition.Ready =>
                "Ready first Sanctuary.GEL formation attempt coheres as a bounded receipted attempt only; it does not stand Sanctuary.Actual, admit survivor standing, grant first use, select models, activate runtime, invoke SLI.Lisp or RTME, or generate Cradle.GEL.",
            SanctuaryGelFirstFormationAttemptDisposition.Held =>
                "Held first Sanctuary.GEL formation attempt preserves represented posture while local, domain, Special Case, counsel, regional, or governance questions remain held.",
            _ =>
                "Refused first Sanctuary.GEL formation attempt because required footing is missing or authority was overclaimed; no Sanctuary.Actual, survivor admission, first use, model selection, runtime, SLI.Lisp, RTME, or Cradle.GEL generation is granted."
        };
    }

    private static IEnumerable<string> WitnessRefs(
        SanctuaryGelFirstFormationAttemptInput input,
        string outcomeCode)
    {
        yield return outcomeCode;

        foreach (var prior in input.PredicatePriors.SelectMany(static prior => prior.WitnessRefs))
        {
            yield return prior;
        }

        foreach (var witness in input.LocalizedPreCertificationDataPool?.WitnessRefs ?? Array.Empty<string>())
        {
            yield return witness;
        }

        foreach (var witness in input.LocalizedFormation?.WitnessRefs ?? Array.Empty<string>())
        {
            yield return witness;
        }

        foreach (var witness in input.RegionalSubstrate?.WitnessRefs ?? Array.Empty<string>())
        {
            yield return witness;
        }

        foreach (var witness in input.PreGoverningStanding?.WitnessRefs ?? Array.Empty<string>())
        {
            yield return witness;
        }

        foreach (var witness in input.FirstUseEligibility?.WitnessRefs ?? Array.Empty<string>())
        {
            yield return witness;
        }
    }

    private static string CreateHandle(string prefix, params string[] parts)
    {
        var material = string.Join("|", parts.Select(static part => part?.Trim() ?? string.Empty));
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return $"{prefix}{Convert.ToHexString(hash).ToLowerInvariant()[..16]}";
    }
}
