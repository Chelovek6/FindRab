using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using FindRab.Controllers;
using FindRab.DataContext;
using FindRab.models;
using FindRab.Models;
using FindRab.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

public class AccountControllerTests
{
    private DbContextOptions<BDContext> _contextOptions;

    public AccountControllerTests()
    {
        // Set up in-memory database
        _contextOptions = new DbContextOptionsBuilder<BDContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    private AccountController CreateController()
    {
        var context = new BDContext(_contextOptions);

        // Set up HttpContext
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "TestUser")
        }, "mock"));

        var httpContext = new DefaultHttpContext
        {
            User = user
        };

        return new AccountController(context)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            }
        };
    }

    [Fact]
    public async Task Register_Post_ReturnsViewWithModel_WhenModelStateIsInvalid()
    {
        // Arrange
        var model = new RegisterViewModel();
        var controller = CreateController();
        controller.ModelState.AddModelError("Username", "Required");

        // Act
        var result = await controller.Register(model);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var returnedModel = Assert.IsType<RegisterViewModel>(viewResult.Model);
        Assert.Equal(model, returnedModel);
    }

    [Fact]
    public async Task Register_Post_ReturnsViewWithModel_WhenUsernameIsTaken()
    {
        // Arrange
        var model = new RegisterViewModel
        {
            Username = "TestUser",
            Password = "Password123"
        };

        using (var context = new BDContext(_contextOptions))
        {
            context.UserM.Add(new User { Username = "TestUser" });
            await context.SaveChangesAsync();
        }

        var controller = CreateController();

        // Act
        var result = await controller.Register(model);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var returnedModel = Assert.IsType<RegisterViewModel>(viewResult.Model);
        Assert.Equal(model, returnedModel);
        Assert.True(controller.ModelState.ContainsKey("Username"));
    }

    [Fact]
    public async Task Register_Post_AddsUserAndRedirectsToMenu_WhenModelStateIsValid()
    {
        // Arrange
        var model = new RegisterViewModel
        {
            Username = "NewUser",
            Password = "Password123"
        };

        var controller = CreateController();

        // Act
        var result = await controller.Register(model);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        Assert.Equal("Menu", redirectToActionResult.ControllerName);

        using (var context = new BDContext(_contextOptions))
        {
            var user = await context.UserM.FirstOrDefaultAsync(u => u.Username == model.Username);
            Assert.NotNull(user);
        }
    }
}
