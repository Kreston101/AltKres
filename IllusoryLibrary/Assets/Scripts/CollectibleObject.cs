using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleObject : MonoBehaviour, IDataPersistence
{
    public bool collected = false;
    public string objId;

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == PlayerController.Instance.gameObject)
        {
            collected = true;
            gameObject.SetActive(false);
        }
    }

    public void LoadData()
    {
        Progress.Instance.progCollectables.TryGetValue(objId, out collected);
        if (collected)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData()
    {
        if (Progress.Instance.progCollectables.ContainsKey(objId))
        {
            Progress.Instance.progCollectables[objId] = collected;
        }
        else
        {
            Progress.Instance.progCollectables.Add(objId, collected);
        }
    }
}
