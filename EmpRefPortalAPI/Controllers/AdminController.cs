using Data_Access_Layer;
using Data_Access_Layer.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EmpRefPortalAPI.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "HR")]
    public class AdminController : Controller
    {
        private readonly AdminDataLayer _dal;

        public AdminController(AdminDataLayer adminDataLayer)
        {
            _dal = adminDataLayer;
        }

        //Respones Cotroller

        [HttpGet(Name = "AdminGetOpenings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IEnumerable<OpeningDTO> GetOpenings()
        {
            return _dal.GetOpenings();
        }

        [HttpGet("{id:int}", Name = "AdminGetOpening")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<OpeningDTO> GetOpening(int id)
        {
            return _dal.GetOpening(id);
        }

        [HttpPost(Name = "AdminCreateOpening")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<OpeningDTO> CreateOpening(OpeningDTO opening)
        {
            if (opening == null)
            {
                return BadRequest();
            }
            var v = _dal.CreateOpening(opening);
            if(v == null)
            {
                return Conflict();
            }
            return v;
        }

        [HttpPost(Name = "AdminEditOpening")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<OpeningDTO> EditOpening(OpeningDTO opening)
        {
            if (opening == null)
            {
                return BadRequest();
            }
            var v = _dal.EditOpening(opening);
            return v;
        }
        [HttpDelete("{id:int}", Name = "Delete Opening")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
            public ActionResult<OpeningDTO> DeleteOpening(int id)
            {
                return _dal.DeleteOpening(id);
            }
        }
    }
