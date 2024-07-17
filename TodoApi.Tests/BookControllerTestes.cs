using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using TodoApi.DTO;
using TodoApi.Models;
using TodoApi.Repositories;

public class BookControllerTests
{
    private readonly IConfiguration _configuration;

    public BookControllerTests()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(x => x["TokenAuthentication:SecretKey"]).Returns("ojt2024_globalwave_secretkeysample");
        mockConfiguration.SetupGet(x => x["TokenAuthentication:Issuer"]).Returns("ToDo");
        _configuration = mockConfiguration.Object;
    }

    [Fact]
    public async Task GetAllBooks_ReturnsAllBooks()
    {
        // Arrange
        var mockRepo = new Mock<IRepositoryWrapper>(); // Adjust IRepositoryWrapper to match your actual repository interface
        var books = new List<Book>
        {
            new Book { book_id = 1, title = "Book 1", author = "Author 1", description = "Description 1", price = 10.0, category = 1 },
            new Book { book_id = 2, title = "Book 2", author = "Author 2", description = "Description 2", price = 20.0, category = 2 },
            // Add more book instances as needed
        };
        mockRepo.Setup(repo => repo.Book.GetAll<Book>(It.IsAny<string>(), It.IsAny<DynamicParameters>()))
                .ReturnsAsync(books);

        var controller = new BookController(mockRepo.Object, _configuration);

        // Act
        var result = await controller.GetAllBooks();

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedBooks = Assert.IsType<List<Book>>(actionResult.Value);
        Assert.Equal(books.Count, returnedBooks.Count);
    }

    [Fact]
    public async Task GetBookByCategoryName_ReturnsBook()
    {
        // Arrange
        var mockRepo = new Mock<IRepositoryWrapper>();
        var categoryName = "Romance";
        var expectedBook = new GetBookByCategoryNameResponseDTO
        {
            Title = "Book 1",
            Author = "Author 1",
            Description = "Description 1",
            Price = 10.0,
            Category = "Romance"
        };
        mockRepo.Setup(repo => repo.Book.GetBookByCategoryName(categoryName))
                .ReturnsAsync(expectedBook);

        var controller = new BookController(mockRepo.Object, _configuration);

        // Act
        var result = await controller.GetBookByCategoryName(categoryName);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedBook = Assert.IsType<GetBookByCategoryNameResponseDTO>(actionResult.Value);
        Assert.Equal(expectedBook, returnedBook);
    }

    [Fact]
    public async Task GetBookByCategoryName_WithNonExistingCategory_ReturnsNotFoundResult()
    {
        // Arrange
        var mockRepo = new Mock<IRepositoryWrapper>();
        var categoryName = "NonExistingCategory";
        mockRepo.Setup(repo => repo.Book.GetBookByCategoryName(categoryName))
                 .ReturnsAsync((GetBookByCategoryNameResponseDTO?)null);

        // Act
        BookController controller = new BookController(mockRepo.Object, _configuration);
        var result = await controller.GetBookByCategoryName(categoryName);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
}