using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObject/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int ID;
    public Sprite playerIcon;
    public string playerName;
    public int baseHP;
    public int baseSP;
    public int baseATK;
    public int baseDEF;

    public List<PlayerActionData> actionDataList;
}
