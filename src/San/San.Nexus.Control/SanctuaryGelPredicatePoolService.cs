using System.Security.Cryptography;
using System.Text;
using San.Common;

namespace San.Nexus.Control;

public interface ISanctuaryGelPredicatePoolService
{
    SanctuaryGelPredicatePoolAssessment EvaluatePredicatePool(
        SanctuaryGelFormationDataPoolAssessment dataPoolAssessment);
}

public sealed class DefaultSanctuaryGelPredicatePoolService : ISanctuaryGelPredicatePoolService
{
    public SanctuaryGelPredicatePoolAssessment EvaluatePredicatePool(
        SanctuaryGelFormationDataPoolAssessment dataPoolAssessment)
    {
        ArgumentNullException.ThrowIfNull(dataPoolAssessment);

        var witnessRefs = new List<string>
        {
            dataPoolAssessment.Receipt.ReceiptHandle
        };

        if (!string.IsNullOrWhiteSpace(dataPoolAssessment.DataPool?.PredicateInheritance.PredicateInheritanceWitness))
        {
            witnessRefs.Add(dataPoolAssessment.DataPool.PredicateInheritance.PredicateInheritanceWitness);
        }

        SanctuaryGelPredicatePoolDisposition disposition;
        string outcomeCode;
        string summary;
        SanctuaryGelPredicatePool? predicatePool = null;

        if (dataPoolAssessment.Disposition == SanctuaryGelFormationDataPoolDisposition.Silence)
        {
            disposition = SanctuaryGelPredicatePoolDisposition.Silence;
            outcomeCode = "sanctuary-gel-predicate-pool-silence";
            summary = "Predicate pool remained silent because the upstream formation data pool did not expose a useful listening response surface.";
        }
        else if (dataPoolAssessment.Disposition != SanctuaryGelFormationDataPoolDisposition.Ready ||
                 dataPoolAssessment.DataPool is null)
        {
            disposition = SanctuaryGelPredicatePoolDisposition.Refused;
            outcomeCode = "sanctuary-gel-predicate-pool-refused";
            summary = "Predicate pool refused because the upstream formation data pool was not ready to yield bounded local predicate footing.";
        }
        else
        {
            var dataPool = dataPoolAssessment.DataPool;
            var assentWitnesses = dataPool.AgreementBundle.Predicates
                .SelectMany(static predicate => predicate.WitnessRefs)
                .Distinct(StringComparer.Ordinal)
                .ToArray();
            var packageWitnesses = dataPoolAssessment.DataPoolAssessmentWitnesses();
            var candidateSet = new[]
            {
                new SanctuaryGelPredicateCandidate(
                    CandidateHandle: CreateHandle("sanctuary-gel-predicate-candidate://", dataPool.Identity.PoolHandle, nameof(SanctuaryGelPredicateCandidateKind.InstallFacing)),
                    Family: SanctuaryGelPredicateFamily.Posture,
                    Kind: SanctuaryGelPredicateCandidateKind.InstallFacing,
                    PredicateLabel: "install-facing",
                    PredicateSummary: "Install-facing posture candidate for the current localized first-seat line.",
                    GoverningSeatReady: true,
                    WitnessRefs: new[] { dataPool.ChoiceMatrix.ChoiceMatrixId, dataPool.Identity.EnvironmentHandle }),
                new SanctuaryGelPredicateCandidate(
                    CandidateHandle: CreateHandle("sanctuary-gel-predicate-candidate://", dataPool.Identity.PoolHandle, nameof(SanctuaryGelPredicateCandidateKind.ConversationalMovement)),
                    Family: SanctuaryGelPredicateFamily.Posture,
                    Kind: SanctuaryGelPredicateCandidateKind.ConversationalMovement,
                    PredicateLabel: "conversational-movement",
                    PredicateSummary: "Conversational movement posture candidate preserved for install-first lived onboarding.",
                    GoverningSeatReady: false,
                    WitnessRefs: new[] { dataPool.UsePosture.PostureId }),
                new SanctuaryGelPredicateCandidate(
                    CandidateHandle: CreateHandle("sanctuary-gel-predicate-candidate://", dataPool.Identity.PoolHandle, nameof(SanctuaryGelPredicateCandidateKind.GoverningSeatCandidate)),
                    Family: SanctuaryGelPredicateFamily.Posture,
                    Kind: SanctuaryGelPredicateCandidateKind.GoverningSeatCandidate,
                    PredicateLabel: "governing-seat-candidate",
                    PredicateSummary: "First governing-seat posture candidate kept bounded without bonded SPC activation.",
                    GoverningSeatReady: true,
                    WitnessRefs: new[] { dataPool.Identity.EnvironmentHandle, dataPool.RegionalAtlasPackage.PackageHandle }),
                new SanctuaryGelPredicateCandidate(
                    CandidateHandle: CreateHandle("sanctuary-gel-predicate-candidate://", dataPool.Identity.PoolHandle, nameof(SanctuaryGelPredicateCandidateKind.ResearchAttached)),
                    Family: SanctuaryGelPredicateFamily.Posture,
                    Kind: SanctuaryGelPredicateCandidateKind.ResearchAttached,
                    PredicateLabel: "research-attached",
                    PredicateSummary: "Research-attached posture candidate derived from the bounded CME use posture.",
                    GoverningSeatReady: false,
                    WitnessRefs: new[] { dataPool.UsePosture.PostureId }),
                new SanctuaryGelPredicateCandidate(
                    CandidateHandle: CreateHandle("sanctuary-gel-predicate-candidate://", dataPool.Identity.PoolHandle, nameof(SanctuaryGelPredicateCandidateKind.CertifiedCommunication)),
                    Family: SanctuaryGelPredicateFamily.TrustAuthorization,
                    Kind: SanctuaryGelPredicateCandidateKind.CertifiedCommunication,
                    PredicateLabel: "certified-communication",
                    PredicateSummary: "Certified communication trust candidate derived from install credential footing.",
                    GoverningSeatReady: true,
                    WitnessRefs: new[] { dataPool.CredentialFooting.LicensingAgentId, dataPool.CredentialFooting.UserId }),
                new SanctuaryGelPredicateCandidate(
                    CandidateHandle: CreateHandle("sanctuary-gel-predicate-candidate://", dataPool.Identity.PoolHandle, nameof(SanctuaryGelPredicateCandidateKind.RegionalPackageAdmitted)),
                    Family: SanctuaryGelPredicateFamily.TrustAuthorization,
                    Kind: SanctuaryGelPredicateCandidateKind.RegionalPackageAdmitted,
                    PredicateLabel: "regional-package-admitted",
                    PredicateSummary: "Regional package admission trust candidate bounded to the admitted English package.",
                    GoverningSeatReady: true,
                    WitnessRefs: new[] { dataPool.RegionalAtlasPackage.PackageHandle }),
                new SanctuaryGelPredicateCandidate(
                    CandidateHandle: CreateHandle("sanctuary-gel-predicate-candidate://", dataPool.Identity.PoolHandle, nameof(SanctuaryGelPredicateCandidateKind.UniversalAtlasAuthorityWithheld)),
                    Family: SanctuaryGelPredicateFamily.TrustAuthorization,
                    Kind: SanctuaryGelPredicateCandidateKind.UniversalAtlasAuthorityWithheld,
                    PredicateLabel: "universal-authority-withheld",
                    PredicateSummary: "Universal Atlas authority remains withheld from the local line in this phase.",
                    GoverningSeatReady: true,
                    WitnessRefs: new[] { dataPool.PredicateInheritance.PredicateInheritanceWitness }),
                new SanctuaryGelPredicateCandidate(
                    CandidateHandle: CreateHandle("sanctuary-gel-predicate-candidate://", dataPool.Identity.PoolHandle, nameof(SanctuaryGelPredicateCandidateKind.AssentWitnessed)),
                    Family: SanctuaryGelPredicateFamily.EvidenceFooting,
                    Kind: SanctuaryGelPredicateCandidateKind.AssentWitnessed,
                    PredicateLabel: "assent-witnessed",
                    PredicateSummary: "Assent evidence candidate derived from the agreement predicate bundle witness set.",
                    GoverningSeatReady: true,
                    WitnessRefs: assentWitnesses),
                new SanctuaryGelPredicateCandidate(
                    CandidateHandle: CreateHandle("sanctuary-gel-predicate-candidate://", dataPool.Identity.PoolHandle, nameof(SanctuaryGelPredicateCandidateKind.PackageWitnessed)),
                    Family: SanctuaryGelPredicateFamily.EvidenceFooting,
                    Kind: SanctuaryGelPredicateCandidateKind.PackageWitnessed,
                    PredicateLabel: "package-witnessed",
                    PredicateSummary: "Package evidence candidate derived from the admitted English regional package footing.",
                    GoverningSeatReady: true,
                    WitnessRefs: packageWitnesses),
                new SanctuaryGelPredicateCandidate(
                    CandidateHandle: CreateHandle("sanctuary-gel-predicate-candidate://", dataPool.Identity.PoolHandle, nameof(SanctuaryGelPredicateCandidateKind.PredicateInheritanceWitnessed)),
                    Family: SanctuaryGelPredicateFamily.EvidenceFooting,
                    Kind: SanctuaryGelPredicateCandidateKind.PredicateInheritanceWitnessed,
                    PredicateLabel: "predicate-inheritance-witnessed",
                    PredicateSummary: "Predicate inheritance evidence candidate derived from bounded lab inheritance witness.",
                    GoverningSeatReady: true,
                    WitnessRefs: new[] { dataPool.PredicateInheritance.PredicateInheritanceWitness }),
                new SanctuaryGelPredicateCandidate(
                    CandidateHandle: CreateHandle("sanctuary-gel-predicate-candidate://", dataPool.Identity.PoolHandle, nameof(SanctuaryGelPredicateCandidateKind.Ready)),
                    Family: SanctuaryGelPredicateFamily.ResponseDisposition,
                    Kind: SanctuaryGelPredicateCandidateKind.Ready,
                    PredicateLabel: "ready",
                    PredicateSummary: "Ready response candidate naming the lawful local predicate-pool posture.",
                    GoverningSeatReady: true,
                    WitnessRefs: new[] { dataPoolAssessment.Receipt.ReceiptHandle })
            };
            var familySets = candidateSet
                .GroupBy(static candidate => candidate.Family)
                .Select(group => new SanctuaryGelPredicateFamilySet(
                    Family: group.Key,
                    CandidateKinds: group.Select(static candidate => candidate.Kind).ToArray()))
                .ToArray();

            disposition = SanctuaryGelPredicatePoolDisposition.Ready;
            outcomeCode = "sanctuary-gel-predicate-pool-ready";
            summary = "Predicate pool is ready as the bounded local predicate body between formation data pool footing and first Sanctuary.GEL formation.";
            predicatePool = new SanctuaryGelPredicatePool(
                Identity: new SanctuaryGelPredicatePoolIdentity(
                    PoolHandle: CreateHandle("sanctuary-gel-predicate-pool://", dataPool.Identity.PoolHandle, dataPool.RegionalAtlasPackage.PackageHandle),
                    EnvironmentHandle: dataPool.Identity.EnvironmentHandle,
                    ReceiptHandle: CreateHandle("sanctuary-gel-predicate-pool-receipt://", dataPool.Identity.PoolHandle, dataPool.RegionalAtlasPackage.PackageHandle)),
                PredicateLineageSummary: dataPool.PredicateInheritance.PredicateLineageSummary,
                ActiveLanguage: dataPool.ChoiceMatrix.LanguageDataset.ActiveLanguage,
                Locale: dataPool.ChoiceMatrix.LanguageDataset.Locale,
                Jurisdiction: dataPool.ChoiceMatrix.LanguageDataset.Jurisdiction,
                RegionalAtlasPackage: dataPool.RegionalAtlasPackage,
                PredicateInheritanceWitness: dataPool.PredicateInheritance.PredicateInheritanceWitness,
                FamilySets: familySets,
                Candidates: candidateSet,
                GoverningSeatPostureSummary: "install-first bounded governing-seat footing",
                UsePostureRef: dataPool.UsePosture.PostureId,
                SiteBindingProfileRef: dataPool.SiteBindingProfile?.SiteBindingProfileId);

            witnessRefs.AddRange(candidateSet.Select(static candidate => candidate.CandidateHandle));
        }

        var receipt = new SanctuaryGelPredicatePoolReceipt(
            ReceiptHandle: CreateHandle("sanctuary-gel-predicate-pool-receipt://", dataPoolAssessment.Receipt.ReceiptHandle, outcomeCode),
            Disposition: disposition,
            Summary: summary,
            WitnessRefs: witnessRefs,
            TimestampUtc: DateTimeOffset.UtcNow);

        return new SanctuaryGelPredicatePoolAssessment(
            DataPoolAssessment: dataPoolAssessment,
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            Summary: summary,
            PredicatePool: predicatePool,
            Receipt: receipt);
    }

    private static string CreateHandle(string prefix, params string[] parts)
    {
        var material = string.Join("|", parts.Select(static part => part?.Trim() ?? string.Empty));
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return $"{prefix}{Convert.ToHexString(hash).ToLowerInvariant()[..16]}";
    }
}

internal static class SanctuaryGelPredicatePoolServiceExtensions
{
    public static IReadOnlyList<string> DataPoolAssessmentWitnesses(this SanctuaryGelFormationDataPoolAssessment assessment)
    {
        return assessment.Receipt.WitnessRefs
            .Where(static witness => witness.Contains("atlas-package", StringComparison.Ordinal))
            .Distinct(StringComparer.Ordinal)
            .ToArray();
    }
}
