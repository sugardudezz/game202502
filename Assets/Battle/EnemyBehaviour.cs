using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Enemy Behaviour")]
    public EnemyImageChanger[] behaviour;

    [Header("External Data")]
    public GameObject enemyObject;
    private EnemyManager enemyData;

    void Awake()
    {
        // Enemy 데이터 스크립트 참조
        if (enemyObject != null)
        {
            enemyData = enemyObject.GetComponent<EnemyManager>();
        }
    }

    private void InitializeDObjectsFromEnemy()
    {
        if (enemyData == null)
        {
            Debug.LogError("Enemy 스크립트(EnemyData)를 찾을 수 없습니다. Enemy GameObject를 연결했는지 확인하세요.");
            return;
        }

        int darknessCount = enemyData.darkness;

        // 배열 길이 초과 방지
        if (darknessCount > behaviour.Length)
        {
            darknessCount = behaviour.Length;
        }

        // darknessCount만큼의 오브젝트에 어둠 그림 적용
        for (int i = 0; i < darknessCount; i++)
        {
            if (i < behaviour.Length && behaviour[i] != null)
            {
                behaviour[i].ChangeImage(3);
            }
        }
    }

    void Start()
    {
        // 씬 시작 시 초기화 로직 실행
        InitializeDObjectsFromEnemy();
    }

    void Update()
    {
        // 예시 - Q 키를 입력 시 적 행동 오브젝트의 이미지를 공격으로 변경
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (behaviour[0] != null)
            {
                behaviour[0].ChangeImage(0);
            }
        }

        // 예시2 - W 키를 입력 시 적 행동 오브젝트의 이미지를 잠식으로 변경
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (behaviour[3] != null)
            {
                behaviour[3].ChangeImage(2);
            }
        }
    }
}