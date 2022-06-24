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
                    alertDiv = "<script language='javascript'>Swal.fire({ position: 'center', icon: 'success', title: '"+message+ "',showConfirmButton: false, timer: 1500})</script>";
                    break;
                case Alerts.Danger:
                    alertDiv = "<script language='javascript'>Swal.fire({ icon: 'error', title: 'Oops...', text: '"+message+ "'})</script>";
                    break;
                case Alerts.Info:
                    alertDiv = "<script language='javascript'>Swal.fire({ icon: 'info', title: '¡Atención!', text: '" + message + "'})</script>";
                    break;
                case Alerts.Warning:
                    alertDiv = "<script language='javascript'>Swal.fire({ icon: 'warning', title: '¡Atención!', text: '" + message + "'})</script>";
                    break;
            }

            return alertDiv;
        }
    }
}
