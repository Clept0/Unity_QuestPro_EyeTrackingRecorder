using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MovingSphere : MonoBehaviour
{
    public GameObject HeadStabilized;
    public GameObject countdownText;

    private GameObject currentObject;
    private GameObject grid;
    private GameObject edges;

    private List<Transform> gridTransforms = new List<Transform>();
    private Transform start = null;    
    private Transform end = null;
    private int startIndex;
    private int endIndex;
    private List<string> log = new List<string>();

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
        StartEvaluation();
        grid.SetActive(false);
        edges.SetActive(true); 
        StartCoroutine(Evaluation());

    }


    // Update is called once per frame
    void Update()
    {
       
    }

    // void OnEnable()
    // {
    //     countdownText.SetActive(true);
    //     StartEvaluation();
    // }

    // void OnDisable()
    // {
    //     countdownText.SetActive(false);
    //     GetComponent<Renderer>().enabled = false;
       
    //     edges.SetActive(false);
    //     currentObject.SetActive(false);
    // }

    public void StartEvaluation()
    {
        Debug.Log("Head Stabilized");
        if (edges != null)
        {
            edges.SetActive(false);
            currentObject.SetActive(false);
        }
        currentObject = HeadStabilized;
        grid = HeadStabilized.transform.Find("Positions").gameObject;
        edges = HeadStabilized.transform.Find("Edges").gameObject;
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

    }

    IEnumerator Evaluation()
    {
        end = null;
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
                transform.position = Vector3.Lerp(start.position, end.position, Mathf.Min(timeElapsed / pathTime, 1));
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            chooseNewPath();
            yield return new WaitForSeconds(1.5f);
        }
        GetComponent<Renderer>().enabled = false;
        countdownText.GetComponent<TextMesh>().text = "Done";
        countdownText.SetActive(true);
        yield return new WaitForSeconds(1);
        countdownText.SetActive(false);
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
}
