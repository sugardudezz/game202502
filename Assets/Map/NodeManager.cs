using UnityEngine;

namespace Scenes.Map
{
    public class NodeManager : MonoBehaviour
    {
        [ContextMenu("Set Values")]
        public void SetValues()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<LevelNode>().SetValues();
            }
        }
    }
}
