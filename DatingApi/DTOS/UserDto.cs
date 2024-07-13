using Core;

namespace DatingApi.DTOS
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime Created { get; set; } 
        public DateTime LastActive { get; set; }
        public int age { get; set; }
        public string Gender { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<PhotoDto> Photos { get; set; }
    }
}
