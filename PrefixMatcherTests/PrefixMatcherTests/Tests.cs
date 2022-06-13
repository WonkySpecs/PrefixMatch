using System.Collections.Generic;
using NUnit.Framework;
using PrefixMatcher;

namespace PrefixMatcherTests;

public class Tests
{
    [Test]
    public void SimpleTests()
    {
        var matcher = new PrefixMatcher.PrefixMatcher(
            new List<string>
            {
                "schedule",
                "school",
                "something else"
            });
        var nonUnique = matcher.Search("s");
        Assert.AreEqual(ResultType.NonUnique, nonUnique.Type);

        var nonUnique2 = matcher.Search("sc");
        Assert.AreEqual(ResultType.NonUnique, nonUnique2.Type);

        var nonUnique3 = matcher.Search("sch");
        Assert.AreEqual(ResultType.NonUnique, nonUnique3.Type);

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
        var matcher = new PrefixMatcher.PrefixMatcher(
            new List<string>
            {
                "she",
                "sheer",
                "sheerwhatthisisntevenaword"
            });
        var nonUnique = matcher.Search("s");
        Assert.AreEqual(ResultType.NonUnique, nonUnique.Type);
        
        nonUnique = matcher.Search("sh");
        Assert.AreEqual(ResultType.NonUnique, nonUnique.Type);
        
        var matchesShortest = matcher.Search("she");
        Assert.AreEqual(ResultType.Success, matchesShortest.Type);
        Assert.AreEqual("she", matchesShortest.Word);
        
        nonUnique = matcher.Search("shee");
        Assert.AreEqual(ResultType.NonUnique, nonUnique.Type);
        
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
        var matcher = new PrefixMatcher.PrefixMatcher(
            new List<string>
            {
                "hi"
            });
        var matches = matcher.Search("h");
        Assert.AreEqual(ResultType.Success, matches.Type);
        Assert.AreEqual("hi", matches.Word);
        
        matcher.AddWord("highest");
        var noLongerUnique = matcher.Search("h");
        Assert.AreEqual(ResultType.NonUnique, noLongerUnique.Type);
        
        matches = matcher.Search("hi");
        Assert.AreEqual(ResultType.Success, matches.Type);
        Assert.AreEqual("hi", matches.Word);
        
        matches = matcher.Search("hig");
        Assert.AreEqual(ResultType.Success, matches.Type);
        Assert.AreEqual("highest", matches.Word);
    }
}