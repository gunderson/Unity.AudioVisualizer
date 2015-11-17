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
    /// Implements access to most of the LgOctEngine components.
    /// Most classes using the engine will use this class as a base class for convenient access to engine functionality.
    /// This is the only LgOctEngine class that is in the public namespace, primarily to support the Standalone classes.
    /// </summary>
    public abstract partial class LgBaseClass
    {
        /// <summary>
        /// Is the active platform set to ios?
        /// </summary>
        protected static bool IsIos()
        {
#if UNITY_IPHONE
        return true;
#else
            return false;
#endif
        }
        /// <summary>
        /// Is the active platform set to android?
        /// </summary>
        protected static bool IsAndroid()
        {
#if UNITY_ANDROID
        return true;
#else
            return false;
#endif
        }
        /// <summary>
        /// Is the active platform set to web?
        /// </summary>
        protected static bool IsWeb()
        {
#if UNITY_WEBPLAYER
        return true;
#else
            return false;
#endif
        }
        /// <summary>
        /// Is the active platform set to standalone?
        /// </summary>
        protected static bool IsStandalone()
        {
#if UNITY_STANDALONE
            return true;
#else
        return false;
#endif
        }
        /// <summary>
        /// If the condition is false, the application will be halted with a fatal error.
        /// Use extensively to exit if the block of code will crash if this condition is not met.
        /// </summary>
        /// <param name="bCondition"></param>
        /// <param name="message">The message displayed if the assertion fails</param>
        /// <param name="contextLevel"></param>
        protected static void Assert(bool bCondition, string errorMessage, int contextLevel = 2)
        {
            if (bCondition == false)
            {
                string err = "ASSERTION FAILED: \"" + errorMessage + "\"";
                HandleFatalException(new LgContextException(err, _GetContext(contextLevel), _GetStackTraceShort(contextLevel + 1)));
            }
        }

        // --------------------------------------------------------------------------------------------
        #region _Internal

        #endregion // _Internal

    }
} // namespace LgOctEngine.CoreClasses
