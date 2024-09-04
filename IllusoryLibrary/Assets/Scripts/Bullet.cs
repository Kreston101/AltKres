using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb2d;
    [SerializeField] private float speed = 50;
    [SerializeField] private int maxPierce = 1;
    [SerializeField] private int piercedCount = 0;
    public float direction;
    public int damage;

    [SerializeField] private LayerMask attackableLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(direction * speed, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if(piercedCount < maxPierce)
            {
                if (collision.GetComponent<Enemy>())
                {
                    collision.gameObject.GetComponent<Enemy>().StartCoroutine("TakeDamage", damage);
                }
                else if (collision.GetComponent<Flyer>())
                {
                    collision.gameObject.GetComponent<Flyer>().StartCoroutine("TakeDamage", damage);
                }
            }
            else
            {
                Destroy(gameObject);
                if (collision.GetComponent<Enemy>())
                {
                    collision.gameObject.GetComponent<Enemy>().StartCoroutine("TakeDamage", damage);
                }
                else if (collision.GetComponent<Flyer>())
                {
                    collision.gameObject.GetComponent<Flyer>().StartCoroutine("TakeDamage", damage);
                }
            }
        }
        else if (collision.gameObject.CompareTag("Breakable"))
        {
            collision.GetComponent<BreakableObject>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
