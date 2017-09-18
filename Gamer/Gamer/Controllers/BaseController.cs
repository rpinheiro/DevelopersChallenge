using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Elmah;

namespace Gamer.Controllers
{
    public class BaseController : Controller
    {
        //create a execption return
        protected JsonResult CriarResultadoException(string message, string redirectUrl = null)
        {
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if (string.IsNullOrWhiteSpace(redirectUrl))
                redirectUrl = string.Empty;

            return new JsonResult()
            {
                Data = new
                {
                    valid = false,
                    validationErrors = string.Empty,
                    message = message,
                    redirectUrl = redirectUrl
                },

                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        //loggin error in elmah
        protected void LogErrorElmah(Exception ex)
        {
            ErrorLog.GetDefault(System.Web.HttpContext.Current).Log(new Error(ex));
        }

    }
}
