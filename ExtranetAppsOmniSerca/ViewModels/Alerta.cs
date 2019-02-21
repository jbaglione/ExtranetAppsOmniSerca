using ExtranetAppsOmniSerca.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ExtranetAppsOmniSerca.ViewModels
{
    public class Alerta
    {
        public string DescripcionAlerta { get; set; }

        public Alerta(DataRow dr)
        {
            DescripcionAlerta = dr["DescripcionAlerta"].ToString();
        }
    }
}