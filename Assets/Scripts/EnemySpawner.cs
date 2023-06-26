using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;     //�� ������
    [SerializeField]
    private GameObject enemyHPSliderPrefab;     //�� ü���� ��Ÿ���� Slider UI ������
    [SerializeField]
    private Transform canvasTransform;     //UI�� ǥ���ϴ� Canvas ������Ʈ�� Transform
    [SerializeField]                    
    private int createNum;            //�� ���� ��
    [SerializeField]
    private float spawnTime;            //�� ���� �ֱ�
    [SerializeField]
    private Transform[] wayPoints;      // ���� ���������� �̵� ���
    [SerializeField]
    private Transform aggregationPoint;
    [SerializeField]
    private PlayerHP playerHP;
    [SerializeField]
    private PlayerGold playerGold;        //�÷��̾� ��� ������Ʈ

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

            SpawnEnemyHPSlider(clone);                               // �� ü���� ��Ÿ���� Slider UI ���� �� ����

            yield return new WaitForSeconds(spawnTime);         //spawnTime �ð� ���� ���
        }
    }
    public void DestroyEnemy(EnemyDestroyType type ,Enemy enemy, int gold)
    {
        //���� ��ǥ�������� �����ϸ�
        if (type == EnemyDestroyType.Arrive)
        {
            //�÷��̾� ü��-1
            playerHP.TakeDamage(1);
        }
        //���� �÷��̾��� �߻�ü���� ���������
        else if(type==EnemyDestroyType.Kill)
        {
            //���� ������ ���� ����� ��� ȹ��
            playerGold.CurrentGold += gold;
        }
        // ����Ʈ���� ����ϴ� �� ���� ����
        enemyList.Remove(enemy);
        //�� ������Ʈ ����
        //enemy.OnDie();
    }
    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        //�� ü���� ��Ÿ���� Slider UI����
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);
        //Slider UI ������Ʈ�� parent("Canvas" ������Ʈ)�� �ڽ����� ����
        //UI �� ĵ������ �ڽĿ�����Ʈ�� �����Ǿ� �־�� ȭ�鿡 ���δ� 
        sliderClone.transform.SetParent(canvasTransform);
        //���� �������� �ٲ� ũ�⸦ �ٽ�(1,1,1)fh tjfwjd
        sliderClone.transform.localScale=Vector3.one;

        //Slider UI�� �Ѿƴٴ� ����� �������� ����
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.GetComponent<Enemy>());
        // Slider UI�� �ڽ��� ü�� ������ ǥ���ϵ��� ����
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }
}
