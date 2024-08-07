using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Surveys.Models
{
    public class RespuestaEnc
    {
        public int idEncuesta { get; set; }
        public int numEncuesta { get; set; }
        public int idPregEnc { get; set; }
        public string codRest { get; set; }
        public string descPregEnc { get; set; }
        public string respuesta { get; set; }
        //public DateTime fecReg { get; set; }
        //public int secuencia { get; set; }
        public UserAgentEnc userAgent { get; set; }
        public string descCorta { get; set; }

        public RespuestaEnc() { }
    }
}