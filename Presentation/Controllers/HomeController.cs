using DataAccess.Concrete.Context;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Models;
using System.Diagnostics;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private HRMSContext _context = new HRMSContext();

        public ActionResult Index()
        {
            List<Tuple<string, double?>> userSalaries = _context.Users
                .Select(user => new Tuple<string, double?>(user.UserName, user.Salary.HasValue ? (double?)user.Salary.Value : null))
                .ToList();

            ViewBag.UserSalaries = userSalaries;
            //--------------------------------------------------------------------------

            List<OffDay> leaves = _context.OffDays.Include(o => o.AppUser).ToList();
            Dictionary<string, int> leaveStats = new Dictionary<string, int>();

            foreach (var leave in leaves)
            {
                string description = leave.AppUser != null ? leave.AppUser.UserName : "Unknown";
                int days = (int)(leave.EndDate - leave.StartDate).TotalDays + 1;

                if (leaveStats.ContainsKey(description))
                {
                    leaveStats[description] += days; // İzin gün sayısı
                }
                else
                {
                    leaveStats[description] = days;
                }
            }

            ViewBag.LeaveStats = leaveStats;

            return View();

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}