namespace PrefixMatcher;

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

    private Result(ResultType type, string? word = null)
    {
        Type = type;
        Word = word;
    }

    public static Result NoMatch() => new(ResultType.NoMatch);
    public static Result NonUnique() => new(ResultType.NonUnique);
    public static Result Success(string word) => new(ResultType.Success, word);
}