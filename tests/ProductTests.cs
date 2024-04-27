using Demo.Api.Models;
using System.Net.Http.Json;

namespace Demo.Api.Tests;

[Collection(nameof(CollectionIntegrationTests))]
public sealed class ProductTests
{
    private readonly IntegrationTestsFactory _factory;

    public ProductTests(IntegrationTestsFactory factory)
        => _factory = factory;


    [Fact]
    public async Task Should_return_201_status_code_and_same_product_added()
    {
        // Arrange
        var product = new ProductRequest(
            "Product 1",
            45,
            10.5);


        // Act
        var idResponse = await _factory.CreateClient()
            .PostAsJsonAsync("/products", product);

        var id = await idResponse.Content.ReadAsAsync<Guid>();
        var productResponse = await _factory.CreateClient()
            .GetAsync($"/products/{id}");


        // Assert
        idResponse.Should().Be201Created();
        productResponse.Should()
            .Be200Ok()
            .And.Satisfy<ProductResponse>(model =>
            {
                model.Name.Should().Be(product.Name);
                model.Quantity.Should().Be(product.Quantity);
                model.Price.Should().Be(product.Price);
            });
    }


    [Fact]
    public async Task Should_return_204_status_code_and_product_updated()
    {
        // Arrange
        var product = new ProductRequest(
            "Product 1",
            45,
            10.5);

        var productUpdated = new ProductRequest(
            "Product 1 Updated",
            451,
            110.5);


        // Act
        var idResponse = await _factory.CreateClient()
            .PostAsJsonAsync("/products", product);
        var id = await idResponse.Content.ReadAsAsync<Guid>();

        var updateResponse = await _factory.CreateClient()
            .PutAsJsonAsync($"/products/{id}", productUpdated);

        var productResponse = await _factory.CreateClient()
            .GetAsync($"/products/{id}");


        // Assert
        updateResponse.Should().Be204NoContent();
        productResponse.Should()
            .Be200Ok()
            .And.Satisfy<ProductResponse>(model =>
            {
                model.Name.Should().Be(productUpdated.Name);
                model.Quantity.Should().Be(productUpdated.Quantity);
                model.Price.Should().Be(productUpdated.Price);
            });
    }

    [Fact]
    public async Task Should_return_200_status_code_and_two_products_added()
    {
        // Arrange
        var product1 = new ProductRequest(
            "Product 1",
            45,
            10.5);

        var product2 = new ProductRequest(
            "Product 2",
            451,
            110.5);


        // Act
        await _factory.CreateClient()
            .PostAsJsonAsync("/products", product1);

        await _factory.CreateClient()
            .PostAsJsonAsync("/products", product2);

        var act = await _factory.CreateClient()
            .GetAsync("/products");


        // Assert
        act.Should()
            .Be200Ok()
            .And.Satisfy<List<ProductResponse>>(model =>
            {
                model.Should().HaveCountGreaterThanOrEqualTo(2);
                model.Should().ContainEquivalentOf(product1);
                model.Should().ContainEquivalentOf(product2);
            });
    }

    [Fact]
    public async Task Should_return_404_status_code_when_get_product_after_delete()
    {
        // Arrange
        var product = new ProductRequest(
            "Product 1",
            45,
            10.5);


        // Act
        var idResponse = await _factory.CreateClient()
            .PostAsJsonAsync("/products", product);
        var id = await idResponse.Content.ReadAsAsync<Guid>();

        var deleteResponse = await _factory.CreateClient()
            .DeleteAsync($"/products/{id}");

        var productResponse = await _factory.CreateClient()
            .GetAsync($"/products/{id}");


        // Assert
        deleteResponse.Should().Be204NoContent();
        productResponse.Should().Be404NotFound();
    }
}
