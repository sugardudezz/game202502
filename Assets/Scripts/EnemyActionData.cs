using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAction", menuName = "ScriptableObject/EnemyAction")]
public class EnemyActionData : ScriptableObject
{
    public int ID;
    public bool isEnhanced;
    public Sprite actionIcon;
    public string actionName;
    public string actionDesc;
}
