using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MyWebApi.Controllers;
using MyWebApi.DTOs;
using MyWebApi.Entities;
using MyWebApi.Repositories;

namespace MyWebApi.Tests
{
    public class ItemsControllerTests
    {
        private readonly Mock<ILogger<ItemsController>> loggerStub = new();
        private readonly Mock<IItemsRepository> repositoryStub = new();
        private readonly Random rnd = new();

        [Fact]
        public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
        {
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync((Item)null);
            
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            var result = await controller.GetItemAsync(Guid.NewGuid());

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
        {
            var expectedItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync(expectedItem);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            var result = await controller.GetItemAsync(Guid.NewGuid());

            result.Value.Should().BeEquivalentTo(expectedItem, options => options.ComparingByMembers<Item>());
        }

        [Fact]
        public async Task GetItemsAsync_WithExistingItems_ReturnsAllItems()
        {
            var expectedItems = new[] { CreateRandomItem(), CreateRandomItem(), CreateRandomItem(), CreateRandomItem(), CreateRandomItem() };
            repositoryStub.Setup(repo => repo.GetItemsAsync()).ReturnsAsync(expectedItems);
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            var actualItems = await controller.GetItemsAsync();

            actualItems.Should().BeEquivalentTo(expectedItems, options => options.ComparingByMembers<Item>());
        }


        [Fact]
        public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
        {
            var itemToCreate = new CreateItemDTO()
            {
                Name = Guid.NewGuid().ToString(),
                Price = rnd.Next(10000)
            };
            repositoryStub.Setup(repo => repo.CreateItemAsync(It.IsAny<Item>())).ReturnsAsync(true);
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            var result = await controller.CreateItemAsync(itemToCreate);

            var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDTO;
            itemToCreate.Should().BeEquivalentTo(createdItem, options => options.ComparingByMembers<ItemDTO>().ExcludingMissingMembers());
            createdItem.Id.Should().NotBeEmpty();
            createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(1000));
        }

        [Fact]
        public async Task UpdeteItemAsync_WithExistingItem_ReturnsNoContent()
        {
            Item existingItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync(existingItem);

            var itemId = existingItem.Id;
            var itemToUpdate = new UpdateItemDTO()
            {
                Name = Guid.NewGuid().ToString(),
                Price = existingItem.Price + 5
            };

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            var result = await controller.UpdateItemAsync(itemId, itemToUpdate);

            result.Should().BeOfType<NoContentResult>();

        }

        [Fact]
        public async Task DeleteItemAsync_WithExistingItem_ReturnsNoContent()
        {
            Item existingItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync(existingItem);
                      
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            var result = await controller.DeleteItemAsync(existingItem.Id);

            result.Should().BeOfType<NoContentResult>();
        }

        private Item CreateRandomItem()
        {
            return new Item()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Price = rnd.Next(10000),
                CreatedDate = DateTime.UtcNow
            };
        }
    }
}