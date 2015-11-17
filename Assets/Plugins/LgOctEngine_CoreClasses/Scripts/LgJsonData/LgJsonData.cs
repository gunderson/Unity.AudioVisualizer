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

using LGMiniJSON;       // Our wrapped json import library

namespace LgOctEngine.CoreClasses
{
    /// <summary>
    /// Implements a JSON format accessor class.
    /// </summary>
    public partial class LgJsonNode : LgBaseClass
    {
        internal object json;
        internal Permissions permissions;

        public enum Permissions
        {
            READONLY=0,
            MODIFIABLE=1,
            CREATABLE=2
        }

        /// <summary>
        /// You cant create these directly - you must use a Create(). Internal engine modules can override this.
        /// </summary>
        internal protected LgJsonNode() {
            permissions = Permissions.READONLY;
        }


        /// <summary>
        /// Deserialize json into List&lt;object&gt; and Dictionary&lt;string, object&gt; and then return it as a LgJsonNode type.
        /// </summary>
        public static T CreateFromJsonString<T>(string jsonString, Permissions p = Permissions.READONLY) where T : LgJsonNode, new()
        {
            if (jsonString == null || jsonString.Length == 0) jsonString = "{}";
            object jsonRoot = Json.Deserialize(jsonString.Trim());
            DebugAssert(jsonRoot != null, "Failed to decode to json: " + jsonString);
            return CreateFromJson<T>(jsonRoot, p);
        }
        /// <summary>
        /// Deserialize json into List&lt;object&gt; and Dictionary&lt;string, object&gt; and then return it as a LgJsonNode type.
        /// </summary>
        public static object CreateFromJsonStringByType(string jsonString, System.Type t, Permissions p = Permissions.READONLY)
        {
            if (jsonString == null || jsonString.Length == 0) jsonString = "{}";
            object jsonRoot = Json.Deserialize(jsonString);
            DebugAssert(jsonRoot != null, "Failed to decode to json: " + jsonString);
            return CreateFromJsonByType(jsonRoot, t, p);
        }
        /// <summary>
        /// Create a LgJsonNode from a List&lt;object&gt; or a Dictionary&lt;string, object&gt;
        /// </summary>
        public static T CreateFromJson<T>(object json, Permissions p = Permissions.READONLY) where T : LgJsonNode, new()
        {
            T t = new T();
            DebugAssert(json != null, "json data passed as null");

            if (json as List<object> != null || json as Dictionary<string, object> != null)
            {
                t.json = json;
                t.permissions = p;
            }
            else
            {
				// WSR- If you get this error, you probably want to be using CreateFromJsonString()
                DebugAssert(false, "Unsupported type: " + json.GetType().ToString());
            }
            CheckForMissmatchTypes(t);
            return t;
        }
        /// <summary>
        /// Create a json node without knowing what the desired result is.
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="p"></param>
        /// <returns>null if can't be parsed, or a LgJsonDictionary or a LgJsonArray</returns>
        public static LgJsonNode CreateFromJsonStringUnknown(string jsonString, Permissions p = Permissions.READONLY)
        {
            LgJsonNode retVal = null;
            if (jsonString == null || jsonString.Length == 0) jsonString = "{}";
            object jsonRoot = Json.Deserialize(jsonString.Trim());
            DebugAssert(jsonRoot != null, "Failed to decode to json: " + jsonString);
            if (jsonRoot as List<object> != null)
            {
                retVal = CreateFromJson<LgJsonArray<object>>(jsonRoot);
            }
            else if (jsonRoot as Dictionary<string, object> != null)
            {
                retVal = CreateFromJson<LgJsonDictionary>(jsonRoot);
            }
            return retVal;
        }
        /// <summary>
        /// An alternate way to create a json node that uses reflection instead of new() and generics.
        /// </summary>
        public static object CreateFromJsonByType(object json, System.Type type, Permissions p = Permissions.READONLY)
        {
            LgJsonNode t = null;
            if (json as List<object> != null || json as Dictionary<string, object> != null)
            {
                DebugAssert(IsJsonNode(type), "You must specific a LgJsonNode subclass: " + type.ToString());
                t = (LgJsonNode)LgTypes.TryNewByType(type);
                Assert(t != null, "Unable to dynamically create: " + type.ToString() + ", you likely need an empty new() constuctor");
                t.json = json;
                t.permissions = p;
            }
            else
            {
                DebugAssert(false, "Unsupported type: " + json.GetType().ToString());
            }
            CheckForMissmatchTypes(t);
            return t;
        }
        private static void CheckForMissmatchTypes(LgJsonNode t)
        {
            if (t.IsDictionary() && t.json as Dictionary<string, object> == null ||
                t.IsList() && t.json as List<object> == null)
            {
                DebugAssert(false, "Missmatched json node: " + t.GetType().ToString() + " with underlying data: " + t.json.GetType().ToString());
            }
        }
        /// <summary>
        /// An alternate way to create a json node that uses reflection instead of new() and generics.
        /// </summary>
        public static object CreateByType(System.Type type, Permissions p = Permissions.CREATABLE)
        {
            object json = null;
            if (IsJsonNodeArray(type)) {
                json = new List<object>();
            }
            else {
                json = new Dictionary<string, object>();
            }
            return CreateFromJsonByType(json, type, p);
        }
        /// <summary>
        /// Create a new LgJsonNode of the specified type.
        /// </summary>
        public static T Create<T>(Permissions p = Permissions.CREATABLE) where T : LgJsonNode, new()
        {
            T t = new T();
            if (t.IsList())
            {
                t = CreateFromJson<T>(new List<object>(), p);
            }
            else if (t.IsDictionary())
            {
                t = CreateFromJson<T>(new Dictionary<string,object>(), p);
            }
            else
            {
                DebugAssert(false, "Unsupported type: " + t.GetType().ToString());
            }
            return t;
        }

