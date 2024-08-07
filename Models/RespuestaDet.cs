using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Surveys.Models
{
    public class RespuestaDet
    {
        public int idEncuesta { get; set; }
        public int numEncuesta { get; set; }
        public int idPregDet { get; set; }
        public string codRest { get; set; }
        public string descPregDet { get; set; }
        public string respuesta { get; set; }
        public int calificacion { get; set; }
        //public DateTime fecReg { get; set; }
        //public int secuencia { get; set; }
        public string descCorta { get; set; }

        public RespuestaDet() { }
    }
}