using Data_Access_Layer.Models;
using Data_Access_Layer.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace EmpRefPortal.Controllers
{
    public class AuthController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7004/api");

        private readonly HttpClient _httpClient;

        public AuthController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        public async Task<ActionResult> Register()
        {
            var model = new UserDTO();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Register(UserDTO obj)
        {
            if (obj == null)
            {
                ModelState.AddModelError("CustomError", "Please enter valid data");
                // Pass the obj parameter to the view to maintain user input in case of errors.
                return View("Register", obj);
            }

            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Auth/Register", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ModelState.AddModelError("CustomError", "API returned a Bad Request.");
                    // Pass the obj parameter to the view to maintain user input in case of errors.
                    return View("Register", obj);
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // Handle 404 Not Found error
                    ModelState.AddModelError("CustomError", "API resource not found.");
                    // Pass the obj parameter to the view to maintain user input in case of errors.
                    return View("Register", obj);
                }
                else if(response.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["RegisterError"] = "This username already exists";
                    return View(obj);
                }
                else
                {
                    // Handle other errors
                    return View("Error");
                }
            }
        }

        public async Task<ActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserDTO obj)
        {
            if (obj == null)
            {
                ModelState.AddModelError("CustomError", "Please enter valid data");
                return View("Login", obj);
            }

            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Auth/Login", content);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddMinutes(30),
                };

                Response.Cookies.Append("UserToken", token, cookieOptions);

                var handler = new JwtSecurityTokenHandler();
                var tokenData = handler.ReadToken(token) as JwtSecurityToken;
                var usernameClaim = tokenData.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name);
                var roleClaim = tokenData.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);

                if (usernameClaim != null && roleClaim != null)
                {
                    string username = usernameClaim.Value;
                    string role = roleClaim.Value;

                    Response.Cookies.Append("UserName", username, cookieOptions);
                    Response.Cookies.Append("UserRole", role, cookieOptions);
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["LoginError"] = "Invalid username or password";
                return View("Login", obj);
            }
        }


        [HttpGet]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("UserToken");
            Response.Cookies.Delete("UserName");
            Response.Cookies.Delete("UserRole");

            return RedirectToAction("Login");
        }

    }
}
