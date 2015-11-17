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

using LgOctEngine.CoreClasses;

/// <summary>
/// A simple test class for the LgBaseClass functionality. You can run this script by attaching it to an object in the scene, and hitting play.
/// </summary>
public class LgJsonData_Test : MonoBehaviour {
    TestClass testClass;

	// Use this for initialization
	void Start () {
        testClass = new TestClass();
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnGUI()
    {
        Rect buttonRect = new Rect(30, 30, 200, 45);

        if (GUI.Button(buttonRect, "SimpleClass Serialize Test"))
        {
            TestClass.SimpleSerializeTest();
        }
        buttonRect.y += buttonRect.height + 10;
        if (GUI.Button(buttonRect, "Simple Array Test"))
        {
            TestClass.SimpleArrayTest();
        }
        //buttonRect.y += buttonRect.height + 10;
        //if (GUI.Button(buttonRect, "Test DebugAssert"))
        //{
        //    TestClass.TestDebugAssert();
        //}
        //buttonRect.y += buttonRect.height + 10;
        //if (GUI.Button(buttonRect, "Test Recursive DebugAssert"))
        //{
        //    TestClass.TestRecursiveDebugAssert();
        //}
        //buttonRect.y += buttonRect.height + 10;
        //if (GUI.Button(buttonRect, "Test Assert"))
        //{
        //    TestClass.TestAssert();
        //}
        //buttonRect.y += buttonRect.height + 10;
        //if (GUI.Button(buttonRect, "Test Exception"))
        //{
        //    TestClass.TestException();
        //}
    }

    private class TestClass : LgBaseClass
    {
        public enum EnumExample
        {
            None,
            ValueOne,
            ValueTwo
        }
        /// <summary>
        /// A json node of various basic types.
        /// </summary>
        public class SimpleClass : LgJsonDictionary
        {
            // Basic types
            public string Name { get { return GetValue<string>("Name", ""); } set { SetValue<string>("Name", value); } }
            public int Counter { get { return GetValue<int>("Counter", 0); } set { SetValue<int>("Counter", value); } }
            public float Percent { get { return GetValue<float>("Percent", 0); } set { SetValue<float>("Percent", value); } }
            public double Time { get { return GetValue<double>("Time", 0); } set { SetValue<double>("Time", value); } }
            public bool CanDisplay { get { return GetValue<bool>("CanDisplay", false); } set { SetValue<bool>("CanDisplay", value); } }
            public EnumExample Type { get { return GetValue<EnumExample>("Type", EnumExample.None); } set { SetValue<EnumExample>("Type", value); } }

            // extended types
            public Color MyColor { get { return GetValue<Color>("MyColor", Color.white); } set { SetValue<Color>("MyColor", value); } }
            public Vector2 Pos2d { get { return GetValue<Vector2>("Pos2d", Vector2.zero); } set { SetValue<Vector2>("Pos2d", value); } }
            public Vector3 Pos3d { get { return GetValue<Vector3>("Pos3d", Vector3.zero); } set { SetValue<Vector3>("Pos3d", value); } }
        }
        /// <summary>
        /// An json node that contains two arrays - one simple, one complex.
        /// </summary>
        public class SimpleArrayClass : LgJsonDictionary
        {
            public LgJsonArray<string> StringArray { get { return GetNode<LgJsonArray<string>>("StringArray"); } set { SetNode<LgJsonArray<string>>("StringArray", value); } }
            public LgJsonArray<SimpleClass> SimpleClassArray { get { return GetNode<LgJsonArray<SimpleClass>>("SimpleClassArray"); } set { SetNode<LgJsonArray<SimpleClass>>("SimpleClassArray", value); } }
        }

        private static SimpleClass CreateSimpleClass()
        {
            SimpleClass sc = LgJsonNode.Create<SimpleClass>();
            sc.Name = "MyName";
            sc.Counter = 456;
            sc.Percent = 75.4f;
            sc.Time = LgDateTime.GetSecondsFromEpoch();
            sc.CanDisplay = true;
            sc.Type = EnumExample.ValueOne;

            sc.MyColor = new Color(0.5f, .75f, 1f);
            sc.Pos2d = new Vector2(200, -1379);
            sc.Pos3d = new Vector3(1, 2, 3);
            return sc;
        }

        public static void SimpleSerializeTest()
        {
            // Dynamically create a node with some data
            SimpleClass sc = CreateSimpleClass();

            // Now test serialization
            string simpleSerialized = sc.Serialize();
            Debug.Log("Serialized simple class: " + simpleSerialized);

            // Now test deserialization
            SimpleClass scAfter = LgJsonNode.CreateFromJsonString<SimpleClass>(simpleSerialized);

            // Do they match?
            DebugAssert(sc.Name == scAfter.Name, "Missmatch Name!");
            DebugAssert(sc.Counter == scAfter.Counter, "Missmatch Count!");
            DebugAssert(sc.Percent == scAfter.Percent, "Missmatch Percent!");
            DebugAssert(sc.Time == scAfter.Time, "Missmatch Time!");
            DebugAssert(sc.CanDisplay == scAfter.CanDisplay, "Missmatch CanDisplay!");
            DebugAssert(sc.Type == scAfter.Type, "Missmatch Type!");
            DebugAssert(sc.MyColor == scAfter.MyColor, "Missmatch MyColor!");
            DebugAssert(sc.Pos2d == scAfter.Pos2d, "Missmatch Pos2d!");
            DebugAssert(sc.Pos3d == scAfter.Pos3d, "Missmatch name!");
            Debug.Log("Compare complete!");
        }
        public static void SimpleArrayTest()
        {
            SimpleArrayClass simpleArrayClass = LgJsonNode.Create<SimpleArrayClass>();
            for (int i = 0; i < 5; i++)
            {
                // Method #1 - Directly add the type to array
                simpleArrayClass.StringArray.Add("string value " + i);
                SimpleClass sc = CreateSimpleClass();
                simpleArrayClass.SimpleClassArray.Add(sc);
            }
            for (int i = 0; i < 5; i++)
            {
                // Method #2 - Use the array to add an entry and THEN fill it out
                simpleArrayClass.StringArray.AddNew();
                simpleArrayClass.StringArray[simpleArrayClass.StringArray.Count - 1] = "string value " + i;
                SimpleClass sc = simpleArrayClass.SimpleClassArray.AddNew();
                sc.Name = "Example Name";
                sc.Percent = 0.12f;
                // No need to 'save' it, we are writing directly to it
            }
            // Serialize it
            string serialized = simpleArrayClass.Serialize();

            // Deserialize it
            SimpleArrayClass simpleArrayClassDeserialized = LgJsonNode.CreateFromJsonString<SimpleArrayClass>(serialized);

            // Paste the output in www.jsonlint.com to easily view and debug it!
            Debug.Log("Serialized output: " + serialized);
            Debug.Log("Deserialized output: " + simpleArrayClassDeserialized.Serialize());
        }
    }
}
