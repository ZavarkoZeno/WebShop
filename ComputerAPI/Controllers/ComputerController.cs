using ComputerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerAPI.Controllers
{
    [Route("computer")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        public ComputerShopDbContext context = new ComputerShopDbContext();
        [HttpPost]
        public object AddNewCompueter([FromBody] AddNewComputerDto addNewComputerDto)
        {
            var cmp = new Computer
            {
                Id = Guid.NewGuid(),
                Brand = addNewComputerDto.Brand,
                Type = addNewComputerDto.Type,
                Display = addNewComputerDto.Display,
                Memory = addNewComputerDto.Memory,
                CreatedTime = DateTime.Now,
                OsysId = addNewComputerDto.OsysId,

            };

            if (cmp != null)
            {
                context.Computers.Add(cmp);
                context.SaveChanges();

                return StatusCode(201, new { message = "Sikeres felvétel", result = cmp });
            }

            return StatusCode(400, new { message = "Sikertelen felvétel", result = cmp });
        }

        [HttpGet]
        public object GetAll()
        {
            var computers = context.Computers.ToList();

            if (computers.Count > 0)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            }

            return StatusCode(404, new { message = "Üres adatbázis", result = computers });
        }

        [HttpGet("byId")]
        public object GetById([FromQuery] Guid id)
        {
            var computer = context.Computers.SingleOrDefault(x => x.Id == id);

            if (computer != null)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = computer });
            }

            return StatusCode(404, new { message = "Nincs találat", result = computer });
        }

        [HttpPut]
        public object UpdateComputer([FromQuery] Guid id, [FromBody] UpdateComputerDto updateComputerDto)
        {
            var computer = context.Computers.Find(id);

            if (computer != null)
            {
                computer.Brand = updateComputerDto.Brand;
                computer.Type = updateComputerDto.Type;
                computer.Display = updateComputerDto.Display;
                computer.Memory = updateComputerDto.Memory;

                context.Computers.Update(computer);
                context.SaveChanges();

                return StatusCode(200, new { message = "Sikeres frissítés", result = computer });
            }

            return StatusCode(404, new { message = "Nincs találat", result = computer });
        }

        [HttpDelete]
        public object DeleteById([FromQuery] Guid id)
        {
            var computer = context.Computers.FirstOrDefault(x => x.Id == id);

            if (computer != null)
            {
                context.Computers.Remove(computer);
                context.SaveChanges();
                return StatusCode(204);
            }

            return StatusCode(404, new { message = "Nincs találat", result = computer });
        }

        [HttpGet("allOsystemAndComputer")]
        public object AllOsystemAndComputer()
        {
            var computers = context.Osystems.Include(o => o.Computers).Select(o => new { o.Name, o.Version, o.Computers });
            var cmp = context.Osystems.Include(o => o.Computers).ToList();
            if (computers.Count > 0)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            }
            return StatusCode(404, new { message = "Sikertelen lekérdezés", result = computers });
        }
    }
}
