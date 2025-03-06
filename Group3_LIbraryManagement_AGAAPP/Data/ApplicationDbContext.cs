using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Group3_LIbraryManagement_AGAAPP.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        internal object PublicationModel;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
