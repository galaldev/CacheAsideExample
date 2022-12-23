namespace CacheAsideExampel.Data
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;

    public class ProjectContext : DbContext
    {
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public string DbPath { get; }
        public ProjectContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "ExampleDB.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
      
    }

  
}
