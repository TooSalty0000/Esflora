using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinEffect : MonoBehaviour
{
    [SerializeField]
    private float spinSpeed = 10f;
    private float offset = 0f;

    private void Start() {
        offset = Random.Range(0f, 360f);
        transform.Rotate(Vector3.up, offset);
    }
    
    private void Update() {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }
}
