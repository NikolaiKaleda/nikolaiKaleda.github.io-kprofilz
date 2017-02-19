using System;
using System.Linq;
using Nop.Core;
using Nop.Core.Data;
using Nop.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Nop.Plugin.Widgets.Gallery.Data
{
    public class GalleryObjectContext : DbContext, IDbContext
    {
        public GalleryObjectContext(string nameOrConnectionString) : base(nameOrConnectionString) { }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new GalleryMap());
            modelBuilder.Configurations.Add(new ImageGalleryMap());
            base.OnModelCreating(modelBuilder);
        }

        public string CreateDatabaseInstallationScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        public void Install()
        {
            //It's required to set initializer to null (for SQL Server Compact).
            //otherwise, you'll get something like "The model backing the 'your context name' context has changed since the database was created. Consider using Code First Migrations to update the database"
            Database.SetInitializer<GalleryObjectContext>(null);

            const string dbScriptImageGallery = @"IF EXISTS (
                                      SELECT 1
                                      FROM   INFORMATION_SCHEMA.TABLES
                                      WHERE  TABLE_NAME = 'ImageGallery'
                                   )
                                    SELECT 1 AS res
                                ELSE
                                    SELECT 0 AS res";

            const string dbScriptGallery = @"IF EXISTS (
                                      SELECT 1
                                      FROM   INFORMATION_SCHEMA.TABLES
                                      WHERE  TABLE_NAME = 'Gallery'
                                   )
                                    SELECT 1 AS res
                                ELSE
                                    SELECT 0 AS res";


            //SQL CE
            var databaseFileName = "Nop.Db.sdf";
            var databasePath = @"|DataDirectory|\" + databaseFileName;
            var connectionString = "Data Source=" + databasePath + ";Persist Security Info=False";

            var settingsManager = new DataSettingsManager();
            var setting = settingsManager.LoadSettings();

            if (Database.SqlQuery<int>(dbScriptImageGallery).ToList()[0] != 0 &&
            Database.SqlQuery<int>(dbScriptGallery).ToList()[0] == 0)
            {
                throw new Exception("Run update script before install plugin");
            }

            if (setting.DataConnectionString == connectionString)
            {
                Database.ExecuteSqlCommand(CreateDatabaseInstallationScript());
                SaveChanges();
            }

            else if (Database.SqlQuery<int>(dbScriptImageGallery).ToList()[0] == 0 ||
                     Database.SqlQuery<int>(dbScriptGallery).ToList()[0] == 0)
            {
                Database.ExecuteSqlCommand(CreateDatabaseInstallationScript());
                SaveChanges();
            }
        }

        public void Uninstall()
        {
            Database.ExecuteSqlCommand("DROP TABLE ImageGallery");
            Database.ExecuteSqlCommand("DROP TABLE Gallery");
            SaveChanges();
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public System.Collections.Generic.IList<TEntity> ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters) where TEntity : BaseEntity, new()
        {
            throw new System.NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public int ExecuteSqlCommand(string sql, bool doNotEnsureTransaction = false, int? timeout = null, params object[] parameters)
        {
            throw new NotImplementedException();
        }


        public void Detach(object entity)
        {
            throw new NotImplementedException();
        }

        public bool ProxyCreationEnabled
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool AutoDetectChangesEnabled
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
