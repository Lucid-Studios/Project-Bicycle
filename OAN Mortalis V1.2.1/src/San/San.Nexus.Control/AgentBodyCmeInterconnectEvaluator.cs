using System.Security.Cryptography;
using System.Text;
using San.Common;
using SLI.Runtime;

namespace San.Nexus.Control;

public interface IAgentBodyCmeInterconnectEvaluator
{
    AgentBodyCmeInterconnectReceipt Evaluate(
        SliCmeActualRoundtripScaffoldResult scaffoldResult,
        IReadOnlyDictionary<string, string> lispModules,
        DateTimeOffset timestampUtc);
}

public sealed class DefaultAgentBodyCmeInterconnectEvaluator : IAgentBodyCmeInterconnectEvaluator
{
    public const string AgentBodyModuleName = "agent-body-cme.lisp";
    public const string PrimeReviewConduitPosture = ":prime-review-conduit :cgoa-insulated";
    public const string CrypticReviewConduitPosture = ":cryptic-review-conduit :telemetry-string-direct";
    public const string TelemetryAuthorityPosture = ":telemetry-authority :not-authority";
    public const string PrimeConduitAuthorityPosture = ":prime-conduit-authority :not-authority";
    public const string CompassShellPosture = ":compass-shell :candidate-only";
    public const string RootingLawLineagePosture = ":rooting-law-lineage :id-chain-required";
    public const string LineagePermissionNilPosture = ":lineage-permission nil";
    public const string PetalCandidatesPosture = ":petal-candidates :skills-abilities-talents-review-only";
    public const string PetalClosureNilPosture = ":petal-closure-claimed nil";
    public const string PetalAuthorizationNilPosture = ":petal-authorization nil";
    public const string EngramAdmissionNilPosture = ":engram-admission nil";
    public const string ContinuityAdmissionNilPosture = ":continuity-admission nil";
    public const string AuthorityAdmissionNilPosture = ":authority-admission nil";
    public const string ActivationRequestedNilPosture = ":activation-requested nil";
    public const string LispEvaluationNilPosture = ":lisp-evaluation-requested nil";
    public const string ModelBindingNilPosture = ":model-binding-requested nil";
    public const string GelPromotionNilPosture = ":gel-promotion-requested nil";
    public const string CmeActualActivationNilPosture = ":cme-actual-activation-requested nil";
    public const string SanctuaryActualActivationNilPosture = ":sanctuary-actual-activation-requested nil";
    public const string ReturnReceiptOnlyPosture = ":return :receipt-only";

    private static readonly string[] RequiredAgentBodyPostures =
    [
        PrimeReviewConduitPosture,
        CrypticReviewConduitPosture,
        TelemetryAuthorityPosture,
        PrimeConduitAuthorityPosture,
        CompassShellPosture,
        RootingLawLineagePosture,
        LineagePermissionNilPosture,
        PetalCandidatesPosture,
        PetalClosureNilPosture,
        PetalAuthorizationNilPosture,
        EngramAdmissionNilPosture,
        ContinuityAdmissionNilPosture,
        AuthorityAdmissionNilPosture,
        ActivationRequestedNilPosture,
        LispEvaluationNilPosture,
        ModelBindingNilPosture,
        GelPromotionNilPosture,
        CmeActualActivationNilPosture,
        SanctuaryActualActivationNilPosture,
        ReturnReceiptOnlyPosture
    ];

    public AgentBodyCmeInterconnectReceipt Evaluate(
        SliCmeActualRoundtripScaffoldResult scaffoldResult,
        IReadOnlyDictionary<string, string> lispModules,
        DateTimeOffset timestampUtc)
    {
        ArgumentNullException.ThrowIfNull(scaffoldResult);

        if (HasForbiddenMotion(scaffoldResult))
        {
            return CreateReceipt(
                scaffoldResult,
                AgentBodyCmeInterconnectDisposition.Refused,
                "agent-body-activation-drift-blocked",
                "Agent Body CME interconnect refused because the scaffold contains runtime, activation, model-binding, GEL, or action drift.",
                timestampUtc);
        }

        if (!HasRequiredReceiptContinuity(scaffoldResult))
        {
            return CreateReceipt(
                scaffoldResult,
                AgentBodyCmeInterconnectDisposition.Refused,
                "agent-body-receipt-continuity-broken",
                "Agent Body CME interconnect refused because the product response did not preserve the ordered receipt-only passage.",
                timestampUtc);
        }

        if (lispModules is null ||
            !lispModules.TryGetValue(AgentBodyModuleName, out var source) ||
            string.IsNullOrWhiteSpace(source))
        {
            return CreateReceipt(
                scaffoldResult,
                AgentBodyCmeInterconnectDisposition.Withheld,
                "agent-body-cme-lisp-module-missing",
                "Agent Body CME interconnect withheld because the inert Agent Body Lisp carrier module is missing.",
                timestampUtc);
        }

        var missingPostures = RequiredAgentBodyPostures
            .Where(posture => !source.Contains(posture, StringComparison.Ordinal))
            .ToArray();

        if (missingPostures.Length > 0)
        {
            return CreateReceipt(
                scaffoldResult,
                AgentBodyCmeInterconnectDisposition.Withheld,
                "agent-body-cme-lisp-posture-missing",
                $"Agent Body CME interconnect withheld because required inert Lisp posture is missing: {string.Join(", ", missingPostures)}.",
                timestampUtc);
        }

        return CreateReceipt(
            scaffoldResult,
            AgentBodyCmeInterconnectDisposition.VerifiedCold,
            "agent-body-cme-cold-interconnect-verified",
            "Agent Body CME interconnect verified Prime cGoA insulation, Cryptic telemetry-string review, Steward-held CMEiD, Compass shell candidate-only posture, and cleaving review without activation.",
            timestampUtc);
    }

