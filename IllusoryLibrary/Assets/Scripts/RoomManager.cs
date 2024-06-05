using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject[] exits;
    public GameObject[] entrances;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(player);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
