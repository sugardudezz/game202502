using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Effect : MonoBehaviour
{
    public int ID;
    public Image effectIcon;
    public TextMeshProUGUI effectSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(EffectData data)
    {
        ID = data.ID;
        effectIcon.sprite = data.EffectIcon;
        effectSize.text = data.EffectSize.ToString();
    }
}
