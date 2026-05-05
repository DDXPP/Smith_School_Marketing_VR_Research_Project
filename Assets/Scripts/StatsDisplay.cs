using UnityEngine;
using UnityEngine.XR;
using TMPro;

public class StatsDisplay : MonoBehaviour
{
    public TextMeshPro text;

    // public InteractionLogger logger;

    private bool isVisible = true;
    private bool lastButtonState = false;

    void Start()
    {
        text.gameObject.SetActive(isVisible);
    }
    void Update()
    {
        // HandleToggle();

        if (IsButtonPressed())
        {
            isVisible = !isVisible;
            text.gameObject.SetActive(isVisible);
        }

        if (isVisible)
        {
            UpdateText();
        }
    }

    bool IsButtonPressed()
    {
        InputDevice leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        bool buttonPressed;
        if (leftHand.TryGetFeatureValue(CommonUsages.primaryButton, out buttonPressed)) // X button
        {   
            if (buttonPressed && !lastButtonState)
            {
                lastButtonState = true;
                return true;
            }
            lastButtonState = buttonPressed;
        }

        return false;
    }

    void UpdateText()
    {
        var logger = Object.FindAnyObjectByType<InteractionLogger>();
        if (logger == null) return;

        string sideDots = (logger.side == "Not in view") ? "" : $"Front: {logger.frontDot.ToString("F2")} | Right: {logger.rightDot.ToString("F2")} | Up: {logger.upDot.ToString("F2")}";

        text.text =
            $"Touch: {logger.isTouching}\n" +
            $"Grab: {logger.isGrabbing}\n\n" +
            $"Time-to-Grab: {logger.lastTimeToGrab:F3}s\n" +
            // $"Hover Dist: {logger.hoverStartDistance:F3}\n" +
            $"Grab Dist: {logger.lastGrabDistance:F3}\n\n" +
            $"grabCount: {logger.grabCount} | touchNoGrabCount: {logger.touchNoGrabCount}\n " + 
            $"Pos: {logger.transform.position}\n" +
            $"Rot: {logger.transform.rotation.eulerAngles}\n\n" + 
            $"Side: {logger.side}\n" + $"{sideDots}"
            ;
    }

}
