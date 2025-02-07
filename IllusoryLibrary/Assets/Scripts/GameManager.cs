using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PlayerController playerRef;
    private Progress progressRef;

    private void Start()
    {
        playerRef = PlayerController.Instance;
        progressRef = Progress.Instance;
    }

    public void RespawnPlayer()
    {
        if(!playerRef.gameObject.activeInHierarchy)
        {
            playerRef.health = progressRef.progPlayerMaxHealth;
            SceneManager.LoadScene(progressRef.progLoadScene);
            Debug.Log("player died");
        }
    }
}
