namespace SaveSystem.Utility {
#if UNITY_EDITOR
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using System.Collections.Generic;
    using SaveSystem.Internal;
    using SaveSystem.Database;


    public static class SaveSystemEditorTools {

        [MenuItem("SaveSystem/OpenPersistentData", priority = 1)]
        public static void OpenPersistentData() {
            OpenInFileBrowser.Open(UnityEngine.Application.persistentDataPath);
        }

        [MenuItem("SaveSystem/UpdateEntities", priority = 1)]
        public static void UpdateEverything() {
            var settings = SaveSystemSettings.Current;

            SaveEntityDatabase.RebuildWithoutReloadOfTheScene();

            //if (settings.SaveAndReloadScene) {
            //
            //    EditorSceneManager.MarkAllScenesDirty();
            //    EditorSceneManager.SaveOpenScenes();
            //    var scene = EditorSceneManager.GetActiveScene();
            //    EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Single);
            //}
        }


        [MenuItem("Tools/Select Missing Scripts")]
        static void SelectMissing(MenuCommand command) {
            Transform[] ts = GameObject.FindObjectsOfType<Transform>();
            List<GameObject> selection = new List<GameObject>();
            foreach (Transform t in ts) {
                Component[] cs = t.gameObject.GetComponents<Component>();
                foreach (Component c in cs) {
                    if (c == null) {
                        selection.Add(t.gameObject);
                    }
                }
            }
            Selection.objects = selection.ToArray();
        }
    }
#endif 
}