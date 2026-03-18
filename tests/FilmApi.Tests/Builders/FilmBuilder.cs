using FilmApi.Models;

namespace FilmApi.Tests.Builders;

public class FilmBuilder
{
    private string _id = "film-1";
    private string _title = "Dune";
    private string _summary = "Un jeune duc...";
    private int _year = 2021;
    private int _durationMinutes = 155;
    private DateTime? _releaseDate = new DateTime(2021, 9, 15);
    private Director _director = new DirectorBuilder().Build();
    private List<Genre> _genres = new() { new GenreBuilder().Build() };
    private List<Actor> _actors = new();
    private Country? _productionCountry = new CountryBuilder().Build();

    public FilmBuilder WithId(string id) { _id = id; return this; }
    public FilmBuilder WithTitle(string title) { _title = title; return this; }
    public FilmBuilder WithSummary(string summary) { _summary = summary; return this; }
    public FilmBuilder WithYear(int year) { _year = year; return this; }
    public FilmBuilder WithDurationMinutes(int durationMinutes) { _durationMinutes = durationMinutes; return this; }
    public FilmBuilder WithReleaseDate(DateTime? releaseDate) { _releaseDate = releaseDate; return this; }
    public FilmBuilder WithDirector(Director director) { _director = director; return this; }
    public FilmBuilder WithGenres(List<Genre> genres) { _genres = genres; return this; }
    public FilmBuilder WithActors(List<Actor> actors) { _actors = actors; return this; }
    public FilmBuilder WithProductionCountry(Country? productionCountry) { _productionCountry = productionCountry; return this; }

    public Film Build() => new()
    {
        Id = _id,
        Title = _title,
        Summary = _summary,
        Year = _year,
        DurationMinutes = _durationMinutes,
        ReleaseDate = _releaseDate,
        Director = _director,
        Genres = _genres,
        Actors = _actors,
        ProductionCountry = _productionCountry
    };
}
