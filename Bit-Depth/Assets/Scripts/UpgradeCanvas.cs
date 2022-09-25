using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCanvas : MonoBehaviour
{

    public static UpgradeCanvas Instance { get; private set; }

    [SerializeField] private Image leftUpgradeIMG;
    [SerializeField] private Image rightUpgradeIMG;
    [SerializeField] private Sprite[] images;
    [SerializeField] private Button leftUpgradeButton;
    [SerializeField] private Button rightUpgradeButton;
    [SerializeField] private GameObject crosshair;

    [SerializeField] AudioClip[] upgradeSFX;

    private Transform panel;

    private bool zoom = false;

    private PlayerGun playerGunRef;

    private int upgradeState = 0;

    private void Awake()
    {
        panel = transform.GetChild(0);

        playerGunRef = GameObject.Find("Player").GetComponent<PlayerGun>();

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        leftUpgradeIMG.sprite = images[0];
        rightUpgradeIMG.sprite = images[1];

        leftUpgradeButton.onClick.AddListener(LeftButton);
        rightUpgradeButton.onClick.AddListener(RightButton);
        leftUpgradeButton.onClick.AddListener(ResumeTime);
        rightUpgradeButton.onClick.AddListener(ResumeTime);

        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if(zoom == true && panel.localScale.x < 1)
        {
            panel.localScale += Vector3.one * 2 * Time.unscaledDeltaTime;
        }
        if (zoom == false && panel.localScale.x > 0)
        {
            panel.localScale -= Vector3.one * 2 * Time.unscaledDeltaTime;
        }
        else if(zoom == false && panel.localScale.x <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void ResumeTime()
    {
        int random = Random.Range(0, 1);
        AudioHelper.PlayClip2D(upgradeSFX[random], 1);
        leftUpgradeButton.enabled = false;
        rightUpgradeButton.enabled = false;
        zoom = false;
        GlobalMaterial.Instance.ColorChange();
        SpawnerController.Instance.ChoseUpgrade();
        Time.timeScale = 1;
        crosshair.SetActive(true);
        Cursor.visible = false;
        
    }

    private void LeftButton()
    {
        switch (upgradeState)
        {
            case 0:
                {
                    playerGunRef.ChangeUpgradeState(false,0.3f, PlayerGun.PlayerUpgrade.TwinShot);
                    leftUpgradeIMG.sprite = images[2];
                    rightUpgradeIMG.sprite = images[3];
                    upgradeState = 1;
                }
                break;
            case 1:
                {
                    playerGunRef.ChangeUpgradeState(false, 0.75f, PlayerGun.PlayerUpgrade.Rifle);
                    leftUpgradeIMG.sprite = images[6];
                    rightUpgradeIMG.sprite = images[7];
                    upgradeState = 3;
                }
                break;
            case 2:
                {
                    playerGunRef.ChangeUpgradeState(true, 0.5f, PlayerGun.PlayerUpgrade.HeavyAuto);
                    leftUpgradeIMG.sprite = images[10];
                    rightUpgradeIMG.sprite = images[11];
                    upgradeState = 5;
                }
                break;
            case 3:
                {
                    playerGunRef.ChangeUpgradeState(false, 0.35f, PlayerGun.PlayerUpgrade.SemiAuto);
                }
                break;
            case 4:
                {
                    playerGunRef.ChangeUpgradeState(false, 1f, PlayerGun.PlayerUpgrade.Shotgun);
                }
                break;
            case 5:
                {
                    playerGunRef.ChangeUpgradeState(true, 1.5f, PlayerGun.PlayerUpgrade.HeavyBurst);
                }
                break;
            case 6:
                {
                    playerGunRef.ChangeUpgradeState(true, 0.2f, PlayerGun.PlayerUpgrade.AutoTwin);
                }
                break;
            default:
                break;
        }
    }

    private void RightButton()
    {
        switch (upgradeState)
        {
            case 0:
                {
                    playerGunRef.ChangeUpgradeState(true, 0.3f, PlayerGun.PlayerUpgrade.Automatic);
                    leftUpgradeIMG.sprite = images[4];
                    rightUpgradeIMG.sprite = images[5];
                    upgradeState = 2;
                }
                break;
            case 1:
                {
                    playerGunRef.ChangeUpgradeState(false, 0.75f, PlayerGun.PlayerUpgrade.TriShot);
                    leftUpgradeIMG.sprite = images[8];
                    rightUpgradeIMG.sprite = images[9];
                    upgradeState = 4;
                }
                break;
            case 2:
                {
                    playerGunRef.ChangeUpgradeState(true, 0.15f, PlayerGun.PlayerUpgrade.LightAuto);
                    leftUpgradeIMG.sprite = images[12];
                    rightUpgradeIMG.sprite = images[13];
                    upgradeState = 6;
                }
                break;
            case 3:
                {
                    playerGunRef.ChangeUpgradeState(false, 1.5f, PlayerGun.PlayerUpgrade.Sniper);
                }
                break;
            case 4:
                {
                    playerGunRef.ChangeUpgradeState(false, 1.0f, PlayerGun.PlayerUpgrade.QuadShot);
                }
                break;
            case 5:
                {
                    playerGunRef.ChangeUpgradeState(true, 0.5f, PlayerGun.PlayerUpgrade.HeavyAngle);
                }
                break;
            case 6:
                {
                    playerGunRef.ChangeUpgradeState(true, 0.04f, PlayerGun.PlayerUpgrade.Uncontrollable);
                }
                break;
            default:
                break;
        }
    }

    private void OnEnable()
    {
        leftUpgradeButton.enabled = true;
        rightUpgradeButton.enabled = true;
        panel.localScale = Vector3.zero;
        zoom = true;
    }

}
