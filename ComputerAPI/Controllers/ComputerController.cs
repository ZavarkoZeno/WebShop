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
        public object AddNewComputer([FromBody] AddNewComputerDTO addNewComputerDto)
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
        public object UpdateComputer([FromQuery] Guid id, [FromBody] UpdateComputerDTO updateComputerDto)
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

            var cmp = context.Osystems.Join(context.Computers, o => o.Id, c => c.OsysId, (o, c) => new { o.Name, o.Version, c.Brand, c.Type });
            if (cmp != null)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = cmp });
            }
            return StatusCode(404, new { message = "Sikertelen lekérdezés", result = cmp });
        }

        [HttpGet("oneOsystemOneComputer")]
        public object OneOsystemOneComputer([FromQuery] Guid id)
        {
            var osystems = context.Osystems.GroupJoin(context.Computers, o => o.Id, c => c.OsysId, (o, c) => new {OsName = o.Name, Ver = o.Version, Count = c.Count() }).OrderBy(o => o.OsName).ThenBy(c => c.Ver);
            if (osystems != null)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = osystems });
            }
            return StatusCode(404, new { message = "Sikertelen lekérdezés", result = osystems });
        }

        [HttpGet("allComputerSortedByOsystem")]
        public object AllComputerSortedByOsystem()
        {
            var computers = context.Computers.Join(context.Osystems, c => c.OsysId, o => o.Id, (c, o) => new { c.Brand, c.Type, o.Name, o.Version }).OrderBy(c => c.Name);
            if (computers.Count() > 0)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            }
            return StatusCode(404, new { message = "Sikertelen lekérdezés", result = computers });
        }

        [HttpGet("getComputersByBrand")]
        public object GetComputersByBrand([FromQuery] string brand)
        {
            var computers = context.Computers.Where(c => c.Brand.ToLower() == brand.ToLower()).Select(c => new { c.Id, c.Brand, c.Type, Osystem = context.Osystems.Where(o => o.Id == c.OsysId).Select(o => new { o.Name, o.Version }).FirstOrDefault()}).ToList();
            if (computers.Count > 0)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            }
            return StatusCode(404, new { message = "Sikertelen lekérdezés", result = computers });
        }

        [HttpGet("getComputersByOsystem")]
        public object GetComputersByOsystem([FromQuery] string OsystemName)
        {
            var computers = context.Computers.Join(context.Osystems, c => c.OsysId, o => o.Id, (c, o) => new { c.Id, c.Brand, c.Type, Osystem = new { o.Name, o.Version } }).Where(co => co.Osystem.Name == OsystemName);
            if (computers.Count > 0)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            }
            return StatusCode(404, new { message = "Sikertelen lekérdezés", result = computers });
        }

        [HttpGet("getLatestComputer")]
        public object GetLatestComputer()
        {
            var latestComputer = context.Computers.OrderByDescending(c => c.CreatedTime).Take(1).Select(c => new { c.Id, c.Brand, c.Type, c.CreatedTime }).ToList();
            if (latestComputer.Count > 0)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = latestComputer });
            }
            return StatusCode(404, new { message = "Sikertelen lekérdezés", result = latestComputer });
        }

        [HttpGet("getBrandSummary")]
        public object GetBrandSummary()
        {
            var brandSummary = context.Computers.GroupBy(c => c.Brand).Select(g => new { Brand = g.Key, Count = g.Count() }).ToList();
            if (brandSummary.Count > 0)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = brandSummary });
            }
            return StatusCode(404, new { message = "Sikertelen lekérdezés", result = brandSummary });
        }
    }
}
