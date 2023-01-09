using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlousCrop : Crop
{
    [SerializeField]
    private SeedInfo EsfloraSeed;
    public override void sleepActivity()
    {
        base.sleepActivity();
        if (!decayed) {
            List<Field> nearbyFields = new List<Field>(field.getNearbyFields());
            while (nearbyFields.Count > 0) {
                int index = Random.Range(0, nearbyFields.Count);
                Field nearbyField = nearbyFields[index];
                nearbyFields.RemoveAt(index);
                if (nearbyField && !nearbyField.GetIsPlanted()) {
                    FieldManager.instance.plantSeedAt(nearbyField, EsfloraSeed);
                    break;
                }
            }
        }
    }
}
