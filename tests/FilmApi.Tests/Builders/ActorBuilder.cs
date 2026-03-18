using FilmApi.Models;

namespace FilmApi.Tests.Builders;

public class ActorBuilder
{
    private string _id = "a1";
    private string _lastName = "Chalamet";
    private string _firstName = "Timothée";
    private string _role = "Paul Atréides";

    public ActorBuilder WithId(string id) { _id = id; return this; }
    public ActorBuilder WithLastName(string lastName) { _lastName = lastName; return this; }
    public ActorBuilder WithFirstName(string firstName) { _firstName = firstName; return this; }
    public ActorBuilder WithRole(string role) { _role = role; return this; }

    public Actor Build() => new()
    {
        Id = _id,
        LastName = _lastName,
        FirstName = _firstName,
        Role = _role
    };
}
