using Business.Abstract;
using DataAccess.Concrete.Context;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Presentation.Models;
using System.Security.Claims;

namespace Presentation.Controllers;


public class EmployeeController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IDepartmentService _departmentService;
    private readonly IJobService _jobService;

    public EmployeeController(UserManager<AppUser> userManager, IDepartmentService departmentService, IJobService jobService)
    {
        _userManager = userManager;
        _departmentService = departmentService;
        _jobService = jobService;
    }


    [Authorize(Roles = "HUMANRESOURCE")]
    [HttpGet]
    public IActionResult Index()
    {
        var users = _userManager.Users
            .Include(u => u.Department)
            .Include(u => u.Job)
            .Select(u => new UserViewModel
            {
                Id = u.Id,
                FullName = u.FullName,
                Username = u.UserName,
                Mail = u.Email,
                DepartmentName = u.Department!.DepartmentName,
                JobName = u.Job!.JobName,
                NationalityId = u.NationalityId,
                Salary = u.Salary,
                BirthDate = (DateTime)u.BirthDate,
                EmployeeCode = u.EmployeeCode,
                PhoneNumber = u.PhoneNumber,
                Status = u.Status

            }).ToList();


        return View(users);
    }


    [Authorize(Roles = "HUMANRESOURCE")]
    [HttpGet]
    public async Task<IActionResult> DeleteUser(int id)
    {

        var response = await _userManager.FindByIdAsync(id.ToString());
        response.Status = false;
        await _userManager.UpdateAsync(response);

        return RedirectToAction("Index");

    }


    [Authorize(Roles = "HUMANRESOURCE")]
    [HttpGet]
    public async Task<IActionResult> UpdateUser(int id)
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

        var response = await _userManager.FindByIdAsync(id.ToString());
        response.SecurityStamp = Guid.NewGuid().ToString();

        var updateUserViewModel = new UpdateUserViewModel
        {
            FullName = response.FullName,
            JobId = response.JobId,
            Mail = response.Email,
            GenderType = (Entities.Enums.GenderType)response.GenderType,
            Salary = response.Salary,
            BirthDate = (DateTime)response.BirthDate,
            DepartmentId = response.DepartmentId,
            NationalityId = response.NationalityId,
            PhoneNumber = response.PhoneNumber,
            Username = response.UserName,
            SecurityStamp = response.SecurityStamp
        };

        return View(updateUserViewModel);
    }


    [Authorize(Roles = "HUMANRESOURCE")]
    [HttpPost]
    public async Task<IActionResult> UpdateUser(UpdateUserViewModel model)
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
            PhoneNumber = model.PhoneNumber,
            GenderType = model.GenderType,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.UpdateAsync(appUser);
        if (result.Succeeded)
        {
            await _userManager.UpdateSecurityStampAsync(appUser);
            return RedirectToAction("Index");
        }
        return View();
    }


    [Authorize(Roles = "HUMANRESOURCE")]
    [HttpGet]
    public IActionResult UserDetail(int id)
    {
        var users = _userManager.Users
            .Include(u => u.Department)
            .Include(u => u.Job)
            .Select(u => new UserViewModel
            {
                Id = u.Id,
                FullName = u.FullName,
                Username = u.UserName,
                Mail = u.Email,
                DepartmentName = u.Department!.DepartmentName,
                JobName = u.Job!.JobName,
                NationalityId = u.NationalityId,
                Salary = u.Salary,
                BirthDate = (DateTime)u.BirthDate,
                EmployeeCode = u.EmployeeCode,
                PhoneNumber = u.PhoneNumber

            }).ToList();

        var response = users.FirstOrDefault(x => x.Id == id);

        return View(response);
    }

    [Authorize(Roles = "EMPLOYEE")]
    [HttpGet]
    public IActionResult MyProfile()
    {
        //var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //var user = _userManager.FindByIdAsync(loggedInUserId).Result;

        //if (user == null)
        //{
        //    return RedirectToAction("AccessDenied"); // Örnek bir yönlendirme
        //}

        //return View(user);

        var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var user = _userManager.Users
                .Include(u => u.Department)
                .Include(u => u.Job)
                .FirstOrDefault(x=>x.Id == loggedInUserId);

        if (user == null)
        {
            return RedirectToAction("AccessDenied"); // Örnek bir yönlendirme
        }

        return View(user);
    }
}