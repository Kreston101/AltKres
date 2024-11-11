using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Progress : MonoBehaviour
{
    private GameManager gm;
    private PlayerController playerRef;
    private Dictionary<string,bool> objsToSave = new Dictionary<string,bool>();

    string objectsSavePath;
    string playerStatsPath;
    //save point, scene + transform

    public static Progress Instance {  get; private set; }

    private void Awake()
    {
        //if (Instance != null && Instance != this)
        //{
        //    Destroy(gameObject);
        //}
        //else
        //{
        //    Instance = this;
        //}
        //DontDestroyOnLoad(this);
        ////gm = GameManager.Instance;
        //playerRef = PlayerController.Instance;
        //LoadObjectStateFromFile();
        //Debug.Log(objsToSave.Count);
        ////Debug.Log("loading game");
    }

    //objects call this
    public void SaveObjectState(string id, bool state)
    {
        if(!objsToSave.ContainsKey(id))
        {
            objsToSave.Add(id, state);
        }
    }

    //objs call this to load their state
    public bool LoadObjectState(string id)
    {
        if (objsToSave.ContainsKey(id))
        {
            return true;
        }
        else return false;
    }

    public void SaveObjectStateToFile()
    {
        string jsonString = JsonConvert.SerializeObject(objsToSave, Formatting.Indented);
        File.WriteAllText("../IllusoryLibrary/Save.json", jsonString);
        Debug.Log("game saved");
    }

    public void LoadObjectStateFromFile()
    {
        if(File.Exists("../IllusoryLibrary/Save.json"))
        {
            string jsonString = File.ReadAllText("../IllusoryLibrary/Save.json");
            objsToSave = JsonConvert.DeserializeObject<Dictionary<string, bool>>(jsonString);
            Debug.Log("game loaded");
        }
        else
        {
            Debug.Log("no save file");
        }
    }
}
