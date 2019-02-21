using ExtranetAppsOmniSerca.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.ViewModels
{
    public class ChangePasswordOrEmail
    {
        public string Resultado { get; set; }
        public string DescripcionError { get; set; }

        public ChangePasswordOrEmail(DataRow dr)
        {
            Resultado = dr["Resultado"].ToString();
            DescripcionError = dr["DescripcionError"].ToString();
        }
    }
}