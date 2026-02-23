using Karting.Domain.Entities;
using Karting.Domain.Exceptions;

namespace Karting.Domain.Tests;

public sealed class LapTimeDomainTests
{
    [Fact]
    public void Create_InvalidLapNumber_ThrowsDomainRuleException()
    {
        var raceId = Guid.NewGuid();
        var driverId = Guid.NewGuid();

        Assert.Throws<DomainRuleException>(() => LapTime.Create(raceId, driverId, 0, 50000));
    }

    [Fact]
    public void Create_InvalidTime_ThrowsDomainRuleException()
    {
        var raceId = Guid.NewGuid();
        var driverId = Guid.NewGuid();

        Assert.Throws<DomainRuleException>(() => LapTime.Create(raceId, driverId, 1, 0));
    }
}
