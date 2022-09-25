using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{

    ParticleSystem ps;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        Destroy(gameObject,0.5f);
        var main = ps.main;
        if (main.duration > 0.5)
        {
            main.startColor = new ParticleSystem.MinMaxGradient(GlobalMaterial.Instance._gradient01.GetColor("Color01"), GlobalMaterial.Instance._gradient01.GetColor("Color04"));
        }
        else
        {
            main.startColor = new ParticleSystem.MinMaxGradient(GlobalMaterial.Instance._gradient01.GetColor("Color03"), GlobalMaterial.Instance._gradient01.GetColor("Color04"));
        }
    }
}
