using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerAPI.Models
{
    public class AddNewComputerDTO
    {
        public string? Brand { get; set; }
        public string? Type { get; set; }
        public double Display { get; set; }
        public int Memory { get; set; }
        public DateTime Createdtime { get; set; }
        public Guid OsysId { get; set; }
    }
}
