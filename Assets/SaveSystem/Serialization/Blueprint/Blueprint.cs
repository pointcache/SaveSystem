namespace SaveSystem.Serialization.Blueprints {

    using System;
    using global::SaveSystem.Serialization;

    [Serializable]
    public class Blueprint {

        public float GameVersion;
        public string Name;
        public SaveObject SaveObject;

    }
}