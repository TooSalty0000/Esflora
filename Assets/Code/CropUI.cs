 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CropUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI cropName;
    [SerializeField]
    private TextMeshProUGUI cropDescription;
    [SerializeField]
    private TextMeshProUGUI cropEnergy;
    [SerializeField]
    private TextMeshProUGUI cropEnergyUsage;
    [SerializeField]
    private TextMeshProUGUI cropPrice;
    [SerializeField]
    private TextMeshProUGUI harvestorCount;
    [SerializeField]
    private TextMeshProUGUI turns;

    public static CropUI instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    public void updateCropUI(CropInfo cropInfo) {
        if (!cropInfo) {
            Debug.LogError("CropInfo is null");
            return;
        }
        cropName.text = cropInfo.cropName;
        cropDescription.text = cropInfo.description;
        cropEnergy.text = "+Energy:" + cropInfo.energy.ToString();
        cropEnergyUsage.text = "-Energy:" + cropInfo.energyUsage.ToString();
        cropPrice.text = "Price:" + cropInfo.price.ToString();
    }
}
