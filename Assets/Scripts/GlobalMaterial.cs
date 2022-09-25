using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GlobalMaterial : MonoBehaviour
{
    public static GlobalMaterial Instance { get; private set; }

    [SerializeField] public int posterizeAmount = 1;

    public Material _gradient01;
    public Material _gradient02;

    [SerializeField] public Color32[,] _gradientColors;
    [SerializeField] public Color32[,] _gradientColorsBG;

    float timeDuration = 1f;
    float duration = 0;

    int nextGradient = 0;
    int prevGradient = 0;

    bool isChanging= false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _gradient01 = Resources.Load<Material>("Materials/Gradient01");
        _gradient02 = Resources.Load<Material>("Materials/Gradient02");

        _gradientColors = new Color32[7, 4];
        _gradientColorsBG = new Color32[7, 4];

        _gradientColors[0, 0] = new Color32(0, 0, 0, 255);
        _gradientColors[0, 1] = new Color32(86, 86, 86, 255);
        _gradientColors[0, 2] = new Color32(172, 172, 172, 255);
        _gradientColors[0, 3] = new Color32(255, 255, 255, 255);

        _gradientColors[1, 0] = new Color32(18, 18, 53, 255);
        _gradientColors[1, 1] = new Color32(104, 104, 156, 255);
        _gradientColors[1, 2] = new Color32(230, 161, 207, 255);
        _gradientColors[1, 3] = new Color32(255, 230, 234, 255);

        _gradientColors[2, 0] = new Color32(13, 26, 26, 255);
        _gradientColors[2, 1] = new Color32(91, 140, 124, 255);
        _gradientColors[2, 2] = new Color32(173, 217, 188, 255);
        _gradientColors[2, 3] = new Color32(242, 255, 242, 255);

        _gradientColors[3, 0] = new Color32(33, 18, 16, 255);
        _gradientColors[3, 1] = new Color32(94, 45, 32, 255);
        _gradientColors[3, 2] = new Color32(199, 107, 42, 255);
        _gradientColors[3, 3] = new Color32(240, 194, 96, 255);

        _gradientColors[4, 0] = new Color32(0, 0, 106, 255);
        _gradientColors[4, 1] = new Color32(0, 0, 255, 255);
        _gradientColors[4, 2] = new Color32(172, 172, 172, 255);
        _gradientColors[4, 3] = new Color32(255, 255, 255, 255);

        _gradientColors[5, 0] = new Color32(0, 67, 6, 255);
        _gradientColors[5, 1] = new Color32(31, 202, 39, 255);
        _gradientColors[5, 2] = new Color32(62, 190, 170, 255);
        _gradientColors[5, 3] = new Color32(255, 255, 255, 255);

        _gradientColors[6, 0] = new Color32(19, 19, 0, 255);
        _gradientColors[6, 1] = new Color32(185, 185, 59, 255);
        _gradientColors[6, 2] = new Color32(255, 212, 0, 255);
        _gradientColors[6, 3] = new Color32(233, 241, 130, 255);



        _gradientColorsBG[0, 0] = new Color32(0, 0, 0, 255);
        _gradientColorsBG[0, 1] = new Color32(86, 86, 86, 255);
        _gradientColorsBG[0, 2] = new Color32(172, 172, 172, 255);
        _gradientColorsBG[0, 3] = new Color32(255, 255, 255, 255);

        _gradientColorsBG[1, 0] = new Color32(0, 0, 22, 255);
        _gradientColorsBG[1, 1] = new Color32(37, 0, 255, 255);
        _gradientColorsBG[1, 2] = new Color32(119, 226, 10, 255);
        _gradientColorsBG[1, 3] = new Color32(248, 255, 230, 255);

        _gradientColorsBG[2, 0] = new Color32(13, 13, 51, 255);
        _gradientColorsBG[2, 1] = new Color32(74, 76, 161, 255);
        _gradientColorsBG[2, 2] = new Color32(131, 132, 171, 255);
        _gradientColorsBG[2, 3] = new Color32(177, 177, 255, 255);

        _gradientColorsBG[3, 0] = new Color32(0, 3, 53, 255);
        _gradientColorsBG[3, 1] = new Color32(83, 89, 233, 255);
        _gradientColorsBG[3, 2] = new Color32(0, 199, 255, 255);
        _gradientColorsBG[3, 3] = new Color32(173, 129, 104, 255);

        _gradientColorsBG[4, 0] = new Color32(0, 0, 0, 255);
        _gradientColorsBG[4, 1] = new Color32(86, 86, 86, 255);
        _gradientColorsBG[4, 2] = new Color32(106, 106, 0, 255);
        _gradientColorsBG[4, 3] = new Color32(255, 255, 0, 255);

        _gradientColorsBG[5, 0] = new Color32(0, 0, 0, 255);
        _gradientColorsBG[5, 1] = new Color32(86, 86, 86, 255);
        _gradientColorsBG[5, 2] = new Color32(152, 34, 145, 255);
        _gradientColorsBG[5, 3] = new Color32(255, 0, 255, 255);

        _gradientColorsBG[6, 0] = new Color32(24, 0, 43, 255);
        _gradientColorsBG[6, 1] = new Color32(214, 9, 143, 255);
        _gradientColorsBG[6, 2] = new Color32(104, 32, 156, 255);
        _gradientColorsBG[6, 3] = new Color32(114, 150, 255, 255);

    }

    void Update()
    {

        if (duration > 0)
        {
            duration -= Time.deltaTime;
            SetGradient();
        }
        else
        {
            isChanging = false;
            SetGradient();
        }

/*        if (Input.GetKeyDown(KeyCode.G))
        {
            duration = timeDuration;
            isChanging = true;

            if (nextGradient == _gradientColors.GetLength(0) - 1)
            {
                nextGradient = 0;
            }
            else
            {
                nextGradient++;
            }
        }*/
    }

    public void Posterize(int tempAmt)
    {

        posterizeAmount = tempAmt;
        _gradient01.SetInt("Posterize_Amount", posterizeAmount);
        _gradient02.SetInt("Posterize_Amount", posterizeAmount);
    }

    private void OnApplicationQuit()
    {
        _gradient01.SetInt("Posterize_Amount", 1);
        _gradient02.SetInt("Posterize_Amount", 1);
    }

    private void SetGradient()
    {
        if (isChanging == true)
        {
            _gradient01.SetColor("Color01", Color.Lerp(_gradient01.GetColor("Color01"), _gradientColors[nextGradient, 0], 2.5f/timeDuration *  Time.deltaTime));
            _gradient01.SetColor("Color02", Color.Lerp(_gradient01.GetColor("Color02"), _gradientColors[nextGradient, 1], 2.5f/timeDuration * Time.deltaTime));
            _gradient01.SetColor("Color03", Color.Lerp(_gradient01.GetColor("Color03"), _gradientColors[nextGradient, 2], 2.5f/timeDuration * Time.deltaTime));
            _gradient01.SetColor("Color04", Color.Lerp(_gradient01.GetColor("Color04"), _gradientColors[nextGradient, 3], 2.5f/timeDuration * Time.deltaTime));

            _gradient02.SetColor("Color01", Color.Lerp(_gradient02.GetColor("Color01"), _gradientColorsBG[nextGradient, 0], 2.5f / timeDuration * Time.deltaTime));
            _gradient02.SetColor("Color02", Color.Lerp(_gradient02.GetColor("Color02"), _gradientColorsBG[nextGradient, 1], 2.5f / timeDuration * Time.deltaTime));
            _gradient02.SetColor("Color03", Color.Lerp(_gradient02.GetColor("Color03"), _gradientColorsBG[nextGradient, 2], 2.5f / timeDuration * Time.deltaTime));
            _gradient02.SetColor("Color04", Color.Lerp(_gradient02.GetColor("Color04"), _gradientColorsBG[nextGradient, 3], 2.5f / timeDuration * Time.deltaTime));
        }
        else
        {
            _gradient01.SetColor("Color01", _gradientColors[nextGradient, 0]);
            _gradient01.SetColor("Color02", _gradientColors[nextGradient, 1]);
            _gradient01.SetColor("Color03", _gradientColors[nextGradient, 2]);
            _gradient01.SetColor("Color04", _gradientColors[nextGradient, 3]);

            _gradient02.SetColor("Color01", _gradientColorsBG[nextGradient, 0]);
            _gradient02.SetColor("Color02", _gradientColorsBG[nextGradient, 1]);
            _gradient02.SetColor("Color03", _gradientColorsBG[nextGradient, 2]);
            _gradient02.SetColor("Color04", _gradientColorsBG[nextGradient, 3]);
        }

    }

    public void ColorChange()
    {
        duration = timeDuration;
        isChanging = true;
        int temp = UnityEngine.Random.Range(1, _gradientColors.GetLength(0));
        if (temp != prevGradient)
        {
            nextGradient = temp;
        }
        else
        {
            if (temp == 1)
            {
                nextGradient = temp + 1;
            }
            nextGradient = temp - 1;
        }
        prevGradient = temp;
    }

}
