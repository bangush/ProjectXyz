﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using Moq;
using ProjectXyz.Data.Sql;
using ProjectXyz.Plugins.Enchantments.Percentage.Sql;
using ProjectXyz.Tests.Xunit.Categories;
using Xunit;

namespace ProjectXyz.Plugins.Enchantments.Percentage.Tests.Integration
{
    [DataLayer]
    [Enchantments]
    public class PercentageEnchantmentStoreRepositoryTests
    {
        #region Fields
        private readonly IDatabase _database;
        #endregion

        #region Constructors
        public PercentageEnchantmentStoreRepositoryTests()
        {
            var connection = new SQLiteConnection("Data Source=:memory:");
            connection.Open();

            _database = SqlDatabase.Create(connection, true);

            SqlDatabaseUpgrader.Create().UpgradeDatabase(_database, 0, 1);
        }
        #endregion

        #region Methods
        [Fact]
        public void Add_ValidEnchantmentStore_Success()
        {
            // Setup
            var enchantmentStoreId = Guid.NewGuid();
            var statId = Guid.NewGuid();
            const double VALUE = 12345;
            
            var enchantmentStore = new Mock<IPercentageEnchantmentStore>(MockBehavior.Strict);
            enchantmentStore
                .Setup(x => x.Id)
                .Returns(enchantmentStoreId);
            enchantmentStore
                .Setup(x => x.StatId)
                .Returns(statId);
            enchantmentStore
                .Setup(x => x.Value)
                .Returns(VALUE);

            var factory = new Mock<IPercentageEnchantmentStoreFactory>(MockBehavior.Strict);
            
            var repository = PercentageEnchantmentStoreRepository.Create(
                _database,
                factory.Object);

            // Execute
            repository.Add(enchantmentStore.Object);

            // Assert
            using (var reader = _database.Query("SELECT COUNT(1) FROM PercentageEnchantments WHERE EnchantmentId=@Id", "@Id", enchantmentStoreId))
            {
                Assert.True(reader.Read(), "Expecting the reader to read a single row.");
                Assert.Equal(1, reader.GetInt32(0));
            }

            enchantmentStore.Verify(x => x.Id, Times.Once);
            enchantmentStore.Verify(x => x.StatId, Times.Once);
            enchantmentStore.Verify(x => x.Value, Times.Once);
        }

        [Fact]
        public void RemoveById_IdExists_Success()
        {
            // Setup
            var enchantmentStoreId = Guid.NewGuid();
            var statId = Guid.NewGuid();
            const double VALUE = 12345;
            
            var factory = new Mock<IPercentageEnchantmentStoreFactory>(MockBehavior.Strict);
            
            var repository = PercentageEnchantmentStoreRepository.Create(
                _database,
                factory.Object);

            var namedParameters = new Dictionary<string, object>()
            {
                { "EnchantmentId", enchantmentStoreId },
                { "StatId", statId },
                { "Value", VALUE },
            };

            using (var command = _database.CreateCommand(
                @"
                INSERT INTO
                    PercentageEnchantments
                (
                    EnchantmentId,
                    StatId,
                    Value
                )
                VALUES
                (
                    @EnchantmentId,
                    @StatId,
                    @Value
                )
                ;",
                namedParameters))
            {
                command.ExecuteNonQuery();
            }

            // Execute
            repository.RemoveById(enchantmentStoreId);

            // Assert
            using (var reader = _database.Query("SELECT COUNT(1) FROM PercentageEnchantments WHERE EnchantmentId=@Id", "@Id", enchantmentStoreId))
            {
                Assert.True(reader.Read(), "Expecting the reader to read a single row.");
                Assert.Equal(0, reader.GetInt32(0));
            }
        }

        [Fact]
        public void GetById_IdExists_Success()
        {
            // Setup
            var enchantmentStoreId = Guid.NewGuid();
            var statId = Guid.NewGuid();
            const double VALUE = 12345;

            var enchantmentStore = new Mock<IPercentageEnchantmentStore>(MockBehavior.Strict);

            var factory = new Mock<IPercentageEnchantmentStoreFactory>(MockBehavior.Strict);
            factory
                .Setup(x => x.CreateEnchantmentStore(enchantmentStoreId, statId, VALUE))
                .Returns(enchantmentStore.Object);
            
            var repository = PercentageEnchantmentStoreRepository.Create(
                _database,
                factory.Object);

            var namedParameters = new Dictionary<string, object>()
            {
                { "EnchantmentId", enchantmentStoreId },
                { "StatId", statId },
                { "Value", VALUE },
            };

            using (var command = _database.CreateCommand(
                @"
                INSERT INTO
                    PercentageEnchantments
                (
                    EnchantmentId,
                    StatId,
                    Value
                )
                VALUES
                (
                    @EnchantmentId,
                    @StatId,
                    @Value
                )
                ;",
                namedParameters))
            {
                command.ExecuteNonQuery();
            }

            // Execute
            var result = repository.GetById(enchantmentStoreId);

            // Assert
            Assert.Equal(enchantmentStore.Object, result);

            factory.Verify(
                x => x.CreateEnchantmentStore(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>(),
                    It.IsAny<double>()), 
                Times.Once);
        }
        #endregion
    }
}