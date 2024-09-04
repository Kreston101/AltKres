using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyer : MonoBehaviour
{
    public int health = 3;
    public int damage = 1;

    private Rigidbody2D rb2d;
    private bool damaged = false;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator TakeDamage(int damage)
    {
        Debug.Log("hit taken");
        damaged = true;
        health -= damage;
        rb2d.velocity = new Vector2(transform.localScale.x * 15 * -1, 0);
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
            PlayerController.Instance.StartCoroutine("TakeDamage", damage);
        }
    }
}
