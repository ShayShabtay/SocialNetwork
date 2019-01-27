using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace UI.Models
{

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class UserIdentityModel
    {
        public UserIdentityModel()
        {
            Name = "";
            Age = 0;
            Address = "";
            WorkPlace = "";
        }
        [Display(Name ="User Id")]
        [HiddenInput(DisplayValue = true)]
        public string UserId { get; set; }

        public string Name { get; set; }

        [Range(0, 120, ErrorMessage ="The range need to between 0 - 120")]
        public int? Age { get; set; }
        public string Address { get; set; }
        [Display(Name = "Work Place")]
        public string WorkPlace { get; set; }

        public string Email { get; set; }
        public bool IsFollow { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}