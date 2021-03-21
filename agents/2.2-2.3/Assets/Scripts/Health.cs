using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;    
    }

    private void Update()
    {
        transform.LookAt(cam.transform.position + cam.transform.forward);
    }
}
