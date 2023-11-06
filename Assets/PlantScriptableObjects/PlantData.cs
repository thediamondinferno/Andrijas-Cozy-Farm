using UnityEngine;

// Create a new menu option to create plant data assets
[CreateAssetMenu(fileName = "New Plant", menuName = "Farming/Plant")]
public class PlantData : ScriptableObject
{
    public Sprite seedSprite;
    public Sprite grownSprite;
    public Sprite deadSprite;
    public float growthTime;
    public float deadTime;
    public int baseMoney;
}
