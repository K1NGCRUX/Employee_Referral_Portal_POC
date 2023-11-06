using Business_Logic_Layer.Exceptions;
using Data_Access_Layer.Data;
using Data_Access_Layer.Models.DTO;
using Microsoft.AspNetCore.Mvc;
namespace Data_Access_Layer
{
    public class AdminDataLayer
    {
        private readonly ApplicationDbContext _db;

        //Initializing the _db to have the ApplicationDbContext class for entering the data into th respective tables     
        public AdminDataLayer(ApplicationDbContext db)
        {
            _db = db;
        }

        //Calls the Openings as a list
        public IEnumerable<OpeningDTO> GetOpenings()
        {
            var openings = _db.Openings.ToList();
            if(openings == null)
            {
                throw new BadRequestException("There was an issue in fetching data from the database");
            }
            return openings;
        }

        //Gets a specific opening
        public ActionResult<OpeningDTO> GetOpening(int id)
        {
            if (id == 0 || id == null)
            {
                throw new BadRequestException("Invalid Id");
            }
            var opening = _db.Openings.FirstOrDefault(o => o.Id == id);

            if (opening == null)
            {
                throw new NotFoundException("The opening was not found, please check Id");
            }

            return opening;
        }

        //Creates the opening based on the OpeningDTO sructure and user's entry
        public ActionResult<OpeningDTO> CreateOpening(OpeningDTO opening)
        {
            if(opening == null)
            {
                throw new BadRequestException("Please enter all the fields");
            }

            var open = _db.Openings.FirstOrDefault(o => o.RoleName == opening.RoleName); //Check for existing data based on Role

            if (open != null)
            {
                throw new ConflictException("This opening already exists");
            }

            //Creating a variable model of OpeningDTO sructure and storing user entry
            OpeningDTO model = new()
            {
                Id = opening.Id,
                RoleName = opening.RoleName,
                RoleLocation = opening.RoleLocation,
                MinExp = opening.MinExp,
                Availability = opening.Availability,
                Description = opening.Description,
                Applied = ""
        };

            _db.Add(model);  //Add model into the OpeningDTO table
            _db.SaveChanges(); //Save the changes into the database

            return model;

        }

        //Updating existing data and saving in the databases based on user entry
        public ActionResult<OpeningDTO> EditOpening(OpeningDTO opening)
        {
            if(opening == null)
            {
                throw new BadRequestException("Please Enter all the data");
            }

            //Fetches the existing opening to update based on Id
            var model = _db.Openings.FirstOrDefault(o => o.Id == opening.Id);

            if(model == null)
            {
                throw new NotFoundException("The particular Opening does not exist");
            }

            model.Id = opening.Id;
            model.RoleName = opening.RoleName;
            model.RoleLocation = opening.RoleLocation;
            model.MinExp = opening.MinExp;
            model.Availability = opening.Availability;
            model.Description = opening.Description;
            model.Applied = opening.Applied;

            _db.Update(model); //Updates the model in the database
            _db.SaveChanges();
            return model;
        }

        //Deletes the opening from the database based on the Id
        public ActionResult<OpeningDTO> DeleteOpening(int id)
        {
            if(id == 0 || id == null)
            {
                throw new BadRequestException("Invalid Id");
            }

            var model = _db.Openings.FirstOrDefault(o => o.Id == id); //Get the opening based on the Id

            if(model == null) 
            {
                throw new NotFoundException("The opening does not exist, please check the Id and try again");
            }
            _db.Remove(model); //Remove the opening from the database
            _db.SaveChanges(); 
            return model;
        }
    }
}