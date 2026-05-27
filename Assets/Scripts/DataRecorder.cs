using UnityEngine;
using UnityEngine.XR;
using Oculus.Interaction;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public class DataRecorder : MonoBehaviour
{
    // private Grabbable grabbable;
    private float logInterval;
    private float lastLogTime = 0f;

    public float SamplesPerSecond = 10f;

    private InteractionLogger logger;
    // private CSVLogger csvLogger;


    // -------------------- CSV file
    private StringBuilder tempString = new StringBuilder();

    // /storage/emulated/0/Android/data/com.UnityTechnologies.com.unity.template.urpblank/files/
    private string filePath;

    // periodically save incase accident app kill
    public float saveInterval = 2f;


    void Start()
    {
        logger = GetComponent<InteractionLogger>();
        logInterval = 1.0f / SamplesPerSecond;

        // build CSV file
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        filePath = Path.Combine(Application.persistentDataPath, $"{timestamp}_{this.gameObject.name}_vr_data.csv");
        tempString.AppendLine("date,time,runtime,object_name,x,y,z,rx,ry,rz,cam_x,cam_y,cam_z,cam_rx,cam_ry,cam_rz,touch,grab,time_to_grab,grab_dist,grab_event,grab_count,grab_duration,touch_no_grab_count,side,frontDot,rightDot,upDot,is_final_selection");
        
        InvokeRepeating(nameof(SaveToFile), saveInterval, saveInterval);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastLogTime < logInterval)
            return;

        lastLogTime = Time.time;

        Log();
    }

    void OnApplicationQuit()
    {
        SaveToFile();
    }

    public void SaveToFile()
    {
        File.WriteAllText(filePath, tempString.ToString());
        Debug.Log("CSV saved to: " + filePath);
    }

    public void Log()
    {
        Log((false, ""));
    }

    public void Log((bool logGrabEvent, string grab_event) t)
    {
        string date = DateTime.Now.ToString("yyyy-MM-dd");
        string time = DateTime.Now.ToString("HH:mm:ss.fff");
        float runtime = Time.time;
        string object_name =  this.gameObject.name;
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        Vector3 cam_pos = logger.cameraTransform.position;
        Quaternion cam_rot = logger.cameraTransform.rotation;
        bool touch = logger.isTouching;
        bool grab = logger.isGrabbing;
        float time_to_grab = logger.lastTimeToGrab;
        float grab_dist = logger.lastGrabDistance;
        string side = logger.side;
        float frontDot = (side == "Not in view" ? 999f : logger.frontDot);
        float rightDot = (side == "Not in view" ? 999f : logger.rightDot);
        float upDot = (side == "Not in view" ? 999f : logger.upDot);
        string grab_count = t.logGrabEvent ? (t.grab_event == "GrabStart" ? logger.grabCount.ToString() : "") : "";
        string grab_duration = t.logGrabEvent ? (t.grab_event == "GrabEnd" ? logger.grabDuration.ToString() : (t.grab_event == "GrabStart" ? "0" : "")) : "";
        string touch_no_grab_count = t.logGrabEvent ? (t.grab_event == "TouchNoGrab" ? logger.touchNoGrabCount.ToString() : "") : "";
        string is_final_selection = logger.isOnFinalSelectionTable ? "true" : "";

        tempString.AppendLine(
            $"{date},{time},{runtime},{object_name}," +
            $"{pos.x},{pos.y},{pos.z}," +
            $"{rot.eulerAngles.x},{rot.eulerAngles.y},{rot.eulerAngles.z}," +
            $"{cam_pos.x},{cam_pos.y},{cam_pos.z}," +
            $"{cam_rot.eulerAngles.x},{cam_rot.eulerAngles.y},{cam_rot.eulerAngles.z}," +
            $"{touch},{grab}," + 
            $"{time_to_grab},{grab_dist}," + 
            $"{t.grab_event},{grab_count},{grab_duration},{touch_no_grab_count}," + 
            $"{side},{frontDot},{rightDot},{upDot}," + 
            $"{is_final_selection}"
        );
    }
}
