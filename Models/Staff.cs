namespace ASM.Models
{
    public class Staff
    {
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public virtual Account Account { get; set; }
    }
}
