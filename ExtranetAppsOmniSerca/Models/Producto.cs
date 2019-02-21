using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace ExtranetAppsOmniSerca.Models
{

    public class Producto
    {
        public long ProductoId { get; set; }
        public long ClienteProductoId { get; set; }
        public decimal PrecioLista { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public decimal Bonificacion { get; set; }
        public decimal Final { get; set; } 
        public string Observaciones { get; set; }
        public Producto()
        { }
        public Producto(DataRow dr)
        {
            ProductoId = Convert.ToInt64(dr["ID"]);
            ClienteProductoId = Convert.ToInt64(dr["ClienteProductoId"]);
            Cantidad = Convert.ToInt32(dr["Cantidad"]);
            Descripcion = dr["Producto"].ToString();
            //PrecioLista = Convert.ToDecimal(dr["ImporteProducto"]);
            
            Bonificacion = Convert.ToDecimal(dr["Bonificacion"]);
            Final = Convert.ToDecimal(dr["Importe"]);
            //Observaciones = dr["Observaciones"].ToString();
        }
}

    
}