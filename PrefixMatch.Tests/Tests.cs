using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PrefixMatch;

namespace PrefixMatcherTests;

public class Tests
{
    [Test]
    public void SimpleTests()
    {
        var words =
            new List<string>
            {
                "schedule",
                "school",
                "something else"
            };
        var matcher = new PrefixMatcher(words);
        var expectedSuggestions = words;

        var nonUnique = matcher.Search("s");
        Assert.AreEqual(ResultType.NonUnique, nonUnique.Type);
        Assert.That(nonUnique.Suggestions, Is.EquivalentTo(expectedSuggestions));

        expectedSuggestions = expectedSuggestions
            .Where(w => w.StartsWith("sc")).ToList();

        var nonUnique2 = matcher.Search("sc");
        Assert.AreEqual(ResultType.NonUnique, nonUnique2.Type);
        Assert.That(nonUnique2.Suggestions, Is.EquivalentTo(expectedSuggestions));

        var nonUnique3 = matcher.Search("sch");
        Assert.AreEqual(ResultType.NonUnique, nonUnique3.Type);
        Assert.That(nonUnique3.Suggestions, Is.EquivalentTo(expectedSuggestions));

        var nonExistent = matcher.Search("scr");
        Assert.AreEqual(ResultType.NoMatch, nonExistent.Type);

        var match = matcher.Search("scho");
        Assert.AreEqual(ResultType.Success, match.Type);
        Assert.AreEqual("school", match.Word);

        var fullMatch = matcher.Search("school");
        Assert.AreEqual(ResultType.Success, fullMatch.Type);

        var overMatch = matcher.Search("school2");
        Assert.AreEqual(ResultType.NoMatch, overMatch.Type);
    }

    [Test]
    public void ShorterWordIsPrefix()
    {
        var words = new List<string>
        {
            "she",
            "sheer",
            "sheerwhatthisisntevenaword"
        };
        var matcher = new PrefixMatcher(words);
        var expectedSuggestions = words;

        var nonUnique = matcher.Search("s");
        Assert.AreEqual(ResultType.NonUnique, nonUnique.Type);
        Assert.That(nonUnique.Suggestions, Is.EquivalentTo(expectedSuggestions));

        nonUnique = matcher.Search("sh");
        Assert.AreEqual(ResultType.NonUnique, nonUnique.Type);
        Assert.That(nonUnique.Suggestions, Is.EquivalentTo(expectedSuggestions));

        var matchesShortest = matcher.Search("she");
        Assert.AreEqual(ResultType.Success, matchesShortest.Type);
        Assert.AreEqual("she", matchesShortest.Word);

        expectedSuggestions = words.Skip(1).ToList();

        nonUnique = matcher.Search("shee");
        Assert.AreEqual(ResultType.NonUnique, nonUnique.Type);
        Assert.That(nonUnique.Suggestions, Is.EquivalentTo(expectedSuggestions));

        var matchesMiddle = matcher.Search("sheer");
        Assert.AreEqual(ResultType.Success, matchesMiddle.Type);
        Assert.AreEqual("sheer", matchesMiddle.Word);

        var matchesLongest = matcher.Search("sheerw");
        Assert.AreEqual(ResultType.Success, matchesLongest.Type);
        Assert.AreEqual("sheerwhatthisisntevenaword", matchesLongest.Word);

        matchesLongest = matcher.Search("sheerwhatthisisntevenaword");
        Assert.AreEqual(ResultType.Success, matchesLongest.Type);
        Assert.AreEqual("sheerwhatthisisntevenaword", matchesLongest.Word);
    }

    [Test]
    public void WordAddedLater_ChangesSearchResults()
    {
        var matcher = new PrefixMatcher(
            new List<string>
            {
                "hi"
            });
        var matches = matcher.Search("h");
        Assert.AreEqual(ResultType.Success, matches.Type);
        Assert.AreEqual("hi", matches.Word);

        matcher.AddWord("highest");
        var expectedSuggestions = new List<string>
        {
            "hi", "highest"
        };

        var noLongerUnique = matcher.Search("h");
        Assert.AreEqual(ResultType.NonUnique, noLongerUnique.Type);
        Assert.That(noLongerUnique.Suggestions, Is.EquivalentTo(expectedSuggestions));

        matches = matcher.Search("hi");
        Assert.AreEqual(ResultType.Success, matches.Type);
        Assert.AreEqual("hi", matches.Word);

        matches = matcher.Search("hig");
        Assert.AreEqual(ResultType.Success, matches.Type);
        Assert.AreEqual("highest", matches.Word);
    }
}