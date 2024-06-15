using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomChanger : MonoBehaviour
{
    [SerializeField] private RoomConnection connector;
    [SerializeField] private string targetRoom;
    [SerializeField] private Transform entryPoint;

    private void Start()
    {
        if (connector == RoomConnection.ActiveConnection)
        {
            PlayerController.Instance.transform.position = entryPoint.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Player")))
        {
            RoomConnection.ActiveConnection = connector;
            SceneManager.LoadScene(targetRoom); 
        }
    }
}
