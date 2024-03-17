using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiLearning.Model.IRepository
{
    public interface IUnitOfWork 
    {
        IEmployeeRepository Employee { get; }
        IDepartmentRepository Department { get; }
        void Save();
    }
}
