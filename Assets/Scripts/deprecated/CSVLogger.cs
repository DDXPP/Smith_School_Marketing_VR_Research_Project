using UnityEngine;
using UnityEngine.XR;
using System;
using System.IO;
using System.Text;

public class CSVLogger : MonoBehaviour
{
    private StringBuilder tempString = new StringBuilder();

    // /storage/emulated/0/Android/data/com.UnityTechnologies.com.unity.template.urpblank/files/
    private string filePath;

    // periodically save incase accident app kill
    public float saveInterval = 2f;


    void Start()
    {
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        filePath = Path.Combine(Application.persistentDataPath, $"{timestamp}_{this.gameObject.name}_vr_data.csv");
        tempString.AppendLine("date,time,object_name,x,y,z,rx,ry,rz,touch,grab");
        InvokeRepeating(nameof(SaveToFile), saveInterval, saveInterval);
    }


    public void Log(Vector3 pos, Quaternion rot, bool touch, bool grab)
    {
        string date = DateTime.Now.ToString("yyyy-MM-dd");
        string time = DateTime.Now.ToString("HH:mm:ss.fff");
        string object_name =  this.gameObject.name;
        tempString.AppendLine(
            $"{date},{time},{object_name}," +
            $"{pos.x},{pos.y},{pos.z}," +
            $"{rot.eulerAngles.x},{rot.eulerAngles.y},{rot.eulerAngles.z}," +
            $"{touch},{grab}"
        );

        // Debug.Log($"------------------------------ | date: {date} | time: {time} | objectName: {object_name} ");

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
}


// TODO: add object name