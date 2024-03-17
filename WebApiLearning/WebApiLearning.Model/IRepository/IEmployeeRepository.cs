using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiLearning.Model.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace WebApiLearning.Model.IRepository
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        void Update(Employee employee);
       // Employee Include(Expression<Func<Employee, object>>includeProperties, int id);
    }
}
