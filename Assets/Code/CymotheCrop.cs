using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CymotheCrop : Crop
{
    int lifeCount = 0;

    public override void sleepActivity()
    {
        base.sleepActivity();
        if (lifeCount > 5) {
            FieldManager.instance.harvestField(field);
        }
    }
    public override void effectActivity() {
        if (decayed) {
            Field[] nearbyFields = field.getNearbyFields();
            foreach (Field nearbyField in nearbyFields) {
                if (nearbyField)
                    nearbyField.addEnergyAmplifier(2f);
            }
            lifeCount++;
        } else {
            Field[] nearbyFields = field.getNearbyFields();
            foreach (Field nearbyField in nearbyFields) {
                if (nearbyField)
                    nearbyField.addEnergyAmplifier(0.5f);
            }
        }
    }
}
