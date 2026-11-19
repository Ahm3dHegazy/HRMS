namespace HRMS.Application.Dtos.Authentication;

public sealed record RegisterDto(string UserName, string Email, string Password);
