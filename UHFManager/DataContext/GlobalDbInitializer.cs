using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UHFManager.DataModel;

namespace UHFManager.DataContext
{
    internal class GlobalDbInitializer : SqliteCreateDatabaseIfNotExists<GlobalDbContext>
    {
        public GlobalDbInitializer(DbModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        public GlobalDbInitializer(DbModelBuilder modelBuilder, bool nullByteFileMeansNotExisting) : base(modelBuilder, nullByteFileMeansNotExisting)
        {
        }

        public override void InitializeDatabase(GlobalDbContext context)
        {
            base.InitializeDatabase(context);
        }

        protected override void Seed(GlobalDbContext context)
        {
            // 初始化数据
            if (context.DeviceUsages.Count() == 0)
            {
                context.DeviceUsages.Add(new DeviceUsage() { Name = "入库", Value = 1 });
                context.DeviceUsages.Add(new DeviceUsage() { Name = "中转", Value = 2 });
                context.DeviceUsages.Add(new DeviceUsage() { Name = "出库", Value = 3 });

                context.SaveChanges();
            }

            base.Seed(context);
        }
    }
}
