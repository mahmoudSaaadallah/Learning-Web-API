using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiLearning.DataAccess.Data;
using WebApiLearning.Model.IRepository;
using WebApiLearning.Model.Models;

namespace WebApiLearning.DataAccess.Repository
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        ApplicationDbContext _context;
        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Department department)
        {
            Department DepartmentDb = _context.Departments.FirstOrDefault(d => d.Id == department.Id);
            if (DepartmentDb != null)
            {
                DepartmentDb.Name =    department.Name;
                DepartmentDb.ManagerName = department.ManagerName;
            
            }
        }
    }
}
