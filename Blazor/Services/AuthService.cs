using Blazor.Interfaces;
using Blazor.Providers;
using Blazored.LocalStorage;
using Domain.Request_Models.Employees;
using Microsoft.AspNetCore.Components.Authorization;
using Shared;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Blazor.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient httpClient,
                           AuthenticationStateProvider authenticationStateProvider,
                           ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
    }

    public async Task<LoginResult> Login(EmployeeLoginModel loginModel)
    {
        var loginAsJson = JsonSerializer.Serialize(loginModel);

        var response = await _httpClient
            .PostAsync("login", new StringContent(loginAsJson, Encoding.UTF8, "application/json"));

        var loginResult = JsonSerializer
            .Deserialize<LoginResult>(
            await response.Content.ReadAsStringAsync(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        if (!response.IsSuccessStatusCode)
        {
            loginResult.Error = "Invalid login attempt";
            return loginResult;
        }

        await _localStorage.SetItemAsync("authToken", loginResult.Token);
        await _localStorage.SetItemAsync("loggedInUserId", loginResult.Id);
        loginResult.Success = response.IsSuccessStatusCode;

        ((ApiAuthenticationStateProvider)_authenticationStateProvider)
            .MarkUserAsAuthenticated(loginModel.UserName);

        _httpClient.DefaultRequestHeaders.Authorization = new
            AuthenticationHeaderValue("Bearer", loginResult.Token);
        
        return loginResult;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("authToken");
        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

}
