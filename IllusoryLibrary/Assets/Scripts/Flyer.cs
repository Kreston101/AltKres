using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flyer : MonoBehaviour
{
    public int health = 3;
    public int damage = 1;
    public float speed = 3;
    public float aggroDist = 10f;

    private Rigidbody2D rb2d;
    private bool damaged = false;
    private bool chase = false;

    private GameObject player;
    private Vector3 origin;
    [SerializeField] private LayerMask playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = PlayerController.Instance.gameObject;
        origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Vector3 distance = player.transform.position - transform.position;
        //Debug.Log(distance);
        //Debug.DrawLine(transform.position, player.transform.position);

        if (distance.magnitude <= aggroDist && !damaged)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, distance, aggroDist);
            for (int i = 1; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject != player)
                {
                    //Debug.Log(hits[i].collider.gameObject);
                    break;
                }
                else
                {
                    //Debug.Log(hits[i].collider.gameObject);
                    if(distance.x < 0)
                    {
                        transform.localScale = new Vector2(-1, transform.localScale.y);
                    }
                    else if(distance.x > 0)
                    { 
                        transform.localScale = new Vector2(1, transform.localScale.y); 
                    }
                    rb2d.MovePosition(Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed));
                }
            }
        }
    }

    public IEnumerator TakeDamage(int damage)
    {
        Debug.Log("hit taken");
        damaged = true;
        health -= damage;
        rb2d.velocity = new Vector2(PlayerController.Instance.transform.localScale.x * 15, 0);
        yield return new WaitForSeconds(0.25f);
        rb2d.velocity = Vector2.zero;
        damaged = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("something entered");
        if (collision.gameObject == PlayerController.Instance.gameObject)
        {
            Debug.Log("player entered");
            PlayerController.Instance.StartCoroutine(PlayerController.Instance.TakeDamage(damage, transform.localScale.x, GetComponent<Collider2D>(), 15f));
        }
    }
}
