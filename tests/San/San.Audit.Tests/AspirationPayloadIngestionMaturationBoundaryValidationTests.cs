using San.Common;
using Xunit;

namespace San.Audit.Tests;

public sealed class AspirationPayloadIngestionMaturationBoundaryValidationTests
{
    private static readonly DateTimeOffset TimestampUtc = DateTimeOffset.Parse("2026-05-16T00:00:00Z");

    [Fact]
    public void Aspiration_Payload_Loads_Ingests_Articulates_And_Matures_As_Review_Only()
    {
        var receipt = Mature(CreateRequest());

        Assert.Equal(AspirationPayloadIngestionMaturationDisposition.MaturedForReviewCold, receipt.Disposition);
        Assert.Equal("aspiration-payload-matured-for-review-cold", receipt.OutcomeCode);
        Assert.True(receipt.PayloadLoadedAsColdEvidence);
        Assert.Equal(8, receipt.Statements.Count);
        Assert.Equal(8, receipt.IngestionLanes.Count);
        Assert.Equal(8, receipt.MaturationCandidates.Count);
        AssertCold(receipt);
    }

    [Fact]
    public void Empty_Aspiration_Payload_Is_Reviewable_But_Not_Authoritative()
    {
        var receipt = Mature(CreateRequest(statements: [], lanes: [], candidates: []));

        Assert.Equal(AspirationPayloadIngestionMaturationDisposition.EmptyReviewCold, receipt.Disposition);
        Assert.Equal("aspiration-payload-empty-review-only", receipt.OutcomeCode);
        Assert.Empty(receipt.Statements);
        Assert.Empty(receipt.IngestionLanes);
        Assert.Empty(receipt.MaturationCandidates);
        Assert.False(receipt.PayloadLoadedAsColdEvidence);
        Assert.False(receipt.AuthorityGranted);
        Assert.False(receipt.ContinuityAdmitted);
        AssertCold(receipt);
    }

    [Fact]
    public void Aspiration_Payload_Does_Not_Increment_Passage_Or_Create_Authority()
    {
        var receipt = Mature(CreateRequest(priorPassageCount: 512));

        Assert.Equal(512, receipt.PriorPassageCount);
        Assert.Equal(512, receipt.PassageCountAfterMaturation);
        Assert.False(receipt.PayloadBecameWarrant);
        Assert.False(receipt.PayloadDensityBecameTruth);
        Assert.False(receipt.IngestionBecameAdmission);
        Assert.False(receipt.ArticulationBecameAuthority);
        Assert.False(receipt.MaturationAdmittedContinuity);
        Assert.False(receipt.CandidateAuthorizedAction);
        Assert.False(receipt.IdentityMutated);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
        AssertCold(receipt);
    }

