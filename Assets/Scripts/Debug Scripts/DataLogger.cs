using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Oculus;

public class DataLogger : MonoBehaviour
{
    private List<string> log = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddFrame(int frameNumber, string movement)
    {
        log.Add(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}",
                transform.name,
                transform.tag,
                System.DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.ffffff"),
                frameNumber,
                movement,
                transform.position,
                transform.localPosition,
                transform.rotation,
                transform.localRotation,
                transform.rotation.normalized,
                transform.eulerAngles,
                transform.localEulerAngles,
                transform.forward,
                transform.right,
                transform.up

                ));
    }

    public void AddHeader()
    {
        log.Clear();
        log.Add(string.Format("{0},{1},{2}",
                "Transform_Name",
                "Transform_Tag",
                "Time",
                "Frame",
                "Movement",
                "Position",
                "Position_Local",
                "Rotation",
                "Rotation_Local",
                "Rotation_Norm",
                "EulerAngles",
                "EulerAngles_Local",
                "Vector_Forward",
                "Vector_Right",
                "Vector_Up"

                ));
    }

    public void SaveFile(string fileName)
    {
        //string filePath = Path.Combine(Application.dataPath, fileName);
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        Debug.Log(filePath);
        File.WriteAllLines(filePath, log);
        log.Clear();
    }
}
