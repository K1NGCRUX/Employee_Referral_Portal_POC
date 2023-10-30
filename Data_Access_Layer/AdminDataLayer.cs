using Data_Access_Layer.Data;
using Data_Access_Layer.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer
{
    public class AdminDataLayer
    {
        private readonly ApplicationDbContext _db;

        public AdminDataLayer(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<OpeningDTO> GetOpenings() 
        { 
            return _db.Openings.ToList();
        }

        public ActionResult<OpeningDTO> GetOpening(int id)
        {
            if(id == 0 || id == null)
            {
                return null;
            }
            var opening = _db.Openings.FirstOrDefault(o => o.Id == id);
            return opening;
        }

        public ActionResult<OpeningDTO> CreateOpening(OpeningDTO opening) 
        {
            var open = _db.Openings.FirstOrDefault(o=>o.RoleName == opening.RoleName);

            if(open !=null)
            {
                return null;
            }

            OpeningDTO model = new()
            {
                Id = opening.Id,
                RoleName = opening.RoleName,
                RoleLocation = opening.RoleLocation,
                MinExp = opening.MinExp,   
                Availability = opening.Availability,
                Description = opening.Description
            };

            _db.Add(model);
            _db.SaveChanges();

            return model;

        }

        public ActionResult<OpeningDTO> EditOpening(OpeningDTO opening)
        {
            var model = _db.Openings.FirstOrDefault(o => o.Id == opening.Id);

            model.Id = opening.Id;
            model.RoleName = opening.RoleName;
            model.RoleLocation = opening.RoleLocation;
            model.MinExp = opening.MinExp;
            model.Availability = opening.Availability;
            model.Description = opening.Description;

            _db.Update(model);
            _db.SaveChanges();
            return model;
        }

        public ActionResult<OpeningDTO> DeleteOpening(int id)
        {
            var model = _db.Openings.FirstOrDefault(o => o.Id == id);
            _db.Remove(model);
            _db.SaveChanges();
            return model;
        }
    }
}