        /// <summary>
        /// Down Casts a json node to a sub-class. If you are attempting to down-cast LgParseJsonObjects, use ParseDownCast instead.
        /// </summary>
        /// <typeparam name="T">The type of class to down cast into.</typeparam>
        /// <param name="node">The super-class node.</param>
        /// <param name="p">The level of permissions to return for the new node. These will not exceed the intrinsic permissions in the current node.</param>
        /// <returns>The down-casted JsonNode.</returns>
        public static T DownCast<T>(LgJsonNode node, Permissions p = Permissions.READONLY) where T : LgJsonNode {
            T retval = null;
            
            if (node.json != null) {
                p = (p < node.permissions) ? p : node.permissions;
                retval = (T)CreateFromJsonByType(node.json, typeof(T), p);
            }

            return retval;
        }

        /// <summary>
        /// Clone this node but with a lower level of access that this one has.
        /// WSR: Problem? This doesnt actually clone.. it just creates anohter reference to the same one...
        /// </summary>
        public object Clone(Permissions p)
        {
            CheckForNull();
            DebugAssert(p <= permissions, "Can't increase permissions with clone.");
            return CreateFromJsonByType(this.json, this.GetType(), p);
        }
        
        public object CloneForReal(Permissions p = Permissions.CREATABLE)
        {
            //string serialized = this.Serialize();
            //return CreateFromJsonStringByType(serialized, this.GetType(), p);
            var retjson = DeepCopy(this.json);
            return CreateFromJsonByType(retjson, this.GetType(), p);
        }

        private static object DeepCopy(object data) {
            object retval = null;
            if (data is Dictionary<string, object>) {
                var retdict = new Dictionary<string, object>();
                Dictionary<string, object> dict_data = (Dictionary<string, object>)data;
                foreach (string key in dict_data.Keys) {
                    retdict.Add(key, DeepCopy(dict_data[key]));
                }
                retval = (object)retdict;
            } else if (data is List<object>) {
                var retarr = new List<object>();
                List<object> list_data = (List<object>)data;
                foreach (var item in list_data) {
                    retarr.Add(DeepCopy(item));
                }
                retval = (object)retarr;
            } else {
                retval = data;
            }
            return retval;
        }

