using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullets : MonoBehaviour
{

    [SerializeField] private float _shotSpeed = 5;
    [SerializeField] private int _damage = 25;
    [SerializeField] private float _knockback = 10;
    [SerializeField] private float _size = 1;
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

    public void BulletSetup(Vector3 shootDir, float shotSpeed, int damage, float knockback, float size)
    {
        _shotSpeed = shotSpeed;
        _damage = damage;
        _knockback = knockback;
        _size = size;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        transform.localScale = new Vector3(_size, _size, _size);  
        float vel = _shotSpeed;
        rb.AddForce(shootDir * vel, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null && gameObject.name == "NormalBullet(Clone)")
        {
            Instantiate(ps, transform.position, Quaternion.identity);
            collision.GetComponent<Enemy>().TakeDamage(_rb.velocity.normalized * _knockback, _damage);
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Instantiate(ps, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}
