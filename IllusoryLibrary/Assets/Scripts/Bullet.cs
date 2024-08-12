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
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            if (piercedCount < maxPierce)
            {
                piercedCount++;
                collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
                Debug.Log("ranged hit");
            }
            else
            {
                Destroy(gameObject);
                collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
        else
        {
            Debug.Log("No coll???");
        }
    }
}
