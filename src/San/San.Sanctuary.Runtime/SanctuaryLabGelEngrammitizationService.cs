using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using SLI.Lisp;

namespace San.Sanctuary.Runtime;

public interface ISanctuaryLabGelEngrammitizationService
{
    SanctuaryLabGelEngrammitizationReceipt Run(
        SanctuaryLabGelEngrammitizationRequest request,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultSanctuaryLabGelEngrammitizationService : ISanctuaryLabGelEngrammitizationService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly ISliLispLabGelEngrammitizationService sliLispLabGelService;

    public DefaultSanctuaryLabGelEngrammitizationService()
        : this(new DefaultSliLispLabGelEngrammitizationService())
    {
    }

    public DefaultSanctuaryLabGelEngrammitizationService(ISliLispLabGelEngrammitizationService sliLispLabGelService)
    {
        this.sliLispLabGelService = sliLispLabGelService;
    }

    public SanctuaryLabGelEngrammitizationReceipt Run(
        SanctuaryLabGelEngrammitizationRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        var source = request.SourceWarmUseReceipt;
        var lineRootPath = source?.LineRootPath ?? string.Empty;
        var installRootPath = source?.InstallRootPath ?? string.Empty;
        var sessionId = NormalizeIdentitySegment(source?.SessionId ?? "warm-use-session", "warm-use-session");
        var turnIndex = Math.Max(0, source?.TurnIndex ?? 0);
        var thoughtForm = string.IsNullOrWhiteSpace(source?.ThoughtForm)
            ? "idle lab GEL predicate formation"
            : source.ThoughtForm.Trim();
        var receiptRootPath = string.IsNullOrWhiteSpace(installRootPath)
            ? string.Empty
            : Path.Combine(installRootPath, "receipts", "lab-gel-engrammitization", sessionId);
        var receiptJsonPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, $"turn-{turnIndex:0000}.json");
        var receiptMarkdownPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, $"turn-{turnIndex:0000}.md");
        var sessionLedgerPath = string.IsNullOrWhiteSpace(receiptRootPath)
            ? string.Empty
            : Path.Combine(receiptRootPath, "lab-gel-session.jsonl");

        if (request.RequestsForbiddenMotion)
        {
            var refusedReceipt = CreateReceipt(
                SanctuaryLabGelEngrammitizationDisposition.Refused,
                "sanctuary-lab-gel-runtime-motion-refused",
                "Sanctuary lab GEL engrammitization refused before Lisp invocation because the host request attempted activation, model binding, arbitrary Lisp evaluation, runtime identity, runtime action, database write, GEL promotion, GEL admission, engram admission, memory admission, SelfGEL mutation, continuity admission, CME.Actual, or Sanctuary.Actual.",
                lineRootPath,
                installRootPath,
                receiptJsonPath,
                receiptMarkdownPath,
                sessionLedgerPath,
                request.PriorLabGelReceiptHandle,
                source,
                sliLispLabGelReceipt: null,
                predicates: [],
                timestampUtc);
            WriteReceiptAndLedgerIfPossible(refusedReceipt);
            return refusedReceipt;
        }

        if (source is null || !source.IsTypedColdReadyWarmUse)
        {
            var withheldReceipt = CreateReceipt(
                SanctuaryLabGelEngrammitizationDisposition.Withheld,
                "sanctuary-lab-gel-source-warm-use-missing",
                "Sanctuary lab GEL engrammitization withheld because a typed cold-ready warm-use receipt is required before predicate extraction and engram candidate formation may be tested.",
                lineRootPath,
                installRootPath,
                receiptJsonPath,
                receiptMarkdownPath,
                sessionLedgerPath,
                request.PriorLabGelReceiptHandle,
                source,
                sliLispLabGelReceipt: null,
                predicates: [],
                timestampUtc);
            WriteReceiptAndLedgerIfPossible(withheldReceipt);
            return withheldReceipt;
        }

