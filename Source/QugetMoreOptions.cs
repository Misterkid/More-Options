using System;
using System.IO;
using System.Reflection;
using ICities;
using UnityEngine;
/* 
 * Author: Eduard Meivogel
 * Website: https://www.facebook.com/EddyMeivogelProjects
 * Creation Year: 2015
 * If you are improving or use this code please leave my name in here and add your own :) 
 *
 */
namespace QMoreOptions
{

    public class QugetMoreOptions : IUserMod 
    {
        public string Name
        {
            get { return "More Options"; }
        }

        public string Description 
        {
            get { return "Opens an extra options window when pressing ALT + O made by Quget"; }
        }
    }

    // Inherit interfaces and implement your mod logic here
    // You can use as many files and subfolders as you wish to organise your code, as long
    // as it remains located under the Source folder.

	public class MoreOptionsMod :LoadingExtensionBase
	{
		public GameObject optionsWindowGo; 
		public override void OnLevelLoaded(LoadMode mode)
		{
			optionsWindowGo = new GameObject();
			optionsWindowGo.AddComponent<OptionsWindow>();
		}
		public override void OnLevelUnloading()
		{
			try
			{
				OptionsWindow optionsWindow = optionsWindowGo.GetComponent<OptionsWindow>();
				optionsWindow.Destroy();
			}
			catch(Exception e)
			{
				
			}
		}
	}
}
