using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairFollowMouse : MonoBehaviour
{
    void Awake()
    {
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;
    }
}
