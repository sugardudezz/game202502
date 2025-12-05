using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Map.UI
{
    public class UIController : MonoBehaviour
    {
        public static event Action OnStartClick;
        
        private VisualElement document;
        
        private Button closeButton;
        private Button startButton;

        private Label levelName;
        private Label levelDescription;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            document = GetComponent<UIDocument>().rootVisualElement;
            document.style.display = DisplayStyle.None;
            
            LevelNode.OnNodeClick += OpenTab; 
            
            closeButton = document.Q<Button>("CloseButton");
            closeButton.clicked += () => document.style.display = DisplayStyle.None;
            
            startButton = document.Q<Button>("StartButton");
            startButton.clicked += () =>
            {
                OnStartClick?.Invoke();
                document.style.display = DisplayStyle.None;
            };
            
            levelName = document.Q<Label>("LevelName");
            levelDescription = document.Q<Label>("LevelDescription");
        }

        private void OpenTab(LevelNode node)
        {
            string nodeType;
            switch (node.type)
            {
                case LevelNode.NodeType.Fight:
                    nodeType = "전투";
                    break;
                case LevelNode.NodeType.Explore:
                    nodeType = "정비";
                    break;
                case LevelNode.NodeType.Boss:
                    nodeType = "전투";
                    break;
                default:
                    nodeType = "??";
                    break;
            }
            levelName.text = $"{node.level} - {nodeType}";

            string levelDesc;
            switch (GameManager.Instance.currentLevel)
            {
                case 12:
                    levelDesc = "출현 몬스터: " + "-" + "<br><br>보상: 선택";
                    break;
                case 13:
                    levelDesc = "출현 몬스터: " + GameManager.Instance.enemyDataList[GameManager.Instance.currentLevel].Name;
                    break;
                default:
                    switch (GameManager.Instance.currentLevel % 3)
                    {
                        case 0:
                            levelDesc = "출현 몬스터: " + GameManager.Instance.enemyDataList[GameManager.Instance.currentLevel].Name + "<br><br>보상: 방어력 + 1";
                            break;
                        case 1:
                            levelDesc = "출현 몬스터: " + GameManager.Instance.enemyDataList[GameManager.Instance.currentLevel].Name + "<br><br>보상: 방어력 + 1";
                            break;
                        case 2:
                            if (node.type == LevelNode.NodeType.Fight)
                                levelDesc = "출현 몬스터: " + GameManager.Instance.enemyDataList[GameManager.Instance.currentLevel].Name + "<br><br>보상: 기력 + 1";
                            else
                                levelDesc = "출현 몬스터: " + "-" + "<br><br>보상: 선택";
                            break;
                        default:
                            levelDesc = "";
                            break;
                    }
                    break;
            }
            levelDescription.text = levelDesc;
            
            document.style.display = DisplayStyle.Flex;
        }
    }
}
