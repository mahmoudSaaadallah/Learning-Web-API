using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiLearning.DataAccess.Data;
using WebApiLearning.Model.IRepository;

namespace WebApiLearning.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
       
        private ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Employee = new EmployeeRepository(_context);
            Department = new DepartmentRepository(_context);
        }
        public IEmployeeRepository Employee{ get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
