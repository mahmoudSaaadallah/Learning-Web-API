using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebApiLearning.DataAccess.Data;
using WebApiLearning.Model.IRepository;
using WebApiLearning.Model.Models;

namespace WebApiLearning.DataAccess.Repository
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context): base(context)
        {
            _context = context;
        }

        

        public void Update(Employee employee)
        {
            Employee empDb= _context.Employees.FirstOrDefault(e => e.Id == employee.Id);
            if (empDb != null)
            {
                empDb.Name = employee.Name;
                empDb.Address = employee.Address;
                empDb.Salary = employee.Salary;
            }
        }
    }
}
