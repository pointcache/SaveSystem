namespace SaveSystem.Database {
    using UnityEngine;
    using System.Collections.Generic;
    using System.IO;
#if UNITY_EDITOR
    using UnityEditor;
    using UnityEditor.SceneManagement;
#endif
    using SaveSystem.Internal;
    using SaveSystem.Utility;

    public class SaveEntityDatabase : MonoBehaviour {


        #region SINGLETON
        private static SaveEntityDatabase _instance;
        public static SaveEntityDatabase instance
        {
            get {
                if (!_instance)
                    _instance = GameObject.FindObjectOfType<SaveEntityDatabase>();
                return _instance;
            }
        }
        #endregion

        static List<GameObject> prefabObjects = new List<GameObject>(1000);

        static Dictionary<int, SaveEntity> entities = new Dictionary<int, SaveEntity>(1000);
        static Dictionary<int, HashSet<int>> Components = new Dictionary<int, HashSet<int>>(10000);

        static DatabaseManifest _manifest;
        static DatabaseManifest manifest
        {
            get {
                if (_manifest == null) {
                    TextAsset manifest_asset = Resources.Load("SaveSystem/" + SaveSystemSettings.Current.DatabaseManifest) as TextAsset;
                    _manifest = SerializationHelper.LoadFromString<DatabaseManifest>(manifest_asset.text);
                }
                return _manifest;
            }
        }


#if UNITY_EDITOR
        [MenuItem("SaveSystem/RebuildIDS")]
        public static void Rebuild() {
            prefabObjects = new List<GameObject>(1000);
            entities = new Dictionary<int, SaveEntity>(1000);
            Components = new Dictionary<int, HashSet<int>>(10000);

            assign_ids_entities();
            assign_ids_components();

            var scene = EditorSceneManager.GetActiveScene();
            EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Single);
        }

        public static void RebuildWithoutReloadOfTheScene() {
            prefabObjects = new List<GameObject>(1000);
            entities = new Dictionary<int, SaveEntity>(1000);
            Components = new Dictionary<int, HashSet<int>>(10000);
            assign_ids_entities();
            assign_ids_components();

        }
#endif

        /// <summary>
        /// Very careful with this one
        /// </summary>
#if UNITY_EDITOR
        [MenuItem("SaveSystem/Danger/Clear all ids(commit before you do it)")]
        public static void clear_all_entity_IDs() {

            var prefabs = Resources.LoadAll(SaveSystemSettings.Current.EntitiesFolder + "/");


            foreach (var p in prefabs) {
                var ent = ((GameObject)p).GetComponent<SaveEntity>();
                Undo.RegisterCompleteObjectUndo(ent, "Clearing Entity Ids");
                if (ent) {
                    ent.entityID = 0;
                }
                EditorUtility.SetDirty(p);
            }

        }
#endif

        static string dbPath
        {
            get {
                return "/Resources/" + SaveSystemSettings.Current.EntitiesFolder + "/";
            }
        }

        public static GameObject GetPrefabByPath(string path) {
            return GetPrefab(GetPrefabID(path));
        }

        public static GameObject GetPrefab(int id) {
            string path = "";
            string idPath = "";
            manifest.entity_id_adress.TryGetValue(id, out idPath);
            if (idPath != "")
                path = SaveSystemSettings.Current.EntitiesFolder + "/" + idPath;
            else
                return null;
            return Resources.Load(path) as GameObject;
        }
#if UNITY_EDITOR

        static void assign_ids_entities() {

            HashSet<int> ids = new HashSet<int>();

            DatabaseManifest manifest = new DatabaseManifest();
            manifest.GameVersion = ProjectInfo.Current.Version;
            string resfolder = "Resources/" + SaveSystemSettings.Current.EntitiesFolder + "/";


            var prefabs = Resources.LoadAll("", typeof(GameObject));

            foreach (var p in prefabs) {

                var ent = ((GameObject)p).GetComponent<SaveEntity>();

                if (!ent)
                    continue;

                if (ent) {

                    Undo.RegisterCompleteObjectUndo(ent, "Assigning new ids");

                    ids.Add(ent.entityID);

                }

                EditorUtility.SetDirty(p);

            }

            var files = Directory.GetFiles(Application.dataPath, "*.prefab", SearchOption.AllDirectories);


            foreach (var f in files) {

                string pathfixed = f.Replace("\\", "/");

                if (!pathfixed.Contains(dbPath))
                    continue;

                int index = pathfixed.IndexOf("/Assets/");
                string adress = pathfixed.Remove(0, index + 1);

                //adress = adress;

                var prefab = AssetDatabase.LoadAssetAtPath(adress, typeof(GameObject)) as GameObject;

                // Resources.Load(URSASettings.Current.DatabaseRootFolder + "/" + adress) as GameObject;

                var entity = prefab.GetComponent<SaveEntity>();

                if (!entity) {
                    Debug.LogError("Database: GameObject without entity found, skipping.", prefab);
                    continue;
                }

                if (entity.entityID == 0) {
                    entity.entityID = GameObjectUtils.GetUniqueID(ids);
                }

                if (entity.instanceID != 0) {
                    entity.instanceID = 0;
                }

                if (entity.blueprintID != 0) {
                    entity.blueprintID = 0;
                }

                int resIndex = adress.IndexOf(resfolder);
                string entityResourcePath = adress.Replace(".prefab", string.Empty).Remove(0, resIndex + resfolder.Length);

                if (manifest.entity_adress_id.ContainsKey(entityResourcePath)) {
                    Debug.LogError("Sadly duplicate Entity names are not allowed as they will no be uniquely identifiable on runtime.", entity);
                    continue;
                }

                if (manifest.entity_id_adress.ContainsKey(entity.entityID)) {
                    Debug.LogError("Trying to add entity with the same id: " + entity.entityID, entity.gameObject);
                }
                else {
                    manifest.entity_id_adress.Add(entity.entityID, entityResourcePath);
                    manifest.entity_adress_id.Add(entityResourcePath, entity.entityID);
                }
            }

            SerializationHelper.Serialize(manifest, Application.dataPath + "/Resources/SaveSystem/" + SaveSystemSettings.Current.DatabaseManifest + ".json", true);
        }

        static void assign_ids_components() {

            var allData = Resources.LoadAll(SaveSystemSettings.Current.EntitiesFolder);
            prefabObjects = new List<GameObject>(1000);
            foreach (var item in allData) {
                GameObject go = item as GameObject;
                var entity = go.GetComponent<SaveEntity>();
                if (!entity) {
                    Debug.LogError("Database: GameObject without entity found, skipping.", go);
                    continue;
                }

                prefabObjects.Add(go);
                entities.Add(entity.entityID, entity);

                SavedComponent[] components = go.GetComponentsInChildren<SavedComponent>();
                HashSet<int> comps_in_entity = null;
                Components.TryGetValue(entity.entityID, out comps_in_entity);

                if (comps_in_entity == null)
                    comps_in_entity = new HashSet<int>();

                foreach (var c in components) {
                    if (c.componentID == 0) {
                        c.componentID = GameObjectUtils.GetUniqueID(comps_in_entity);
                    }
                    comps_in_entity.Add(c.componentID);
                }
            }
        }
#endif

        internal static string GetPrefabPath(int databaseID) {
            string result = "";
            manifest.entity_id_adress.TryGetValue(databaseID, out result);
            if (result == "") {
                Debug.LogError("Entity by id: " + databaseID + " not found in manifest.");
                Debug.LogError("Dont forget to update the database.");

            }
            return result;
        }
        internal static int GetPrefabID(string path) {
            int result = 0;
            manifest.entity_adress_id.TryGetValue(path, out result);
            if (result == 0) {
                Debug.LogError("Entity by path: " + path + " not found in manifest.");
                Debug.LogError("Dont forget to update the database.");

            }
            return result;
        }
    }

}