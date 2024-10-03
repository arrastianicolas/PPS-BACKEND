using Infrastructure.TempModels;

namespace Application.Models
{
    public class ClientUserDto
    {
        public ClientDto ClientDto { get; set; } = null!;
        public UserDto UserDto { get; set; } = null!;
    }

    public static class ClientUserDtoMapper
    {
        public static ClientUserDto Create(Client client, User user)
        {
            return new ClientUserDto
            {
                ClientDto = ClientDto.Create(client),
                UserDto = UserDto.Create(user)        
            };
        }
    }
}
