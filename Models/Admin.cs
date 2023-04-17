namespace ASM.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string AdminName { get; set; }
        public virtual Account Account { get; set; }
    }
}
