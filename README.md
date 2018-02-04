# SaveSystem
save system for unity engine

This is a complete working save solution for unity, its a branch of https://github.com/pointcache/URSA serialization
made into a separate plugin. Look at URSA's save system for a tutorial, later ill add more docs here, im just busy for now.

# Terminology
  Currently the project uses the term Entity to describe a prefab that can be saved (this is because it was part of an ecs, its not anymore and i will change the name later.

# Info

SaveSystem uses prefabs as a starting point, when saving each "Entity" position/rotation (But not scale) is saved. 

You can save any data inside the components that reside inside the entity, for that however you would have to do some setup.



# Usage

System requires some setup prior to work

  1. Folder structure :
    1. Inside SaveSystem main folder, find SaveSystemSettings asset and check it out:
    Entities Folder must be a directory inside your Resources folder, where you would put all the prefabs(Entities) that need to be saved. So make sure you have that folder, by default its "Resources/Entities".
    2. Make sure you have folder at path "Resources/SaveSystem" its where we are going to store the manifest.
    
  2. Scene setup :
    1. create a gameobject in scene
    2. Add SaveSystem, and SaveEntityDatabase components to it.
    3. In SaveSystem you will see a field "Entities root", this must be a gameobject in the scene, to which all entities should be parented.
    
  3. Preparing to save the objects:
     1. Create an object you want to save, for now let it just be a cube.
     2. Add "SaveEntity" component to it.
     3. Make a prefab out of it and place the prefab inside the "Resources/Entities" (or how did you call that folder)
     4. Final step is to run the command from SaveSystem menu on top "SaveSystem/UpdateEntities", this command assigns id's to entities, so **it should be called each time you create a new entity prefab**.
     5. Make sure your entity in the scene is parented to EntityRoot object (refer to point 2.3)
     
  4. If you setup everything correctly you can now save your scene from the menu to test "SaveSystem/Save" and "SaveSystem/Load"
    1. If not, check Example scene once more.
    
# Using SavedComponent

1. Check example scene, Cube1 has a TestComponent attached

```cs

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

```

1. Component must inherit SavedComponent
2. Data that you plan on serializing must be inside a new Data class that inherits SaveData
  and be a public field that is called "data". SaveSystem will look for it.
3. OnSave, OnLoad are **optional** and can be used to customize what data you want to save.
4. After you added a SavedComponent to an Entity, you must once again run the "SaveSystem/UpdateEntities" command, to assign proper ID's to SavedComponents.

