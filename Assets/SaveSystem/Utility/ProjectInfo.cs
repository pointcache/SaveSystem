namespace SaveSystem.Utility {
    using UnityEngine;
    using UnityEngine.UI;
    using System;
    using System.Collections.Generic;

    public class ProjectInfo : ScriptableObject {

        public static ProjectInfo _current;
        public static ProjectInfo Current
        {
            get {
                if (_current == null)
                    _current = Resources.LoadAll<ProjectInfo>("")[0];
                return _current;
            }
        }

        public int Version = 0;
    }

}