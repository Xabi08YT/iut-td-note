using FilmApi.Models;

namespace FilmApi.Tests.Builders;

public class DirectorBuilder
{
    private string _id = "dir-1";
    private string _lastName = "Villeneuve";
    private string _firstName = "Denis";
    private string _nationality = "CA";
    private DateTime? _birthDate = new DateTime(1967, 10, 3);

    public DirectorBuilder WithId(string id) { _id = id; return this; }
    public DirectorBuilder WithLastName(string lastName) { _lastName = lastName; return this; }
    public DirectorBuilder WithFirstName(string firstName) { _firstName = firstName; return this; }
    public DirectorBuilder WithNationality(string nationality) { _nationality = nationality; return this; }
    public DirectorBuilder WithBirthDate(DateTime? birthDate) { _birthDate = birthDate; return this; }

    public Director Build() => new()
    {
        Id = _id,
        LastName = _lastName,
        FirstName = _firstName,
        Nationality = _nationality,
        BirthDate = _birthDate
    };
}
