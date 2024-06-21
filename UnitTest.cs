using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Common;
using Ductus.FluentDocker.XUnit;
using FluentAssertions;
using MySqlConnector;

namespace TryDockerTest;

public class MySqlDockerTestBase : FluentDockerTestBase
{
    protected override ContainerBuilder Build()
    {
        return new Builder()
            .UseContainer()
            .UseImage("mysql")
            .ExposePort(3306, 3306)
            .WithName("test-mysql")
            .ReuseIfExists()
            .WithEnvironment("MYSQL_ROOT_PASSWORD=root")
            .WaitForPort("3306/tcp", TimeSpan.FromSeconds(5), "127.0.0.1")
            .Wait("Try to connect to MySQL.", (service, count) =>
            {
                if (count > 20)
                    throw new FluentDockerException("Failed to wait for mysql server.");

                using var connection = new MySqlConnection("Server=127.0.0.1; Port=3306; User ID=root; Password=root;");
                try
                {
                    connection.Open();
                    return TimeSpan.Zero.Milliseconds;
                }
                catch (Exception)
                {
                    return TimeSpan.FromMilliseconds(500).Milliseconds;
                }
            });
    }
}


[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<MySqlDockerTestBase>
{
}

[Collection("Database collection")]
public class UnitTest1
{
    private readonly MySqlConnection _connection;

    public UnitTest1(MySqlConnection connection)
    {
        _connection = connection;
    }

    [Fact]
    public async Task try_use_docker()
    {
        await _connection.OpenAsync();
        var pong = await _connection.PingAsync();
        pong.Should().BeTrue();
        Console.WriteLine("try_use_docker 1");
        await Task.Delay(3000);
    }
}

[Collection("Database collection")]
public class UnitTest2
{
    private readonly MySqlConnection _connection;

    public UnitTest2(MySqlConnection connection)
    {
        _connection = connection;
    }

    [Fact]
    public async Task try_use_docker2()
    {
        await _connection.OpenAsync();
        var pong = await _connection.PingAsync();
        pong.Should().BeTrue();
        Console.WriteLine("try_use_docker 2");
        await Task.Delay(3000);
    }
}