using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnterinCrop : Crop
{
    private int energyAddon = 0;
    public override void effectActivity()
    {
        energyAddon = 0;
        Field[] nearbyFields = field.getNearbyFields();
        foreach (Field nearbyField in nearbyFields) {
            if (nearbyField)
            {
                if (nearbyField.GetIsPlanted()) {
                    energyAddon += 1;
                }
            }
        }
    }

    public override void sleepCalculation(Field field)
    {
        totalEnergy = cropInfo.energy + energyAddon;
        totalEnergyUse = cropInfo.energyUsage + energyAddon;
        foreach (var amp in field.getEnergyAmplifier()) {
            totalEnergy = Mathf.FloorToInt(totalEnergy * amp);
            totalEnergyUse = Mathf.FloorToInt(totalEnergyUse * amp);
        }
    }
}
