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
    /// LgTypes is a catch-all for general purpose types used in LgOctEngine, and methods to operate on those types/
    /// </summary>
    public class LgTypes : LgBaseClass
    {
        /// <summary>
        /// Returns true if the type implements the specified method. 
        /// </summary>
        public static bool ImplementsMethod(System.Type t, string methodName)
        {
            bool retVal = false;
            if (t != null && string.IsNullOrEmpty(methodName) == false) {
                System.Reflection.MethodInfo mi = t.GetMethod(methodName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
                if (mi != null)
                {
                    if (mi.DeclaringType == t)
                    {
                        // Strange case where abstract subclass of abstract class would trip the above, so rule it out
                        if (mi.IsAbstract == false)
                        {
                            retVal = true;
                        }
                    }
                }
                else
                {
                    // Could indicate reflection isnt working on this unity platform?
                    DebugAssert(false, "Method not found: " + methodName + " in system type: " + t.ToString());
                }
            }
            return retVal;
        }
        /// <summary>
        /// Creates a new instance of a class without using the new() keyword, which can be very useful in odd situations.
        /// This will fail if the object doesnt have an empty new() constructor.
        /// </summary>
        public static object TryNewByType(System.Type type)
        {
            object retVal = null;
            try
            {
                //System.Runtime.Remoting.ObjectHandle newObj = System.Activator.CreateInstance(type.Assembly.FullName, type.FullName);
                //retVal = newObj.Unwrap();

                // WSR: Version above will NOT work in webplayer, this version does
                // reference this link: http://docs.unity3d.com/412/Documentation/ScriptReference/MonoCompatibility.html
                retVal = System.Activator.CreateInstance(type);
            }
            catch (System.Exception e)  
            {
                // Catch and output exception, but keep running
                LogException(e);
            }
            return retVal;
        }
        /// <summary>
        /// See @TryNewByType(System.Type type)
        /// </summary>
        public static T TryNewByType<T>()
        {
            return (T)TryNewByType(typeof(T));
        }

        /// <summary>
        ///  Note that Color32 and Color implictly convert to each other. You may pass a Color object to this method without first casting it.
        /// </summary>
        public static string ColorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") + color.a.ToString("X2");
            return hex;
        }

        /// <summary>
        /// Note that Color32 and Color implictly convert to each other. You may pass a Color object to this method without first casting it.
        /// </summary>
        public static Color HexToColor(string hex)
        {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
			byte a = 255;
			if (hex.Length > 6) //Our old hex string might not have had an alpha value. 
			{
				if (!byte.TryParse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber, null, out a))//If not, assume 255.
				{
					a = 255;
				}
			}
            return new Color32(r, g, b, a);
        }

        /// <summary>
        /// Converts a Vector2 to a string in the format "(x,y)"
        /// </summary>
        public static string Vector2ToString(Vector2 vec2)
        {
            string x = vec2.x.ToString();
            string y = vec2.y.ToString();
            return "(" + x + "," + y + ")";
        }

        /// <summary>
        /// Converts a string in format "(x,y)" to a Vector2
        /// </summary>
        public static Vector2 StringToVector2(string str)
        {
            string trimmedStr = str.Remove(0, 1);
            trimmedStr = trimmedStr.Remove(trimmedStr.Length - 1);

            string[] splitStr = trimmedStr.Split(new char[] { '(', ',', ')' });
            //Debug.Log("Split String Length = " + splitStr.Length);
            //Debug.Log("Split String = " + splitStr[0] + "," + splitStr[1]);
            DebugAssert(splitStr.Length == 2, "String not in correct format");
            return new Vector2(float.Parse(splitStr[0]), float.Parse(splitStr[1]));
        }

		/// <summary>
		/// Converts a Vector3 to a string in the format "(x,y,z)"
		/// </summary>
		public static string Vector3ToString(Vector3 vec3)
		{
			string x = vec3.x.ToString();
			string y = vec3.y.ToString();
			string z = vec3.z.ToString();
			return "(" + x + "," + y + "," + z + ")";
		}

		/// <summary>
		/// Converts a string in format "(x,y,z)" to a Vector3
		/// </summary>
		public static Vector3 StringToVector3(string str)
		{
			string trimmedStr = str.Remove(0, 1);
			trimmedStr = trimmedStr.Remove(trimmedStr.Length - 1);

			string[] splitStr = trimmedStr.Split(new char[] { '(', ',', ')' });
            //Debug.Log("Split String Length = " + splitStr.Length);
            //Debug.Log("Split String = " + splitStr[0] + "," + splitStr[1]);
			DebugAssert(splitStr.Length == 3, "String not in correct format");
			return new Vector3(float.Parse(splitStr[0]), float.Parse(splitStr[1]), float.Parse(splitStr[2]));
		}
    }
} // namespace LgOctEngine.CoreClasses

