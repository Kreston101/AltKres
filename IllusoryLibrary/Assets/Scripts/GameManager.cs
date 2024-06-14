using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int targetEntryId = 0;

    public static GameManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SwitchRoom(int entryPointId, string targetRoomName)
    {
        SceneManager.LoadScene(targetRoomName);
        StartCoroutine("WaitForRoomToLoad");
        targetEntryId = entryPointId;
        Debug.Log(targetEntryId);
        GameObject[] entryPoints = GameObject.FindGameObjectsWithTag("RoomExit");
        foreach (GameObject entry in entryPoints)
        {
            Debug.Log(entry.name);
            RoomExit roomExit = entry.GetComponent<RoomExit>();
            if (roomExit.myEntryId == entryPointId)
            {
                Debug.Log("yes");
                Debug.Log(targetEntryId + " " + roomExit.myEntryId);
                PlayerController.Instance.transform.position = roomExit.myEntryPoint.transform.position + Vector3.up * 10;
            }
            else
            {
                Debug.Log("no");
                Debug.Log(targetEntryId + " " + roomExit.myEntryId);
            }
        }
        entryPoints = GameObject.FindGameObjectsWithTag("RoomExit");
        Debug.Log(entryPoints[0].name);
    }

    IEnumerator WaitForRoomToLoad()
    {
        Debug.Log("Started");
        yield return new WaitForSeconds(1f);
    }
}
