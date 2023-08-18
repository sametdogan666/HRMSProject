﻿using DataAccess.Abstract;
using DataAccess.Concrete.Context;
using DataAccess.Repositories;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework;

public class EfExpenseDal : GenericRepository<Expense>, IExpenseDal
{
    public EfExpenseDal(HRMSContext context) : base(context)
    {
    }
}