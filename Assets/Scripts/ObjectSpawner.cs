using UnityEngine;
using UnityEngine.XR;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;


public class ObjectSpawner : MonoBehaviour
{
    private GameObject currentObject;
    public Vector3 spawnPosition = new Vector3(0f, 1.05f, 0.5f);
    public float SamplesPerSecond = 10f;
    public float saveInterval = 2f;

    public void SpawnObject(GameObject prefab)
    {
        // if (currentObject != null)
        // {
        //     Destroy(currentObject);
        // }
        // ^ uncomment this if only one object is allowed in the scene

        currentObject = Instantiate(
            prefab,
            spawnPosition,
            Quaternion.identity
        );
        currentObject.name = prefab.name;

        AddRequiredComponents();
        AddHandGrab();
    }

    void AddRequiredComponents()
    {
        if (currentObject.GetComponent<Rigidbody>() == null)
        {
            currentObject.AddComponent<Rigidbody>();
            currentObject.GetComponent<Rigidbody>().mass = 1f;
            currentObject.GetComponent<Rigidbody>().linearDamping = 8f;
            currentObject.GetComponent<Rigidbody>().angularDamping = 11f;
        }
            
        if (currentObject.GetComponent<Grabbable>() == null)
            currentObject.AddComponent<Grabbable>();

        if (currentObject.GetComponent<InteractionLogger>() == null)
            currentObject.AddComponent<InteractionLogger>();

        if (currentObject.GetComponent<DataRecorder>() == null) 
            currentObject.AddComponent<DataRecorder>();
        currentObject.GetComponent<DataRecorder>().SamplesPerSecond = SamplesPerSecond;
        currentObject.GetComponent<DataRecorder>().saveInterval = saveInterval;

        // if (currentObject.GetComponent<CSVLogger>() == null)
        //     currentObject.AddComponent<CSVLogger>();

        if (currentObject.GetComponent<MeshCollider>() != null)
            currentObject.GetComponent<MeshCollider>().convex = true;
    }

    void AddHandGrab()
    {
        GameObject handGrab = new GameObject("HandGrabInstallationRoutine");
        handGrab.transform.SetParent(currentObject.transform);

        handGrab.AddComponent<HandGrabInteractable>();
        handGrab.AddComponent<GrabInteractable>();

        HandGrabInteractable hgi = handGrab.GetComponent<HandGrabInteractable>();
        GrabInteractable gi = handGrab.GetComponent<GrabInteractable>();

        Rigidbody rb = currentObject.GetComponent<Rigidbody>();
        Grabbable pointable = currentObject.GetComponent<Grabbable>();

        hgi.InjectRigidbody(rb);
        gi.InjectRigidbody(rb);

        hgi.InjectOptionalPointableElement(pointable);
        gi.InjectOptionalPointableElement(pointable);
    }

    public void DestroyObject(GameObject prefab)
    {
        GameObject temp = GameObject.Find(prefab.name);
        if (temp != null)
        {
            Destroy(temp);
        }
    }
}




