using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using TestTH.ActivityData;
using TestTH.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestTH
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private IActivityData _activityData;

        /// <summary>
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>
        public ActivityController(IActivityData activityData)
        {
            _activityData = activityData;
        }

        /// <summary>
        /// Obtiene un lista de actividades.
        /// </summary>
        /// <remarks>
        /// Obtiene un lista de actividades.
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>        
        [Authorize]
        [HttpGet("GetActivities")]        
        public IActionResult GetActivities()
        {
            return Ok(_activityData.GetActivities());
        }

        /// <summary>
        /// Obtiene un lista de actividades segun filtros.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /AddActivity
        ///     {
        ///        "fec_ini": 2021-08-13T09:30:00,
        ///        "fec_fin": "2021-08-13T09:30:00",
        ///        "status": "ACTIVE",
        ///     }
        ///
        /// </remarks>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>      
        [Authorize]
        [HttpGet("GetFilterActivities")]
        public IActionResult GetFilterActivities([FromQuery] ActivityParameters activityparameters)
        {
            return Ok(_activityData.GetFilterActivities(activityparameters));
        }

        /// <summary>
        /// Obtiene una actividad por su ID.
        /// </summary>
        /// <remarks>
        /// Obtiene una actividad por su ID.
        /// </remarks>
        /// <param name="id">Id (GUID) del objeto.</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>   
        [Authorize]        
        [HttpGet("GetActivity/{id}")]
        public IActionResult GetActivity(int id)
        {
            var activity = _activityData.GetActivity(id);
            if (activity != null)
            {
                return Ok(activity);
            }
            return NotFound($"Activity with id {id} not found");
        }

        /// <summary>
        /// Agrega una actividad.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /AddActivity
        ///     {
        ///        "propertyid": 1,
        ///        "schedule": "2021-08-13T09:30:00",
        ///        "title": "title1",
        ///     }
        ///
        /// </remarks>
        /// <param name="activity">Datos de la actividad</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="201">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response>   
        [Authorize]
        [HttpPost("AddActivity")]
        public IActionResult AddActivity(Activity activity)
        {
            try
            {
                var _findProperty = _activityData.GetProperty(activity.propertyid);
                if (_findProperty != null)
                {
                    if (_findProperty.status.ToUpper().Equals("ACTIVE"))
                    {
                        //Valida que no coincida con otra actividad
                        bool ok = true;
                        string act_over = "";
                        var _findActivities = _activityData.GetFilterActivities(new ActivityParameters { fec_ini = activity.schedule.AddHours(-2), 
                            fec_fin = activity.schedule.AddHours(2), status = "ACTIVE" });

                        foreach(ActivityResult ac in _findActivities)
                        {
                            DateTime tmp_fi = ac.schedule;
                            DateTime tmp_ff = ac.schedule.AddHours(1);
                            if ((activity.schedule > tmp_fi && activity.schedule < tmp_ff) || 
                                (activity.schedule.AddHours(1) > tmp_fi && activity.schedule.AddHours(1) < tmp_ff))
                            {
                                if (ac.PropertyResult.ID.Equals(activity.propertyid))
                                {
                                    ok = false;
                                    act_over = ac.title;
                                }
                            }
                        }

                        if (ok)
                        {
                            activity.created_at = DateTime.Now;
                            activity.updated_at = DateTime.Now;
                            activity.status = "ACTIVE";
                            _activityData.AddActivity(activity);
                            return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + activity.activityid, activity);
                        }
                        else
                        {
                            return BadRequest($"Activity unavailable at time selected, overrides with {act_over}");
                        }
                    }
                    else
                    {
                        return BadRequest("Property unavailable");
                    }
                }
                else
                {
                    return BadRequest("Property not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Modifica la fecha de una actividad.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /RescheduleActivity
        ///     {
        ///        "schedule": "2021-08-13T09:30:00"
        ///     }
        ///
        /// </remarks>
        /// <param name="id">ID de la actividad</param>
        /// <param name="activity">Datos de la actividad</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response> 
        [Authorize]
        [HttpPatch("RescheduleActivity/{id}")]
        public IActionResult EditActivity(int id, Activity activity)
        {
            try
            {
                var find_activity = _activityData.GetActivity(id);
                if (find_activity != null)
                {
                    if (find_activity.status.ToUpper().Equals("ACTIVE"))
                    {
                        //Valida que no coincida con otra actividad
                        bool ok = true;
                        string act_over = "";
                        var _findActivities = _activityData.GetFilterActivities(new ActivityParameters
                        {
                            fec_ini = activity.schedule.AddHours(-2),
                            fec_fin = activity.schedule.AddHours(2),
                            status = "ACTIVE"
                        });

                        foreach (ActivityResult ac in _findActivities)
                        {
                            DateTime tmp_fi = ac.schedule;
                            DateTime tmp_ff = ac.schedule.AddHours(1);
                            if ((activity.schedule > tmp_fi && activity.schedule < tmp_ff) ||
                                (activity.schedule.AddHours(1) > tmp_fi && activity.schedule.AddHours(1) < tmp_ff))
                            {
                                if (ac.PropertyResult.ID.Equals(activity.propertyid))
                                {
                                    ok = false;
                                    act_over = ac.title;
                                }
                            }
                        }

                        if (ok)
                        {
                            activity.activityid = find_activity.activityid;
                            _activityData.EditActivity(activity);
                            return Ok();
                        }
                        else
                        {
                            return BadRequest($"Activity unavailable at time selected, overrides with {act_over}");
                        }
                    }
                    else
                    {
                        return BadRequest("Activity canceled");
                    }                    
                }

                return NotFound($"Activity with id {id} not found");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Cancela una actividad.
        /// </summary>
        /// <remarks>        
        /// </remarks>
        /// <param name="id">ID de la actividad</param>
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el Token JWT de acceso.</response>              
        /// <response code="200">OK. Devuelve el objeto solicitado.</response>        
        /// <response code="404">NotFound. No se ha encontrado el objeto solicitado.</response> 
        [Authorize]
        [HttpPatch("CancelActivity/{id}")]
        public IActionResult CancelActivity(int id)
        {
            var activity = _activityData.GetActivity(id);
            if(activity!=null)
            {
                _activityData.CancelActivity(activity);
                return Ok();
            }

            return NotFound($"Activity with id {id} not found");
        }
    }
}
