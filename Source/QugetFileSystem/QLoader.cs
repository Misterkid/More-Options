using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

/* 
 * Author: Eduard Meivogel
 * Website: https://www.facebook.com/EddyMeivogelProjects
 * Creation Year: 2015
 * If you are improving or use this code please leave my name in here and add your own :) 
 *
 */
//What are Regular Expressions? Time to learn They can make my life easier
//http://www.zytrax.com/tech/web/regex.htm
namespace QMoreOptions.QugetFileSystem
{
    public class QLoader
    {
        // public Dictionary<string, object> loadedValues = new Dictionary<string, object>();
        public QData qData = new QData();
        public QLoader(string path, bool encrypted = false)
        {
            try
            {
                StreamReader streamReader = new StreamReader(path);
                //string wholeFile = streamReader.ReadToEnd();
                //streamReader.Close();
                /*
               // MatchCollection matches = Regex.Matches(wholeFile, @"{([^{}]*)");


                MatchCollection matches = Regex.Matches(wholeFile, "([^{}]*)");
                for(int i = 0; i < matches.Count; i++)
                {

                    Debug.Log(matches[i].Value);
                }*/
                /*
                Match match = Regex.Match(wholeFile,"({[^{}]*)");
                for(int i = 0; i < match.Groups.Count + 1; i++)
                {
                    Debug.Log(match.Groups[i].ToString());
                }
                */
                //StringReader stringReader = new StringReader(wholeFile);
                //ToDo Improve
                string line = "";
                while ((line = streamReader.ReadLine()) != null)
                {

                    string[] split = line.Split('=');
                    split[0] = split[0].Replace(" ", string.Empty);

                    if (split[1].Contains("\""))
                    {
                        Match ragexMatch = Regex.Match(split[1], @"""([^""]*)");
                        qData.AddToValues(split[0], (string)ragexMatch.Groups[1].Value);
                        // loadedValues.Add(split[0], (string)ragexMatch.Groups[1].Value);
                    }
                    else if (split[1].Contains("[") && split[1].Contains("]"))
                    {
                        split[1].Replace("f", string.Empty);
                        Match ragexMatch = Regex.Match(split[1], @"\[([^\]]*)");
                        string[] result = ragexMatch.Groups[1].Value.Split(',');//ragexMatch.Groups[1].Value;
                        if (result.Length > 0)/* && result.Length <= 4)*/
                        {
                            switch (result.Length)
                            {
                                case 0:
                                    Debug.LogError("Nothing filled in");
                                    break;

                                case 1:
                                    if ((result[0].Contains(".")))
                                        //loadedValues.Add(split[0], float.Parse(result[0]));
                                        qData.AddToValues(split[0], float.Parse(result[0]));
                                    else
                                        //loadedValues.Add(split[0], int.Parse(result[0]));
                                        qData.AddToValues(split[0], int.Parse(result[0]));
                                    break;

                                case 2:
                                    //loadedValues.Add(split[0], new Vector2(float.Parse(result[0]), float.Parse(result[1])));
                                    qData.AddToValues(split[0], new Vector2(float.Parse(result[0]), float.Parse(result[1])));
                                    break;
                                case 3:
                                    //loadedValues.Add(split[0], new Vector3(float.Parse(result[0]), float.Parse(result[1]), float.Parse(result[2])));
                                    qData.AddToValues(split[0], new Vector3(float.Parse(result[0]), float.Parse(result[1]), float.Parse(result[2])));
                                    break;

                                default:
                                    bool isFloat = false;
                                    for (int i = 0; i < result.Length; i++)
                                    {
                                        if ((result[i].Contains(".")))
                                        {
                                            isFloat = true;
                                            break;
                                        }
                                    }
                                    if (isFloat)
                                    {
                                        float[] floats = new float[result.Length];
                                        for (int f = 0; f < floats.Length; f++)
                                        {
                                            floats[f] = float.Parse(result[f]);
                                        }

                                        //loadedValues.Add(split[0], floats);
                                        qData.AddToValues(split[0], floats);
                                    }
                                    else
                                    {
                                        int[] ints = new int[result.Length];
                                        for (int i = 0; i < ints.Length; i++)
                                        {
                                            ints[i] = int.Parse(result[i]);
                                        }

                                        //loadedValues.Add(split[0], ints);
                                        qData.AddToValues(split[0], ints);
                                    }
                                    break;
                            }
                        }

                    }
                }
                streamReader.Close();
            }
            catch(Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }
}
