using SLI.Lisp;
using Xunit;

namespace San.Audit.Tests;

public sealed class SliLispRuntimeLoadServiceTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-19T00:00:00Z");

    [Fact]
    public void LoadResidentMembrane_LiveLoads_Embedded_Sli_Lisp_Body_Through_Sbcl()
    {
        var receipt = new DefaultSliLispRuntimeLoadService().LoadResidentMembrane(
            new SliLispRuntimeLoadRequest(),
            TimestampUtc);

        Assert.Equal(SliLispRuntimeLoadDisposition.LoadedCold, receipt.Disposition);
        Assert.Equal("sli-lisp-resident-membrane-loaded-cold", receipt.OutcomeCode);
        Assert.Equal("SBCL", receipt.RuntimeKind);
        Assert.Equal(43, receipt.ModuleCount);
        Assert.Contains("core.lisp", receipt.ModuleNames);
        Assert.Contains("peer-review-predicate-bridge.lisp", receipt.ModuleNames);
        Assert.True(receipt.LoadedFromEmbeddedResources);
        Assert.True(receipt.LoadAttempted);
        Assert.True(receipt.LoadSucceeded);
        Assert.True(receipt.ResidentModuleLoadAllowed);
        Assert.True(receipt.TopLevelLoadEvaluationExpected);
        Assert.Contains("SAN-SLI-LISP-RUNTIME-LOAD-BEGIN", receipt.StandardOutput, StringComparison.Ordinal);
        Assert.Contains("SAN-SLI-LISP-RUNTIME-LOAD-OK module-count=43", receipt.StandardOutput, StringComparison.Ordinal);
        Assert.False(receipt.ArbitraryEvaluationAllowed);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.ActivationAllowed);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ModelBindingAllowed);
        Assert.False(receipt.GelPromotionAllowed);
        Assert.False(receipt.CmeActualAllowed);
        Assert.False(receipt.SanctuaryActualAllowed);
    }

    [Fact]
    public void LoadResidentMembrane_Refuses_Arbitrary_Eval_Action_And_Activation()
    {
        var receipt = new DefaultSliLispRuntimeLoadService().LoadResidentMembrane(
            new SliLispRuntimeLoadRequest(
                ArbitraryEvaluationRequested: true,
                RuntimeActionRequested: true,
                ActivationRequested: true),
            TimestampUtc);

        Assert.Equal(SliLispRuntimeLoadDisposition.Refused, receipt.Disposition);
        Assert.Equal("sli-lisp-runtime-motion-refused", receipt.OutcomeCode);
        Assert.False(receipt.LoadAttempted);
        Assert.False(receipt.LoadSucceeded);
        Assert.False(receipt.ResidentModuleLoadAllowed);
        Assert.False(receipt.TopLevelLoadEvaluationExpected);
        Assert.False(receipt.ArbitraryEvaluationAllowed);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.ActivationAllowed);
        Assert.False(receipt.AuthorityGranted);
        Assert.Contains("arbitrary eval, action, and activation remain refused", receipt.StandardError, StringComparison.Ordinal);
    }
}
