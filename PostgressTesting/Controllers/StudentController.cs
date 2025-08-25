using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using PostgressTesting.Models;
using PostgressTesting.Repository.Abstraction;
using PostgressTesting.Repository.Implementation;

namespace PostgressTesting.Controllers
{
    public class StudentController : Controller
    {
        private readonly PostGressSqlContext _context;
        private readonly IStudent _studentRepo;
        public StudentController(PostGressSqlContext context, IStudent studentRepo)
        {
            _context = context;
            _studentRepo = studentRepo;
        }
        public IActionResult Index()
        {
            var students = _context.Students.ToList();
            return View(students);
        }
        public IActionResult Create()
        {
            return PartialView("_Create");
        }
        [HttpPost]
        public IActionResult Create(Student student)
        {
           
            if(ModelState.IsValid)
            {
                _context.Students.Add(student);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView("_Create", student);
        }
        public IActionResult Edit(int id)
        {
            var student = _studentRepo.GetStudentById(id); 
            return PartialView("_Edit", student);         
        }
        [HttpPost]
        public IActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                _studentRepo.UpdateStudent(student);  
                return RedirectToAction("Index");
            }
            return View(student);
        }

    }
}
