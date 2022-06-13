# PrefixMatch

Given a string, find the string it's a unique prefix of from a selection of strings, if it is one.

## Usage

```
var matcher = new PrefixMatcher.PrefixMatcher(
    new List<string>
    {
        "schedule",
        "school",
        "something else"
    });

var result = matcher.Search("scho");
// result.Type -> Success
// result.Word -> "school"

result = matcher.Search("a");
// result.Type -> NoMatch

result = matcher.Search("sc");
// result.Type -> NonUnique
// result.Suggestions -> ["schedule", "school"]
```
