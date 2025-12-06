using System;
using Map.UI;
using UnityEngine;

namespace Map
{
    public class LevelNode : MonoBehaviour
    {
        public static event Action<LevelNode> OnLevelEnter;
        public static event Action<LevelNode> OnNodeClick;

        private static LevelNode selectedNode;

        public enum NodeType
        {
            Fight,
            Explore,
            Boss,
        }
    
        public int level;

        public NodeType type;

        public enum NodeState
        {
            Completed,
            Current,
            Available,
            Unlocked,
        }
        
        [SerializeField]
        private NodeState _state;

        public NodeState state
        {
            get => _state;
            set
            {
                SpriteRenderer sprite = GetComponent<SpriteRenderer>();
                _state = value;
                switch (value)
                {
                    case NodeState.Unlocked: 
                        sprite.color = new Color(1, 1, 1, 0f);
                        break;
                    case NodeState.Completed:
                        sprite.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                        break;
                    case NodeState.Current:
                        sprite.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                        break;
                    case NodeState.Available:
                        sprite.enabled = true;
                        sprite.color = Color.white;
                        break;
                }
            }
        }

        private void Start()
        {
            UIController.OnStartClick += StartClick;
        }

        private void OnDestroy()
        {
            UIController.OnStartClick -= StartClick;
        }

        private void StartClick()
        {
            if (selectedNode != this) return;

            OnLevelEnter?.Invoke(this);
        }

        private void OnMouseUpAsButton()
        {
            if (state == NodeState.Available)
            {
                selectedNode = this;

                OnNodeClick?.Invoke(this);
            }
        }
    }
}
