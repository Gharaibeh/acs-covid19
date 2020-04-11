using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("LoadNext", 3);
        
    }

    // Update is called once per frame
    void LoadNext()
    {
        Application.LoadLevel(1);
    }
}
