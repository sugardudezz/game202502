using UnityEngine;

public class ToggleBehaviour : MonoBehaviour
{
    public GameObject Behaviour_Choice;

    public void Toggle_Behaviour()
    {
        if (Behaviour_Choice != null)
        {
            // 행동 선택창 활성화
            Behaviour_Choice.SetActive(!Behaviour_Choice.activeSelf);
        }
    }
}