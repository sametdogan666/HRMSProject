using Business.Abstract;
using DataAccess.Concrete.Context;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Presentation.Models;
using SkiaSharp;
using System.Security.Claims;

namespace Presentation.Controllers;


public class OffDayController : Controller
{
    private readonly IOffDayService _offDayService;
    private readonly UserManager<AppUser> _userManager;
    private readonly HRMSContext _context;
    public OffDayController(IOffDayService offDayService, UserManager<AppUser> userManager, HRMSContext context)
    {
        _offDayService = offDayService;
        _userManager = userManager;
        _context = context;
    }

    [Authorize(Roles = "HUMANRESOURCE")]
    public IActionResult Index()
    {
        var offDay = _offDayService.GetOffDaysWithUserName();



        return View(offDay);
    }

    [Authorize(Roles = "HUMANRESOURCE")]
    [HttpGet]
    public IActionResult AddOffDay()
    {
        var appUserList = (from user in _userManager.Users
                           select new SelectListItem
                           {
                               Value = user.Id.ToString(),
                               Text = user.FullName
                           }).ToList();

        ViewBag.AppUserList = appUserList;

        return View();
    }

    [Authorize(Roles = "HUMANRESOURCE")]
    [HttpPost]
    public IActionResult AddOffDay(OffDay offDay)
    {
        _offDayService.Insert(offDay);
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "HUMANRESOURCE")]
    public IActionResult DeleteOffDay(int id)
    {
        var value = _offDayService.GetById(id);
        _offDayService.Delete(value);
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "HUMANRESOURCE")]
    [HttpGet]
    public IActionResult UpdateOffDay(int id)
    {
        var appUserList = (from user in _userManager.Users
                           select new SelectListItem
                           {
                               Value = user.Id.ToString(),
                               Text = user.FullName
                           }).ToList();

        ViewBag.AppUserList = appUserList;

        var value = _offDayService.GetOffDaysWithUserName().FirstOrDefault(x => x.Id == id);

        return View(value);
    }

    [Authorize(Roles = "HUMANRESOURCE")]
    [HttpPost]
    public IActionResult UpdateOffDay(OffDay offDay)
    {
        _offDayService.Update(offDay);
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "EMPLOYEE")]
    public IActionResult MyOffDays()
    {
        // Giriş yapan kullanıcının Id'sini al
        var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var user = _context.Users
            .Include(u => u.OffDays)
            .FirstOrDefault(u => u.Id == loggedInUserId);
        if (user == null)
        {
            return RedirectToAction("AccessDenied"); // Örnek bir yönlendirme
        }
        var offDays = user.OffDays;
        var appUserNames = offDays.Select(p => p.AppUser.FullName).Distinct().ToList();
        var selectList = new SelectList(appUserNames);

        ViewBag.AppUserNames = selectList;
        return View(offDays);
    }
}

