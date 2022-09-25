using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MaterialChange : MonoBehaviour
{

    SpriteRenderer _mat;
    TilemapRenderer _mat2;
    LineRenderer _mat3;
    GameObject _globalMat;

    void Awake()
    {
        if (GetComponent<SpriteRenderer>() != null)
        {
            _mat = GetComponent<SpriteRenderer>(); 
        }
        else if (GetComponent<TilemapRenderer>() != null)
        {
            _mat2 = GetComponent<TilemapRenderer>();
        }
        else if (GetComponent<LineRenderer>() != null)
        {
            _mat3 = GetComponent<LineRenderer>();
        }
    }

    void Update()
    {
        _globalMat = GameObject.Find("GlobalMaterial");
        if(_mat !=null)
        _mat.material = _globalMat.GetComponent<GlobalMaterial>()._gradient01;
        if(_mat2 !=null)
        _mat2.material = _globalMat.GetComponent<GlobalMaterial>()._gradient02;
        if(_mat3 != null)
        _mat3.material = _globalMat.GetComponent<GlobalMaterial>()._gradient01;
    }
}
