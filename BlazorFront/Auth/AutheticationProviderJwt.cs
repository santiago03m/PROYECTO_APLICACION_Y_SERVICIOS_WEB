using BlazorFront.Helpers;
using BlazorFront.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorFront.Auth
{
    public class AuthenticationProviderJWT : AuthenticationStateProvider, ILoginService
    {
        private readonly IJSRuntime _js;
        private readonly HttpClient _httpClient;
        public static readonly string TOKENKEY = "TOKENKEY";
        private static AuthenticationState Anonimo => new(new ClaimsPrincipal(new ClaimsIdentity()));

        public AuthenticationProviderJWT(IJSRuntime js, HttpClient httpClient)
        {
        _js = js;
        _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _js.GetFromLocalStorage(TOKENKEY);

            if (string.IsNullOrEmpty(token))
            {
                return Anonimo;
            }

            var authState = BuildAuthenticationState(token);
            return await Task.FromResult(authState);
        }


        private AuthenticationState BuildAuthenticationState(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
        }

        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs!.TryGetValue("role", out object? roles);

            if (roles != null)
            {
                if (roles.ToString()!.Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString()!);

                    foreach (var parsedRole in parsedRoles!)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()!));
                }

                keyValuePairs.Remove("role");
            }

            foreach (var kvp in keyValuePairs)
            {
                var key = kvp.Key;
                var value = kvp.Value.ToString() ?? "";

                // Si es unique_name, lo tratamos como ClaimTypes.Name
                if (key == "unique_name")
                {
                    claims.Add(new Claim(ClaimTypes.Name, value));
                }
                else
                {
                    claims.Add(new Claim(key, value));
                }
            }

            foreach (var claim in claims)
            {
                Console.WriteLine(claim.Type + " " + claim.Value);
            }
            return claims;
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        public async Task Login(string token)
        {
            await _js.SetInLocalStorage(TOKENKEY, token);
            var authState = BuildAuthenticationState(token);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            await _js.RemoveItem(TOKENKEY);
            NotifyAuthenticationStateChanged(Task.FromResult(Anonimo));
        }
    }
}