using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FilmApi.Models;
using FilmApi.Tests.Builders;
using Xunit;

namespace FilmApi.Tests;

/// <summary>
/// Tests d'intégration : HTTP → API → Service → Repository → MongoDB.
/// </summary>
public sealed class FilmApiIntegrationTests : IClassFixture<MongoFixture>, IAsyncLifetime, IDisposable
{
    private readonly MongoFixture _mongo;
    private readonly FilmApiAppFactory _factory;
    private readonly HttpClient _client;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public FilmApiIntegrationTests(MongoFixture mongo)
    {
        _mongo = mongo;
        _factory = new FilmApiAppFactory(mongo);
        _client = _factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        await _mongo.InitializeAsync();
        await _mongo.ClearFilmsAsync();
    }

    public void Dispose() => _factory.Dispose();

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task POST_films_Returns_201_And_Film()
    {
        // Arrange
        var director = new DirectorBuilder()
            .WithLastName("Dupont")
            .WithFirstName("Jean")
            .WithNationality("FR")
            .Build();
            
        var request = new CreateFilmRequest(
            Title: "Mon Film",
            Summary: "Résumé.",
            Year: 2024,
            DurationMinutes: 90,
            ReleaseDate: null,
            Director: director,
            Genres: new List<Genre> { new GenreBuilder().WithName("Drame").Build() },
            Actors: new List<Actor>(),
            ProductionCountry: new CountryBuilder().WithCode("FR").WithName("France").Build()
        );

        // Act
        var response = await _client.PostAsJsonAsync("/films", request, JsonOptions);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var film = await response.Content.ReadFromJsonAsync<Film>(JsonOptions);
        Assert.NotNull(film);
        Assert.False(string.IsNullOrEmpty(film.Id));
        Assert.Equal("Mon Film", film.Title);
        Assert.Equal(2024, film.Year);
    }

    [Fact]
    public async Task GET_films_id_Returns_200_After_Post()
    {
        // Arrange
        var director = new DirectorBuilder().WithLastName("Martin").Build();
        var request = new CreateFilmRequest(
            "Film pour GET",
            "Résumé GET",
            2023,
            100,
            null,
            director,
            new List<Genre> { new GenreBuilder().WithName("Comédie").Build() },
            new List<Actor>(),
            null
        );
        var postResponse = await _client.PostAsJsonAsync("/films", request, JsonOptions);
        postResponse.EnsureSuccessStatusCode();
        var created = await postResponse.Content.ReadFromJsonAsync<Film>(JsonOptions);
        Assert.NotNull(created);

        // Act
        var response = await _client.GetAsync($"/films/{created.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var film = await response.Content.ReadFromJsonAsync<Film>(JsonOptions);
        Assert.NotNull(film);
        Assert.Equal(created.Id, film.Id);
        Assert.Equal("Film pour GET", film.Title);
        Assert.Equal("Martin", film.Director.LastName);
    }

    [Fact]
    public async Task GET_films_With_ReleaseYear_Filter_Returns_Only_Matching_Films()
    {
        // Arrange
        var director = new DirectorBuilder().Build();
        var film2020 = new CreateFilmRequest("Film 2020", "Summary", 2020, 100, new DateTime(2020, 5, 20, 0, 0, 0, DateTimeKind.Utc), director, new(), new(), null);
        var film2021 = new CreateFilmRequest("Film 2021", "Summary", 2021, 100, new DateTime(2021, 9, 15, 0, 0, 0, DateTimeKind.Utc), director, new(), new(), null);
        
        await _client.PostAsJsonAsync("/films", film2020, JsonOptions);
        await _client.PostAsJsonAsync("/films", film2021, JsonOptions);

        // Act
        var response = await _client.GetAsync("/films?releaseYear=2021");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<PagedResult<Film>>(JsonOptions);
        Assert.NotNull(result);
        Assert.Equal(1, result.TotalCount);
        Assert.Single(result.Items);
        Assert.Equal(2021, result.Items[0].Year);
        Assert.Equal(new DateTime(2021, 9, 15, 0, 0, 0, DateTimeKind.Utc), result.Items[0].ReleaseDate);
    }




    [Fact]
    public async Task DELETE_films_id_Removes_Film_Successfully()
    {
        // Arrange
        var request = new CreateFilmRequest("To Delete", "Summary", 2024, 100, null, new DirectorBuilder().Build(), new(), new(), null);
        var postResponse = await _client.PostAsJsonAsync("/films", request, JsonOptions);
        var created = await postResponse.Content.ReadFromJsonAsync<Film>(JsonOptions);
        Assert.NotNull(created);

        // Act
        var deleteResponse = await _client.DeleteAsync($"/films/{created.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        
        var getResponse = await _client.GetAsync($"/films/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}

