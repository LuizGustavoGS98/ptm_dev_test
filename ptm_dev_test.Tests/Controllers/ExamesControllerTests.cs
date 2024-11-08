using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using ptm_dev_test.Controllers;
using ptm_dev_test.Dtos;
using ptm_dev_test.Models;
using ptm_dev_test.Services.IServices;
using System.Security.Claims;

namespace ptm_dev_test.Tests.Controllers
{
    public class ExamesControllerTests
    {
        private readonly Mock<IExamesService> _examesServiceMock;
        private readonly ExamesController _controller;

        public ExamesControllerTests()
        {
            _examesServiceMock = new Mock<IExamesService>();
            _controller = new ExamesController(_examesServiceMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "claim-value"),
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task CreateExame_ValidExame_ReturnsCreatedResult()
        {
            // Arrange
            var exameDto = new ExamesDto { Nome = "Exame1", Idade = 30, Genero = "Masculino" };
            var createdExame = new ExamesModel { Id = 1, Nome = "Exame1", Idade = 30, Genero = "Masculino" };
            _examesServiceMock.Setup(service => service.CreateExameAsync(exameDto))
                              .ReturnsAsync(createdExame);

            // Act
            var result = await _controller.CreateExame(exameDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetExameById", createdResult.ActionName);
            Assert.Equal(createdExame, createdResult.Value);
        }

        [Fact]
        public async Task GetExames_ReturnsOkResult()
        {
            // Arrange
            var exames = new List<ExamesModel> { new ExamesModel { Id = 1, Nome = "Exame1" } };
            _examesServiceMock.Setup(service => service.GetExamesAsync(null, null, null, 1, 10))
                              .ReturnsAsync(new { currentPage = 1, pageSize = 10, totalPages = 1, totalItems = 1, items = exames });

            // Act
            var result = await _controller.GetExames(null, null, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(exames, ((dynamic)okResult.Value).items);
        }

        [Fact]
        public async Task GetExameById_ValidId_ReturnsOkResult()
        {
            // Arrange
            var exame = new ExamesModel { Id = 1, Nome = "Exame1" };
            _examesServiceMock.Setup(service => service.GetExameByIdAsync(1))
                              .ReturnsAsync(exame);

            // Act
            var result = await _controller.GetExameById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(exame, okResult.Value);
        }

        [Fact]
        public async Task CreateExame_DbUpdateException_ReturnsServerError()
        {
            // Arrange
            var exameDto = new ExamesDto();
            _examesServiceMock.Setup(service => service.CreateExameAsync(It.IsAny<ExamesDto>()))
                              .ThrowsAsync(new DbUpdateException("Erro ao tentar salvar o exame no banco de dados."));

            // Act
            var result = await _controller.CreateExame(exameDto);

            // Assert
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, serverErrorResult.StatusCode);

            var mensagemProperty = serverErrorResult.Value.GetType().GetProperty("mensagem");
            var mensagem = mensagemProperty?.GetValue(serverErrorResult.Value, null)?.ToString();

            Assert.Equal("Erro ao tentar salvar o exame no banco de dados.", mensagem);
        }

        [Fact]
        public async Task GetExames_DbUpdateException_ReturnsServerError()
        {
            // Arrange
            _examesServiceMock.Setup(service => service.GetExamesAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                              .ThrowsAsync(new DbUpdateException("Erro ao buscar exames no banco de dados."));

            // Act
            var result = await _controller.GetExames(null, null, null);

            // Assert
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, serverErrorResult.StatusCode);

            var mensagemProperty = serverErrorResult.Value.GetType().GetProperty("mensagem");
            var mensagem = mensagemProperty?.GetValue(serverErrorResult.Value, null)?.ToString();

            Assert.Equal("Erro ao buscar exames no banco de dados.", mensagem);
        }

        [Fact]
        public async Task GetExameById_DbUpdateException_ReturnsServerError()
        {
            // Arrange
            _examesServiceMock.Setup(service => service.GetExameByIdAsync(It.IsAny<long>()))
                              .ThrowsAsync(new DbUpdateException("Erro ao tentar buscar o exame."));

            // Act
            var result = await _controller.GetExameById(1);

            // Assert
            var serverErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, serverErrorResult.StatusCode);

            var mensagemProperty = serverErrorResult.Value.GetType().GetProperty("mensagem");
            var mensagem = mensagemProperty?.GetValue(serverErrorResult.Value, null)?.ToString();

            Assert.Equal("Erro ao tentar buscar o exame.", mensagem);
        }

        [Fact]
        public async Task GetExameById_KeyNotFoundException_ReturnsNotFound()
        {
            // Arrange
            _examesServiceMock.Setup(service => service.GetExameByIdAsync(It.IsAny<long>()))
                              .ThrowsAsync(new KeyNotFoundException("Exame não encontrado."));

            // Act
            var result = await _controller.GetExameById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            var mensagemProperty = notFoundResult.Value.GetType().GetProperty("mensagem");
            var mensagem = mensagemProperty?.GetValue(notFoundResult.Value, null)?.ToString();

            Assert.Equal("Exame não encontrado.", mensagem);
        }
    }
}
