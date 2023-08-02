using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WorldStabilized : MonoBehaviour
{
    public GameObject worldStabilized;
    public GameObject countdownText;
    public GameObject leftHandController;
    public GameObject rightHandController;
    
    [SerializeField]
    OVRCameraRig cameraRig;
    
    private GameObject currentObject;
    private GameObject grid;
    private GameObject edges;

    private List<Transform> gridTransforms = new List<Transform>();
    private Transform start = null;    
    private Transform end = null;
    private int startIndex;
    private int endIndex;
    private List<string> log = new List<string>();

    private bool recording = false;
    private string filename;
    private string movement = "start";
    private int frameNumber;
    private bool isEvaluating = false;
    private bool isReady = false;
    private float pathTime = 5f;

    private int[] nextPos = {
                    24,19, 0,23,20,
                    9, 0, 0, 0,21,
                     0, 0, 0, 0, 0,
                     3, 0, 0, 0,15,
                     1, 2, 0, 5, 4};

    // Start is called before the first frame update
    void Start()
    {


    }

    void OnEnable()
    {
        countdownText.GetComponent<TextMesh>().text = "World Stabilized_Body Constrained";
        countdownText.SetActive(true);
        StartEvaluation();
    }

    void OnDisable()
    {
        countdownText.SetActive(false);
        
        edges.SetActive(false);
        currentObject.SetActive(false);
        GetComponent<Renderer>().enabled = false;
        isEvaluating = false;
        isReady = false;
        movement = "start";
        frameNumber = 0;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Home");
    }

    // Update is called once per frame
    void Update()
    {
       if (recording)
        {
            cameraRig.GetComponent<CameraController>().AddFrame(frameNumber, movement);
            frameNumber++;
        }
        if (Input.GetKeyDown("q"))
        {
            StartEvaluation();
        }
        if (Input.GetKeyDown("return") && !isEvaluating)
        {
            if (isReady)
            {
                grid = worldStabilized.transform.Find("Positions").gameObject;
                edges = worldStabilized.transform.Find("Edges").gameObject;
                grid.SetActive(false);
                edges.SetActive(false);
                recording = true;
                StartCoroutine(Evaluation());
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            StartCoroutine(LoadHomeScene());
        }


        if (Input.GetKeyDown("up") && !isEvaluating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position += currentObject.transform.up * 0.05f;
            }
        }
        if (Input.GetKeyDown("down") && !isEvaluating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position -= currentObject.transform.up * 0.05f;
            }
        }
        if (Input.GetKeyDown("left") && !isEvaluating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position -= currentObject.transform.right * 0.05f;
            }
        }
        if (Input.GetKeyDown("right") && !isEvaluating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position += currentObject.transform.right * 0.05f;
            }
        }
        if (Input.GetKeyDown("w") && !isEvaluating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position += currentObject.transform.forward * 0.05f;
            }
        }
        if (Input.GetKeyDown("s") && !isEvaluating)
        {
            if (currentObject != null)
            {
                currentObject.transform.position -= currentObject.transform.forward * 0.05f;
            }
        }
        if (Input.GetKeyDown("a") && !isEvaluating)
        {
            if (currentObject != null)
            {
                currentObject.transform.Rotate(0.0f, -30.0f, 0.0f, Space.World);
            }
        }
        if (Input.GetKeyDown("d") && !isEvaluating)
        {
            if (currentObject != null)
            {
                currentObject.transform.Rotate(0.0f, 30.0f, 0.0f, Space.World);
            }
        }
    }


    public void StartEvaluation()
    {
        isEvaluating = false;

        if (edges != null)
        {
            edges.SetActive(false);
            currentObject.SetActive(false);
        }
        currentObject = worldStabilized;
        grid = worldStabilized.transform.Find("Positions").gameObject;
        edges = worldStabilized.transform.Find("Edges").gameObject;
        edges.SetActive(true);
        currentObject.SetActive(true);
        gridTransforms.Clear();
        foreach (Transform child in grid.transform)
        {
            gridTransforms.Add(child);
        }

        startIndex = 0;
        start = gridTransforms[startIndex];
        transform.position = start.position;
        GetComponent<Renderer>().enabled = true;
        isReady = true;
        Debug.Log("isReady" + isReady);

    }

    IEnumerator Evaluation()
    {
        leftHandController.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRInteractorLineVisual>().enabled = false;
        rightHandController.GetComponent<UnityEngine.XR.Interaction.Toolkit.XRInteractorLineVisual>().enabled = false;

        isEvaluating = true;
        end = null;
        filename = "worldStabilized_bodyConstrained_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
        frameNumber = 0;
        cameraRig.GetComponent<CameraController>().AddHeader();
        chooseNewPath();
        transform.position = start.position;
        countdownText.SetActive(true);
        for (int i = 3; i > 0; i--)
        {
            countdownText.GetComponent<TextMesh>().text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        countdownText.SetActive(false);

        for (int i = 0; i < 12; i++)
        {
            float timeElapsed = 0.0f;
            while (timeElapsed < pathTime || transform.position != end.position)
            {
                movement = "moving";
                transform.position = Vector3.Lerp(start.position, end.position, Mathf.Min(timeElapsed / pathTime, 1));
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            movement = "static";
            chooseNewPath();
            yield return new WaitForSeconds(1.5f);
        }
        GetComponent<Renderer>().enabled = false;
        recording = false;
        cameraRig.GetComponent<CameraController>().SaveFile(filename);
        isReady = false;
        isEvaluating = false;
        countdownText.GetComponent<TextMesh>().text = "Done";
        countdownText.SetActive(true);
        yield return new WaitForSeconds(3);
        countdownText.SetActive(false);
        transform.gameObject.SetActive(false);
    }

    void chooseNewPath()
    {
        if (end != null)
        {
            start = end;
            startIndex = endIndex;
            endIndex = nextPos[startIndex];
            end = gridTransforms[endIndex];
        }
        else
        {
            startIndex = 0;
            endIndex = nextPos[startIndex];
            start = gridTransforms[startIndex];
            end = gridTransforms[endIndex];
        }
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
