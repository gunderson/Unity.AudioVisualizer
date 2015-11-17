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
    /// Extends the base dictionary class with some additional functionality for accessing static data - originally written for accessing xls based data.
    /// </summary>
    public class LgJsonParamsDictionary : LgJsonDictionary
    {
        /// <summary>
        /// Try to get a value, and if not present return the default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        public T TryGetValue<T>(string name, T defValue) where T : System.IConvertible
        {
            T retVal = defValue;
            retVal = GetValue<T>(name, defValue);
            return retVal;
        }
        /// <summary>
        /// Get the value specified, or throw an error if its not found.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetValue<T>(string name) where T : System.IConvertible
        {
            T retVal = default(T);
            if (this.Contains(name) == false) Debug.LogError("Key not found: " + name);
            retVal = GetValue<T>(name, retVal);
            return retVal;
        }
        /// <summary>
        /// Extract an int value from a string separated by | for example: "1|2|3|4"
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public int GetIndexedIntValue(string key, int index)
        {
            string strArray = GetValue<string>(key, null);
            DebugAssert(string.IsNullOrEmpty(strArray) == false, "Null string for key: " + key);
            string[] vals = strArray.Split('|');
            return Mathf.RoundToInt(float.Parse(vals[index]));
        }
        /// <summary>
        /// Extract an float value from a string separated by | for example: "1.1|2.1|3.1|4.1"
        /// </summary>
        /// <param name="key"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public float GetIndexedFloatValue(string key, int index)
        {
            string strArray = GetValue<string>(key, null);
            DebugAssert(string.IsNullOrEmpty(strArray) == false, "Null string for key: " + key);
            string[] vals = strArray.Split('|');
            return float.Parse(vals[index]);
        }
    }
}