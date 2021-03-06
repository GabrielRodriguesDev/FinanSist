using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FinanSist.Database
{
    public class ContextFactory : IDesignTimeDbContextFactory<FinanSistContext>
    {
        public FinanSistContext CreateDbContext(string[] args)
        {
            //var connectionString = "Server=localhost;Port=7000;Database=FinanSist;Uid=root;Pwd=fx870"; //Migration Container
            //var connectionString = "Server=172.18.0.2;Port=3306;Database=FinanSist;Uid=root;Pwd=fx870"; //Container
            //var connectionString = "Server=localhost;Port=3306;Database=FinanSist;Uid=root;Pwd=fx870";
            var connectionString = "Server=bd.asp.hostazul.com.br;Port=4406;Database=9198_finansist;Uid=9198_finansist;Pwd=Fx@870Fx@870"; //HostAzul

            var optionsBuilder = new DbContextOptionsBuilder<FinanSistContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            return new FinanSistContext(optionsBuilder.Options);

        }
    }
}