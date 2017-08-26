namespace SaveSystem.Serialization.Blueprints {
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif
    using global::SaveSystem.Serialization;
    using global::SaveSystem.Internal;
    using global::SaveSystem.Utility;

    public class BlueprintLoader : MonoBehaviour {

        [Tooltip("Used to work with blueprints in editor")]
        public TextAsset Blueprint;
        public bool LoadOnEnable;
        public bool SelfDestructAfterLoad;
        [Tooltip("Unboxed blueprint entities will be dumped in the scene")]
        public bool DontParent;

#if UNITY_EDITOR
        [MenuItem("Assets/SaveSystem/NewBlueprint")]
        public static void New() {
            GameObject root = new GameObject("Blueprint");
            Selection.activeGameObject = root;
            root.AddComponent<BlueprintLoader>();
            SceneView.lastActiveSceneView.FrameSelected();
        }
#endif
        public Blueprint Save() {
            if (transform.childCount == 0)
                return null;
            Blueprint bp = new Blueprint();
            bp.SaveObject = SaveSystem.CreateBlueprintFromTransform(transform);
            bp.Name = gameObject.name;
            return bp;
        }

        public void Load() {
            if (Blueprint == null)
                return;
            else {
                transform.DestroyChildren();
                SaveSystem.LoadBlueprint(Blueprint.text, DontParent ? null : transform);
            }
        }

        public void OnEnable() {
            if (transform.childCount > 0) {
                transform.DestroyChildren();
            }
            if(LoadOnEnable)
            Load();

            if (SelfDestructAfterLoad) {
                Destroy(this);
            }
        }

    }

}