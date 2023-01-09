using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegumintCrop : Crop
{
    public override void lateSleepActivity()
    {
        if (decayed) return;
        Field[] nearbyFields = field.getNearbyFields();
        foreach (Field nearbyField in nearbyFields) {
            if (nearbyField && nearbyField.getCrop()) {
                if (nearbyField.getCrop().decayed) {
                    nearbyField.getCrop().revive();
                }
            }
        }
    }
}