        var lispReceipt = sliLispLabGelService.Run(
            new SliLispLabGelEngrammitizationRequest(
                OperatorId: source.OperatorId,
                Domain: source.Domain,
                Role: source.Role,
                JobClass: source.JobClass,
                SessionId: source.SessionId,
                TurnIndex: source.TurnIndex,
                SourceWarmUseReceiptHandle: source.ReceiptHandle,
                ThoughtForm: source.ThoughtForm,
                RuntimePath: request.SliLispRuntimePath),
            timestampUtc);

        var completed = lispReceipt.IsLabGelPreAdmissionEngrammitization;
        var predicates = completed ? CreatePredicates(source, lispReceipt) : [];
        var receipt = CreateReceipt(
            completed
                ? SanctuaryLabGelEngrammitizationDisposition.CompletedCold
                : SanctuaryLabGelEngrammitizationDisposition.Withheld,
            completed
                ? "sanctuary-lab-gel-engrammitization-completed-cold"
                : "sanctuary-lab-gel-engrammitization-withheld",
            completed
                ? "Sanctuary lab GEL engrammitization formed lab GEL predicate receipts and a pre-admission engram candidate from typed warm-use residue. The candidate may be read back as lab substrate; GEL admission, SelfGEL mutation, continuity, authority, action, model binding, arbitrary Lisp evaluation, CME.Actual, and Sanctuary.Actual remain refused."
                : "Sanctuary lab GEL engrammitization withheld because the bounded SLI.Lisp lab GEL entrypoint did not complete cold.",
            lineRootPath,
            installRootPath,
            receiptJsonPath,
            receiptMarkdownPath,
            sessionLedgerPath,
            request.PriorLabGelReceiptHandle,
            source,
            lispReceipt,
            predicates,
            timestampUtc);

