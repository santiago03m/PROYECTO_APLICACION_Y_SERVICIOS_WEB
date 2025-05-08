using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

public class AuthService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthService(AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<string?> GetUserEmailAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        return user.FindFirst(ClaimTypes.Name)?.Value;
    }

    public async Task<List<string>> GetUserRolesAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        // Buscar el claim "role" que contiene un array de roles
        var rawRoles = user.FindFirst("role")?.Value;

        if (string.IsNullOrEmpty(rawRoles))
            return new List<string>();  // Retornar lista vacía si no se encuentran roles

        try
        {
            // Intentar deserializar el valor de los roles (esperado en formato JSON)
            return JsonSerializer.Deserialize<List<string>>(rawRoles) ?? new List<string>();
        }
        catch
        {
            return new List<string>();  // Retornar lista vacía si ocurre un error en la deserialización
        }
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        return user.Identity?.IsAuthenticated ?? false;
    }

    public async Task<string?> GetUserNameAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        return user.Identity?.Name;
    }
}
