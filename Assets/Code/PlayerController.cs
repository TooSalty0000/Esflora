using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField]
    private int coins = 0;
    [SerializeField]
    private int energyCount = 0;

    
    private Field selectedField;
    private Field previousField;
    private turnStages currentStage;
    private int energyUsage;
    [SerializeField]
    private int dayCount = 0;
    private int seedPrice = 2;
    private int harvesterCount = 3;
    private int energyEndCost = 30;
    private int energyEndCostIncrease = 30;

    private bool plantingMode = false;
    private bool movingMode = false;
    private bool gameOver = false;

    private FieldManager fieldManager;
    private TutorialManager tutorialManager;

    

    //UI
    [Header("UI")]
    [SerializeField]
    private GameObject plantNotification;
    [SerializeField]
    private GameObject moveNotification;
    [SerializeField]
    private GameObject seedPurchaseUI;
    [SerializeField]
    private GameObject fieldCropUI;
    [SerializeField]
    private DayPassAnimation dayPassAnimation;

    // Start is called before the first frame update

    // Player Stats UI
    [Header("Player Stats UI")]
    [SerializeField]
    private TextMeshProUGUI dayCountText;
    [SerializeField]
    private TextMeshProUGUI energyCountText;
    [SerializeField]
    private TextMeshProUGUI coinsCountText;
    [SerializeField]
    private TextMeshProUGUI seedPriceText;
    [SerializeField]
    private TextMeshProUGUI harvesterCountText;
    [SerializeField]
    private TextMeshProUGUI energyEndCostText;

    void Start() {
        if (GetComponent<TutorialManager>())
            tutorialManager = GetComponent<TutorialManager>();
        fieldManager = GetComponent<FieldManager>();
        mainCamera = Camera.main;
        currentStage = turnStages.sleep;
        dayCount = 0;
        startNextStage();
    }

    // Update is called once per frame
    void Update() {
        if (tutorialManager) {
            if (coins == 0) {
                tutorialManager.getAction("@SPENDALL");
            }
            if (dayCount == 2) {
                tutorialManager.getAction("@NEXTDAY");
            }
        }

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                // dayCountText.text = hit.collider.name;
                if (hit.collider.gameObject.tag == "Field") {
                    if (selectedField) {
                        selectedField.hideCropInfo();
                    }
                    previousField = selectedField;
                    selectedField = hit.collider.gameObject.GetComponent<Field>();
                } else if (hit.collider.gameObject.tag == "Crop") {
                    if (selectedField) {
                        selectedField.hideCropInfo();
                    }
                    previousField = selectedField;
                    selectedField = hit.collider.gameObject.GetComponentInParent<Field>();
                } else if (hit.collider.name != "ClickController") {
                    if (selectedField) {
                        selectedField.hideCropInfo();
                    }
                    selectedField = null;
                } else {
                    previousField = null;
                }
                selectMethod();
            }
        }
        updateUI();
    }

    private void selectMethod () {
        switch (currentStage) {
            case turnStages.start:
                break;
            case turnStages.plant:
                if (selectedField) {
                    if (plantingMode && coins >= seedPrice) {
                        if (!selectedField.GetIsPlanted()) {
                            fieldManager.plantSeedAt(selectedField);
                            AudioManager.instance.playSound("Dig");
                            coins -= seedPrice;
                            selectedField = null;
                            seedPurchaseUI.SetActive(true);
                            fieldCropUI.SetActive(false);
                            if (coins < seedPrice) {
                                plantingMode = false;
                                plantNotification.SetActive(false);
                            }
                        }
                    } else if (movingMode) {
                        if (selectedField && previousField) {
                            if (selectedField.GetIsPlanted()) {
                                selectedField = previousField;
                            } else {
                                if (tutorialManager) {
                                    tutorialManager.getAction("@SELECTNEW");
                                }
                                fieldManager.moveCrop(previousField, selectedField);
                                fieldManager.onFieldChange();
                                harvesterCount--;
                                movingMode = false;
                                selectedField = null;
                                previousField = null;
                                seedPurchaseUI.SetActive(true);
                                fieldCropUI.SetActive(false);
                            }
                        }
                    } else {
                        plantingMode = false;
                        movingMode = false;
                        plantNotification.SetActive(false);
                        moveNotification.SetActive(false);
                        if (selectedField.GetIsPlanted()) {
                            AudioManager.instance.playSound("Rustle");
                            selectedField.showCropInfo();
                            fieldManager.updateUI();
                            seedPurchaseUI.SetActive(false);
                            fieldCropUI.SetActive(true);
                        } else {
                            seedPurchaseUI.SetActive(true);
                            fieldCropUI.SetActive(false);
                        }
                    }
                } else {
                    seedPurchaseUI.SetActive(true);
                    fieldCropUI.SetActive(false);
                }
                break;
            case turnStages.sleep:
                break;
        }
        updateUI();
    }

    private void updateUI() {
        dayCountText.text = dayCount.ToString();
        int totalEnergyDelta = fieldManager.getEnergyReturn() - fieldManager.getEnergyRequired();
        energyCountText.text = energyCount.ToString() + "\n(" + (totalEnergyDelta >= 0 ? "+" : "") + totalEnergyDelta + ")";
        coinsCountText.text = coins.ToString();
        seedPriceText.text = seedPrice.ToString();
        harvesterCountText.text = "x" + harvesterCount.ToString();
        energyEndCostText.text = energyEndCost.ToString();
    }



    private void startNextStage() {
        //go to next stage
        switch (currentStage) {
            case turnStages.start:
                currentStage = turnStages.plant;
                break;
            case turnStages.plant:
                currentStage = turnStages.sleep;
                break;
            case turnStages.sleep:
                currentStage = turnStages.start;
                break;
        }

        switch (currentStage) {
            case turnStages.start:
                if (dayCount == 0) {
                    startGame();
                }
                dayCount++;
                harvesterCount = 3 + Mathf.FloorToInt(dayCount / 10);
                // reset all actions
                plantingMode = false;
                movingMode = false;
                plantNotification.SetActive(false);
                moveNotification.SetActive(false);
                if (selectedField) {
                    selectedField.hideCropInfo();
                }
                selectedField = null;

                if (dayCount > 1) fieldManager.plantSeedAtRandomField();
                // update UI
                updateUI();
                dayPassAnimation.nextDay(dayCount);
                AudioManager.instance.playSound("Bell");
                currentStage = turnStages.plant;
                break;
            case turnStages.plant:
                break;
            case turnStages.sleep:
                fieldManager.sleepActivities();
                updateSeedPrice();
                if (dayCount % 5 == 0) {
                    StopCoroutine(doCalculations());
                    StartCoroutine(doCalculations());
                } else {
                    startNextStage();
                }
                break;
        }
    }

    private void startGame() {

        // start game
        // set energy
        energyCount = 0;
        // set coins
        coins = 10;
        // set seed price
        seedPrice = 2;
        // set harvester count
        harvesterCount = 3;
        // // set min seed price
        // minSeedPrice = 4;
    }


    public void gainEnergy(int energy) {
        this.energyCount += energy;
    }

    public void updateSeedPrice() {
        seedPrice = Random.Range(1, 10);
    }
    
    public void endDayButton() {
        StopCoroutine(endDayAnimation());
        StartCoroutine(endDayAnimation());
    }

    public Field getSelectedField() {
        return selectedField;
    }

    public void harvestCrop() {
        if (selectedField && harvesterCount > 0) {
            if (!selectedField.GetIsPlanted()) {
                return;
            }
            if (selectedField.GetCropInfo().cropName != "???") {
                coins += selectedField.getCrop().sellPrice;
            }
            if (selectedField.GetCropInfo().cropName == "Sticka") {
                Field[] nearbyFields = selectedField.getNearbyFields();
                for (int i = 0; i < nearbyFields.Length; i++) {
                    if (nearbyFields[i] && nearbyFields[i].GetIsPlanted()) {
                        if (nearbyFields[i].GetCropInfo().cropName == "Sticka") {
                            fieldManager.harvestField(nearbyFields[i]);
                        }
                    }
                }
            }
            fieldManager.harvestField(selectedField);
            selectedField = null;
            seedPurchaseUI.SetActive(true);
            fieldCropUI.SetActive(false);
            harvesterCount--;
            updateUI();
        }
    }

    public void setPlantingMode() {
        if (coins < seedPrice) {
            plantingMode = false;
            plantNotification.SetActive(false);
            return;
        }
        if (plantingMode) {
            plantingMode = false;
            plantNotification.SetActive(false);
        } else {
            plantingMode = true;
            plantNotification.SetActive(true);
        }
    }

    public void setMoveMode() {
        if (selectedField) {
            if (selectedField.GetCropInfo().cropName == "???") {
                return;
            }
        }
        if (harvesterCount <= 0) {
            movingMode = false;
            moveNotification.SetActive(false);
            return;
        }
        if (movingMode) {
            movingMode = false;
            moveNotification.SetActive(false);
        } else {
            movingMode = true;
            moveNotification.SetActive(true);
        }
    }

    private IEnumerator endDayAnimation() {
        dayPassAnimation.blackOut();
        yield return new WaitForSeconds(1);
        startNextStage();
    }

    private IEnumerator doCalculations() {
        int energyNeeded = energyEndCost;
        dayPassAnimation.updateNumber(energyCount, energyNeeded);
        dayPassAnimation.startCounting();
        yield return new WaitForSeconds(1);
        int _energyNeeded = energyNeeded;
        AudioManager.instance.playSound("Beep");
        while (energyNeeded != 0 && energyCount != 0) {
            energyCount--;
            energyNeeded--;
            dayPassAnimation.updateNumber(energyCount, energyNeeded);
            yield return new WaitForSeconds(0.000001f);
        }
        yield return new WaitForSeconds(1);
        if (energyNeeded == 0) {
            dayPassAnimation.endCounting();
            yield return new WaitForSeconds(1);
            if (dayCount == 30) {
                dayPassAnimation.victory();
            } else {
                energyEndCost += energyEndCostIncrease;
                energyEndCostIncrease += 20;
                startNextStage();
            }
        } else {
            AudioManager.instance.playSound("Fail", true);
            dayPassAnimation.endGame();
            gameOver = true;
        }
    }

    public void continueAfterVictory() {
        StartCoroutine(continueAfterVictoryCoroutine());
    }

    public IEnumerator continueAfterVictoryCoroutine() {
        dayPassAnimation.victoryEnd();
        yield return new WaitForSeconds(1);
        startNextStage();
    }

    
    public enum turnStages {
        start, // use energy
        plant, // plant things
        sleep, // gain energy
    }
}
