using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using ChirpLogger;
using QugetFileSystem;
using System;
using System.CodeDom;
using System.Linq;

/* 
 * Author: Eduard Meivogel
 * Website: https://www.facebook.com/EddyMeivogelProjects
 * Creation Year: 2015
 * If you are improving or use this code please leave my name in here and add your own :) 
 * ToDo clean up and make more classes.... I should have thought this trough first xD
 */
public class OptionsWindow : MonoBehaviour 
{
    //public bool activated = false;
    public Vector2 sWidthHeight = new Vector2(250, 25);
    public Vector2 bWidthHeight = new Vector2(100, 25);
    public Rect optionWindowRect = new Rect(0,0, 550, 550);
    public Color32 headingColor = new Color32(255, 255, 255, 255);
    private Vector2 optionScrollPosition = Vector2.zero;
    public bool activated = false;
   // private Vector2 resScrollPosition = Vector2.zero;
    private float maxYPosScroll = 0;
    //Resolution
    private bool fullScreen;
    //Rendering
    private float antiAliasing = 0;
    private float maxAntiAliasing = 8;
    private AnisotropicFiltering anisotropicFilt;
    private float textureQuality = 0;
    private float maxTextureQuality = 10;
    private float pixelLightCount = 0;
    private float maxPixelLightCount = 4;
    //Shadows
    private ShadowProjection shadowProjection;
    private float maxShadowDistance = 0;
    private float shadowDistanceLimit = 20000;
    private float shadowCascade = 0;
    private float maxShadowCascade = 4;
    //Other
    private float vSync = 0;
    private float maxvSync = 2;
    
    private float LoDLevel = 0;
    private float LoDLevelMax = 10;
    
    private float LoDBias = 0;
    private float LoDBiasMax = 10;
    
    private float particleRaycastBudget = 0;
    private float maxParticleRaycastBudget = 4096;

    private float frameRate = 0;
    private float maxFrameRate = 300;
    //Get FPS
    public double refreshRateMS = 500;
    private float lastFrameRate = 0.0f;
    private bool setFrameRate = true;
    private Timer frameUpdateTimer;
    //Extra Value's
    //private Dictionary<string, MonoBehaviour> cameraEffects = new Dictionary<string, MonoBehaviour>();
    private MonoBehaviour[] cameraBehaviours;
    private List<string> cameraEffects = new List<string>();
    private bool killSeagull = false;
    /* Render Properties */
    private MonoBehaviour renderProperties;
    /* Fog Related */
    private float m_fogHeight = 0;
    private float m_fogHeightMax = 10000;
    
    private float m_edgeFogDistance = 0;
    private float m_edgeFogDistanceMax = 10000;
    
    private bool m_useVolumeFog = true;
    
    private float m_volumeFogDensity = 0;
    private float m_volumeFogDensityMax = 1;
    
    private float m_volumeFogStart = 0;
    private float m_volumeFogStartMax = 4000;
    
    private float m_volumeFogDistance = 0;
    private float m_volumeFogDistanceMax = 8000;
    private float m_pollutionFogIntensity = 0;
    private float m_pollutionFogIntensityMax = 1;
    
