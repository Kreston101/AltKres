using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveStation : MonoBehaviour
{
    private GameManager gm;
    private PlayerController playerRef;
    private Progress progress;
    public string sceneName;
    public bool canSaveGame = false;

    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        progress = Progress.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) && canSaveGame) 
        {
            progress.SaveObjectStateToFile();
            Debug.Log("interacted with save object");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canSaveGame = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canSaveGame = false;
        }
    }
}
