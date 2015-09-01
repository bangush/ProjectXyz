﻿using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using ProjectXyz.Data.Interface.Items.Materials;
using ProjectXyz.Data.Sql.Items.Materials;
using ProjectXyz.Tests.Integration;
using ProjectXyz.Tests.Xunit.Categories;
using Xunit;

namespace ProjectXyz.Data.Sql.Tests.Integration.Items.Materials
{
    [DataLayer]
    [Enchantments]
    public class MaterialTypeRepositoryTests : DatabaseTest
    {
        #region Methods
        [Fact]
        public void GetAll_NoItems_NoResults()
        {
            // Setup
            var factory = new Mock<IMaterialTypeFactory>(MockBehavior.Strict);

            var repository = MaterialTypeRepository.Create(
                Database,
                factory.Object);

            // Execute
            var result = repository.GetAll();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetAll_MultipleItems_MultipleResults()
        {
            // Setup
            var genericCollectionResult = new Mock<IMaterialType>(MockBehavior.Strict);

            var factory = new Mock<IMaterialTypeFactory>(MockBehavior.Strict);
            factory
                .Setup(x => x.Create(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>()))
                .Returns(genericCollectionResult.Object);

            var repository = MaterialTypeRepository.Create(
                Database,
                factory.Object);

            const int NUMBER_OF_RESULTS = 5;
            for (int i = 0; i < NUMBER_OF_RESULTS; i++)
            {
                CreateMaterialType(
                    Guid.NewGuid(),
                    Guid.NewGuid());
            }

            // Execute
            var result = repository.GetAll();

            // Assert
            Assert.Equal(NUMBER_OF_RESULTS, result.Count());

            factory.Verify(
                x => x.Create(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>()),
                Times.Exactly(NUMBER_OF_RESULTS));
        }

        [Fact]
        public void GetById_NoItem_ThrowsException()
        {
            // Setup
            var factory = new Mock<IMaterialTypeFactory>(MockBehavior.Strict);

            var repository = MaterialTypeRepository.Create(
                Database,
                factory.Object);

            // Execute
            Assert.ThrowsDelegateWithReturn method = () => repository.GetById(Guid.NewGuid());

            // Assert
            var result = Assert.Throws<InvalidOperationException>(method);
        }

        [Fact]
        public void GetById_ItemExists_ExpectedResult()
        {
            // Setup
            var expectedResult = new Mock<IMaterialType>(MockBehavior.Strict);

            var id = Guid.NewGuid();
            var nameStringResourceId = Guid.NewGuid();

            var factory = new Mock<IMaterialTypeFactory>(MockBehavior.Strict);
            factory
                .Setup(x => x.Create(
                    id,
                    nameStringResourceId))
                .Returns(expectedResult.Object);

            var repository = MaterialTypeRepository.Create(
                Database,
                factory.Object);

            CreateMaterialType(
                id,
                nameStringResourceId);

            // Execute
            var result = repository.GetById(id);

            // Assert
            Assert.Equal(expectedResult.Object, result);

            factory.Verify(
                x => x.Create(
                    It.IsAny<Guid>(),
                    It.IsAny<Guid>()),
                Times.Once);
        }

        [Fact]
        public void RemoveById_ItemExists_RemovedFromDatabase()
        {
            // Setup
            var factory = new Mock<IMaterialTypeFactory>(MockBehavior.Strict);
            
            var repository = MaterialTypeRepository.Create(
                Database,
                factory.Object);

            var id = Guid.NewGuid();
            CreateMaterialType(
                id,
                Guid.NewGuid());

            // Execute
            repository.RemoveById(id);

            // Assert
            using (var command = Database.CreateCommand("SELECT * FROM MaterialTypes"))
            {
                using (var reader = command.ExecuteReader())
                {
                    Assert.False(reader.Read(), "Not expecting any rows.");
                }
            }
        }

        [Fact]
        public void Add_ValidItem_AddedToDatabase()
        {
            // Setup
            var id = Guid.NewGuid();
            var nameStringResourceId = Guid.NewGuid();

            var createdEntry = new Mock<IMaterialType>(MockBehavior.Strict);

            var factory = new Mock<IMaterialTypeFactory>(MockBehavior.Strict);
            factory
                .Setup(x => x.Create(id, nameStringResourceId))
                .Returns(createdEntry.Object);

            var repository = MaterialTypeRepository.Create(
                Database,
                factory.Object);

            // Execute
            var result = repository.Add(
                id, 
                nameStringResourceId);

            // Assert
            Assert.Equal(createdEntry.Object, result);

            using (var command = Database.CreateCommand("SELECT * FROM MaterialTypes"))
            {
                using (var reader = command.ExecuteReader())
                {
                    Assert.True(reader.Read(), "Expecting to read one row.");
                    Assert.False(reader.Read(), "Not expecting additional rows.");
                }
            }

            factory.Verify(x => x.Create(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
        }

        private void CreateMaterialType(
            Guid id,
            Guid nameStringResourceId)
        {
            var namedParameters = new Dictionary<string, object>()
            {
                { "Id", id },
                { "NameStringResourceId", nameStringResourceId },
            };

            using (var command = Database.CreateCommand(
                @"
                INSERT INTO
                    MaterialTypes
                (
                    Id,
                    NameStringResourceId
                )
                VALUES
                (
                    @Id,
                    @NameStringResourceId
                )
                ;",
                namedParameters))
            {
                command.ExecuteNonQuery();
            }
        }
        #endregion
    }
}