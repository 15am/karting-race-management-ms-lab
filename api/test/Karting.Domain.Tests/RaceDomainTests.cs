using Karting.Domain.Entities;
using Karting.Domain.Enums;
using Karting.Domain.Exceptions;

namespace Karting.Domain.Tests;

public sealed class RaceDomainTests
{
    [Fact]
    public void Create_NonUtcDate_ThrowsDomainRuleException()
    {
        var date = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
        Assert.Throws<DomainRuleException>(() => Race.Create("Sprint", date, RaceStatus.Scheduled));
    }

    [Fact]
    public void Create_ValidRace_ReturnsRace()
    {
        var date = DateTime.UtcNow;
        var race = Race.Create("Sprint", date, RaceStatus.Scheduled);

        Assert.NotEqual(Guid.Empty, race.Id.Value);
        Assert.Equal("Sprint", race.Name.Value);
        Assert.Equal(RaceStatus.Scheduled, race.Status);
    }
}
