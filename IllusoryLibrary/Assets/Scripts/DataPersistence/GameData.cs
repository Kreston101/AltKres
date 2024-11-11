using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string loadScene;
    public Dictionary<string, bool> walls;
    public Dictionary<string, bool> collectables;

    public GameData()
    {
        loadScene = "StartRoom"; 
        //file data handler > datapersist > scene manager(?) Load()
        //in theory that should load the scene name into the scene manager
        //then it knows to load the scene
        //maybe change it back to objs calling load?
        //im srs considering rewriting this shit again
        //or keep watching that tutorial, trim as needed
        walls = new Dictionary<string, bool>();
        collectables = new Dictionary<string, bool>();
    }
}
