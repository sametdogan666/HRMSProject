using Business.Abstract;
using Business.ValidationRules.FluentValidation.ExpenseValidator;
using Core.Aspects.Autofac.Validation;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete;

public class ExpenseManager : IExpenseService
{
    private readonly IExpenseDal _expenseDal;

    public ExpenseManager(IExpenseDal expenseDal)
    {
        _expenseDal = expenseDal;
    }

    [ValidationAspect(typeof(ExpenseValidator))]
    public void Insert(Expense entity)
    {
        _expenseDal.Insert(entity);
    }

    [ValidationAspect(typeof(ExpenseValidator))]
    public void Update(Expense entity)
    {
        _expenseDal.Update(entity);
    }

    public void Delete(Expense entity)
    {
        _expenseDal.Delete(entity);
    }

    public List<Expense> GetAll()
    {
        return _expenseDal.GetAll();
    }

    public Expense? GetById(int id)
    {
        return _expenseDal.GetById(id);
    }
}