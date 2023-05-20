using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.IntegrationTests.Context;
public class TestContextFactory : IDisposable
{
    private bool disposedValue = false;

    public ApplicationDbContext Create()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(connection).Options;

        var context = new ApplicationDbContext(options);

        if (context != null)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        return context!;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }
}
