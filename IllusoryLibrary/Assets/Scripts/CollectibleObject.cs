using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleObject : MonoBehaviour//, IDataPersistence
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

    //public void LoadData(GameData data)
    //{
    //    data.collectables.TryGetValue(objId, out collected);
    //    if (collected)
    //    {
    //        gameObject.SetActive(false);
    //    }
    //}

    //public void SaveData(ref GameData data)
    //{
    //    if (data.collectables.ContainsKey(objId))
    //    {
    //        data.collectables[objId] = collected;
    //    }
    //    else
    //    {
    //        data.collectables.Add(objId, collected);
    //    }
    //}
}
