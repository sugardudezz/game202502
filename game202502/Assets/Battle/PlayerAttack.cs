using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public HealthBarManager EnemyHealthBar;

    public float attackDamage = 0; // 피해량

    public void AttackPlayer()
    {
        if (EnemyHealthBar != null)
        {
            EnemyHealthBar.Damage(attackDamage);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) // 테스트용 - A 키 입력시 플레이어가 피해량 만큼 공격
        {
            attackDamage = 25f;
            AttackPlayer();
        }
    }
}