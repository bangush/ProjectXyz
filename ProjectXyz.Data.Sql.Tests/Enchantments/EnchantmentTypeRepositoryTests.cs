﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Moq;
using ProjectXyz.Data.Interface;
using ProjectXyz.Data.Interface.Enchantments;
using ProjectXyz.Data.Sql.Enchantments;
using ProjectXyz.Tests.Xunit.Categories;
using Xunit;

namespace ProjectXyz.Data.Sql.Tests.Unit.Enchantments
{
    [DataLayer]
    [Enchantments]
    public class EnchantmentTypeRepositoryTests
    {
        #region Methods
        [Fact]
        public void GetByEnchantmentDefinitionId_IdExists_ExpectedValues()
        {
            // Setup
            var enchantmentTypeId = Guid.NewGuid();
            Guid enchantmentDefinitionId = Guid.NewGuid();
            const string STORE_CLASS_NAME = "the store class name";
            const string DEFINITION_CLASS_NAME = "the definition class name";

            var reader = new Mock<IDataReader>(MockBehavior.Strict);
            reader
                .Setup(x => x.Read())
                .Returns(true);
            reader
                .Setup(x => x.GetOrdinal("Id"))
                .Returns(0);
            reader
                .Setup(x => x.GetGuid(0))
                .Returns(enchantmentTypeId);
            reader
                .Setup(x => x.GetOrdinal("StoreRepositoryClassName"))
                .Returns(1);
            reader
                .Setup(x => x.GetString(1))
                .Returns(STORE_CLASS_NAME);
            reader
                .Setup(x => x.GetOrdinal("DefinitionRepositoryClassName"))
                .Returns(2);
            reader
                .Setup(x => x.GetString(2))
                .Returns(DEFINITION_CLASS_NAME);
            reader
                .Setup(x => x.Dispose());

            var command = new Mock<IDbCommand>(MockBehavior.Strict);
            command
                .Setup(x => x.ExecuteReader())
                .Returns(reader.Object);
            command
                .Setup(x => x.Dispose());

            var database = new Mock<IDatabase>();
            database
                .Setup(x => x.CreateCommand(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
                .Returns(command.Object);

            var expectedResult = new Mock<IEnchantmentType>(MockBehavior.Strict);

            var factory = new Mock<IEnchantmentTypeFactory>(MockBehavior.Strict);
            factory
                .Setup(x => x.Create(enchantmentTypeId, STORE_CLASS_NAME, DEFINITION_CLASS_NAME))
                .Returns(expectedResult.Object);

            var repository = EnchantmentTypeRepository.Create(
                database.Object,
                factory.Object);

            // Execute
            var result = repository.GetByEnchantmentDefinitionId(enchantmentDefinitionId);

            // Assert
            Assert.Equal(expectedResult.Object, result);

            reader.Verify(x => x.Read(), Times.Once());
            reader.Verify(x => x.GetOrdinal(It.IsAny<string>()), Times.Exactly(3));
            reader.Verify(x => x.GetGuid(It.IsAny<int>()), Times.Once());
            reader.Verify(x => x.GetString(It.IsAny<int>()), Times.Exactly(2));
            reader.Verify(x => x.Dispose(), Times.Once);

            database.Verify(x => x.CreateCommand(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()), Times.Once);

            command.Verify(x => x.ExecuteReader(), Times.Once);
            command.Verify(x => x.Dispose(), Times.Once);

            factory.Verify(x => x.Create(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void GetByEnchantmentDefinitionId_IdDoesNotExist_ThrowsInvalidOperationException()
        {
            // Setup
            Guid enchantmentDefinitionId = Guid.NewGuid();

            var reader = new Mock<IDataReader>(MockBehavior.Strict);
            reader
                .Setup(x => x.Read())
                .Returns(false);
            reader
                .Setup(x => x.Dispose());

            var command = new Mock<IDbCommand>(MockBehavior.Strict);
            command
                .Setup(x => x.ExecuteReader())
                .Returns(reader.Object);
            command
                .Setup(x => x.Dispose());

            var database = new Mock<IDatabase>();
            database
                .Setup(x => x.CreateCommand(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
                .Returns(command.Object);

            var factory = new Mock<IEnchantmentTypeFactory>(MockBehavior.Strict);

            var repository = EnchantmentTypeRepository.Create(
                database.Object,
                factory.Object);

            Assert.ThrowsDelegateWithReturn method = () => repository.GetByEnchantmentDefinitionId(enchantmentDefinitionId);

            // Execute
            var result = Assert.Throws<InvalidOperationException>(method);

            // Assert
            Assert.True(
                Regex.IsMatch(result.Message, "No enchantment type with enchantmentdefinition Id '.+' was found\\."),
                "Not expecting message: " + result.Message);

            reader.Verify(x => x.Read(), Times.Once());
            reader.Verify(x => x.Dispose(), Times.Once);

            database.Verify(x => x.CreateCommand(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()), Times.Once);

            command.Verify(x => x.ExecuteReader(), Times.Once);
            command.Verify(x => x.Dispose(), Times.Once);
        }

        [Fact]
        public void GetById_IdExists_ExpectedValues()
        {
            // Setup
            var enchantmentTypeId = Guid.NewGuid();
            Guid enchantmentDefinitionId = Guid.NewGuid();
            const string STORE_CLASS_NAME = "the store class name";
            const string DEFINITION_CLASS_NAME = "the definition class name";

            var reader = new Mock<IDataReader>(MockBehavior.Strict);
            reader
                .Setup(x => x.Read())
                .Returns(true);
            reader
                .Setup(x => x.GetOrdinal("Id"))
                .Returns(0);
            reader
                .Setup(x => x.GetGuid(0))
                .Returns(enchantmentTypeId);
            reader
                .Setup(x => x.GetOrdinal("StoreRepositoryClassName"))
                .Returns(1);
            reader
                .Setup(x => x.GetString(1))
                .Returns(STORE_CLASS_NAME);
            reader
                .Setup(x => x.GetOrdinal("DefinitionRepositoryClassName"))
                .Returns(2);
            reader
                .Setup(x => x.GetString(2))
                .Returns(DEFINITION_CLASS_NAME);
            reader
                .Setup(x => x.Dispose());

            var command = new Mock<IDbCommand>(MockBehavior.Strict);
            command
                .Setup(x => x.ExecuteReader())
                .Returns(reader.Object);
            command
                .Setup(x => x.Dispose());

            var database = new Mock<IDatabase>();
            database
                .Setup(x => x.CreateCommand(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
                .Returns(command.Object);

            var expectedResult = new Mock<IEnchantmentType>(MockBehavior.Strict);

            var factory = new Mock<IEnchantmentTypeFactory>(MockBehavior.Strict);
            factory
                .Setup(x => x.Create(enchantmentTypeId, STORE_CLASS_NAME, DEFINITION_CLASS_NAME))
                .Returns(expectedResult.Object);

            var repository = EnchantmentTypeRepository.Create(
                database.Object,
                factory.Object);

            // Execute
            var result = repository.GetById(enchantmentDefinitionId);

            // Assert
            Assert.Equal(expectedResult.Object, result);

            reader.Verify(x => x.Read(), Times.Once());
            reader.Verify(x => x.GetOrdinal(It.IsAny<string>()), Times.Exactly(3));
            reader.Verify(x => x.GetGuid(It.IsAny<int>()), Times.Once());
            reader.Verify(x => x.GetString(It.IsAny<int>()), Times.Exactly(2));
            reader.Verify(x => x.Dispose(), Times.Once);

            database.Verify(x => x.CreateCommand(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()), Times.Once);

            command.Verify(x => x.ExecuteReader(), Times.Once);
            command.Verify(x => x.Dispose(), Times.Once);

            factory.Verify(x => x.Create(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void GetById_IdDoesNotExist_ThrowsInvalidOperationException()
        {
            // Setup
            Guid enchantmentDefinitionId = Guid.NewGuid();

            var reader = new Mock<IDataReader>(MockBehavior.Strict);
            reader
                .Setup(x => x.Read())
                .Returns(false);
            reader
                .Setup(x => x.Dispose());

            var command = new Mock<IDbCommand>(MockBehavior.Strict);
            command
                .Setup(x => x.ExecuteReader())
                .Returns(reader.Object);
            command
                .Setup(x => x.Dispose());

            var database = new Mock<IDatabase>();
            database
                .Setup(x => x.CreateCommand(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()))
                .Returns(command.Object);

            var factory = new Mock<IEnchantmentTypeFactory>(MockBehavior.Strict);

            var repository = EnchantmentTypeRepository.Create(
                database.Object,
                factory.Object);

            Assert.ThrowsDelegateWithReturn method = () => repository.GetById(enchantmentDefinitionId);

            // Execute
            var result = Assert.Throws<InvalidOperationException>(method);

            // Assert
            Assert.True(
                Regex.IsMatch(result.Message, "No enchantment type with Id '.+' was found\\."),
                "Not expecting message: " + result.Message);

            reader.Verify(x => x.Read(), Times.Once());
            reader.Verify(x => x.Dispose(), Times.Once);

            database.Verify(x => x.CreateCommand(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()), Times.Once);

            command.Verify(x => x.ExecuteReader(), Times.Once);
            command.Verify(x => x.Dispose(), Times.Once);
        }
        #endregion
    }
}