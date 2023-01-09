using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoorshCrop : Crop
{
    public override void effectActivity()
    {
        Field[] nearbyFields = field.getNearbyFields();
        //if there are no nearby fields with crops, addEnergyAmplifier(2f) to self
        foreach (Field nearbyField in nearbyFields) {
            if (nearbyField)
            {
                if (nearbyField.GetIsPlanted())
                    return;
            }
        }
        field.addEnergyAmplifier(2f);
    }
}
