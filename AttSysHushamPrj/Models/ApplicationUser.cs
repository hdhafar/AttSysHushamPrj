using Microsoft.AspNetCore.Identity;

namespace AttSysHushamPrj.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? BadgeNumber { get; set; } = 10;
        public int? DeptID { get; set; }



    }
}
