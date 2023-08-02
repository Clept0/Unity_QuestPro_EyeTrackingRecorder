using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Oculus;
using UnityEngine.XR;
using UnityEngine.SceneManagement;


public class Calibration : MonoBehaviour
{
    public GameObject CalibrationObject;
    public GameObject countdownText;
    
    [SerializeField]
    OVRCameraRig cameraRig;
    
    private GameObject currentObject;
    private GameObject grid;
    private GameObject edges;

    private List<Transform> gridTransforms = new List<Transform>();
    private Transform end = null;

    private bool recording = false;
    private string filename;
    private string movement = "start";

    private int frameNumber = 0;
    private bool isCalibrating = false;
    private bool isReady = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        countdownText.GetComponent<TextMesh>().text = "Calibration";
        countdownText.SetActive(true);
        StartCalibration();
    }

    void OnDisable()
    {
        countdownText.SetActive(false);
        //countdownText.GetComponent<TextMesh>().text = Application.persistentDataPath;

        edges.SetActive(false);
        currentObject.SetActive(false);
        GetComponent<Renderer>().enabled = false;
        isCalibrating = false;
        isReady = false;
        movement = "start";
        frameNumber = 0;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Home");

    }


    // Update is called once per frame
    void Update()
    {
        if(recording) {
            cameraRig.GetComponent<CameraController>().AddFrame(frameNumber, movement);
            frameNumber++;
        }

        if (Input.GetKeyDown("q"))
        {
            StartCalibration();
        }

        if (Input.GetKeyDown("return") && !isCalibrating)
        {
            if (isReady)
            {
                edges.SetActive(false);
                currentObject.SetActive(false);
                recording = true;
                StartCoroutine(calibration());
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            StartCoroutine(LoadHomeScene());
        }


        if (Input.GetKeyDown("up") && !isCalibrating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position += currentObject.transform.up * 0.005f;
            }
        }
        if (Input.GetKeyDown("down") && !isCalibrating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position -= currentObject.transform.up * 0.005f;
            }
        }
        if (Input.GetKeyDown("left") && !isCalibrating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position -= currentObject.transform.right * 0.005f;
            }
        }
        if (Input.GetKeyDown("right") && !isCalibrating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position += currentObject.transform.right * 0.005f;
            }
        }
        if (Input.GetKeyDown("w") && !isCalibrating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position += currentObject.transform.forward * 0.005f;
            }
        }
        if (Input.GetKeyDown("s") && !isCalibrating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position -= currentObject.transform.forward * 0.005f;
            }
        }
        if (Input.GetKeyDown("a") && !isCalibrating)
        {
            if (currentObject != null)
            {
                currentObject.transform.Rotate(0.0f, -30.0f, 0.0f, Space.World);
            }
        }
        if (Input.GetKeyDown("d") && !isCalibrating)
        {
            if (currentObject != null)
            {
                currentObject.transform.Rotate(0.0f, 30.0f, 0.0f, Space.World);
            }
        }
    }

    public void StartCalibration()
    {
        isCalibrating = false;

        if (edges != null)
        {
            edges.SetActive(false);
            currentObject.SetActive(false);
        }

        currentObject = CalibrationObject;
        grid = currentObject.transform.Find("Positions").gameObject;
        edges = currentObject.transform.Find("Edges").gameObject;
        edges.SetActive(true);
        currentObject.SetActive(true);
        gridTransforms.Clear();
        foreach (Transform child in grid.transform)
        {
            gridTransforms.Add(child);
        }
        
        isReady = true;
        Debug.Log("isReady" + isReady);

    }

    IEnumerator calibration()
    {
        isReady = false;
        isCalibrating = true;
        filename = "calibration_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
        frameNumber = 0;
        movement = "start";

        cameraRig.GetComponent<CameraController>().AddHeader();
        countdownText.SetActive(true);

        List<int> indices = new List<int>();
        for (int i = 0; i < 13; i++)
        {
            indices.Add(i);
        }
        int nextIndex = Random.Range(0, indices.Count-1);
        indices.Remove(nextIndex);
        end = gridTransforms[nextIndex];
        countdownText.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            countdownText.GetComponent<TextMesh>().text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        countdownText.SetActive(false);
        movement = "transition";
        GetComponent<Renderer>().enabled = true;
        while (indices.Count > 0)
        {
            transform.position = end.position;
            movement = "transition";
            yield return new WaitForSeconds(0.5f);
            movement = "static";
            yield return new WaitForSeconds(2);
            nextIndex = indices[Random.Range(0, indices.Count-1)];
            indices.Remove(nextIndex);
            end = gridTransforms[nextIndex];
        }
        transform.position = end.position;
        movement = "transition";
        yield return new WaitForSeconds(0.5f);
        movement = "static";
        yield return new WaitForSeconds(2);

        GetComponent<Renderer>().enabled = false;
        recording = false;
        cameraRig.GetComponent<CameraController>().SaveFile(filename);
        frameNumber = 0;
        isCalibrating = false;
        countdownText.SetActive(true);
        countdownText.GetComponent<TextMesh>().text = "Done";
        countdownText.SetActive(true);
        yield return new WaitForSeconds(3);
        countdownText.SetActive(false);
        transform.gameObject.SetActive(false);
    }

    IEnumerator LoadHomeScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Home");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}