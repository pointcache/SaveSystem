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



        private void OnEnable() {
            SaveEntityManager.RegisterEntity(this);
            AddEntityReferenceToColliders();
        }

        private void OnDisable() {
            SaveEntityManager.UnRegisterEntity(this);
        }


        private void AddEntityReferenceToColliders() {

            Transform tr = transform;

            int count = tr.childCount;

            for (int i = 0; i < count; i++) {

                AddEntityReferenceToCollidersRecursive(tr.GetChild(i));

            }

        }

        private void AddEntityReferenceToCollidersRecursive(Transform tr) {

            int count = tr.childCount;

            for (int i = 0; i < count; i++) {

                AddEntityReferenceToCollidersRecursive(tr.GetChild(i));

            }

            if (tr.GetComponent<Collider>() && !tr.GetComponent<EntityReference>())
                tr.gameObject.AddComponent<EntityReference>();
        }

        public void MakePersistent() {
            PersistentDataSystem.MakePersistent(this);
        }
    }
}