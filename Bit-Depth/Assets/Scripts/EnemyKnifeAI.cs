using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnifeAI : MonoBehaviour
{
    private Transform playerRef;
    [SerializeField] Transform gunEndpoint;
    [SerializeField] GameObject bulletRef;

    [SerializeField] float speed;

    [SerializeField] private float stopDistance;
    [SerializeField] private float retreatDistance;

    [SerializeField] private float startAttackDelay;
    private float attackDelay = 0;

    [SerializeField] private Sprite level2;
    [SerializeField] private Sprite level3;

    [SerializeField] AudioClip[] dashSFX;

    private Rigidbody2D rb;

    private Vector2 playerKnock;

    private Camera cam;

    private int enemyLevel = 1;

    private void Awake()
    {
        playerRef = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    private void Start()
    {
        enemyLevel = Mathf.RoundToInt(Random.Range(1.0f, (float)SpawnerController.Instance.waveLevelIndex + 1));
        if (enemyLevel == 2)
        {
            transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = level2;
        }
        else if (enemyLevel == 3)
        {
            transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = level3;
        }
            speed = speed + SpawnerController.Instance.extraSpeed + Random.Range(-0.25f, 0.25f);
    }

    void FixedUpdate()
    {
        if (playerRef != null)
        {
            if (attackDelay <= 0)
            {

                // Move Towards Player
                if (Vector2.Distance(transform.position, playerRef.position) > stopDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, playerRef.position, speed * Time.deltaTime);
                }

                // Not Moving
                else if (Vector2.Distance(transform.position, playerRef.position) < stopDistance && Vector2.Distance(transform.position, playerRef.position) > retreatDistance)
                {
                    transform.position = this.transform.position;
                }

                // Move Away From Player
                else if (Vector2.Distance(transform.position, playerRef.position) < retreatDistance)
                {
                    transform.position = Vector2.MoveTowards(transform.position, playerRef.position, -speed * Time.deltaTime);
                }
            }
            // Will dash attack when close and not moving
            if (attackDelay <= 0 && (Vector2.Distance(transform.position, playerRef.position) < stopDistance && Vector2.Distance(transform.position, playerRef.position) > retreatDistance))
            {
                if (enemyLevel == 1)
                {
                    Vector3 dashDir = (playerRef.position - this.transform.position).normalized;
                    rb.AddForce(dashDir.normalized * 10, ForceMode2D.Impulse);
                    playerKnock = (dashDir.normalized * 2.5f);
                    attackDelay = startAttackDelay;
                }
                else if (enemyLevel == 2)
                {
                    Vector3 dashDir = (playerRef.position - this.transform.position).normalized;
                    rb.AddForce(dashDir.normalized * 12, ForceMode2D.Impulse);
                    playerKnock = (dashDir.normalized * 7.5f);
                    attackDelay = startAttackDelay + 1.0f;
                }
                else if (enemyLevel >= 3)
                {
                    Vector3 dashDir = (playerRef.position - this.transform.position).normalized;
                    rb.AddForce(dashDir.normalized * 8f, ForceMode2D.Impulse);
                    playerKnock = (dashDir.normalized * 1f);
                    attackDelay = startAttackDelay - 1.35f;
                }

                int random = Random.Range(0, 2);
                AudioHelper.PlayClip2D(dashSFX[random], 1);

            }
            else if (attackDelay > 0)
               {
                     attackDelay -= Time.deltaTime;
               }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerHealth>() != null)
        {
            if(collision.gameObject.GetComponent<PlayerHealth>().invincible == false)
            {
                collision.rigidbody.AddForce(playerKnock, ForceMode2D.Impulse);
            }
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
        }
    }

}
