using System;
using UnityEngine;

namespace Map
{
    public class LevelNode : MonoBehaviour
    {
        public static event Action<LevelNode> OnLevelClick;
        
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
                        sprite.color = new Color(1f, 1f, 1f, 0.5f);
                        break;
                    case NodeState.Current:
                        break;
                    case NodeState.Available:
                        sprite.enabled = true;
                        sprite.color = Color.white;
                        break;
                }
            }
        }

        private void OnMouseUpAsButton()
        {
            if (state == NodeState.Available)
            {
                OnLevelClick?.Invoke(this);
            }
        }
    }
}
