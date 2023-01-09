using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FieldManager : MonoBehaviour
{
    public static FieldManager instance;

    [SerializeField]
    private Field[] fields;
    private List<Field> emptyFields = new List<Field>();
    private List<Field> plantedFields = new List<Field>();

    // Seed List
    [SerializeField]
    private SeedInfo[] seedList;

    [SerializeField]
    private TextMeshProUGUI cropName;
    [SerializeField]
    private TextMeshProUGUI cropDescription;

    private PlayerController playerController;

    private int energyRequired;
    // Start is called before the first frame update

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
        foreach (var field in fields)
        {
            field.fieldIndex = System.Array.IndexOf(fields, field);
        }
        emptyFields = new List<Field>(fields);
        plantedFields = new List<Field>();
    }

    private void Start() {
        playerController = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerController>();
    }
    
    public void sleepActivities() {
        playerController.gainEnergy(getTotalEnergy());
        foreach (Field field in fields) {
            field.resetActivity();
        }
        foreach (Field field in fields) {
            field.sleepActivity();
        }
        foreach (Field field in fields) {
            field.growthActivity();
        }
        foreach (Field field in fields) {
            field.lateSleepActivity();
        }
        foreach (Field field in fields) {
            field.effectActivity();
        }
        foreach (Field field in fields) {
            field.cropCalculation();
        }
    }

    public int getEnergyRequired() {
        energyRequired = 0;
        foreach (Field field in fields) {
            energyRequired += field.getEnergyRequired();
        }
        return energyRequired;
    }

    public int getEnergyReturn() {
        int energyReturn = 0;
        foreach (Field field in fields) {
            energyReturn += field.getEnergyReturn();
        }
        return energyReturn;
    }

    public int getTotalEnergy() {
        return getEnergyReturn() - getEnergyRequired();
    }

    public SeedInfo[] getSeedList() {
        return seedList;
    }

    public void updateUI() {
        Field field = playerController.getSelectedField();
        if (field) {
            cropName.text = field.GetCropInfo().cropName;
            cropDescription.text = field.GetCropInfo().description;
        } else {
            cropName.text = "No Crop";
            cropDescription.text = "No Crop";
        }
    }

    public void moveCrop(Field previousField, Field newField) {
        newField.setCrop(previousField.getCrop());
        previousField.removeCrop();
        emptyFields.Add(previousField);
        plantedFields.Remove(previousField);
        emptyFields.Remove(newField);
        plantedFields.Add(newField);
    }

    public void plantSeedAt(Field field) {
        field.plantSeedAtField();
        emptyFields.Remove(field);
        plantedFields.Add(field);
        onFieldChange();
    }

    public void plantSeedAt(Field field, SeedInfo seedInfo) {
        field.plantSeedAtField(seedInfo);
        emptyFields.Remove(field);
        plantedFields.Add(field);
        onFieldChange();
    }

    public void harvestField(Field field) {
        field.removeCrop();
        plantedFields.Remove(field);
        emptyFields.Add(field);
        onFieldChange();
    }

    public Field getFieldAt(int x, int z) {
        foreach (var field in fields)
        {
            if (field.transform.position.x == x && field.transform.position.z == z)
            {
                return field;
            }
        }
        return null;
    }

    public void onFieldChange() {
        foreach (Field field in fields) {
            field.resetActivity();
        }
        foreach (Field field in fields) {
            field.effectActivity();
        }
        foreach (Field field in fields) {
            field.cropCalculation();
        }
    }

    public void plantSeedAtRandomField() {
        // plant seed at random field, repeat random from 1 to 3 times until success
        int random = Random.Range(1, 4);
        for (int i = 0; i < random; i++) {
            if (emptyFields.Count > 0) {
                int index = Random.Range(0, emptyFields.Count);
                Field field = emptyFields[index];
                plantSeedAt(field);
            }
        }
        onFieldChange();
    }

}
