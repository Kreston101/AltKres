using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//can also be interfaced later?
public class BreakableObject : MonoBehaviour, IDataPersistence
{
    public int health = 5;
    public bool destroyed = false;
    public string objId;

    private void Start()
    {

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            gameObject.SetActive(false);
            destroyed = true;
        }
    }

    public void LoadData()
    {
        Progress.Instance.progWalls.TryGetValue(objId, out destroyed);
        if (destroyed)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData()
    {
        if (Progress.Instance.progWalls.ContainsKey(objId))
        {
            Progress.Instance.progWalls[objId] = destroyed;
        }
        else
        {
            Progress.Instance.progWalls.Add(objId, destroyed);
        }
    }
}
