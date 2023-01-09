using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExloseCrop : Crop
{

    public override void sleepCalculation(Field field) {
        totalEnergy = (turns - 2 == cropInfo.decayTurns) ? 30 : cropInfo.energy;
        foreach (var amp in field.getEnergyAmplifier()) {
            totalEnergy = Mathf.FloorToInt(totalEnergy * amp);
        }
        totalEnergyUse = cropInfo.energyUsage;
        foreach (var amp in field.getEnergyUseAmplifier()) {
            totalEnergyUse = Mathf.FloorToInt(totalEnergyUse * amp);
        }
    }
}
