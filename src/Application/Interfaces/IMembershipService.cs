using Application.Models.Requests;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMembershipService
    {
        List<MembershipDto> Get();
        MembershipDto AddMembership(MembershipRequest membershipRequest);

        void Update(MembershipRequest membershipRequest);
        void Delete(string typeMembership);
    }
}
