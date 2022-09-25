using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPistolAI : MonoBehaviour
{
    private Transform playerRef;
    [SerializeField] Transform gunEndpoint;
    [SerializeField] GameObject bulletRef;

    [SerializeField] float speed;

    [SerializeField] private float stopDistance;
    [SerializeField] private float retreatDistance;

    [SerializeField] private float startFireDelay;
    private float fireDelay;

    [SerializeField] private Sprite level2;
    [SerializeField] private Sprite level3;

    [SerializeField] AudioClip[] enemyShootSFX;

    private Camera cam;

    private int enemyLevel = 1;

    private void Awake()
    {
        playerRef = GameObject.Find("Player").transform;
        cam = Camera.main;
    }

    private void Start()
    {
        enemyLevel = Mathf.RoundToInt(Random.Range(1.0f, (float)SpawnerController.Instance.waveLevelIndex + 1));
        speed = speed + SpawnerController.Instance.extraSpeed + Random.Range(-0.25f, 0.25f);
        if (enemyLevel == 2)
        {
            transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = level2;
        }
        else if (enemyLevel == 3)
        {
            transform.GetChild(3).GetComponent<SpriteRenderer>().sprite = level3;
        }
    }

    void FixedUpdate()
    {
        if (playerRef != null)
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

            // Will only fire when not running away and on screen 
            Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
            if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
            {
                if (fireDelay <= 0 && ((Vector2.Distance(transform.position, playerRef.position) > stopDistance) || ( (Vector2.Distance(transform.position, playerRef.position) < stopDistance) && (Vector2.Distance(transform.position, playerRef.position) > retreatDistance))))
                {
                    if (enemyLevel == 1)
                    {
                        Transform bulletTransform = Instantiate(bulletRef.transform, gunEndpoint.position, Quaternion.identity);
                        Vector3 shootDir = ((playerRef.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f))) - transform.position).normalized;
                        bulletTransform.GetComponent<EnemyBullets>().BulletSetup(shootDir);
                        fireDelay = startFireDelay;
                    }
                    else if (enemyLevel == 2)
                    {
                        Transform bulletTransform = Instantiate(bulletRef.transform, gunEndpoint.position, Quaternion.identity);
                        Vector3 shootDir = (playerRef.position - transform.position).normalized;
                        shootDir = Quaternion.AngleAxis(7.5f, Vector3.forward) * shootDir;
                        bulletTransform.GetComponent<EnemyBullets>().BulletSetup(shootDir);

                        Transform bulletTransform2 = Instantiate(bulletRef.transform, gunEndpoint.position, Quaternion.identity);
                        Vector3 shootDir2 = (playerRef.position - transform.position).normalized;
                        shootDir2 = Quaternion.AngleAxis(-7.5f, Vector3.forward) * shootDir2;
                        bulletTransform2.GetComponent<EnemyBullets>().BulletSetup(shootDir2);
                        fireDelay = startFireDelay;
                    }
                    else if (enemyLevel >= 3)
                    {
                        Transform bulletTransform = Instantiate(bulletRef.transform, gunEndpoint.position, Quaternion.identity);
                        Vector3 shootDir = (playerRef.position - transform.position).normalized;
                        shootDir = Quaternion.AngleAxis(12.5f, Vector3.forward) * shootDir;
                        bulletTransform.GetComponent<EnemyBullets>().BulletSetup(shootDir);

                        Transform bulletTransform2 = Instantiate(bulletRef.transform, gunEndpoint.position, Quaternion.identity);
                        Vector3 shootDir2 = (playerRef.position - transform.position).normalized;
                        shootDir2 = Quaternion.AngleAxis(0, Vector3.forward) * shootDir2;
                        bulletTransform2.GetComponent<EnemyBullets>().BulletSetup(shootDir2);

                        Transform bulletTransform3 = Instantiate(bulletRef.transform, gunEndpoint.position, Quaternion.identity);
                        Vector3 shootDir3 = (playerRef.position - transform.position).normalized;
                        shootDir3 = Quaternion.AngleAxis(-12.5f, Vector3.forward) * shootDir3;
                        bulletTransform3.GetComponent<EnemyBullets>().BulletSetup(shootDir3);
                        fireDelay = startFireDelay;
                    }

                    int random = Random.Range(0, 1);
                    AudioHelper.PlayClip2D(enemyShootSFX[random], 1);
                }
                else
                {
                    fireDelay -= Time.deltaTime;
                }
            }
        }
    }
}
