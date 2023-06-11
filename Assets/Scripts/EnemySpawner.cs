using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;     //�� ������
    [SerializeField]                    
    private int createNum;            //�� ���� ��
    [SerializeField]
    private float spawnTime;            //�� ���� �ֱ�
    [SerializeField]
    private Transform[] wayPoints;      // ���� ���������� �̵� ���
    [SerializeField]
    private Transform aggregationPoint; 

    private List<Enemy> enemyList;// ���� �ʿ� �����ϴ� ��� ���� ����

    //���� ������ ������ EnemySpawner���� �ϱ⶧���� set�� �ʿ����
    public List<Enemy> EnemyList=>enemyList;

    private GameObject[] enemys;

    private void Awake()
    {
        enemyList= new List<Enemy>();
        enemys =new GameObject[createNum];
        Vector3 position = aggregationPoint.position;
        for (int i = 0; i < createNum; i++)
        {
            enemys[i] = Instantiate(enemyPrefab, transform);
            enemys[i].transform.position = position;
        }
        //�� ���� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine("SpawnEnemy");
    }
    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            //GameObject clone = Instantiate(enemyPrefab);        //�� ������Ʈ ����
            Enemy readyEnemy = enemys.Select(e => e.GetComponent<Enemy>()).FirstOrDefault(e => e != null && e.IsReady);
            if (readyEnemy == null) // readyEnemy �� null �� �ƴ϶�� �غ�� ���� ���°� ���
            {
                break;
            }
            GameObject clone = readyEnemy.gameObject;

            Enemy enemy =clone.GetComponent<Enemy>();            //��� ������ ���� Enemy ������Ʈ

            enemy.Setup(this,wayPoints, aggregationPoint);                             //wayPoint ������ �Ű������� Setup() ȣ��
            enemyList.Add(enemy);

            yield return new WaitForSeconds(spawnTime);         //spawnTime �ð� ���� ���
        }
    }
    public void DestroyEnemy(Enemy enemy)
    {
        // ����Ʈ���� ����ϴ� �� ���� ����
        enemyList.Remove(enemy);
        //�� ������Ʈ ����
        enemy.OnDie();
    }

}
