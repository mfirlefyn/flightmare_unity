using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    // Find cube in scene that is representing the nest
    public GameObject cube;
    
    private bool cubeVisible = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // toggle cube with "h"
        if (Input.GetKeyDown(KeyCode.H)) {
            cubeVisible = !cubeVisible;
            //cube.gameObject.SetActive(cubeVisible);
            cube.GetComponent<Renderer>().enabled = cubeVisible;
            Debug.Log("cube visibility: " + cubeVisible);
            Debug.Log("cube: " + cube);
        }

        // take screenshot with "c"
        if (Input.GetKeyDown(KeyCode.C)) {
            ScreenCapture.CaptureScreenshot("scene.png",4);
        }
    }
}
