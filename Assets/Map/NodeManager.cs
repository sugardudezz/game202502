using System;
using UnityEngine;

namespace Map
{
    public class NodeManager : MonoBehaviour
    {
        public void InitializeMap()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.currentLevel = 0;
            }
            SetValues();
            SetStates();
        }

        [ContextMenu("Set Values")]
        public void SetValues()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                LevelNode child = transform.GetChild(i).gameObject.GetComponent<LevelNode>();
                child.level = i + 1 - (int)Math.Floor((i + 1) / 4.0);
                switch (i + 1)
                {
                    case 15:
                        child.type = LevelNode.NodeType.Explore;
                        break;
                    case 16:
                        child.type = LevelNode.NodeType.Boss;
                        child.level += 1;
                        break;
                    default:
                        child.type = ((i + 1) % 4 == 0) ? LevelNode.NodeType.Explore : LevelNode.NodeType.Fight;
                        break;
                }

                child.name = $"{child.level}_{child.type}";
                child.state = (i == 0) ? LevelNode.NodeState.Available : LevelNode.NodeState.Unlocked;
            }
        }

        private void Start()
        {
            if (GameManager.Instance.currentLevel <= 0)
            {
                InitializeMap();
                GameManager.Instance.InitializePlayer();
            }

            SetValues();
            SetStates();
            GameManager.OnSceneChange += SceneChanged;
        }

        private void OnDestroy()
        {
            GameManager.OnSceneChange -= SceneChanged;
        }

        private void SceneChanged(string sceneName)
        {
            if (sceneName != "Map") return;
            SetStates();
        }

        private void SetStates()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                LevelNode child = transform.GetChild(i).gameObject.GetComponent<LevelNode>();
                
                if (child.level < GameManager.Instance.currentLevel)
                {
                    child.state = LevelNode.NodeState.Completed;
                } 
                else if (child.level == GameManager.Instance.currentLevel)
                {
                    child.state = LevelNode.NodeState.Current;
                }
                else if (child.level == GameManager.Instance.currentLevel + 1)
                {
                    child.state = LevelNode.NodeState.Available;
                }
                else
                {
                    child.state = LevelNode.NodeState.Unlocked;
                }
            }
        }
    }  
}
