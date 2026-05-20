using Xunit;

namespace San.Audit.Tests;

public sealed class ReadmeBoundaryPostureTests
{
    [Fact]
    public void Readme_Distinguishes_Sli_Lisp_Code_Body_From_External_Lisp_Evaluation()
    {
        var readmePath = Path.Combine(FindLineRoot(), "README.md");
        var readme = File.ReadAllText(readmePath);

        Assert.Contains("SLI.Lisp Is Code Body", readme, StringComparison.Ordinal);
        Assert.Contains("SLI.Lisp is part of the tool body's code membrane", readme, StringComparison.Ordinal);
        Assert.Contains("live-loads the", readme, StringComparison.Ordinal);
        Assert.Contains("resident SLI.Lisp membrane through SBCL", readme, StringComparison.Ordinal);
        Assert.Contains("Live load means the Lisp body is read by a real Common Lisp runtime", readme, StringComparison.Ordinal);
        Assert.Contains("arbitrary Lisp evaluation is open", readme, StringComparison.Ordinal);
        Assert.Contains("Arbitrary eval, model binding, action, GEL promotion, GEL", readme, StringComparison.Ordinal);
        Assert.Contains("admission, engram admission, SelfGEL mutation, and activation remain refused", readme, StringComparison.Ordinal);
    }

    private static string FindLineRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);

        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "San.sln")) &&
                File.Exists(Path.Combine(current.FullName, "README.md")) &&
                Directory.Exists(Path.Combine(current.FullName, "src", "SLI", "SLI.Lisp")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new InvalidOperationException("Unable to locate Project Bicycle line root from test output path.");
    }
}
