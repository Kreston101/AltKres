using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public int health = 5;
    public bool destroyed = false;
    public int objId;

    private void Start()
    {
        destroyed = Progress.Instance.LoadObjectState(objId);
        if (destroyed)
        {
            gameObject.SetActive(false);
        }
        else gameObject.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            gameObject.SetActive(false);
            destroyed = true;
            Progress.Instance.SaveObjectState(objId, destroyed);
        }
    }
}
