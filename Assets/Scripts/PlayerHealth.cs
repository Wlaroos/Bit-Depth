using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField] private SpriteRenderer healthBarRef;
    [SerializeField] private Sprite[] sprite;
    [SerializeField] ParticleSystem ps;
    [SerializeField] GameObject namePick;
    [SerializeField] GameObject crosshair;

    [SerializeField] AudioClip[] playerHitSFX;
    [SerializeField] AudioClip[] playerDeathSFX;
    [SerializeField] AudioClip[] gameOverSFX;

    private SpriteRenderer playerSprite;

    private float iTimeStart = 1.5f;
    private float iTime;
    public bool invincible;
    private bool back = false;

    private int maxHealth = 3;
    private int currentHealth = 3;

    private void Awake()
    {
        playerSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (iTime > 0)
        {
            iTime -= Time.deltaTime;
            Flash();
        }
        else
        {
            invincible = false;
            playerSprite.color = Color.white;
        }

 /*       if (Input.GetKeyDown(KeyCode.Backspace))
        {
            TakeDamage(1);
        }*/
    }

    public void TakeDamage(int damage)
    {
        if (invincible == false)
        {
            int random = Random.Range(0,2);
            AudioHelper.PlayClip2D(playerHitSFX[random],1);

            currentHealth -= damage;

            // Particles

            // coolio effect 1 health

            iTime = iTimeStart;
            invincible = true;

            healthBarRef.sprite = sprite[3 - currentHealth];

            Camera.main.GetComponent<CameraShake>().Shake(0.15f, 0.15f);

            ComboController.Instance.ResetCombo();

            if (currentHealth <= 0)
            {
                Death();
            }

        }

    }

    public void Heal()
    {
        if (currentHealth != maxHealth)
        {
            currentHealth = maxHealth;
        }
        healthBarRef.sprite = sprite[3 - currentHealth];
    }

    private void Death()
    {
        AudioHelper.PlayClip2D(playerDeathSFX[0], 1);
        AudioHelper.PlayClip2D(playerDeathSFX[1], 1);
        AudioHelper.PlayClip2D(gameOverSFX[1], 1f);
        Instantiate(ps, transform.position, Quaternion.identity);
        namePick.SetActive(true);
        crosshair.SetActive(false);
        Cursor.visible = true;
        Destroy(gameObject);
    }

    private void Flash()
    {
        if (invincible == true)
        {
            if (back == false)
            {
                playerSprite.color = Color.Lerp(playerSprite.color, new Color(1, 0, 0, 0), 0.05f);
            }
            else
            {
                playerSprite.color = Color.Lerp(playerSprite.color, new Color(1, 1, 1, 1), 0.05f);
            }

            if (playerSprite.color.a < 0.2)
            {
                back = true;
            }
            else if (playerSprite.color.a >= 0.8)
            {
                back = false;
            }

        }
    }
}
