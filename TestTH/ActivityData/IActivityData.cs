
using System;
using System.Collections.Generic;
using TestTH.Models;

namespace TestTH.ActivityData
{
    public interface IActivityData
    {
        List<ActivityResult> GetActivities();

        List<Property> GetProperties();

        List<ActivityResult> GetFilterActivities(ActivityParameters activityparameters);

        Activity GetActivity(int id);

        Property GetProperty(int id);

        Activity AddActivity(Activity activity);

        Activity CancelActivity(Activity activity);

        Activity EditActivity(Activity activity);
    }
}
