namespace SaveSystem {
    using System.Collections;
    using SaveSystem.Utility;
    using UnityEngine;

    public class SavedComponent : MonoBehaviour {

        [NotEditableInt]
        public int componentID;

        protected bool m_initialized;
        public bool Initialized { get { return m_initialized; } }

        protected virtual void Awake() {
            if (!m_initialized) {
                Initialize();
                m_initialized = true;
            }
            entity = GetComponentInParent<SaveEntity>();
        }

        public void InjectEntity(SaveEntity entity) {
            this.entity = entity;
        }

        SaveEntity entity;
        public SaveEntity Entity
        {
            get {
                if (((object)entity) == null)
                    entity = GetComponentInParent<SaveEntity>();
                return entity;
            }
        }

        /// <summary>
        /// This method will be called before the component is saved. 
        /// </summary>
        protected virtual void OnBeforeSave() {

        }

        /// <summary>
        /// This method will be called as soon as we complete deserialization of the component. 
        /// </summary>
        protected virtual void OnAfterLoad() {

        }

        /// <summary>
        /// This method will be called the first time the Entity is created in the world. 
        /// Then it wont be called even if you save/load scene.
        /// Perform once in a lifetime operations here.
        /// </summary>
        protected virtual void Initialize() {

        }
    }


    public abstract class SaveData {

    }

}