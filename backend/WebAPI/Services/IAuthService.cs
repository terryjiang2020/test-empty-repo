using ProjectManager.Models;
using ProjectManager.Models.DTOs;

namespace ProjectManager.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string? Token, string? Message)> LoginAsync(UserLoginDto loginDto);
        Task<(bool Success, string? Message)> RegisterAsync(UserRegisterDto registerDto);
    }
}