    private string saveTag = "QGT_SCL_";
    private QData qData;
    private string filePath;
    /*
    private ScreenSpaceAmbientOcclusion sSAOC;
    private ScreenSpaceAmbientObscurance sSAOB;
    private EdgeDetection edgeDetection;
    private CreaseShading creaseShading;
    */
    /* Fog */
    /*
    private float m_3DFogAmount = 0f;
    private float m_3DFogAmountMax = 1f;
    
    private float m_3DFogStart  = 0f;
    private float m_3DFogStartMax = 1f;
    
    private float m_3DFogDistance = 0;
    private float m_3DFogDistanceMax = 100000f;
    
    private float m_3DNoiseStepSize = 0f;
    private float m_3DNoiseStepSizeMax = 120f;
    
    private float m_3DNoiseScale = 0;
    private float m_3DNoiseScaleMax = 1f;
    */
    //Game
    //private bool useButtons;
    //private bool useCollision;
    // Use this for initialization
    void Start () 
    {
        //Resolution
        fullScreen = Screen.fullScreen;
        /*
        //Rendering
        antiAliasing = QualitySettings.antiAliasing;
        anisotropicFilt = QualitySettings.anisotropicFiltering;
        textureQuality = QualitySettings.masterTextureLimit;
        pixelLightCount = QualitySettings.pixelLightCount;
        //Shadow
        shadowProjection = QualitySettings.shadowProjection;
        shadowDistance = QualitySettings.shadowDistance;
        shadowCascade = QualitySettings.shadowCascades;
        //Other
        vSync = QualitySettings.vSyncCount;
        particleRaycastBudget = QualitySettings.particleRaycastBudget;
        frameRate = Application.targetFrameRate;
        
        LoDLevel = QualitySettings.maximumLODLevel;
        LoDBias = QualitySettings.lodBias;*/
        //FPS
        frameUpdateTimer = new Timer(refreshRateMS);
        frameUpdateTimer.Elapsed += new ElapsedEventHandler(frameUpdateTimer_Elapsed);
        frameUpdateTimer.AutoReset = true;
        frameUpdateTimer.Start();
        optionWindowRect = new Rect((Screen.width / 2) - (optionWindowRect.width / 2), (Screen.height / 2) - (optionWindowRect.height / 2), optionWindowRect.width, optionWindowRect.height);
        cameraBehaviours = Camera.main.GetComponents<MonoBehaviour>() as MonoBehaviour[];
        //Get MonoBehaviours here.
        MonoBehaviour[] behaviours = GameObject.FindObjectsOfType<MonoBehaviour>() as MonoBehaviour[];
        for( int i = 0; i < behaviours.Length; i++ )
        {
            if(behaviours[i].GetType().Name == "RenderProperties" )
                renderProperties = behaviours[i];
        }
        
        //m_fogHeight = (float)EUtils.GetFieldValue(renderProperties,"m_fogHeight");
        /*
        m_fogHeight = (float)EUtils.GetFieldValue(renderProperties,"m_fogHeight");
        EUtils.SetFieldValue(renderProperties,"m_fogHeight",m_fogHeight);
        
        m_edgeFogDistance = (float)EUtils.GetFieldValue(renderProperties,"m_edgeFogDistance");
        EUtils.SetFieldValue(renderProperties,"m_edgeFogDistance",m_edgeFogDistance);
        
        m_useVolumeFog = (bool)EUtils.GetFieldValue(renderProperties,"m_useVolumeFog");
        EUtils.SetFieldValue(renderProperties,"m_useVolumeFog",m_useVolumeFog);
        
        m_volumeFogDensity = (float)EUtils.GetFieldValue(renderProperties,"m_volumeFogDensity");
        EUtils.SetFieldValue(renderProperties,"m_volumeFogDensity",m_volumeFogDensity);
        
        m_volumeFogStart = (float)EUtils.GetFieldValue(renderProperties,"m_volumeFogStart");
        EUtils.SetFieldValue(renderProperties,"m_volumeFogStart",m_volumeFogStart);
        
        m_volumeFogDistance = (float)EUtils.GetFieldValue(renderProperties,"m_volumeFogDistance");
        EUtils.SetFieldValue(renderProperties,"m_volumeFogDistance",m_volumeFogDistance);	
        
        m_pollutionFogIntensity = (float)EUtils.GetFieldValue(renderProperties,"m_pollutionFogIntensity");
        EUtils.SetFieldValue(renderProperties,"m_pollutionFogIntensity",m_pollutionFogIntensity);	
        */
        /* Fog */
        /*
        m_3DFogAmount =  (float)EUtils.GetFieldValue(GetCameraBehaviour("FogEffect"),"m_3DFogAmount"); 
        m_3DFogStart  = (float)EUtils.GetFieldValue(GetCameraBehaviour("FogEffect"),"m_3DFogStart"); 
        m_3DFogDistance = (float)EUtils.GetFieldValue(GetCameraBehaviour("FogEffect"),"m_3DFogDistance"); 
        m_3DNoiseStepSize = (float)EUtils.GetFieldValue(GetCameraBehaviour("FogEffect"),"m_3DNoiseStepSize"); 
        m_3DNoiseScale = (float)EUtils.GetFieldValue(GetCameraBehaviour("FogEffect"),"m_3DNoiseScale"); 
        */
        
        //Added Effects
        /*
        sSAOC = Camera.main.gameObject.AddComponent<ScreenSpaceAmbientOcclusion>();
        sSAOB = Camera.main.gameObject.AddComponent<ScreenSpaceAmbientObscurance>();
        edgeDetection = Camera.main.gameObject.AddComponent<EdgeDetection>();
        creaseShading = Camera.main.gameObject.AddComponent<CreaseShading>();
        
        sSAOC.enabled = false;
        sSAOB.enabled = false;
        edgeDetection.enabled = false;
        creaseShading.enabled = false;*/
        
        
        //QugetFileLoader Alpha.
        filePath = Application.persistentDataPath + "\\qMoreOptionsConfig.qgt";
        QLoader qLoader = new QLoader(filePath);
        qData = qLoader.qData;
        if(qData == null)
            qData = new QData();
        
        if(qData.GetValueByKey("DONT_REMOVE_THIS") == null)
        {
            //OVERRIDE
            qData.AddToValues("DONT_REMOVE_THIS","ELSE_IT_RESETS!");
            ResetToDefault();
        }
        else
        {
            
            //Default
            antiAliasing = Load(saveTag + GetName (new { antiAliasing }));
            //anisotropicFilt = Load(saveTag + GetName (new { anisotropicFilt }));
            float anisoFloat = Load(saveTag + GetName (new { anisotropicFilt }));
            if (anisoFloat ==0)
            {
                anisotropicFilt = AnisotropicFiltering.Disable;
            }
            if (anisoFloat == 1)
            {
                anisotropicFilt = AnisotropicFiltering.Enable;
            }
            if (anisoFloat == 2)
            {
               anisotropicFilt = AnisotropicFiltering.ForceEnable;
            }
            
            textureQuality = Load(saveTag + GetName (new { textureQuality }));
            pixelLightCount = Load(saveTag + GetName (new { pixelLightCount }));
            //Shadow
            float shadowProjFloat  = Load(saveTag + GetName (new { shadowProjection }));
            if (shadowProjFloat == 0)
            {
                shadowProjection = ShadowProjection.CloseFit;
            }
            if (shadowProjFloat == 1)
            {
                shadowProjection = ShadowProjection.StableFit;
            }
            maxShadowDistance = Load(saveTag + GetName (new { shadowDistance = maxShadowDistance }));
            shadowCascade = Load(saveTag + GetName (new { shadowCascade }));
            //Other
            vSync = Load(saveTag + GetName (new { vSync }));
            particleRaycastBudget = Load(saveTag + GetName (new { particleRaycastBudget }));
            frameRate = Load(saveTag + GetName (new { frameRate }));
            
            LoDLevel = Load(saveTag + GetName (new { LoDLevel }));
            LoDBias = Load(saveTag + GetName (new { LoDBias }));
            
            //Fog
            m_fogHeight = Load(saveTag + GetName (new { m_fogHeight }));
            m_edgeFogDistance = Load(saveTag + GetName (new { m_edgeFogDistance }));
            
            float convertToBool = Load(saveTag + GetName (new { m_useVolumeFog }));
            if(convertToBool == 0)
                m_useVolumeFog = false;
            else
                m_useVolumeFog = true;
            
            m_volumeFogDensity = Load(saveTag + GetName (new { m_volumeFogDensity }));
            m_volumeFogStart = Load(saveTag + GetName (new { m_volumeFogStart }));
            m_volumeFogDistance = Load(saveTag + GetName (new { m_volumeFogDistance }));
            m_pollutionFogIntensity = Load(saveTag + GetName (new { m_pollutionFogIntensity }));
            
            //Update
            QualitySettings.antiAliasing = (int)antiAliasing;
            QualitySettings.anisotropicFiltering = anisotropicFilt;
            QualitySettings.masterTextureLimit = (int)textureQuality;
            QualitySettings.pixelLightCount = (int)pixelLightCount;
            //Shadow
            QualitySettings.shadowProjection = shadowProjection;
            
            QualitySettings.shadowDistance = maxShadowDistance;
            QualitySettings.shadowCascades = (int)shadowCascade;
            //Other
            QualitySettings.vSyncCount = (int)vSync;
            QualitySettings.particleRaycastBudget = (int)particleRaycastBudget;
            Application.targetFrameRate = (int)frameRate;

            QualitySettings.maximumLODLevel = (int)LoDLevel;
            QualitySettings.lodBias = LoDBias;
            //Fog
            EUtils.SetFieldValue(renderProperties,"m_fogHeight",m_fogHeight);
            EUtils.SetFieldValue(renderProperties,"m_edgeFogDistance",m_edgeFogDistance);
            EUtils.SetFieldValue(renderProperties,"m_useVolumeFog",m_useVolumeFog);
            EUtils.SetFieldValue(renderProperties,"m_volumeFogDensity",m_volumeFogDensity);
            EUtils.SetFieldValue(renderProperties,"m_volumeFogStart",m_volumeFogStart);
            EUtils.SetFieldValue(renderProperties,"m_volumeFogDistance",m_volumeFogDistance);	
            EUtils.SetFieldValue(renderProperties,"m_pollutionFogIntensity",m_pollutionFogIntensity);	
        }
    }
    
