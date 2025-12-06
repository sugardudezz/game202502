using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    GameManager.PlayerInfo player;

    public List<Sprite> backgroundList;
    public GameObject background;

    private Vector3 originalScale;
    public Vector3 hoverScale;

    // Start에서 원래 크기를 저장합니다.
    void Start()
    {
        player = GameManager.Instance.player;

        background.GetComponent<SpriteRenderer>().sprite = backgroundList[(GameManager.Instance.currentLevel + 1) / 4];
    }

    public void Rest()
    {
        player.baseMHP += 15;
        player.MHP = player.baseMHP;
        player.CHP = player.MHP;
        ChangeScene("Map");
    }

    public void Upgrade()
    {
        player.baseATK += 2;
        ChangeScene("Map");
    }

    // 마우스 진입 시 호출
    public void SetHoverScale(GameObject target)
    {
        originalScale = target.transform.localScale;
        target.transform.localScale = hoverScale;
    }

    // 마우스 이탈 시 호출
    public void SetOriginalScale(GameObject target)
    {
        target.transform.localScale = originalScale;
    }

    public void ChangeScene(string sceneName)
    {
        GameManager.Instance.ChangeScene(sceneName);
    }
}
