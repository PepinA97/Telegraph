using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Net;

namespace MyServer
{
    class Security
    {
        static Dictionary<IPAddress, DateTime[]> Strikes;

        public static void Initialize()
        {
            Strikes = new Dictionary<IPAddress, DateTime[]>();
        }

        public static string DecryptPassword(byte[] encryptedPassword)
        {
            return Encoding.ASCII.GetString(encryptedPassword);
        }

        public static string CreatePasswordHash(byte[] passwordData)
        {
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();

            byte[] hash = MD5.ComputeHash(passwordData);

            return Encoding.ASCII.GetString(hash);
        }

        public static bool IsUsernameWithinRules(string username)
        {
            if (username.Length > 16)
            {
                return false;
            }

            foreach (char c in username)
            {
                if (!Char.IsLetterOrDigit(c))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsStruckOut(IPAddress ipAddress)
        {
            DateTime[] strikes = Strikes.GetValueOrDefault(ipAddress);

            int numValidStrikes = 0;

            // Iterate through all strikes
            foreach (DateTime strike in strikes)
            {
                DateTime strikePlusFive = strike.AddMinutes(5);

                if (DateTime.Now.CompareTo(strikePlusFive) < 0)
                {
                    // If right now is earlier than the strike plus five minutes, it is a valid strike
                    numValidStrikes++;
                }
            }

            if (numValidStrikes >= Constants.Session.MaxStrikesAllowed)
            {
                return true;
            }

            return false;
        }

        public static void Strike(IPAddress ipAddress)
        {
            DateTime[] strikes = Strikes.GetValueOrDefault(ipAddress);

            // Find earliest strike
            int earliestStrikeIndex = 0;

            for (int i = 0; i < Constants.Session.MaxStrikesAllowed; i++)
            {
                if (strikes[i] != null)
                {
                    if (strikes[i].CompareTo(strikes[earliestStrikeIndex]) < 0)
                    {
                        earliestStrikeIndex = i;
                    }
                }
            }

            if (IsStruckOut(ipAddress))
            {
                Log.IPAddressStruckOut(ipAddress);
            }

            // Replace with new strike
            strikes[earliestStrikeIndex] = DateTime.Now;
        }

        public static void AddNewStrikee(IPAddress ipAddress)
        {
            if (!Strikes.ContainsKey(ipAddress))
            {
                DateTime[] strikes = new DateTime[Constants.Session.MaxStrikesAllowed];

                Strikes.Add(ipAddress, strikes);
            }
        }
    }
}
