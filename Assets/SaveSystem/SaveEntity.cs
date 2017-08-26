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

        public bool SaveDisabled;


        public int ID
        {
            get {
                if (instanceID == 0) {
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



        private void Awake() {
            if (SaveDisabled)
                SaveEntityManager.RegisterEntity(this);
        }

        private void OnEnable() {
            if (!SaveDisabled)
                SaveEntityManager.RegisterEntity(this);
        }

        private void OnDisable() {
            if (!SaveDisabled)
                SaveEntityManager.RegisterEntity(this);
        }

        private void OnDestroy() {
            if (SaveDisabled)
                SaveEntityManager.UnRegisterEntity(this);
        }

        public void MakePersistent() {
            PersistentDataSystem.MakePersistent(this);
        }
    }
}