using Application.Models.Requests;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IMembershipService
    {
        List<MembershipDto> Get();
        MembershipDto AddMembership(MembershipRequest membershipRequest);

        void Update(MembershipRequest membershipRequest);
        void Delete(string typeMembership);
        MembershipDto GetByType(string type);
        List<object> GetClientCountByMembership();
    }

}
