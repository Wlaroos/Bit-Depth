using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComboController : MonoBehaviour
{

    public static ComboController Instance { get; private set; }

    private int comboLevel = 1;
    private float comboAmount = 0;

    [SerializeField] private float maxTimer = 0.2f;
    private float timer = 0.2f;

    [SerializeField] private float comboTimeBuffer = 2f;
    private float comboTimer;

    private bool safe = false;

    private RectTransform sliderRef;

    [SerializeField] TMP_Text level;
    [SerializeField] TMP_Text amount;

    [SerializeField] PanelPulse bitPulse;

    [SerializeField] AudioClip[] comboSFX;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        sliderRef = transform.GetChild(0).GetComponent<RectTransform>();
        timer = maxTimer;
        sliderRef.sizeDelta = new Vector2(comboAmount * 517, sliderRef.sizeDelta.y);
    }

    void Update()
    {
/*        if(Input.GetKeyDown(KeyCode.O))
        {
            AddCombo(0.5f);
        }    */

        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
        }
        else
        {
            safe = false;
        }

        amount.SetText("[ " + comboAmount.ToString("F2") + " ]");
        level.SetText("Bits x " + (int)(Mathf.Pow(2f, (comboLevel - 1.0f))));

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            TickDown();
            timer = maxTimer;
        }
    }

    private void TickDown()
    {
        if (comboAmount > 0)
        {
            comboAmount -= 0.01f;
            sliderRef.sizeDelta = new Vector2(comboAmount * 517, sliderRef.sizeDelta.y);
        }

        if (comboAmount <= 0 && comboLevel != 1 && safe == false)
        {
            ComboLevelDown();
        }
    }

    public void AddCombo(float addAmount)
    {

        comboAmount += (addAmount * 1.12f);
        if(comboAmount >= 1.0f && comboLevel <= 4)
        {
            AudioHelper.PlayClip2D(comboSFX[0], 1);
            ComboLevelUp();
        }

        if (comboLevel > 4)
        {
            comboLevel = 5;
            comboAmount = 1;
            sliderRef.sizeDelta = new Vector2(comboAmount * 517, sliderRef.sizeDelta.y);
        }

    }

    private void ComboLevelUp()
    {
        comboLevel += (int)(comboAmount / 1.0f);
        comboAmount %= 1.0f;
        sliderRef.sizeDelta = new Vector2(comboAmount * 517, sliderRef.sizeDelta.y);
        GameScore.Instance.ChangeMult(comboLevel);

        safe = true;
        comboTimer = comboTimeBuffer;

        GlobalMaterial.Instance.Posterize((int)(Mathf.Pow(2f, (comboLevel - 1.0f))));

        // Visuals
    }

    private void ComboLevelDown()
    {
        AudioHelper.PlayClip2D(comboSFX[1], 1);
        comboAmount = 1.0f;
        comboLevel -= 1;
        sliderRef.sizeDelta = new Vector2(comboAmount * 517, sliderRef.sizeDelta.y);
        GameScore.Instance.ChangeMult(comboLevel);

        GlobalMaterial.Instance.Posterize((int)(Mathf.Pow(2f , (comboLevel - 1.0f))));

        // Visuals
    }

    public void ResetCombo()
    {
        comboLevel = 1;
        comboAmount = 0;
        sliderRef.sizeDelta = new Vector2(comboAmount * 517, sliderRef.sizeDelta.y);

        GlobalMaterial.Instance.Posterize((int)(Mathf.Pow(2f, (comboLevel - 1.0f))));

        GameScore.Instance.ChangeMult(comboLevel);
    }

}
