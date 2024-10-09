using Application.Interfaces;
using Application.Models.Requests;
using Application.Models;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain.Exceptions;
using Domain.Entities;

namespace Application.Services
{
    public class MembershipService  : IMembershipService 
    {
        private readonly IMembershipRepository _membershipRepository;

        public MembershipService(IMembershipRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }


        public List<MembershipDto> Get() 
        {
            var memberships = _membershipRepository.Get();

            if (memberships == null || !memberships.Any())
            {
                return new List<MembershipDto>();
            }

            var membership =  MembershipDto.CreateList(memberships);

            return membership;

        }
       public MembershipDto AddMembership(MembershipRequest membershipRequest) 
       {

            var membership = new Membership()
            {
                Type = membershipRequest.Type,
                Price = membershipRequest.Price,
                Description = membershipRequest.Description,

            };

            _membershipRepository.Add(membership);

            return MembershipDto.Create(membership);

       }

       public void Update(MembershipRequest membershipRequest) 
       {
            var membership = _membershipRepository.GetByMembership(membershipRequest.Type);

            if (membership == null) 
            {
                throw new NotFoundException("No se ha encontrado la membresia");
            }
            
            membership.Price = membershipRequest.Price;
            membership.Description = membershipRequest.Description;

            _membershipRepository.Update(membership);


       }
       public void Delete(string typeMembership) 
       {
            var membership = _membershipRepository.GetByMembership(typeMembership);

            if (membership == null)
            {
                throw new NotFoundException("No se ha encontrado la membresia");
            }

            _membershipRepository.Remove(membership);
        }
        public MembershipDto GetByType(string type) 
        {
        
            var membership = _membershipRepository.GetByMembership(type);
            return MembershipDto.Create(membership);
        }
        
    }
}
