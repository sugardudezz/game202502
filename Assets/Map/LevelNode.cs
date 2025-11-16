using System;
using UnityEngine;

public class LevelNode : MonoBehaviour
{
    public enum NodeType
    {
        Fight,
        Explore,
        Boss,
    }
    
    public int level;
    public NodeType type;
    
    public void SetValues()
    {
        Transform parent = transform.parent;
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i) == this.transform)
            {
                level = i + 1;
                switch (level)
                {
                    case 15:
                        type = NodeType.Explore;
                        break;
                    case 16:
                        type = NodeType.Boss;
                        break;
                    default:
                        type = (level % 4 == 0) ? NodeType.Explore : NodeType.Fight;
                        break;
                }
            }
        }
    }

    private void OnMouseUpAsButton()
    {
        GameManager.instance.mapPlayer.EnterLevel(this);
    }
}
