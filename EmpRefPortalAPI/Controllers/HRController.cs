using Data_Access_Layer;
using Data_Access_Layer.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmpRefPortalAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class HRController : Controller
    {
        private readonly HRDataLayer _dal;

        public HRController(HRDataLayer HRDataLayer)
        {
            _dal = HRDataLayer;
        }
        //Referrals

        [HttpGet(Name = "HRGetReferrals")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IEnumerable<ReferralDTO> GetReferrals()
        {
            return _dal.GetReferrals();
        }

        [HttpGet("{id:int}", Name = "HRGetReferral")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ReferralDTO> GetReferral(int id)
        {
            return _dal.GetReferral(id);
        }

        [HttpPost(Name = "HRCreateReferral")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ReferralDTO> CreateReferral(ReferralDTO referral)
        {
            if(referral == null)
            {
                return BadRequest();
            }
            var v =  _dal.CreateReferral(referral);
            if(v==null)
            {
                return Conflict();
            }
            return v;
        }

        [HttpPost(Name = "HREditReferral")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ReferralDTO> EditReferral(ReferralDTO referral)
        {
            if(referral == null)
            {
                return BadRequest();
            }
            return _dal.EditReferral(referral);
        }

        [HttpDelete("{id:int}", Name = "DeleteReferral")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ReferralDTO> DeleteReferral(int id)
        {
            return _dal.DeleteReferral(id);
        }
    }                                                                                       
}
