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
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Auth/Register", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Successfully Registered";
                return RedirectToAction("Login");
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["RegisterError"] = "This username already exists";
                    return View(obj);
                }
                else
                {
                    TempData["ServerError"] = "There seems to be an issue with the server";
                    return View("Register", obj);
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
                TempData["Success"] = "Login Successful";
                return RedirectToAction("Index", "Home");
            }
            else if(response.StatusCode == HttpStatusCode.NotFound)
            {
                TempData["LoginError"] = "This user does not exist, please check username and password";
                return View("Login", obj);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest) 
            {
                TempData["WrongPass"] = "Wrong Password";
                return View(obj);
            }
            else
            {
                TempData["ServerIssue"] = "There seems to be an issue with the server";
                return View(obj);
            }
        }


        [HttpGet]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("UserToken");
            Response.Cookies.Delete("UserName");
            Response.Cookies.Delete("UserRole");

            TempData["Logout"] = "Logout Successful";
            return RedirectToAction("Login");
        }

    }
}
