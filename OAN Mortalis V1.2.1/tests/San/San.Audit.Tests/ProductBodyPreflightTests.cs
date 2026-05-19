using San.Product.Preflight;
using System.Text.Json;
using Xunit;

namespace San.Audit.Tests;

public sealed class ProductBodyPreflightTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-12T00:00:00Z");

    [Fact]
    public void Evaluate_Verifies_V121_As_Current_Runnable_Truth_With_Retained_Parent()
    {
        var status = Evaluate(activationRequested: false);

        Assert.Equal(ProductBodyPreflightDisposition.VerifiedCold, status.Disposition);
        Assert.Equal("cold-product-body-preflight-verified", status.OutcomeCode);
        Assert.Equal("OAN Mortalis V1.2.1", status.Manifest!.LineName);
        Assert.Equal("1.3.18", status.Manifest.LineVersion);
        Assert.Equal("current-run-pointing-cold-truth", status.Manifest.Posture);
        Assert.Equal("OAN Mortalis V1.1.1", status.Manifest.ParentLine);
        Assert.Equal("OAN Mortalis V1.2.1", status.Manifest.ActiveExecutableTruth);
        Assert.True(status.RetainedParentPreserved);
        Assert.False(status.SidecarPreserved);
        Assert.True(status.Buildable);
        Assert.True(status.SourceMaterialized);
        Assert.False(status.RuntimeMaterialized);
        Assert.True(status.SolutionPresent);
        Assert.True(status.ParentLinePresent);
        Assert.True(status.ColdCorridorPresent);
        Assert.False(status.ActivationAuthorityPresent);
        Assert.True(status.ActivationRefused);
        Assert.Equal("activation-authority-absent", status.RefusalCode);
        Assert.Equal(DefaultProductBodyPreflightService.DefaultNextAllowedLane, status.NextAllowedLane);
        Assert.Contains(status.Checks, check => check.CheckId == "v121-current-run-pointing-cold-truth-preserved");
        Assert.Contains(status.Checks, check => check.CheckId == "line-is-run-pointing-cold-truth");
        Assert.All(status.Checks, check => Assert.Equal(ProductBodyCheckStatus.Pass, check.Status));
        AssertNoActivationOutputs(status);
    }

    [Fact]
    public void Evaluate_Refuses_Activation_Request_Without_Opening_Runtime()
    {
        var status = Evaluate(activationRequested: true);

        Assert.Equal(ProductBodyPreflightDisposition.Refused, status.Disposition);
        Assert.Equal("activation-authority-absent", status.OutcomeCode);
        Assert.True(status.RetainedParentPreserved);
        Assert.False(status.SidecarPreserved);
        Assert.True(status.ColdCorridorPresent);
        Assert.False(status.ActivationAuthorityPresent);
        Assert.True(status.ActivationRefused);
        Assert.Contains("refused activation", status.GovernanceTrace, StringComparison.OrdinalIgnoreCase);
        AssertNoActivationOutputs(status);
    }

    [Fact]
    public void Evaluate_Verifies_Lab_Sanctuary_Build_Testing_Profile_Without_Activation()
    {
        var lineRoot = FindLineRoot();
        var labContextRoot = CreateLabContextFixture();
        var buildTestingPointer = CreateBuildTestingPointerFixture(lineRoot, labContextRoot);

        var status = Evaluate(
            activationRequested: false,
            verificationProfile: ProductBodyVerificationProfiles.LabSanctuaryBuildTesting,
            labContextRootPath: labContextRoot,
            buildTestingPointerPath: buildTestingPointer);

        Assert.Equal(ProductBodyPreflightDisposition.VerifiedCold, status.Disposition);
        Assert.Equal("lab-sanctuary-build-verification-verified-cold", status.OutcomeCode);
        Assert.Equal(ProductBodyVerificationProfiles.LabSanctuaryBuildTesting, status.VerificationProfile);
        Assert.Equal(DefaultProductBodyPreflightService.LabSanctuaryNextAllowedLane, status.NextAllowedLane);
        Assert.Equal(labContextRoot, status.LabContextRootPath);
        Assert.Equal(buildTestingPointer, status.BuildTestingPointerPath);
        Assert.Contains("current run-pointing cold truth", status.GovernanceTrace, StringComparison.Ordinal);
        Assert.DoesNotContain("current runnable truth", status.GovernanceTrace, StringComparison.Ordinal);
        Assert.Contains(status.Checks, check =>
            check.CheckId == "lab-sanctuary-remains-non-activating" &&
            check.Status == ProductBodyCheckStatus.Pass);
        Assert.All(status.Checks, check => Assert.Equal(ProductBodyCheckStatus.Pass, check.Status));
        AssertNoActivationOutputs(status);
    }

    [Fact]
    public void Report_Writer_Emits_Readable_Json_And_Markdown_Posture()
    {
        var status = Evaluate(activationRequested: false);

        var json = ProductBodyReportWriter.ToJson(status);
        var markdown = ProductBodyReportWriter.ToMarkdown(status);

        Assert.Contains("\"outcomeCode\": \"cold-product-body-preflight-verified\"", json, StringComparison.Ordinal);
        Assert.Contains("# Sanctuary Product Body Preflight", markdown, StringComparison.Ordinal);
        Assert.Contains("Profile: `cold-product-body`", markdown, StringComparison.Ordinal);
        Assert.Contains("Active run-pointing truth", markdown, StringComparison.Ordinal);
        Assert.DoesNotContain("Active executable truth", markdown, StringComparison.Ordinal);
        Assert.DoesNotContain("current runnable truth", markdown, StringComparison.Ordinal);
        Assert.Contains("Activation refused: `True`", markdown, StringComparison.Ordinal);
        Assert.Contains("CME.Actual allowed: `False`", markdown, StringComparison.Ordinal);
        Assert.Contains("Sanctuary.Actual allowed: `False`", markdown, StringComparison.Ordinal);
    }

    [Fact]
    public void RunPointingColdTruth_Does_Not_Claim_Runtime_Authority()
    {
        var status = Evaluate(activationRequested: false);

        Assert.Equal("current_run_pointing_cold_truth_package_plan_review_only", status.NextAllowedLane);
        Assert.False(status.RuntimeMaterialized);
        Assert.False(status.ActivationAuthorityPresent);
        Assert.True(status.ActivationRefused);
        Assert.Contains("run-pointing cold truth", status.GovernanceTrace, StringComparison.Ordinal);
        Assert.DoesNotContain("current runnable truth", status.GovernanceTrace, StringComparison.Ordinal);
        AssertNoActivationOutputs(status);
    }

    private static ProductBodyPreflightStatus Evaluate(
        bool activationRequested,
        string verificationProfile = ProductBodyVerificationProfiles.ColdProductBody,
        string? labContextRootPath = null,
        string? buildTestingPointerPath = null)
    {
        var lineRoot = FindLineRoot();
        var request = new ProductBodyPreflightRequest(
            LineRootPath: lineRoot,
            ActivationRequested: activationRequested,
            ModelBindingRequested: activationRequested,
            LispEvaluationRequested: activationRequested,
            RuntimeIdentityRequested: activationRequested,
            RuntimeActionRequested: activationRequested,
            DatabaseWriteRequested: activationRequested,
            GelPromotionRequested: activationRequested,
            CmeActualRequested: activationRequested,
            SanctuaryActualRequested: activationRequested,
            VerificationProfile: verificationProfile,
            LabContextRootPath: labContextRootPath,
            BuildTestingPointerPath: buildTestingPointerPath);

        return new DefaultProductBodyPreflightService().Evaluate(request, TimestampUtc);
    }

    private static string FindLineRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "San.sln")) &&
                File.Exists(Path.Combine(current.FullName, "build", "line-manifest.json")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new InvalidOperationException("Unable to locate OAN Mortalis V1.2.1 line root from test output path.");
    }

    private static string CreateLabContextFixture()
    {
        var root = Path.Combine(
            Path.GetTempPath(),
            "SanProductPreflightTests",
            Guid.NewGuid().ToString("N"),
            "Lab Context");
        Directory.CreateDirectory(root);
        Directory.CreateDirectory(Path.Combine(root, "Domain Universal"));

        File.WriteAllText(Path.Combine(root, "LAB_CONTEXT_ANCHOR.md"), "# Lab Context Anchor");
        File.WriteAllText(Path.Combine(root, "LAB_POSTURE_ACTIVE.md"), "# Lab Posture Active");
        File.WriteAllText(Path.Combine(root, "drive-file-index.csv"), "path,status");
        File.WriteAllText(Path.Combine(root, "Domain Universal", "domain-universal-index.csv"), "path,status");

        return root;
    }

    private static string CreateBuildTestingPointerFixture(
        string lineRoot,
        string labContextRoot)
    {
        var pointerDirectory = Path.Combine(
            Path.GetTempPath(),
            "SanProductPreflightTests",
            Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(pointerDirectory);

        var pointerPath = Path.Combine(pointerDirectory, "LAB_BUILD_PREDICATE_COMPILER.pointer.json");
        var pointer = new Dictionary<string, string>
        {
            ["activeBuildLine"] = lineRoot,
            ["retainedParentBuildLine"] = Path.GetTempPath(),
            ["activePosture"] = Path.Combine(labContextRoot, "LAB_POSTURE_ACTIVE.md")
        };

        File.WriteAllText(pointerPath, JsonSerializer.Serialize(pointer));
        return pointerPath;
    }

    private static void AssertNoActivationOutputs(ProductBodyPreflightStatus status)
    {
        Assert.False(status.ModelBindingAllowed);
        Assert.False(status.LispEvaluationAllowed);
        Assert.False(status.RuntimeIdentityAllowed);
        Assert.False(status.RuntimeActionAllowed);
        Assert.False(status.DatabaseWriteAllowed);
        Assert.False(status.GelPromotionAllowed);
        Assert.False(status.CmeActualAllowed);
        Assert.False(status.SanctuaryActualAllowed);
    }
}
