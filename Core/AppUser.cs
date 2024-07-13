using Core.Extensions;
using Microsoft.AspNetCore.Identity;


namespace Core
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public DateTime Created { get; set; }=DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public DateTime DOB { get; set; } = DateTime.Now;
        public string Gender{ get; set; }
        public string Interests{ get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos { get; set; }
        
        public int getAge()
        {
            return DOB.getAge();
        }

    }
}