    private string GetName<T>(T item) where T : class 
    {
      return typeof(T).GetProperties()[0].Name;
    }
    private void ResetToDefault()
    {
        //Default
        antiAliasing = QualitySettings.antiAliasing;
        anisotropicFilt = QualitySettings.anisotropicFiltering;
        textureQuality = QualitySettings.masterTextureLimit;
        pixelLightCount = QualitySettings.pixelLightCount;
        //Shadow
        shadowProjection = QualitySettings.shadowProjection;
        maxShadowDistance = QualitySettings.shadowDistance;
        shadowCascade = QualitySettings.shadowCascades;
        //Other
        vSync = QualitySettings.vSyncCount;
        particleRaycastBudget = QualitySettings.particleRaycastBudget;
        frameRate = Application.targetFrameRate;
        
        LoDLevel = QualitySettings.maximumLODLevel;
        LoDBias = QualitySettings.lodBias;
        //fog
        m_fogHeight = 5000;
        m_edgeFogDistance = 0;
        m_useVolumeFog = true;
        m_volumeFogDensity = 0.0002f;
        m_volumeFogStart = 1711f;
        m_volumeFogDistance = 2903f;
        m_pollutionFogIntensity = 0.48f;
        
        
        qData.AddToValues("DONT_REMOVE_THIS","ELSE_IT_RESETS!");
        
        
        //Save
        Save(saveTag + GetName (new { antiAliasing }),antiAliasing);
        
        float anisoFloat = -1;
        if (anisotropicFilt == AnisotropicFiltering.Disable)
        {
            anisoFloat = 0;
        }
        if (anisotropicFilt == AnisotropicFiltering.Enable)
        {
            anisoFloat = 1;
        }
        if (anisotropicFilt == AnisotropicFiltering.ForceEnable)
        {
           anisoFloat = 2;
        }
        Save(saveTag + GetName (new { anisotropicFilt }),anisoFloat);
        
        Save(saveTag + GetName (new { textureQuality }),textureQuality);
        Save(saveTag + GetName (new { pixelLightCount }),pixelLightCount);
        
        float shadowProjFloat = -1;
        if (shadowProjection == ShadowProjection.CloseFit)
        {
            shadowProjFloat = 0;
        }
        if (shadowProjection == ShadowProjection.StableFit)
        {
           shadowProjFloat = 1;
        }
        Save(saveTag + GetName (new { shadowProjection }),shadowProjFloat);
        
        Save(saveTag + GetName (new { shadowDistance = maxShadowDistance }),maxShadowDistance);
        Save(saveTag + GetName (new { shadowCascade }),shadowCascade);
        Save(saveTag + GetName (new { vSync }),vSync);
        Save(saveTag + GetName (new { particleRaycastBudget }),particleRaycastBudget);
        Save(saveTag + GetName (new { frameRate }),frameRate);
        Save(saveTag + GetName (new { LoDLevel }),LoDLevel);
        Save(saveTag + GetName (new { LoDBias }),LoDBias);
        //Fog
        Save(saveTag + GetName (new { m_fogHeight }),m_fogHeight);
        Save(saveTag + GetName (new { m_edgeFogDistance }),m_edgeFogDistance);		
        Save(saveTag + GetName (new { m_useVolumeFog }),1);
        Save(saveTag + GetName (new { m_volumeFogDensity }),m_volumeFogDensity);
        Save(saveTag + GetName (new { m_volumeFogStart }),m_volumeFogStart);
        Save(saveTag + GetName (new { m_volumeFogDistance }),m_volumeFogDistance);
        Save(saveTag + GetName (new { m_pollutionFogIntensity }),m_pollutionFogIntensity);
            
        EUtils.SetFieldValue(renderProperties,"m_fogHeight",m_fogHeight);
        EUtils.SetFieldValue(renderProperties,"m_edgeFogDistance",m_edgeFogDistance);
        EUtils.SetFieldValue(renderProperties,"m_useVolumeFog",m_useVolumeFog);
        EUtils.SetFieldValue(renderProperties,"m_volumeFogDensity",m_volumeFogDensity);
        EUtils.SetFieldValue(renderProperties,"m_volumeFogStart",m_volumeFogStart);
        EUtils.SetFieldValue(renderProperties,"m_volumeFogDistance",m_volumeFogDistance);	
        EUtils.SetFieldValue(renderProperties,"m_pollutionFogIntensity",m_pollutionFogIntensity);	
        /* Fog */
    }
    private void Save(string key, float value)
    {
        if(qData != null)
            qData.AddToValues(key,value);
        //PlayerPrefs.SetFloat(key, value);
    }
    private float Load(string key)
    {
        var value = qData.GetValueByKey(key);
        if(!(value is float))
            return -1;
        else
            return (float)value;
    }

