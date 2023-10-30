using EmpRefPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EmpRefPortal.Controllers
{
    public class AdminController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7004/api");

        private readonly HttpClient _httpClient;

        public AdminController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        [HttpGet]
        public async Task<IActionResult> AdminOpenings()
        {
            List<OpeningDTO> openings = new List<OpeningDTO>();

            var handler = new HttpClientHandler
            {
                UseCookies = true,
                UseDefaultCredentials = false
            };

            var client = new HttpClient(handler);

            var authCookie = Request.Cookies["UserToken"];

            if (!string.IsNullOrEmpty(authCookie))
            {
                // Set the "Authorization" header with the bearer token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authCookie);

                HttpResponseMessage response = await client.GetAsync(_httpClient.BaseAddress + "/Admin/GetOpenings");

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    openings = JsonConvert.DeserializeObject<List<OpeningDTO>>(data);

                    //var roles = openings.Select(o => o.RoleName).ToList();
                    //ViewBag.Roles = roles;

                    return View(openings);
                }
            }

            return View();
        }



        [HttpGet]
        public async Task<IActionResult> GetOpening(int id)
        {
            List<OpeningDTO> opening = new List<OpeningDTO>();
            string apiUrl = $"{_httpClient.BaseAddress}/Admin/GetOpening?id={id}";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                opening = JsonConvert.DeserializeObject<List<OpeningDTO>>(data);
                return View(opening);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult CreateOpening()
        {
            var initialModel = new OpeningDTO();
            return View(initialModel);
        }

        public async Task<IActionResult> CreateOpening(OpeningDTO obj)
        {

                if (obj == null)
                {
                    ModelState.AddModelError("CustomError", "Please enter valid data");
                    return View("CreateOpening", obj);
                }

            var handler = new HttpClientHandler
            {
                UseCookies = true,
                UseDefaultCredentials = false
            };

            var client = new HttpClient(handler);

            var authCookie = Request.Cookies["UserToken"];

            if (!string.IsNullOrEmpty(authCookie))
            {
                // Set the "Authorization" header with the bearer token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authCookie);
            }

                var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(_httpClient.BaseAddress + "/Admin/CreateOpening", content);

                if (response.IsSuccessStatusCode)
                {
                TempData["created"] = "The entry has be successfully created";
                    return RedirectToAction("AdminOpenings");
                }

            else
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    TempData["createBadReq"] = "Please check the entered data and try again";
                    return View("CreateOpening", obj);
                }
                else if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    TempData["createConflict"] = "This role already exists";
                    return View("CreateOpening", obj);
                }
                else
                {
                    TempData["error"] = "Please try again, there seems to be some issues with the API";
                    return View("AdminOpenings");
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> EditOpening(int? id)
        {
            OpeningDTO opening = new OpeningDTO();
            string apiUrl = $"{_httpClient.BaseAddress}/Admin/GetOpening/{id}";

            var handler = new HttpClientHandler
            {
                UseCookies = true,
                UseDefaultCredentials = false
            };

            var client = new HttpClient(handler);

            var authCookie = Request.Cookies["UserToken"];

            if (!string.IsNullOrEmpty(authCookie))
            {
                // Set the "Authorization" header with the bearer token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authCookie);
            }

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                opening = JsonConvert.DeserializeObject<OpeningDTO>(data);
                return View(opening);
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditOpening(OpeningDTO obj)
        {
            if (obj == null)
            {
                ModelState.AddModelError("CustomError", "Please enter valid data");
                // Pass the obj parameter to the view to maintain user input in case of errors.
                return View();
            }

            var handler = new HttpClientHandler
            {
                UseCookies = true,
                UseDefaultCredentials = false
            };

            var client = new HttpClient(handler);

            var authCookie = Request.Cookies["UserToken"];

            if (!string.IsNullOrEmpty(authCookie))
            {
                // Set the "Authorization" header with the bearer token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authCookie);
            }

            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(_httpClient.BaseAddress + "/Admin/EditOpening", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["edited"] = "The data has been edited";
                return RedirectToAction("AdminOpenings");
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    TempData["editBadReq"] = "Please check the data and try again";
                    return View("EditOpening", obj);
                }
                else
                {
                    TempData["editError"] = "The API is not working, please try again";
                    return View("AdminOpenings");
                }
            }
        }

        public async Task<ActionResult> DeleteOpening(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError("CustomError", "Please check the Id");
                return View("AdminOpenings");
            }

            var handler = new HttpClientHandler
            {
                UseCookies = true,
                UseDefaultCredentials = false
            };

            var client = new HttpClient(handler);

            var authCookie = Request.Cookies["UserToken"];

            if (!string.IsNullOrEmpty(authCookie))
            {
                // Set the "Authorization" header with the bearer token
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authCookie);
            }

            HttpResponseMessage response = await client.DeleteAsync(_httpClient.BaseAddress + $"/Admin/DeleteOpening/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["deleted"] = "The referral was successfully deleted";
                return RedirectToAction("AdminOpenings");
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                TempData["notFound"] = "The referral does not exist, please try again";
                return View("AdminOpenings");
            }
            else
            {
                TempData["error"] = "The API is not working, please try again";
                return View("AdminOpenings");
            }
        }

    }
}
