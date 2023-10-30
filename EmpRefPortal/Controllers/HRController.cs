using Data_Access_Layer.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace EmpRefPortal.Controllers
{
    public class HRController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7004/api");

        private readonly HttpClient _httpClient;

        public HRController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

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

        [HttpGet]
        public async Task<IActionResult> ViewReferrals()
        {
            List<ReferralDTO> referrals = new List<ReferralDTO>();

            using (var client = CreateHttpClient())
            {
                HttpResponseMessage response = client.GetAsync(_httpClient.BaseAddress + "/HR/GetReferrals").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    referrals = JsonConvert.DeserializeObject<List<ReferralDTO>>(data);
                    return View(referrals);
                }
                else
                {
                    return View();

                }
            }
        }

        [HttpGet]
        public IActionResult CreateReferral()
        {
            var model = new ReferralDTO();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReferral(ReferralDTO obj)
        {
            if(obj==null)
                {
                return View("Error");
            }

            using(var client = CreateHttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(_httpClient.BaseAddress + "/HR/CreateReferral", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["created"] = "The referral has be successfully created";
                    return RedirectToAction("ViewReferrals");
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        TempData["createBadReq"] = "Please check the data and try again";
                        return View("CreateReferral", obj);
                    }
                    else if (response.StatusCode == HttpStatusCode.Conflict)
                    {
                        TempData["createConflict"] = "This referral already exists";
                        return View("CreateReferral", obj);
                    }
                    else
                    {
                        TempData["createError"] = "There is a API error, please try again";
                        return View("ViewReferrals");
                    }
                }
            }
        }

        [HttpGet]
        public async Task<ActionResult> EditReferral(int? id)
        {
            ReferralDTO referral = new ReferralDTO();
            string apiUrl = $"{_httpClient.BaseAddress}/HR/GetReferral/{id}";

            using (var client = CreateHttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    referral = JsonConvert.DeserializeObject<ReferralDTO>(data);
                    return View(referral);
                }
                else
                {
                    return View("Error");
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditReferral(ReferralDTO obj)
        {
            if (obj == null)
            {
                return View("Error");
            }

            using (var client = CreateHttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(_httpClient.BaseAddress + "/HR/EditReferral", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["edited"] = "The referral was successfully edited";
                    return RedirectToAction("ViewReferrals");
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        TempData["editBadReq"] = "Please check the data and try again";
                        return View("EditReferral", obj);
                    }
                    else
                    {
                        // Handle other errors
                        return View("ViewReferral");
                    }
                }
            }
        }

        public async Task<ActionResult> DeleteReferral(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError("CustomError", "Please check the Id");
                return View("ViewReferrals");
            }

            using (var client = CreateHttpClient())
            {
                HttpResponseMessage response = await client.DeleteAsync(_httpClient.BaseAddress + $"/HR/DeleteReferral/{id}");

                if (response.IsSuccessStatusCode)
                {
                    TempData["deleted"] = "The referral was successfully deleted";
                    return RedirectToAction("ViewReferrals");
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    TempData["notFound"] = "The specific entry was nt found, please try again";
                    return View("ViewReferrals");
                }
                else
                {
                    TempData["error"] = "There was a problem with the API, please try again";
                    return View("ViewReferrals");
                }
            }
        }
    }
}
