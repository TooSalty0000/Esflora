using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public CropInfo cropInfo;
    public int turns;
    public int sellPrice;
    public bool decayed = false;
    public int totalEnergy = 0;
    public int totalEnergyUse = 0;
    public Field field;

    public PlayerController playerController;
    // Start is called before the first frame update
    void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerController>();
        sellPrice = cropInfo.price;
        turns = 1;
    }

    public virtual void sleepActivity() {
        turns++;
        if (turns > cropInfo.decayTurns) {
            decayed = true;
            // get all mesh renderers and set the material color to black
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in meshRenderers) {
                meshRenderer.material.color = Color.black;
                meshRenderer.material.SetColor("_EmissionColor", Color.black);
            }
            sellPrice /= 2;
        }
    }

    public virtual void lateSleepActivity() {
    }

    public virtual void sleepCalculation(Field field) {
        totalEnergy = cropInfo.energy;
        foreach (var amp in field.getEnergyAmplifier()) {
            totalEnergy = Mathf.FloorToInt(totalEnergy * amp);
        }
        totalEnergyUse = cropInfo.energyUsage;
        foreach (var amp in field.getEnergyUseAmplifier()) {
            totalEnergyUse = Mathf.FloorToInt(totalEnergyUse * amp);
        }
    } 

    public void setField(Field field) {
        this.field = field;
    }

    public virtual void effectActivity() {
        
    }

    public void revive() {
        turns = 0;
        decayed = false;
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < meshRenderers.Length; i++) {
            meshRenderers[i].material.color = Color.white;
        }
    }
}
