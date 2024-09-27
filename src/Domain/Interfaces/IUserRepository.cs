using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        User GetByUserEmail(string email);
        IEnumerable<User> GetUsersByType(string type);
    }
}
