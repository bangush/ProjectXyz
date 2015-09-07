﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using ProjectXyz.Data.Interface;
using ProjectXyz.Data.Interface.Enchantments;

namespace ProjectXyz.Data.Sql.Enchantments
{
    public sealed class EnchantmentTypeRepository : IEnchantmentTypeRepository
    {
        #region Fields
        private readonly IDatabase _database;
        private readonly IEnchantmentTypeFactory _factory;
        #endregion

        #region Constructors
        private EnchantmentTypeRepository(
            IDatabase database,
            IEnchantmentTypeFactory factory)
        {
            Contract.Requires<ArgumentNullException>(database != null);
            Contract.Requires<ArgumentNullException>(factory != null);

            _database = database;
            _factory = factory;
        }
        #endregion

        #region Methods
        public static IEnchantmentTypeRepository Create(
            IDatabase database,
            IEnchantmentTypeFactory factory)
        {
            Contract.Requires<ArgumentNullException>(database != null);
            Contract.Requires<ArgumentNullException>(factory != null);
            Contract.Ensures(Contract.Result<IEnchantmentTypeRepository>() != null);

            return new EnchantmentTypeRepository(
                database,
                factory);
        }

        public IEnchantmentType Add(
            Guid id, 
            string storeRepositoryClassName, 
            string definitionRepositoryClassName)
        {
            var namedParameters = new Dictionary<string, object>()
            {
                { "Id", id },
                { "StoreRepositoryClassName", storeRepositoryClassName },
                { "DefinitionRepositoryClassName", definitionRepositoryClassName },
            };

            using (var command = _database.CreateCommand(
                @"
                INSERT INTO
                    EnchantmentTypes
                (
                    Id,
                    StoreRepositoryClassName,
                    DefinitionRepositoryClassName
                )
                VALUES
                (
                    @Id,
                    @StoreRepositoryClassName,
                    @DefinitionRepositoryClassName
                )
                ;",
                namedParameters))
            {
                command.ExecuteNonQuery();
            }

            var enchantmentType = _factory.Create(
                id,
                storeRepositoryClassName,
                definitionRepositoryClassName);
            return enchantmentType;
        }

        public IEnchantmentType GetById(Guid id)
        {
            using (var command = _database.CreateCommand(
            @"
                SELECT 
                    *
                FROM
                    EnchantmentTypes
                WHERE
                    Id = @Id
                LIMIT 1",
               "Id",
               id))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new InvalidOperationException("No enchantment type with Id '" + id + "' was found.");
                    }

                    return GetFromReader(reader, _factory);
                }
            }
        }

        public IEnchantmentType GetByEnchantmentDefinitionId(Guid enchantmentDefinitionId)
        {
            using (var command = _database.CreateCommand(
            @"
                SELECT 
                    *
                FROM
                    EnchantmentDefinitions
                LEFT OUTER JOIN 
                    EnchantmentTypes
                ON
                    EnchantmentDefinitions.EnchantmentTypeId=EnchantmentTypes.Id
                WHERE
                    EnchantmentDefinitions.Id = @EnchantmentDefinitionid
                LIMIT 1",
               "EnchantmentDefinitionid",
               enchantmentDefinitionId))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        throw new InvalidOperationException("No enchantment type with enchantmentdefinition Id '" + enchantmentDefinitionId + "' was found.");
                    }

                    return GetFromReader(reader, _factory);
                }
            }
        }

        public IEnumerable<IEnchantmentType> GetAll()
        {
            using (var command = _database.CreateCommand(
            @"
                SELECT 
                    *
                FROM
                    EnchantmentTypes
                WHERE
                    Id = @Id"))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return GetFromReader(reader, _factory);   
                    }
                }
            }
        }

        public void RemoveById(Guid id)
        {
            using (var command = _database.CreateCommand(
                @"
                DELETE FROM
                    EnchantmentTypes
                WHERE
                    Id = @id
                ;",
                "@id",
                id))
            {
                command.ExecuteNonQuery();
            }
        }

        private IEnchantmentType GetFromReader(IDataReader reader, IEnchantmentTypeFactory factory)
        {
            Contract.Requires<ArgumentNullException>(reader != null);
            Contract.Requires<ArgumentNullException>(factory != null);
            Contract.Ensures(Contract.Result<IEnchantmentType>() != null);

            return factory.Create(
                reader.GetGuid(reader.GetOrdinal("Id")),
                reader.GetString(reader.GetOrdinal("StoreRepositoryClassName")),
                reader.GetString(reader.GetOrdinal("DefinitionRepositoryClassName")));
        }

        [ContractInvariantMethod]
        private void InvariantMethod()
        {
            Contract.Invariant(_database != null);
            Contract.Invariant(_factory != null);
        }
        #endregion
    }
}
