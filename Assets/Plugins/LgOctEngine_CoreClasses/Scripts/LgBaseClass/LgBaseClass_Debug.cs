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

using System.Diagnostics;

namespace LgOctEngine.CoreClasses
{
    /// <summary>
    /// Adds output logging methods that are globally useful in the Unity environment.
    /// </summary>
    public abstract partial class LgBaseClass
    {
        /// <summary>
        /// Is this a unity debug build? If false, we assume it's a production build and do much less output/defensive programming.
        /// </summary>
        protected static bool IsDebug()
        {
            return UnityEngine.Debug.isDebugBuild;
        }

        /// <summary>
        /// If the condition is false the application will throw a warning.
        /// DebugAsserts are used extensively in the LgOctEngine to help developers spot trouble before making production builds.
        /// </summary>
        /// <param name="bCondition"></param>
        /// <param name="message"></param>
        /// <param name="contextLevel">Used to specific how far up the callstack to jump when taken to the line of code we might be interested in.</param>
        [System.Diagnostics.Conditional("LGOCTENGINE_DEBUG_ASSERT")]
        protected static void DebugAssert(bool bCondition, string message, int contextLevel = 2)
        {
            if (bCondition == false)
            {
                LogError("DEBUG ASSERT FAILED: " + message);
                LogError("CALLSTACK: " + _GetStackTraceShort(contextLevel));
                if (IsDebug())
                {
                    if (IsEditor())
                    {
                        if (EditorPromptUser("DEBUG ASSERT FAILED:", message, "View", "Ignore"))
                        {
                            StackFrame sf = _GetContext(contextLevel);
                            EditorOpenFileForEdit(sf.GetFileName(), sf.GetFileLineNumber());
                            DebugBreak();
                        }
                    }
                    // No matter what, we need to throw an assert to jump out of this code execution
                    // Failure to do this can cause infinite loops in the editor prompt up above
                    Assert(bCondition, message, contextLevel + 1);
                }
            }
        }

        /// <summary>
        /// Triggers a break point ONLY if we are in the editor, and this is a debug build.
        /// </summary>
        protected static void DebugBreak()
        {
            if (IsDebug() && IsEditor())
            {
                UnityEngine.Debug.Break();
            }
        }
        /// <summary>
        /// Alias for DebugBreak().
        /// </summary>
        protected static void Break()
        {
            DebugBreak();
        }

        // --------------------------------------------------------------------------------------------
        #region _Internal

        #endregion // _Internal

    }
} // namespace LgOctEngine.CoreClasses
