namespace QMoreOptions
{
    public class Configuration
    {
        public static Configuration instance = new Configuration();
        public static Configuration defaults = new Configuration();

        public ToneMapping toneMapping = new ToneMapping();
        public FogClassic fogClassic = new FogClassic();

        public bool isFogClassicDirty;
        public bool isToneMappingDirty;
        public bool IsDirty => isFogClassicDirty || isToneMappingDirty;


        public struct ToneMapping
        {
            public bool enabled;
        }

        public struct FogEffect
        {
            public bool enabled;
        }

        public struct FogClassic
        {
            public float fogHeight;
            public float edgeFogDistance;
            public float volumeFogDensity;
            public float volumeFogStart;
            public float volumeFogDistance;
            public float pollutionFogIntensity;
            public bool useVolumeFog;
        }

        public static void Save()
        {
            //TODO(earalov): save to XML
        }

        public static void Load()
        {
            //TODO(earalov): load from XML
        }
    }
}