    [Theory]
    [InlineData("missing-boundary")]
    [InlineData("not-review")]
    [InlineData("no-load")]
    [InlineData("no-ingestion")]
    [InlineData("no-articulation")]
    [InlineData("no-maturation")]
    [InlineData("no-lanes")]
    [InlineData("no-evidence")]
    [InlineData("no-witness")]
    [InlineData("no-cooling")]
    [InlineData("no-return")]
    [InlineData("no-steward")]
    [InlineData("payload-warrant")]
    [InlineData("density-truth")]
    [InlineData("ingestion-admission")]
    [InlineData("articulation-authority")]
    [InlineData("maturation-continuity")]
    [InlineData("runtime")]
    [InlineData("identity")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Boundary_Refuses_Promotional_Aspiration_Collapse(string mutation)
    {
        var receipt = Mature(CreateRequest(boundary: MutateBoundary(CreateBoundary(), mutation)));

        var expected = mutation == "missing-boundary"
            ? "aspiration-payload-boundary-missing"
            : "aspiration-payload-promotional-boundary";
        AssertRefused(receipt, expected);
    }

    [Theory]
    [InlineData("aspiration-warrant")]
    [InlineData("density-truth")]
    [InlineData("ingestion-admission")]
    [InlineData("articulation-authority")]
    [InlineData("maturation-continuity")]
    [InlineData("action")]
    [InlineData("lisp")]
    [InlineData("packet")]
    [InlineData("replay")]
    [InlineData("passage")]
    [InlineData("activation")]
    public void Non_Promotion_Boundary_Refuses_Payload_As_Warrant(string mutation)
    {
        var receipt = Mature(CreateRequest(nonPromotion: MutateNonPromotion(CreateNonPromotion(), mutation)));

        AssertRefused(receipt, "aspiration-payload-non-promotion-boundary-invalid");
    }

    [Fact]
    public void Aspiration_Payload_Refuses_Duplicate_Statement_Handles()
    {
        var validStatements = CreateStatements();
        var validLanes = CreateLanes(validStatements);
        var statements = validStatements.ToArray();
        statements[1] = statements[1] with { StatementHandle = statements[0].StatementHandle };

        var receipt = Mature(CreateRequest(
            statements: statements,
            lanes: validLanes,
            candidates: CreateCandidates(validStatements, validLanes)));

        AssertRefused(receipt, "aspiration-payload-duplicate-statement-handle");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("missing-source")]
    [InlineData("missing-surface")]
    [InlineData("missing-text")]
    [InlineData("missing-evidence")]
    [InlineData("missing-witness")]
    [InlineData("missing-cooling")]
    [InlineData("missing-return")]
    [InlineData("not-review")]
    [InlineData("no-evidence-body")]
    [InlineData("no-witness-body")]
    [InlineData("no-cooling")]
    [InlineData("no-return")]
    [InlineData("no-ingestion")]
    [InlineData("no-articulation")]
    [InlineData("no-maturation")]
    [InlineData("aspiration-warrant")]
    [InlineData("density-truth")]
    [InlineData("ingestion-admission")]
    [InlineData("articulation-authority")]
    [InlineData("maturation-continuity")]
    [InlineData("action")]
    [InlineData("identity")]
    [InlineData("lisp")]
    public void Statement_Remains_Cold_Aspiration_Not_Warrant(string mutation)
    {
        var validStatements = CreateStatements();
        var validLanes = CreateLanes(validStatements);
        var statements = validStatements.ToArray();
        statements[0] = MutateStatement(statements[0], mutation);

        var receipt = Mature(CreateRequest(
            statements: statements,
            lanes: validLanes,
            candidates: CreateCandidates(validStatements, validLanes)));

        AssertRefused(receipt, "aspiration-payload-statement-invalid");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("missing-source")]
    [InlineData("missing-target")]
    [InlineData("missing-class")]
    [InlineData("not-review")]
    [InlineData("not-ingested")]
    [InlineData("no-lineage")]
    [InlineData("no-evidence")]
    [InlineData("no-witness")]
    [InlineData("no-cooling")]
    [InlineData("no-return")]
    [InlineData("admission")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("action")]
    [InlineData("lisp")]
    public void Ingestion_Lane_Remains_Typed_Review_Only(string mutation)
    {
        var statements = CreateStatements();
        var validLanes = CreateLanes(statements);
        var lanes = validLanes.ToArray();
        lanes[0] = MutateLane(lanes[0], mutation);

        var receipt = Mature(CreateRequest(
            statements: statements,
            lanes: lanes,
            candidates: CreateCandidates(statements, validLanes)));

        AssertRefused(receipt, "aspiration-payload-ingestion-lane-invalid");
    }

    [Fact]
    public void Ingestion_Lane_Must_Bind_To_Known_Statement()
    {
        var statements = CreateStatements();
        var validLanes = CreateLanes(statements);
        var lanes = validLanes.ToArray();
        lanes[0] = lanes[0] with { SourceStatementHandle = "urn:san:aspiration-payload:statement:missing" };

        var receipt = Mature(CreateRequest(
            statements: statements,
            lanes: lanes,
            candidates: CreateCandidates(statements, validLanes)));

        AssertRefused(receipt, "aspiration-payload-ingestion-lane-unbound");
    }

    [Fact]
    public void Ingestion_Lane_Refuses_Duplicate_Lane_Handles()
    {
        var statements = CreateStatements();
        var validLanes = CreateLanes(statements);
        var lanes = validLanes.ToArray();
        lanes[1] = lanes[1] with { LaneHandle = lanes[0].LaneHandle };

        var receipt = Mature(CreateRequest(
            statements: statements,
            lanes: lanes,
            candidates: CreateCandidates(statements, validLanes)));

        AssertRefused(receipt, "aspiration-payload-duplicate-lane-handle");
    }

    [Theory]
    [InlineData("missing-handle")]
    [InlineData("missing-source")]
    [InlineData("missing-lane")]
    [InlineData("missing-form")]
    [InlineData("missing-posture")]
    [InlineData("not-review")]
    [InlineData("not-articulated")]
    [InlineData("not-matured")]
    [InlineData("not-candidate")]
    [InlineData("no-lineage")]
    [InlineData("no-steward")]
    [InlineData("no-return")]
    [InlineData("authority")]
    [InlineData("continuity")]
    [InlineData("warrant")]
    [InlineData("action")]
    [InlineData("lisp")]
    [InlineData("activation")]
    public void Maturation_Candidate_Remains_Candidate_Only(string mutation)
    {
        var statements = CreateStatements();
        var lanes = CreateLanes(statements);
        var candidates = CreateCandidates(statements, lanes);
        candidates[0] = MutateCandidate(candidates[0], mutation);

        var receipt = Mature(CreateRequest(statements: statements, lanes: lanes, candidates: candidates));

        AssertRefused(receipt, "aspiration-payload-maturation-candidate-invalid");
    }

    [Fact]
    public void Maturation_Candidate_Must_Bind_To_Known_Statement_And_Lane()
    {
        var statements = CreateStatements();
        var lanes = CreateLanes(statements);
        var candidates = CreateCandidates(statements, lanes);
        candidates[0] = candidates[0] with { SourceStatementHandle = "urn:san:aspiration-payload:statement:missing" };

        var receipt = Mature(CreateRequest(statements: statements, lanes: lanes, candidates: candidates));

        AssertRefused(receipt, "aspiration-payload-maturation-candidate-unbound");
    }

    [Fact]
    public void Maturation_Candidate_May_Not_Cross_Bind_Statement_Through_Another_Lane()
    {
        var statements = CreateStatements();
        var lanes = CreateLanes(statements);
        var candidates = CreateCandidates(statements, lanes);
        candidates[0] = candidates[0] with { LaneHandle = lanes[1].LaneHandle };

        var receipt = Mature(CreateRequest(statements: statements, lanes: lanes, candidates: candidates));

        AssertRefused(receipt, "aspiration-payload-maturation-candidate-misaligned");
    }

    [Fact]
    public void Non_Empty_Aspiration_Statement_Must_Enter_A_Typed_Lane()
    {
        var statements = CreateStatements();
        var lanes = CreateLanes(statements)
            .Where(lane => lane.SourceStatementHandle != statements[0].StatementHandle)
            .ToArray();

        var receipt = Mature(CreateRequest(
            statements: statements,
            lanes: lanes,
            candidates: CreateCandidates(statements.Skip(1).ToArray(), lanes)));

        AssertRefused(receipt, "aspiration-payload-statement-unlaned");
    }

    [Fact]
    public void Non_Empty_Ingestion_Lane_Must_Resolve_To_Maturation_Candidate()
    {
        var statements = CreateStatements();
        var lanes = CreateLanes(statements);
        var candidates = CreateCandidates(statements, lanes)
            .Where(candidate => candidate.LaneHandle != lanes[0].LaneHandle)
            .ToArray();

        var receipt = Mature(CreateRequest(statements: statements, lanes: lanes, candidates: candidates));

        AssertRefused(receipt, "aspiration-payload-lane-without-candidate");
    }

    [Fact]
    public void Lisp_Body_Carries_Aspiration_Payload_As_Inert_Maturation_Register()
    {
        var lispPath = Path.Combine(
            FindRepositoryRoot(),
            "src",
            "SLI",
            "SLI.Lisp",
            "aspiration-payload-ingestion-maturation.lisp");

        var body = File.ReadAllText(lispPath);

        Assert.Contains(":posture :cme-aspiration-payload-ingestion-maturation-boundary", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-role :inert-aspiration-payload-carrier", body, StringComparison.Ordinal);
        Assert.Contains(":process-chain (:load :ingest :articulate :mature :return-for-review)", body, StringComparison.Ordinal);
        Assert.Contains(":aspiration-payload-not-warrant", body, StringComparison.Ordinal);
        Assert.Contains(":payload-density-not-truth", body, StringComparison.Ordinal);
        Assert.Contains(":ingestion-not-admission", body, StringComparison.Ordinal);
        Assert.Contains(":articulation-not-authority", body, StringComparison.Ordinal);
        Assert.Contains(":maturation-not-continuity", body, StringComparison.Ordinal);
        Assert.Contains(":payload-may-authorize nil", body, StringComparison.Ordinal);
        Assert.Contains(":lisp-evaluation-allowed nil", body, StringComparison.Ordinal);
        Assert.Contains(":return :receipt-only", body, StringComparison.Ordinal);
        Assert.DoesNotContain("(eval", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(compile", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("(load", body, StringComparison.OrdinalIgnoreCase);
    }

    private static AspirationPayloadIngestionMaturationReceipt Mature(
        AspirationPayloadIngestionMaturationRequest request) =>
        new DefaultAspirationPayloadIngestionMaturationBoundaryValidator().Mature(request, TimestampUtc);

    private static AspirationPayloadIngestionMaturationRequest CreateRequest(
        IReadOnlyList<AspirationPayloadStatement>? statements = null,
        IReadOnlyList<AspirationPayloadIngestionLane>? lanes = null,
        IReadOnlyList<AspirationMaturationCandidate>? candidates = null,
        AspirationPayloadIngestionMaturationBoundary? boundary = null,
        AspirationPayloadNonPromotionBoundary? nonPromotion = null,
        int priorPassageCount = 188) 
    {
        var statementSet = statements ?? CreateStatements();
        var laneSet = lanes ?? CreateLanes(statementSet);
        return new(
            Statements: statementSet,
            IngestionLanes: laneSet,
            MaturationCandidates: candidates ?? CreateCandidates(statementSet, laneSet),
            Boundary: boundary ?? CreateBoundary(),
            NonPromotionBoundary: nonPromotion ?? CreateNonPromotion(),
            PriorPassageCount: priorPassageCount);
    }

    private static AspirationPayloadStatement[] CreateStatements() =>
    [
        Statement(AspirationPayloadLaneKind.PrimeBody, "Prime remains body-side invariant posture."),
        Statement(AspirationPayloadLaneKind.CrypticMind, "Cryptic retains unresolved pressure without seizing body authority."),
        Statement(AspirationPayloadLaneKind.StewardWitness, "Steward witnesses custody and interlock without owning meaning."),
        Statement(AspirationPayloadLaneKind.SliLisp, "SLI.Lisp carries symbolic posture without evaluation."),
        Statement(AspirationPayloadLaneKind.EngineeredCognition, "Engineered cognition may form meaning shells without continuity admission."),
        Statement(AspirationPayloadLaneKind.Pedagogy, "Pedagogy reconstructs participation without coronation."),
        Statement(AspirationPayloadLaneKind.Telemetry, "Telemetry may measure pressure without becoming authority."),
        Statement(AspirationPayloadLaneKind.OperatorIntent, "Operator intent may orient review without bypassing Steward law.")
    ];

    private static AspirationPayloadStatement Statement(
        AspirationPayloadLaneKind laneKind,
        string text)
    {
        var suffix = Slug(laneKind);
        return new(
            StatementHandle: $"urn:san:aspiration-payload:statement:{suffix}",
            SourceWaveCascadeHandle: "urn:san:wave-cascade:review:full-stack",
            LaneKind: laneKind,
            SourceSurface: $"urn:san:surface:{suffix}",
            StatementText: text,
            EvidenceHandle: $"urn:san:evidence:aspiration-payload:{suffix}",
            WitnessHandle: $"urn:san:witness:aspiration-payload:{suffix}",
            CoolingPathHandle: $"urn:san:cooling:aspiration-payload:{suffix}",
            ReturnPathHandle: $"urn:san:return:aspiration-payload:{suffix}",
            ReviewOnly: true,
            EvidenceBodyPresent: true,
            WitnessBodyPresent: true,
            CoolingPathPresent: true,
            ReturnPathPresent: true,
            IngestionAllowed: true,
            ArticulationAllowed: true,
            MaturationAllowed: true,
            TreatsAspirationAsWarrant: false,
            TreatsPayloadDensityAsTruth: false,
            TreatsIngestionAsAdmission: false,
            TreatsArticulationAsAuthority: false,
            TreatsMaturationAsContinuity: false,
            AuthorizesAction: false,
            MutatesIdentity: false,
            EvaluatesLisp: false);
    }

    private static AspirationPayloadIngestionLane[] CreateLanes(
        IReadOnlyList<AspirationPayloadStatement> statements) =>
        statements
            .Select(statement =>
            {
                var suffix = Slug(statement.LaneKind);
                return new AspirationPayloadIngestionLane(
                    LaneHandle: $"urn:san:aspiration-payload:lane:{suffix}",
                    SourceStatementHandle: statement.StatementHandle,
                    LaneKind: statement.LaneKind,
                    TargetBodySurface: $"urn:san:target-body-surface:{suffix}",
                    PayloadClass: "aspiration-review-only",
                    ReviewOnly: true,
                    IngestedForReview: true,
                    PreservesSourceLineage: true,
                    RequiresEvidence: true,
                    RequiresWitness: true,
                    RequiresCooling: true,
                    RequiresReturnPath: true,
                    AllowsAdmission: false,
                    AllowsAuthority: false,
                    AllowsContinuity: false,
                    AllowsAction: false,
                    AllowsLispEvaluation: false);
            })
            .ToArray();

    private static AspirationMaturationCandidate[] CreateCandidates(
        IReadOnlyList<AspirationPayloadStatement> statements,
        IReadOnlyList<AspirationPayloadIngestionLane> lanes) =>
        lanes
            .Select(lane =>
            {
                var statement = statements.First(candidateStatement =>
                    string.Equals(candidateStatement.StatementHandle, lane.SourceStatementHandle, StringComparison.Ordinal));
                var suffix = Slug(statement.LaneKind);
                return new AspirationMaturationCandidate(
                    CandidateHandle: $"urn:san:aspiration-payload:candidate:{suffix}",
                    SourceStatementHandle: statement.StatementHandle,
                    LaneHandle: lane.LaneHandle,
                    ArticulatedForm: $"review-candidate:{suffix}",
                    MaturationPosture: "candidate-only-under-steward-review",
                    ReviewOnly: true,
                    ArticulatedForReview: true,
                    MaturedAsCandidate: true,
                    CandidateOnly: true,
                    PreservesPayloadLineage: true,
                    RequiresStewardReview: true,
                    RequiresReturnPath: true,
                    ArticulationBecomesAuthority: false,
                    MaturationBecomesContinuity: false,
                    CandidateBecomesWarrant: false,
                    CandidateAuthorizesAction: false,
                    CandidateEvaluatesLisp: false,
                    CandidateActivates: false);
            })
            .ToArray();

    private static AspirationPayloadIngestionMaturationBoundary CreateBoundary(string? mutation = null) =>
        MutateBoundary(
            new AspirationPayloadIngestionMaturationBoundary(
                BoundaryCode: "aspiration-payload-ingestion-maturation-review-only",
                Present: true,
                ReviewOnly: true,
                AllowsPayloadLoad: true,
                AllowsIngestion: true,
                AllowsArticulation: true,
                AllowsMaturation: true,
                RequiresTypedLanes: true,
                RequiresEvidence: true,
                RequiresWitness: true,
                RequiresCooling: true,
                RequiresReturnPath: true,
                RequiresStewardReview: true,
                AllowsPayloadAsWarrant: false,
                AllowsPayloadDensityAsTruth: false,
                AllowsIngestionAsAdmission: false,
                AllowsArticulationAsAuthority: false,
                AllowsMaturationAsContinuity: false,
                AllowsRuntimeAction: false,
                AllowsIdentityMutation: false,
                AllowsLispEvaluation: false,
                AllowsPacketEmission: false,
                AllowsReceiptReplay: false,
                IncrementsPassageCount: false,
                AllowsActivation: false),
            mutation);

    private static AspirationPayloadNonPromotionBoundary CreateNonPromotion() =>
        new(
            BoundaryLaw: "Aspirations may be loaded, ingested, articulated, and matured as candidates; none may become warrant, truth, admission, authority, continuity, action, Lisp evaluation, packet emission, replay, passage, or activation.",
            AspirationMayBecomeWarrant: false,
            PayloadDensityMayBecomeTruth: false,
            IngestionMayBecomeAdmission: false,
            ArticulationMayBecomeAuthority: false,
            MaturationMayAdmitContinuity: false,
            CandidateMayAuthorizeAction: false,
            CandidateMayEvaluateLisp: false,
            CandidateMayEmitPacket: false,
            CandidateMayReplayReceipts: false,
            CandidateMayIncrementPassage: false,
            CandidateMayActivate: false);

    private static AspirationPayloadIngestionMaturationBoundary MutateBoundary(
        AspirationPayloadIngestionMaturationBoundary boundary,
        string? mutation) =>
        mutation switch
        {
            null => boundary,
            "missing-boundary" => boundary with { BoundaryCode = string.Empty, Present = false },
            "not-review" => boundary with { ReviewOnly = false },
            "no-load" => boundary with { AllowsPayloadLoad = false },
            "no-ingestion" => boundary with { AllowsIngestion = false },
            "no-articulation" => boundary with { AllowsArticulation = false },
            "no-maturation" => boundary with { AllowsMaturation = false },
            "no-lanes" => boundary with { RequiresTypedLanes = false },
            "no-evidence" => boundary with { RequiresEvidence = false },
            "no-witness" => boundary with { RequiresWitness = false },
            "no-cooling" => boundary with { RequiresCooling = false },
            "no-return" => boundary with { RequiresReturnPath = false },
            "no-steward" => boundary with { RequiresStewardReview = false },
            "payload-warrant" => boundary with { AllowsPayloadAsWarrant = true },
            "density-truth" => boundary with { AllowsPayloadDensityAsTruth = true },
            "ingestion-admission" => boundary with { AllowsIngestionAsAdmission = true },
            "articulation-authority" => boundary with { AllowsArticulationAsAuthority = true },
            "maturation-continuity" => boundary with { AllowsMaturationAsContinuity = true },
            "runtime" => boundary with { AllowsRuntimeAction = true },
            "identity" => boundary with { AllowsIdentityMutation = true },
            "lisp" => boundary with { AllowsLispEvaluation = true },
            "packet" => boundary with { AllowsPacketEmission = true },
            "replay" => boundary with { AllowsReceiptReplay = true },
            "passage" => boundary with { IncrementsPassageCount = true },
            "activation" => boundary with { AllowsActivation = true },
            _ => boundary
        };

    private static AspirationPayloadNonPromotionBoundary MutateNonPromotion(
        AspirationPayloadNonPromotionBoundary boundary,
        string mutation) =>
        mutation switch
        {
            "aspiration-warrant" => boundary with { AspirationMayBecomeWarrant = true },
            "density-truth" => boundary with { PayloadDensityMayBecomeTruth = true },
            "ingestion-admission" => boundary with { IngestionMayBecomeAdmission = true },
            "articulation-authority" => boundary with { ArticulationMayBecomeAuthority = true },
            "maturation-continuity" => boundary with { MaturationMayAdmitContinuity = true },
            "action" => boundary with { CandidateMayAuthorizeAction = true },
            "lisp" => boundary with { CandidateMayEvaluateLisp = true },
            "packet" => boundary with { CandidateMayEmitPacket = true },
            "replay" => boundary with { CandidateMayReplayReceipts = true },
            "passage" => boundary with { CandidateMayIncrementPassage = true },
            "activation" => boundary with { CandidateMayActivate = true },
            _ => boundary
        };

    private static AspirationPayloadStatement MutateStatement(
        AspirationPayloadStatement statement,
        string mutation) =>
        mutation switch
        {
            "missing-handle" => statement with { StatementHandle = string.Empty },
            "missing-source" => statement with { SourceWaveCascadeHandle = string.Empty },
            "missing-surface" => statement with { SourceSurface = string.Empty },
            "missing-text" => statement with { StatementText = string.Empty },
            "missing-evidence" => statement with { EvidenceHandle = string.Empty },
            "missing-witness" => statement with { WitnessHandle = string.Empty },
            "missing-cooling" => statement with { CoolingPathHandle = string.Empty, CoolingPathPresent = false },
            "missing-return" => statement with { ReturnPathHandle = string.Empty, ReturnPathPresent = false },
            "not-review" => statement with { ReviewOnly = false },
            "no-evidence-body" => statement with { EvidenceBodyPresent = false },
            "no-witness-body" => statement with { WitnessBodyPresent = false },
            "no-cooling" => statement with { CoolingPathPresent = false },
            "no-return" => statement with { ReturnPathPresent = false },
            "no-ingestion" => statement with { IngestionAllowed = false },
            "no-articulation" => statement with { ArticulationAllowed = false },
            "no-maturation" => statement with { MaturationAllowed = false },
            "aspiration-warrant" => statement with { TreatsAspirationAsWarrant = true },
            "density-truth" => statement with { TreatsPayloadDensityAsTruth = true },
            "ingestion-admission" => statement with { TreatsIngestionAsAdmission = true },
            "articulation-authority" => statement with { TreatsArticulationAsAuthority = true },
            "maturation-continuity" => statement with { TreatsMaturationAsContinuity = true },
            "action" => statement with { AuthorizesAction = true },
            "identity" => statement with { MutatesIdentity = true },
            "lisp" => statement with { EvaluatesLisp = true },
            _ => statement
        };

    private static AspirationPayloadIngestionLane MutateLane(
        AspirationPayloadIngestionLane lane,
        string mutation) =>
        mutation switch
        {
            "missing-handle" => lane with { LaneHandle = string.Empty },
            "missing-source" => lane with { SourceStatementHandle = string.Empty },
            "missing-target" => lane with { TargetBodySurface = string.Empty },
            "missing-class" => lane with { PayloadClass = string.Empty },
            "not-review" => lane with { ReviewOnly = false },
            "not-ingested" => lane with { IngestedForReview = false },
            "no-lineage" => lane with { PreservesSourceLineage = false },
            "no-evidence" => lane with { RequiresEvidence = false },
            "no-witness" => lane with { RequiresWitness = false },
            "no-cooling" => lane with { RequiresCooling = false },
            "no-return" => lane with { RequiresReturnPath = false },
            "admission" => lane with { AllowsAdmission = true },
            "authority" => lane with { AllowsAuthority = true },
            "continuity" => lane with { AllowsContinuity = true },
            "action" => lane with { AllowsAction = true },
            "lisp" => lane with { AllowsLispEvaluation = true },
            _ => lane
        };

    private static AspirationMaturationCandidate MutateCandidate(
        AspirationMaturationCandidate candidate,
        string mutation) =>
        mutation switch
        {
            "missing-handle" => candidate with { CandidateHandle = string.Empty },
            "missing-source" => candidate with { SourceStatementHandle = string.Empty },
            "missing-lane" => candidate with { LaneHandle = string.Empty },
            "missing-form" => candidate with { ArticulatedForm = string.Empty },
            "missing-posture" => candidate with { MaturationPosture = string.Empty },
            "not-review" => candidate with { ReviewOnly = false },
            "not-articulated" => candidate with { ArticulatedForReview = false },
            "not-matured" => candidate with { MaturedAsCandidate = false },
            "not-candidate" => candidate with { CandidateOnly = false },
            "no-lineage" => candidate with { PreservesPayloadLineage = false },
            "no-steward" => candidate with { RequiresStewardReview = false },
            "no-return" => candidate with { RequiresReturnPath = false },
            "authority" => candidate with { ArticulationBecomesAuthority = true },
            "continuity" => candidate with { MaturationBecomesContinuity = true },
            "warrant" => candidate with { CandidateBecomesWarrant = true },
            "action" => candidate with { CandidateAuthorizesAction = true },
            "lisp" => candidate with { CandidateEvaluatesLisp = true },
            "activation" => candidate with { CandidateActivates = true },
            _ => candidate
        };

    private static void AssertCold(AspirationPayloadIngestionMaturationReceipt receipt)
    {
        Assert.True(receipt.IsColdAspirationMaturation);
        Assert.Null(receipt.Refusal);
        Assert.True(receipt.ReviewOnly);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterMaturation);
        Assert.False(receipt.PayloadBecameWarrant);
        Assert.False(receipt.PayloadDensityBecameTruth);
        Assert.False(receipt.IngestionBecameAdmission);
        Assert.False(receipt.ArticulationBecameAuthority);
        Assert.False(receipt.MaturationAdmittedContinuity);
        Assert.False(receipt.CandidateAuthorizedAction);
        Assert.False(receipt.IdentityMutated);
        Assert.False(receipt.RuntimeActionAllowed);
        Assert.False(receipt.LispEvaluationAllowed);
        Assert.False(receipt.NewPacketEmitted);
        Assert.False(receipt.ReceiptsReplayed);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
    }

    private static void AssertRefused(
        AspirationPayloadIngestionMaturationReceipt receipt,
        string outcomeCode)
    {
        Assert.Equal(AspirationPayloadIngestionMaturationDisposition.Refused, receipt.Disposition);
        Assert.Equal(outcomeCode, receipt.OutcomeCode);
        Assert.True(receipt.IsRetainedAspirationRefusal);
        Assert.NotNull(receipt.Refusal);
        Assert.Empty(receipt.Statements);
        Assert.Empty(receipt.IngestionLanes);
        Assert.Empty(receipt.MaturationCandidates);
        Assert.Equal(receipt.PriorPassageCount, receipt.PassageCountAfterMaturation);
        Assert.False(receipt.CandidateAuthorizedAction);
        Assert.False(receipt.ContinuityAdmitted);
        Assert.False(receipt.AuthorityGranted);
        Assert.True(receipt.ActivationRefused);
    }

    private static string Slug(AspirationPayloadLaneKind laneKind) =>
        ToKebabCase(laneKind.ToString());

    private static string ToKebabCase(string value)
    {
        var builder = new System.Text.StringBuilder();
        foreach (var character in value)
        {
            if (char.IsUpper(character) && builder.Length > 0)
            {
                builder.Append('-');
            }

            builder.Append(char.ToLowerInvariant(character));
        }

        return builder.ToString();
    }

    private static string FindRepositoryRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null)
        {
            var candidate = Path.Combine(current.FullName, "src", "SLI", "SLI.Lisp");
            if (Directory.Exists(candidate))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new DirectoryNotFoundException("Unable to locate repository root.");
    }
}
