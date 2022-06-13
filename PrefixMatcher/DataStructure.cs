namespace PrefixMatcher;

public class DataStructure
{
    private readonly Dictionary<char, DataStructure> _trails;
    private string? _leaf;

    public DataStructure()
    {
        _trails = new();
    }

    private DataStructure(string leaf) : this()
    {
        _leaf = leaf;
    }

    public void Insert(string word) => Insert(word, 0);
    private void Insert(string word, int charNum)
    {
        if (string.IsNullOrWhiteSpace(word)) return;
        if (charNum == word.Length)
        {
            _leaf = word;
            return;
        }

        var letter = word[charNum];
        if (_trails.ContainsKey(letter))
        {
            _trails[letter].Insert(word, charNum + 1);
        }
        else
        {
            if (_leaf == null || _leaf == word[..charNum])
            {
                _trails[letter] = new DataStructure(word);
            }
            else
            {
                _trails[_leaf[charNum]] = new DataStructure(_leaf);
                _leaf = null;
                Insert(word, charNum);
            }
        }
    }

    public Result Search(string prefix, int charNum = 0)
    {
        if (_leaf == prefix) return Result.Success(_leaf);

        if (_trails.Count == 0)
        {
            return _leaf?.StartsWith(prefix) ?? false
                ? Result.Success(_leaf)
                : Result.NoMatch();
        }

        if (charNum == prefix.Length) return Result.NonUnique();

        var letter = prefix[charNum];
        return _trails.ContainsKey(letter)
            ? _trails[letter].Search(prefix, charNum + 1)
            : Result.NoMatch();
    }
}