using System.Security.Cryptography;
using System.Text;

namespace San.Common;

public enum PersistentWitnessStoreDisposition
{
    EmptyReviewCold = 0,
    StoredForReviewCold = 1,
    Refused = 2
}

public enum PersistentWitnessStoreRecordKind
{
    PassageReceipt = 0,
    RefusalReceipt = 1,
    QueryReceipt = 2,
    SelectionReceipt = 3,
    WitnessSummary = 4,
    CompassPressure = 5,
    EngramCandidate = 6,
    SwarmBraid = 7,
    ArtifactLineage = 8
}

public sealed record PersistentWitnessStoreScopeBoundary(
    string ScopeCode,
    bool Present,
    bool ReviewOnly,
    bool LocalOnly,
    bool AllowsDatabaseWrite,
    bool AllowsProviderVisibleAccess,
    bool AllowsModelMemory,
    bool AllowsResearchUse,
    bool AllowsEvidenceReplacement,
    bool AllowsReceiptReplay,
    bool AllowsPacketEmission,
    bool AllowsContinuityAdmission,
    bool AllowsAuthority,
    bool AllowsActivation,
    bool AllowsRuntimeAction);

public sealed record PersistentWitnessStoreCustodyContext(
    string CustodyOwner,
    string WitnessSurface,
    bool WitnessPresent,
    bool SeparateCustody,
    bool LocalOnly,
    bool AppendOnly,
    bool ReviewOnly);

public sealed record PersistentWitnessStoreEntryAuthorityBoundary(
    bool AuthorityRequested,
    bool ContinuityAdmissionRequested,
    bool ActivationRequested,
    bool RuntimeActionRequested,
    bool DatabaseWriteRequested,
    bool ModelMemoryRequested,
    bool ProviderVisibleAccessRequested,
    bool EvidenceReplacementRequested,
    bool ReceiptReplayRequested,
    bool PacketEmissionRequested,
    bool IncrementsPassageCount)
{
    public bool RequestsForbiddenMotion =>
        AuthorityRequested ||
        ContinuityAdmissionRequested ||
        ActivationRequested ||
        RuntimeActionRequested ||
        DatabaseWriteRequested ||
        ModelMemoryRequested ||
        ProviderVisibleAccessRequested ||
        EvidenceReplacementRequested ||
        ReceiptReplayRequested ||
        PacketEmissionRequested ||
        IncrementsPassageCount;
}

public sealed record PersistentWitnessStoreEntry(
    string EntryHandle,
    string OriginalReceiptHandle,
    PersistentWitnessStoreRecordKind RecordKind,
    IReadOnlyList<string> SourceHandles,
    IReadOnlyList<string> ArtifactLineage,
    string GovernanceTrace,
    bool PreservesOriginalReceiptHandle,
    bool PreservesArtifactLineage,
    bool ReviewOnly,
    PersistentWitnessStoreEntryAuthorityBoundary AuthorityBoundary)
{
    public bool IsColdEvidence =>
        !string.IsNullOrWhiteSpace(EntryHandle) &&
        !string.IsNullOrWhiteSpace(OriginalReceiptHandle) &&
        SourceHandles.Count > 0 &&
        SourceHandles.All(static handle => !string.IsNullOrWhiteSpace(handle)) &&
        ArtifactLineage.Count > 0 &&
        ArtifactLineage.All(static artifact => !string.IsNullOrWhiteSpace(artifact)) &&
        !string.IsNullOrWhiteSpace(GovernanceTrace) &&
        PreservesOriginalReceiptHandle &&
        PreservesArtifactLineage &&
        ReviewOnly &&
        !AuthorityBoundary.RequestsForbiddenMotion;
}

public sealed record PersistentWitnessStoreNonAuthorityBoundary(
    bool StorageMayBecomeAuthority,
    bool StorageMayBecomeContinuity,
    bool StorageMayBecomeModelMemory,
    bool StorageMayBecomeDatabaseWrite,
    bool StorageMayBecomeProviderVisibleAccess,
    bool StorageMayReplaceEvidence,
    bool StorageMayReplayReceipts,
    bool StorageMayEmitPackets,
    bool StorageMayIncrementPassageCount,
    string BoundaryLaw);

public sealed record PersistentWitnessStoreRefusalReceipt(
    string ReceiptHandle,
    string RefusalCode,
    string GovernanceTrace,
    bool Retained);

public sealed record PersistentWitnessStoreRequest(
    string StoreHandle,
    IReadOnlyList<PersistentWitnessStoreEntry> Entries,
    PersistentWitnessStoreScopeBoundary ScopeBoundary,
    PersistentWitnessStoreCustodyContext CustodyContext,
    int PriorPassageCount);

