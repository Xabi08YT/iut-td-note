using FilmApi.Models;

namespace FilmApi.Tests.Builders;

public class GenreBuilder
{
    private string _id = "g1";
    private string _name = "Science-Fiction";

    public GenreBuilder WithId(string id) { _id = id; return this; }
    public GenreBuilder WithName(string name) { _name = name; return this; }

    public Genre Build() => new()
    {
        Id = _id,
        Name = _name
    };
}
