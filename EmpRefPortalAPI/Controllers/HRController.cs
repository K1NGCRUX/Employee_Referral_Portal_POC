using Data_Access_Layer;
using Data_Access_Layer.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Business_Logic_Layer.Exceptions;

namespace EmpRefPortalAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HRController : Controller
    {
        private readonly HRDataLayer _dal;

        public HRController(HRDataLayer HRDataLayer)
        {
            _dal = HRDataLayer;
        }
        //Referrals

        [HttpGet(Name = "HRGetReferrals")]
        public IEnumerable<ReferralDTO> GetReferrals()
        {
            return _dal.GetReferrals();
        }

        [HttpGet("{id:int}", Name = "HRGetReferral")]
        public ActionResult<ReferralDTO> GetReferral(int id)
        {
            return _dal.GetReferral(id);
        }

        [HttpPost(Name = "HRCreateReferral")]
        [Authorize(Roles = "User")]
        public ActionResult<ReferralDTO> CreateReferral(ReferralDTO referral)
        {

             return _dal.CreateReferral(referral);
        }

        [HttpPost(Name = "HREditReferral")]
        [Authorize(Roles = "User")]
        public ActionResult<ReferralDTO> EditReferral(ReferralDTO referral)
        {
                return _dal.EditReferral(referral);
        }

        [HttpDelete("{id:int}", Name = "DeleteReferral")]
        [Authorize(Roles = "User")]
        public ActionResult<ReferralDTO> DeleteReferral(int id)
        {
                return _dal.DeleteReferral(id);
        }
    }
}