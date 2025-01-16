using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class InkBossController : MonoBehaviour
{
    public bool fightStarted;
    public int damage;
    public int health;

    public float minIdleTime;
    public float maxIdleTime;

    public int hitsDuringIdle;

    public Vector3 centerOfArena;
    public GameObject shockwaves;

    private PlayerController player;
    private Rigidbody2D rb2d;
    private bool damaged = false;
    private bool isIdle = true;
    private float timer = 0f;
    private float randomIdleTime;
    private bool grounded = false;
    [SerializeField] private GameObject roomManager;

    //DONE: BUT CLUNKY moves towards player by jumping (set distance or towards player location)?
    //DONE: moves away from player by sliding backwards
    //DONE: idles a bit
    //DONE: charges a knockback wave (varible charge time?)

    private void Start()
    {
        player = PlayerController.Instance;
        rb2d = GetComponent<Rigidbody2D>();
        randomIdleTime = UnityEngine.Random.Range(minIdleTime, maxIdleTime);
    }

    private void Update()
    {
        if (fightStarted)
        {
            if (isIdle)
            {
                timer += Time.deltaTime;
                if (timer > randomIdleTime)
                {
                    int rand = UnityEngine.Random.Range(0, 10);
                    if (rand > 7)
                    {
                        StartCoroutine(ShockwaveAttack());
                        Debug.Log(hitsDuringIdle);
                        hitsDuringIdle = 0;
                    }
                    else
                    {
                        StartCoroutine(JumpAtPlayer());
                        Debug.Log(hitsDuringIdle);
                        hitsDuringIdle = 0;
                    }
                }
                else if (hitsDuringIdle > 3)
                {
                    StartCoroutine(SlideBack());
                    timer = randomIdleTime;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("something entered");
        if (collision.gameObject == PlayerController.Instance.gameObject)
        {
            Debug.Log("player entered");
            PlayerController.Instance.StartCoroutine(PlayerController.Instance.TakeDamage(damage, transform.localScale.x, GetComponent<Collider2D>(), 25f));
        }
        if(collision.gameObject.layer == 6)
        {
            grounded = true;
        }
    }

    public void TakeDamage(int playerDamage)
    {
        if (health > 0)
        {
            health -= playerDamage;
            if (isIdle)
            {
                hitsDuringIdle++;
            }
        }
        else
        {
            //add killed state to save
            gameObject.SetActive(false);
            fightStarted = false;
            roomManager.GetComponent<BossRoomManager>().OnBossFightEnded();
        }
    }

    IEnumerator SlideBack()
    {
        isIdle = false;
        Vector2 direction = transform.position - player.transform.position;
        if (direction.x > 0)
        {
            rb2d.velocity = Vector2.right * 10f;
        }
        else
        {
            rb2d.velocity = Vector2.left * 10f;
        }
        yield return new WaitForSeconds(0.5f);
        rb2d.velocity = Vector2.zero;
        randomIdleTime = UnityEngine.Random.Range(minIdleTime, maxIdleTime);
        isIdle = true;
    }

    IEnumerator JumpAtPlayer()
    {
        isIdle = false;
        float distance = player.transform.position.x - transform.position.x;
        if (distance < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
        else if (distance > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
        rb2d.velocity = new Vector2(distance/2f, 11f);
        yield return new WaitForSeconds(0.75f);
        timer = 0;
        randomIdleTime = UnityEngine.Random.Range(minIdleTime, maxIdleTime);
        isIdle = true;
    }

    IEnumerator ShockwaveAttack()
    {
        isIdle = false;
        Vector2 distance = centerOfArena - transform.position;
        if (distance.x < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
        else if (distance.x > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
        rb2d.velocity = new Vector2(distance.x / 2f, 11f);
        grounded = false;
        yield return new WaitUntil(() => grounded == true);
        //charge anim
        yield return new WaitForSeconds(3f);
        rb2d.velocity = new Vector2(0, 13f);
        grounded = false;
        yield return new WaitUntil(() => grounded == true);
        GameObject holder;
        holder = Instantiate(shockwaves, gameObject.transform.position, Quaternion.identity);
        holder.GetComponent<Shockwave>().damage = damage;
        holder.GetComponent<Shockwave>().direcion = Vector2.left;
        holder.GetComponent<Shockwave>().transform.localScale = new Vector3(-0.5f, 3, 1);
        holder = Instantiate(shockwaves, gameObject.transform.position, Quaternion.identity);
        holder.GetComponent<Shockwave>().damage = damage;
        holder.GetComponent<Shockwave>().direcion = Vector2.right;
        timer = 0;
        randomIdleTime = UnityEngine.Random.Range(minIdleTime, maxIdleTime);
        isIdle = true;
    }
}
