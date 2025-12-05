using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObject/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int ID;
    public Sprite Icon;
    public string Name;
    public int initialMHP;
    public int initialMSP;
    public int initialATK;
    public int initialDEF;
    public List<PlayerActionData> actionDataList;
}
