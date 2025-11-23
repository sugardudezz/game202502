using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Map
{
    public class MapPlayer : MonoBehaviour
    {
        Vector3 endPos;
        Vector3 direction;
        
        private bool moving;
        private LevelNode currentNode;

        void Start()
        {
            moving = false;
            transform.position = (GameManager.instance.currentLevel != 0) ? currentNode.transform.position : new Vector3(-6, -1, 0);
            LevelNode.OnLevelClick += EnterLevel;
        }

        private void OnDestroy()
        {
            LevelNode.OnLevelClick -= EnterLevel;
        }

        private IEnumerator _MovePlayer(Vector3 targetPos, string targetScene)
        {
            endPos = targetPos;
            direction = (endPos - transform.position).normalized;
            while (Vector3.Distance(this.transform.position, endPos) > 0.1f)
            {
                transform.Translate(direction * (Time.deltaTime * 10f));
                yield return null;
            }

            moving = false;
            GameManager.instance.currentLevel++;
            GameManager.instance.ChangeScene(targetScene);
        }

        private void EnterLevel(LevelNode node)
        {
            if (moving) return; // 이미 이동 중일 시 노드 클릭 무시
            moving = true;
            if (node.state == LevelNode.NodeState.Available)
            {
                Vector3 targetPos = node.transform.position;
                String targetScene;
                switch(node.type)
                {
                    case LevelNode.NodeType.Fight:
                        targetScene = "Battle";
                        break;
                    case LevelNode.NodeType.Boss:
                        targetScene = "Battle";
                        break;
                    case LevelNode.NodeType.Explore:
                        targetScene = "Main";
                        break;
                    default:
                        targetScene = "Main";
                        break;
                }
                currentNode = node;
                StartCoroutine(_MovePlayer(targetPos, targetScene));
            }
        }
    }
}
