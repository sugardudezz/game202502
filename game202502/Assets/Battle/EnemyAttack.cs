using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public HealthBarManager playerHealthBar;

    public float attackDamage = 0; // 피해량

    public void AttackPlayer()
    {
        if (playerHealthBar != null)
        {
            playerHealthBar.Damage(attackDamage);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 테스트용 - space 키 입력시 피해량 만큼 적이 공격
        {
            attackDamage = 25f;
            AttackPlayer();
        }
    }
}