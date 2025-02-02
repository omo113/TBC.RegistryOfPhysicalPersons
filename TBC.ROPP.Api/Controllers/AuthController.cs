using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TBC.ROPP.Api.Abstractions;
using TBC.ROPP.Application.Models.Auth;
using TBC.ROPP.Application.Services.Abstractions;
using TBC.ROPP.Domain.IdentityEntities;
using TBC.ROPP.Infrastructure.UnitOfWork.Abstractions;
using TBC.ROPP.Shared.ApplicationInfrastructure;

namespace TBC.ROPP.Api.Controllers;


[Route("api/auth")]
public class AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
    : ApiControllerBase
{

    [HttpPost("sign-in")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn([FromBody] LoginModel loginModel)
    {
        var user = await userManager.FindByNameAsync(loginModel.Username);
        if (user == null || !await userManager.CheckPasswordAsync(user, loginModel.Password)) return Unauthorized("Unauthorized");

        var jwtToken = await tokenService.GenerateJwtToken(user);
        var refreshToken = tokenService.GenerateRefreshToken();

        await unitOfWork.SaveAsync();

        var result = new ApplicationResult<OkObjectResult, BadRequestObjectResult>(Ok(new LoginDto(jwtToken, refreshToken)));
        return result.Match(token => token, errors => errors as IActionResult);
    }
}