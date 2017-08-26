namespace SaveSystem.ECS.Entity {

    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using SaveSystem.Utility;
    using SaveSystem.Serialization;
    using SaveSystem;

    internal static class SaveEntityManager {

        internal static Dictionary<int, global::SaveSystem.SaveEntity> SceneEntities = new Dictionary<int, SaveEntity>(1000);
        internal static Dictionary<int, global::SaveSystem.SaveEntity> PersistentEntities = new Dictionary<int, SaveEntity>(1000);

        internal static event Action<SaveEntity> OnAdded;
        internal static event Action<SaveEntity> OnRemoved;

        private static HashSet<int> registeredEntitiesIDs = new HashSet<int>();
        
        public static void MarkPersistent(SaveEntity e) {
            SceneEntities.Remove(e.entityID);
            PersistentEntities.Add(e.entityID, e);
        }

        internal static void RegisterEntity(SaveEntity e) {
            var persistent_root = e.transform.GetComponentInParent<PersistentDataSystem>();
            if (persistent_root.Null()) {
                if (SceneEntities.ContainsKey(e.ID)) {
                    Debug.Log("Entity with this ID already exists, i will assume you duplicated it in the editor, so ill assign a new instance ID for you.");
                    e.instanceID = 0;
                }
                SceneEntities.Add(e.ID, e);

            }
            else {
                if (PersistentEntities.ContainsKey(e.ID)) {
                    Debug.Log("Entity with this ID already exists, i will assume you duplicated it in the editor, so ill assign a new instance ID for you.");
                    e.instanceID = 0;
                }
                PersistentEntities.Add(e.ID, e);
            }

            if (OnAdded != null)
                OnAdded(e);
        }

        internal static void UnRegisterEntity(SaveEntity e) {
            SceneEntities.Remove(e.ID);
            PersistentEntities.Remove(e.ID);
            if (OnRemoved != null)
                OnRemoved(e);
        }

        internal static int GetUniqieInstanceID() {
            int id = registeredEntitiesIDs.Count + 1;
            if (registeredEntitiesIDs.Contains(id))
                id = GetUniqueIdRecursive(id);
            registeredEntitiesIDs.Add(id);
            return id;  
        }

        private static int GetUniqueIdRecursive(int previous) {
            int id = previous + 1;
            if (registeredEntitiesIDs.Contains(id))
                return GetUniqueIdRecursive(id);
            else
                return id;
        }

    }

}