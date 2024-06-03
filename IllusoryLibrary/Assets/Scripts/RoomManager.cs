using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] levels;

    private GameObject currentLevel;

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = levels[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
