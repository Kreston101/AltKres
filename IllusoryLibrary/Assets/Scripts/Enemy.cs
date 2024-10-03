using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Enemy : MonoBehaviour //make as base class at some point or smthing
{
    public int health = 5;
    public int damage = 1;

    private Rigidbody2D rb2d;
    private float timer = 0f;
    [SerializeField] float walkTime = 4f;
    private bool damaged = false;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
        }

        //if(!damaged)
        //{
        //    if (timer <= walkTime)
        //    {
        //        timer += Time.deltaTime;
        //        rb2d.velocity = new Vector2(transform.localScale.x * 3, 0);
        //    }
        //    else
        //    {
        //        timer = 0;
        //        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        //    }
        //}
    }

    public IEnumerator TakeDamage(int damage)
    {
        damaged = true;
        health -= damage;
        rb2d.velocity = new Vector2(PlayerController.Instance.transform.localScale.x * 15, 0);
        yield return new WaitForSeconds(0.1f);
        rb2d.velocity = Vector2.zero;
        damaged = false;
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
