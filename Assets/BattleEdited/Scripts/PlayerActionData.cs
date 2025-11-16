using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAction", menuName = "ScriptableObject/PlayerAction")]
public class PlayerActionData : ScriptableObject
{
    public int ID;
    public bool isEnhanced;
    public Sprite actionIcon;
    public string actionName;
    public string actionDesc;
}
