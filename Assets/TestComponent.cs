using System.Collections;
using System.Collections.Generic;
using SaveSystem;
using UnityEngine;

public class TestComponent : SavedComponent {

    public Data data;

    [System.Serializable]
    public class Data : SaveData {

        public Color color;
        public string somestring;

    }

    protected override void OnBeforeSave() {
        base.OnBeforeSave();

        Debug.Log("I am saving some data!");
    }

    protected override void OnAfterLoad() {
        base.OnAfterLoad();

        Debug.Log("I finished saving data!");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
