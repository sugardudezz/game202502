using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour
{
    public int ID;
    public Sprite effectIcon;
    public int effectSize;

    public Image iconUI;
    public TextMeshProUGUI textUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(EffectData data, int size)
    {
        ID = data.ID;
        effectIcon = data.Icon;
        effectSize = size;

        iconUI.sprite = effectIcon;
        textUI.text = "<sub>" + effectSize.ToString() + "</sub>";
    }
}
