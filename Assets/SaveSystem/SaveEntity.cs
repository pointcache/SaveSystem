namespace SaveSystem {
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using SaveSystem.ECS;
    using SaveSystem.Utility;

    using SaveSystem.Serialization;
    using SaveSystem.ECS.Entity;
    using System.Collections.ObjectModel;
    using System.Collections;

    public class SaveEntity : MonoBehaviour {

        [NotEditableInt]
        public int entityID;
        [NotEditableInt]
        public int instanceID;
        [NotEditableInt]
        public int blueprintID;

        public int ID
        {
            get {
                if (instanceID == 0 && Application.isPlaying) {
                    instanceID = SaveEntityManager.GetUniqieInstanceID();
                }
                return instanceID;
            }
        }

#if UNITY_EDITOR
        private void Reset() {
            for (int i = 0; i < 50; i++) {
                UnityEditorInternal.ComponentUtility.MoveComponentUp(this);

            }
        }
#endif

        public void InitializeDisabled() {
            var comps = GetComponentsInChildren<SavedComponent>(true);
            foreach (var comp in comps) {
                comp.InjectEntity(this);
            }
        }

        private void Awake() {
            SaveEntityManager.RegisterEntity(this);
        }

        private void OnDestroy() {
            SaveEntityManager.UnRegisterEntity(this);
        }

        public void MakePersistent() {
            PersistentDataSystem.MakePersistent(this);
        }
    }
}