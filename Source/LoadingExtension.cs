using ICities;
using UnityEngine;

namespace QMoreOptions
{
    public class LoadingExtension : LoadingExtensionBase
    {
        private const string GameObjectName = "MoreOptionsOptionsWindow";

        public override void OnLevelLoaded(LoadMode mode)
        {
            var optionsWindowGo = new GameObject(GameObjectName);
            optionsWindowGo.AddComponent<OptionsWindow>();
        }


        public override void OnLevelUnloading()
        {
            var optionsWindowGo = GameObject.Find(GameObjectName);
            if (optionsWindowGo == null)
            {
                return;
            }
            Object.Destroy(optionsWindowGo);
        }
    }
}