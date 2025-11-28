using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObject/EffectData")]
public class EffectData : ScriptableObject
{
    public int ID;
    public Sprite effectIcon;
}
