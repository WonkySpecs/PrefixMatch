namespace PrefixMatch;

public enum ResultType
{
    Success,
    NoMatch,
    NonUnique
}

public class Result
{
    public readonly ResultType Type;
    public readonly string? Word;
    public readonly List<string>? Suggestions;

    private Result(ResultType type, string? word = null, List<string>? suggestions = null)
    {
        Type = type;
        Word = word;
        Suggestions = suggestions;
    }

    public static Result NoMatch() => new(ResultType.NoMatch);
    public static Result NonUnique(List<string> suggestions) =>
        new(ResultType.NonUnique, suggestions: suggestions);
    public static Result Success(string word) => new(ResultType.Success, word);
}