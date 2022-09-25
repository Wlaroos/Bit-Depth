using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosterizeTimer : MonoBehaviour
{

    private Material _gradient01;
    private int posterizeAmount = 16;

    private void Awake()
    {
        _gradient01 = Resources.Load<Material>("Materials/TitleGradient");
    }

    void Start()
    {
        InvokeRepeating("Posterize", 1.0f, 1.0f);
    }

    private void Posterize()
    {
        if (posterizeAmount != 1)
        {
            posterizeAmount = posterizeAmount / 2;
            _gradient01.SetInt("Posterize_Amount", posterizeAmount);
        }
        else
        {
            posterizeAmount = 16;
            _gradient01.SetInt("Posterize_Amount", posterizeAmount);
        }

    }

    private void OnApplicationQuit()
    {
        _gradient01.SetInt("Posterize_Amount", 16);
    }

}
