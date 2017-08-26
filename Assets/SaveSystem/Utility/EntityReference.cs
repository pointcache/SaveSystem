namespace SaveSystem {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class EntityReference : MonoBehaviour {

        SaveEntity entity;
        public SaveEntity Entity
        {
            get {
                if (((object)entity) == null)
                    entity = GetComponentInParent<SaveEntity>();
                return entity;
            }
        }
    }

}