using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(LoadCalibrationScene());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(LoadScreenStabilizedScene());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(LoadWorldStabilizedScene());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartCoroutine(LoadScreenStabilizedWalkingScene());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            StartCoroutine(LoadWorldStabilizedWalkingScene());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            StartCoroutine(LoadWorldStabilizedSphereScene());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            StartCoroutine(LoadWorldStabilizedVRSphereScene());
        }
        else {
            Debug.Log("invalid scene number");
        }
    }

    IEnumerator LoadCalibrationScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Calibration_Arrow");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadScreenStabilizedScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ScreenStabilized_Arrow");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadWorldStabilizedScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("WorldStabilized_Arrow");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadScreenStabilizedWalkingScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("ScreenStabilized_Arrow_Walking");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadWorldStabilizedWalkingScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("WorldStabilized_Arrow_Walking");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadWorldStabilizedSphereScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("WorldStabilized_Sphere");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadWorldStabilizedVRSphereScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("WorldStabilized_Sphere_VR");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
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
