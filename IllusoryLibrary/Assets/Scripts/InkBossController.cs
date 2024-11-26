using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class InkBossController : MonoBehaviour
{
    private int waveCount = 1;
    [SerializeField] private int maxWaves;
    [SerializeField] private GameObject[] waveSpawnPoints;
    [SerializeField] private GameObject[] orbSpawnPoints;
    private bool waveCleared;

    public GameObject flyerFab;
    public GameObject walkerFab;
    public GameObject orbFab;

    private void CreateWave()
    {
        List<GameObject> tempWaveList = waveSpawnPoints.ToList();
        List<GameObject> tempListOrb = orbSpawnPoints.ToList();

        for (int i = 0; i < tempWaveList.Count; i++)
        {
            int randInt = UnityEngine.Random.Range(0, tempWaveList.Count);

            if(randInt == 3 || randInt == 6)
            {
                tempListOrb.Remove(waveSpawnPoints[randInt]);
            }

            if(randInt <= 3)
            {
                Instantiate(flyerFab, waveSpawnPoints[randInt].transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(walkerFab, waveSpawnPoints[randInt].transform.position, Quaternion.identity);
            }

            tempWaveList.RemoveAt(randInt);
        }
    }
}
