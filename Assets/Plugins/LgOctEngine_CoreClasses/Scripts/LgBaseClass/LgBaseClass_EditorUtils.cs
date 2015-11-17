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
    /// Adds utility methods that are globally useful in the Unity environment.
    /// Be conservative about adding methods here: 90% of possible games should use this method for it to warrant being included!!
    /// </summary>
    public abstract partial class LgBaseClass
    {
        /// <summary>
        /// Are we running in the editor?
        /// </summary>
        protected static bool IsEditor()
        {
#if UNITY_EDITOR
            return true;
#else 
        return false;
#endif
        }

        /// <summary>
        /// Implements an editor-only dialog prompt, useful for debugging.
        /// This halts execution until the user presses a button.
        /// </summary>
        /// <returns>true if button 1 was pressed, otherwise false.</returns>
        protected static bool EditorPromptUser(string title, string message, string button1, string button2 = null)
        {
            bool bRetVal = false;
            string clipMessage = "";
            for (int i = 0; i < message.Length; i+=256 )
            {
                string nextSnip = message.Substring(i, Math.Min(message.Length-i, 256));
                if (nextSnip.IndexOf("\n")==-1) {
                    nextSnip += "\n";
                }
                clipMessage += nextSnip;
            }
#if UNITY_EDITOR
            if (button2 == null)
            {
                bRetVal = UnityEditor.EditorUtility.DisplayDialog(title, clipMessage, button1);
            }
            else
            {
                bRetVal = UnityEditor.EditorUtility.DisplayDialog(title, clipMessage, button1, button2);
            }
#endif
            return bRetVal;
        }
        /// <summary>
        /// Implements an editor-only file open and edit routine.
        /// </summary>
        protected static void EditorOpenFileForEdit(string fileWithPath, int line = 1)
        {
#if UNITY_EDITOR
            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fileWithPath, line);
#endif
        }


        // --------------------------------------------------------------------------------------------
        #region _Internal
        #endregion // _Internal

    }
} // namespace LgOctEngine.CoreClasses