        public bool IsReadOnly()
        {
            return permissions == Permissions.READONLY;
        }
        public bool IsModifiable()
        {
            return permissions == Permissions.MODIFIABLE || IsCreateable();
        }
        public bool IsCreateable()
        {
            return permissions == Permissions.CREATABLE;
        }
        /// <summary>
        /// Returns true if this node is a List&lt;object&gt; type - pass null for it to apply to this node's type.
        /// </summary>
        internal bool IsList()
        {
            return IsJsonNodeArray(this.GetType());
        }
        /// <summary>
        /// Returns true if this node is a Dictionary&lt;string, object&gt; type.
        /// </summary>
        internal bool IsDictionary()
        {
            return IsJsonNodeDictionary(this.GetType());
        }
        /// <summary>
        /// Shortcut to determine if a type is a LgJsonNode (or subclass) or not.
        /// </summary>
        public static bool IsJsonNode(System.Type type)
        {
            return type == typeof(LgJsonNode) || type.IsSubclassOf(typeof(LgJsonNode));
        }
        public static bool IsJsonNodeArray(System.Type type) {
            return type.IsSubclassOf(typeof(LgJsonArrayBase));
        }
        public static bool IsJsonNodeDictionary(System.Type type) {
            return type == typeof(LgJsonDictionary) || type.IsSubclassOf(typeof(LgJsonDictionary));
        }
        /// <summary>
        /// Return the number of elements in this node.
        /// </summary>
        public int Count
        {
            get {
                int count = 0;
                if (IsList())
                {
                    List<object> list = json as List<object>;
                    if (list != null)
                    {
                        count = list.Count;
                    }
                }
                else //if (IsDictionary())
                {
                    Dictionary<string, object> dic = json as Dictionary<string, object>;
                    if (dic != null)
                    {
                        count = dic.Count;
                    }
                }
                return count;
            }
        }
        /// <summary>
        /// Null nodes can be created when you reference parentNode.childNode and child isn't in the parent.
        /// </summary>
        public bool IsNull()
        {
            return json == null;
        }
        /// <summary>
        /// Returns true if the node has no data (null) or has an empty list/dictionary.
        /// </summary>
        public bool IsNullOrEmpty()
        {
            return IsNull() || Count == 0;
        }
        /// <summary>
        /// This is to help check if you have permissions to do what you are doing...
        /// </summary>
        protected void CheckForNull()
        {
            // If you have Create access this shouldnt trip even if it doesnt exist
            // If you have Modify or Readonly access, you need to make sure its there before you reference it.
            DebugAssert(json != null, "The node is null - try using IsNullOrEmpty() before referencing this member.", 3);
        }
        /// <summary>
        /// Shorthand acces to this as a dictionary node - can return null if you are wrong!
        /// </summary>
        public LgJsonDictionary AsDictionary()
        {
            CheckForNull();
            return this as LgJsonDictionary;
        }
        /// <summary>
        /// Shorthand acces to this as an array node - can return null if you are wrong!
        /// </summary>
        public LgJsonArrayBase AsArray()
        {
            CheckForNull();
            return this as LgJsonArrayBase;
        }
        /// <summary>
        /// Return true if this is a serializable basic type.
        /// </summary>
        public bool IsPrimitiveType(System.Type type)
        {
            return true;
        }
        /// <summary>
        /// Converts basic types, as well as some extra unity classes to a serialized format. 
        /// This is not a standard serialization format, and must be deserialized using the ConvertTo<> method below.
        /// </summary>
        /// <param name="var">Currently supports iConvertable, Color, Vector2/3</param>
        /// <returns></returns>
        public static string ConvertToString(object var)
        {
            string retVal = "";

            //Check to see if we are a special type
            //Enum
            if (var.GetType().IsEnum)
            {
                retVal = var.ToString();
            }
            //Color
            else if (var.GetType() == typeof(Color))
            {
                retVal = LgTypes.ColorToHex((Color)var);
            }
            //Vector2
            else if (var.GetType() == typeof(Vector2))
            {
                retVal = LgTypes.Vector2ToString((Vector2)var);
            }
            else if (var.GetType() == typeof(Vector3))
            {
                retVal = LgTypes.Vector3ToString((Vector3)var);
            }
            //System IConvertable
            else if (var is System.IConvertible)//This needs to be last for JsonData performance reasons -Kit
            {
                retVal = (string)System.Convert.ChangeType(var, typeof(string));
            }
            else
            {
                // WSR - if you get this, see if you are doing a SetValue instead of SetNode
                DebugAssert(false, var.ToString() + " is not a supported type: " + var.GetType() + ". Add support in LgTypes.cs");
            }
            return retVal;
        }

