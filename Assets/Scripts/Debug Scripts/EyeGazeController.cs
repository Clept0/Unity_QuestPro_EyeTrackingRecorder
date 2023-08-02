using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeGazeController : MonoBehaviour
{
    public GameObject arrow;
    OVREyeGaze eyeGaze;
    public GameObject debugText;

    void Start()
    {
        eyeGaze = GetComponent<OVREyeGaze>();
    }

    void Update()
    {
        if (eyeGaze == null) {
            debugText.GetComponent<TextMesh>().text = "Gaze Not Detected";
            return;
        }

        if (eyeGaze.EyeTrackingEnabled) {
            arrow.transform.rotation = eyeGaze.transform.rotation;
            arrow.transform.position = eyeGaze.transform.position;

            debugText.GetComponent<TextMesh>().text = "Eye Tracking Enabled";
            string name = eyeGaze.transform.name;
            string worldRot = eyeGaze.transform.rotation.ToString();
            string worldPos = eyeGaze.transform.position.ToString();
            string eulerAngles = eyeGaze.transform.eulerAngles.ToString();

            // string locRot = (eyeGaze.transform.rotation * transform.worldToLocalMatrix).ToString();
            // string locPos = eyeGaze.localPosition.ToString();
            // string locEulerAngles = eyeGaze.transform.localEulerAngles.ToString();

            string forward = eyeGaze.transform.forward.ToString();
            string right = eyeGaze.transform.right.ToString();
            string up = eyeGaze.transform.up.ToString();
            string trackingMode = eyeGaze.TrackingMode.ToString();
            string confidence = eyeGaze.Confidence.ToString();


            debugText.GetComponent<TextMesh>().text = "Name: " + name + "\n" +
                                                    "WorldRot: " + worldRot + "\n" + 
                                                    "WorldPos: " + worldPos + "\n" + 
                                                    "EulerAngles: " + eulerAngles + "\n" + 
                                                    // "LocRot: " + locRot + "\n" + 
                                                    // "LocPos: " + locPos + "\n" +
                                                    // "LocEulerAngles: " + locEulerAngles + "\n" + 
                                                    "ForwardVec: " + forward + "\n" + 
                                                    "RightVec: " + right + "\n" + 
                                                    "UpVec: " + up + "\n" + 
                                                    "TrackingMode: " + trackingMode + "\n" + 
                                                    "Confidence: " + confidence;
        }
    }
}