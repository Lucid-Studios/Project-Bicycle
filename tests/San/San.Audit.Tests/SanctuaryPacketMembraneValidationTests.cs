using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class SanctuaryPacketMembraneValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-12T00:00:00Z");

    [Fact]
    public void Validate_Refuses_Packet_Without_Source()
    {
        var receipt = Validate(CreatePacket(source: string.Empty));

        AssertRefused(receipt, "packet-source-missing");
    }

    [Fact]
    public void Validate_Refuses_Packet_Without_Target()
    {
        var receipt = Validate(CreatePacket(target: string.Empty));

        AssertRefused(receipt, "packet-target-missing");
    }

    [Fact]
    public void Validate_Refuses_Telemetry_That_Attempts_Authority()
    {
        var receipt = Validate(CreatePacket(
            source: SanctuaryPacketSurfaces.Telemetry,
            target: SanctuaryPacketSurfaces.Steward,
            telemetryAttemptsAuthority: true));

        AssertRefused(receipt, "telemetry-authority-refused");
    }

    [Fact]
    public void Validate_Refuses_Compass_Shell_That_Claims_Engram()
    {
        var receipt = Validate(CreatePacket(
            source: SanctuaryPacketSurfaces.Compass,
            target: SanctuaryPacketSurfaces.Steward,
            compassShell: new CompassShellPacket(
                ClaimsEngram: true,
                ClaimsTruth: false,
                ClaimsAuthority: false)));

        AssertRefused(receipt, "compass-shell-promotion-refused");
    }

    [Fact]
    public void Validate_Requires_Prime_To_Route_Through_CGoa()
    {
        var directReceipt = Validate(CreatePacket(
            source: SanctuaryPacketSurfaces.Prime,
            target: SanctuaryPacketSurfaces.Steward,
            route: SanctuaryPacketRoutes.Direct));
        var cgoaReceipt = Validate(CreatePacket(
            source: SanctuaryPacketSurfaces.Prime,
            target: SanctuaryPacketSurfaces.Steward,
            route: SanctuaryPacketRoutes.CGoaInsulated));

        AssertRefused(directReceipt, "prime-steward-cgoa-required");
        AssertAccepted(cgoaReceipt);
    }

    [Fact]
    public void Validate_Allows_Cryptic_To_Reach_Steward_By_Telemetry_String()
    {
        var receipt = Validate(CreatePacket(
            source: SanctuaryPacketSurfaces.Cryptic,
            target: SanctuaryPacketSurfaces.Steward,
            route: SanctuaryPacketRoutes.TelemetryString));

        AssertAccepted(receipt);
    }

    [Fact]
    public void Validate_Refuses_Self_Witness_As_Self_Authorization()
    {
        var receipt = Validate(CreatePacket(
            attemptsSelfAuthorization: true,
            witness: new WitnessReceipt(
                ReceiptHandle: "witness://self",
                WitnessSurface: SanctuaryPacketSurfaces.Prime,
                SeparateCustody: false)));

        AssertRefused(receipt, "packet-self-authorization-refused");
    }

    [Fact]
    public void Validate_Refusal_Retains_Receipt()
    {
        var receipt = Validate(CreatePacket(authorityMayAuthorize: true));

        AssertRefused(receipt, "packet-undeclared-authority-refused");
        Assert.NotNull(receipt.Refusal);
        Assert.True(receipt.Refusal!.Retained);
    }

    private static PacketValidationReceipt Validate(SanctuaryPacket packet) =>
        new DefaultSanctuaryPacketMembraneValidator().Validate(packet, TimestampUtc);

    private static SanctuaryPacket CreatePacket(
        string source = SanctuaryPacketSurfaces.Prime,
        string target = SanctuaryPacketSurfaces.Steward,
        string route = SanctuaryPacketRoutes.CGoaInsulated,
        bool authorityMayAuthorize = false,
        bool telemetryAttemptsAuthority = false,
        bool attemptsSelfAuthorization = false,
        CompassShellPacket? compassShell = null,
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
                MayAuthorize: authorityMayAuthorize,
                MayPromoteContinuity: false,
                MayActivate: false),
            CustodyEnvelope: new CustodyEnvelope(
                CustodyOwner: "Steward",
                RevocationPath: "refusal-receipt",
                WitnessRefs: ["witness://standard"]),
            Telemetry: new TelemetryString(
                TraceId: "trace://standard",
                Route: route,
                AttemptsAuthority: telemetryAttemptsAuthority),
            Witness: witness ?? new WitnessReceipt(
                ReceiptHandle: "witness://standard",
                WitnessSurface: "Steward",
                SeparateCustody: true),
            CompassShell: compassShell,
            AttemptsRuntimeAction: false,
            AttemptsActivation: false,
            AttemptsContinuityPromotion: false,
            AttemptsSelfAuthorization: attemptsSelfAuthorization);

    private static void AssertAccepted(PacketValidationReceipt receipt)
    {
        Assert.Equal(SanctuaryPacketValidationDisposition.AcceptedCold, receipt.Disposition);
        Assert.Equal("packet-accepted-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsColdAccepted);
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ContinuityAdmitted);
    }

    private static void AssertRefused(PacketValidationReceipt receipt, string outcomeCode)
    {
        Assert.Equal(SanctuaryPacketValidationDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedRefusal);
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ContinuityAdmitted);
    }
}
