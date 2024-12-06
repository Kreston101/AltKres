using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class InkBossController : MonoBehaviour
{
    public bool fightStarted;
    public int damage;
    public int health;

    private PlayerController player;
    private bool damaged = false;

    //moves towards player by jumping (set distance or towards player location)?
    //moves away from player by sliding backwards
    //idles a bit
    //charges a knockback wave (varible charge time?)

    private void Start()
    {
        player = PlayerController.Instance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("something entered");
        if (collision.gameObject == PlayerController.Instance.gameObject)
        {
            Debug.Log("player entered");
            PlayerController.Instance.StartCoroutine(PlayerController.Instance.TakeDamage(damage, transform.localScale.x));
        }
    }
}
