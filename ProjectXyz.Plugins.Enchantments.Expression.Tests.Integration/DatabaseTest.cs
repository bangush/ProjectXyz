﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Moq;
using ProjectXyz.Data.Interface.Enchantments;
using ProjectXyz.Data.Sql;
using ProjectXyz.Plugins.Enchantments.Expression.Sql;
using ProjectXyz.Tests.Xunit.Categories;
using Xunit;

namespace ProjectXyz.Plugins.Enchantments.Expression.Tests.Integration
{
    public abstract class DatabaseTest : IDisposable
    {
        #region Fields
        private readonly IDatabase _database;
        #endregion

        #region Constructors
        public DatabaseTest()
        {
            var connection = new SQLiteConnection("Data Source=:memory:");
            connection.Open();

            _database = SqlDatabase.Create(connection, true);

            SqlDatabaseUpgrader.Create().UpgradeDatabase(_database, 0, 1);
        }

        ~DatabaseTest()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Properties
        protected IDatabase Database
        {
            get { return _database; }
        }
        #endregion

        #region Methods
        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _database.Dispose();
            }
        }
        #endregion
    }
}