    private static bool HasForbiddenMotion(SliCmeActualRoundtripScaffoldResult scaffoldResult) =>
        scaffoldResult.EngramPacket.RawGelPromoted ||
        scaffoldResult.EngramPacket.RuntimeIdentityEmissionAllowed ||
        scaffoldResult.EngramPacket.AnchorContinuityReceipt.HasForbiddenActivation ||
        scaffoldResult.EngramPacket.NonActivationReceipt.HasPrematureActivation ||
        scaffoldResult.EngramPacket.ReceiptContinuityReceipt.HasForbiddenActivation ||
        scaffoldResult.AdmissionReceipt.NonActivationReceipt.HasPrematureActivation ||
        scaffoldResult.AdmissionReceipt.ReceiptContinuityReceipt.HasForbiddenActivation ||
        scaffoldResult.AdmissionReceipt.RuntimeIdentityEmissionAllowed ||
        scaffoldResult.CmeActualContract.NonActivationReceipt.HasPrematureActivation ||
        scaffoldResult.CmeActualContract.ReceiptContinuityReceipt.HasForbiddenActivation ||
        scaffoldResult.CmeActualContract.RuntimeIdentityEmitted ||
        scaffoldResult.TelemetryEvent.NonActivationReceipt.HasPrematureActivation ||
        scaffoldResult.TelemetryEvent.ReceiptContinuityReceipt.HasForbiddenActivation ||
        scaffoldResult.TelemetryEvent.RuntimeIdentityEmitted ||
        scaffoldResult.TelemetryEvent.RuntimeActionExecuted ||
        scaffoldResult.ProductResponse.Disposition != ProductEngramResponseDisposition.ReceiptOnly ||
        scaffoldResult.ProductResponse.NonActivationReceipt.HasPrematureActivation ||
        scaffoldResult.ProductResponse.ReceiptContinuityReceipt.HasForbiddenActivation ||
        scaffoldResult.ProductResponse.PublicationReady ||
        scaffoldResult.ProductResponse.RuntimeIdentityEmitted ||
        scaffoldResult.ProductResponse.RuntimeActionExecuted;

    private static bool HasRequiredReceiptContinuity(SliCmeActualRoundtripScaffoldResult scaffoldResult) =>
        scaffoldResult.AdmissionReceipt.ReceiptContinuityReceipt.ExtendsReceipt(scaffoldResult.EngramPacket.ReceiptContinuityReceipt) &&
        scaffoldResult.CmeActualContract.ReceiptContinuityReceipt.ExtendsReceipt(scaffoldResult.AdmissionReceipt.ReceiptContinuityReceipt) &&
        scaffoldResult.TelemetryEvent.ReceiptContinuityReceipt.ExtendsReceipt(scaffoldResult.CmeActualContract.ReceiptContinuityReceipt) &&
        scaffoldResult.ProductResponse.ReceiptContinuityReceipt.ExtendsReceipt(scaffoldResult.TelemetryEvent.ReceiptContinuityReceipt) &&
        scaffoldResult.ProductResponse.ReceiptContinuityReceipt.ContainsPassageRef("product-engram-response", scaffoldResult.ProductResponse.ResponseHandle);

