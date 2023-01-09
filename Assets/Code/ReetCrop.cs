using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReetCrop : Crop
{
    private int lifeCount = 0;
    
    public override void sleepActivity() {
        base.sleepActivity();
        lifeCount++;
    }

    public override void sleepCalculation(Field field) {
        totalEnergy = cropInfo.energy + lifeCount;
        foreach (var amp in field.getEnergyAmplifier()) {
            totalEnergy = Mathf.FloorToInt(totalEnergy * amp);
        }
        totalEnergyUse = cropInfo.energyUsage;
        foreach (var amp in field.getEnergyUseAmplifier()) {
            totalEnergyUse = Mathf.FloorToInt(totalEnergyUse * amp);
        }
    } 
}
