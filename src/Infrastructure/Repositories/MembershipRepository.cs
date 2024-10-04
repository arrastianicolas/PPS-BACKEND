using Domain.Interfaces;
using Infrastructure.TempModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MembershipRepository : BaseRepository<Membership>, IMembershipRepository
    {
        private readonly ApplicationDbContext _context;
        public MembershipRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Membership? GetByMembership(string typeMembership)
        {
            return _context.Set<Membership>().FirstOrDefault(m => m.Type == typeMembership);
        }
    }
}
