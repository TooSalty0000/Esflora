using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraikeCrop : Crop
{
    public int energyAbsorbed = 0;
    public override void lateSleepActivity() {
        Field[] nearbyFields = field.getNearbyFields();
        foreach (Field nearbyField in nearbyFields) {
            if (nearbyField && nearbyField.getCrop() && nearbyField.getCrop().decayed) {
                FieldManager.instance.harvestField(nearbyField);
                energyAbsorbed += 1;
            }
        }
    }

    public override void sleepCalculation(Field field)
    {
        totalEnergy = cropInfo.energy - energyAbsorbed;
        foreach (var amp in field.getEnergyAmplifier()) {
            totalEnergy = Mathf.FloorToInt(totalEnergy * amp);
        }
    }
}
