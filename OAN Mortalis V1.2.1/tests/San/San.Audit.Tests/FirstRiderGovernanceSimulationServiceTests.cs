using San.Product.Preflight;
using Xunit;

namespace San.Audit.Tests;

public sealed class FirstRiderGovernanceSimulationServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-17T00:00:00Z");

    [Fact]
    public void Simulate_Returns_Cold_First_Rider_Receipt_When_Route_Artifacts_Are_Installed()
    {
        using var fixture = RiderFixture.Create();

        var receipt = new DefaultFirstRiderGovernanceSimulationService().Simulate(
            fixture.CreateRequest("A coherent thought form asks to become action without warrant."),
            TimestampUtc);

        Assert.Equal(FirstRiderGovernanceSimulationDisposition.SimulatedCold, receipt.Disposition);
        Assert.Equal("first-rider-governance-simulated-cold", receipt.OutcomeCode);
        Assert.True(receipt.IsColdRiderReceipt);
        Assert.Equal("tiny-bicycle-001", receipt.RiderName);
        Assert.Equal(12, receipt.Stages.Count);
        Assert.True(receipt.RouteComplete);
        Assert.True(receipt.ReviewOnly);
        Assert.True(receipt.SimulatedOnly);
        Assert.True(receipt.ArtifactBodyVerified);
        Assert.True(receipt.ActionRefused);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.Empty(receipt.MissingArtifacts);
        AssertForbiddenMotionFalse(receipt);
        Assert.Contains(receipt.Stages, stage => stage.StageId == "listening-frame-emanation");
        Assert.Contains(receipt.Stages, stage => stage.StageId == "compass-pressure-orientation");
        Assert.Contains(receipt.Stages, stage => stage.StageId == "dialogos-discernment");
        Assert.Contains(receipt.Stages, stage => stage.StageId == "steward-harmonic-interlock");
        Assert.Contains(receipt.Stages, stage => stage.StageId == "membrane-morphology-transition");
        Assert.Contains(receipt.Stages, stage => stage.StageId == "review-only-return-to-prime");
        Assert.All(receipt.Stages, stage =>
        {
            Assert.True(stage.ArtifactSurfaceVerified);
            Assert.True(stage.ReviewOnly);
            Assert.False(stage.AuthorityGranted);
            Assert.False(stage.ActionAuthorized);
            Assert.False(stage.ContinuityMutated);
            Assert.False(stage.RuntimeMotionRequested);
            Assert.Empty(stage.MissingArtifacts);
        });
    }

    [Fact]
    public void Simulate_Withholds_When_Route_Artifact_Is_Missing()
    {
        using var fixture = RiderFixture.Create();
        var missingArtifact = DefaultFirstRiderGovernanceSimulationService
            .RequiredStages
            .Single(stage => stage.StageId == "dialogos-discernment")
            .RequiredArtifacts
            .First();
        File.Delete(Path.Combine(fixture.CellRootPath, missingArtifact));

        var receipt = new DefaultFirstRiderGovernanceSimulationService().Simulate(
            fixture.CreateRequest(),
            TimestampUtc);

        Assert.Equal(FirstRiderGovernanceSimulationDisposition.Withheld, receipt.Disposition);
        Assert.Equal("first-rider-required-artifact-missing", receipt.OutcomeCode);
        Assert.False(receipt.IsColdRiderReceipt);
        Assert.False(receipt.RouteComplete);
        Assert.False(receipt.ArtifactBodyVerified);
        Assert.Contains(missingArtifact, receipt.MissingArtifacts);
        Assert.Contains(receipt.Stages, stage =>
            stage.StageId == "dialogos-discernment" &&
            !stage.ArtifactSurfaceVerified &&
            stage.MissingArtifacts.Contains(missingArtifact));
        AssertForbiddenMotionFalse(receipt);
    }

    [Fact]
    public void Simulate_Refuses_When_Runtime_Motion_Is_Requested()
    {
        using var fixture = RiderFixture.Create();
        var request = fixture.CreateRequest() with
        {
            CmeActualRequested = true
        };

        var receipt = new DefaultFirstRiderGovernanceSimulationService().Simulate(request, TimestampUtc);

        Assert.Equal(FirstRiderGovernanceSimulationDisposition.Refused, receipt.Disposition);
        Assert.Equal("first-rider-runtime-motion-refused", receipt.OutcomeCode);
        Assert.False(receipt.IsColdRiderReceipt);
        Assert.True(receipt.ActionRefused);
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.CmeActualAllowed);
        Assert.Empty(receipt.Stages);
    }

    private static void AssertForbiddenMotionFalse(FirstRiderGovernanceSimulationReceipt receipt)
    {
        Assert.True(receipt.ActivationRefused);
        Assert.False(receipt.ModelBindingAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.RuntimeIdentityAllowed);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.DatabaseWriteAllowed);
        Assert.False(receipt.GelPromotionAllowed);
        Assert.False(receipt.CmeActualAllowed);
        Assert.False(receipt.SanctuaryActualAllowed);
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
            var fixture = new RiderFixture(Path.Combine(Path.GetTempPath(), $"san-first-rider-tests-{Guid.NewGuid():N}"));
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
