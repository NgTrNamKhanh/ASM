using Microsoft.AspNetCore.Identity;

namespace ASM.Models
{
    public class StaffViewModel
    {
        public IList<IdentityUser> staffUsers { get; set; }
        public List<StaffRevenueViewModel> staffRevenue { get; set; }
    }
}
