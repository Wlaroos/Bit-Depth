using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerGun : MonoBehaviour
{

    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {

        public Vector3 gunEndPointPosition;
        public Vector3 shootPosition;

    }

    [SerializeField] GameObject bulletRef;

    [SerializeField] AudioClip[] playerShootSFX;

    private Transform aimTransform;
    private Transform spriteTransform;
    private Transform aimGunEndPointTransform;
    private Transform aimGunEndPointTransform1;
    private Transform aimGunEndPointTransform2;
    private Transform aimGunEndPointTransform3;
    private Transform aimGunEndPointTransform4;
    private Transform reloadBarTransform;

    private Vector3 mousePos;
    private Vector3 bulletAxis = Vector3.forward;

    private Animator aimAnimator;
    private Animator animator;

    [SerializeField] private bool  _isAuto;
    [SerializeField] private float _fireDelay;
    private float _startFireTime;
    private float fireTimer;

    public enum PlayerUpgrade
    {None, TwinShot, Rifle, TriShot, SemiAuto, Sniper, Shotgun, QuadShot, Automatic, HeavyAuto, LightAuto, HeavyBurst, HeavyAngle, AutoTwin, Uncontrollable }

    public PlayerUpgrade UpgradeState = PlayerUpgrade.None;

    void Awake()
    {
        spriteTransform = transform.GetChild(0);
        aimTransform = transform.GetChild(1);
        reloadBarTransform = transform.GetChild(2);
        aimAnimator = aimTransform.GetComponent<Animator>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        aimGunEndPointTransform = aimTransform.Find("GunEndPointPosition");
        aimGunEndPointTransform1 = aimTransform.Find("GunEndPointPosition1");
        aimGunEndPointTransform2 = aimTransform.Find("GunEndPointPosition2");
        aimGunEndPointTransform3 = aimTransform.Find("GunEndPointPosition3");
        aimGunEndPointTransform4 = aimTransform.Find("GunEndPointPosition4");
    }

    private void Start()
    {
        this.OnShoot += PlayerRef_OnShoot;
    }

    void Update()
    {
        if (Time.timeScale == 1)
        {
            Aim();
            Shoot();
            if (fireTimer < _fireDelay)
            {
                fireTimer += Time.deltaTime;
                reloadBarTransform.localScale = new Vector3((fireTimer / _fireDelay) * 0.315f, reloadBarTransform.localScale.y, reloadBarTransform.localScale.z);
            }
            else
            {
                fireTimer = _fireDelay;
                reloadBarTransform.localScale = new Vector3((fireTimer / _fireDelay) * 0.315f, reloadBarTransform.localScale.y, reloadBarTransform.localScale.z);
            }
        }
    }

    private void Aim()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Vector3 aimDir = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 aimLocalScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            bulletAxis = Vector3.back;
            aimLocalScale.y = -1f;
        }
        else
        {
            bulletAxis = Vector3.forward;
            aimLocalScale.y = 1f;
        }
        aimTransform.localScale = aimLocalScale;
        if(animator.GetFloat("Horizontal") == 1 || animator.GetFloat("Vertical") == -1 || animator.GetFloat("Speed") == 0)
        {
            spriteTransform.localScale = new Vector3(aimLocalScale.y, 1, 1);
        }
        else
        {
            spriteTransform.localScale = new Vector3(-aimLocalScale.y, 1, 1);
        }
    }

    private void Shoot()
    {
        if (_isAuto)
        {
            if (Input.GetMouseButton(0) && Time.time > _fireDelay + _startFireTime)
            {
                //aimAnimator.SetTrigger("Shoot");
                OnShoot?.Invoke(this, new OnShootEventArgs { gunEndPointPosition = aimGunEndPointTransform.position, shootPosition = mousePos, });
                int random = UnityEngine.Random.Range(0, 3);
                AudioHelper.PlayClip2D(playerShootSFX[random], 0.35f);
                _startFireTime = Time.time;
                fireTimer = 0;
            }
        }
        else if (!_isAuto)
        {
            if (Input.GetMouseButtonDown(0) && Time.time > _startFireTime + _fireDelay)
            {
                //aimAnimator.SetTrigger("Shoot");

                OnShoot?.Invoke(this, new OnShootEventArgs { gunEndPointPosition = aimGunEndPointTransform.position, shootPosition = mousePos, });
                int random = UnityEngine.Random.Range(0, 3);
                AudioHelper.PlayClip2D(playerShootSFX[random], .7f);
                _startFireTime = Time.time;
                fireTimer = 0;
            }
        }
    }

    private void PlayerRef_OnShoot(object sender, PlayerGun.OnShootEventArgs e)
    {
        switch (UpgradeState)
        {
            case PlayerUpgrade.None:
                {
                    Camera.main.GetComponent<CameraShake>().Shake(0.05f, 0.075f);
                    Transform bulletTransform = Instantiate(bulletRef.transform, e.gunEndPointPosition, Quaternion.identity);
                    Vector3 shootDir = (e.shootPosition - transform.position).normalized;
                    bulletTransform.GetComponent<PlayerBullets>().BulletSetup(shootDir, 5, 25, 10, 1);
                }
                break;
            case PlayerUpgrade.TwinShot:
                {
                    Camera.main.GetComponent<CameraShake>().Shake(0.05f, 0.075f);
                    Transform bulletTransform = Instantiate(bulletRef.transform, aimGunEndPointTransform2.position, Quaternion.identity);
                    Vector3 shootDir = (e.shootPosition - transform.position).normalized;
                    bulletTransform.GetComponent<PlayerBullets>().BulletSetup(shootDir, 5, 20, 5, 0.85f);

                    Transform bulletTransform2 = Instantiate(bulletRef.transform, aimGunEndPointTransform3.position, Quaternion.identity);
                    Vector3 shootDir2 = (e.shootPosition - transform.position).normalized;
                    bulletTransform2.GetComponent<PlayerBullets>().BulletSetup(shootDir2, 5, 20, 5, 0.85f);
                }
                break;
            case PlayerUpgrade.Rifle:
                {
                    Camera.main.GetComponent<CameraShake>().Shake(0.075f, 0.1f);
                    Transform bulletTransform = Instantiate(bulletRef.transform, e.gunEndPointPosition, Quaternion.identity);
                    Vector3 shootDir = (e.shootPosition - transform.position).normalized;
                    bulletTransform.GetComponent<PlayerBullets>().BulletSetup(shootDir, 7.5f, 75, 15, 1.1f);
                }
                break;
            case PlayerUpgrade.TriShot:
                {
                    Camera.main.GetComponent<CameraShake>().Shake(0.05f, 0.075f);
                    Transform bulletTransform = Instantiate(bulletRef.transform, aimGunEndPointTransform2.position, Quaternion.identity);
                    Vector3 shootDir = (e.shootPosition - transform.position).normalized;
                    shootDir = Quaternion.AngleAxis(5, bulletAxis) * shootDir;
                    bulletTransform.GetComponent<PlayerBullets>().BulletSetup(shootDir, 6, 35, 5, 0.9f);

                    Transform bulletTransform2 = Instantiate(bulletRef.transform, aimGunEndPointTransform3.position, Quaternion.identity);
                    Vector3 shootDir2 = (e.shootPosition - transform.position).normalized;
                    shootDir2 = Quaternion.AngleAxis(-5, bulletAxis) * shootDir2;
                    bulletTransform2.GetComponent<PlayerBullets>().BulletSetup(shootDir2, 6, 40, 5, 0.9f);  

                    Transform bulletTransform3 = Instantiate(bulletRef.transform, e.gunEndPointPosition, Quaternion.identity);
                    Vector3 shootDir3 = (e.shootPosition - transform.position).normalized;
                    bulletTransform3.GetComponent<PlayerBullets>().BulletSetup(shootDir3, 6, 35, 5, 0.9f);
                }
                break;
            case PlayerUpgrade.SemiAuto:
                {
                    Camera.main.GetComponent<CameraShake>().Shake(0.05f, 0.075f);
                    Transform bulletTransform = Instantiate(bulletRef.transform, e.gunEndPointPosition, Quaternion.identity);
                    Vector3 shootDir = (e.shootPosition - transform.position).normalized;
                    bulletTransform.GetComponent<PlayerBullets>().BulletSetup(shootDir, 7.5f, 70, 10, 1.25f);
                }
                break;
            case PlayerUpgrade.Sniper:
                {
                    Camera.main.GetComponent<CameraShake>().Shake(0.075f, 0.1f);
                    Transform bulletTransform = Instantiate(bulletRef.transform, e.gunEndPointPosition, Quaternion.identity);
                    Vector3 shootDir = (e.shootPosition - transform.position).normalized;
                    bulletTransform.GetComponent<PlayerBullets>().BulletSetup(shootDir, 10.0f, 175, 10, 1.25f);
                }
                break;
            case PlayerUpgrade.Shotgun:
                {
                    Camera.main.GetComponent<CameraShake>().Shake(0.05f, 0.075f);
                    Transform bulletTransform = Instantiate(bulletRef.transform, aimGunEndPointTransform1.position, Quaternion.identity);
                    Vector3 shootDir = (e.shootPosition - transform.position).normalized;
                    shootDir = Quaternion.AngleAxis(UnityEngine.Random.Range(5.0f, 10.0f), bulletAxis) * shootDir;
                    bulletTransform.GetComponent<PlayerBullets>().BulletSetup(shootDir, UnityEngine.Random.Range(6.0f,8.0f), 25, 5, UnityEngine.Random.Range(.6f, .8f));

                    Transform bulletTransform2 = Instantiate(bulletRef.transform, aimGunEndPointTransform2.position, Quaternion.identity);
                    Vector3 shootDir2 = (e.shootPosition - transform.position).normalized;
                    shootDir2 = Quaternion.AngleAxis(UnityEngine.Random.Range(0.0f, 5.0f), bulletAxis) * shootDir2;
                    bulletTransform2.GetComponent<PlayerBullets>().BulletSetup(shootDir2, UnityEngine.Random.Range(6.0f, 8.0f), 25, 5, UnityEngine.Random.Range(.6f, .8f));

                    Transform bulletTransform3 = Instantiate(bulletRef.transform, aimGunEndPointTransform3.position, Quaternion.identity);
                    Vector3 shootDir3 = (e.shootPosition - transform.position).normalized;
                    shootDir3 = Quaternion.AngleAxis(-UnityEngine.Random.Range(0.0f, 5.0f), bulletAxis) * shootDir3;
                    bulletTransform3.GetComponent<PlayerBullets>().BulletSetup(shootDir3, UnityEngine.Random.Range(6.0f, 8.0f), 25, 5, UnityEngine.Random.Range(.6f, .8f));

                    Transform bulletTransform4 = Instantiate(bulletRef.transform, aimGunEndPointTransform4.position, Quaternion.identity);
                    Vector3 shootDir4 = (e.shootPosition - transform.position).normalized;
                    shootDir4 = Quaternion.AngleAxis(-UnityEngine.Random.Range(5.0f, 10.0f), bulletAxis) * shootDir4;
                    bulletTransform4.GetComponent<PlayerBullets>().BulletSetup(shootDir4, UnityEngine.Random.Range(6.0f, 8.0f), 25, 5, UnityEngine.Random.Range(.6f, .8f));

                    Transform bulletTransform5 = Instantiate(bulletRef.transform, aimGunEndPointTransform1.position, Quaternion.identity);
                    Vector3 shootDir5 = (e.shootPosition - transform.position).normalized;
                    shootDir5 = Quaternion.AngleAxis(UnityEngine.Random.Range(5.0f, 10.0f), bulletAxis) * shootDir5;
                    bulletTransform5.GetComponent<PlayerBullets>().BulletSetup(shootDir5, UnityEngine.Random.Range(6.0f, 8.0f), 25, 5, UnityEngine.Random.Range(.6f, .8f));

                    Transform bulletTransform6 = Instantiate(bulletRef.transform, aimGunEndPointTransform2.position, Quaternion.identity);
                    Vector3 shootDir6 = (e.shootPosition - transform.position).normalized;
                    shootDir6 = Quaternion.AngleAxis(UnityEngine.Random.Range(0.0f, 5.0f), bulletAxis) * shootDir6;
                    bulletTransform6.GetComponent<PlayerBullets>().BulletSetup(shootDir6, UnityEngine.Random.Range(6.0f, 8.0f), 25, 5, UnityEngine.Random.Range(.6f, .8f));

                    Transform bulletTransform7 = Instantiate(bulletRef.transform, aimGunEndPointTransform3.position, Quaternion.identity);
                    Vector3 shootDir7 = (e.shootPosition - transform.position).normalized;
                    shootDir7 = Quaternion.AngleAxis(-UnityEngine.Random.Range(0.0f, 5.0f), bulletAxis) * shootDir7;
                    bulletTransform7.GetComponent<PlayerBullets>().BulletSetup(shootDir7, UnityEngine.Random.Range(6.0f, 8.0f), 25, 5, UnityEngine.Random.Range(.6f, .8f));

                    Transform bulletTransform8 = Instantiate(bulletRef.transform, aimGunEndPointTransform4.position, Quaternion.identity);
                    Vector3 shootDir8 = (e.shootPosition - transform.position).normalized;
                    shootDir8 = Quaternion.AngleAxis(-UnityEngine.Random.Range(5.0f, 10.0f), bulletAxis) * shootDir8;
                    bulletTransform8.GetComponent<PlayerBullets>().BulletSetup(shootDir8, UnityEngine.Random.Range(6.0f, 8.0f), 25, 5, UnityEngine.Random.Range(.6f, .8f));
                }
                break;
            case PlayerUpgrade.QuadShot:
                {
                    Camera.main.GetComponent<CameraShake>().Shake(0.05f, 0.075f);
                    Transform bulletTransform = Instantiate(bulletRef.transform, aimGunEndPointTransform1.position, Quaternion.identity);
                    Vector3 shootDir = (e.shootPosition - transform.position).normalized;
                    shootDir = Quaternion.AngleAxis(5f, bulletAxis) * shootDir;
                    bulletTransform.GetComponent<PlayerBullets>().BulletSetup(shootDir, 7, 35, 5, .9f);

                    Transform bulletTransform2 = Instantiate(bulletRef.transform, aimGunEndPointTransform2.position, Quaternion.identity);
                    Vector3 shootDir2 = (e.shootPosition - transform.position).normalized;
                    shootDir2 = Quaternion.AngleAxis(2f, bulletAxis) * shootDir2;
                    bulletTransform2.GetComponent<PlayerBullets>().BulletSetup(shootDir2, 7, 40, 5, .9f);

                    Transform bulletTransform3 = Instantiate(bulletRef.transform, aimGunEndPointTransform3.position, Quaternion.identity);
                    Vector3 shootDir3 = (e.shootPosition - transform.position).normalized;
                    shootDir3 = Quaternion.AngleAxis(-2f, bulletAxis) * shootDir3;
                    bulletTransform3.GetComponent<PlayerBullets>().BulletSetup(shootDir3, 7, 40, 5, .9f);

                    Transform bulletTransform4 = Instantiate(bulletRef.transform, aimGunEndPointTransform4.position, Quaternion.identity);
                    Vector3 shootDir4 = (e.shootPosition - transform.position).normalized;
                    shootDir4 = Quaternion.AngleAxis(-5f, bulletAxis) * shootDir4;
                    bulletTransform4.GetComponent<PlayerBullets>().BulletSetup(shootDir4, 7, 35, 5, .9f);
                }
                break;
            case PlayerUpgrade.Automatic:
                {
                    Camera.main.GetComponent<CameraShake>().Shake(0.025f, 0.05f);
                    Transform bulletTransform = Instantiate(bulletRef.transform, e.gunEndPointPosition, Quaternion.identity);
                    Vector3 shootDir = (e.shootPosition - transform.position).normalized;
                    bulletTransform.GetComponent<PlayerBullets>().BulletSetup(shootDir, 5.5f, 30, 12.5f, 1);
                }
                break;
            case PlayerUpgrade.HeavyAuto:
                {
                    Camera.main.GetComponent<CameraShake>().Shake(0.05f, 0.075f);
                    Transform bulletTransform = Instantiate(bulletRef.transform, e.gunEndPointPosition, Quaternion.identity);
                    Vector3 shootDir = (e.shootPosition - transform.position).normalized;
                    bulletTransform.GetComponent<PlayerBullets>().BulletSetup(shootDir, 3.0f, 60, 15, 1.25f);
                }
                break;
            case PlayerUpgrade.LightAuto:
                {
                    Camera.main.GetComponent<CameraShake>().Shake(0.05f, 0.0125f);
                    Transform bulletTransform = Instantiate(bulletRef.transform, e.gunEndPointPosition, Quaternion.identity);
                    Vector3 shootDir = (e.shootPosition - transform.position).normalized;
                    bulletTransform.GetComponent<PlayerBullets>().BulletSetup(shootDir, 7.5f, 25, 5, .8f);
                }
                break;
            case PlayerUpgrade.HeavyBurst:
                {
                    Invoke("HeavyBurst", 0f);
                    Invoke("HeavyBurst", 0.15f);
                    Invoke("HeavyBurst", 0.3f);
                    Invoke("HeavyBurst", 0.45f);
                }
                break;
            case PlayerUpgrade.HeavyAngle:
                {
                    Camera.main.GetComponent<CameraShake>().Shake(0.05f, 0.075f);
                    Transform bulletTransform = Instantiate(bulletRef.transform, e.gunEndPointPosition, Quaternion.identity);
                    Vector3 shootDir = (e.shootPosition - transform.position).normalized;
                    shootDir = Quaternion.AngleAxis(-15f, bulletAxis) * shootDir;
                    bulletTransform.GetComponent<PlayerBullets>().BulletSetup(shootDir, 4.0f, 75, 15, 1.25f);

                    Transform bulletTransform2 = Instantiate(bulletRef.transform, e.gunEndPointPosition, Quaternion.identity);
                    Vector3 shootDir2 = (e.shootPosition - transform.position).normalized;
                    shootDir2 = Quaternion.AngleAxis(15f, bulletAxis) * shootDir;
                    bulletTransform2.GetComponent<PlayerBullets>().BulletSetup(shootDir2, 4.0f, 75, 15, 1.25f);
                }
                break;
            case PlayerUpgrade.AutoTwin:
                {
                    Camera.main.GetComponent<CameraShake>().Shake(0.025f, 0.05f);
                    Transform bulletTransform = Instantiate(bulletRef.transform, aimGunEndPointTransform2.position, Quaternion.identity);
                    Vector3 shootDir = (e.shootPosition - transform.position).normalized;
                    bulletTransform.GetComponent<PlayerBullets>().BulletSetup(shootDir, 6, 30, 7.5f, 0.9f);

                    Transform bulletTransform2 = Instantiate(bulletRef.transform, aimGunEndPointTransform3.position, Quaternion.identity);
                    Vector3 shootDir2 = (e.shootPosition - transform.position).normalized;
                    bulletTransform2.GetComponent<PlayerBullets>().BulletSetup(shootDir2, 6, 30, 7.5f, 0.9f);
                }
                break;
            case PlayerUpgrade.Uncontrollable:
                {
                    Camera.main.GetComponent<CameraShake>().Shake(0.05f, 0.0125f);
                    Transform bulletTransform = Instantiate(bulletRef.transform, e.gunEndPointPosition, Quaternion.identity);
                    Vector3 shootDir = (e.shootPosition - transform.position).normalized;
                    shootDir = Quaternion.AngleAxis(UnityEngine.Random.Range(-20.0f, 20.0f), bulletAxis) * shootDir;
                    bulletTransform.GetComponent<PlayerBullets>().BulletSetup(shootDir, 5, 12, 0.5f, 0.5f);
                }
                break;
            default:
                break;
        }
    }

    public void ChangeUpgradeState(bool isAuto, float fireDelay, PlayerUpgrade upgrade)
    {
        _isAuto = isAuto;
        _fireDelay = fireDelay;
        UpgradeState = upgrade;
    }

    private void HeavyBurst()
    {
        Camera.main.GetComponent<CameraShake>().Shake(0.05f, 0.075f);
        Transform bulletTransform = Instantiate(bulletRef.transform, aimGunEndPointTransform.position, Quaternion.identity);
        Vector3 shootDir = (mousePos - transform.position).normalized;
        bulletTransform.GetComponent<PlayerBullets>().BulletSetup(shootDir, 4.0f, 60, 15, 1.25f);
    }

}
