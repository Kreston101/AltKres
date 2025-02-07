using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private PlayerController playerRef;
    private Progress progressRef;

    //ref for health strip
    //ref for bullet strip

    // Start is called before the first frame update
    void Start()
    {
        playerRef = PlayerController.Instance;
        progressRef = Progress.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
