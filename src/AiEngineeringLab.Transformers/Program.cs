using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.Tokenizers;

var vocabPath = Path.Combine(
    AppContext.BaseDirectory,
    "Models",
    "bert-tiny",
    "vocab.txt");

var modelPath = Path.Combine(
    AppContext.BaseDirectory,
    "Models",
    "bert-tiny",
    "model.onnx");

var tokenizer = BertTokenizer.Create(vocabPath);

var samples = new[]
{
    "I really appreciate your help.",
    "You are stupid and useless."
};

using var session = new InferenceSession(modelPath);

foreach (var text in samples)
{
    Console.WriteLine();
    Console.WriteLine($"Texto: {text}");

    var tokenIds = tokenizer.EncodeToIds(
        text,
        addSpecialTokens: true);

    var sequenceLength = tokenIds.Count;

    var inputIds = tokenIds
        .Select(id => (long)id)
        .ToArray();

    var attentionMask = Enumerable
        .Repeat(1L, sequenceLength)
        .ToArray();

    var inputIdsTensor =
        new DenseTensor<long>(
            inputIds,
            new[] { 1, sequenceLength });

    var attentionMaskTensor =
        new DenseTensor<long>(
            attentionMask,
            new[] { 1, sequenceLength });

    var inputs = new List<NamedOnnxValue>
    {
        NamedOnnxValue.CreateFromTensor(
            "input_ids",
            inputIdsTensor),

        NamedOnnxValue.CreateFromTensor(
            "attention_mask",
            attentionMaskTensor)
    };

    using var results = session.Run(inputs);

    var logits = results
        .First(result => result.Name == "logits")
        .AsTensor<float>()
        .ToArray();

    var maxLogit = logits.Max();

    var expValues = logits
        .Select(x => Math.Exp(x - maxLogit))
        .ToArray();

    var sumExp = expValues.Sum();

    var probabilities = expValues
        .Select(x => x / sumExp)
        .ToArray();

    Console.WriteLine($"Not toxic: {probabilities[0]:P2}");
    Console.WriteLine($"Toxic:     {probabilities[1]:P2}");

    var predictedClass =
        probabilities[1] > probabilities[0]
            ? "TOXIC"
            : "NOT TOXIC";

    Console.WriteLine($"Resultado: {predictedClass}");
}
