using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObject/EnemyData")]
public class EnemyData : ScriptableObject
{
    public int ID;
    public Sprite enemyIcon;
    public string enemyName;
    public int baseHP;
    public int baseSP;
    public int baseATK;
    public int baseDEF;

    public List<EnemyActionData> actionDataList;
}
