using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleRandomCrop : MonoBehaviour
{
    [SerializeField]
    private GameObject[] allCrops;
    // Start is called before the first frame update
    void Start()
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++) {
            GameObject crop = Instantiate(allCrops[Random.Range(0, allCrops.Length)], transform.GetChild(i).position, Quaternion.identity, transform.GetChild(i));
            if (crop.GetComponent<Crop>()) {
                Destroy(crop.GetComponent<Crop>());
            }
        }
    }
}