public sealed record PersistentWitnessStoreReceipt(
    string ReceiptHandle,
    PersistentWitnessStoreDisposition Disposition,
    string OutcomeCode,
    string GovernanceTrace,
    string StoreHandle,
    IReadOnlyList<PersistentWitnessStoreEntry> StoredEntries,
    IReadOnlyList<string> PreservedOriginalReceiptHandles,
    IReadOnlyList<string> PreservedSourceHandles,
    IReadOnlyList<string> PreservedArtifactLineage,
    PersistentWitnessStoreNonAuthorityBoundary NonAuthorityBoundary,
    PersistentWitnessStoreRefusalReceipt? Refusal,
    int PriorPassageCount,
    int PassageCountAfterStore,
    bool ReviewOnly,
    bool LocalOnly,
    bool AppendOnly,
    bool SeparateCustody,
    bool DatabaseWriteRequested,
    bool ModelMemoryGranted,
    bool ProviderVisibleAccessGranted,
    bool EvidenceReplaced,
    bool ReceiptsReplayed,
    bool NewPacketEmitted,
    bool ContinuityAdmitted,
    bool ActivationRefused,
    bool AuthorityGranted,
    DateTimeOffset TimestampUtc)
{
    public bool IsColdWitnessStore =>
        (Disposition is PersistentWitnessStoreDisposition.StoredForReviewCold or PersistentWitnessStoreDisposition.EmptyReviewCold) &&
        ReviewOnly &&
        LocalOnly &&
        AppendOnly &&
        SeparateCustody &&
        !DatabaseWriteRequested &&
        !ModelMemoryGranted &&
        !ProviderVisibleAccessGranted &&
        !EvidenceReplaced &&
        !ReceiptsReplayed &&
        !NewPacketEmitted &&
        !ContinuityAdmitted &&
        ActivationRefused &&
        !AuthorityGranted &&
        PassageCountAfterStore == PriorPassageCount;
}

public sealed class DefaultPersistentWitnessStoreCustodyBoundaryValidator
{
    private static readonly PersistentWitnessStoreNonAuthorityBoundary NonAuthorityBoundary = new(
        StorageMayBecomeAuthority: false,
        StorageMayBecomeContinuity: false,
        StorageMayBecomeModelMemory: false,
        StorageMayBecomeDatabaseWrite: false,
        StorageMayBecomeProviderVisibleAccess: false,
        StorageMayReplaceEvidence: false,
        StorageMayReplayReceipts: false,
        StorageMayEmitPackets: false,
        StorageMayIncrementPassageCount: false,
        BoundaryLaw: "Witness storage may preserve evidence for review. Storage may not become memory, authority, warrant, continuity, activation, replay, packet emission, or database write.");

    public PersistentWitnessStoreReceipt Store(
        PersistentWitnessStoreRequest request,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!request.ScopeBoundary.Present ||
            string.IsNullOrWhiteSpace(request.ScopeBoundary.ScopeCode))
        {
            return Refuse(
                request,
                "persistent-witness-store-scope-boundary-missing",
                "Persistent witness store custody refused because a local review-only scope boundary is required.",
                timestampUtc);
        }

        if (!IsColdScope(request.ScopeBoundary))
        {
            return Refuse(
                request,
                "persistent-witness-store-promotional-scope-refused",
                "Persistent witness store custody refused because scope must be review-only and local-only while refusing database write, provider-visible access, model memory, research use, evidence replacement, replay, packet emission, continuity, authority, activation, and runtime action.",
                timestampUtc);
        }

        if (!IsColdCustody(request.CustodyContext))
        {
            return Refuse(
                request,
                "persistent-witness-store-custody-context-refused",
                "Persistent witness store custody refused because separate local append-only witness custody is required.",
                timestampUtc);
        }

        if (request.Entries.Count == 0)
        {
            return CreateReceipt(
                request,
                PersistentWitnessStoreDisposition.EmptyReviewCold,
                "persistent-witness-store-empty-review-only",
                "Persistent witness store custody found no entries. Empty storage is reviewable but grants no authority, continuity, activation, model memory, database write, provider-visible access, replay, packet emission, or evidence replacement.",
                entries: [],
                timestampUtc);
        }

        if (request.Entries.Any(static entry => !entry.IsColdEvidence))
        {
            return Refuse(
                request,
                "persistent-witness-store-entry-not-cold",
                "Persistent witness store custody refused because each entry must preserve original receipt handle, source handles, artifact lineage, governance trace, review-only posture, and non-promotional authority boundary.",
                timestampUtc);
        }

        if (request.Entries
            .GroupBy(static entry => entry.EntryHandle, StringComparer.Ordinal)
            .Any(static group => group.Count() > 1))
        {
            return Refuse(
                request,
                "persistent-witness-store-duplicate-entry-refused",
                "Persistent witness store custody refused because each stored evidence entry must have a distinct entry handle.",
                timestampUtc);
        }