        /// <summary>
        /// Converts basic types, and also deserializes to some unity classes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="var"></param>
        /// <returns></returns>
        public static T ConvertTo<T>(object var)
        {
            T retVal = default(T);

            try //Are we an IConvertable Type?
            {
                retVal = (T)System.Convert.ChangeType(var, typeof(T));
            }
            catch (System.InvalidCastException e)
            {
                //If not, we must be a string that can be converted to a special type
                string castedObj = "";

                try
                {
                    castedObj = (string)var;
                }
                catch (System.InvalidCastException e2)
                {
                    DebugAssert(false, "If it's not an IConvertable type, the object specified must be castable to a string. Add support in LgTypes.ConvertTo<T> and LgTypes.ConvertToString.");
                }

                //Enum
                if (typeof(T).IsEnum)
                {
                    retVal = (T)System.Enum.Parse(typeof(T), castedObj);
                }
                //Color
                else if (typeof(T) == typeof(Color))
                {
                    retVal = (T)(object)LgTypes.HexToColor(castedObj);
                }
                //Vector 2
                else if (typeof(T) == typeof(Vector2))
                {
                    retVal = (T)(object)LgTypes.StringToVector2(castedObj);
                }
                //Vector 3
                else if (typeof(T) == typeof(Vector3))
                {
                    retVal = (T)(object)LgTypes.StringToVector3(castedObj);
                }
                //Unsupported
                else
                {
                    DebugAssert(false, var.ToString() + " is not a supported type: " + typeof(T) + ". Add support in LgTypes.ConvertTo<T> and LgTypes.ConvertToString.");
                }
            }
            return retVal;
        }
    }

    public class LgJsonDictionary : LgJsonNode
    {
        internal Dictionary<string, object> dictionary
        {
            get {
                DebugAssert(json as Dictionary<string, object> != null, "This is not a dictionary but you are referencing it as though it is.");
                return json as Dictionary<string, object>; }
        }

        /// <summary>
        /// Is the specified key present in the dictionary?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            CheckForNull();
            return dictionary.ContainsKey(key);
        }

        /// <summary>
        /// If the key is present, return its system type, otherwise return null.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>LgJsonDictionary, LgJsonArrayBase, object, or null (if not present)</returns>
        public System.Type GetKeysValueType(string key)
        {
            CheckForNull();
            System.Type retVal = null;
            object obj;
            if (dictionary.TryGetValue(key, out obj))
            {
                if (obj is List<object>)
                {
                    retVal = typeof(LgJsonArrayBase);
                }
                else if (obj is Dictionary<string, object>)
                {
                    retVal = typeof(LgJsonDictionary);
                }
                else
                {
                    retVal = typeof(object);
                }
            }
            return retVal;
        }

        /// <summary>
        /// Get the value as the desired type - this will possibly convert the underlying type without notice.
        /// </summary>
        public T GetValue<T>(string keyVal, T defVal)
        {
            CheckForNull();

            T retVal = defVal;
            object tmpVal = default(T);
            if (dictionary.TryGetValue(keyVal, out tmpVal))
            {
                if (tmpVal != null) //this check was the result of trying to get an int from the xls that was (intentionally) not filled out
                {
                    retVal = ConvertTo<T>(tmpVal);
                }
            }

            return retVal;
        }

        /// <summary>
        /// Updates or creates a var and enters its value;
        /// </summary>
        public void SetValue<T>(string key, T val)
        {
            CheckForNull();

			if (val is System.IConvertible)
			{
				SetObjectValue(key, (object)val);
			}
			else
			{
				SetObjectValue(key, ConvertToString(val));
			}
        }

