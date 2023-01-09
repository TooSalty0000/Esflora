using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Seed", menuName = "Seed", order = 0)]
public class SeedInfo : ScriptableObject
{
    public CropInfo cropInfo;
    public int growthTurns = 1;
}
