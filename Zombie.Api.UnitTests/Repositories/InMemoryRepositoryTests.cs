﻿using Xunit;
using Zombie.Api.Repositories;
using Zombie.Api.Repositories.Enums;

namespace Zombie.Api.UnitTests.Repositories
{
    public class InMemoryRepositoryTests
    {
        [Fact]
        public void GivenEntityWithoutId_WhenInsertNew_ThenEntityAdded_AndEntityWithIdReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            var entity = new TestEntity
            {
                Name = "Foo",
                Value = 101
            };

            // Act
            var result = sut.InsertNew(entity);

            // Assert
            Assert.Equal(Status.Success, result.Status);
            Assert.False(string.IsNullOrEmpty(result.Value?.Id));
        }

        [Fact]
        public void GivenEntityWithId_WhenInsertNew_ThenEntityAdded_AndEntityWithNewIdReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            var initialId = Guid.NewGuid().ToString();
            var entity = new TestEntity
            {
                Id = initialId,
                Name = "Foo",
                Value = 101
            };

            // Act
            var result = sut.InsertNew(entity);

            // Assert
            Assert.Equal(Status.Success, result.Status);
            Assert.NotEqual(initialId, result?.Value?.Id);
            Assert.False(string.IsNullOrEmpty(result?.Value?.Id));
        }

        [Fact]
        public void GivenEntityId_AndEntityExists_WhenGet_ThenEntityReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            var entity = new TestEntity
            {
                Name = "Foo",
                Value = 101
            };
            var inserted = sut.InsertNew(entity);

            // Act
            var result = sut.Get(inserted.Value!.Id);

            // Assert
            Assert.Equal(entity, result.Value);
        }

        [Fact]
        public void GivenEntityId_AndEntityNotExists_WhenGet_ThenNullReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();

            // Act
            var result = sut.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GivenEntityId_AndEntityExists_WhenDelete_ThenEntityRemoved_AndTrueReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            var entity = new TestEntity
            {
                Name = "Foo",
                Value = 101
            };
            var inserted = sut.InsertNew(entity);

            // Act
            var result = sut.Delete(inserted.Value!.Id);

            // Assert
            Assert.True(result);
            Assert.Equal(Status.NotFound, sut.Get(inserted.Value!.Id).Status);
        }

        [Fact]
        public void GivenEntity_AndEntityExists_WhenDelete_ThenEntityRemoved_AndTrueReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            var entity = new TestEntity
            {
                Name = "Foo",
                Value = 101
            };
            var inserted = sut.InsertNew(entity);

            // Act
            var result = sut.Delete(inserted.Value!);

            // Assert
            Assert.True(result);
            Assert.Equal(Status.Success, sut.Get(inserted.Value!.Id).Status);
        }

        [Fact]
        public void GivenEntityWithoutId_WhenDelete_ThenArgumentExceptionThrown()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            var entity = new TestEntity
            {
                Name = "Foo",
                Value = 101
            };

            // Act & Assert
            Assert.ThrowsAny<ArgumentException>(() =>
            {
                var result = sut.Delete(entity);
            });
        }

        [Fact]
        public void GivenEntity_AndEntityExists_WhenUpdate_ThenEntityUpdated_AndEntityReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            var entity = new TestEntity
            {
                Name = "Foo",
                Value = 101
            };
            var inserted = sut.InsertNew(entity)!;
            var updated = new TestEntity
            {
                Id = inserted.Value!.Id,
                Name = "Bar",
                Value = 202
            };

            // Act
            var result = sut.Update(updated);

            // Assert
            Assert.NotNull(result);
            var check = sut.Get(inserted.Value!.Id);
            Assert.Equal(Status.Success, check.Status);
            Assert.Equal(updated.Name, check.Value!.Name);
            Assert.Equal(updated.Value, check.Value!.Value);
        }

        [Fact]
        public void GivenEntity_AndEntityNotExists_WhenUpdate_ThenNullReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            var entity = new TestEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Foo",
                Value = 101
            };

            // Act
            var result = sut.Update(entity);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GivenEntityWithoutId_WhenUpdate_ThenArgumentExceptionThrown()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            var entity = new TestEntity
            {
                Name = "Foo",
                Value = 101
            };

            // Act & Assert
            Assert.ThrowsAny<ArgumentException>(() =>
            {
                var result = sut.Update(entity);
            });
        }

        [Fact]
        public void GivenSeveralEntitiesExist_AndFilter_WhenGet_ThenCorrectEntitiesReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            sut.InsertNew(new TestEntity
            {
                Name = "Brown",
                Value = 0
            });
            sut.InsertNew(new TestEntity
            {
                Name = "Orange",
                Value = 1
            });
            sut.InsertNew(new TestEntity
            {
                Name = "Blue",
                Value = 2
            });
            sut.InsertNew(new TestEntity
            {
                Name = "Red",
                Value = 3
            });
            sut.InsertNew(new TestEntity
            {
                Name = "Yellow",
                Value = 4
            });

            // Act
            var results1 = sut.Get<int>(x => x.Value > 2, y => y.Value);
            var results2 = sut.Get<int>(x => x.Name.StartsWith('B'), y => y.Value);

            // Assert
            Assert.Equal(2, results1.Count());
            Assert.Equal("Red", results1.First().Name);
            Assert.Equal("Yellow", results1.Last().Name);

            Assert.Equal(2, results2.Count());
            Assert.Equal("Brown", results2.First().Name);
            Assert.Equal("Blue", results2.Last().Name);
        }

        [Fact]
        public void GivenSeveralEntitiesExist_AndNoFilter_WhenGet_ThenAllEntitiesReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            sut.InsertNew(new TestEntity
            {
                Name = "Brown",
                Value = 0
            });
            sut.InsertNew(new TestEntity
            {
                Name = "Orange",
                Value = 1
            });
            sut.InsertNew(new TestEntity
            {
                Name = "Blue",
                Value = 2
            });
            sut.InsertNew(new TestEntity
            {
                Name = "Red",
                Value = 3
            });
            sut.InsertNew(new TestEntity
            {
                Name = "Yellow",
                Value = 4
            });

            // Act
            var results = sut.Get<int>();

            // Assert
            Assert.Equal(5, results.Count());
        }
    }
}
