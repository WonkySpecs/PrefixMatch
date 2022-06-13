namespace PrefixMatch;

public class PrefixMatcher
{
    private readonly DataStructure _data = new();

    public PrefixMatcher(List<string> words)
    {
        words.ForEach(AddWord);
    }

    public void AddWord(string word)
    {
        _data.Insert(word);
    }

    public Result Search(string prefix)
    {
        return _data.Search(prefix);
    }
}