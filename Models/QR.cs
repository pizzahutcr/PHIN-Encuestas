using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Surveys.Models
{
    public class QR //v1
    {
        // public string Domain { get; set; }
        public string codRest { get; set; }
        public string idEncuesta { get; set; }
        public string GeneratedUrl { get; set; }
    }

    //public class QR
    //{
    //    // public string Domain { get; set; }
    //    public int SelectedOptionLocales { get; set; }
    //    public IEnumerable<QRLocales> OptionsLocales { get; set; }
    //    //public int SelectedOptionEncuesta { get; set; }
    //    //public IEnumerable<QREncuesta> OptionsEncuesta { get; set; }
    //}
    public class QREncuesta
    {
        // public string Domain { get; set; }
        public string idEncuesta { get; set; }

        public string nombre { get; set; }
       // public string token { get; set; } = string.Empty;
    }
    public class QRLocales
    {
        // public string Domain { get; set; }
        public string codRest { get; set; }

        public string nombre { get; set; }
        public string token { get; set; }=string.Empty;
    }

    public class QRViewModel
    {
        public string SelectedEncuestaId { get; set; }
        public string SelectedCodRest { get; set; }
        public IEnumerable<SelectListItem> ltsEncuestas { get; set; }
        public IEnumerable<SelectListItem> ltsLocales { get; set; }
    }
}