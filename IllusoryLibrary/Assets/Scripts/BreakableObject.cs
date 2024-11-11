using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void LoadData(GameData data)
    {
        data.walls.TryGetValue(objId, out destroyed);
        if (destroyed)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        if(data.walls.ContainsKey(objId))
        {
            data.walls[objId] = destroyed;
        }
        else
        {
            data.walls.Add(objId, destroyed);
        }
    }
}
