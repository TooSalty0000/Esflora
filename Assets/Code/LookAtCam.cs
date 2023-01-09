using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    private Transform cam;
    [SerializeField]
    private bool flip = false;
    private void Awake() {
        cam = Camera.main.transform;
        transform.LookAt(transform.position + cam.forward);
        // transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);
        if (flip) {
            transform.Rotate(0, 180, 0);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.LookAt(transform.position + cam.forward);
        // transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);
        if (flip) {
            transform.Rotate(0, 180, 0);
        }
        
    }
}
