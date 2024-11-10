using IdettaTestServer.DAL;
using IdettaTestServer.DAL.DAO;
using IdettaTestServer.DAL.DomainClasses;
using IdettaTestServer.DAL.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IdettaTestServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public AdminController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        [Route("load_activites")]
        public async Task<IActionResult> LoadActivities()
        {
            try
            {
                const string dir = @".\ActivityJSON";
                string[] fileNames = Directory.GetFiles(dir);

                ActivityDAO dao = new ActivityDAO(_appDbContext);
                foreach (string file in fileNames)
                {
                    Activity newActivity = new();
                    ActivityHelper activity = new();
                    using (StreamReader fileStrm = System.IO.File.OpenText(file))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        activity = (ActivityHelper)serializer.Deserialize(fileStrm, typeof(ActivityHelper))!;
                        newActivity.Name = activity.Name!;
                        newActivity.Func = activity.Func!;
                        newActivity.Filename = file.Substring(file.LastIndexOf('\\') + 1);
                    }
                    List<Activity> list = await dao.GetAllActivities();
                    bool alreadyInDb = false;
                    foreach (Activity a in list)
                    {
                        if (a.Filename == file)
                        {
                            newActivity.Id = a.Id;
                            dao.Update(newActivity);
                            alreadyInDb = true;
                            break;
                        }
                    }

                    if (!alreadyInDb)
                    {
                        dao.Add(newActivity);
                    }
                }
                return Ok("Activities loaded");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpPost]
        public IActionResult testPost(string id) 
        {
            if (!string.IsNullOrEmpty(id)) 
            {
                return Ok("It Worked!");
            }

            return BadRequest("Not Good");
        }
    }
}
