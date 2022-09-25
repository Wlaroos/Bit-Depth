using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{

    private TMP_Text text;

    private void Awake()
    {
        Destroy(gameObject, .5f);
        text = GetComponent<TMP_Text>();
        transform.position = new Vector3(Random.Range(transform.position.x - 0.75f, transform.position.x + 0.75f), Random.Range(transform.position.y + 0.35f, transform.position.y + 0.75f), transform.position.z);
        text.color = GlobalMaterial.Instance._gradient01.GetColor("Color04");
        text.outlineWidth = 0.15f;
        text.outlineColor = GlobalMaterial.Instance._gradient01.GetColor("Color01");
    }

    private void Start()
    {
        text.SetText("+" + text.text);
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.005f, transform.position.z);
        transform.localScale -= new Vector3(0.0025f, 0.0025f, 0.0025f);
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - 0.005f);
    }
}
