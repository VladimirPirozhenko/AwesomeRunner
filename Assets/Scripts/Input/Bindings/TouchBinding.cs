using UnityEngine;

public enum ETouchGesture
{
    NONE,
    TAP,
    SWIPE_LEFT,
    SWIPE_RIGHT, 
    SWIPE_UP, 
    SWIPE_DOWN
}
public class TouchBinding : IBinding
{
    private ETouchGesture wantedTouchGesture;
    private ETouchGesture actualGesture;
    private bool isDragging = false;
    private int swipeThreshold = 100;
    private Vector2 startTouch;
    private Vector2 swipeDelta;

    public bool IsPressed()
    { 
        
        if (wantedTouchGesture == ETouchGesture.NONE)
            return false;

        ReadGesture();
       
        if (actualGesture == wantedTouchGesture)
        {
            return true;
        }
        return false;
    }

    public bool IsReleased()
    {
        return false;
    }

    public bool IsRestricted { get; set; }

    public TouchBinding(ETouchGesture gesture)
    {
        wantedTouchGesture = gesture; 
        IsRestricted = false;
    }

    private void ReadGesture()
    {
        actualGesture = ETouchGesture.NONE;

        #region ForPC
        #if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            actualGesture = ETouchGesture.TAP;
            isDragging = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            actualGesture = ETouchGesture.NONE;
            Reset();
        }
        #endif
        #endregion

        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                actualGesture = ETouchGesture.TAP;
                isDragging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDragging = false;
                Reset();
            }
        }

        swipeDelta = Vector2.zero;
        if (isDragging)
        {
            if (Input.touches.Length < 0)
                swipeDelta = Input.touches[0].position - startTouch;
            else if (Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
        }
  
       
        if (swipeDelta.magnitude > swipeThreshold)
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {

                if (x < 0)
                    actualGesture = ETouchGesture.SWIPE_LEFT;
                else
                    actualGesture = ETouchGesture.SWIPE_RIGHT;
            }
            else
            {

                if (y < 0)
                    actualGesture = ETouchGesture.SWIPE_DOWN;
                else
                    actualGesture = ETouchGesture.SWIPE_UP;
            }
            Reset();
           
        }

    }

    private void Reset()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDragging = false;
    }
}

