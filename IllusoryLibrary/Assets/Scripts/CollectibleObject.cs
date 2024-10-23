using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    public bool collected = false;
    public string objId;

    private void Start()
    {
        collected = Progress.Instance.LoadObjectState(objId);
        if (collected)
        {
            gameObject.SetActive(false);
        }
        else gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == PlayerController.Instance.gameObject)
        {
            collected = true;
            gameObject.SetActive(false);
            Progress.Instance.SaveObjectState(objId, collected); 
        }
    }
}
