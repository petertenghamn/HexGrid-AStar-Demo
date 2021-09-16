using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private Canvas canvas;
    private GameController controller;

    private void Start()
    {
        canvas = transform.GetComponent<Canvas>();
        controller = transform.GetComponentInParent<GameController>();
    }

    public bool OverCheck()
    {
        bool overUI = false;
        var mousePos = Input.mousePosition;

        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();
            Vector2 localMousePos = child.InverseTransformPoint(mousePos);
            if (child.rect.Contains(localMousePos))
            {
                overUI = true;
            }
        }
        return overUI;
    }
    
    public void PlayTurn()
    {
        controller.RequestPlayTurn();
    }
}
