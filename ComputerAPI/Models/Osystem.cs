using System.ComponentModel.DataAnnotations;

namespace ComputerAPI.Models
{
    public class Osystem
    {
        [Key]
        [MaxLength(36)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public DateTime RegisterTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public ICollection<Computer> Computers { get; set; }
    }
}
