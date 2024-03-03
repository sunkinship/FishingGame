using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingController : MonoBehaviour
{
    public const float CATCH_THRESHOLD = 2.4f;

    [SerializeField] private Transform movePoint, minPosition, maxPosition;
    [SerializeField] private FishingLine fishingLine;
   
    private float mouseYPos;

    [SerializeField] private float slowReelSpeed;

    private Coroutine co_movingPoint = null;
    private bool movingPoint => co_movingPoint != null;

    private void Start()
    {
        Transform[] points = new Transform[2] { minPosition, movePoint };
        fishingLine.InitializeLine(points);
    }

    private void Update()
    {
        //update mouse position
        mouseYPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;

        //cannot move fishing line if zapped
        if (GameManager.Instance.IsZapped)
            return;

        //move fishing line
        if (GameManager.Instance.HookedRockFish == false)
            ControlLine();
        else
            SlowedControl();

        //check for mouse input
        MouseButtonPressed();
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

    private void MouseButtonPressed()
    {
        //catch
        if (Input.GetMouseButtonDown(0))
        {
            //if not fish is caught, cannot catch or release
            if (GameManager.Instance.IsFishCaught == false)
                return;

            //if lure is above threshold, catch fish
            if (movePoint.position.y >= CATCH_THRESHOLD)
                GameManager.Instance.OnCatchConfirm();
        }
        //release
        if (Input.GetMouseButtonDown(1))
        {
            //if not fish is caught, cannot catch or release
            if (GameManager.Instance.IsFishCaught == false)
                return;

            //if lure is below threshold, release fish
            if (movePoint.position.y < CATCH_THRESHOLD)
                GameManager.Instance.OnFishRelease();
        }
    }

    #region SLOW CONTROL
    //slows down the speed of the line for when a rock fish is caught
    private void SlowedControl()
    {
        //mouse is above bounds so bring line to top
        if (MouseAboveBounds())
        {
            SlowReelMovement(minPosition.position);
        }
        //mouse is below bounds so expand line to max length
        else if (MouseBelowBounds())
        {
            movePoint.position = maxPosition.position;
        }
        //mouse is within bounds so move line to mouse position
        else
        {
            Vector3 target = new(movePoint.position.x, mouseYPos, movePoint.position.z);

            //move up
            if (target.y > movePoint.position.y)
                SlowReelMovement(new Vector3(movePoint.position.x, mouseYPos, movePoint.position.z));
            //move down
            else
                movePoint.position = new Vector3(movePoint.position.x, mouseYPos, movePoint.position.z);
        }
    }

    private Coroutine SlowReelMovement(Vector3 target)
    {
        if (movingPoint)
            StopCoroutine(co_movingPoint);

        co_movingPoint = StartCoroutine(MoveToTarget(target));

        return co_movingPoint;
    }

    private IEnumerator MoveToTarget(Vector3 target)
    {      
        while (Vector3.Distance(movePoint.position, target) > 0)
        {
            movePoint.position = Vector3.MoveTowards(movePoint.position, target, Time.deltaTime * slowReelSpeed);
            yield return null;
        }

        co_movingPoint = null;
    }
    #endregion
}
