using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using QMoreOptions.Constants;
using QMoreOptions.QugetFileSystem;
using UnityEngine;

/* 
 * Author: Eduard Meivogel
 * Website: https://www.facebook.com/EddyMeivogelProjects
 * Creation Year: 2015
 * If you are improving or use this code please leave my name in here and add your own :) 
 * ToDo clean up and make more classes.... I should have thought this trough first xD
 */
namespace QMoreOptions
{
    public class OptionsWindow : MonoBehaviour
    {
        //public bool activated = false;
        public Vector2 sWidthHeight = new Vector2(250, 25);
        public Vector2 bWidthHeight = new Vector2(100, 25);
        public Rect optionWindowRect = new Rect(0, 0, 550, 550);
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

        private RenderProperties renderProperties;
        private DayNightCloudsProperties dayNightCloudsProperties;
        private FogProperties fogProperties;
        private DayNightProperties dayNightProperties;

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
        void Start()
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
            foreach (var t in FindObjectsOfType<MonoBehaviour>())
            {
                var properties = t as RenderProperties;
                if (properties != null)
                {
                    this.renderProperties = properties;
                }
                var properties1 = t as DayNightProperties;
                if (properties1 != null)
                {
                    this.dayNightProperties = properties1;
                }
                var properties2 = t as DayNightCloudsProperties;
                if (properties2 != null)
                {
                    this.dayNightCloudsProperties = properties2;
                }
                var properties3 = t as FogProperties;
                if (properties3 != null)
                {
                    this.fogProperties = properties3;
                }
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
            if (qData == null)
                qData = new QData();

            if (qData.GetValueByKey("DONT_REMOVE_THIS") == null)
            {
                //OVERRIDE
                qData.AddToValues("DONT_REMOVE_THIS", "ELSE_IT_RESETS!");
                ResetToDefault();
            }
            else
            {

                //Default
                antiAliasing = Load(saveTag + GetName(new { antiAliasing }));
                //anisotropicFilt = Load(saveTag + GetName (new { anisotropicFilt }));
                float anisoFloat = Load(saveTag + GetName(new { anisotropicFilt }));
                if (anisoFloat == 0)
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

                textureQuality = Load(saveTag + GetName(new { textureQuality }));
                pixelLightCount = Load(saveTag + GetName(new { pixelLightCount }));
                //Shadow
                float shadowProjFloat = Load(saveTag + GetName(new { shadowProjection }));
                if (shadowProjFloat == 0)
                {
                    shadowProjection = ShadowProjection.CloseFit;
                }
                if (shadowProjFloat == 1)
                {
                    shadowProjection = ShadowProjection.StableFit;
                }
                maxShadowDistance = Load(saveTag + GetName(new { shadowDistance = maxShadowDistance }));
                shadowCascade = Load(saveTag + GetName(new { shadowCascade }));
                //Other
                vSync = Load(saveTag + GetName(new { vSync }));
                particleRaycastBudget = Load(saveTag + GetName(new { particleRaycastBudget }));
                frameRate = Load(saveTag + GetName(new { frameRate }));

                LoDLevel = Load(saveTag + GetName(new { LoDLevel }));
                LoDBias = Load(saveTag + GetName(new { LoDBias }));

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
                BackupFogClassicOptions();
                Configuration.instance.fogClassic = Configuration.defaults.fogClassic;
                SetFogClassicOptions();
            }
        }

        private void BackupFogClassicOptions()
        {
            Configuration.defaults.fogClassic.fogHeight = renderProperties.m_fogHeight;
            Configuration.defaults.fogClassic.edgeFogDistance = renderProperties.m_edgeFogDistance;
            Configuration.defaults.fogClassic.useVolumeFog = renderProperties.m_useVolumeFog;
            Configuration.defaults.fogClassic.volumeFogDensity = renderProperties.m_volumeFogDensity;
            Configuration.defaults.fogClassic.volumeFogStart = renderProperties.m_volumeFogStart;
            Configuration.defaults.fogClassic.volumeFogDistance = renderProperties.m_volumeFogDistance;
            Configuration.defaults.fogClassic.pollutionFogIntensity = renderProperties.m_pollutionFogIntensity;
        }

