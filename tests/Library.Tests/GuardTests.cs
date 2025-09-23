using System;
using Json.Masker.Abstract.Utils;
using Xunit;

namespace Json.Masker.Tests;

public class GuardTests
{
    [Fact]
    public void NotNull_AllowsNonNull()
    {
        Guard.NotNull("value", nameof(NotNull_AllowsNonNull));
    }

    [Fact]
    public void NotNull_ThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => Guard.NotNull<object>(null!, "param"));
    }
}
