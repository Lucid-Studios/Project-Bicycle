using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class ReceiptReplayBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-12T00:00:00Z");

    [Fact]
    public void Replay_Of_Passage_Receipt_Is_Review_Only()
    {
        var original = CreateRoutingReceipt(CreatePacket());

        var replay = Replay(CreateRequest(originalReceipt: original));

        AssertReplay(replay);
        Assert.Equal(PacketReceiptRoutingDisposition.RoutedPassageCold, replay.OriginalDisposition);
        Assert.Equal(original.ReceiptHandle, replay.OriginalReceiptHandle);
    }

    [Fact]
    public void Replay_Of_Refusal_Receipt_Is_Review_Only()
    {
        var packet = CreatePacket(telemetryAttemptsAuthority: true);
        var original = CreateRoutingReceipt(packet);

        var replay = Replay(CreateRequest(originalReceipt: original));

        AssertReplay(replay);
        Assert.Equal(PacketReceiptRoutingDisposition.RoutedRefusalCold, replay.OriginalDisposition);
        Assert.Equal(original.ReceiptHandle, replay.OriginalReceiptHandle);
    }

    [Fact]
    public void Replay_Does_Not_Emit_New_Packet()
    {
        var replay = Replay(CreateRequest());

        AssertReplay(replay);
        Assert.False(replay.NewPacketEmitted);
        Assert.False(replay.NonReentryBoundary.EmitsNewPacket);
    }

    [Fact]
    public void Replay_Does_Not_Authorize_Future_Packet()
    {
        var replay = Replay(CreateRequest());

        AssertReplay(replay);
        Assert.False(replay.AuthorityGranted);
        Assert.False(replay.NonReentryBoundary.AuthorizesFuturePacket);
    }

    [Fact]
    public void Replay_Does_Not_Admit_Continuity()
    {
        var replay = Replay(CreateRequest());

        AssertReplay(replay);
        Assert.False(replay.ContinuityAdmitted);
        Assert.False(replay.NonReentryBoundary.AdmitsContinuity);
    }

    [Fact]
    public void Replay_Requires_Original_Receipt()
    {
        var replay = Replay(CreateRequest(omitOriginalReceipt: true));

        AssertRefused(replay, "receipt-replay-original-receipt-missing");
    }

    [Fact]
    public void Replay_Requires_Scope_Boundary()
    {
        var request = CreateRequest(scopeBoundary: new ReplayScopeBoundary(
            ScopeCode: string.Empty,
            Present: false,
            ReviewOnly: true,
            AllowsReentry: false));

        var replay = Replay(request);

        AssertRefused(replay, "receipt-replay-scope-boundary-missing");
    }

    [Fact]
    public void Replay_Refuses_Missing_Witness_Context()
    {
        var request = CreateRequest(witnessContext: new ReplayWitnessContext(
            WitnessSurface: string.Empty,
            WitnessPresent: false,
            SeparateCustody: false));

        var replay = Replay(request);

        AssertRefused(replay, "receipt-replay-witness-context-missing");
    }

    [Fact]
    public void Replay_Does_Not_Increment_Passage_Count()
    {
        var replay = Replay(CreateRequest(priorPassageCount: 7));

        AssertReplay(replay);
        Assert.Equal(7, replay.PriorPassageCount);
        Assert.Equal(7, replay.PassageCountAfterReplay);
        Assert.False(replay.NonReentryBoundary.IncrementsPassageCount);
    }

    private static ReceiptReplayReceipt Replay(ReceiptReplayRequest request) =>
        new DefaultReceiptReplayBoundaryValidator().Replay(request, TimestampUtc);

    private static ReceiptReplayRequest CreateRequest(
        PacketReceiptRoutingReceipt? originalReceipt = null,
        ReplayWitnessContext? witnessContext = null,
        ReplayScopeBoundary? scopeBoundary = null,
        int priorPassageCount = 1,
        bool omitOriginalReceipt = false) =>
        new(
            ReplayHandle: $"receipt-replay://{Guid.NewGuid():N}",
            OriginalReceipt: omitOriginalReceipt
                ? null
                : originalReceipt ?? CreateRoutingReceipt(CreatePacket()),
            ReplaySurface: new ReceiptReplaySurface(
                SurfaceName: "StewardReview",
                ReviewOnly: true),
            WitnessContext: witnessContext ?? new ReplayWitnessContext(
                WitnessSurface: SanctuaryPacketSurfaces.Steward,
                WitnessPresent: true,
                SeparateCustody: true),
            ScopeBoundary: scopeBoundary ?? new ReplayScopeBoundary(
                ScopeCode: "review-only",
                Present: true,
                ReviewOnly: true,
                AllowsReentry: false),
            PriorPassageCount: priorPassageCount);

    private static PacketReceiptRoutingReceipt CreateRoutingReceipt(SanctuaryPacket packet)
    {
        var validationReceipt = new DefaultSanctuaryPacketMembraneValidator().Validate(packet, TimestampUtc);
        return new DefaultPacketReceiptRoutingValidator().Route(packet, validationReceipt, TimestampUtc);
    }

    private static SanctuaryPacket CreatePacket(bool telemetryAttemptsAuthority = false) =>
        new(
            PacketHandle: $"packet://{Guid.NewGuid():N}",
            PacketKind: "candidate-structure",
            Address: new MembraneAddress(
                SourceSurface: SanctuaryPacketSurfaces.Prime,
                TargetSurface: SanctuaryPacketSurfaces.Steward,
                Route: SanctuaryPacketRoutes.CGoaInsulated),
            AuthorityCeiling: new AuthorityCeiling(
                CeilingCode: "review-only",
                MayAuthorize: false,
                MayPromoteContinuity: false,
                MayActivate: false),
            CustodyEnvelope: new CustodyEnvelope(
                CustodyOwner: SanctuaryPacketSurfaces.Steward,
                RevocationPath: "revocation://packet",
                WitnessRefs: ["witness://steward"]),
            Telemetry: new TelemetryString(
                TraceId: "trace://packet",
                Route: SanctuaryPacketRoutes.CGoaInsulated,
                AttemptsAuthority: telemetryAttemptsAuthority),
            Witness: new WitnessReceipt(
                ReceiptHandle: "witness://steward",
                WitnessSurface: SanctuaryPacketSurfaces.Steward,
                SeparateCustody: true),
            CompassShell: null,
            AttemptsRuntimeAction: false,
            AttemptsActivation: false,
            AttemptsContinuityPromotion: false,
            AttemptsSelfAuthorization: false);

    private static void AssertReplay(ReceiptReplayReceipt replay)
    {
        Assert.Equal(ReceiptReplayDisposition.ReplayedForReviewCold, replay.Disposition);
        Assert.Equal("receipt-replay-review-only", replay.OutcomeCode);
        Assert.True(replay.IsColdReplay);
        Assert.True(replay.ReviewOnly);
        Assert.False(replay.NewPacketEmitted);
        Assert.False(replay.NewPassageCreated);
        Assert.True(replay.ActivationRefused);
        Assert.False(replay.AuthorityGranted);
        Assert.False(replay.ContinuityAdmitted);
    }

    private static void AssertRefused(ReceiptReplayReceipt replay, string outcomeCode)
    {
        Assert.Equal(ReceiptReplayDisposition.Refused, replay.Disposition);
        Assert.Equal(outcomeCode, replay.OutcomeCode);
        Assert.True(replay.IsRetainedReplayRefusal);
        Assert.NotNull(replay.Refusal);
        Assert.True(replay.Refusal!.Retained);
        Assert.False(replay.NewPacketEmitted);
        Assert.False(replay.NewPassageCreated);
        Assert.True(replay.ActivationRefused);
        Assert.False(replay.AuthorityGranted);
        Assert.False(replay.ContinuityAdmitted);
    }
}
