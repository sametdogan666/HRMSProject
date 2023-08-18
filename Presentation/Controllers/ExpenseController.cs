using Business.Abstract;
using DataAccess.Concrete.Context;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize(Roles = "HUMANRESOURCE")]
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService, HRMSContext context)
        {
            _expenseService = expenseService;
        }
        public IActionResult Index()
        {
            var values = _expenseService.GetAll();
            return View(values);
        }
        [HttpGet]
        public IActionResult AddExpense()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddExpense(Expense expense)
        {
            _expenseService.Insert(expense);
            return RedirectToAction("Index");
        }
        public IActionResult DeleteExpense(int id)
        {
            var value = _expenseService.GetById(id);
            _expenseService.Delete(value);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult UpdateExpense(int id)
        {
            var value = _expenseService.GetById(id);
            return View(value);
        }
        [HttpPost]
        public IActionResult UpdateExpense(Expense expense)
        {
            _expenseService.Update(expense);
            return RedirectToAction("Index");
        }
        public ActionResult ExpenseDetails(int id)
        {
            var value = _expenseService.GetById(id);
            return View(value);
        }
    }
}
