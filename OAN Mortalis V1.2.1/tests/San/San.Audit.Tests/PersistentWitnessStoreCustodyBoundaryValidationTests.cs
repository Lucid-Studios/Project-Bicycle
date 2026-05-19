using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class PersistentWitnessStoreCustodyBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-13T00:00:00Z");

    [Fact]
    public void Store_Accepts_Local_Append_Only_Review_Evidence()
    {
        var store = Store(CreateRequest());

        AssertColdStore(store);
        Assert.Equal(PersistentWitnessStoreDisposition.StoredForReviewCold, store.Disposition);
        Assert.Equal("persistent-witness-store-review-only", store.OutcomeCode);
        Assert.Equal(3, store.StoredEntries.Count);
    }

    [Fact]
    public void Store_Preserves_Original_Receipt_Handles_Source_Handles_And_Artifact_Lineage()
    {
        var entries = CreateEntries();

        var store = Store(CreateRequest(entries: entries));

        AssertColdStore(store);
        Assert.All(entries.Select(static entry => entry.OriginalReceiptHandle), handle =>
            Assert.Contains(handle, store.PreservedOriginalReceiptHandles));
        Assert.All(entries.SelectMany(static entry => entry.SourceHandles), handle =>
            Assert.Contains(handle, store.PreservedSourceHandles));
        Assert.All(entries.SelectMany(static entry => entry.ArtifactLineage), artifact =>
            Assert.Contains(artifact, store.PreservedArtifactLineage));
    }

    [Fact]
    public void Empty_Store_Is_Reviewable_But_Not_Authoritative()
    {
        var store = Store(CreateRequest(entries: []));

        AssertColdStore(store);
        Assert.Equal(PersistentWitnessStoreDisposition.EmptyReviewCold, store.Disposition);
        Assert.Empty(store.StoredEntries);
        Assert.Empty(store.PreservedOriginalReceiptHandles);
        Assert.False(store.AuthorityGranted);
        Assert.False(store.ContinuityAdmitted);
    }

    [Fact]
    public void Store_Does_Not_Replay_Emit_Packets_Increment_Passage_Or_Admit_Continuity()
    {
        var store = Store(CreateRequest(priorPassageCount: 177));

        AssertColdStore(store);
        Assert.Equal(177, store.PriorPassageCount);
        Assert.Equal(177, store.PassageCountAfterStore);
        Assert.False(store.ReceiptsReplayed);
        Assert.False(store.NewPacketEmitted);
        Assert.False(store.ContinuityAdmitted);
        Assert.False(store.NonAuthorityBoundary.StorageMayReplayReceipts);
        Assert.False(store.NonAuthorityBoundary.StorageMayEmitPackets);
        Assert.False(store.NonAuthorityBoundary.StorageMayIncrementPassageCount);
        Assert.False(store.NonAuthorityBoundary.StorageMayBecomeContinuity);
    }

    [Fact]
    public void Store_Does_Not_Become_Database_Write_Model_Memory_Provider_Access_Or_Authority()
    {
        var store = Store(CreateRequest());

        AssertColdStore(store);
        Assert.False(store.DatabaseWriteRequested);
        Assert.False(store.ModelMemoryGranted);
        Assert.False(store.ProviderVisibleAccessGranted);
        Assert.False(store.AuthorityGranted);
        Assert.False(store.NonAuthorityBoundary.StorageMayBecomeDatabaseWrite);
        Assert.False(store.NonAuthorityBoundary.StorageMayBecomeModelMemory);
        Assert.False(store.NonAuthorityBoundary.StorageMayBecomeProviderVisibleAccess);
        Assert.False(store.NonAuthorityBoundary.StorageMayBecomeAuthority);
    }

    [Fact]
    public void Store_Requires_Scope_Boundary()
    {
        var store = Store(CreateRequest(scopeBoundary: new PersistentWitnessStoreScopeBoundary(
            ScopeCode: string.Empty,
            Present: false,
            ReviewOnly: true,
            LocalOnly: true,
            AllowsDatabaseWrite: false,
            AllowsProviderVisibleAccess: false,
            AllowsModelMemory: false,
            AllowsResearchUse: false,
            AllowsEvidenceReplacement: false,
            AllowsReceiptReplay: false,
            AllowsPacketEmission: false,
            AllowsContinuityAdmission: false,
            AllowsAuthority: false,
            AllowsActivation: false,
            AllowsRuntimeAction: false)));

        AssertRefused(store, "persistent-witness-store-scope-boundary-missing");
    }

    [Theory]
    [InlineData("review-only")]
    [InlineData("local-only")]
    [InlineData("database-write")]
    [InlineData("provider-access")]
    [InlineData("model-memory")]
    [InlineData("research-use")]
    [InlineData("evidence-replacement")]
    [InlineData("receipt-replay")]
    [InlineData("packet-emission")]
    [InlineData("continuity")]
    [InlineData("authority")]
    [InlineData("activation")]
    [InlineData("runtime-action")]
    public void Store_Refuses_Promotional_Scope(string forbiddenScope)
    {
        var store = Store(CreateRequest(scopeBoundary: CreateScopeBoundary(forbiddenScope)));

        AssertRefused(store, "persistent-witness-store-promotional-scope-refused");
    }

    [Theory]
    [InlineData("missing-owner")]
    [InlineData("missing-witness")]
    [InlineData("witness-not-present")]
    [InlineData("same-custody")]
    [InlineData("non-local")]
    [InlineData("non-append")]
    [InlineData("not-review")]
    public void Store_Refuses_Missing_Or_Promotional_Custody(string custodyCase)
    {
        var store = Store(CreateRequest(custodyContext: CreateCustodyContext(custodyCase)));

        AssertRefused(store, "persistent-witness-store-custody-context-refused");
    }

    [Theory]
    [InlineData("missing-entry-handle")]
    [InlineData("missing-original-receipt")]
    [InlineData("missing-source")]
    [InlineData("missing-artifact")]
    [InlineData("missing-governance")]
    [InlineData("no-original-preservation")]
    [InlineData("no-artifact-preservation")]
    [InlineData("not-review-only")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("activation")]
    [InlineData("runtime-action")]
    [InlineData("database-write")]
    [InlineData("model-memory")]
    [InlineData("provider-access")]
    [InlineData("evidence-replacement")]
    [InlineData("receipt-replay")]
    [InlineData("packet-emission")]
    [InlineData("passage-increment")]
    public void Store_Refuses_Entry_That_Is_Not_Cold_Evidence(string entryCase)
    {
        var entries = CreateEntries();
        entries[0] = MutateEntry(entries[0], entryCase);

        var store = Store(CreateRequest(entries: entries));

        AssertRefused(store, "persistent-witness-store-entry-not-cold");
    }

    [Fact]
    public void Store_Refuses_Duplicate_Entry_Handles()
    {
        var entries = CreateEntries();
        entries[1] = entries[1] with { EntryHandle = entries[0].EntryHandle };

        var store = Store(CreateRequest(entries: entries));

        AssertRefused(store, "persistent-witness-store-duplicate-entry-refused");
    }

    private static PersistentWitnessStoreReceipt Store(PersistentWitnessStoreRequest request) =>
        new DefaultPersistentWitnessStoreCustodyBoundaryValidator().Store(request, TimestampUtc);

    private static PersistentWitnessStoreRequest CreateRequest(
        IReadOnlyList<PersistentWitnessStoreEntry>? entries = null,
        PersistentWitnessStoreScopeBoundary? scopeBoundary = null,
        PersistentWitnessStoreCustodyContext? custodyContext = null,
        int priorPassageCount = 81) =>
        new(
            StoreHandle: $"urn:san:persistent-witness-store:{Guid.NewGuid():N}",
            Entries: entries ?? CreateEntries(),
            ScopeBoundary: scopeBoundary ?? CreateScopeBoundary(),
            CustodyContext: custodyContext ?? CreateCustodyContext(),
            PriorPassageCount: priorPassageCount);

    private static PersistentWitnessStoreEntry[] CreateEntries() =>
    [
        Entry("passage", "urn:san:receipt:packet-passage:01", PersistentWitnessStoreRecordKind.PassageReceipt),
        Entry("summary", "urn:san:receipt:witness-summary:01", PersistentWitnessStoreRecordKind.WitnessSummary),
        Entry("braid", "urn:san:receipt:swarm-braid:01", PersistentWitnessStoreRecordKind.SwarmBraid)
    ];

    private static PersistentWitnessStoreEntry Entry(
        string handleSuffix,
        string originalReceiptHandle,
        PersistentWitnessStoreRecordKind kind) =>
        new(
            EntryHandle: $"urn:san:persistent-witness-entry:{handleSuffix}",
            OriginalReceiptHandle: originalReceiptHandle,
            RecordKind: kind,
            SourceHandles:
            [
                $"urn:san:source:{handleSuffix}:a",
                $"urn:san:source:{handleSuffix}:b"
            ],
            ArtifactLineage:
            [
                $"artifact:{handleSuffix}:contract-map",
                $"artifact:{handleSuffix}:non-authority-ledger"
            ],
            GovernanceTrace: "retained as local append-only witness evidence for review",
            PreservesOriginalReceiptHandle: true,
            PreservesArtifactLineage: true,
            ReviewOnly: true,
            AuthorityBoundary: CreateEntryAuthorityBoundary());

    private static PersistentWitnessStoreScopeBoundary CreateScopeBoundary(string? forbiddenScope = null) =>
        new(
            ScopeCode: "persistent-witness-store-review-only",
            Present: true,
            ReviewOnly: forbiddenScope != "review-only",
            LocalOnly: forbiddenScope != "local-only",
            AllowsDatabaseWrite: forbiddenScope == "database-write",
            AllowsProviderVisibleAccess: forbiddenScope == "provider-access",
            AllowsModelMemory: forbiddenScope == "model-memory",
            AllowsResearchUse: forbiddenScope == "research-use",
            AllowsEvidenceReplacement: forbiddenScope == "evidence-replacement",
            AllowsReceiptReplay: forbiddenScope == "receipt-replay",
            AllowsPacketEmission: forbiddenScope == "packet-emission",
            AllowsContinuityAdmission: forbiddenScope == "continuity",
            AllowsAuthority: forbiddenScope == "authority",
            AllowsActivation: forbiddenScope == "activation",
            AllowsRuntimeAction: forbiddenScope == "runtime-action");

    private static PersistentWitnessStoreCustodyContext CreateCustodyContext(string? custodyCase = null) =>
        new(
            CustodyOwner: custodyCase == "missing-owner" ? string.Empty : "Steward",
            WitnessSurface: custodyCase == "missing-witness" ? string.Empty : "WitnessLedger",
            WitnessPresent: custodyCase != "witness-not-present",
            SeparateCustody: custodyCase != "same-custody",
            LocalOnly: custodyCase != "non-local",
            AppendOnly: custodyCase != "non-append",
            ReviewOnly: custodyCase != "not-review");

    private static PersistentWitnessStoreEntryAuthorityBoundary CreateEntryAuthorityBoundary(string? forbiddenMotion = null) =>
        new(
            AuthorityRequested: forbiddenMotion == "authority",
            ContinuityAdmissionRequested: forbiddenMotion == "continuity",
            ActivationRequested: forbiddenMotion == "activation",
            RuntimeActionRequested: forbiddenMotion == "runtime-action",
            DatabaseWriteRequested: forbiddenMotion == "database-write",
            ModelMemoryRequested: forbiddenMotion == "model-memory",
            ProviderVisibleAccessRequested: forbiddenMotion == "provider-access",
            EvidenceReplacementRequested: forbiddenMotion == "evidence-replacement",
            ReceiptReplayRequested: forbiddenMotion == "receipt-replay",
            PacketEmissionRequested: forbiddenMotion == "packet-emission",
            IncrementsPassageCount: forbiddenMotion == "passage-increment");

    private static PersistentWitnessStoreEntry MutateEntry(
        PersistentWitnessStoreEntry entry,
        string entryCase) =>
        entryCase switch
        {
            "missing-entry-handle" => entry with { EntryHandle = string.Empty },
            "missing-original-receipt" => entry with { OriginalReceiptHandle = string.Empty },
            "missing-source" => entry with { SourceHandles = [] },
            "missing-artifact" => entry with { ArtifactLineage = [] },
            "missing-governance" => entry with { GovernanceTrace = string.Empty },
            "no-original-preservation" => entry with { PreservesOriginalReceiptHandle = false },
            "no-artifact-preservation" => entry with { PreservesArtifactLineage = false },
            "not-review-only" => entry with { ReviewOnly = false },
            _ => entry with { AuthorityBoundary = CreateEntryAuthorityBoundary(entryCase) }
        };

    private static void AssertColdStore(PersistentWitnessStoreReceipt store)
    {
        Assert.True(store.IsColdWitnessStore);
        Assert.True(store.ReviewOnly);
        Assert.True(store.LocalOnly);
        Assert.True(store.AppendOnly);
        Assert.True(store.SeparateCustody);
        Assert.True(store.ActivationRefused);
        Assert.False(store.AuthorityGranted);
        Assert.False(store.ContinuityAdmitted);
        Assert.False(store.DatabaseWriteRequested);
        Assert.False(store.ModelMemoryGranted);
        Assert.False(store.ProviderVisibleAccessGranted);
        Assert.False(store.EvidenceReplaced);
        Assert.False(store.ReceiptsReplayed);
        Assert.False(store.NewPacketEmitted);
    }

    private static void AssertRefused(PersistentWitnessStoreReceipt store, string outcomeCode)
    {
        Assert.Equal(PersistentWitnessStoreDisposition.Refused, store.Disposition);
        Assert.Equal(outcomeCode, store.OutcomeCode);
        Assert.NotNull(store.Refusal);
        Assert.Equal(store.PriorPassageCount, store.PassageCountAfterStore);
        Assert.False(store.AuthorityGranted);
        Assert.False(store.ContinuityAdmitted);
        Assert.False(store.DatabaseWriteRequested);
        Assert.False(store.NewPacketEmitted);
    }
}
