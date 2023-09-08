using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWithMouse : MonoBehaviour
{
    private float rotationMouseX = 0f;
    private float rotationMouseY = 0f;

    public float senesitivity = 15f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotationMouseX += Input.GetAxis("Mouse X") * senesitivity;
        rotationMouseY += Input.GetAxis("Mouse Y") * senesitivity * -1;
        transform.localEulerAngles = new Vector3(rotationMouseY, rotationMouseX, 0);
    }
}
