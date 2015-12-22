using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/* 
 * Author: Eduard Meivogel
 * Website: https://www.facebook.com/EddyMeivogelProjects
 * Creation Year: 2015
 * If you are improving or use this code please leave my name in here and add your own :) 
 *
 */
namespace QMoreOptions.QugetFileSystem
{
    public class QSaver
    {
        private StreamWriter streamWriter;
        private QData qData;
        public QSaver()
        {

        }
        public void QSaverSave(string path, QData qData, bool encrypted = false)
        {
            streamWriter = new StreamWriter(path, false);
            foreach (KeyValuePair<string, object> keyValue in qData.AllData())
            {
                AddLine(keyValue.Key, keyValue.Value);
            }
            streamWriter.Flush();
            streamWriter.Close();
        }
        private void AddLine(string key, object value, string comment = "")
        {
            String stringToWrite = key + " = ";// = String.Format("{0} = {1}",key,value);
            if (value is string)
            {
                stringToWrite += "\"" + value + "\"";
            }
            else if (value is int || value is Single)
            {
                stringToWrite += "[" + value + "]";
            }
            else if (value is int[])
            {
                int[] newValueInt = value as int[];
                stringToWrite += "[";
                for (int i = 0; i < newValueInt.Length; i++)
                {
                    stringToWrite += newValueInt[i];// +",";
                    if (i != (newValueInt.Length - 1))
                    {
                        stringToWrite += ",";
                    }
                }
                stringToWrite += "]";
            }
            else if (value is Vector2)
            {
                Vector2 newValueVector2 = (Vector2)value;// as Vector2;
                stringToWrite += "[" + newValueVector2.x + "," + newValueVector2.y + "]";

            }
            else if (value is Vector3)
            {
                Vector3 newValueVector3 = (Vector3)value;// as Vector2;
                stringToWrite += "[" + newValueVector3.x + "," + newValueVector3.y + "," + newValueVector3.z + "]";
            }
            else
            {
                Debug.Log("huh?");
            }
            if (comment != "")
            {
                stringToWrite += " #" + comment;
            }
            streamWriter.WriteLine(stringToWrite);
        }
    }
}