        /// <summary>
        /// Set value without checking types.. use the typed version above if possible.
        /// </summary>
        public void SetObjectValue(string key, object val)
        {
            CheckForNull();

            //try
            //{
            //    //ToDo: Make not dumb. (Check against all supported types)
            //}
            //catch (System.InvalidCastException e)
            //{
            //    DebugAssert(false, "This is not a supported type");
            //}

            // CheckForOKType()
            DebugAssert(IsModifiable(), "You don't have modify access to this data: " + this.GetType().ToString());
            object value = val;

            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                if (IsCreateable())
                {
                    dictionary.Add(key, value);
                }
                else
                {
                    DebugAssert(false, "You don't have create access to " + this.GetType().ToString() + " data, and that node doesn't exist.");
                }
            }
        }
        public void Remove(string key)
        {
            dictionary.Remove(key);
        }
        public void SetNode<T>(string key, T value) where T : LgJsonNode, new() 
        {
            CheckForNull();
            DebugAssert(IsModifiable(), "You don't have modify access to this data: " + this.GetType().ToString());
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value.json;
            }
            else
            {
                if (IsCreateable())
                {
                    // Insert this node when referenced
                    dictionary.Add(key, value.json);
                }
                else
                {
                    DebugAssert(false, "You don't have create access to " + this.GetType().ToString() + " data, and that node doesn't exist.");
                }
            }
        }

        public T GetNode<T>(string key) where T : LgJsonNode, new()
        {
            CheckForNull();

            T retVal = null;
            if (dictionary.ContainsKey(key))
            {
                object obj = dictionary[key];
                // Return the result as a node
                retVal = CreateFromJson<T>(obj, permissions);
            }
            else // missing element
            {
                // Getting a node NOT in the dictionary
                if (IsCreateable() == true)
                {
                    // Create this node when its referenced
                    retVal = Create<T>(permissions);
                    dictionary.Add(key, retVal.json);
                }
                else
                {
                    // Create and return a null node - DO NOT ADD to dictionary
                    retVal = Create<T>(permissions);
                    retVal.json = null;
                }
            }
            return retVal;
        }

        /// <summary>
        /// Alternate form of GetNode, in case you dont have the exact type at compile time.
        /// </summary>
        public object GetNodeByType(System.Type type, string key)
        {
            object retVal = null;
            if (dictionary.ContainsKey(key))
            {
                //object obj = dictionary[key];
                retVal = CreateFromJsonByType(json, type);
            }
            return retVal;
        }

        /// <summary>
        /// Gets the keys in the dictionary so you can iterate and edit. I put this in a dynamic dict
        /// since you shouldnt be editing static dictionaries, so you shouldnt need those keys.
        /// </summary>
        public string[] GetKeys()
        {
            CheckForNull();
            string[] retStrings = new string[dictionary.Keys.Count];
            dictionary.Keys.CopyTo(retStrings, 0);
            return retStrings;
        }
        public void Clear()
        {
            CheckForNull();
            DebugAssert(IsModifiable(), "You don't have modify access to this data: " + this.GetType().ToString());
            if (dictionary != null)
            {
                dictionary.Clear();
            }
        }
    }

    public abstract class LgJsonArrayBase : LgJsonNode
    {
        internal List<object> array
        {
            get { return json as List<object>; }
        }

        public abstract System.Type GetDataType();

        public abstract object Add();
        public abstract object Remove(int i);
        public abstract object Get(int i);
        public abstract void Set(int i, object data);

        public void Clear()
        {
            CheckForNull();
            DebugAssert(IsModifiable(), "You don't have modify access to this data: " + this.GetType().ToString());
            if (this.array != null)
            {
                this.array.Clear();
            }
        }

        public T[] ToArray<T>()
        {
            CheckForNull();
            return array.ToArray() as T[];
        }
        /// <summary>
        /// Helper function - only useful for retrieving the underlying type.
        /// </summary>
        internal object GetOrAssert(int index)
        {
            DebugAssert(index >= 0 && index < array.Count, "Array index our of bounds: " + index);
            return array[index];
        }
    }

    public class LgJsonArray<T> : LgJsonArrayBase {
        System.Type myType;

        public LgJsonArray()
        {
            this.myType = typeof(T);
        }

        public override System.Type GetDataType()
        {
            return this.myType;
        }

        public T RemoveAt(int index)
        {
            CheckForNull();
            T retVal = default(T);

            DebugAssert(IsModifiable(), "You don't have modify access to this data: " + this.GetType().ToString());

            object obj = GetOrAssert(index);
            if (IsJsonNode(myType))
            {
                retVal = (T)CreateFromJsonByType(obj, myType, permissions);
            }
            else
            {
                retVal = (T)System.Convert.ChangeType(obj, typeof(T));
            }
            array.RemoveAt(index);
            return retVal;
        }
        public T GetAt(int index)
        {
            CheckForNull();
            T retVal = default(T);
            object obj = GetOrAssert(index);
            if (IsJsonNode(myType))
            {
                retVal = (T)CreateFromJsonByType(obj, myType, permissions);
            }
            else
            {
                if (typeof(T).IsEnum)
                {
                    // Enums are stored as string
                    retVal = (T)System.Enum.Parse(myType, (string)obj);
                }
                else
                {
                    //retVal = (T)obj;
                    retVal = (T)System.Convert.ChangeType(obj, typeof(T));
                }
            }
            return retVal;
        }

        /// <summary>
        /// Helper for baseclass access - use GetAt instead.
        /// </summary>
        public override object Get(int i)
        {
            return GetAt(i);
        }
        /// <summary>
        /// Helper for baseclass access - use SetAt instead.
        /// </summary>
        public override void Set(int i, object data)
        {
            SetAt(i, (T)data); 
        }

        public T SetAt(int index, T value)
        {
            CheckForNull();
            DebugAssert(IsModifiable(), "You don't have modify access to this data: " + this.GetType().ToString());
            DebugAssert(index >= 0 && index < array.Count, "Array index our of bounds: " + index);
            if (IsJsonNode(typeof(T)))
            {
                LgJsonNode node = value as LgJsonNode;
                array[index] = node.json;
            }
            else
            {
                array[index] = value;
            }
            return value;
        }
        public T Add(T obj)
        {
            CheckForNull();
            DebugAssert(IsCreateable(), "You don't have create access to this data: " + this.GetType().ToString());
            if (IsJsonNode(myType))
            {
                LgJsonNode jnode = obj as LgJsonNode;
                try
                {
                    array.Add(jnode.json);
                }
                catch
                {
                    Debug.Log("jnode " + (jnode != null));
                    Debug.Log("array " + (array != null) + " " + this.GetType().ToString());
                    Debug.Log("dict " + IsDictionary() + " " + this.GetType().ToString());
                    Debug.Log("list " + IsList() + " " + this.GetType().ToString());
                    Debug.Log("json is: " + json.GetType().ToString());
                }
                // WSR: Not sure on this one, but probably shouldnt augment permissions during an add
                // you should have created this node with the permissions you wanted
                //jnode.permissions = this.permissions;
            }
            else
            {
                array.Add(obj);
            }
            return obj;
        }
        /// <summary>
        /// Adds a new default element to the array.
        /// </summary>
        public T AddNew()
        {
            T t = default(T);
            if (IsJsonNode(myType))
            {
                DebugAssert(permissions == Permissions.CREATABLE, "Why did we lose create permissions on node");
                t = (T)LgJsonNode.CreateByType(myType, permissions);
                DebugAssert(t != null, "Unable to create by type: " + typeof(T).ToString());
                LgJsonNode node = t as LgJsonNode;
                DebugAssert(node.permissions == Permissions.CREATABLE, "Why did we lose create permissions on child");
            }
            else
            {
                // We are just adding a basic type
                if (typeof(T) == typeof(string)) {
                    string s = "";
                    t = (T)((object)s);
                }
            }
            // Do the typed add element
            return Add(t);
        }
        /// <summary>
        /// Support for a blind add of an object - use Add() if you can access it.
        /// </summary>
        public override object Add()
        {
            return Add();
        }

        /// <summary>
        /// Support for a blind remove by index.
        /// </summary>
        public override object Remove(int i)
        {
            return RemoveAt(i);
        }
        
        public T this[int index]
        {
            get
            {
                return GetAt(index);
            }
            
            set
            {
                SetAt(index, value);
            }
        }
    }
} // namespace LgOctEngine.CoreClasses
