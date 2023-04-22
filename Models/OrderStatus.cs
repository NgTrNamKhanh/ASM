using System.ComponentModel.DataAnnotations;

namespace ASM.Models
{
    public class OrderStatus
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public String StatusName { get; set; }


    }
}