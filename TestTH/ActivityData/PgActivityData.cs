using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TestTH.Models;

namespace TestTH.ActivityData
{
    public class PgActivityData : IActivityData
    {
        private ActivityContext _activityContext;
        public PgActivityData(ActivityContext activityContext)
        {
            _activityContext = activityContext;
        }

        public Activity AddActivity(Activity activity)
        {
            _activityContext.Activity.Add(activity);
            _activityContext.SaveChanges();
            return activity;
        }

        public Activity CancelActivity(Activity activity)
        {
            var findActivity = _activityContext.Activity.Find(activity.activityid);

            if (findActivity != null)
            {
                findActivity.status = "CANCELED";
                findActivity.updated_at = DateTime.Now;
                _activityContext.Activity.Update(findActivity);
                _activityContext.SaveChanges();
            }

            return findActivity;
        }

        public Activity EditActivity(Activity activity)
        {
            var findActivity = _activityContext.Activity.Find(activity.activityid);
            if (findActivity != null)
            {
                findActivity.schedule = activity.schedule;
                findActivity.updated_at = DateTime.Now;
                _activityContext.Activity.Update(findActivity);
                _activityContext.SaveChanges();
            }

            return activity;
        }

        public Activity GetActivity(int id)
        {
            var activity = _activityContext.Activity.Find(id);
            return activity;
        }

        public Property GetProperty(int id)
        {
            var property = _activityContext.Property.Find(id);
            return property;
        }

        public List<Property> GetProperties()
        {
            return _activityContext.Property.ToList();
        }

        public List<ActivityResult> GetActivities()
        {
            //return _activityContext.Activity.Include(r => r.Property).Include(s => s.Survey).ToList();
            return _activityContext.Activity
                .Include(p => p.Property)
                .Include(p => p.Survey)
                .Where(p => p.schedule >= DateTime.Now.AddDays(-3))
                .Where(p => p.schedule <= DateTime.Now.AddDays(14))
                .Select(p => new
                {
                    p.activityid,
                    p.title,
                    p.schedule,
                    p.created_at,
                    p.status,
                    address_p = p.Property.address,
                    id_p = p.Property.propertyid,
                    title_p = p.Property.title,
                    surv = p.Survey.FirstOrDefault() != null ? Convert.ToString(p.Survey.FirstOrDefault().surveyid) : ""
                }).AsEnumerable()
                .Select(p => new ActivityResult
                {
                    ID = p.activityid,
                    schedule = p.schedule,
                    title = p.title,
                    created_at = p.created_at,
                    status = p.status,
                    condition = p.status == "ACTIVE" && p.schedule >= DateTime.Now ? "Pendiente de realizar" :
                    (p.status == "ACTIVE" && p.schedule <= DateTime.Now ? "Atrasada" : 
                    (p.status == "DONE" ? "Finalizada" : (p.status == "CANCELED" ? "Cancelada" : "No definido"))),
                    PropertyResult = new PropertyResult { ID = p.id_p, address = p.address_p, title = p.title_p },
                    survey = String.IsNullOrEmpty(p.surv) ? "" : "http://surveymonkey.com?id=" + p.surv
                }).ToList();
        }

        public List<ActivityResult> GetFilterActivities(ActivityParameters activityparameters)
        {
            return _activityContext.Activity
                .Include(r => r.Property)
                .Include(s => s.Survey)
                .Where(x => x.schedule >= activityparameters.fec_ini)
                .Where(x => x.schedule <= activityparameters.fec_fin)
                .Where(x => String.IsNullOrEmpty(activityparameters.status) ? 1 == 1 : x.status == activityparameters.status)
                .Select(p => new
                {
                    p.activityid,
                    p.title,
                    p.schedule,
                    p.created_at,
                    p.status,
                    address_p = p.Property.address,
                    id_p = p.Property.propertyid,
                    title_p = p.Property.title,
                    surv = p.Survey.FirstOrDefault() != null ? Convert.ToString(p.Survey.FirstOrDefault().surveyid) : ""
                }).AsEnumerable()
                .Select(p => new ActivityResult
                {
                    ID = p.activityid,
                    schedule = p.schedule,
                    title = p.title,
                    created_at = p.created_at,
                    status = p.status,
                    condition = p.status == "ACTIVE" && p.schedule >= DateTime.Now ? "Pendiente de realizar" :
                    (p.status == "ACTIVE" && p.schedule <= DateTime.Now ? "Atrasada" :
                    (p.status == "DONE" ? "Finalizada" : (p.status == "CANCELED" ? "Cancelada" : "No definido"))),
                    PropertyResult = new PropertyResult { ID = p.id_p, address = p.address_p, title = p.title_p },
                    survey = String.IsNullOrEmpty(p.surv) ? "" : "http://surveymonkey.com?id=" + p.surv
                }).ToList();
        }

    }
}
