namespace SLI.Lisp;

public enum SliLispRuntimeLoadDisposition
{
    LoadedCold = 0,
    Withheld = 1,
    Refused = 2
}

public sealed record SliLispRuntimeLoadRequest(
    string? RuntimePath = null,
    bool ArbitraryEvaluationRequested = false,
    bool RuntimeActionRequested = false,
    bool ActivationRequested = false,
    TimeSpan? Timeout = null)
{
    public bool RequestsForbiddenMotion =>
        ArbitraryEvaluationRequested ||
        RuntimeActionRequested ||
        ActivationRequested;
}

public sealed record SliLispRuntimeLoadReceipt(
    string ReceiptHandle,
    SliLispRuntimeLoadDisposition Disposition,
    string OutcomeCode,
    string RuntimeKind,
    string RuntimePath,
    IReadOnlyList<string> ModuleNames,
    int ModuleCount,
    bool LoadedFromEmbeddedResources,
    bool LoadAttempted,
    bool LoadSucceeded,
    bool ResidentModuleLoadAllowed,
    bool TopLevelLoadEvaluationExpected,
    bool ArbitraryEvaluationAllowed,
    bool RuntimeActionAllowed,
    bool ActivationAllowed,
    bool AuthorityGranted,
    bool ModelBindingAllowed,
    bool GelPromotionAllowed,
    bool CmeActualAllowed,
    bool SanctuaryActualAllowed,
    int? ExitCode,
    string StandardOutput,
    string StandardError,
    DateTimeOffset TimestampUtc);
