using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplodCrop : Crop
{
    public override void sleepActivity() {
        base.sleepActivity();
        if (decayed) {
            Field[] nearbyFields = field.getNearbyFields();
            foreach (Field nearbyField in nearbyFields) {
                if (nearbyField && nearbyField.GetIsPlanted()) {
                    FieldManager.instance.harvestField(nearbyField);
                }
            }
            FieldManager.instance.harvestField(field);
        }
    }
}
