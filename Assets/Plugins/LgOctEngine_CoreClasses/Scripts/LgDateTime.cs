// <commonheader>
// ------------------------------------------------------------------------------------------------
// Copyright (c) 2013-14 Leviathan Games
// www.leviathangames.com
//
// This code is released under the "DO WHATEVER YOU WANT WITH IT EXCEPT DELETE THIS HEADER" policy.
// If anything bad happens as a result of using this code, don't blame us.
//
// If you want support, buy this code via the UnityAsset store.
//
// ------------------------------------------------------------------------------------------------
#pragma warning disable 0219 // unused assignment
#pragma warning disable 0168 // assigned not used
#pragma warning disable 0414 // unused variables

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// </commonheader>

namespace LgOctEngine.CoreClasses
{
    /// <summary>
    /// Implements commonly used date and time methods.
    /// </summary>
    public class LgDateTime : LgBaseClass {

        /// <summary>
        /// Convert a number of seconds to a number of hours.
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static double SecondsToHours(double seconds)
        {
            return seconds / (60 * 60);
        }

        /// <summary>
        /// Convert epoch time to an int day of the year.
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        public static int EpochToDayOfYear(double epoch)
        {
            System.DateTime dt = new System.DateTime(1970, 1, 1);
            dt = dt.AddSeconds(epoch);
            return dt.DayOfYear;
        }

        /// <summary>
        /// Get delta time from epoch, in whole seconds.
        /// </summary>
        public static double GetSecondsFromEpoch()
        {
            System.DateTime epoch = new System.DateTime(1970, 1, 1);
            System.TimeSpan ts = System.DateTime.UtcNow - epoch;
            return Math.Floor(ts.TotalSeconds);
        }
        /// <summary>
        /// Get delta time from epoch, in whole miliseconds.
        /// </summary>
        public static double GetMilisecondsFromEpoch()
        {
            System.DateTime epoch = new System.DateTime(1970, 1, 1);
            System.TimeSpan ts = System.DateTime.UtcNow - epoch;
            return Math.Floor(ts.TotalMilliseconds);
        }
        /// <summary>
        /// Get delta time from epoch, in whole seconds, in local timezone.
        /// </summary>
        public static double GetSecondsFromEpochLocal()
        {
            System.DateTime epoch = new System.DateTime(1970, 1, 1);
            System.TimeSpan ts = System.DateTime.Now - epoch;
            return Math.Floor(ts.TotalSeconds);
        }
        /// <summary>
        /// Return the current utc time in the format: YYYY-MM-DDThh:mm:ss (used by mixpanel, and other online services)
        /// </summary>
        /// <returns></returns>
        public static string GetTimeNowUtc()
        {
            return DateTime.UtcNow.ToString("s");
        }

        /// <summary>
        /// Convert a string in hours:mins:seconds to seconds.
        /// </summary>
        public static int HmsToSeconds(string hms)
        {
            string[] split = hms.Split(':');
            DebugAssert(split.Length == 3, "Split HMS needs 3 pieces: " + hms);
            int hours = 0;
            try
            {
                hours = System.Convert.ToInt16(split[0]);
            }
            catch { }
            int mins = 0;
            try
            {
                mins = System.Convert.ToInt16(split[1]);
            }
            catch { }
            int secs = 0;
            try
            {
                secs = System.Convert.ToInt16(split[2]);
            }
            catch { }
            int retVal = (hours * 60 * 60) + (mins * 60) + secs;
            //LgBaseClass.DebugLog("Read: " + hms + " as: " + retVal);
            return retVal;
        }
        /// <summary>
        /// Converts a number of seconds to Hours:Minutes:Seconds string format.
        /// </summary>
        public static string SecondsToHms(int seconds)
        {
            // Cap time from going negative.
            if (seconds < 0) seconds = 0;
            seconds = Mathf.Abs(seconds);
            int hours = seconds / 60 / 60;
            seconds -= hours * 60 * 60;
            int mins = seconds / 60;
            seconds -= mins * 60;
            string result = "";
            if (hours < 10) result += "0";
            result += "" + hours + ":";
            if (mins < 10) result += "0";
            result += "" + mins + ":";
            if (seconds < 10) result += "0";
            result += "" + seconds;
            //LgBaseClass.DebugLog("Read " + seconds + " as " + result);
            return result;
        }
    }
} // namespace LgOctEngine.CoreClasses