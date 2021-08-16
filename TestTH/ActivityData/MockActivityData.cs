using System;
using System.Collections.Generic;

namespace TestTH.ActivityData
{
    public class MockActivityData : IActivityData
    {
        private List<Activity> activities = new List<Activity>()
        {

            new Activity()
            {
               id=1,
                created_at= DateTime.Now,
                property_id= 1,
                schedule= DateTime.Now,
                status = "OK",
                title="Titulo 1",
                updated_at=DateTime.Now
            },
            new Activity()
            {
               id=2,
                created_at= DateTime.Now,
                property_id= 2,
                schedule= DateTime.Now,
                status = "OK2",
                title="Titulo 2",
                updated_at=DateTime.Now
            }
        };

        public Activity AddActivity(Activity activity)
        {
            throw new NotImplementedException();
        }

        public void DeleteActivity(Activity activity)
        {
            throw new NotImplementedException();
        }

        public Activity EditActivity(Activity activity)
        {
            throw new NotImplementedException();
        }

        public Activity GetActivity(int id)
        {
            throw new NotImplementedException();
        }

        public List<Activity> GetActivityes()
        {
            return activities;
        }
    }
}
