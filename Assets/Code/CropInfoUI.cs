using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CropInfoUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI cropEnergy;
    [SerializeField]
    private TextMeshProUGUI cropEnergyUsage;
    [SerializeField]
    private TextMeshProUGUI cropPrice;
    [SerializeField]
    private TextMeshProUGUI decayTurns;

    public void setCropInfo(Crop _crop) {
        if (_crop.totalEnergy == -999) {
            cropEnergy.text = "???";
        } else {
            cropEnergy.text = _crop.totalEnergy.ToString();
        }
        if (_crop.totalEnergyUse == -999) {
            cropEnergyUsage.text = "???";
        } else {
            cropEnergyUsage.text = _crop.totalEnergyUse.ToString();
        }
        if (_crop.sellPrice == -999) {
            cropPrice.text = "???";
        } else {
            cropPrice.text = _crop.sellPrice.ToString();
        }
        if (_crop.cropInfo.decayTurns == -999) {
            decayTurns.text = "???";
        } else {
            decayTurns.text = _crop.cropInfo.decayTurns.ToString();
        }
    }

    public void setCropInfo(CropInfo _crop) {
        if (_crop.energy == -999) {
            cropEnergy.text = "???";
        } else {
            cropEnergy.text = _crop.energy.ToString();
        }
        if (_crop.energyUsage == -999) {
            cropEnergyUsage.text = "???";
        } else {
            cropEnergyUsage.text = _crop.energyUsage.ToString();
        }
        if (_crop.price == -999) {
            cropPrice.text = "???";
        } else {
            cropPrice.text = _crop.price.ToString();
        }
        if (_crop.decayTurns == -999) {
            decayTurns.text = "???";
        } else {
            decayTurns.text = _crop.decayTurns.ToString();
        }
    }

}
