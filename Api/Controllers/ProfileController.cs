using Application.Services.Interface.ProfileService;
using Application.ViewModels.Profile;
using Application.ViewModels.Profile.ChangePassword;
using Application.ViewModels.Teacher;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
public class ProfileController : BaseController
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet("[action]")]
    public async Task<ResponseGetProfileViewModel> Profile()
    {
        return await _profileService.Profile();
    }
    
  

    [HttpPut("[action]")]
    public async Task<bool> ChangePassword([FromBody] ChangePasswordViewModel model)
    {
        return await _profileService.ChangePassword(model);
    }

    [HttpPut("[action]")]
    public async Task<bool> UpdateUser([FromForm] UpdateUserViewModel model)
    {
        return await _profileService.UpdateUser(model);
    }

    [HttpGet("[action]")]
    public Task<IResult> ServeUserImage(string fileName)
    {
        var response = _profileService.GetUserFileImage(fileName).Result;

        return Task.FromResult(Results.File(response.MemoryStream.ToArray(), "application/octet-stream",
            Path.GetFileName(response.FileName)));
    }
}