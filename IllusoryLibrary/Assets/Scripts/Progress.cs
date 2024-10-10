using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Progress : MonoBehaviour
{
    GameManager gm;
    PlayerController playerRef;
    Dictionary<int,bool> objsToSave = new Dictionary<int,bool>();
    //save point, scene + transform

    public static Progress Instance {  get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
        //gm = GameManager.Instance;
        playerRef = PlayerController.Instance;
        Debug.Log(objsToSave.Count);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            SaveToFile();
        }
    }

    //objects call this
    public void AddObjectToSave(int id, bool state)
    {
        if(!objsToSave.ContainsKey(id))
        {
            objsToSave.Add(id, state);
        }
    }

    //objs call this to load their state
    public bool LoadState(int id)
    {
        if (objsToSave.ContainsKey(id)) 
        {
            return true;
        }
        else return false;
    }

    public void SaveToFile()
    {
        string jsonString = JsonConvert.SerializeObject(objsToSave, Formatting.Indented);
        File.WriteAllText("save.json", jsonString);
        Debug.Log("Wrote file...idk where");
    }
}
