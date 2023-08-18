using Business.Abstract;
using DataAccess.Concrete.Context;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize(Roles = "HUMANRESOURCE")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly HRMSContext _context;
        public DepartmentController(IDepartmentService departmentService, HRMSContext context)
        {
            _departmentService = departmentService;
            _context = context;
        }
        public IActionResult Index()
        {
            var values = _departmentService.GetAll();
            return View(values);
        }
        [HttpGet]
        public IActionResult AddDepartment()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddDepartment(Department department)
        {
            _departmentService.Insert(department);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteDepartment(int id)
        {
            var value = _departmentService.GetById(id);
            _departmentService.Delete(value);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult UpdateDepartment(int id)
        {
            var value = _departmentService.GetById(id);
            return View(value);
        }
        [HttpPost]
        public IActionResult UpdateDepartment(Department department)
        {
            _departmentService.Update(department);
            return RedirectToAction("Index");
        }
        public ActionResult DepartmentDetails(int id)
        {
            var value = _departmentService.GetById(id);
            return View(value);
        }
    }
}