        private void SetFogClassicOptions()
        {
            renderProperties.m_fogHeight = Configuration.instance.fogClassic.fogHeight;
            renderProperties.m_edgeFogDistance = Configuration.instance.fogClassic.edgeFogDistance;
            renderProperties.m_useVolumeFog = Configuration.instance.fogClassic.useVolumeFog;
            renderProperties.m_volumeFogDensity = Configuration.instance.fogClassic.volumeFogDensity;
            renderProperties.m_volumeFogStart = Configuration.instance.fogClassic.volumeFogStart;
            renderProperties.m_volumeFogDistance = Configuration.instance.fogClassic.volumeFogDistance;
            renderProperties.m_pollutionFogIntensity = Configuration.instance.fogClassic.pollutionFogIntensity;
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
            Configuration.instance.fogClassic = Configuration.defaults.fogClassic;
            Configuration.Save();
            SetFogClassicOptions();

            qData.AddToValues("DONT_REMOVE_THIS", "ELSE_IT_RESETS!");


            //Save
            Save(saveTag + GetName(new { antiAliasing }), antiAliasing);

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
            Save(saveTag + GetName(new { anisotropicFilt }), anisoFloat);

            Save(saveTag + GetName(new { textureQuality }), textureQuality);
            Save(saveTag + GetName(new { pixelLightCount }), pixelLightCount);

            float shadowProjFloat = -1;
            if (shadowProjection == ShadowProjection.CloseFit)
            {
                shadowProjFloat = 0;
            }
            if (shadowProjection == ShadowProjection.StableFit)
            {
                shadowProjFloat = 1;
            }
            Save(saveTag + GetName(new { shadowProjection }), shadowProjFloat);

            Save(saveTag + GetName(new { shadowDistance = maxShadowDistance }), maxShadowDistance);
            Save(saveTag + GetName(new { shadowCascade }), shadowCascade);
            Save(saveTag + GetName(new { vSync }), vSync);
            Save(saveTag + GetName(new { particleRaycastBudget }), particleRaycastBudget);
            Save(saveTag + GetName(new { frameRate }), frameRate);
            Save(saveTag + GetName(new { LoDLevel }), LoDLevel);
            Save(saveTag + GetName(new { LoDBias }), LoDBias);
        }
        private void Save(string key, float value)
        {
            if (qData != null)
                qData.AddToValues(key, value);
            //PlayerPrefs.SetFloat(key, value);
        }
        private float Load(string key)
        {
            var value = qData.GetValueByKey(key);
            if (!(value is float))
                return -1;
            else
                return (float)value;
        }

        MonoBehaviour GetCameraBehaviour(string name)
        {
            for (int i = 0; i < cameraBehaviours.Length; i++)
            {
                if (cameraBehaviours[i].GetType().Name == name)
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
        void Update()
        {
            if (setFrameRate)
            {
                lastFrameRate = (int)(1.0f / Time.deltaTime);
                setFrameRate = false;
            }
            if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.O)/*Input.GetKeyDown(KeyCode.Escape)*/)
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
                activated = activated ? false : true;
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

            if (maxYPosScroll == 0)
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
        float RenderingGroup(float xStart, float yStart)
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
                Save(saveTag + GetName(new { antiAliasing }), antiAliasing);
            }
            //End anti Analysing
            //Anisotropic Filtering
            yPos = yPos + sWidthHeight.y;
            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Anisotropic Filtering: " + QualitySettings.anisotropicFiltering.ToString());

