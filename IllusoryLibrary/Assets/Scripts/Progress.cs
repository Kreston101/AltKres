using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Progress : MonoBehaviour
{
    private GameData gameData;
    private PlayerController playerRef;
    [SerializeField]private List<IDataPersistence> dataPersistenceObjects;

    //needs a copy of all varibles in GameData obj
    public string progLoadScene;
    public Dictionary<string, bool> progWalls;
    public Dictionary<string, bool> progCollectables;

    public static Progress Instance {  get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.Log("destroyed dupe");
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
        LoadFromFile();
        playerRef = PlayerController.Instance;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    //get all save able objs after scene loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadObjsData();
    }

    private void OnSceneUnloaded(Scene current)
    {
        SaveObjsData();
    }

    private void OnApplicationQuit()
    {
        SaveToFile();
    }

    //call ON SCENE ENTER
    public void LoadObjsData()
    {
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData();
            Debug.Log(dataPersistenceObj.ToString());
        }
    }

    //called ON SCENE EXIT
    public void SaveObjsData()
    {
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData();
            Debug.Log(dataPersistenceObj.ToString());
        }
    }

    //gets everything implementing save interface
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().
            OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    //saves all data to file
    public void SaveToFile()
    {
        SaveObjsData();

        gameData.loadScene = progLoadScene; 
        gameData.walls = progWalls; 
        gameData.collectables = progCollectables;

        string jsonString = JsonConvert.SerializeObject(gameData, Formatting.Indented);
        File.WriteAllText("../IllusoryLibrary/Save.json", jsonString);
        Debug.Log("game saved");
    }

    //loads data from file if there is any, if not starts new game i guess
    public void LoadFromFile()
    {
        if(File.Exists("../IllusoryLibrary/Save.json"))
        {
            string jsonString = File.ReadAllText("../IllusoryLibrary/Save.json");
            gameData = JsonConvert.DeserializeObject<GameData>(jsonString);

            progLoadScene = gameData.loadScene;
            progWalls = gameData.walls;
            progCollectables = gameData.collectables;

            Debug.Log("save loaded");
        }
        else
        {
            gameData = new GameData();

            //load default values
            progLoadScene = gameData.loadScene;
            progWalls = gameData.walls;
            progCollectables = gameData.collectables;

            Debug.Log("no save file");
            Debug.Log(gameData);
        }
    }
}
