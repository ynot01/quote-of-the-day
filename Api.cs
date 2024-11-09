namespace ZenApi;
using System.Text.Json.Serialization;

public record class ZenApi(
    [property: JsonPropertyName("q")] string CurrentQuote,
    [property: JsonPropertyName("a")] string CurrentAuthor
);
