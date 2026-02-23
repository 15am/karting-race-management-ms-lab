using Karting.Domain.Entities;
using Karting.Domain.Exceptions;

namespace Karting.Domain.Tests;

public sealed class DriverDomainTests
{
    [Fact]
    public void Create_InvalidName_ThrowsDomainRuleException()
    {
        Assert.Throws<DomainRuleException>(() => Driver.Create(" "));
    }

    [Fact]
    public void Create_ValidName_ReturnsDriver()
    {
        var driver = Driver.Create("Lewis");

        Assert.NotEqual(Guid.Empty, driver.Id.Value);
        Assert.Equal("Lewis", driver.Name.Value);
        Assert.Equal(DateTimeKind.Utc, driver.CreatedAtUtc.Kind);
    }
}
