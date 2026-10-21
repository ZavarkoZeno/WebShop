using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ComputerAPI.Models
{
    public class Computer
    {
        [Key]
        [MaxLength(36)]
        public Guid Id { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string? Brand { get; set; }
        [Column(TypeName = "varchar(40)")]
        public string? Type { get; set; }
        [Column(TypeName = "float")]
        public double Display { get; set; }
        public int Memory { get; set; }
        public DateTime CreatedTime { get; set; }
        [JsonIgnore]
        public Osystem Osys { get; internal set; }
        public Guid OsysId { get; set; }
    }
}
