namespace SaveSystem {

    using UnityEngine;
    using System;
    using FullSerializer;
    using SaveSystem.Serialization;

    [Serializable, fsObject(Processor = typeof(CompRefSerializationProcessor))]
    public class CompRef {

        [HideInInspector]
        public int entity_ID;
        [HideInInspector]
        public int component_ID;
        [HideInInspector]
        public bool isNull;
        [HideInInspector]
        public string entityName;

        public SavedComponent component;

        public static implicit operator SavedComponent(CompRef var) {
            return var.component;
        }
    }

    public class CompRef<T> : CompRef where T : SavedComponent {

        public static implicit operator T(CompRef<T> var) {
            return var.component as T;
        }
    }
}