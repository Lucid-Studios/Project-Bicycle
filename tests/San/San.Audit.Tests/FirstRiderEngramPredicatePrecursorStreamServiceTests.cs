using San.Common;
using San.Product.Preflight;
using Xunit;

namespace San.Audit.Tests;

public sealed class FirstRiderEngramPredicatePrecursorStreamServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-18T00:00:00Z");

    [Fact]
    public void Emit_From_Cold_Rider_Emits_Six_Residue_Classes()
    {
        using var fixture = RiderFixture.Create();
        var rider = CreateColdRider(fixture);

        var receipt = new DefaultFirstRiderEngramPredicatePrecursorStreamService().Emit(rider, TimestampUtc);

        Assert.Equal(EngramPredicatePrecursorStreamDisposition.EmittedCold, receipt.Disposition);
        Assert.Equal("engram-predicate-precursor-stream-emitted-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsColdPrecursorStream);
        Assert.Equal(rider.ReceiptHandle, receipt.SourceRiderReceiptHandle);
        Assert.Equal(6, receipt.Residues.Count);
        Assert.Contains(receipt.Residues, residue => residue.ResidueClass == EngramPredicateResidueClass.Semantic);
        Assert.Contains(receipt.Residues, residue => residue.ResidueClass == EngramPredicateResidueClass.Pressure);
        Assert.Contains(receipt.Residues, residue => residue.ResidueClass == EngramPredicateResidueClass.Witness);
        Assert.Contains(receipt.Residues, residue => residue.ResidueClass == EngramPredicateResidueClass.Governance);
        Assert.Contains(receipt.Residues, residue => residue.ResidueClass == EngramPredicateResidueClass.Morphology);
        Assert.Contains(receipt.Residues, residue => residue.ResidueClass == EngramPredicateResidueClass.Return);
        Assert.All(receipt.Residues, residue => Assert.True(residue.IsColdResidue));
        Assert.Equal(rider.Stages.Count, receipt.WitnessRoute.StageIds.Count);
        Assert.True(receipt.WitnessRoute.IsColdWitnessRoute);
    }

    [Fact]
    public void Predicate_Residue_Does_Not_Become_Engram()
    {
        using var fixture = RiderFixture.Create();
        var receipt = EmitCold(fixture);

        Assert.True(receipt.PreEngramOnly);
        Assert.False(receipt.StreamAdmitsEngram);
        Assert.True(receipt.CandidacyGate.GateClosed);
        Assert.False(receipt.CandidacyGate.AdmitsEngram);
        Assert.All(receipt.Residues, residue =>
        {
            Assert.True(residue.IsPreEngram);
            Assert.False(residue.IsAdmittedEngram);
            Assert.True(residue.RequiresCandidacyReview);
        });
    }

    [Fact]
    public void Witness_Residue_Does_Not_Become_Memory()
    {
        using var fixture = RiderFixture.Create();
        var receipt = EmitCold(fixture);

        Assert.False(receipt.StreamAdmitsMemory);
        Assert.False(receipt.WitnessRoute.AdmitsMemory);
        Assert.False(receipt.CandidacyGate.AdmitsMemory);
        Assert.All(receipt.Residues, residue => Assert.False(residue.IsMemoryAdmitting));
        Assert.All(receipt.RefusalCoolingMarkers, marker => Assert.False(marker.AdmitsMemory));
    }

    [Fact]
    public void Candidacy_Gate_Remains_Closed()
    {
        using var fixture = RiderFixture.Create();
        var receipt = EmitCold(fixture);

        Assert.True(receipt.CandidacyGate.IsColdGate);
        Assert.True(receipt.CandidacyGate.CandidateMaterialAvailable);
        Assert.True(receipt.CandidacyGate.CandidacyReviewRequired);
        Assert.True(receipt.CandidacyGate.GateClosed);
        Assert.False(receipt.CandidacyGate.AdmitsContinuity);
        Assert.False(receipt.CandidacyGate.GrantsAuthority);
        Assert.False(receipt.CandidacyGate.AuthorizesAction);
        Assert.False(receipt.CandidacyGate.PromotesSelfGel);
    }

    [Fact]
    public void Source_Rider_Runtime_Motion_Request_Refuses_Epps()
    {
        using var fixture = RiderFixture.Create();
        var rider = new DefaultFirstRiderGovernanceSimulationService().Simulate(
            fixture.CreateRequest() with { CmeActualRequested = true },
            TimestampUtc);

        var receipt = new DefaultFirstRiderEngramPredicatePrecursorStreamService().Emit(rider, TimestampUtc);

        Assert.Equal(EngramPredicatePrecursorStreamDisposition.Refused, receipt.Disposition);
        Assert.Equal("epps-source-rider-not-cold", receipt.OutcomeCode);
        Assert.False(receipt.IsColdPrecursorStream);
        Assert.Empty(receipt.Residues);
        Assert.Empty(receipt.RefusalCoolingMarkers);
        Assert.False(receipt.CandidacyGate.CandidateMaterialAvailable);
        Assert.True(receipt.CandidacyGate.GateClosed);
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.StreamAdmitsEngram);
        Assert.False(receipt.StreamAdmitsMemory);
        Assert.False(receipt.StreamAdmitsContinuity);
    }

    private static EngramPredicatePrecursorStreamReceipt EmitCold(RiderFixture fixture)
    {
        var rider = CreateColdRider(fixture);
        return new DefaultFirstRiderEngramPredicatePrecursorStreamService().Emit(rider, TimestampUtc);
    }

    private static FirstRiderGovernanceSimulationReceipt CreateColdRider(RiderFixture fixture)
    {
        var rider = new DefaultFirstRiderGovernanceSimulationService().Simulate(
            fixture.CreateRequest("A coherent thought form asks to become predicate evidence without becoming memory."),
            TimestampUtc);

        Assert.True(rider.IsColdRiderReceipt);
        return rider;
    }

    private sealed class RiderFixture : IDisposable
    {
        private RiderFixture(string rootPath)
        {
            RootPath = rootPath;
            LineRootPath = Path.Combine(rootPath, "line-root");
            InstallRootPath = Path.Combine(rootPath, "Sanctuary");
            CellRootPath = Path.Combine(InstallRootPath, "receipts", "spiral-build", "cells");
        }

        public string RootPath { get; }

        public string LineRootPath { get; }

        public string InstallRootPath { get; }

        public string CellRootPath { get; }

        public static RiderFixture Create()
        {
            var fixture = new RiderFixture(Path.Combine(Path.GetTempPath(), $"san-first-rider-epps-tests-{Guid.NewGuid():N}"));
            Directory.CreateDirectory(fixture.LineRootPath);
            Directory.CreateDirectory(Path.Combine(fixture.InstallRootPath, "product"));
            Directory.CreateDirectory(fixture.CellRootPath);
            File.WriteAllText(Path.Combine(fixture.InstallRootPath, "sanctuary.cmd"), string.Empty);
            File.WriteAllText(Path.Combine(fixture.InstallRootPath, "product", "San.Launcher.exe"), string.Empty);

            foreach (var artifact in DefaultFirstRiderGovernanceSimulationService.RequiredStages.SelectMany(static stage => stage.RequiredArtifacts).Distinct(StringComparer.OrdinalIgnoreCase))
            {
                File.WriteAllText(Path.Combine(fixture.CellRootPath, artifact), "{}");
            }

            return fixture;
        }

        public FirstRiderGovernanceSimulationRequest CreateRequest(string? thoughtForm = null) =>
            new(
                LineRootPath: LineRootPath,
                InstallRootPath: InstallRootPath,
                ThoughtForm: thoughtForm);

        public void Dispose()
        {
            if (Directory.Exists(RootPath))
            {
                Directory.Delete(RootPath, recursive: true);
            }
        }
    }
}
