using UnityEngine;

[CreateAssetMenu(fileName = "New Crop", menuName = "Farm/CropData")]
public class CropData : ScriptableObject
{
    public string cropName;
    public GameObject cropPrefab;
    public float growTime = 1f;
    public Sprite fullGrownSprite;
    public Sprite midGrowSprite;
}