    private static AgentBodyCmeInterconnectReceipt CreateReceipt(
        SliCmeActualRoundtripScaffoldResult scaffoldResult,
        AgentBodyCmeInterconnectDisposition disposition,
        string outcomeCode,
        string governanceTrace,
        DateTimeOffset timestampUtc)
    {
        var seed = ShortHash(
            scaffoldResult.ProductResponse.ResponseHandle,
            scaffoldResult.CmeActualContract.ContractHandle,
            outcomeCode);
        var agentBodyCmeId = $"urn:san:agent-body-cmeid:{seed}";
        var stewardHolderRef = $"urn:san:steward-holder:{seed}";
        var telemetryStringRef = $"urn:san:telemetry-string:{seed}";
        var cgoaBundleRef = $"urn:san:cgoa-bundle:{seed}";
        var compassShellHandle = $"urn:san:compass-shell-candidate:{seed}";
        var cleavingReceiptHandle = $"urn:san:cleaving-discernment:{seed}";

        var prime = new PrimeCarrierSet(
            SoulFrameCmeId: $"urn:san:soul-frame:{seed}",
            OeCmeId: $"urn:san:oe:{seed}",
            SelfGelCmeId: $"urn:san:selfgel:{seed}",
            CgoaBundleCmeId: cgoaBundleRef,
            SlmSeedCmeId: $"urn:san:slm-seed:{seed}");

        var cryptic = new CrypticCarrierSet(
            AgentiCoreCmeId: $"urn:san:agenti-core:{seed}",
            COeCmeId: $"urn:san:coe:{seed}",
            CSelfGelCmeId: $"urn:san:cselfgel:{seed}",
            LispMembraneCmeId: $"urn:san:lisp-membrane:{seed}",
            TelemetryStringRef: telemetryStringRef);

        var steward = new StewardRegulationField(
            CSharpHostBoundaryRef: "urn:san:csharp-host-boundary:v121-sanctuary-cold",
            ZedDeltaRef: $"urn:san:zed-delta:{seed}",
            SituationalAwarenessRef: $"urn:san:situational-awareness:{seed}",
            TelemetryStringRef: telemetryStringRef,
            SliLispInnerChamberRef: $"urn:san:sli-lisp-inner-chamber:{seed}");

        var primeConduit = new CgoaInsulatedPrimeConduit(
            ConduitHandle: $"urn:san:prime-review-conduit:{seed}",
            ConduitKind: AgentBodyReviewConduitKind.CgoaInsulatedPrime,
            CgoaBundleRef: cgoaBundleRef,
            StewardHolderRef: stewardHolderRef,
            InsulatesPrimeActual: true,
            GrantsAuthority: false,
            GrantsIdentity: false);

        var crypticConduit = new TelemetryStringCrypticConduit(
            ConduitHandle: $"urn:san:cryptic-review-conduit:{seed}",
            ConduitKind: AgentBodyReviewConduitKind.TelemetryStringCryptic,
            TelemetryStringRef: telemetryStringRef,
            StewardHolderRef: stewardHolderRef,
            DirectCrypticActualReach: true,
            GrantsAuthority: false,
            SelfAuthorizes: false);

        var compassShell = new CompassShellCandidate(
            ShellHandle: compassShellHandle,
            ListeningFrameRef: $"urn:san:listening-frame:{seed}",
            CompassRef: $"urn:san:compass:{seed}",
            PredicatePressureRefs:
            [
                scaffoldResult.EngramPacket.PacketHandle,
                scaffoldResult.AdmissionReceipt.ReceiptHandle,
                scaffoldResult.TelemetryEvent.EventHandle
            ],
            Status: disposition == AgentBodyCmeInterconnectDisposition.Refused
                ? CompassShellCandidateStatus.Refused
                : disposition == AgentBodyCmeInterconnectDisposition.Withheld
                    ? CompassShellCandidateStatus.Withheld
                    : CompassShellCandidateStatus.CandidateOnly,
            IsEngram: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false);

        var cleavingDiscernment = new CleavingDiscernmentReceipt(
            ReceiptHandle: cleavingReceiptHandle,
            CompassShellHandle: compassShellHandle,
            Disposition: disposition == AgentBodyCmeInterconnectDisposition.Refused
                ? CleavingDiscernmentDisposition.Refused
                : disposition == AgentBodyCmeInterconnectDisposition.Withheld
                    ? CleavingDiscernmentDisposition.Withheld
                    : CleavingDiscernmentDisposition.CandidateOnly,
            OutcomeCode: outcomeCode,
            ReviewPathRefs:
            [
                primeConduit.ConduitHandle,
                crypticConduit.ConduitHandle,
                steward.SliLispInnerChamberRef
            ],
            EcStartRequested: false,
            RuntimeActionRequested: false,
            ContinuityAdmitted: false,
            AuthorityGranted: false);

        return new AgentBodyCmeInterconnectReceipt(
            ReceiptHandle: $"urn:san:agent-body-cme-interconnect:{seed}",
            Disposition: disposition,
            OutcomeCode: outcomeCode,
            GovernanceTrace: governanceTrace,
            AgentBodyCmeId: agentBodyCmeId,
            Prime: prime,
            Cryptic: cryptic,
            Steward: steward,
            PrimeReviewConduit: primeConduit,
            CrypticReviewConduit: crypticConduit,
            CompassShell: compassShell,
            CleavingDiscernment: cleavingDiscernment,
            SliRoundtripResponseHandle: scaffoldResult.ProductResponse.ResponseHandle,
            ReceiptRefs: scaffoldResult.ProductResponse.ReceiptRefs
                .Concat(
                [
                    primeConduit.ConduitHandle,
                    crypticConduit.ConduitHandle,
                    compassShellHandle,
                    cleavingReceiptHandle
                ])
                .ToArray(),
            RuntimeIdentityEmitted: false,
            RuntimeActionExecuted: false,
            ModelBindingRequested: false,
            LispEvaluationRequested: false,
            GelPromotionAllowed: false,
            CmeActualActivated: false,
            SanctuaryActualActivated: false,
            TimestampUtc: timestampUtc);
    }

    private static string ShortHash(params string[] values)
    {
        var material = string.Join("|", values);
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(material));
        return Convert.ToHexString(hash).ToLowerInvariant()[..16];
    }
}
