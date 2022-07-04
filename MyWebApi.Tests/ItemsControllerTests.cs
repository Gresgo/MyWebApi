using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MyWebApi.Controllers;
using MyWebApi.Dtos;
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

            result.Value.Should().BeEquivalentTo(expectedItem);
        }

        [Fact]
        public async Task GetItemsAsync_WithExistingItems_ReturnsAllItems()
        {
            var expectedItems = new[] { CreateRandomItem(), CreateRandomItem(), CreateRandomItem(), CreateRandomItem(), CreateRandomItem() };
            repositoryStub.Setup(repo => repo.GetItemsAsync()).ReturnsAsync(expectedItems);
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            var actualItems = await controller.GetItemsAsync();

            actualItems.Should().BeEquivalentTo(expectedItems);
        }


        [Fact]
        public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
        {
            var itemToCreate = new CreateItemDto(Guid.NewGuid().ToString(), "", rnd.Next(10000));

            repositoryStub.Setup(repo => repo.CreateItemAsync(It.IsAny<Item>())).ReturnsAsync(true);
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            var result = await controller.CreateItemAsync(itemToCreate);

            var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;
            itemToCreate.Should().BeEquivalentTo(createdItem, options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers());
            createdItem.Id.Should().NotBeEmpty();
            createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(1000));
        }

        [Fact]
        public async Task UpdeteItemAsync_WithExistingItem_ReturnsNoContent()
        {
            Item existingItem = CreateRandomItem();
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync(existingItem);

            var itemId = existingItem.Id;
            var itemToUpdate = new UpdateItemDto(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), existingItem.Price + 5);

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

        [Fact]
        public async Task GetItemsAsync_WithMatchingItems_ReturnsMatchingItems()
        {
            var allItems = new[] { 
                new Item() {Name = "Potion"},
                new Item() {Name = "Super potion"},
                new Item() {Name = "Sword"},
                new Item() {Name = "Helmet"},
                new Item() {Name = "Mega potion"}
            };
            var nameToMatch = "Potion";

            repositoryStub.Setup(repo => repo.GetItemsAsync()).ReturnsAsync(allItems);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            IEnumerable<ItemDto> foundItems = await controller.GetItemsAsync(nameToMatch);

            foundItems.Should().OnlyContain(item => item.Name == allItems[0].Name 
            || item.Name == allItems[1].Name
            || item.Name == allItems[4].Name);
        }

        private Item CreateRandomItem()
        {
            return new Item()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Price = rnd.Next(10000),
                CreatedDate = DateTime.UtcNow
            };
        }
    }
}