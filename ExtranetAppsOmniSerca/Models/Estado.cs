using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace ExtranetAppsOmniSerca.Models
{
    public class Estado
    {
        public int ID { get; set; }

        public int Numero{ get; set; }

        public String Descripcion { get; set; }


    }
}