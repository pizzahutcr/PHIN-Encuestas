using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UAParser;

namespace Surveys.Models
{
    public static class DeviceExtensions
    {
        public static bool IsMobile(this Device device)
        {
            if (device == null) return false;

            string[] mobileDevices = { "Mobile", "Tablet", "iPhone", "iPad", "Android" };
            return mobileDevices.Any(d => device.Family.Contains(d));
        }
    }
}