        WriteReceiptAndLedgerIfPossible(receipt);
        return receipt;
    }

    private static IReadOnlyList<LabGelPredicateReceipt> CreatePredicates(
        SanctuaryTypedWarmUseRehearsalReceipt source,
        SliLispLabGelEngrammitizationReceipt lispReceipt)
    {
        var classes = lispReceipt.LabGelPredicateClasses.Count == 6
            ? lispReceipt.LabGelPredicateClasses
            : ["semantic", "pressure", "witness", "governance", "morphology", "return"];
        var predicates = new List<LabGelPredicateReceipt>(capacity: 6);

        for (var index = 0; index < classes.Count; index += 1)
        {
            var residueClass = NormalizeIdentitySegment(classes[index], $"predicate-{index}");
            var predicateClass = ToPredicateClass(residueClass);
            var predicateCode = $"lab-gel-{residueClass}-predicate-pre-admission";
            var predicateHandle = $"urn:san:lab-gel-predicate:{ShortHash(source.ReceiptHandle, residueClass, predicateCode)}";

            predicates.Add(new LabGelPredicateReceipt(
                PredicateHandle: predicateHandle,
                PredicateClass: predicateClass,
                PredicateCode: predicateCode,
                SourceWarmUseReceiptHandle: source.ReceiptHandle,
                SourceResidueClass: residueClass,
                EvidenceHandle: $"urn:san:lab-gel-evidence:{ShortHash(predicateHandle, "evidence")}",
                WitnessHandle: $"urn:san:lab-gel-witness:{ShortHash(predicateHandle, "witness")}",
                ReviewOnly: true,
                PreAdmissionOnly: true,
                LabSubstrateOnly: true,
                MayEnterEngramCandidacy: true,
                GelAdmitted: false,
                SelfGelMutated: false,
                ContinuityAdmitted: false,
                AuthorityGranted: false,
                ActionAuthorized: false));
        }

        return predicates;
    }

    private static SanctuaryLabGelEngrammitizationReceipt CreateReceipt(
        SanctuaryLabGelEngrammitizationDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        string lineRootPath,
        string installRootPath,
        string receiptJsonPath,
        string receiptMarkdownPath,
        string sessionLedgerPath,
        string? priorLabGelReceiptHandle,
        SanctuaryTypedWarmUseRehearsalReceipt? source,
        SliLispLabGelEngrammitizationReceipt? sliLispLabGelReceipt,
        IReadOnlyList<LabGelPredicateReceipt> predicates,
        DateTimeOffset timestampUtc)
    {
        var sourceWarmUseReceiptHandle = source?.ReceiptHandle ?? string.Empty;
        var sourceSliReceiptHandle = source?.SliLispWarmUseReceipt?.ReceiptHandle ?? string.Empty;
        var operatorId = sliLispLabGelReceipt?.OperatorId ?? source?.OperatorId ?? string.Empty;
        var domain = sliLispLabGelReceipt?.Domain ?? source?.Domain ?? string.Empty;
        var role = sliLispLabGelReceipt?.Role ?? source?.Role ?? string.Empty;
        var jobClass = sliLispLabGelReceipt?.JobClass ?? source?.JobClass ?? string.Empty;
        var sessionId = sliLispLabGelReceipt?.SessionId ?? source?.SessionId ?? string.Empty;
        var turnIndex = sliLispLabGelReceipt?.TurnIndex ?? source?.TurnIndex ?? 0;
        var thoughtForm = source?.ThoughtForm ?? string.Empty;
        var completed = disposition == SanctuaryLabGelEngrammitizationDisposition.CompletedCold &&
            sliLispLabGelReceipt?.IsLabGelPreAdmissionEngrammitization == true &&
            predicates.Count == 6;
        var predicateHandles = predicates.Select(static predicate => predicate.PredicateHandle).ToArray();
        var evidenceBody = completed
            ? new LabGelEvidenceBody(
                EvidenceBodyHandle: $"urn:san:lab-gel-evidence-body:{ShortHash(sourceWarmUseReceiptHandle, outcomeCode, "evidence-body")}",
                SourceWarmUseReceiptHandle: sourceWarmUseReceiptHandle,
                PredicateHandles: predicateHandles,
                EvidenceBoundToWarmUseReceipt: true,
                EvidenceBoundToSliLispTelemetry: true,
                EvidenceCeilingPassive: true,
                ReviewOnly: true,
                GrantsWarrant: false,
                AdmitsContinuity: false,
                AuthorizesAction: false)
            : null;
        var witnessBody = completed
            ? new LabGelWitnessBody(
                WitnessBodyHandle: $"urn:san:lab-gel-witness-body:{ShortHash(sourceWarmUseReceiptHandle, outcomeCode, "witness-body")}",
                SourceWarmUseReceiptHandle: sourceWarmUseReceiptHandle,
                SourceSliLispReceiptHandle: sourceSliReceiptHandle,
                SessionId: sessionId,
                TurnIndex: turnIndex,
                PreservesWarmUseLineage: true,
                PreservesSessionLineage: true,
                SeparateCustody: true,
                ReviewOnly: true,
                AdmitsMemory: false,
                GrantsAuthority: false,
                AuthorizesAction: false)
            : null;
        var candidate = completed && evidenceBody is not null && witnessBody is not null
            ? new EngramCandidateReceipt(
                CandidateHandle: $"urn:san:engram-candidate:{ShortHash(sourceWarmUseReceiptHandle, outcomeCode, "candidate")}",
                SourceWarmUseReceiptHandle: sourceWarmUseReceiptHandle,
                LabGelPredicateFamily: "lab-gel-pre-admission-warm-use-predicate-family",
                PredicateCount: predicates.Count,
                EvidenceBodyHandle: evidenceBody.EvidenceBodyHandle,
                WitnessBodyHandle: witnessBody.WitnessBodyHandle,
                CandidateFormed: true,
                PreAdmissionOnly: true,
                EvidenceBodyPresent: true,
                WitnessBodyPresent: true,
                CoolingRequired: true,
                StewardReviewRequired: true,
                GelAdmitted: false,
                EngramAdmitted: false,
                MemoryAdmitted: false,
                SelfGelMutated: false,
                ContinuityAdmitted: false,
                AuthorityGranted: false,
                ActionAuthorized: false)
            : null;
        var cooling = candidate is not null
            ? new EngramCandidateCoolingReceipt(
                CoolingReceiptHandle: $"urn:san:engram-candidate-cooling:{ShortHash(candidate.CandidateHandle, "cooling")}",
                CandidateHandle: candidate.CandidateHandle,
                CoolingRoute: "return-to-prime-lab-substrate-hold",
                HeldAsLabSubstrate: true,
                ReturnToPrimePreserved: true,
                ReviewOnly: true,
                AdmitsGel: false,
                AdmitsSelfGel: false,
                GrantsAuthority: false,
                AuthorizesAction: false)
            : null;
        var review = candidate is not null
            ? new EngramPreAdmissionReviewReceipt(
                ReviewReceiptHandle: $"urn:san:engram-pre-admission-review:{ShortHash(candidate.CandidateHandle, "review")}",
                CandidateHandle: candidate.CandidateHandle,
                ReviewOutcomeCode: "retain-as-lab-substrate-pre-admission",
                StewardReviewed: true,
                RecommendRetainAsLabSubstrate: true,
                RequiresFutureAdmissionGate: true,
                PerformsAdmission: false,
                MutatesGel: false,
                MutatesSelfGel: false,
                GrantsAuthority: false,
                AuthorizesAction: false)
            : null;
        var readback = candidate is not null
            ? new LabGelReadbackReceipt(
                ReadbackReceiptHandle: $"urn:san:lab-gel-readback:{ShortHash(candidate.CandidateHandle, "readback")}",
                CandidateHandle: candidate.CandidateHandle,
                ReadbackScope: "lab-substrate-pre-admission-only",
                ReadbackAvailable: true,
                PreAdmissionOnly: true,
                LabSubstrateOnly: true,
                MayInformFutureRehearsal: true,
                MayInformActionAuthority: false,
                AdmitsMemory: false,
                AdmitsContinuity: false,
                GrantsAuthority: false,
                AuthorizesAction: false)
            : null;

        return new SanctuaryLabGelEngrammitizationReceipt(
            ReceiptHandle: $"urn:san:lab-gel-engrammitization:{ShortHash(sourceWarmUseReceiptHandle, sessionId, turnIndex.ToString(System.Globalization.CultureInfo.InvariantCulture), outcomeCode)}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            LineRootPath: lineRootPath,
            InstallRootPath: installRootPath,
            ReceiptJsonPath: receiptJsonPath,
            ReceiptMarkdownPath: receiptMarkdownPath,
            SessionLedgerPath: sessionLedgerPath,
            SourceWarmUseReceiptHandle: sourceWarmUseReceiptHandle,
            SourceSliLispWarmUseReceiptHandle: sourceSliReceiptHandle,
            PriorLabGelReceiptHandle: priorLabGelReceiptHandle?.Trim() ?? string.Empty,
            OperatorId: operatorId,
            Domain: domain,
            Role: role,
            JobClass: jobClass,
            SessionId: sessionId,
            TurnIndex: turnIndex,
            ThoughtForm: thoughtForm,
            SliLispLabGelReceipt: sliLispLabGelReceipt,
            Predicates: predicates,
            EvidenceBody: evidenceBody,
            WitnessBody: witnessBody,
            EngramCandidate: candidate,
            CoolingReceipt: cooling,
            PreAdmissionReview: review,
            ReadbackReceipt: readback,
            ReviewOnly: true,
            SliLispOwnedEngineMotion: sliLispLabGelReceipt?.IsLabGelPreAdmissionEngrammitization == true,
            LabGelPredicateFormed: completed,
            EngramCandidateFormed: completed,
            EvidenceBodyFormed: completed,
            WitnessBodyFormed: completed,
            CoolingHeld: completed,
            PreAdmissionReviewRequired: completed,
            LabGelReadbackAvailable: completed,
            CandidateRetainedAsLabSubstrate: completed,
            LabGelAdmitted: false,
            EngramAdmitted: false,
            MemoryAdmitted: false,
            SelfGelMutated: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false,
            ActionAuthorized: false,
            ActivationRefused: true,
            ModelBindingAllowed: sliLispLabGelReceipt?.ModelBindingAllowed == true,
            ArbitraryLispEvaluationAllowed: sliLispLabGelReceipt?.ArbitraryEvaluationAllowed == true,
            RuntimeIdentityAllowed: false,
            RuntimeActionAllowed: sliLispLabGelReceipt?.RuntimeActionAllowed == true,
            DatabaseWriteAllowed: false,
            GelPromotionAllowed: sliLispLabGelReceipt?.GelPromotionAllowed == true,
            CmeActualAllowed: sliLispLabGelReceipt?.CmeActualActivationAllowed == true,
            SanctuaryActualAllowed: sliLispLabGelReceipt?.SanctuaryActualActivationAllowed == true,
            TimestampUtc: timestampUtc);
    }

    private static void WriteReceiptAndLedgerIfPossible(SanctuaryLabGelEngrammitizationReceipt receipt)
    {
        if (string.IsNullOrWhiteSpace(receipt.ReceiptJsonPath) ||
            string.IsNullOrWhiteSpace(receipt.ReceiptMarkdownPath))
        {
            return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(receipt.ReceiptJsonPath) ?? receipt.InstallRootPath);
        File.WriteAllText(receipt.ReceiptJsonPath, SanctuaryLabGelEngrammitizationReportWriter.ToJson(receipt), Encoding.UTF8);
        File.WriteAllText(receipt.ReceiptMarkdownPath, SanctuaryLabGelEngrammitizationReportWriter.ToMarkdown(receipt), Encoding.UTF8);

        if (!string.IsNullOrWhiteSpace(receipt.SessionLedgerPath))
        {
            var ledgerRecord = new
            {
                receipt.TimestampUtc,
                receipt.ReceiptHandle,
                receipt.Disposition,
                receipt.OutcomeCode,
                receipt.SourceWarmUseReceiptHandle,
                receipt.OperatorId,
                receipt.Domain,
                receipt.Role,
                receipt.JobClass,
                receipt.SessionId,
                receipt.TurnIndex,
                receipt.PriorLabGelReceiptHandle,
                receipt.LabGelPredicateFormed,
                PredicateCount = receipt.Predicates.Count,
                receipt.EngramCandidateFormed,
                receipt.CandidateRetainedAsLabSubstrate,
                receipt.LabGelAdmitted,
                receipt.SelfGelMutated,
                receipt.ContinuityAdmitted,
                receipt.AuthorityGranted,
                receipt.ActionAuthorized,
                receipt.CmeActualAllowed,
                receipt.SanctuaryActualAllowed
            };
            File.AppendAllText(receipt.SessionLedgerPath, JsonSerializer.Serialize(ledgerRecord, JsonOptions) + Environment.NewLine, Encoding.UTF8);
        }
    }

    private static LabGelPredicateClass ToPredicateClass(string value)
    {
        if (Enum.TryParse<LabGelPredicateClass>(value, ignoreCase: true, out var predicateClass))
        {
            return predicateClass;
        }

        return LabGelPredicateClass.Semantic;
    }

    private static string NormalizeIdentitySegment(string value, string fallback)
    {
        var source = string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
        var builder = new StringBuilder();
        foreach (var ch in source)
        {
            if (char.IsLetterOrDigit(ch) || ch is '-' or '_' or '.')
            {
                builder.Append(ch);
            }
        }

        return builder.Length == 0 ? fallback : builder.ToString();
    }

    private static string ShortHash(params string[] values)
    {
        var material = string.Join("|", values.Select(static value => value?.Trim() ?? string.Empty));
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return Convert.ToHexString(hash).ToLowerInvariant()[..16];
    }
}
