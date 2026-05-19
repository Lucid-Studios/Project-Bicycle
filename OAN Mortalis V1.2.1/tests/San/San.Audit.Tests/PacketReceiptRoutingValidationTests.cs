using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class PacketReceiptRoutingValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-12T00:00:00Z");

    [Fact]
    public void Route_Accepted_Packet_Emits_Passage_Receipt()
    {
        var packet = CreatePacket();
        var validationReceipt = Validate(packet);

        var routingReceipt = Route(packet, validationReceipt);

        AssertPassage(routingReceipt);
        Assert.Equal(packet.PacketHandle, routingReceipt.Passage!.PacketHandle);
        Assert.True(routingReceipt.Passage.ProvesPassage);
        Assert.False(routingReceipt.Passage.GrantsPermission);
    }

    [Fact]
    public void Route_Refused_Packet_Emits_Refusal_Routing_Receipt()
    {
        var packet = CreatePacket(telemetryAttemptsAuthority: true);
        var validationReceipt = Validate(packet);

        var routingReceipt = Route(packet, validationReceipt);

        AssertRefusalRouting(routingReceipt, "telemetry-authority-refused");
        Assert.Equal(validationReceipt.Refusal!.ReceiptHandle, routingReceipt.RefusalRouting!.Refusal.ReceiptHandle);
    }

    [Fact]
    public void Route_Passage_Receipt_Routes_To_Steward_Witness()
    {
        var packet = CreatePacket(
            witness: new WitnessReceipt(
                ReceiptHandle: "witness://steward",
                WitnessSurface: SanctuaryPacketSurfaces.Steward,
                SeparateCustody: true));

        var routingReceipt = Route(packet, Validate(packet));

        AssertPassage(routingReceipt);
        Assert.True(routingReceipt.Passage!.WitnessRoute.StewardWitnessPresent);
        Assert.True(routingReceipt.Passage.WitnessRoute.SeparateCustody);
    }

    [Fact]
    public void Route_Prime_Packet_Receipt_Retains_CGoa_Route()
    {
        var packet = CreatePacket(
            source: SanctuaryPacketSurfaces.Prime,
            target: SanctuaryPacketSurfaces.Steward,
            route: SanctuaryPacketRoutes.CGoaInsulated);

        var routingReceipt = Route(packet, Validate(packet));

        AssertPassage(routingReceipt);
        Assert.Equal(SanctuaryPacketRoutes.CGoaInsulated, routingReceipt.Passage!.TelemetryObservation.ObservedRoute);
    }

    [Fact]
    public void Route_Cryptic_Packet_Receipt_Retains_Telemetry_String_Route()
    {
        var packet = CreatePacket(
            source: SanctuaryPacketSurfaces.Cryptic,
            target: SanctuaryPacketSurfaces.Steward,
            route: SanctuaryPacketRoutes.TelemetryString);

        var routingReceipt = Route(packet, Validate(packet));

        AssertPassage(routingReceipt);
        Assert.Equal(SanctuaryPacketRoutes.TelemetryString, routingReceipt.Passage!.TelemetryObservation.ObservedRoute);
    }

    [Fact]
    public void Route_Receipt_Cannot_Authorize_Future_Packet()
    {
        var packet = CreatePacket();
        var routingReceipt = Route(packet, Validate(packet));

        AssertPassage(routingReceipt);
        Assert.False(routingReceipt.AuthorityBoundary.ReceiptMayAuthorizeFuturePacket);
        Assert.False(routingReceipt.AuthorityGranted);
    }

    [Fact]
    public void Route_Receipt_Cannot_Become_Continuity()
    {
        var packet = CreatePacket();
        var routingReceipt = Route(packet, Validate(packet));

        AssertPassage(routingReceipt);
        Assert.False(routingReceipt.AuthorityBoundary.ReceiptMayAdmitContinuity);
        Assert.False(routingReceipt.ContinuityAdmitted);
    }

    [Fact]
    public void Route_Accepted_Packet_Requires_Revocation_Path()
    {
        var packet = CreatePacket(revocationPath: string.Empty);
        var validationReceipt = Validate(packet);

        var routingReceipt = Route(packet, validationReceipt);

        AssertRefusalRouting(routingReceipt, "accepted-packet-revocation-path-missing");
        Assert.True(routingReceipt.RefusalRouting!.RetainsRefusal);
    }

    private static PacketValidationReceipt Validate(SanctuaryPacket packet) =>
        new DefaultSanctuaryPacketMembraneValidator().Validate(packet, TimestampUtc);

    private static PacketReceiptRoutingReceipt Route(
        SanctuaryPacket packet,
        PacketValidationReceipt validationReceipt) =>
        new DefaultPacketReceiptRoutingValidator().Route(packet, validationReceipt, TimestampUtc);

    private static SanctuaryPacket CreatePacket(
        string source = SanctuaryPacketSurfaces.Prime,
        string target = SanctuaryPacketSurfaces.Steward,
        string route = SanctuaryPacketRoutes.CGoaInsulated,
        string revocationPath = "revocation://packet",
        bool telemetryAttemptsAuthority = false,
        WitnessReceipt? witness = null) =>
        new(
            PacketHandle: $"packet://{Guid.NewGuid():N}",
            PacketKind: "candidate-structure",
            Address: new MembraneAddress(
                SourceSurface: source,
                TargetSurface: target,
                Route: route),
            AuthorityCeiling: new AuthorityCeiling(
                CeilingCode: "review-only",
                MayAuthorize: false,
                MayPromoteContinuity: false,
                MayActivate: false),
            CustodyEnvelope: new CustodyEnvelope(
                CustodyOwner: SanctuaryPacketSurfaces.Steward,
                RevocationPath: revocationPath,
                WitnessRefs: ["witness://steward"]),
            Telemetry: new TelemetryString(
                TraceId: "trace://packet",
                Route: route,
                AttemptsAuthority: telemetryAttemptsAuthority),
            Witness: witness ?? new WitnessReceipt(
                ReceiptHandle: "witness://steward",
                WitnessSurface: SanctuaryPacketSurfaces.Steward,
                SeparateCustody: true),
            CompassShell: null,
            AttemptsRuntimeAction: false,
            AttemptsActivation: false,
            AttemptsContinuityPromotion: false,
            AttemptsSelfAuthorization: false);

    private static void AssertPassage(PacketReceiptRoutingReceipt receipt)
    {
        Assert.Equal(PacketReceiptRoutingDisposition.RoutedPassageCold, receipt.Disposition);
        Assert.Equal("packet-passage-receipt-routed-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsColdPassageRoute);
        Assert.Null(receipt.RefusalRouting);
        Assert.NotNull(receipt.Passage);
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityBoundary.ReceiptMayActivate);
    }

    private static void AssertRefusalRouting(PacketReceiptRoutingReceipt receipt, string outcomeCode)
    {
        Assert.Equal(PacketReceiptRoutingDisposition.RoutedRefusalCold, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsColdRefusalRoute);
        Assert.Null(receipt.Passage);
        Assert.NotNull(receipt.RefusalRouting);
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityBoundary.ReceiptMayAuthorizeFuturePacket);
    }
}
