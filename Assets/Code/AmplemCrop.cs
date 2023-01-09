using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmplemCrop : Crop
{
    public override void lateSleepActivity() {
        Field[] nearbyFields = field.getNearbyFields();
        foreach (Field nearbyField in nearbyFields) {
            if (nearbyField && nearbyField.getCrop() && nearbyField.getCrop().turns > 0 && nearbyField.getCrop().cropInfo.cropName != "Amplem") {
                nearbyField.getCrop().turns--;
            }
        }
    }
}
