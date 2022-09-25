using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullets : MonoBehaviour
{

    [SerializeField] private float _shotSpeed = 5;
    [SerializeField] private float _knockback = 10;
    [SerializeField] private int _animState = 0;
    [SerializeField] ParticleSystem ps;
    Rigidbody2D _rb;

    private void Awake()
    {
        Animator anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        anim.SetInteger("State", _animState);
    }

    private void Start()
    {
        Destroy(gameObject, 8.0f);
    }

    public void BulletSetup(Vector3 shootDir)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float vel = _shotSpeed;
        rb.AddForce(shootDir * vel, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Instantiate(ps, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (collision.GetComponent<PlayerMovement>() != null && gameObject.name != ("NormalBullet(Clone)"))
        {

            Instantiate(ps, transform.position, Quaternion.identity);
            if(gameObject.name == ("SkullBullet(Clone)"))
            {
                collision.GetComponent<PlayerHealth>().TakeDamage(2);
            }
            else
            {
                collision.GetComponent<PlayerHealth>().TakeDamage(1);
            }
            if (collision.GetComponent<PlayerHealth>().invincible == false)
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(_rb.velocity.normalized * 4, ForceMode2D.Impulse);
            }
            Destroy(gameObject);
        }
    }
}
