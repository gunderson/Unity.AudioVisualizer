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
        /// Recursively call the supplied function on all this game object's children. 
        /// Useful when hunting through a transform hierarchy for something.
        /// </summary>
        protected static void GameObjectChildCallback(GameObject obj, System.Action<GameObject> func)
        {
            DebugAssert(func != null, "Null func provided to GameObjectChildCallback");
            DebugAssert(obj != null, "Null object provided to GameObjectChildCallback with func: " + func.ToString());
            func(obj);
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                GameObjectChildCallback(obj.transform.GetChild(i).gameObject, func);
            }
        }

        // --------------------------------------------------------------------------------------------
        #region _Internal

        #endregion // _Internal

    }
} // namespace LgOctEngine.CoreClasses
