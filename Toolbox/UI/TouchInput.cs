using System;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    public float sensitivity = 1f;
    public float frameSensitivity = 20f;
    public float pinchSensitivity = 1f;
    public float swipeThreshold = 1;
    public bool logFrameDelta;
    public float minPinchDistance = 0;
    public float minFrameDistance = 0;

    [ReadOnly] public Vector2 delta;
    [ReadOnly] public float deltaX;
    [ReadOnly] public float deltaY;
    [ReadOnly] public Vector2 frameDelta;
    [ReadOnly] public float frameDeltaX;
    [ReadOnly] public float frameDeltaY;
    [ReadOnly] public float pinchDelta;
    [ReadOnly] public Vector2 pinchCenter;
    public Action onSwipeLeft, onSwipeRight, onSwipeUp, onSwipeDown;
    public Action<float> onSwipeHorizontal, onSwipeVertical;

    private Vector3 mouseStart;
    private Vector3 mousePrev;
    private float prevPinchDistance = 0;
    private bool swipable;
    private Vector3 mouse2Start;
    private Vector3 normalizeRate;

    private Vector3 MousePos => Vector3.Scale(Input.mousePosition, normalizeRate);

    private void Start()
    {
        normalizeRate = new Vector3(1f / Screen.width, 1f / Screen.height, 1);
    }

    void Update()
    {
        // pinch
        if (Input.touchCount == 2)
        {
            var touch1 = Input.GetTouch(0);
            var touch2 = Input.GetTouch(1);
            if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                var t1 = Vector3.Scale(touch1.position, normalizeRate);
                var t2 = Vector3.Scale(touch2.position, normalizeRate);
                var pinchDistance = (t1 - t2).magnitude;
                if (prevPinchDistance == 0)
                {
                    prevPinchDistance = pinchDistance;
                    return;
                }
                var deltaDistance = pinchDistance - prevPinchDistance;
                if (Mathf.Abs(deltaDistance) > minPinchDistance)
                {
                    pinchDelta = deltaDistance * pinchSensitivity;
                    pinchCenter = (touch1.position + touch2.position) / 2;
                }
                prevPinchDistance = pinchDistance;
            }
            mousePrev = Vector3.zero;
        }
        else
        {
            pinchDelta = 0;
            prevPinchDistance = 0;
            PinchSimulate();
            if (Input.GetMouseButtonDown(0)) mouseStart = MousePos;
            if (Input.GetMouseButton(0))
            {
                Vector3 mouse = MousePos;

                // start delta
                delta = (mouse - mouseStart) * sensitivity * Time.deltaTime;
                deltaX = delta.x;
                deltaY = delta.y;

                // frame delta
                if (mousePrev != Vector3.zero)
                {
                    frameDelta = (mouse - mousePrev) * frameSensitivity;// * Time.deltaTime;
                    if (frameDelta.magnitude < minFrameDistance) frameDelta = Vector3.zero;
                    frameDeltaX = frameDelta.x;
                    frameDeltaY = frameDelta.y;

                    // log
                    if (logFrameDelta)
                    {
                        Debug.Log("Frame delta x: " + frameDeltaX);
                        Debug.Log("Frame delta y: " + frameDeltaY);
                    }

                    // swipe detect
                    if (swipable)
                    {
                        if (Mathf.Abs(frameDeltaX) > swipeThreshold)
                        {
                            var dir = Mathf.Sign(frameDeltaX);
                            onSwipeHorizontal?.Invoke(dir);
                            if (dir > 0) onSwipeRight?.Invoke();
                            else onSwipeLeft?.Invoke();
                            swipable = false;
                        }
                        if (Mathf.Abs(frameDeltaY) > swipeThreshold)
                        {
                            var dir = Mathf.Sign(frameDeltaY);
                            onSwipeVertical?.Invoke(dir);
                            if (dir > 0) onSwipeUp?.Invoke();
                            else onSwipeDown?.Invoke();
                            swipable = false;
                        }
                    }
                }

                mousePrev = MousePos;
            }
            else
            {
                delta = Vector2.zero;
                deltaX = deltaY = 0;
                frameDelta = Vector2.zero;
                frameDeltaX = frameDeltaY = 0;
                mousePrev = Vector3.zero;
                swipable = true;
            }
        }
    }

    private void PinchSimulate()
    {
        pinchDelta = Input.GetAxis("Mouse ScrollWheel") * pinchSensitivity;
        pinchCenter = Input.mousePosition;
        //if (Input.GetMouseButtonDown(1)) mouse2Start = Input.mousePosition;
        //if (Input.GetMouseButton(1))
        //{
        //    var pinchDistance = (Input.mousePosition - mouse2Start).magnitude;
        //    Debug.Log(Input.mousePosition);
        //    Debug.Log(mouse2Start);
        //    if (prevPinchDistance == 0)
        //    {
        //        prevPinchDistance = pinchDistance;
        //        return;
        //    }
        //    var deltaDistance = pinchDistance - prevPinchDistance;
        //    if (Mathf.Abs(deltaDistance) > minPinchDistance)
        //    {
        //        pinchDelta = deltaDistance * pinchSensitivity;
        //    }
        //    prevPinchDistance = pinchDistance;
        //}
    }
}
