using Business_Logic_Layer.Exceptions;
using Data_Access_Layer;
using Data_Access_Layer.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmpRefPortalAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly AdminDataLayer _dal;

        //Introducing the Data_Access_Layer to the variable
        public AdminController(AdminDataLayer adminDataLayer)
        {
            _dal = adminDataLayer;
        }

        //Method to get Openings
        [HttpGet(Name = "AdminGetOpenings")]
        public IEnumerable<OpeningDTO> GetOpenings()
        {
            return _dal.GetOpenings(); //Redirceting to the data layer using _dal
        }

        //Getting a particular Id opening
        [HttpGet("{id:int}", Name = "AdminGetOpening")]
        [Authorize(Roles = "HR")] //Using Role based authenticaion, the claim will have the role, based on that will allow to view or not
        public ActionResult<OpeningDTO> GetOpening(int id)
        {
            try
            {
                return _dal.GetOpening(id);
            }
            catch
            {
                throw new ForbiddenException("");
            }
        }

        //Creating a new opening based on the use   r entry
        [HttpPost]
        [Authorize(Roles = "HR")]
        public ActionResult<OpeningDTO> CreateOpening(OpeningDTO opening)
        {
            return _dal.CreateOpening(opening);
        }


        //Editing existing data based on user entry
        [HttpPost(Name = "AdminEditOpening")]
        [Authorize(Roles = "HR")]
        public ActionResult<OpeningDTO> EditOpening(OpeningDTO opening)
        {
            return _dal.EditOpening(opening);
        }

        //Delets opening based on the user's selection
        [HttpDelete("{id:int}", Name = "Delete Opening")]
        [Authorize(Roles = "HR")]
        public ActionResult<OpeningDTO> DeleteOpening(int id)
        {
            return _dal.DeleteOpening(id);
        }
    }
}