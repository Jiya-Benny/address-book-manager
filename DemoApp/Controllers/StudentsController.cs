using DemoApp.Models;
using DemoApp.Models.Entities;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DemoApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public StudentsController(ApplicationDbContext dbContext)   //Constructor of StudentsController
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index(int id)
        {
            var student = dbContext.Students.FirstOrDefault(s => s.UserId == id);
            return View(student);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(AddStudentViewModel viewModel)          //Action for getting form inputs
        {
            if (ModelState.IsValid)
            {
                // Retrieve the UserId from UserRegister (assuming the email is used to identify the user)
                var user = dbContext.UserRegister.FirstOrDefault(u => u.Email == viewModel.Email);

                if(user != null)
                {
                    // Create a new student object
                    var student = new Student
                    {

                        Name = viewModel.Name,
                        Email = viewModel.Email,
                        Phone = viewModel.Phone,
                        DateOfBirth = viewModel.DateOfBirth,
                        Gender = viewModel.Gender,
                        State = viewModel.State,
                        Country = viewModel.Country,
                        UserId = user.UserId,  // Associate UserId from UserRegister table
                    };

                    dbContext.Students.Add(student);
                    dbContext.SaveChanges();

                    return RedirectToAction("Index");
                }
                
                else
                {
                    ModelState.AddModelError(string.Empty, "Incorrect email address");
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public IActionResult List(string sortOrder, string searchString, int pageNumber = 1)      //action for displaying list of employees, and sorting
        {
            // Get the logged-in user's UserId from the claims
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var student = from s in dbContext.Students
                          where s.UserId != userId
                          select s;

            // Search logic
            if (!string.IsNullOrEmpty(searchString)) // Check if searchQuery is not null or empty
            {
                student = from s in student
                          where s.Name.StartsWith(searchString)
                          select s; // Search by name
            }

            // Sorting logic
            switch (sortOrder)
            {
                case "name_desc":
                    student = from s in student     //descending order by name
                              orderby s.Name descending
                              select s;
                    break;
                case "name_asc":
                    student = student.OrderBy(s => s.Name); // ascending order by name
                    break;
                case "email_asc":
                    student = from s in student       //descending order by email
                              orderby s.Email
                              select s;
                    break;
                case "email_desc":
                    student = student.OrderByDescending(s => s.Email);           //ascending order by email
                    break;
                default:
                    break;
            }

            //return View(student.ToList());
            int pageSize = 5; // Number of items per page
            var paginatedStudents = PaginatedResult<Student>.Create(student, pageNumber, pageSize);

            return View(paginatedStudents);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ProfileView(int id)
        {
            var student = (from s in dbContext.Students
                           where s.UserId == id
                           select s).Include(s => s.UserRegister).FirstOrDefault();

            if (student == null)
                return NotFound();

            return View(student);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit()    //action for editing
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));  // Get the user ID from claims

            var student = (from s in dbContext.Students
                           where s.UserId == userId
                           select s).FirstOrDefault();

            if (student == null)
            {
                return NotFound();      //Return 404 if no employee with id is found
            }

            return View(student);      //passing student to edit view
        }

        [HttpPost]
        public IActionResult Edit(Student viewModel)         //action for displaying the edited data in the list
        {
            var student = (from s in dbContext.Students
                           where s.UserId == viewModel.UserId
                           select s).FirstOrDefault();
            if (student is not null)
            {
                student.Name = viewModel.Name;
                student.Email = viewModel.Email;
                student.Phone = viewModel.Phone;
                student.DateOfBirth = viewModel.DateOfBirth;
                student.Gender = viewModel.Gender;                
                student.State = viewModel.State;
                student.Country = viewModel.Country;

                dbContext.SaveChanges();
            }

            return RedirectToAction("List", "Students");
        }

        [HttpPost]
        public IActionResult Delete(int UserId)          //action for deleting a single data
        {
            var student = (from s in dbContext.Students
                           where s.UserId == UserId
                           select s).FirstOrDefault();

            if (student is not null)
            {
                dbContext.Students.Remove(student);
                dbContext.SaveChanges();
            }

            return RedirectToAction("List", "Students");
        }

        [HttpGet]
        public JsonResult CheckEmailUnique(string email)
        {
            bool isUnique = !dbContext.Students.Any(s => s.Email.ToLower() == email.ToLower());
            return Json(isUnique);
        }
    }
}
