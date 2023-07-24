using Xunit;
using Zombie.Api.Repositories;
using Zombie.Api.Repositories.Enums;

namespace Zombie.Api.UnitTests.Repositories
{
    public class InMemoryRepositoryTests
    {
        [Fact]
        public void GivenEntityWithoutId_WhenInsert_ThenSuccessReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            var entity = new TestEntity
            {
                Key = "Folder1/Folder2/Pop",
                Name = "Foo",
                Value = 101
            };

            // Act
            var result = sut.Insert(entity);

            // Assert
            Assert.Equal(Status.Success, result.Status);
        }

        [Fact]
        public void GivenEntityId_AndEntityExists_WhenGet_ThenEntityReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            var entity = new TestEntity
            {
                Key = "Folder1/Folder2/pop",
                Name = "Foo",
                Value = 101
            };
            var inserted = sut.Insert(entity);

            // Act
            var result = sut.Get(inserted.Value!.Key);

            // Assert
            Assert.Equal(entity, result.Value);
        }

        [Fact]
        public void GivenEntityId_AndEntityNotExists_WhenGet_ThenNotFoundReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();

            // Act
            var result = sut.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal(Status.NotFound, result.Status);
        }

        [Fact]
        public void GivenEntityId_AndEntityExists_WhenDelete_ThenEntityRemoved_AndTrueReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            var entity = new TestEntity
            {
                Key = "Folder1/Folder2/pop",
                Name = "Foo",
                Value = 101
            };
            var inserted = sut.Insert(entity);

            // Act
            var result = sut.Delete(inserted.Value!.Key);

            // Assert
            Assert.True(result);
            Assert.Equal(Status.NotFound, sut.Get(inserted.Value!.Key).Status);
        }

        [Fact]
        public void GivenEntity_AndEntityExists_WhenDelete_ThenEntityRemoved_AndEntityNotFoundOnSubsequentCheck()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            var entity = new TestEntity
            {
                Key = "Folder1/Folder2/pop",
                Name = "Foo",
                Value = 101
            };
            var inserted = sut.Insert(entity);

            // Act
            var result = sut.Delete(inserted.Value!);

            // Assert
            Assert.True(result);
            Assert.Equal(Status.NotFound, sut.Get(inserted.Value!.Key).Status);
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
                Key = "Folder/Folder2/pop",
                Name = "Foo",
                Value = 101
            };
            var inserted = sut.Insert(entity)!;
            var updated = new TestEntity
            {
                Key = inserted.Value!.Key,
                Name = "Bar",
                Value = 202
            };

            // Act
            var result = sut.Update(updated);

            // Assert
            Assert.NotNull(result);
            var check = sut.Get(inserted.Value!.Key);
            Assert.Equal(Status.Success, check.Status);
            Assert.Equal(updated.Name, check.Value!.Name);
            Assert.Equal(updated.Value, check.Value!.Value);
        }

        [Fact]
        public void GivenEntity_AndEntityNotExists_WhenUpdate_ThenNotFoundReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            var entity = new TestEntity
            {
                Key = "Folder1/Folder2/pop",
                Name = "Foo",
                Value = 101
            };

            // Act
            var result = sut.Update(entity);

            // Assert
            Assert.Equal(Status.NotFound, result.Status);
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
            sut.Insert(new TestEntity
            {
                Key = "pop1",
                Name = "Brown",
                Value = 0
            });
            sut.Insert(new TestEntity
            {
                Key = "pop2",
                Name = "Orange",
                Value = 1
            });
            sut.Insert(new TestEntity
            {
                Key = "pop3",
                Name = "Blue",
                Value = 2
            });
            sut.Insert(new TestEntity
            {
                Key = "pop4",
                Name = "Red",
                Value = 3
            });
            sut.Insert(new TestEntity
            {
                Key = "pop5",
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
            sut.Insert(new TestEntity
            {
                Key = "pop1",
                Name = "Brown",
                Value = 0
            });
            sut.Insert(new TestEntity
            {
                Key = "pop2",
                Name = "Orange",
                Value = 1
            });
            sut.Insert(new TestEntity
            {
                Key = "pop3",
                Name = "Blue",
                Value = 2
            });
            sut.Insert(new TestEntity
            {
                Key = "pop4",
                Name = "Red",
                Value = 3
            });
            sut.Insert(new TestEntity
            {
                Key = "pop5",
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
