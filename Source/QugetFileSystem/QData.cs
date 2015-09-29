using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/* 
 * Author: Eduard Meivogel
 * Website: https://www.facebook.com/EddyMeivogelProjects
 * Creation Year: 2015
 * If you are improving or use this code please leave my name in here and add your own :) 
 *
 */
namespace QugetFileSystem
{
    public class QData
    {
        private Dictionary<string, object> loadedValues = new Dictionary<string, object>();
        private string name;

        public QData()
        {
            //name = dataName;
            //loadedValues = values;
        }
        public Dictionary<string, object> AllData()
        {
            return loadedValues;
        }
        public object GetValueByKey(string key)
        {
            if (loadedValues.ContainsKey(key))
                return (loadedValues[key]);

            return null;
        }
        public void AddToValues(string key, object value)
        {
            if (!loadedValues.ContainsKey(key))
                loadedValues.Add(key, value);
            else
                loadedValues[key] = value;
        }
    }
}
