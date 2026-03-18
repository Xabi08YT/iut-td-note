using FilmApi.Models;
using FilmApi.Services;
using FilmApi.Repositories;
using FilmApi.Tests.Builders;
using NSubstitute;
using Xunit;
using VerifyXunit;

namespace FilmApi.Tests;

/// <summary>
/// Refactorisé pour utiliser Verify (Snapshot Testing) avec scrubbing des Ids et Dates.
/// </summary>
public class FilmDetailSnapshotTests
{
    [Fact]
    public async Task GetById_Returns_Complex_Film_Structure()
    {
        // Arrange
        var substituteRepo = Substitute.For<IFilmRepository>();
        
        var director = new DirectorBuilder()
            .WithId("dir-1")
            .WithLastName("Villeneuve")
            .WithFirstName("Denis")
            .WithNationality("CA")
            .WithBirthDate(new DateTime(1967, 10, 3))
            .Build();
            
        var actors = new List<Actor>
        {
            new ActorBuilder().WithId("a1").WithLastName("Chalamet").WithFirstName("Timothée").WithRole("Paul Atréides").Build(),
            new ActorBuilder().WithId("a2").WithLastName("Zendaya").WithFirstName("").WithRole("Chani").Build()
        };
        
        var genres = new List<Genre>
        {
            new GenreBuilder().WithId("g1").WithName("Science-Fiction").Build(),
            new GenreBuilder().WithId("g2").WithName("Aventure").Build()
        };
        
        var film = new FilmBuilder()
            .WithId("film-abc-123")
            .WithTitle("Dune")
            .WithSummary("Sur la planète Arrakis...")
            .WithYear(2021)
            .WithDurationMinutes(155)
            .WithReleaseDate(new DateTime(2021, 9, 15))
            .WithDirector(director)
            .WithActors(actors)
            .WithGenres(genres)
            .WithProductionCountry(new CountryBuilder().WithCode("US").WithName("États-Unis").Build())
            .Build();
            
        substituteRepo.GetByIdAsync("film-abc-123").Returns(film);

        var service = new FilmService(substituteRepo);

        // Act
        var result = await service.GetByIdAsync("film-abc-123");

        // Assert
        await Verifier.Verify(result)
            .ScrubMembers("Id", "ReleaseDate", "BirthDate");
    }
}


