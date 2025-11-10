using System.Collections;
using UnityEngine;

public class MapPlayer : MonoBehaviour
{
    Vector3 endPos;
    Vector3 direction;

    void Start()
    {
        GameManager.instance.mapPlayer = this;
    }

    IEnumerator _MovePlayer(Vector3 targetPos)
    {
        endPos = targetPos;
        direction = (endPos - transform.position).normalized;
        while (Vector3.Distance(this.transform.position, endPos) > 0.1f)
        {
            transform.Translate(direction * (Time.deltaTime * 10f));
            yield return null;
        }
        //GameManager.instance.ChangeScene("fight");
    }

    public void EnterLevel(LevelNode level)
    {
        print("aaa");
        Vector3 targetPos = level.transform.position;
        StartCoroutine(_MovePlayer(targetPos));
        print(level.level + level.type);
    }
}
