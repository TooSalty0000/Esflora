using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public int fieldIndex = 0;
    [SerializeField]
    private GameObject seedPrefab;
    [SerializeField]
    private GameObject seedlingPrefab;
    [SerializeField]
    private GameObject cropInfoUI;
    [SerializeField]
    private CropInfo nullCrop;
    private int index;

    private GameObject cropObject;
    private Crop crop;
    private Seed seed;
    private bool isPlanted = false;

    private GameObject[] seedTracker;
    private GameObject[] seedlingTracker;

    private List<SeedInfo> possibleSeeds = new List<SeedInfo>();
    private FieldManager fieldManager;

    private List<float> energyAmplifier = new List<float>();
    private List<float> energyUseAmplifier = new List<float>();

    private Field[] nearbyFields;

    // Start is called before the first frame update
    
    private int seedCount = 16;

    void Start()
    {
        fieldManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<FieldManager>();
        seed = new Seed();
        nearbyFields = calculateNearbyFields();
    }

    public void resetActivity() {
        if (!isPlanted) {
            return;
        }
        possibleSeeds.Clear();
        SeedInfo[] allSeeds = fieldManager.getSeedList();
        foreach (SeedInfo seedInfo in allSeeds) {
            possibleSeeds.Add(seedInfo);
        }
        energyAmplifier.Clear();
        energyUseAmplifier.Clear();
    }
    public void sleepActivity() {
        if (!isPlanted) {
            return;
        }
        if (crop)
            crop.sleepActivity();
    }

    public void lateSleepActivity() {
        if (!isPlanted) {
            return;
        }
        if (crop)
            crop.lateSleepActivity();
    }

    public void effectActivity() {
        if (!isPlanted) {
            return;
        }
        if (crop)
            crop.effectActivity();
    }

    public void cropCalculation() {
        if (!isPlanted) {
            return;
        }
        if (crop) {
            crop.sleepCalculation(this);
        }
    }

    public void growthActivity() {
        if (isPlanted && crop == null) {
            seed.turns++;
            if (seed.turns == 1) {
                // for each seed, instantiate seedling at the seed's position and destroy the seed
                seedlingTracker = new GameObject[seedCount];
                for (int i = 0; i < seedCount; i++) {
                    if (Random.Range(0f, 1f) < i * 0.1f) {
                        Vector3 seedlingPosition = new Vector3(seedTracker[i].transform.position.x, seedTracker[i].transform.position.y - .1f, seedTracker[i].transform.position.z);
                        seedlingTracker[i] = Instantiate(seedlingPrefab, seedlingPosition, Quaternion.identity);
                        Destroy(seedTracker[i]);
                    } else {
                        Destroy(seedTracker[i]);
                    }
                }
                // set seed from possibleSeeds
                if (seed.seed.cropInfo.cropName == "???") {
                    seed.seed = possibleSeeds[Random.Range(0, possibleSeeds.Count)];
                }
            }
            if (seed.seed.growthTurns == seed.turns) {
                // destroy all seedlings and instantiate crop
                for (int i = 0; i < seedlingTracker.Length; i++) {
                    Destroy(seedlingTracker[i]);
                }
                cropObject = Instantiate(seed.seed.cropInfo.prefab, transform.position, Quaternion.identity);
                crop = cropObject.GetComponent<Crop>();
                crop.setField(this);
                cropInfoUI.SetActive(true);
                cropInfoUI.GetComponent<CropInfoUI>().setCropInfo(crop);
                cropInfoUI.SetActive(false);
            }
        }
    }

    public void showCropInfo() {
        cropInfoUI.SetActive(true);
        if (crop) {
            cropInfoUI.GetComponent<CropInfoUI>().setCropInfo(crop);
        } else {
            cropInfoUI.GetComponent<CropInfoUI>().setCropInfo(nullCrop);
        }
    }

    public void hideCropInfo() {
        cropInfoUI.SetActive(false);
    }

    public CropInfo GetCropInfo()
    {
        if (isPlanted) {
            if (crop) {
                return crop.cropInfo;
            }
            return nullCrop;
        }
        return null;
    }

    public bool GetIsPlanted()
    {
        return isPlanted;
    }

    public void plantSeedAtField () {
        isPlanted = true;
        seedTracker = new GameObject[seedCount];
        // instantiate seed at random position in -0.5 to 0.5
        for (int i = 0; i < seedCount; i++) {
            Vector3 seedPosition = new Vector3(transform.position.x + Random.Range(-0.4f, 0.4f), transform.position.y + 2, transform.position.z + Random.Range(-0.4f, 0.4f));
            seedTracker[i] = Instantiate(seedPrefab, seedPosition, Quaternion.identity);
        }
        seed.seed = ScriptableObject.CreateInstance("SeedInfo") as SeedInfo;
        seed.seed.cropInfo = nullCrop;
        seed.turns = 0;
        cropInfoUI.SetActive(true);
        cropInfoUI.GetComponent<CropInfoUI>().setCropInfo(nullCrop);
        cropInfoUI.SetActive(false);
    }

    public void plantSeedAtField (SeedInfo seedInfo) {
        isPlanted = true;
        seedTracker = new GameObject[seedCount];
        // instantiate seed at random position in -0.5 to 0.5
        for (int i = 0; i < seedCount; i++) {
            Vector3 seedPosition = new Vector3(transform.position.x + Random.Range(-0.4f, 0.4f), transform.position.y + 2, transform.position.z + Random.Range(-0.4f, 0.4f));
            seedTracker[i] = Instantiate(seedPrefab, seedPosition, Quaternion.identity);
        }
        seed.seed = ScriptableObject.CreateInstance("SeedInfo") as SeedInfo;
        seed.seed = seedInfo;
        seed.turns = 0;
        cropInfoUI.SetActive(true);
        cropInfoUI.GetComponent<CropInfoUI>().setCropInfo(nullCrop);
        cropInfoUI.SetActive(false);
    }

    public int getEnergyRequired() {
        if (isPlanted) {
            if (crop && crop.decayed) {
                return crop.totalEnergyUse;
            }
        }
        return 0;
    }

    public int getEnergyReturn() {
        if (isPlanted) {
            if (crop && !crop.decayed) {
                return crop.totalEnergy;
            }
        }
        return 0;
    }

    public Crop getCrop() {
        return crop;
    }

    public void setCrop(Crop _crop) {
        isPlanted = true;
        cropObject = Instantiate(_crop.cropInfo.prefab, transform.position, Quaternion.identity);
        crop = cropObject.GetComponent<Crop>();
        crop.decayed = _crop.decayed;
        crop.turns = _crop.turns;
        if (crop.decayed) {
            MeshRenderer[] meshRenderers = crop.transform.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in meshRenderers) {
                meshRenderer.material.color = Color.black;
                meshRenderer.material.SetColor("_EmissionColor", Color.black);
            }
        }
        crop.setField(this);
        cropInfoUI.SetActive(true);
        cropInfoUI.GetComponent<CropInfoUI>().setCropInfo(crop);
        cropInfoUI.SetActive(false);

    }

    public void removeCrop() {
        if (isPlanted) {
            if (crop) {
                crop = null;
                Destroy(cropObject);
                cropObject = null;
            } else {
                if (seedTracker != null) {
                    for (int i = 0; i < seedTracker.Length; i++) {
                        if (seedTracker[i]) {
                            Destroy(seedTracker[i]);
                        }
                    }
                }
                if (seedlingTracker != null) {
                    for (int i = 0; i < seedlingTracker.Length; i++) {
                        if (seedlingTracker[i]) {
                            Destroy(seedlingTracker[i]);
                        }
                    }
                }
                
            }
            cropInfoUI.SetActive(true);
            cropInfoUI.GetComponent<CropInfoUI>().setCropInfo(nullCrop);
            cropInfoUI.SetActive(false);
            isPlanted = false;
        }
    }

    public void addEnergyAmplifier(float _energyAmplifier) {
        energyAmplifier.Add(_energyAmplifier);
    }

    public void addEnergyUseAmplifier(float _energyUseAmplifier) {
        energyUseAmplifier.Add(_energyUseAmplifier);
    }

    public Field[] getNearbyFields() {
        return nearbyFields;
    }

    public Field[] calculateNearbyFields() {
        Field[] nearbyFields = new Field[8];
        int x = (int)transform.position.x;
        int z = (int)transform.position.z;
        nearbyFields[0] = fieldManager.getFieldAt(x + 1, z);
        nearbyFields[1] = fieldManager.getFieldAt(x + 1, z + 1);
        nearbyFields[2] = fieldManager.getFieldAt(x, z + 1);
        nearbyFields[3] = fieldManager.getFieldAt(x - 1, z + 1);
        nearbyFields[4] = fieldManager.getFieldAt(x - 1, z);
        nearbyFields[5] = fieldManager.getFieldAt(x - 1, z - 1);
        nearbyFields[6] = fieldManager.getFieldAt(x, z - 1);
        nearbyFields[7] = fieldManager.getFieldAt(x + 1, z - 1);
        return nearbyFields;
    }

    public float[] getEnergyAmplifier() {
        float[] _energyAmplifier = new float[energyAmplifier.Count];
        for (int i = 0; i < energyAmplifier.Count; i++) {
            _energyAmplifier[i] = energyAmplifier[i];
        }
        return _energyAmplifier;
    }

    public float[] getEnergyUseAmplifier() {
        float[] _energyUseAmplifier = new float[energyUseAmplifier.Count];
        for (int i = 0; i < energyUseAmplifier.Count; i++) {
            _energyUseAmplifier[i] = energyUseAmplifier[i];
        }
        return _energyUseAmplifier;
    }
}
