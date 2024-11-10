using IdettaTestServer.DAL.DomainClasses;
using IdettaTestServer.DAL.Helpers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace IdettaTestServer.DAL.DAO
{
    public class ActivityDAO
    {
        AppDbContext db;

        public ActivityDAO(AppDbContext db)
        {
            this.db = db;
        }

        public int Add(Activity activity)
        {           
            db.Add(activity);
            db.SaveChanges();
            return activity.Id;
        }

        public async Task<List<Activity>> GetAllActivities() 
        {
            List<Activity> activities = new List<Activity>();

            activities = await db.Activities.ToListAsync();

            return activities;
        }

        public async Task<Activity?> GetById(int id)
        {
            return await db.Activities.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async void Update(Activity activity)
        {
            db.Update(activity);
            await db.SaveChangesAsync();
        }
    }
}
