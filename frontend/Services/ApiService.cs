using System.Text;
using System.Text.Json;
using frontend.Models;
using Microsoft.AspNetCore.Mvc;

namespace frontend.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _baseUrl;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;

            // Configure HttpClient
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            // Set base address and default headers
            _baseUrl = "https://localhost:7005/api/";
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<(bool success, UserResponse user, string error)> Login(LoginViewModel model)
        {
            try
            {
                var loginData = new { username = model.Username, password = model.Password };
                var content = new StringContent(
                    JsonSerializer.Serialize(loginData),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("account/login", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (response.IsSuccessStatusCode)
                {
                    var loginResult = JsonSerializer.Deserialize<UserResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (loginResult != null)
                    {
                        // Store user information in session
                        _httpContextAccessor.HttpContext?.Session.SetString("JWTToken", loginResult.Token);
                        _httpContextAccessor.HttpContext?.Session.SetString("UserName", loginResult.UserName);
                        _httpContextAccessor.HttpContext?.Session.SetString("UserEmail", loginResult.Email);
                        _httpContextAccessor.HttpContext?.Session.SetString("UserId", loginResult.Id);

                        // Create AppUserViewModel from login response
                        var appUser = new AppUserViewModel
                        {
                            Id = loginResult.Id,
                            UserName = loginResult.UserName,
                            Email = loginResult.Email,
                            Token = loginResult.Token
                        };

                        // Store the AppUserViewModel in session as JSON
                        _httpContextAccessor.HttpContext?.Session.SetString("CurrentUser", 
                            JsonSerializer.Serialize(appUser));
                    }

                    return (true, loginResult, null);
                }

                return (false, null, responseContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                return (false, null, "An error occurred while connecting to the server. Please try again later.");
            }
        }

        public async Task<(bool success, UserResponse user, string error)> Register(RegisterViewModel model)
        {
            try
            {
                var registerData = new 
                { 
                    userName = model.UserName,
                    email = model.Email,
                    password = model.Password
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(registerData),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("account/register", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                if (response.IsSuccessStatusCode)
                {
                    var registerResult = JsonSerializer.Deserialize<UserResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return (true, registerResult, null);
                }

                return (false, null, responseContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration error: {ex.Message}");
                return (false, null, "An error occurred during registration. Please try again later.");
            }
        }

        public async Task<(bool success, List<PortfolioListViewModel> stocks, string error)> GetUserPortfolio()
        {
            try
            {
                AddAuthenticationHeader();
                var response = await _httpClient.GetAsync("portfolio");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var stocks = JsonSerializer.Deserialize<List<PortfolioListViewModel>>(responseContent, options);

                    if (stocks == null)
                    {
                        return (false, null, "Failed to deserialize portfolio data.");
                    }

                    return (true, stocks, null);
                }

                return (false, null, responseContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get portfolio error: {ex.Message}");
                return (false, null, "An error occurred while fetching your portfolio. Please try again later.");
            }
        }

        public async Task<(bool success, string error)> AddToPortfolio(string symbol)
        {
            try
            {
                AddAuthenticationHeader();
                var response = await _httpClient.PostAsync($"portfolio?symbol={symbol}", null);
                var responseContent = await response.Content.ReadAsStringAsync();

                return (response.IsSuccessStatusCode, response.IsSuccessStatusCode ? null : responseContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Add to portfolio error: {ex.Message}");
                return (false, "An error occurred while adding the stock to your portfolio.");
            }
        }

        public async Task<(bool success, string error)> RemoveFromPortfolio(string symbol)
        {
            try
            {
                AddAuthenticationHeader();
                var response = await _httpClient.DeleteAsync($"portfolio?symbol={symbol}");
                var responseContent = await response.Content.ReadAsStringAsync();

                return (response.IsSuccessStatusCode, response.IsSuccessStatusCode ? null : responseContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Remove from portfolio error: {ex.Message}");
                return (false, "An error occurred while removing the stock from your portfolio.");
            }
        }

        public async Task<(bool success, List<StockViewModel> stocks, string error)> GetAllStocks()
        {
            try
            {
                AddAuthenticationHeader();
                var response = await _httpClient.GetAsync("stock");
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var stocks = JsonSerializer.Deserialize<List<StockViewModel>>(responseContent, options);
                    return (true, stocks ?? new List<StockViewModel>(), null);
                }

                return (false, null, responseContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get stocks error: {ex.Message}");
                return (false, null, "An error occurred while fetching stocks. Please try again later.");
            }
        }

        public async Task<(bool success, StockViewModel stock, string error)> GetStockById(int id)
        {
            try
            {
                AddAuthenticationHeader();
                var response = await _httpClient.GetAsync($"stock/{id}");
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"GetStockById Response: {responseContent}");

                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var stockDto = await response.Content.ReadFromJsonAsync<StockViewModel>(options);
                    if (stockDto == null)
                    {
                        return (false, null, "Failed to deserialize stock data");
                    }

                    return (true, stockDto, null);
                }

                return (false, null, responseContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Get stock error: {ex.Message}");
                return (false, null, "An error occurred while fetching the stock. Please try again later.");
            }
        }

        public async Task<(bool success, string error)> AddComment(string symbol, string title, string content)
        {
            try
            {
                AddAuthenticationHeader();
                var commentData = new { title = title, content = content };
                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(commentData),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync($"comment/{symbol}", jsonContent);
                var responseContent = await response.Content.ReadAsStringAsync();

                return (response.IsSuccessStatusCode, response.IsSuccessStatusCode ? null : responseContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Add comment error: {ex.Message}");
                return (false, "An error occurred while adding your comment. Please try again later.");
            }
        }

        // Helper method to add JWT token to requests
        public void AddAuthenticationHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWTToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        // Add helper method to get current user
        public AppUserViewModel GetCurrentUser()
        {
            var userJson = _httpContextAccessor.HttpContext?.Session.GetString("CurrentUser");
            if (!string.IsNullOrEmpty(userJson))
            {
                return JsonSerializer.Deserialize<AppUserViewModel>(userJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new AppUserViewModel();
            }
            return new AppUserViewModel();
        }
    }

    public class UserResponse
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }

    // Update Stocto clas
}