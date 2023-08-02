using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRoomController : MonoBehaviour
{
    public GameObject debugText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            transform.position += transform.up * 0.05f;
        }
        if (Input.GetKeyDown("k"))
        {
            transform.position -= transform.up * 0.05f;
        }
        if (Input.GetKeyDown("j"))
        {
            transform.position -= transform.right * 0.05f;
        }
        if (Input.GetKeyDown("l"))
        {
            transform.position += transform.right * 0.05f;
        }
        if (Input.GetKeyDown("v"))
        {
            transform.position += transform.forward * 0.05f;
        }
        if (Input.GetKeyDown("b"))
        {
            transform.position -= transform.forward * 0.05f; 
        }
        if (Input.GetKeyDown("n"))
        {
            transform.Rotate(0.0f, -0.5f, 0.0f, Space.World);
        }
        if (Input.GetKeyDown("m"))
        {
            transform.Rotate(0.0f, 0.5f, 0.0f, Space.World);
        }
        
        if(Input.GetKeyDown("p"))
        {
            debugText.GetComponent<TextMesh>().text = "position: " + transform.position + "\n rotation: " + transform.eulerAngles;
        }
    }

}
