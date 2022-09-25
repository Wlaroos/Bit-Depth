using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class Enemy : MonoBehaviour
{

    public Vector3 gunEndPointPosition;
    public Vector3 shootPosition;

    [SerializeField] GameObject _scorePopup;

    [SerializeField] private int maxHealth = 3;
    private int currentHealth = 3;
    private Rigidbody2D rb;

    [SerializeField] private int _score = 1;
    private int initScore;
    [SerializeField] private float _combo = 0.1f;
    private float initCombo;
    [SerializeField] private float _knockbackMult = 1.0f;
    [SerializeField] ParticleSystem ps;

    [SerializeField] AudioClip[] enemyHitSFX;
    [SerializeField] AudioClip[] enemyDeathSFX;

    private Transform playerRef;

    private Transform aimTransform;
    private Transform spriteTransform;
    private Transform aimGunEndPointTransform;
    private Transform healthBar;

    private Animator aimAnimator;
    private Animator animator;

    void Awake()
    {
        initScore = _score;
        initCombo = _combo;
        playerRef = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        aimTransform = transform.GetChild(1);
        spriteTransform = transform.GetChild(0);
        aimAnimator = aimTransform.GetComponent<Animator>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        aimGunEndPointTransform = aimTransform.GetChild(1);

        healthBar = transform.GetChild(2);
    }

    private void Start()
    {
        maxHealth = maxHealth + SpawnerController.Instance.extraHP;
        currentHealth = maxHealth;
    }

    void FixedUpdate()
    {
        Aim();
    }

    private void Aim()
    {

        if (playerRef != null)
        {
                Vector3 aimDir = (playerRef.position - transform.position).normalized;
                float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
                aimTransform.eulerAngles = new Vector3(0, 0, angle);

                Vector3 aimLocalScale = Vector3.one;
                if (angle > 90 || angle < -90)
                {
                    aimLocalScale.y = -1f;
                }
                else
                {
                    aimLocalScale.y = 1f;
                }
                aimTransform.localScale = aimLocalScale;
        }
/*        if (animator.GetFloat("Horizontal") == 1 || animator.GetFloat("Vertical") == -1 || animator.GetFloat("Speed") == 0)
        {
            spriteTransform.localScale = new Vector3(aimLocalScale.y, 1, 1);
        }
        else
        {
            spriteTransform.localScale = new Vector3(-aimLocalScale.y, 1, 1);
        }

 */
    }

    public void TakeDamage(Vector2 force, int damage)
    {
        rb.AddForce(force * _knockbackMult, ForceMode2D.Impulse);
        currentHealth -= damage;
        int random = UnityEngine.Random.Range(0, 4);
        AudioHelper.PlayClip2D(enemyHitSFX[random], 1);
        healthBar.localScale = new Vector3((((float)currentHealth / (float)maxHealth) * 0.315f), healthBar.localScale.y, healthBar.localScale.z);

        if (currentHealth <= 0)
        {
            Death();
        }
        else
        {
            _score = damage;
            _combo = ((float)damage / 10f) * 0.01f;
            GameObject scorePop = Instantiate(_scorePopup, transform.position, Quaternion.identity);
            scorePop.GetComponent<TMP_Text>().text = ((_score * GameScore.Instance.scoreMult).ToString());
            GameScore.Instance.AddScore(_score);
            ComboController.Instance.AddCombo(_combo);
        }
    }

    private void Death()
    {
        Instantiate(ps, transform.position, Quaternion.identity);
        int random = UnityEngine.Random.Range(0, 3);
        AudioHelper.PlayClip2D(enemyDeathSFX[random], 1);
        GameObject scorePop = Instantiate(_scorePopup, transform.position, Quaternion.identity);
        scorePop.GetComponent<TMP_Text>().text = ((_score * 5 * GameScore.Instance.scoreMult).ToString());

        GameScore.Instance.AddScore(initScore * 5);
        ComboController.Instance.AddCombo(initCombo * 5);

        Destroy(gameObject);
    }
}
