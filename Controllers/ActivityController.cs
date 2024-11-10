using IdettaTestServer.DAL;
using IdettaTestServer.DAL.DAO;
using IdettaTestServer.DAL.DomainClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace IdettaTestServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ActivityController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Index(int id)
        {
            ActivityDAO dao = new(_context);
            Activity? activity = await dao.GetById(id);
            if (activity == null) 
            {
                return NotFound();
            }
            return Ok(activity);
        }
    }
}
