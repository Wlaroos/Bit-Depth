using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySniperAI : MonoBehaviour
{
    private Transform playerRef;
    [SerializeField] Transform gunEndpoint;
    [SerializeField] GameObject bulletRef;
    [SerializeField] GameObject skullBulletRef;

    [SerializeField] float speed;

    [SerializeField] private float stopDistance;
    [SerializeField] private float retreatDistance;

    [SerializeField] private float startFireDelay;
    private float fireDelay;

    [SerializeField] private Sprite level2;
    [SerializeField] private Sprite level3;

    [SerializeField] AudioClip[] enemyShootSFX;

    public LineRenderer sniperLine;

    Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.15f;

    private Camera cam;

    private int enemyLevel = 1;

    private void Awake()
    {
        playerRef = GameObject.Find("Player").transform;

        cam = Camera.main;

        sniperLine = transform.GetChild(1).gameObject.AddComponent<LineRenderer>();
        sniperLine.positionCount = 2;
        sniperLine.material.SetTexture("_MainTex", Resources.Load<Texture2D>("Materials/Square"));
        sniperLine.material.shader = Shader.Find("Sprites/Default");
        sniperLine.startWidth = 0.05f;
        sniperLine.endWidth = 0.02f;
        sniperLine.enabled = false;
    }

    void Start()
    {
        sniperLine.startColor = GlobalMaterial.Instance._gradient01.GetColor("Color04");
        sniperLine.endColor = GlobalMaterial.Instance._gradient01.GetColor("Color04");
        enemyLevel = Mathf.RoundToInt(Random.Range(1.0f, (float)SpawnerController.Instance.waveLevelIndex + 1));
        speed = speed + SpawnerController.Instance.extraSpeed + Random.Range(-0.25f, 0.25f);
        startFireDelay += 0.75f;
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

            sniperLine.SetPosition(0, gunEndpoint.position);
            sniperLine.SetPosition(1, playerRef.position);

            // Move Towards Player

            Vector3 viewPos = cam.WorldToViewportPoint(transform.position);

            if ((Vector2.Distance(transform.position, playerRef.position) > stopDistance) || !(viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1))
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

            // Will only fire if they are not running or chasing
            if (fireDelay <= 0 && (Vector2.Distance(transform.position, playerRef.position) > retreatDistance && (Vector2.Distance(transform.position, playerRef.position) < stopDistance)))
            {
                if (enemyLevel == 1)
                {
                    sniperLine.enabled = true;
                    Invoke("FireBullet", 0.75f);
                    fireDelay = startFireDelay;
                }
                else if (enemyLevel == 2)
                {
                    sniperLine.enabled = true;
                    Invoke("FireSkullBullet", 0.75f);
                    fireDelay = startFireDelay;
                }
                else if (enemyLevel >= 3)
                {
                    sniperLine.enabled = true;
                    Invoke("FireDoubleBullet", 0.75f);
                    fireDelay = startFireDelay;
                }

            }
            else
            {
                fireDelay -= Time.deltaTime;
            }
        }
    }

    private void FireBullet()
    {
        sniperLine.enabled = false;

        int random = Random.Range(0, 1);
        AudioHelper.PlayClip2D(enemyShootSFX[random], 1);

        Transform bulletTransform = Instantiate(bulletRef.transform, gunEndpoint.position, Quaternion.identity);
        Vector3 shootDir = (playerRef.position - transform.position).normalized;
        bulletTransform.GetComponent<EnemyBullets>().BulletSetup(shootDir);

    }

    private void FireDoubleBullet()
    {
        sniperLine.enabled = false;

        int random = Random.Range(0, 1);
        AudioHelper.PlayClip2D(enemyShootSFX[random], 1);

        Transform bulletTransform = Instantiate(bulletRef.transform, gunEndpoint.position, Quaternion.identity);
        Vector3 shootDir = (playerRef.position - transform.position).normalized;
        shootDir = Quaternion.AngleAxis(3f, Vector3.forward) * shootDir;
        bulletTransform.GetComponent<EnemyBullets>().BulletSetup(shootDir);

        Transform bulletTransform2 = Instantiate(bulletRef.transform, gunEndpoint.position, Quaternion.identity);
        Vector3 shootDir2 = (playerRef.position - transform.position).normalized;
        shootDir2 = Quaternion.AngleAxis(-3f, Vector3.forward) * shootDir2;
        bulletTransform2.GetComponent<EnemyBullets>().BulletSetup(shootDir2);
    }

    private void FireSkullBullet()
    {
        sniperLine.enabled = false;

        int random = Random.Range(0, 1);
        AudioHelper.PlayClip2D(enemyShootSFX[random], 1);

        Transform bulletTransform = Instantiate(skullBulletRef.transform, gunEndpoint.position, Quaternion.identity);
        Vector3 shootDir = (playerRef.position - transform.position).normalized;
        bulletTransform.GetComponent<EnemyBullets>().BulletSetup(shootDir);

    }
}
