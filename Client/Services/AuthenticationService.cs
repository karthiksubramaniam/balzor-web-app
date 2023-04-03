using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using balzor_web_app.Client.Authentication;
using balzor_web_app.Client.Common;
using balzor_web_app.Shared.Dtos.Login;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace balzor_web_app.Client.Services
{
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigationManager;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public AuthenticationService(
            HttpClient httpClient,
            ILocalStorageService localStorageService,
            NavigationManager navigationManager,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorageService;
            _navigationManager = navigationManager;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> Authenticate(LoginRequestDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("account/login", dto);

            if (response == null || !response.IsSuccessStatusCode)
            {
                return false;
            }

            var responseAsString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<LoginResponseDto>(responseAsString,
                new JsonSerializerOptions(JsonSerializerDefaults.Web));

            await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, result.Token);

            await ((AppAuthenticationStateProvider)this._authenticationStateProvider).StateChangedAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Token);
            _navigationManager.NavigateTo("dashboard");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Logout()
        {
            await _localStorage.RemoveItemAsync(StorageConstants.Local.AuthToken);
            ((AppAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
            return await Task.FromResult(true);
        }
    }
}