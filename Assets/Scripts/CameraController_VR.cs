using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CameraController_VR : MonoBehaviour
{
    public GameObject leftArrow;
    public GameObject rightArrow;
    public GameObject debugText;
    public GameObject trackingSphere;

    public Camera cameraRig;
    OVREyeGaze[] eyeGazes;
    OVREyeGaze leftGaze, rightGaze;

    private List<string> log = new List<string>();

    void Start()
    {
        eyeGazes = GetComponentsInChildren<OVREyeGaze>();

        foreach(OVREyeGaze gaze in eyeGazes)
        {
            if(gaze.name == "Left EyeGazeController")
            {
                leftGaze = gaze;

            }

            if(gaze.name == "Right EyeGazeController")
            {
                rightGaze = gaze;

            }
        }
        
    }    

    void Update()
    {
        if (cameraRig == null) {
            debugText.GetComponent<TextMesh>().text = "Camera Not Detected";
            return;
        }

        if (leftGaze == null || rightGaze == null) {
            debugText.GetComponent<TextMesh>().text = "Gaze Not Detected";
            return;
        }


        // camera data
        string cameraPos = cameraRig.transform.position.ToString();
        string cameraRot = cameraRig.transform.rotation.ToString();
       

        string cameraDebug = "CameraPos: " + cameraPos + "\n" + 
                            "CameraRot: " + cameraRot;
        
        if (leftGaze.EyeTrackingEnabled && rightGaze.EyeTrackingEnabled) {
        //    debugText.GetComponent<TextMesh>().text = "Eye tracking enabled";

            // left eye gaze controller data
            // arrow for visual tracking indicator
            leftArrow.transform.rotation = leftGaze.transform.rotation;
            leftArrow.transform.position = leftGaze.transform.position;

            string leftName = leftGaze.transform.name;
            string leftWorldRot = leftGaze.transform.rotation.ToString();
            string leftWorldPos = leftGaze.transform.position.ToString();
            string leftEulerAngles = leftGaze.transform.eulerAngles.ToString();
            string leftForward = leftGaze.transform.forward.ToString();
            string leftRight = leftGaze.transform.right.ToString();
            string leftUp = leftGaze.transform.up.ToString();
            string leftTrackingMode = leftGaze.TrackingMode.ToString();
            string leftConfidence = leftGaze.Confidence.ToString();


            string leftEyeControllerDebug = "Name: " + leftName + "\n" +
                                            "WorldRot: " + leftWorldRot + "\n" + 
                                            "WorldPos: " + leftWorldPos + "\n" + 
                                            "EulerAngles: " + leftEulerAngles + "\n" + 
                                            "ForwardVec: " + leftForward + "\n" + 
                                            "RightVec: " + leftRight + "\n" + 
                                            "UpVec: " + leftUp + "\n" + 
                                            "TrackingMode: " + leftTrackingMode + "\n" + 
                                            "Confidence: " + leftConfidence;

            // right eye gaze controller data      
            // arrow for visual tracking indicator
            rightArrow.transform.rotation = rightGaze.transform.rotation;
            rightArrow.transform.position = rightGaze.transform.position;

            string rightName = rightGaze.transform.name;
            string rightWorldRot = rightGaze.transform.rotation.ToString();
            string rightWorldPos = rightGaze.transform.position.ToString();
            string rightEulerAngles = rightGaze.transform.eulerAngles.ToString();
            string rightForward = rightGaze.transform.forward.ToString();
            string rightRight = rightGaze.transform.right.ToString();
            string rightUp = rightGaze.transform.up.ToString();
            string rightTrackingMode = rightGaze.TrackingMode.ToString();
            string rightConfidence = rightGaze.Confidence.ToString();


            string rightEyeControllerDebug = "Name: " + rightName + "\n" +
                                            "WorldRot: " + rightWorldRot + "\n" + 
                                            "WorldPos: " + rightWorldPos + "\n" + 
                                            "EulerAngles: " + rightEulerAngles + "\n" + 
                                            "ForwardVec: " + rightForward + "\n" + 
                                            "RightVec: " + rightRight + "\n" + 
                                            "UpVec: " + rightUp + "\n" + 
                                            "TrackingMode: " + rightTrackingMode + "\n" + 
                                            "Confidence: " + rightConfidence;
            
            // tracking sphere location
            string ballPos = trackingSphere.transform.position.ToString();
            string ballRot = trackingSphere.transform.rotation.ToString();
            string ballDebug = "ballPos: " + ballPos + "\n" +
                                "ballRot: " + ballRot;

            //debugText.GetComponent<TextMesh>().text = cameraDebug + "\n\n" + leftEyeControllerDebug + "\n\n" + rightEyeControllerDebug + "\n\n" + ballDebug;

        }
        
    }

    public void AddFrame(int frameNumber, string movement)
    {

        log.Add(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22}",
            System.DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.ffffff"),
            frameNumber,
            movement,
            // camera data
            cameraRig.transform.position,
            cameraRig.transform.rotation,
            
            leftGaze.transform.position,
            leftGaze.transform.rotation,
            leftGaze.transform.eulerAngles,
            leftGaze.transform.forward,
            leftGaze.transform.right,
            leftGaze.transform.up,
            leftGaze.TrackingMode,
            leftGaze.Confidence,

            rightGaze.transform.position,
            rightGaze.transform.rotation,
            rightGaze.transform.eulerAngles,
            rightGaze.transform.forward,
            rightGaze.transform.right,
            rightGaze.transform.up,
            rightGaze.TrackingMode,
            rightGaze.Confidence,

            trackingSphere.transform.position,
            trackingSphere.transform.rotation

            ));
    }
    
    public void AddHeader()
    {
        log.Clear();
        log.Add(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42},{43},{44},{45},{46},{47},{48},{49},{50},{51},{52},{53},{54},{55},{56},{57},{58}",
            "Date",
            "Frame",
            "Movement",

            "Camera_Position_x",
            "Camera_Position_y",
            "Camera_Position_z",
            "Camera_Rotation_x",
            "Camera_Rotation_y",
            "Camera_Rotation_z",
            "Camera_Rotation_w",
            
            "Gaze_Left Eye Position_x",
            "Gaze_Left Eye Position_y",
            "Gaze_Left Eye Position_z",
            "Gaze_Left Eye Rotation_x",
            "Gaze_Left Eye Rotation_y",
            "Gaze_Left Eye Rotation_z",
            "Gaze_Left Eye Rotation_w",

            "Gaze_Left Euler Angles_x",
            "Gaze_Left Euler Angles_y",
            "Gaze_Left Euler Angles_z",

            "Gaze_Left Forward Vector_x",
            "Gaze_Left Forward Vector_y",
            "Gaze_Left Forward Vector_z",
            "Gaze_Left Right Vector_x",
            "Gaze_Left Right Vector_y",
            "Gaze_Left Right Vector_z",
            "Gaze_Left Up Vector_x",
            "Gaze_Left Up Vector_y",
            "Gaze_Left Up Vector_z",

            "Gaze_Left Tracking Mode",
            "Gaze_Left Confidence Value",

            "Gaze_Right Eye Position_x",
            "Gaze_Right Eye Position_y",
            "Gaze_Right Eye Position_z",
            "Gaze_Right Eye Rotation_x",
            "Gaze_Right Eye Rotation_y",
            "Gaze_Right Eye Rotation_z",
            "Gaze_Right Eye Rotation_w",

            "Gaze_Right Euler Angles_x",
            "Gaze_Right Euler Angles_y",
            "Gaze_Right Euler Angles_z",

            "Gaze_Right Forward Vector_x",
            "Gaze_Right Forward Vector_y",
            "Gaze_Right Forward Vector_z",
            "Gaze_Right Right Vector_x",
            "Gaze_Right Right Vector_y",
            "Gaze_Right Right Vector_z",
            "Gaze_Right Up Vector_x",
            "Gaze_Right Up Vector_y",
            "Gaze_Right Up Vector_z",

            "Gaze_Right Tracking Mode",
            "Gaze_Right Confidence Value",

            "Ball Position_x",
            "Ball Position_y",
            "Ball Position_z",
            "Ball Rotation_x",
            "Ball Rotation_y",
            "Ball Rotation_z",
            "Ball Rotation_w"

            ));
    }

    public void SaveFile(string fileName)
    {
        //string filePath = Path.Combine(Application.dataPath, fileName);
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        Debug.Log(filePath);

        for(int i = 0; i < log.Count; i++) {
            if(log[i].Contains("(")) {
                log[i] = log[i].Replace("(", "");
            }

            if(log[i].Contains(")")) {
                log[i] = log[i].Replace(")", "");
            }
        }
        
        File.WriteAllLines(filePath, log);
        log.Clear();
    }

}