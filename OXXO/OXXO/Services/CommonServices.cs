using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OXXO.Enums;

namespace OXXO.Services
{
    public class CommonServices
    {
        public static string ShowAlert(Alerts obj, string message)
        {
            string alertDiv = null;

            switch (obj)
            {
                case Alerts.Success:
                    alertDiv = "<div class='alert alert-success alert-dismissable' id='alert'><button type='button' class='close' data-dismiss='alert'>&times;</button>" + message + "</a>.</div>";
                    break;
                case Alerts.Danger:
                    alertDiv = "<div class='alert alert-danger alert-dismissible' id='alert'><button type='button' class='close' data-dismiss='alert'>&times;</button>" + message + "</a>.</div>";
                    break;
                case Alerts.Info:
                    alertDiv = "<div class='alert alert-info alert-dismissable' id='alert'><button type='button' class='close' data-dismiss='alert'>&times;</button>" + message + "</a>.</div>";
                    break;
                case Alerts.Warning:
                    alertDiv = "<div class='alert alert-warning alert-dismissable' id='alert'><button type='button' class='close' data-dismiss='alert'>&times;</button>" + message + "</a>.</div>";
                    break;
            }

            return alertDiv;
        }
    }
}
