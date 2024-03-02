using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField] private Transform movePoint, minPosition, maxPosition;
    [SerializeField] private FishingLine fishingLine;
    private float mouseYPos;

    private void Start()
    {
        Transform[] points = new Transform[2] { minPosition, movePoint };
        fishingLine.InitializeLine(points);
    }

    private void Update()
    {
        mouseYPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        ControlLine();
    }

    private void ControlLine()
    {
        //mouse is above bounds so bring line to top
        if (MouseAboveBounds())
        {
            movePoint.position = minPosition.position;
        }
        //mouse is below bounds so expand line to max length
        else if (MouseBelowBounds())
        {
            movePoint.position = maxPosition.position;
        }
        //mouse is within bounds so move line to mouse position
        else
        {
            movePoint.position = new Vector3(movePoint.position.x, mouseYPos, movePoint.position.z);
        }
    }

    //mouse position is above the bounds
    private bool MouseAboveBounds()
    {
        float mouseHeight = mouseYPos;
        return mouseHeight > minPosition.position.y;
    }

    //mouse position is below the bounds
    private bool MouseBelowBounds()
    {
        float mouseHeight = mouseYPos;
        return mouseHeight < maxPosition.position.y;
    }
}
