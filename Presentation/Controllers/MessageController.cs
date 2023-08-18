using DataAccess.Concrete.Context;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Presentation.Controllers
{
    public class MessageController : Controller
    {
        private readonly HRMSContext _context;

        public MessageController(HRMSContext context)
        {
            _context = context;
        }


        [Authorize(Roles = "HUMANRESOURCE")]
        public ActionResult Index()
        {
            var values = _context.Messages.ToList();
            return View(values);
        }

        [HttpGet]
        public ActionResult AddMessage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMessage(Message p)
        {
            _context.Messages.Add(p);
            _context.SaveChanges();
            return RedirectToAction("AddMessage", "Message");
        }

        [Authorize(Roles = "HUMANRESOURCE")]
        public ActionResult DeleteMessage(int id)
        {
            var message = _context.Messages.Find(id);

            if (message != null)
            {
                _context.Messages.Remove(message);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "HUMANRESOURCE")]
        public ActionResult ViewMessage(int id)
        {
            var message = _context.Messages.Find(id);

            if (message == null)
            {
                return RedirectToAction("ListMessages");
            }

            return View(message);
        }
    }
}
