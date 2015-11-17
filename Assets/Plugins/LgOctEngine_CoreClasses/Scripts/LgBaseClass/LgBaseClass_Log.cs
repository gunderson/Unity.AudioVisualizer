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
    /// Adds output logging methods that are globally useful in the Unity environment.
    /// </summary>
    public abstract partial class LgBaseClass
    {
        /// <summary>
        /// Log a message to the console.
        /// </summary>
        /// <param name="message"></param>
        protected static void Log(object message)
        {
            if (_LogLevel == LogLevel.Everything)
            {
                UnityEngine.Debug.Log(message);
            }
        }
        /// <summary>
        /// Log a warning to the console.
        /// </summary>
        protected static void LogWarning(object message)
        {
            if (_LogLevel <= LogLevel.WarningsAndErrors)
            {
                UnityEngine.Debug.LogWarning(message);
            }
        }
        /// <summary>
        /// Log an error to the console.
        /// </summary>
        protected static void LogError(object message)
        {
            if (_LogLevel <= LogLevel.ErrorsOnly)
            {
                UnityEngine.Debug.LogError(message);
            }
        }
        /// <summary>
        /// Output details of this exception using the LogError channel.
        /// </summary>
        protected static void LogException(System.Exception e)
        {
            if (e != null && string.IsNullOrEmpty(e.Message) == false)
            {
                Debug.LogError("EXCEPTION THROWN: " + e.Message);
                if (e.InnerException != null && string.IsNullOrEmpty(e.InnerException.Message) == false)
                {
                    Debug.LogError("With INNER EXCEPTION: " + e.InnerException.Message);
                }
            }
        }

        /// <summary>
        /// Set the level of logging that will output to the console.
        /// It is normal for an application to use reduced logging in production builds.
        /// </summary>
        /// <param name="logLevel"></param>
        protected static void SetLogLevel(LogLevel level)
        {
            _LogLevel = level;
        }

        /// <summary>
        /// A definition of the types of logging to perform.
        /// </summary>
        protected enum LogLevel
        {
            Everything,
            WarningsAndErrors,
            ErrorsOnly,
            Nothing
        }

        // --------------------------------------------------------------------------------------------
        #region _Internal
        private static LogLevel _LogLevel = LogLevel.Everything;
        #endregion // _Internal

    }
} //namespace LgOctEngine.CoreClasses
