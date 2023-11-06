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

        //Gets the referrals as a list
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
                    referrals = JsonConvert.DeserializeObject<List<ReferralDTO>>(data); //Deserializing the value for display 
                    return View(referrals);
                }
                else if(response.StatusCode == HttpStatusCode.BadRequest)
                {
                    TempData["BadReq"] = "There seems to be an issue with the uri";
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    TempData["Server"] = "There seems to be an issue with the Server when fetching Openings";
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        //Gets the Referral for view, here we are passing the role applied for as well as the Name of the person referring 
        [HttpGet]
        public IActionResult CreateReferral(string role)
        {
            var model = new ReferralDTO();

            string username = HttpContext.Request.Cookies["Username"];
            TempData["Rolename"] = role; //Temporary storage of the role applyng for
            ViewBag.Username = username; // Stores the username of the person who logged in in a Dynamic store

            return View(model);
        }

        //Creates the referral for the employee
        [HttpPost]
        public async Task<IActionResult> CreateReferral(ReferralDTO obj)
        {
            //If the user has not entered any data it will check here
            if(obj==null)
                {
                return View("Error");
            }

            //Uses the method created above for the token parse in request header
            using(var client = CreateHttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"); //Seralizes the values for entry 

                HttpResponseMessage response = await client.PostAsync(_httpClient.BaseAddress + "/HR/CreateReferral", content); // Sends data to API, stores the respone here

                //If Success, then it will continue, else check for errors
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
                        TempData["createConflict"] = "This user has already applied for this role";
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

        //Gets the edit data to be edited and sends it to the form
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
                    return View(referral); // Sends the particular referral to be edited back to the EditReferral View
                }
                else if(response.StatusCode == HttpStatusCode.NotFound)
                {
                    TempData["NotFoundEdit"] = "The referral you wanted to edit was not found";
                    return View(referral);
                }
                else
                {
                    TempData["ServerEdit"] = "There was an issue connecting with the server";
                    return View();
                }
            }
        }

        //Updates the Edited data back to the database
        [HttpPost]
        public async Task<IActionResult> EditReferral(ReferralDTO obj)
        { 
            //Creates the client for Http requests and responses
            using (var client = CreateHttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"); //

                HttpResponseMessage response = await client.PostAsync(_httpClient.BaseAddress + "/HR/EditReferral", content);
                
                //If response is success it will continue, else check for errors and display
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
                    else if(response.StatusCode == HttpStatusCode.NotFound)
                    {
                        TempData["NotFoundAfter"] = "The particular entry to be edited was not found";
                        return View("ViewReferrals");
                    }
                    else
                    {
                        TempData["AfterEdit"] = "There was an issue with connecting with the server";
                        return View("ViewReferral");
                    }
                }
            }
        }

        //Deletes the particular referral 
        public async Task<ActionResult> DeleteReferral(int? id)
        {
            //Checks if there is no Id
            if (id == null)
            {
                TempData["Idnull"] = "Please check the Id";
                return View("ViewReferrals");
            }

            //Creates client and then sends the request
            using (var client = CreateHttpClient())
            {
                HttpResponseMessage response = await client.DeleteAsync(_httpClient.BaseAddress + $"/HR/DeleteReferral/{id}");

                //If success, it will continue else will check for error
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
