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
    /// Adds utility methods that are globally useful in the Unity environment.
    /// Be conservative about adding methods here: 90% of possible games should use this method for it to warrant being included!!
    /// </summary>
    public abstract partial class LgBaseClass
    {
        /// <summary>
        /// Implements an exception type with additional information about the context (line number, callstack) of the cause.
        /// </summary>
        protected class LgContextException : System.Exception
        {
            public System.Diagnostics.StackFrame Sf;
            public string StackTraceString;
            public LgContextException(string condition, System.Diagnostics.StackFrame sf = null, string stackTraceString = null)
                : base(condition)
            {
                // Keep the stackframe so the editor can open the file to the right line!
                this.Sf = sf;
                this.StackTraceString = stackTraceString;
            }
        }

        /// <summary>
        /// Implements the application's primary exception handler.
        /// </summary>
        /// <param name="e"></param>
        protected static void HandleFatalException(System.Exception e)
        {
            if (e != null)
            {
                if (_ApplicationFatalExceptionHandler != null)
                {
                    _ApplicationFatalExceptionHandler(e);
                }
                else
                {
                    if (IsEditor() && IsDebug())
                    {
                        StackFrame sf;
                        if (e.InnerException != null)
                        {
                            sf = _GetContextFromException(e.InnerException);
                            EditorPromptUser("INNER EXCEPTION CAUGHT:", e.InnerException.Message, "View");
                            EditorOpenFileForEdit(sf.GetFileName(), sf.GetFileLineNumber());
                            DebugBreak();
                        }
                        EditorPromptUser("EXCEPTION CAUGHT:", e.Message, "View");
                        sf = _GetContextFromException(e);
                        EditorOpenFileForEdit(sf.GetFileName(), sf.GetFileLineNumber());
                        DebugBreak();
                    }
                    else
                    {
                        // Escalate up the exception
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// Override the default exception handling with an application-defined handler.
        /// </summary>
        /// <param name="applicationExceptionHandler"></param>
        protected static void SetFatalExceptionHandler(System.Action<System.Exception> applicationFatalExceptionHandler)
        {
            _ApplicationFatalExceptionHandler = applicationFatalExceptionHandler;
        }

        // --------------------------------------------------------------------------------------------
        #region _Internal
        private static System.Action<System.Exception> _ApplicationFatalExceptionHandler;

        private static string _GetContextMethod(int level = 1)
        {
            string output = "";
            System.Diagnostics.StackFrame CallStack = new System.Diagnostics.StackFrame(level, true);
            string callerClass = CallStack.GetMethod().DeclaringType.ToString();
            // Remove args
            int parenIndex = callerClass.IndexOf("(");
            if (parenIndex > 0)
            {
                callerClass = callerClass.Substring(0, parenIndex);
            }
            // get method with full signature
            string method = CallStack.GetMethod().ToString();
            // Remove leading return type
            method = method.Substring(method.IndexOf(" ") + 1);
            output = callerClass + "." + method + ":" + CallStack.GetFileLineNumber();
            return output;
        }
        private static System.Diagnostics.StackFrame _GetContextFromException(System.Exception e, int level = 0)
        {
            string filename = "";
            int linenumber = 1;
            // Will only work if we have debug info
            if (e != null)
            {
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(e, true);
                for (int i = level; i < st.FrameCount; i++)
                {
                    filename = st.GetFrame(i).GetFileName();
                    linenumber = st.GetFrame(i).GetFileLineNumber();

                    // Keep going up the stack until we are in a file in our project
                    if (string.IsNullOrEmpty(filename) == false && filename.Contains("Assets") && filename.Contains(".cs"))
                    {
                        break;
                    }
                }
            }
            System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(filename, linenumber, 1);
            return sf;
        }
        private static System.Diagnostics.StackFrame _GetContext(int level = 1, System.Diagnostics.StackFrame fs = null)
        {
            //System.Diagnostics.StackFrame fs = null;
            try
            {
                fs = new System.Diagnostics.StackFrame(level, true);
            }
            catch { }
            return fs;
        }

        /// <summary>
        /// Shorten the provided stack trace for polite output - if none is provided, grab the current stack.
        /// </summary>
        private static string _GetStackTraceShort(int level = 2, string stackTrace = null)
        {
            string output = "";

            if (stackTrace == null) stackTrace = Environment.StackTrace;

            // Only works on debug..
            if (IsDebug())
            {
                output = _ShortenStackTraceOutput(stackTrace, level);
            }
            return output;
        }
        private static string _ShortenStackTraceOutput(string callstack, int level)
        {
            string retVal = "";

            //We only want to do this if on a debug build
            if (IsDebug() && string.IsNullOrEmpty(callstack) != true)
            {
                string[] callers = callstack.Split(new string[] { "/", "\\", " at ", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                int skipped = 0;
                if (callers.Length > 0)
                {
                    retVal = "";
                    for (int i = 0; i < callers.Length; i++)
                    {
                        string thisCaller = callers[i];
                        if (thisCaller.Contains(".cs"))
                        {
                            if (++skipped > level)
                            {
                                retVal += "(" + thisCaller + ") ";
                            }
                        }
                    }
                }
            }

            return retVal;
        }
        #endregion // _Internal

    }
} // namespace LgOctEngine.CoreClasses
