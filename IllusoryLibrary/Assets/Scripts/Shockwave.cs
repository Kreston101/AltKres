using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    public int damage;
    public float speed;
    public Vector2 direcion;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 5f);
    }

    private void FixedUpdate()
    {
        rb2d.velocity = direcion * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerController.Instance.gameObject)
        {
            Debug.Log("whack");
            PlayerController.Instance.StartCoroutine(PlayerController.Instance.TakeDamage(damage, transform.localScale.x, GetComponent<Collider2D>(), 15f));
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
