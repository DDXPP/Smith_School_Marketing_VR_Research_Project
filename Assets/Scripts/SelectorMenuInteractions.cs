using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectorMenuInteractions : MonoBehaviour
{
    public GameObject menuRoot; // assign ObjectSelectingMenu or Panel

    private bool isMenuVisible = false;


    private List<Button> buttons = new List<Button>();
    private int currentIndex = 0;

    private InputDevice leftHand;

    private bool lastPressed = false;
    private float pressStartTime = 0f;

    public float longPressThreshold = 0.6f;

    private bool longPressTriggered = false;

    void Start()
    {
        leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        menuRoot.SetActive(true); 
        Invoke(nameof(InitButtons), 0.1f);
    }

    void InitButtons()
    {
        buttons.Clear();
        buttons.AddRange(menuRoot.GetComponentsInChildren<Button>(true));

        HighlightButton();
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (!leftHand.isValid)
        {
            leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            return;
        }

        bool pressed;
        if (leftHand.TryGetFeatureValue(CommonUsages.secondaryButton, out pressed))
        {
            // Button just pressed
            if (pressed && !lastPressed)
            {
                pressStartTime = Time.time;
                longPressTriggered = false;

                if (!isMenuVisible)
                {
                    ShowMenu();
                    leftHand.SendHapticImpulse(0u, 0.5f, 0.1f);  // vibrate controller
                    lastPressed = pressed;
                    return; 
                }
            }

            // Button held
            if (pressed && isMenuVisible && !longPressTriggered)
            {
                if (Time.time - pressStartTime >= longPressThreshold)
                {
                    SelectCurrent();
                    leftHand.SendHapticImpulse(0u, 0.5f, 0.1f);  // vibrate controller
                    longPressTriggered = true;
                }
            }

            // Button released
            if (!pressed && lastPressed)
            {
                float pressDuration = Time.time - pressStartTime;

                if (isMenuVisible)
                {
                    // Short press → move
                    if (pressDuration < longPressThreshold && !longPressTriggered)
                    {
                        MoveNext();
                    }
                }
                
            }

            lastPressed = pressed;
        }
    }

    void MoveNext()
    {
        currentIndex = (currentIndex + 1) % buttons.Count;
        HighlightButton();
    }

    void SelectCurrent()
    {
        buttons[currentIndex].onClick.Invoke();
        Debug.Log("Selected: " + buttons[currentIndex].name);
        HideMenu();
    }

    void HighlightButton()
    {
        for (int i = 0; i < buttons.Count; i++)
        {   
            if (i == currentIndex)
            {
                buttons[i].transform.localScale = Vector3.one * 1.1f;
            }
            else
            {
                buttons[i].transform.localScale = Vector3.one;
            }
        }
    }

    void ShowMenu()
    {
        isMenuVisible = true;
        menuRoot.SetActive(true);
        currentIndex = 0;
        HighlightButton();
    }

    void HideMenu()
    {
        isMenuVisible = false;
        menuRoot.SetActive(false);
    }
}