    MonoBehaviour GetCameraBehaviour(string name)
    {
        for( int i = 0; i < cameraBehaviours.Length; i++ )
        {
            if(cameraBehaviours[i].GetType().Name == name)
            {
                return cameraBehaviours[i];
            }
            
        }
        return null;
    }
    void frameUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
        setFrameRate = true;
    }
    
    // Update is called once per frame
    void Update () 
    {
        if (setFrameRate)
        {
            lastFrameRate = (int)(1.0f / Time.deltaTime);
            setFrameRate = false;
        }
        if(Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.O)/*Input.GetKeyDown(KeyCode.Escape)*/)
        {
            /*
            GameObject[] currentObjects = GameObject.FindObjectsOfType<GameObject>() as GameObject[];
            MonoBehaviour[] behaviours;
            for(int s = 0; s < currentObjects.Length; s++)
            {
                //ChirpLog.Debug("#####" + currentObjects[s].name +"#####");
                if(currentObjects[s].name == "Seagull")
                {
                    behaviours = currentObjects[s].GetComponents<MonoBehaviour>() as MonoBehaviour[];
                    for(int i = 0; i < behaviours.Length; i++)
                    {
                        ChirpLog.Debug(behaviours[i].GetType().Name);
                        
                        BindingFlags bindingFlags = BindingFlags.Public |
                            BindingFlags.NonPublic |
                            BindingFlags.Instance |
                            BindingFlags.Static;
                        foreach (FieldInfo field in behaviours[i].GetType().GetFields(bindingFlags))
                        {
                            ChirpLog.Debug(field.Name +":" +  EUtils.GetFieldValue(behaviours[i],field.Name) );
                        }
                    }
                }
            }
            ChirpLog.Flush();
            */
            activated = activated?false:true;
        }
    }
    private void OnGUI()
    {
        if (activated)
        {
            optionWindowRect = GUI.Window(133843242, optionWindowRect, OptionWindow, "More Options");
        }
    }
    private void OptionWindow(int id)
    {
        optionScrollPosition = GUI.BeginScrollView(new Rect(-5, 20, optionWindowRect.width, optionWindowRect.height - 30), optionScrollPosition, new Rect(0, 0, optionWindowRect.width - 20, maxYPosScroll));
        float yPos = 0;
        GUI.Label(new Rect(25, 0, sWidthHeight.x, sWidthHeight.y), string.Format("Fps:{0} refresh Fps rate: {1}", lastFrameRate, refreshRateMS));
        
        if (GUI.Button(new Rect(sWidthHeight.x + 25, yPos, bWidthHeight.x, bWidthHeight.y), "Exit"))
        {
            activated = false;
        }
        yPos = yPos + sWidthHeight.y;
        
        //yPos = ResolutionGroup(0, yPos);
        yPos = RenderingGroup(0, yPos);
        yPos = ShadowGroup(0, yPos);
        yPos = OtherGroup(0, yPos);
        yPos = ExtraGroup(0, yPos);
        yPos = yPos + bWidthHeight.y;
        if (GUI.Button(new Rect(25, yPos, bWidthHeight.x, bWidthHeight.y), "Exit"))
        {
            activated = false;
        }

        if(maxYPosScroll == 0)
            maxYPosScroll = yPos + 100;

        GUI.EndScrollView();
        GUI.DragWindow();
    }
