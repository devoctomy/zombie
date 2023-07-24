using Xunit;
using Zombie.Api.Repositories;
using Zombie.Api.Repositories.Enums;

namespace Zombie.Api.UnitTests.Repositories
{
    public class InMemoryRepositoryTests
    {
        [Fact]
        public async Task GivenEntityWithoutId_WhenInsert_ThenSuccessReturned()
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
            var result = await sut.Insert(entity);

            // Assert
            Assert.Equal(Status.Success, result.Status);
        }

        [Fact]
        public async Task GivenEntityId_AndEntityExists_WhenGet_ThenEntityReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            var entity = new TestEntity
            {
                Key = "Folder1/Folder2/pop",
                Name = "Foo",
                Value = 101
            };
            var inserted = await sut.Insert(entity);

            // Act
            var result = await sut.Get(inserted.Value!.Key);

            // Assert
            Assert.Equal(entity, result.Value);
        }

        [Fact]
        public async Task GivenEntityId_AndEntityNotExists_WhenGet_ThenNotFoundReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();

            // Act
            var result = await sut.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal(Status.NotFound, result.Status);
        }

        [Fact]
        public async Task GivenEntityId_AndEntityExists_WhenDelete_ThenEntityRemoved_AndTrueReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            var entity = new TestEntity
            {
                Key = "Folder1/Folder2/pop",
                Name = "Foo",
                Value = 101
            };
            var inserted = await sut.Insert(entity);

            // Act
            var result = await sut.Delete(inserted.Value!.Key);

            // Assert
            Assert.True(result);
            Assert.Equal(Status.NotFound, (await sut.Get(inserted.Value!.Key)).Status);
        }

        [Fact]
        public async Task GivenEntity_AndEntityExists_WhenDelete_ThenEntityRemoved_AndEntityNotFoundOnSubsequentCheck()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            var entity = new TestEntity
            {
                Key = "Folder1/Folder2/pop",
                Name = "Foo",
                Value = 101
            };
            var inserted = await sut.Insert(entity);

            // Act
            var result = await sut.Delete(inserted.Value!);

            // Assert
            Assert.True(result);
            Assert.Equal(Status.NotFound, (await sut.Get(inserted.Value!.Key)).Status);
        }

        [Fact]
        public async Task GivenEntityWithoutId_WhenDelete_ThenArgumentExceptionThrown()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            var entity = new TestEntity
            {
                Name = "Foo",
                Value = 101
            };

            // Act & Assert
            await Assert.ThrowsAnyAsync<ArgumentException>(async () =>
            {
                var result = await sut.Delete(entity);
            });
        }

        [Fact]
        public async Task GivenEntity_AndEntityExists_WhenUpdate_ThenEntityUpdated_AndEntityReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            var entity = new TestEntity
            {
                Key = "Folder/Folder2/pop",
                Name = "Foo",
                Value = 101
            };
            var inserted = await sut.Insert(entity)!;
            var updated = new TestEntity
            {
                Key = inserted.Value!.Key,
                Name = "Bar",
                Value = 202
            };

            // Act
            var result = await sut.Update(updated);

            // Assert
            Assert.NotNull(result);
            var check = await sut.Get(inserted.Value!.Key);
            Assert.Equal(Status.Success, check.Status);
            Assert.Equal(updated.Name, check.Value!.Name);
            Assert.Equal(updated.Value, check.Value!.Value);
        }

        [Fact]
        public async Task GivenEntity_AndEntityNotExists_WhenUpdate_ThenNotFoundReturned()
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
            var result = await sut.Update(entity);

            // Assert
            Assert.Equal(Status.NotFound, result.Status);
        }

        [Fact]
        public async Task GivenEntityWithoutId_WhenUpdate_ThenArgumentExceptionThrown()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            var entity = new TestEntity
            {
                Name = "Foo",
                Value = 101
            };

            // Act & Assert
            await Assert.ThrowsAnyAsync<ArgumentException>(async () =>
            {
                var result = await sut.Update(entity);
            });
        }

        [Fact]
        public async Task GivenSeveralEntitiesExist_AndFilter_WhenGet_ThenCorrectEntitiesReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            await sut.Insert(new TestEntity
            {
                Key = "pop1",
                Name = "Brown",
                Value = 0
            });
            await sut.Insert(new TestEntity
            {
                Key = "pop2",
                Name = "Orange",
                Value = 1
            });
            await sut.Insert(new TestEntity
            {
                Key = "pop3",
                Name = "Blue",
                Value = 2
            });
            await sut.Insert(new TestEntity
            {
                Key = "pop4",
                Name = "Red",
                Value = 3
            });
            await sut.Insert(new TestEntity
            {
                Key = "pop5",
                Name = "Yellow",
                Value = 4
            });

            // Act
            var results1 = await sut.Get<int>(x => x.Value > 2, y => y.Value);
            var results2 = await sut.Get<int>(x => x.Name.StartsWith('B'), y => y.Value);

            // Assert
            Assert.Equal(2, results1.Count());
            Assert.Equal("Red", results1.First().Name);
            Assert.Equal("Yellow", results1.Last().Name);

            Assert.Equal(2, results2.Count());
            Assert.Equal("Brown", results2.First().Name);
            Assert.Equal("Blue", results2.Last().Name);
        }

        [Fact]
        public async Task GivenSeveralEntitiesExist_AndNoFilter_WhenGet_ThenAllEntitiesReturned()
        {
            // Arrange
            var sut = new InMemoryRepository<TestEntity>();
            await sut.Insert(new TestEntity
            {
                Key = "pop1",
                Name = "Brown",
                Value = 0
            });
            await sut.Insert(new TestEntity
            {
                Key = "pop2",
                Name = "Orange",
                Value = 1
            });
            await sut.Insert(new TestEntity
            {
                Key = "pop3",
                Name = "Blue",
                Value = 2
            });
            await sut.Insert(new TestEntity
            {
                Key = "pop4",
                Name = "Red",
                Value = 3
            });
            await sut.Insert(new TestEntity
            {
                Key = "pop5",
                Name = "Yellow",
                Value = 4
            });

            // Act
            var results = await sut.Get<int>();

            // Assert
            Assert.Equal(5, results.Count());
        }
    }
}
