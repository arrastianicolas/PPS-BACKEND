using Infrastructure.TempModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class MembershipDto
    {
        public string Type { get; set; } = null!;

        public float Price { get; set; }

        public string Description { get; set; } = null!;

        public static MembershipDto Create(Membership membership)
        {
            return new MembershipDto
            {
                Type = membership.Type,
                Price = membership.Price,
                Description = membership.Description
            };
        }

        public static List<MembershipDto> CreateList(IEnumerable<Membership> memberships)
        {
            return memberships.Select(Create).ToList();
        }

    }
}
