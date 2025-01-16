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

    private void Update()
    {
        if(!playerRef.gameObject.activeInHierarchy)
        {
            SceneManager.LoadScene(progressRef.progLoadScene);
        }
    }
}
