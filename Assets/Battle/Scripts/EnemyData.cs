using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObject/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int ID;
    public Sprite Icon;
    public string Name;
    public int initialMHP;
    public int initialMSP;
    public int initialATK;
    public int initialDEF;
    public List<EnemyActionData> actionDataList;
}
