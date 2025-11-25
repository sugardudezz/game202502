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
            String nodeType;
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
            
            document.style.display = DisplayStyle.Flex;
        }
    }
}
