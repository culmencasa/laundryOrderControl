using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UHFManager.DataModel;

namespace UHFManager.DataContext
{
    public class GlobalDbContext : DbContext
    {
        #region 数据集合

        public DbSet<DeviceSetting> DeviceSettings { get { return Set<DeviceSetting>(); } }
        public DbSet<DeviceUsage> DeviceUsages { get { return Set<DeviceUsage>(); } }


        #endregion


        public GlobalDbContext() : base("globalDatabase")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);


            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.AddFromAssembly(typeof(GlobalDbContext).Assembly);
#if DEBUG
            Database.SetInitializer(new GlobalDbInitializer(modelBuilder));
#endif
                        

            //modelBuilder.Entity<Book>();
            //var init = new SqliteCreateDatabaseIfNotExists<GlobalDbContext>(modelBuilder);
            //Database.SetInitializer(init);
        }

    }
    

}
