using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Surveys.Models
{
    public class UserAgentEnc
    {
        public string UABrowser { get; set; }
        public string UAOS { get; set; }
        public string UADevice { get; set; }
        public string UADeviceModel { get; set; } = "N/A";

        public UserAgentEnc() { }

    }
}