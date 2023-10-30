using Data_Access_Layer.Data;
using Data_Access_Layer.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer
{
    public class HRDataLayer
    {
        private readonly ApplicationDbContext _db;

        public HRDataLayer(ApplicationDbContext db)
        {
            _db = db;
        }

        //Referrals

        public IEnumerable<ReferralDTO> GetReferrals()
        {
            return _db.Referrals.ToList();
        }

        public ActionResult<ReferralDTO> GetReferral(int id)
        {
            if (id == 0 || id == null)
            {
                return null;
            }
            var referral = _db.Referrals.FirstOrDefault(o => o.Id == id);
            return referral;
        }

        public ActionResult<ReferralDTO> CreateReferral(ReferralDTO referral)
        {
            var a = _db.Referrals.FirstOrDefault(o => o.CandidateName == referral.CandidateName);

            if(a != null) 
            {
                return null;
            }

            ReferralDTO model = new()
            {
                Id = referral.Id,
                ForRole = referral.ForRole,
                CandidateName = referral.CandidateName,
                CandidateContact = referral.CandidateContact,
                RefEmployee = referral.RefEmployee,
                Status = referral.Status
            };
            _db.Add(model);
            _db.SaveChanges();

            return model;
        }

        public ActionResult<ReferralDTO> EditReferral(ReferralDTO referral)
        {
            var model = _db.Referrals.FirstOrDefault(o => o.Id == referral.Id);

            model.Id = referral.Id;
            model.ForRole = referral.ForRole;
            model.CandidateName = referral.CandidateName;
            model.RefEmployee = referral.RefEmployee;
            model.Status = referral.Status;

            _db.Update(model);
            _db.SaveChanges();
            return model;
        }
        public ActionResult<ReferralDTO> DeleteReferral(int id)
        {
            var model = _db.Referrals.FirstOrDefault(o => o.Id == id);
            _db.Remove(model);
            _db.SaveChanges();
            return model; 
        }
    }
}