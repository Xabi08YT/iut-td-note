using FilmApi.Models;
using FilmApi.Repositories;
using FilmApi.Services;
using FilmApi.Tests.Builders;
using NSubstitute;
using Xunit;

namespace FilmApi.Tests;

/// <summary>
/// Tests unitaires du FilmService avec un mock du repository.
/// </summary>
public class FilmServiceUnitTests
{
    [Fact]
    public async Task CreateAsync_Calls_Repository_AddAsync_And_Returns_Film()
    {
        // Arrange
        var substituteRepo = Substitute.For<IFilmRepository>();
        var service = new FilmService(substituteRepo);
        
        var director = new DirectorBuilder().Build();
        var genre = new GenreBuilder().Build();
        var expectedFilm = new FilmBuilder()
            .WithId("film1")
            .WithDirector(director)
            .WithGenres(new List<Genre> { genre })
            .Build();

        substituteRepo
            .AddAsync(Arg.Any<Film>())
            .Returns(expectedFilm);

        var request = new CreateFilmRequest(
            Title: expectedFilm.Title,
            Summary: expectedFilm.Summary,
            Year: expectedFilm.Year,
            DurationMinutes: expectedFilm.DurationMinutes,
            ReleaseDate: expectedFilm.ReleaseDate,
            Director: director,
            Genres: new List<Genre> { genre },
            Actors: new List<Actor>(),
            ProductionCountry: expectedFilm.ProductionCountry
        );

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.Equal("film1", result.Id);
        Assert.Equal(expectedFilm.Title, result.Title);
        Assert.Equal(expectedFilm.Year, result.Year);
        await substituteRepo
            .Received(1)
            .AddAsync(Arg.Is<Film>(f => f.Title == expectedFilm.Title));
    }

    [Fact]
    public async Task GetByIdAsync_Returns_Film_When_Exists()
    {
        // Arrange
        var substituteRepo = Substitute.For<IFilmRepository>();
        var service = new FilmService(substituteRepo);
        
        var director = new DirectorBuilder()
            .WithLastName("Nolan")
            .Build();
        var film = new FilmBuilder()
            .WithId("f2")
            .WithTitle("Inception")
            .WithYear(2010)
            .WithDirector(director)
            .Build();
            
        substituteRepo.GetByIdAsync("f2").Returns(film);

        // Act
        var result = await service.GetByIdAsync("f2");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Inception", result.Title);
        Assert.Equal("Nolan", result.Director.LastName);
    }

    [Fact]
    public async Task DeleteAsync_Returns_True_When_Repository_Deletes()
    {
        // Arrange
        var substituteRepo = Substitute.For<IFilmRepository>();
        substituteRepo.DeleteByIdAsync("f1").Returns(true);
        var service = new FilmService(substituteRepo);

        // Act
        var result = await service.DeleteAsync("f1");

        // Assert
        Assert.True(result);
        await substituteRepo.Received(1).DeleteByIdAsync("f1");
    }

    [Fact]
    public async Task DeleteAsync_Returns_False_When_Not_Found()
    {
        // Arrange
        var substituteRepo = Substitute.For<IFilmRepository>();
        substituteRepo.DeleteByIdAsync("missing").Returns(false);
        var service = new FilmService(substituteRepo);

        // Act
        var result = await service.DeleteAsync("missing");

        // Assert
        Assert.False(result);
        await substituteRepo.Received(1).DeleteByIdAsync("missing");
    }
}

