using AiEngineeringLab.Core.AI.Evaluation;

namespace AiEngineeringLab.UnitTests.AI.Evaluation;

public sealed class RagEvaluationDatasetLoaderTests
{
    [Fact]
    public async Task LoadAsync_ShouldLoadEvaluationCases()
    {
        var path = Path.GetTempFileName();

        try
        {
            var json = """
            [
              {
                "id": "test-001",
                "question": "What is BOS?",
                "expectedAnswer": "A framework.",
                "expectedDocumentIds": ["bos-overview"],
                "answerExpected": true
              }
            ]
            """;

            await File.WriteAllTextAsync(path, json);

            var sut = new RagEvaluationDatasetLoader();

            var dataset = await sut.LoadAsync(path);

            var evaluationCase = Assert.Single(dataset.Cases);

            Assert.Equal("test-001", evaluationCase.Id);
            Assert.Equal("What is BOS?", evaluationCase.Question);
            Assert.Equal("A framework.", evaluationCase.ExpectedAnswer);
            Assert.True(evaluationCase.AnswerExpected);
            Assert.Contains("bos-overview", evaluationCase.ExpectedDocumentIds);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public async Task LoadAsync_ShouldSupportCasesWithoutExpectedAnswer()
    {
        var path = Path.GetTempFileName();

        try
        {
            var json = """
            [
              {
                "id": "test-002",
                "question": "What is the annual revenue?",
                "expectedAnswer": null,
                "expectedDocumentIds": [],
                "answerExpected": false
              }
            ]
            """;

            await File.WriteAllTextAsync(path, json);

            var sut = new RagEvaluationDatasetLoader();

            var dataset = await sut.LoadAsync(path);

            var evaluationCase = Assert.Single(dataset.Cases);

            Assert.Null(evaluationCase.ExpectedAnswer);
            Assert.False(evaluationCase.AnswerExpected);
            Assert.Empty(evaluationCase.ExpectedDocumentIds);
        }
        finally
        {
            File.Delete(path);
        }
    }
}
