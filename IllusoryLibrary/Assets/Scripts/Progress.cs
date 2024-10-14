using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Progress : MonoBehaviour
{
    private GameManager gm;
    private PlayerController playerRef;
    private Dictionary<int,bool> objsToSave = new Dictionary<int,bool>();

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
        LoadFromFile();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveToFile();
        }
    }

    //objects call this
    public void SaveObjectState(int id, bool state)
    {
        if(!objsToSave.ContainsKey(id))
        {
            objsToSave.Add(id, state);
        }
    }

    //objs call this to load their state
    public bool LoadObjectState(int id)
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
        File.WriteAllText("../IllusoryLibrary/save.json", jsonString);
    }

    public void LoadFromFile()
    {
        if(File.Exists("../IllusoryLibrary/save.json"))
        {
            string jsonString = File.ReadAllText("../IllusoryLibrary/save.json");
            objsToSave = JsonConvert.DeserializeObject<Dictionary<int, bool>>(jsonString);
        }
        else
        {
            Debug.Log("no save file");
        }
    }
}
