using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Calendare.Data;

public class TestContextFactory : IDesignTimeDbContextFactory<CalendareContext>
{
    public TestContextFactory() { }

    public CalendareContext CreateDbContext(string[] args)
    {

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<TestContextFactory>()
            .Build();
        var optionsBuilder = new DbContextOptionsBuilder<CalendareContext>();
        optionsBuilder.ConfigureCalendareNpgsql(configuration.GetSection("Postgresql"));
        return new CalendareContext(optionsBuilder.Options);
    }
}
