using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomManager : MonoBehaviour
{
    public GameObject roomBoss;
    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        roomBoss.GetComponent<InkBossController>().fightStarted = true;
        OnBossFightStarted();
        Debug.Log("fight started");
    }

    public void OnBossFightStarted()
    {
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
    }

    public void OnBossFightEnded()
    {
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(true);
        }
    }
}
