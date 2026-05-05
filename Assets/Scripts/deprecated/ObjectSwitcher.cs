using UnityEngine;
using UnityEngine.XR;

public class ObjectSwitcher : MonoBehaviour
{
    public GameObject[] objectList;
    private GameObject currentObject;
    private int currentIndex = 0;
    private bool lastButtonState = false;

    void Start()
    {
         currentObject = Instantiate(
            objectList[0], 
            new Vector3(0.0f, 1.058f, 0.5f), 
            Quaternion.identity
        );
    }

    // Update is called once per frame
    void Update()
    {
        if (IsButtonPressed())
        {
            SwitchObject();
        }
    }

    bool IsButtonPressed()
    {
        InputDevice leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        bool buttonPressed;
        if (leftHand.TryGetFeatureValue(CommonUsages.secondaryButton, out buttonPressed))
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

    void SwitchObject()
    {
        if (currentObject != null)
        {
            Destroy(currentObject);
        }

        currentIndex = (currentIndex + 1) % objectList.Length;

        // Spawn new
        currentObject = Instantiate(
            objectList[currentIndex], 
            new Vector3(0.0f, 1.058f, 0.5f), 
            Quaternion.identity
        );
    }
}
