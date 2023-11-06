using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 51)]
public class Dialogue : ScriptableObject {
    [TextArea(3, 10)]
    public string[] lines;
    public Sprite speakerSprite;
}
