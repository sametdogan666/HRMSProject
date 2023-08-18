using Business.Abstract;
using DataAccess.Concrete.Context;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Presentation.Models;
using System.Security.Claims;

namespace Presentation.Controllers;


public class ProjectController : Controller
{
    private readonly IProjectService _projectService;
    private readonly UserManager<AppUser> _userManager;
    private readonly HRMSContext _context;

    public ProjectController(IProjectService projectService, UserManager<AppUser> userManager, HRMSContext context)
    {
        _projectService = projectService;
        _userManager = userManager;
        _context = context;
    }

    [Authorize(Roles = "HUMANRESOURCE")]
    public IActionResult Index()
    {
        var projects = _projectService.GetProjectsWithAppUser();


        return View(projects);
    }

    [Authorize(Roles = "HUMANRESOURCE")]
    [HttpGet]
    public IActionResult AddProject()
    {

        ViewBag.AppUsersId = new SelectList(_userManager.Users, "Id", "FullName");

        return View();
    }

    [Authorize(Roles = "HUMANRESOURCE")]
    [HttpPost]
    public IActionResult AddProject(Project project, List<int> AppUsersId)
    {
        foreach (var userId in AppUsersId)
        {
            var appUser = _userManager.Users.FirstOrDefault(x => x.Id == userId);
            if (appUser != null)
            {
                project.AppUsers.Add(appUser);
            }
        }

        _projectService.Insert(project);
        return RedirectToAction("Index");
    }

    [Authorize(Roles = "HUMANRESOURCE")]
    [HttpGet]
    public IActionResult DeleteProject(int id)
    {

        var response = _projectService.GetById(id);
        response.StatusDelete = false;
        _projectService.Update(response);

        return RedirectToAction("Index");

    }

    [Authorize(Roles = "HUMANRESOURCE")]
    [HttpGet]
    public IActionResult EditProject(int id)
    {
        var project = _projectService.GetById(id);

        if (project == null)
        {
            return NotFound();
        }

        var viewModel = new ProjectViewModel
        {
            Id = project.Id,
            ProjectTitle = project.ProjectTitle,
            ProjectDescription = project.ProjectDescription,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Status = project.Status,
            SelectedAppUserIds = project.AppUsers.Select(u => u.Id).ToList(),
            AvailableAppUsers = _userManager.Users.ToList() // veya gerekli kullanıcı listesini burada alabilirsiniz
        };

        return View(viewModel);
    }

    [Authorize(Roles = "HUMANRESOURCE")]
    [HttpPost]
    public IActionResult EditProject(ProjectViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var project = _projectService.GetById(viewModel.Id);

            if (project == null)
            {
                return NotFound();
            }

            // Güncelleme işlemleri
            project.ProjectTitle = viewModel.ProjectTitle;
            project.ProjectDescription = viewModel.ProjectDescription;
            project.StartDate = viewModel.StartDate;
            project.EndDate = viewModel.EndDate;
            project.Status = viewModel.Status;

            // İlgili kullanıcıları güncelle
            project.AppUsers.Clear();
            var selectedAppUsers = _userManager.Users.Where(u => viewModel.SelectedAppUserIds.Contains(u.Id));
            project.AppUsers.AddRange(selectedAppUsers);

            _projectService.Update(project);

            return RedirectToAction("Index");
        }

        // Model geçerli değilse, View yeniden yüklenir ve hatalar görüntülenir
        viewModel.AvailableAppUsers = _userManager.Users.ToList();
        return View(viewModel);
    }



    [Authorize(Roles = "EMPLOYEE")]
    public IActionResult MyProjects()
    {
        // Giriş yapan kullanıcının Id'sini al
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        // Kullanıcının sahip olduğu projeleri getir
        var userProjects = _context.Projects
            .Where(p => p.AppUsers.Any(u => u.Id == userId))
            .ToList();

        return View(userProjects);
    }

}