//    float ResolutionGroup(float xStart, float yStart)
//    {
//        float xPos = xStart + 25;
//        float yPos = yStart + 25;
//        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), EUtils.UnityColoredText("Resolutions", headingColor));
//        yPos = yPos + sWidthHeight.y;
//        fullScreen = GUI.Toggle(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), fullScreen, "Full screen");
//        yPos = yPos + sWidthHeight.y;
//        float maxHeight = yPos + (Mathf.Floor(Screen.resolutions.Length / 3) * (sWidthHeight.y));
//        Resolution[] resolutions = Screen.resolutions;
//        for (int i = 0; i < resolutions.Length; i++)
//        {
//            float newXPos = xPos + (i % 3) * (bWidthHeight.x);
//            float newYPos = yPos + (Mathf.Floor(i / 3) * (bWidthHeight.y));
//            if (GUI.Button(new Rect(newXPos, newYPos, bWidthHeight.x, bWidthHeight.y), resolutions[i].width + "x" + resolutions[i].height + " " + resolutions[i].refreshRate))
//            {
//                Screen.SetResolution(resolutions[i].width, resolutions[i].height, fullScreen, resolutions[i].refreshRate);
//                optionWindowRect.x = 0;
//                optionWindowRect.y = 0;
//            }
//        }
//        return maxHeight;
//    }
    float RenderingGroup(float xStart,float yStart)
    {
        float xPos = xStart + 25;
        float yPos = yStart + 25;
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), EUtils.UnityColoredText("Rendering", headingColor));
        yPos = yPos + sWidthHeight.y;
        //Anti Analysing
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Antialiasing: " + QualitySettings.antiAliasing);
        antiAliasing = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), antiAliasing, 0, maxAntiAliasing);
        if (antiAliasing != QualitySettings.antiAliasing)
        {
            antiAliasing = (int)Mathf.ClosestPowerOfTwo((int)antiAliasing);
            if (antiAliasing == 1)
                antiAliasing = 0;
            QualitySettings.antiAliasing = (int)Mathf.ClosestPowerOfTwo((int)antiAliasing);
            Save(saveTag + GetName (new { antiAliasing }),antiAliasing);
        }
        //End anti Analysing
        //Anisotropic Filtering
        yPos = yPos + sWidthHeight.y;
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Anisotropic Filtering: " + QualitySettings.anisotropicFiltering.ToString());

        yPos = yPos + sWidthHeight.y;
        if (GUI.Button(new Rect(xPos, yPos, bWidthHeight.x, bWidthHeight.y), AnisotropicFiltering.Disable.ToString()))
        {
            anisotropicFilt = AnisotropicFiltering.Disable;
            Save(saveTag + GetName (new { anisotropicFilt }),0);
        }
        if (GUI.Button(new Rect(xPos + bWidthHeight.x, yPos, bWidthHeight.x, bWidthHeight.y), AnisotropicFiltering.Enable.ToString()))
        {
            anisotropicFilt = AnisotropicFiltering.Enable;
            Save(saveTag + GetName (new { anisotropicFilt }),1);
        }
        if (GUI.Button(new Rect(xPos + (bWidthHeight.x * 2), yPos, bWidthHeight.x, bWidthHeight.y), AnisotropicFiltering.ForceEnable.ToString()))
        {
            anisotropicFilt = AnisotropicFiltering.ForceEnable;
            Save(saveTag + GetName (new { anisotropicFilt }),2);
        }
        QualitySettings.anisotropicFiltering = anisotropicFilt;
        //End Anisotropic Filtering

        //Texture Quality
        yPos = yPos + bWidthHeight.y;
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Texture Quality(Beter to worse): " + QualitySettings.masterTextureLimit);
        textureQuality = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, 25), textureQuality, 0, maxTextureQuality);
        QualitySettings.masterTextureLimit = (int)textureQuality;
        Save(saveTag + GetName (new { textureQuality }),textureQuality);
        //End Texture Quality

        //Pixel Light Count
        yPos = yPos + sWidthHeight.y;
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Pixel Light Count: " + QualitySettings.pixelLightCount);
        pixelLightCount = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), pixelLightCount, 0, maxPixelLightCount);
        QualitySettings.pixelLightCount = (int)pixelLightCount;
        Save(saveTag + GetName (new { pixelLightCount }),pixelLightCount);
        //End Pixel Light Count
        return yPos;
    }
    float ShadowGroup(float xStart, float yStart)
    {
        float xPos = xStart + 25;
        float yPos = yStart + 25;

        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y),  EUtils.UnityColoredText("Shadow",headingColor));
        yPos = yPos + sWidthHeight.y;
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Shadow Projection: " + QualitySettings.shadowProjection.ToString());
        yPos = yPos + sWidthHeight.y;
        if (GUI.Button(new Rect(xPos, yPos, bWidthHeight.x, bWidthHeight.y), ShadowProjection.CloseFit.ToString()))
        {
            shadowProjection = ShadowProjection.CloseFit;
            Save(saveTag + GetName (new { shadowProjection }),0);
        }
        if (GUI.Button(new Rect(xPos + bWidthHeight.x, yPos, bWidthHeight.x, bWidthHeight.y), ShadowProjection.StableFit.ToString()))
        {
            shadowProjection = ShadowProjection.StableFit;
            Save(saveTag + GetName (new { shadowProjection }),1);
        }
        QualitySettings.shadowProjection = shadowProjection;
        yPos = yPos + bWidthHeight.y;

        var cameraController = GameObject.FindObjectsOfType<CameraController>().First();
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Max Shadow Distance: " + cameraController.m_maxShadowDistance);
        maxShadowDistance = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), maxShadowDistance, 0, shadowDistanceLimit);
        cameraController.m_maxShadowDistance = maxShadowDistance;
        Save(saveTag + GetName (new { shadowDistance = maxShadowDistance }),maxShadowDistance);
        yPos = yPos + sWidthHeight.y;
        
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Shadow Cascade: " + QualitySettings.shadowCascades);
        shadowCascade = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), shadowCascade, 0, maxShadowCascade);
        shadowCascade = (int)Mathf.ClosestPowerOfTwo((int)shadowCascade);
        Save(saveTag + GetName (new { shadowCascade }),shadowCascade);
        if (shadowCascade == 1)
            shadowCascade = 0;
        QualitySettings.shadowCascades = (int)Mathf.ClosestPowerOfTwo((int)shadowCascade);

        return yPos;
    }
    float OtherGroup(float xStart, float yStart)
    {
        float xPos = xStart + 25;
        float yPos = yStart + 25;

        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y),  EUtils.UnityColoredText("Other",headingColor));
        yPos = yPos + sWidthHeight.y;
        //vSync
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "vSync: " + QualitySettings.vSyncCount);
        vSync = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), vSync, 0, maxvSync);
        QualitySettings.vSyncCount = (int)vSync;
        Save(saveTag + GetName (new { vSync }),vSync);
        yPos = yPos + sWidthHeight.y;
        //End vSync
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x + 100, sWidthHeight.y), "Limit frame rate only works when vSync is disabled(0)");
        yPos = yPos + sWidthHeight.y;
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Frame rate (0 = unlimited): " + Application.targetFrameRate);
        frameRate = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), frameRate, 0, maxFrameRate);
        Application.targetFrameRate = (int)frameRate;
        Save(saveTag + GetName (new { frameRate }),frameRate);
        yPos = yPos + sWidthHeight.y;
        //LoD Stuff
        LoDLevel = QualitySettings.maximumLODLevel;
        LoDBias = QualitySettings.lodBias;
        
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Maximum LoD Level: " + QualitySettings.maximumLODLevel);
        LoDLevel = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), LoDLevel, 0, LoDLevelMax);
        QualitySettings.maximumLODLevel = (int)LoDLevel;
        Save(saveTag + GetName (new { LoDLevel }),LoDLevel);
        yPos = yPos + sWidthHeight.y;
        
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "LoD Bias: " + QualitySettings.lodBias);
        LoDBias = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), LoDBias, 0, LoDBiasMax);
        QualitySettings.lodBias = (int)LoDBias;
        Save(saveTag + GetName (new { LoDBias }),LoDBias);
        yPos = yPos + sWidthHeight.y;
        
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Particle Raycast Budget: " + QualitySettings.particleRaycastBudget);
        particleRaycastBudget = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), particleRaycastBudget, 0, maxParticleRaycastBudget);
        particleRaycastBudget = (int)Mathf.ClosestPowerOfTwo((int)particleRaycastBudget);
        if (particleRaycastBudget < 4)
            particleRaycastBudget = 4;
        Save(saveTag + GetName (new { particleRaycastBudget }),particleRaycastBudget);
        QualitySettings.particleRaycastBudget = (int)Mathf.ClosestPowerOfTwo((int)particleRaycastBudget);
        return yPos;
    }
    float ExtraGroup(float xStart, float yStart)
    {
        float xPos = xStart + 25;
        float yPos = yStart + 25;
        GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), EUtils.UnityColoredText("Extra Options", headingColor));
        yPos = yPos + sWidthHeight.y;
        
        if(GetCameraBehaviour("FogEffect") != null)
        {
            GetCameraBehaviour("FogEffect").enabled = GUI.Toggle(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), GetCameraBehaviour("FogEffect").enabled, "Fog Effect GameObject");
            yPos = yPos + sWidthHeight.y;
            /*
            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "3D Fog Amount: " + EUtils.GetFieldValue(GetCameraBehaviour("FogEffect"),"m_3DFogAmount"));
            m_3DFogAmount = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), m_3DFogAmount, 0, m_3DFogAmountMax);
            EUtils.SetFieldValue(GetCameraBehaviour("FogEffect"),"m_3DFogAmount",m_3DFogAmount);
            yPos = yPos + sWidthHeight.y;
            
            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "3D Fog Start: " + EUtils.GetFieldValue(GetCameraBehaviour("FogEffect"),"m_3DFogStart"));
            m_3DFogStart = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), m_3DFogStart, 0, m_3DFogStartMax);
            EUtils.SetFieldValue(GetCameraBehaviour("FogEffect"),"m_3DFogStart",m_3DFogStart);
            yPos = yPos + sWidthHeight.y;
            
            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "3D Fog Distance: " + EUtils.GetFieldValue(GetCameraBehaviour("FogEffect"),"m_3DFogDistance"));
            m_3DFogDistance = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), m_3DFogDistance, 0, m_3DFogDistanceMax);
            EUtils.SetFieldValue(GetCameraBehaviour("FogEffect"),"m_3DFogDistance",m_3DFogDistance);
            yPos = yPos + sWidthHeight.y;
            
            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "3D Noise Step Size: " + EUtils.GetFieldValue(GetCameraBehaviour("FogEffect"),"m_3DNoiseStepSize"));
            m_3DNoiseStepSize = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), m_3DNoiseStepSize, 0, m_3DNoiseStepSizeMax);
            EUtils.SetFieldValue(GetCameraBehaviour("FogEffect"),"m_3DNoiseStepSize",m_3DNoiseStepSize);
            yPos = yPos + sWidthHeight.y;
            
            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "3D Noise Scale: " + EUtils.GetFieldValue(GetCameraBehaviour("FogEffect"),"m_3DNoiseScale"));
            m_3DNoiseScale = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), m_3DNoiseScale, 0, m_3DFogAmountMax);
            EUtils.SetFieldValue(GetCameraBehaviour("FogEffect"),"m_3DNoiseScale",m_3DNoiseScale);
            yPos = yPos + sWidthHeight.y;
            */
            
        }
        //renderProperties
        if(renderProperties != null)
        {
            //Fog
            m_useVolumeFog = GUI.Toggle(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), (bool) EUtils.GetFieldValue(renderProperties,"m_useVolumeFog"), "Use Volume Fog");
            EUtils.SetFieldValue(renderProperties,"m_useVolumeFog",m_useVolumeFog);			
            if(m_useVolumeFog)
                Save(saveTag + GetName (new { m_useVolumeFog }),1);
            else
                Save(saveTag + GetName (new { m_useVolumeFog }),0);
            yPos = yPos + sWidthHeight.y;
                
            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Fog Height: " + EUtils.GetFieldValue(renderProperties,"m_fogHeight"));
            m_fogHeight = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), m_fogHeight, 0, m_fogHeightMax);
            EUtils.SetFieldValue(renderProperties,"m_fogHeight",m_fogHeight);
            Save(saveTag + GetName (new { m_fogHeight }),m_fogHeight);
            yPos = yPos + sWidthHeight.y;
            
            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Edge Fog Distance: " + EUtils.GetFieldValue(renderProperties,"m_edgeFogDistance"));
            m_edgeFogDistance = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), m_edgeFogDistance, 0, m_edgeFogDistanceMax);
            EUtils.SetFieldValue(renderProperties,"m_edgeFogDistance",m_edgeFogDistance);
            Save(saveTag + GetName (new { m_volumeFogStart }),m_edgeFogDistance);
            yPos = yPos + sWidthHeight.y;

            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Volume Fog Density: " + EUtils.GetFieldValue(renderProperties,"m_volumeFogDensity"));
            m_volumeFogDensity = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), m_volumeFogDensity, 0, m_volumeFogDensityMax);
            EUtils.SetFieldValue(renderProperties,"m_volumeFogDensity",m_volumeFogDensity);
            Save(saveTag + GetName (new { m_volumeFogStart }),m_volumeFogDensity);
            yPos = yPos + sWidthHeight.y;
            
            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Volume Fog Start: " + EUtils.GetFieldValue(renderProperties,"m_volumeFogStart"));
            m_volumeFogStart = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), m_volumeFogStart, 0, m_volumeFogStartMax);
            EUtils.SetFieldValue(renderProperties,"m_volumeFogStart",m_volumeFogStart);
            Save(saveTag + GetName (new { m_volumeFogStart }),m_volumeFogStart);
            yPos = yPos + sWidthHeight.y;
            
            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Volume Fog Distance: " + EUtils.GetFieldValue(renderProperties,"m_volumeFogDistance"));
            m_volumeFogDistance = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), m_volumeFogDistance, 0, m_volumeFogDistanceMax);
            EUtils.SetFieldValue(renderProperties,"m_volumeFogDistance",m_volumeFogDistance);
            Save(saveTag + GetName (new { m_volumeFogDistance }),m_volumeFogDistance);
            yPos = yPos + sWidthHeight.y;
            
            
            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Pollution Fog Intensity: " + EUtils.GetFieldValue(renderProperties,"m_pollutionFogIntensity"));
            m_pollutionFogIntensity = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), m_pollutionFogIntensity, 0, m_pollutionFogIntensityMax);
            EUtils.SetFieldValue(renderProperties,"m_pollutionFogIntensity",m_pollutionFogIntensity);
            Save(saveTag + GetName (new { m_pollutionFogIntensity }),m_pollutionFogIntensity);
            yPos = yPos + sWidthHeight.y;
            
            if (GUI.Button(new Rect(xPos, yPos, bWidthHeight.x *2, bWidthHeight.y), "Reset Fog Settings"))
            {
                ResetToDefault();
            }
            yPos = yPos + sWidthHeight.y;
        }
        
        if(GetCameraBehaviour("Bloom") != null)
        {		
            GetCameraBehaviour("Bloom").enabled = GUI.Toggle(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), GetCameraBehaviour("Bloom").enabled, "Bloom");
            yPos = yPos + sWidthHeight.y;
        }
        if(GetCameraBehaviour("ToneMapping") != null)
        {		
            GetCameraBehaviour("ToneMapping").enabled = GUI.Toggle(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), GetCameraBehaviour("ToneMapping").enabled, "Tone Mapping");
            yPos = yPos + sWidthHeight.y;
        }
        if(GetCameraBehaviour("ColorCorrectionLut") != null)
        {		
            GetCameraBehaviour("ColorCorrectionLut").enabled = GUI.Toggle(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), GetCameraBehaviour("ColorCorrectionLut").enabled, "Color Correction Lut");
            yPos = yPos + sWidthHeight.y;
        }
        if(GetCameraBehaviour("OverLayEffect") != null)
        {
            GetCameraBehaviour("OverLayEffect").enabled = GUI.Toggle(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), GetCameraBehaviour("OverLayEffect").enabled, "Over Lay Effect");
            yPos = yPos + sWidthHeight.y;
        }
        /*
        if(GetCameraBehaviour("SMAA") != null)
        {		
            GetCameraBehaviour("SMAA").enabled = GUI.Toggle(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), GetCameraBehaviour("SMAA").enabled, "SMAA");
            yPos = yPos + sWidthHeight.y;	
        }*/
        /*Added Effects
        sSAOC.enabled = GUI.Toggle(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y),sSAOC.enabled, "SSOAC");
        yPos = yPos + sWidthHeight.y;	
        sSAOB.enabled = GUI.Toggle(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y),sSAOB.enabled, "sSAOB");
        yPos = yPos + sWidthHeight.y;	
        edgeDetection.enabled = GUI.Toggle(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y),edgeDetection.enabled, "Edge Detection");
        yPos = yPos + sWidthHeight.y;	
        creaseShading.enabled = GUI.Toggle(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y),creaseShading.enabled, "Crease Shading");
        yPos = yPos + sWidthHeight.y;	*/
        /*
        //Kill Seagull sounds
        killSeagullSounds = GUI.Toggle(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), killSeagullSounds, "Kill Seagull Sounds");
        yPos = yPos + sWidthHeight.y;
            */
        //END
        //killSeagull = GUI.Toggle(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), killSeagull, "Kill Seagulls");
        //yPos = yPos + sWidthHeight.y;	
        //End
        return yPos;
    }
    public void Destroy()
    {
        frameUpdateTimer.Elapsed -= new ElapsedEventHandler(frameUpdateTimer_Elapsed);
        frameUpdateTimer.Dispose();
        frameUpdateTimer = null;
        
        QSaver qSaver = new QSaver();
        qSaver.QSaverSave(filePath, qData);
        
        if(this.gameObject != null);
            Destroy(this.gameObject);
    }
    private void OnApplicationQuit()
    {
        frameUpdateTimer.Elapsed -= new ElapsedEventHandler(frameUpdateTimer_Elapsed);
        frameUpdateTimer.Dispose();
        frameUpdateTimer = null;
        
        QSaver qSaver = new QSaver();
        qSaver.QSaverSave(filePath, qData);
    }
}
