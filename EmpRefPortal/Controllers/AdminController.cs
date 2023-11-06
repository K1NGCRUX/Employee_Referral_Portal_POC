using EmpRefPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

/* As this is the MVC side, this side will contain Uri which will be used to call the database methods in the API. */

namespace EmpRefPortal.Controllers
{
    public class AdminController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7004/api");  //Base address for API's

        //For HTTP Calls and requests
        private readonly HttpClient _httpClient;

        public AdminController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        //Creating a common method to pass the token from the cookie
        private HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler
            {
                UseCookies = true,
                UseDefaultCredentials = false
            };

            var client = new HttpClient(handler);

            var authCookie = Request.Cookies["UserToken"];

            if (!string.IsNullOrEmpty(authCookie))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authCookie);
            }

            return client;
        }

        //To get Openings List
        [HttpGet]
        public async Task<IActionResult> AdminOpenings()
        {
            List<OpeningDTO> openings = new List<OpeningDTO>();

            var client = CreateHttpClient();

            HttpResponseMessage response = await client.GetAsync(_httpClient.BaseAddress + "/Admin/GetOpenings");

            if (response.IsSuccessStatusCode)
            {
                //Getting the role and storing it in a ViewBag to be accessed in the front-end
                string userRole = HttpContext.Request.Cookies["UserRole"];
                ViewBag.UserRole = userRole;

                //Getting the data and deserializing the values for display as it will comes as JSON object format first
                string data = await response.Content.ReadAsStringAsync();
                openings = JsonConvert.DeserializeObject<List<OpeningDTO>>(data);

                return View(openings);
            }
            else
            {
                TempData["OpeningsServer"] = "There was an issue in fetching the data from the server";
                return RedirectToAction("Index", "Home");
            }
        }

        //This method will display the form
        public IActionResult CreateOpening()
        {
            var model = new OpeningDTO();
            return View(model);
        }

        //THis method will push the data into the database
        [HttpPost]
        public async Task<IActionResult> CreateOpening(OpeningDTO opening)
        {
            var client = CreateHttpClient();

            var content = new StringContent(JsonConvert.SerializeObject(opening), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(_httpClient.BaseAddress + "/Admin/CreateOpening", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["created"] = "The entry has be successfully created";
                return RedirectToAction("AdminOpenings");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                TempData["createBadReq"] = "Please check the entered data and try again";
                return View("CreateOpening", opening);
            }
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                TempData["createConflict"] = "This role already exists";
                return View("CreateOpening", opening);
            }
            else
            {
                TempData["error"] = "Please try again, there seems to be some issues with the API";
                return View("AdminOpenings");
            }
        }

        //Opening the Edit view with the current opening data
        [HttpGet]
        public async Task<ActionResult> EditOpening(int? id)
        {
            var client = CreateHttpClient();

            OpeningDTO opening = new OpeningDTO();

            string apiUrl = $"{_httpClient.BaseAddress}/Admin/GetOpening/{id}"; //Setting the uri to fetch the data for edit

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                opening = JsonConvert.DeserializeObject<OpeningDTO>(data);
                return View(opening);// Displaying the view with the edit data fetched with the Id
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    TempData["NotFound"] = "The particular entry does not exist";
                    return View("AdminOpenings");
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    TempData["BadReq"] = "There is an issue wiht the API try again";
                    return View("AdminOpenings");
                }
            }
            TempData["Server"] = "There is an issue conecting with the Database";
            return View("AdminOpenings");
        }

        //Pushing the new edited data into the database
        [HttpPost]
        public async Task<ActionResult> EditOpening(OpeningDTO obj)
        {
            var client = CreateHttpClient();

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

        //Method to delete the item selected
        public async Task<ActionResult> DeleteOpening(int? id)
        {
            var client = CreateHttpClient();

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
                TempData["deleteError"] = "The API is not working, please try again";
                return View("AdminOpenings");
            }
        }
    }
}

