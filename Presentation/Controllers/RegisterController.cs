using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.Models;
using System.Text;

namespace Presentation.Controllers;

[Authorize(Roles = "HUMANRESOURCE")]

public class RegisterController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IDepartmentService _departmentService;
    private readonly IJobService _jobService;
    public RegisterController(UserManager<AppUser> userManager, IDepartmentService departmentService, IJobService jobService)
    {
        _userManager = userManager;
        _departmentService = departmentService;
        _jobService = jobService;
    }
    [HttpGet]
    public IActionResult Index()
    {
        var departmentList = (from department in _departmentService.GetAll()
                              select new SelectListItem
                              {
                                  Value = department.Id.ToString(),
                                  Text = department.DepartmentName
                              }).ToList();

        ViewBag.DepartmentList = departmentList;

        var jobList = (from job in _jobService.GetAll()
                       select new SelectListItem
                       {
                           Value = job.Id.ToString(),
                           Text = job.JobName
                       }).ToList();

        ViewBag.JobList = jobList;

        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Index(RegisterViewModel model)
    {
        AppUser appUser = new AppUser()
        {
            FullName = model.FullName,
            Email = model.Mail,
            UserName = model.Username,
            DepartmentId = model.DepartmentId,
            JobId = model.JobId,
            NationalityId = model.NationalityId,
            Salary = model.Salary,
            BirthDate = model.BirthDate,
            EmployeeCode = GenerateRandomString(),
            PhoneNumber = model.PhoneNumber,
            GenderType = model.GenderType

        };
        var result = await _userManager.CreateAsync(appUser, model.Password);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Employee");
        }
        return View();
    }

    private string GenerateRandomString()
    {
        Random random = new Random();
        const string letters = "EMP";
        const string digits = "0123456789";

        StringBuilder builder = new StringBuilder();

        builder.Append(letters);


        for (int i = 0; i < 3; i++)
        {
            int randomIndex = random.Next(digits.Length);
            builder.Append(digits[randomIndex]);
        }

        return builder.ToString();
    }
}