using ComputerAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerAPI.Controllers
{
    [Route("osystem")]
    [ApiController]
    public class OsystemController : ControllerBase
    {
        public ComputerShopDbContext context = new ComputerShopDbContext();
        [HttpGet("getAll")]
        public object getAllOsystem()
        {
            var users = context.Osystems.ToList();
            return new { message = "Sikeres lekérdezés", result = "" };
        }

        [HttpPost]
        public object AddNewOsystem(AddNewOsystemDTO addNewOsystemDTO)
        {
            var osystem = new Osystem()
            {
                Id = Guid.NewGuid(),
                Name = addNewOsystemDTO.Name,
                Version = addNewOsystemDTO.Version,
                RegisterTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };
            context.Osystems.Add(osystem);
            context.SaveChanges();
            return StatusCode(201, new { message = "Sikeres hozzáadás", result = osystem });
        }

        [HttpPut]

        public object UpdateOsystem([FromQuery] Guid id, [FromBody] UpdateOsystemDTO updateOsystemDTO)
        {
            var osystem = context.Osystems.FirstOrDefault(osystem => osystem.Id == id);

            if (osystem == null)
            {
                osystem.Name = updateOsystemDTO.Name;
                osystem.Version = updateOsystemDTO.Version;
                osystem.UpdateTime = DateTime.Now;
                context.Osystems.Update(osystem);

                context.SaveChanges();
                return StatusCode(200, new { message = "Sikeres frissítés", result = osystem });
            }
            return StatusCode(404, new { message = "Sikertelen frissítés", result = osystem });
        }

        [HttpDelete]
        public object DeleteOsystem([FromQuery] Guid id)
        {
            var osystem = context.Osystems.Find(id);
            if (osystem != null)
            {
                context.Osystems.Remove(osystem);
                context.SaveChanges();

                return StatusCode(204, new { message = "Sikeres törlés", result = osystem });

            }
            return StatusCode(404, new { message = "Sikertelen törlés", result = osystem });
        }

        [HttpGet("allOsystemAndComputer")]
        public object AllOsystemAndComputer()
        {
            var computers = context.Osystems.Include(o => o.Computers).ToList();
            if (computers.Count > 0)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            }
            return StatusCode(404, new { message = "Sikertelen lekérdezés", result = computers });
        }

        [HttpGet("oneOsystemAndComputer")]
        public object OneOsystemAndComputer([FromQuery] Guid id)
        {
            var osystem = context.Osystems.Where(o => o.Id == id).Join(context.Computers, o => o.Id, c => c.OsysId, (o, c) => new {o.Name, c});
            if (osystem != null)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = osystem });
            }
            return StatusCode(404, new { message = "Sikertelen lekérdezés", result = osystem });
        }
    }
}
