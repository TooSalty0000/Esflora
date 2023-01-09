using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crop", menuName = "Crop", order = 0)]
public class CropInfo : ScriptableObject
{
    public string cropName = "crop";
    [TextArea(3, 10)]
    public string description = "Some kind of descriptoin";
    public GameObject prefab;
    public int decayTurns = 3;
    public int energy = 2;
    public int energyUsage = 1;
    public int price = 10;
}
