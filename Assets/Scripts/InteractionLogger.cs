using UnityEngine;
using UnityEngine.XR;
using Oculus.Interaction;
using OVR;
using System.Collections.Generic;

public class InteractionLogger : MonoBehaviour
{
    private Grabbable grabbable;
    // private DataRecorder dataRecorder;
    private float touchStartTime = -1f;
    private Vector3 hoverStartHandPos;
    private float hoverStartDistance;


    public bool isTouching = false;
    public bool isGrabbing = false;
    public float lastTimeToGrab;
    public float lastGrabDistance;

    public Transform cameraTransform; // CenterEyeAnchor
    public float upDot;
    public float rightDot;
    public float frontDot;
    public string side;

    public int grabCount = 0;
    public int touchNoGrabCount = 0;
    public float grabDuration;
    private bool grabbedDuringTouch = false;
    private float grabStartTime;


    void Awake()
    {
        grabbable = GetComponent<Grabbable>();
        // dataRecorder = GetComponent<DataRecorder>();

        if (cameraTransform == null)
        {
            Camera cam = Camera.main;
            if (cam != null)
                cameraTransform = cam.transform;
        }
    }

    void OnEnable()
    {
        if (grabbable != null)
        {
            // Debug.Log("---------------------- grabbable name: " + grabbable.name);
            grabbable.WhenPointerEventRaised += OnPointerEvent;
            // Debug.Log("---------------------- grabbable enabled");
        }
    }

    void OnDisable()
    {
        if (grabbable != null)
        {
            grabbable.WhenPointerEventRaised -= OnPointerEvent;
        }
    }

    // https://developers.meta.com/horizon/documentation/unity/unity-isdk-pointer-events/
    private void OnPointerEvent(PointerEvent evt)
    {
        DataRecorder dataRecorder = GetComponent<DataRecorder>();

        switch (evt.Type)
        {
            // touch
            case PointerEventType.Hover:
                isTouching = true;
                grabbedDuringTouch = false;

                if (touchStartTime < 0f)
                {
                    touchStartTime = Time.time;

                    if (TryGetRightHandPosition(out Vector3 handPos))
                    {
                        hoverStartHandPos = handPos;
                        hoverStartDistance = Vector3.Distance(handPos, transform.position);
                    }
                }
                break;

            case PointerEventType.Unhover:
                isTouching = false;
                touchStartTime = -1f;

                if (!grabbedDuringTouch)
                {
                    touchNoGrabCount++;
                    dataRecorder.Log((true, "TouchNoGrab"));
                }   
                break;

            // grab
            case PointerEventType.Select:
                isGrabbing = true;

                grabCount++;
                grabStartTime = Time.time;
                dataRecorder.Log((true, "GrabStart"));
                

                if (isTouching)
                    grabbedDuringTouch = true;

                if (touchStartTime > 0f)
                {
                    lastTimeToGrab = Time.time - touchStartTime;

                    if (TryGetRightHandPosition(out Vector3 handPos))
                    {
                        lastGrabDistance = Vector3.Distance(handPos, transform.position);
                    }

                    // Debug.Log($"Time-to-Grab: {lastTimeToGrab:F3}s | HoverDist: {hoverStartDistance:F3} | GrabDist: {lastGrabDistance:F3}");
                }
                break;

            case PointerEventType.Unselect:
                isGrabbing = false;
                grabDuration = Time.time - grabStartTime;
                dataRecorder.Log((true, "GrabEnd"));

                touchStartTime = -1f;
                // grabStartTime = -1f;
                break;

            
        }
    }

    void Update()
    {
        // Debug.Log("----------------------" + cameraTransform.position);
        ViewSide();
    }

    void ViewSide()
    {
        if (cameraTransform == null) return;

        Vector3 cameraForward = cameraTransform.forward; 
        Vector3 cameraPos = cameraTransform.position;   
        Vector3 objPos = this.gameObject.transform.position;

        // Check if in view
        Vector3 toObject = (objPos - cameraPos).normalized;
        float viewDot = Vector3.Dot(cameraForward, toObject);

        if (viewDot < 0.5f) 
        {
            side = "Not in view";
            return; // not in view
        }

        Vector3 viewDir = (cameraPos - objPos).normalized;
        frontDot = Vector3.Dot(viewDir, this.gameObject.transform.forward);
        rightDot = Vector3.Dot(viewDir, this.gameObject.transform.right);
        upDot = Vector3.Dot(viewDir, this.gameObject.transform.up);
        side = GetSide(frontDot, rightDot, upDot);

        // Debug.Log($"--------------- side: {side} | frontDot: {frontDot} | rightDot: {rightDot} | upDot: {upDot}");
    }

    string GetSide(float frontDot, float rightDot, float upDot)
    {
        float absFront = Mathf.Abs(frontDot);
        float absRight = Mathf.Abs(rightDot);
        float absUp = Mathf.Abs(upDot);

        if (absFront > absRight && absFront > absUp)
            return frontDot > 0 ? "Front" : "Back";
        else if (absRight > absUp)
            return rightDot > 0 ? "Right" : "Left";
        else
            return upDot > 0 ? "Top" : "Bottom";
    }

    private bool TryGetRightHandPosition(out Vector3 position)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (device.TryGetFeatureValue(CommonUsages.devicePosition, out position))
        {
            return true;
        }

        position = Vector3.zero;
        return false;
    }
}

