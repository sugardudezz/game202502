using System;
using System.Collections;
using UnityEngine;

namespace Map
{
    public class MapPlayer : MonoBehaviour
    {
        private Vector3 endPos;
        private Vector3 direction;
        
        private bool moving;
        private static Vector3 lastPos = new (-6, -1, 0);

        private void Start()
        {
            transform.position = lastPos;
            moving = false;
            LevelNode.OnLevelEnter += EnterLevel;
            Debug.Log("star");
        }

        private void OnDestroy()
        {
            LevelNode.OnLevelEnter -= EnterLevel;
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
            GameManager.Instance.currentLevel++;
            GameManager.Instance.ChangeScene(targetScene);
        }

        private void EnterLevel(LevelNode node)
        {
            if (node.state != LevelNode.NodeState.Available) return;
            if (moving) return; // 이미 이동 중일 시 노드 클릭 무시
            moving = true;
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
                    Debug.Log("This node type has undefined scene target!");
                    return;
            }
            StartCoroutine(_MovePlayer(targetPos, targetScene));
            lastPos = targetPos;
        }
        /*
        private void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * (Time.deltaTime * 10f));
            } 
            else if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * (Time.deltaTime * 10f));
            }
        }
        */
    }
}