            yPos = yPos + sWidthHeight.y;
            if (GUI.Button(new Rect(xPos, yPos, bWidthHeight.x, bWidthHeight.y), AnisotropicFiltering.Disable.ToString()))
            {
                anisotropicFilt = AnisotropicFiltering.Disable;
                Save(saveTag + GetName(new { anisotropicFilt }), 0);
            }
            if (GUI.Button(new Rect(xPos + bWidthHeight.x, yPos, bWidthHeight.x, bWidthHeight.y), AnisotropicFiltering.Enable.ToString()))
            {
                anisotropicFilt = AnisotropicFiltering.Enable;
                Save(saveTag + GetName(new { anisotropicFilt }), 1);
            }
            if (GUI.Button(new Rect(xPos + (bWidthHeight.x * 2), yPos, bWidthHeight.x, bWidthHeight.y), AnisotropicFiltering.ForceEnable.ToString()))
            {
                anisotropicFilt = AnisotropicFiltering.ForceEnable;
                Save(saveTag + GetName(new { anisotropicFilt }), 2);
            }
            QualitySettings.anisotropicFiltering = anisotropicFilt;
            //End Anisotropic Filtering

            //Texture Quality
            yPos = yPos + bWidthHeight.y;
            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Texture Quality(Beter to worse): " + QualitySettings.masterTextureLimit);
            textureQuality = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, 25), textureQuality, 0, maxTextureQuality);
            QualitySettings.masterTextureLimit = (int)textureQuality;
            Save(saveTag + GetName(new { textureQuality }), textureQuality);
            //End Texture Quality

            //Pixel Light Count
            yPos = yPos + sWidthHeight.y;
            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Pixel Light Count: " + QualitySettings.pixelLightCount);
            pixelLightCount = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), pixelLightCount, 0, maxPixelLightCount);
            QualitySettings.pixelLightCount = (int)pixelLightCount;
            Save(saveTag + GetName(new { pixelLightCount }), pixelLightCount);
            //End Pixel Light Count
            return yPos;
        }
        float ShadowGroup(float xStart, float yStart)
        {
            float xPos = xStart + 25;
            float yPos = yStart + 25;

            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), EUtils.UnityColoredText("Shadow", headingColor));
            yPos = yPos + sWidthHeight.y;
            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Shadow Projection: " + QualitySettings.shadowProjection.ToString());
            yPos = yPos + sWidthHeight.y;
            if (GUI.Button(new Rect(xPos, yPos, bWidthHeight.x, bWidthHeight.y), ShadowProjection.CloseFit.ToString()))
            {
                shadowProjection = ShadowProjection.CloseFit;
                Save(saveTag + GetName(new { shadowProjection }), 0);
            }
            if (GUI.Button(new Rect(xPos + bWidthHeight.x, yPos, bWidthHeight.x, bWidthHeight.y), ShadowProjection.StableFit.ToString()))
            {
                shadowProjection = ShadowProjection.StableFit;
                Save(saveTag + GetName(new { shadowProjection }), 1);
            }
            QualitySettings.shadowProjection = shadowProjection;
            yPos = yPos + bWidthHeight.y;

            var cameraController = GameObject.FindObjectsOfType<CameraController>().First();
            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Max Shadow Distance: " + cameraController.m_maxShadowDistance);
            maxShadowDistance = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), maxShadowDistance, 0, shadowDistanceLimit);
            cameraController.m_maxShadowDistance = maxShadowDistance;
            Save(saveTag + GetName(new { shadowDistance = maxShadowDistance }), maxShadowDistance);
            yPos = yPos + sWidthHeight.y;

            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Shadow Cascade: " + QualitySettings.shadowCascades);
            shadowCascade = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), shadowCascade, 0, maxShadowCascade);
            shadowCascade = (int)Mathf.ClosestPowerOfTwo((int)shadowCascade);
            Save(saveTag + GetName(new { shadowCascade }), shadowCascade);
            if (shadowCascade == 1)
                shadowCascade = 0;
            QualitySettings.shadowCascades = (int)Mathf.ClosestPowerOfTwo((int)shadowCascade);

            return yPos;
        }
        float OtherGroup(float xStart, float yStart)
        {
            float xPos = xStart + 25;
            float yPos = yStart + 25;

            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), EUtils.UnityColoredText("Other", headingColor));
            yPos = yPos + sWidthHeight.y;
            //vSync
            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "vSync: " + QualitySettings.vSyncCount);
            vSync = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), vSync, 0, maxvSync);
            QualitySettings.vSyncCount = (int)vSync;
            Save(saveTag + GetName(new { vSync }), vSync);
            yPos = yPos + sWidthHeight.y;
            //End vSync
            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x + 100, sWidthHeight.y), "Limit frame rate only works when vSync is disabled(0)");
            yPos = yPos + sWidthHeight.y;
            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Frame rate (0 = unlimited): " + Application.targetFrameRate);
            frameRate = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), frameRate, 0, maxFrameRate);
            Application.targetFrameRate = (int)frameRate;
            Save(saveTag + GetName(new { frameRate }), frameRate);
            yPos = yPos + sWidthHeight.y;
            //LoD Stuff
            LoDLevel = QualitySettings.maximumLODLevel;
            LoDBias = QualitySettings.lodBias;

            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Maximum LoD Level: " + QualitySettings.maximumLODLevel);
            LoDLevel = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), LoDLevel, 0, LoDLevelMax);
            QualitySettings.maximumLODLevel = (int)LoDLevel;
            Save(saveTag + GetName(new { LoDLevel }), LoDLevel);
            yPos = yPos + sWidthHeight.y;

            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "LoD Bias: " + QualitySettings.lodBias);
            LoDBias = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), LoDBias, 0, LoDBiasMax);
            QualitySettings.lodBias = (int)LoDBias;
            Save(saveTag + GetName(new { LoDBias }), LoDBias);
            yPos = yPos + sWidthHeight.y;

            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), "Particle Raycast Budget: " + QualitySettings.particleRaycastBudget);
            particleRaycastBudget = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), particleRaycastBudget, 0, maxParticleRaycastBudget);
            particleRaycastBudget = (int)Mathf.ClosestPowerOfTwo((int)particleRaycastBudget);
            if (particleRaycastBudget < 4)
                particleRaycastBudget = 4;
            Save(saveTag + GetName(new { particleRaycastBudget }), particleRaycastBudget);
            QualitySettings.particleRaycastBudget = (int)Mathf.ClosestPowerOfTwo((int)particleRaycastBudget);
            return yPos;
        }
        float ExtraGroup(float xStart, float yStart)
        {
            float xPos = xStart + 25;
            float yPos = yStart + 25;
            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), EUtils.UnityColoredText("Extra Options", headingColor));
            yPos = yPos + sWidthHeight.y;

            if (GetCameraBehaviour("FogEffect") != null)
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

            if (GetCameraBehaviour("DayNightFogEffect") != null)
            {
                GetCameraBehaviour("DayNightFogEffect").enabled =
                    GUI.Toggle(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y),
                        GetCameraBehaviour("DayNightFogEffect").enabled, "Day Night Fog Effect GameObject");
                yPos = yPos + sWidthHeight.y;
            }

            if (renderProperties != null)
            {
                ToggleGroup("Use Volume Fog", xPos, ref yPos, renderProperties.m_useVolumeFog,
                    ref Configuration.instance.fogClassic.useVolumeFog, ref Configuration.instance.isFogClassicDirty);

                HorizontalSliderGroup("Fog Height", xPos, ref yPos, renderProperties.m_fogHeight,
                    FogClassicConstants.FogHeightMin, FogClassicConstants.FogHeightMax,
                    ref Configuration.instance.fogClassic.fogHeight, ref Configuration.instance.isFogClassicDirty);

                HorizontalSliderGroup("Edge Fog Distance", xPos, ref yPos, renderProperties.m_edgeFogDistance,
                    FogClassicConstants.EdgeFogDistanceMin, FogClassicConstants.EdgeFogDistanceMax,
                    ref Configuration.instance.fogClassic.edgeFogDistance, ref Configuration.instance.isFogClassicDirty);

                HorizontalSliderGroup("Volume Fog Density", xPos, ref yPos, renderProperties.m_volumeFogDensity,
                    FogClassicConstants.VolumeFogDensityMin, FogClassicConstants.VolumeFogDensityMax,
                    ref Configuration.instance.fogClassic.volumeFogDensity, ref Configuration.instance.isFogClassicDirty);

                HorizontalSliderGroup("Volume Fog Start", xPos, ref yPos, renderProperties.m_volumeFogStart,
                    FogClassicConstants.VolumeFogStartMin, FogClassicConstants.VolumeFogStartMax,
                    ref Configuration.instance.fogClassic.volumeFogStart, ref Configuration.instance.isFogClassicDirty);

                HorizontalSliderGroup("Volume Fog Distance", xPos, ref yPos, renderProperties.m_volumeFogDistance,
                    FogClassicConstants.VolumeFogDistanceMin, FogClassicConstants.VolumeFogDistanceMax,
                    ref Configuration.instance.fogClassic.volumeFogDistance, ref Configuration.instance.isFogClassicDirty);

                HorizontalSliderGroup("Pollution Fog Intensity", xPos, ref yPos, renderProperties.m_pollutionFogIntensity,
                    FogClassicConstants.PollutionFogIntensityMin, FogClassicConstants.PollutionFogIntensityMax,
                    ref Configuration.instance.fogClassic.pollutionFogIntensity, ref Configuration.instance.isFogClassicDirty);

                if (Configuration.instance.isFogClassicDirty)
                {
                    SetFogClassicOptions();
                }

                if (GUI.Button(new Rect(xPos, yPos, bWidthHeight.x * 2, bWidthHeight.y), "Reset Fog Settings"))
                {
                    ResetToDefault();
                }
                yPos = yPos + sWidthHeight.y;
            }

            if (GetCameraBehaviour("Bloom") != null)
            {
                GetCameraBehaviour("Bloom").enabled = GUI.Toggle(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), GetCameraBehaviour("Bloom").enabled, "Bloom");
                yPos = yPos + sWidthHeight.y;
            }
            if (GetCameraBehaviour("ToneMapping") != null)
            {
                GetCameraBehaviour("ToneMapping").enabled = GUI.Toggle(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), GetCameraBehaviour("ToneMapping").enabled, "Tone Mapping");
                yPos = yPos + sWidthHeight.y;
            }
            if (GetCameraBehaviour("ColorCorrectionLut") != null)
            {
                GetCameraBehaviour("ColorCorrectionLut").enabled = GUI.Toggle(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), GetCameraBehaviour("ColorCorrectionLut").enabled, "Color Correction Lut");
                yPos = yPos + sWidthHeight.y;
            }
            if (GetCameraBehaviour("OverLayEffect") != null)
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

        private void ToggleGroup(string labelText, float xPos, ref float yPos, bool readValue,
            ref bool configurationValue, ref bool configurationDirty)
        {
            var newValue = GUI.Toggle(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), readValue, labelText);
            if (newValue != configurationValue)
            {
                configurationValue = newValue;
                configurationDirty = true;
            }
            yPos = yPos + sWidthHeight.y;
        }


        private void HorizontalSliderGroup(string labeltext, float xPos, ref float yPos, float readValue, float min, float max,
            ref float configurationValue, ref bool configurationDirty)
        {
            GUI.Label(new Rect(xPos, yPos, sWidthHeight.x, sWidthHeight.y), $"{labeltext}: {readValue}");
            var newValue = GUI.HorizontalSlider(new Rect(xPos + sWidthHeight.x, yPos, sWidthHeight.x, sWidthHeight.y), readValue, min, max);
            if (Math.Abs(configurationValue - newValue) > 0.0f)
            {
                configurationValue = newValue;
                configurationDirty = true;
            }
            yPos = yPos + sWidthHeight.y;
        }

        public void Destroy()
        {
            frameUpdateTimer.Elapsed -= new ElapsedEventHandler(frameUpdateTimer_Elapsed);
            frameUpdateTimer.Dispose();
            frameUpdateTimer = null;

            QSaver qSaver = new QSaver();
            qSaver.QSaverSave(filePath, qData);

            if (this.gameObject != null) ;
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
}
