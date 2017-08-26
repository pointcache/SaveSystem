namespace SaveSystem.Internal {

    using UnityEngine;

    public class SaveSystemSettings : ScriptableObject {

        static SaveSystemSettings _settings;
        public static SaveSystemSettings Current
        {
            get {
                if (_settings == null)
                    _settings = Resources.LoadAll<SaveSystemSettings>("")[0] as SaveSystemSettings;
                return _settings;
            }
        }


        [Header("SaveSystem")]
        public string EntitiesFolder = "Entities";
        public string DatabaseManifest = "manifest.db";
        public string CustomDataFolder = "CustomData";
    }

}