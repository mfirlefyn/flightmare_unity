using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class getSize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string path = "Assets/size.txt";
        StreamWriter writer = new StreamWriter(path, true);
        var size = GetComponent<Renderer>().bounds.size;
        writer.WriteLine(size);
        writer.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
