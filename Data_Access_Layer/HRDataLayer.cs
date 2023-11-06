using Business_Logic_Layer.Exceptions;
using Data_Access_Layer.Data;
using Data_Access_Layer.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Data_Access_Layer
{
    public class HRDataLayer : Controller
    {
        private readonly ApplicationDbContext _db;

        //This calls the ApplicationDbContext for doing database functions
        public HRDataLayer(ApplicationDbContext db)
        {
            _db = db;
        }

        //This calls all the referrals as a list
        public IEnumerable<ReferralDTO> GetReferrals()
        {
            return _db.Referrals.ToList();
        }

        //Gets a referral based on the Id
        public ActionResult<ReferralDTO> GetReferral(int id)
        {
            if (id == 0 || id == null)
            {
                throw new BadRequestException("Invalid ID");
            }
            var referral = _db.Referrals.FirstOrDefault(o => o.Id == id);

            if (referral == null)
            {
                throw new NotFoundException("Referral not found");
            }
            return referral;
        }

        //Creates the referral based on the user input
        public ActionResult<ReferralDTO> CreateReferral(ReferralDTO referral)
        {
            var refer = _db.Referrals.FirstOrDefault(o => o.CandidateName == referral.CandidateName && o.ForRole == referral.ForRole);

            if (refer != null)
            {
                // A record with the same candidate name and role already exists, handle it as needed (e.g., return a response or handle the situation differently).
                // You can return an appropriate response or handle the case differently based on your application's requirements.
                throw new ConflictException("This referral already exists, pleas try again!");
            }

            //Create a model of ReferralDTO type to store the user's input
            ReferralDTO model = new ReferralDTO
            {
                ForRole = referral.ForRole,
                CandidateName = referral.CandidateName,
                CandidateContact = referral.CandidateContact,
                RefEmployee = referral.RefEmployee
            };

            if(model == null)
            {
                throw new BadRequestException("Please enter all the data and try again");
            }

            _db.Referrals.Add(model); // Adds the model to the Referrals table
            _db.SaveChanges();

            // Find the relevant opening based on the role name
            var opening = _db.Openings.FirstOrDefault(o => o.RoleName == model.ForRole);

            //This is done to update the list of referrals who have applied for the job, only HR can view this
            if (opening != null)
            {
                //If there is no record
                if (string.IsNullOrEmpty(opening.Applied))
                {
                    opening.Applied = referral.CandidateName;
                }
                //If there is, it will add it to the string seperating it with a ", "
                else
                {
                    opening.Applied += ", " + referral.CandidateName;
                }

                _db.Update(opening);
                _db.SaveChanges();
            }
            return model;
        }

        //This method edits the selected referral and updates it 
        public ActionResult<ReferralDTO> EditReferral(ReferralDTO referral)
        {
            if(referral == null)
            {
                throw new BadRequestException("Please enter all the data before submitting");
            }

            //Finds the data to be updated based on the Id
            var model = _db.Referrals.FirstOrDefault(o => o.Id == referral.Id);

            if (model == null) 
            {
                throw new NotFoundException("This referral does not exist, please try again");
            }

            var open = _db.Openings.FirstOrDefault(o => o.RoleName == model.ForRole);

            string s1 = referral.CandidateName;
            open.Applied = open.Applied.Replace(referral.CandidateName.Trim(), s1);
            _db.Openings.Update(open);
            _db.SaveChanges();
            
            model.Id = referral.Id;
            model.ForRole = referral.ForRole;
            model.CandidateName = referral.CandidateName;
            model.RefEmployee = referral.RefEmployee;

            _db.Update(model); //Updates the model
            _db.SaveChanges();
            return model;
        }

        //Deletes the selected referral
        public ActionResult<ReferralDTO> DeleteReferral(int id)
        {
            //Finds the entry to be deleted based on the Id
            var model = _db.Referrals.FirstOrDefault(o => o.Id == id);

            if (model == null)
            {
                throw new NotFoundException("This referral does not exist, please try again");
            }

            var open = _db.Openings.FirstOrDefault(o => o.RoleName == model.ForRole);

            string s1 = model.CandidateName;
            open.Applied = string.Join(", ", open.Applied.Split(", ").Where(s => s != s1));

            _db.Openings.Update(open);

            _db.Remove(model);
            _db.SaveChanges();
            return model; 
        }
    }
}