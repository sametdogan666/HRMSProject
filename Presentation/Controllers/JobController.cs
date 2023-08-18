using Business.Abstract;
using DataAccess.Concrete.Context;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize(Roles = "HUMANRESOURCE")]
public class JobController : Controller
{
    private readonly IJobService _jobService;


    public JobController(IJobService jobService)
    {
        _jobService = jobService;
    }
    public IActionResult Index()
    {
        var values = _jobService.GetAll();
        return View(values);
    }
    [HttpGet]
    public IActionResult AddJob()
    {
        return View();
    }
    [HttpPost]
    public IActionResult AddJob(Job job)
    {
        _jobService.Insert(job);
        return RedirectToAction("Index");
    }
    public IActionResult DeleteJob(int id)
    {
        var value = _jobService.GetById(id);
        _jobService.Delete(value);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public IActionResult UpdateJob(int id)
    {
        var value = _jobService.GetById(id);
        return View(value);
    }
    [HttpPost]
    public IActionResult UpdateJob(Job job)
    {
        _jobService.Update(job);
        return RedirectToAction("Index");
    }
    public ActionResult JobDetails(int id)
    {
        var value = _jobService.GetById(id);
        return View(value);
    }
}

