using UnityEngine;
using System.Collections;
using System;
using System.Globalization;
using System.Reflection;
using ChirpLogger;
/* 
 * Author: Eduard Meivogel
 * Website: https://www.facebook.com/EddyMeivogelProjects
 * Creation Year: 2014
 * E for Eddy! 
 * If you are improving or use this code please leave my name in here and add your own :) 
 */
/// <summary>
/// E for Eddy!
/// This is a class containing some awesome functions!
/// </summary> 
public static class EUtils
{
    //Unity color to hex
    /// <summary>
    /// Convert Unity3d Color32 to Color Hex
    /// </summary>
    ///
    public static string ColorToHex(Color32 color)
    {
        //Unity colors goes from 0-1 but we want 0 to 255(Hex goes from 0 to 255(00 to FF) ) so colors * will make it go from 0 to 255
        //X2 means hex size is 2! so 0 is 00 and not 0
        byte r = color.r;
        byte g = color.g;
        byte b = color.b;
        byte a = color.a;
        return "#" + r.ToString("X2") + g.ToString("X2") + b.ToString("X2") + a.ToString("X2");
    }
    /// <summary>
    /// Convert Unity3d Color to Color Hex
    /// </summary>
    public static string ColorToHex(Color color)
    {
        //Unity colors goes from 0-1 but we want 0 to 255(Hex goes from 0 to 255(00 to FF) ) so colors * will make it go from 0 to 255
        //X2 means hex size is 2! so 0 is 00 and not 0
        int r = (int)(color.r * 255);
        int g = (int)(color.g * 255);
        int b = (int)(color.b * 255);
        int a = (int)(color.a * 255);
        return "#" + Mathf.FloorToInt(r).ToString("X2") + Mathf.FloorToInt(g).ToString("X2") + Mathf.FloorToInt(b).ToString("X2") + Mathf.FloorToInt(a).ToString("X2");
    }
    
    /// <summary>
    /// Convert Color Hex(#RRGGBBAA) to Unity3d Color32!
    /// </summary> 
    public static Color32 HexToColor(string hex)
    {
        hex = hex.Replace("#", string.Empty);//We don't need #
        if (hex.Length != 8)// The length should be 8 !
        {
            throw new Exception("Are you missing transparancy? Please add it it should look like #RRGGBBAA");
        }
        //Convert hex to int
        byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);//01
        byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);//23
        byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);//45
        byte a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);//67

        //Unity wants 0 - 1 so we devide it by 255.
        return new Color32(r , g , b , a );
    }
    /// <summary>
    /// Put colors in your GUI text using this function!
    /// </summary> 
    public static String UnityColoredText(string text,Color32 color)
    {
        string retText = "<color=" + ColorToHex(color) + ">" + text + "</color>";
        return retText;
    }
    /// <summary>
    /// Put colors in your GUI text using this function!
    /// </summary> 
    public static String UnityColoredText(string text,string htmlHex)
    {
        string retText = "<color=" + htmlHex + ">" + text + "</color>";
        return retText;
    }
	
	public static object GetFieldValue(MonoBehaviour monoObject, string fieldName)
	{
		try
		{
			BindingFlags bindingFlags = BindingFlags.Public |
					BindingFlags.NonPublic |
					BindingFlags.Instance |
					BindingFlags.Static;
					
			FieldInfo fieldInfo = monoObject.GetType().GetField(fieldName,bindingFlags);
			//fieldInfo.SetValue(monoObject,value);
			return fieldInfo.GetValue(monoObject);
			//return true;
		}
		catch(Exception e)
		{
			ChirpLog.Error( "Unable to get Value: " + monoObject.GetType().Name + ":" + fieldName + ":" + e.Message);
			ChirpLog.Flush();
			return 0;
		}
		
	}
	public static object CallMethod(MonoBehaviour monoObject, string methodName, object[] value = null)
	{
		object returnValue = null;
		try
		{
			MethodInfo[] methods = monoObject.GetType().GetMethods();
			foreach (MethodInfo info in methods)
			{
				if (info.Name == methodName)
				{
					returnValue = info.Invoke(monoObject, value); // [2]
				}
			}
			return returnValue;
		}
		catch(Exception e)
		{
			ChirpLog.Error("Unable to Call Method: " + monoObject.GetType().Name + ":" + methodName + ":" + e.Message);
			ChirpLog.Flush();
			return returnValue;
		}
		
	}
	public static void SetFieldValue(MonoBehaviour monoObject, string fieldName, object value)
	{
		try
		{
			BindingFlags bindingFlags = BindingFlags.Public |
					BindingFlags.NonPublic |
					BindingFlags.Instance |
					BindingFlags.Static;
					
			FieldInfo fieldInfo = monoObject.GetType().GetField(fieldName,bindingFlags);
			fieldInfo.SetValue(monoObject,value);
		}
		catch(Exception e)
		{
			ChirpLog.Error("Unable to set value:" + monoObject.GetType().Name + ":" + fieldName + ":" + e.Message);
			ChirpLog.Flush();
		}
	}
}
