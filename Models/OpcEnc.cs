using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Surveys.Models
{
    public class OpcEnc
    {
        public int idTipoResp { get; set; }
        public string descripcion { get; set; }
        public string componente { get; set; }
        public bool activaCodRest { get; set; }
        public bool activo { get; set; }
       public List<OpcDet> listaOpcDet { get; set; }

        public OpcEnc() { }
        public OpcEnc(int idTipoResp, string descripcion, string componente, bool activaCodRest, bool activo)
        {
            this.idTipoResp = idTipoResp;
            this.descripcion = descripcion;
            this.componente = componente;
            this.activaCodRest = activaCodRest;
            this.activo = activo;
          //  this.listaOpcDet = listaOpcDet;
        }
    }

    public enum TipoRespuesta
    {
        Otro,
        text,
        checkbox,     
        date,
        textArea,
        dropDown

    }

}