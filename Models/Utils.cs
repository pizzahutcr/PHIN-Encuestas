using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Surveys.Models
{
    public class Utils
    {
        private static Random random = new Random();

        public static string GenerateRandomId()
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int idLength = 15;

            StringBuilder builder = new StringBuilder(idLength);
            for (int i = 0; i < idLength; i++)
            {
                int randomIndex = random.Next(0, allowedChars.Length);
                builder.Append(allowedChars[randomIndex]);
            }

            return builder.ToString();
        }
    }
}