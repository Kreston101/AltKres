using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
//    [Header("File storage config")]
//    [SerializeField] private string fileName;

//    private GameData gameData;
//    private List<IDataPersistence> dataPersistenceObjects;
//    private FileDataHandler dataHandler;

//    public static DataPersistenceManager Instance { get; private set; }

//    private void Awake()
//    {
//        if(Instance != null)
//        {
//            Debug.LogError("Theres more than one manager?!");
//        }
//        Instance = this;
//    }

//    private void Start()
//    {
//        //change the path
//        this.dataHandler = new FileDataHandler("C:/GamesStuff/Projects/AltKres/IllusoryLibrary/", fileName);
//        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
//        LoadGame();
//    }

//    public void NewGame()
//    {
//        this.gameData = new GameData();
//    }

//    public void LoadGame()
//    {
//        this.gameData = dataHandler.Load();

//        if (this.gameData == null)
//        {
//            Debug.Log("No save file");
//            NewGame();
//        }

//        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
//        {
//            dataPersistenceObj.LoadData(gameData);
//        }
//        //Debug.Log($"current dmg taken is {gameData.dmgCount}");
//    }

//    public void SaveGame()
//    {
//        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
//        {
//            dataPersistenceObj.SaveData(ref gameData);
//        }
//        //Debug.Log($"saved {gameData.dmgCount} damage taken");
//        dataHandler.Save(gameData);
//    }

//    private void OnApplicationQuit()
//    {
//        SaveGame();
//    }

//    private List<IDataPersistence> FindAllDataPersistenceObjects()
//    {
//        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().
//            OfType<IDataPersistence>();

//        return new List<IDataPersistence>(dataPersistenceObjects);
//    }
}