        return CreateReceipt(
            request,
            PersistentWitnessStoreDisposition.StoredForReviewCold,
            "persistent-witness-store-review-only",
            "Persistent witness store custody retained cold evidence for review while preserving original handles and artifact lineage, and refusing authority, continuity, activation, model memory, database write, provider-visible access, evidence replacement, replay, packet emission, and passage increment.",
            request.Entries.ToArray(),
            timestampUtc);
    }

    private static bool IsColdScope(PersistentWitnessStoreScopeBoundary scope) =>
        scope.ReviewOnly &&
        scope.LocalOnly &&
        !scope.AllowsDatabaseWrite &&
        !scope.AllowsProviderVisibleAccess &&
        !scope.AllowsModelMemory &&
        !scope.AllowsResearchUse &&
        !scope.AllowsEvidenceReplacement &&
        !scope.AllowsReceiptReplay &&
        !scope.AllowsPacketEmission &&
        !scope.AllowsContinuityAdmission &&
        !scope.AllowsAuthority &&
        !scope.AllowsActivation &&
        !scope.AllowsRuntimeAction;

    private static bool IsColdCustody(PersistentWitnessStoreCustodyContext custody) =>
        !string.IsNullOrWhiteSpace(custody.CustodyOwner) &&
        !string.IsNullOrWhiteSpace(custody.WitnessSurface) &&
        custody.WitnessPresent &&
        custody.SeparateCustody &&
        custody.LocalOnly &&
        custody.AppendOnly &&
        custody.ReviewOnly;

    private static PersistentWitnessStoreReceipt CreateReceipt(
        PersistentWitnessStoreRequest request,
        PersistentWitnessStoreDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        IReadOnlyList<PersistentWitnessStoreEntry> entries,
        DateTimeOffset timestampUtc)
    {
        var originalHandles = entries
            .Select(static entry => entry.OriginalReceiptHandle)
            .Where(static handle => !string.IsNullOrWhiteSpace(handle))
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        var sourceHandles = entries
            .SelectMany(static entry => entry.SourceHandles)
            .Where(static handle => !string.IsNullOrWhiteSpace(handle))
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        var artifactLineage = entries
            .SelectMany(static entry => entry.ArtifactLineage)
            .Where(static artifact => !string.IsNullOrWhiteSpace(artifact))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        return new PersistentWitnessStoreReceipt(
            ReceiptHandle: $"urn:san:persistent-witness-store:review:{ShortHash(request.StoreHandle, outcomeCode, entries.Count.ToString(System.Globalization.CultureInfo.InvariantCulture), timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            StoreHandle: request.StoreHandle,
            StoredEntries: entries,
            PreservedOriginalReceiptHandles: originalHandles,
            PreservedSourceHandles: sourceHandles,
            PreservedArtifactLineage: artifactLineage,
            NonAuthorityBoundary: NonAuthorityBoundary,
            Refusal: null,
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterStore: request.PriorPassageCount,
            ReviewOnly: true,
            LocalOnly: true,
            AppendOnly: true,
            SeparateCustody: true,
            DatabaseWriteRequested: false,
            ModelMemoryGranted: false,
            ProviderVisibleAccessGranted: false,
            EvidenceReplaced: false,
            ReceiptsReplayed: false,
            NewPacketEmitted: false,
            ContinuityAdmitted: false,
            ActivationRefused: true,
            AuthorityGranted: false,
            TimestampUtc: timestampUtc);
    }

    private static PersistentWitnessStoreReceipt Refuse(
        PersistentWitnessStoreRequest request,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc) =>
        new(
            ReceiptHandle: $"urn:san:persistent-witness-store:refused:{ShortHash(request.StoreHandle, outcomeCode, timestampUtc.UtcTicks.ToString(System.Globalization.CultureInfo.InvariantCulture))}",
            Disposition: PersistentWitnessStoreDisposition.Refused,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            StoreHandle: request.StoreHandle,
            StoredEntries: [],
            PreservedOriginalReceiptHandles: [],
            PreservedSourceHandles: [],
            PreservedArtifactLineage: [],
            NonAuthorityBoundary: NonAuthorityBoundary,
            Refusal: new PersistentWitnessStoreRefusalReceipt(
                ReceiptHandle: $"urn:san:persistent-witness-store-refusal:{ShortHash(request.StoreHandle, outcomeCode)}",
                RefusalCode: outcomeCode,
                GovernanceTrace: governanceTrace,
                Retained: true),
            PriorPassageCount: request.PriorPassageCount,
            PassageCountAfterStore: request.PriorPassageCount,
            ReviewOnly: true,
            LocalOnly: true,
            AppendOnly: true,
            SeparateCustody: true,
            DatabaseWriteRequested: false,
            ModelMemoryGranted: false,
            ProviderVisibleAccessGranted: false,
            EvidenceReplaced: false,
            ReceiptsReplayed: false,
            NewPacketEmitted: false,
            ContinuityAdmitted: false,
            ActivationRefused: true,
            AuthorityGranted: false,
            TimestampUtc: timestampUtc);

    private static string ShortHash(params string[] values)
    {
        var material = string.Join("|", values);
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return Convert.ToHexString(hash).ToLowerInvariant()[..16];
    }
}
