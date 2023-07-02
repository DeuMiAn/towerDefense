using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static WaveSystem;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField]
    //private GameObject enemyPrefab;     //�� ������
    [SerializeField]
    private GameObject enemyHPSliderPrefab;     //�� ü���� ��Ÿ���� Slider UI ������
    [SerializeField]
    private Transform canvasTransform;     //UI�� ǥ���ϴ� Canvas ������Ʈ�� Transform
    //[SerializeField]                    
    //private int createNum;            //�� ���� ��
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
    private Wave currentWave;           //���� ���̺� ����(�� ������, �� �����ֱ� ��������)
    private int currentEnemyCount;        // ���� ���̺� �� ���� (���̺� ���۽� max -> ����� -1)
    private List<Enemy> enemyList;// ���� �ʿ� �����ϴ� ��� ���� ����

    //���� ������ ������ EnemySpawner���� �ϱ⶧���� set�� �ʿ����
    public List<Enemy> EnemyList=>enemyList;


    //���� ���̺��� ���� �ִ� ��, �ִ� �� ����
    public int CurrentEnemyCount => currentEnemyCount;
    public int MaxEnemyCount => currentWave.maxEnemyCount;

    private GameObject[] enemys;

    private void Awake()
    {
        enemyList= new List<Enemy>();
        /*enemys =new GameObject[createNum];
        Vector3 position = aggregationPoint.position;
        for (int i = 0; i < createNum; i++)
        {
            enemys[i] = Instantiate(enemyPrefab, transform);
            enemys[i].transform.position = position;
        }
        //�� ���� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine("SpawnEnemy");*/
    }
    public void StartWave(Wave wave)
    {
        //�Ű������� �޾ƿ� ���̺� ���� ����
        currentWave = wave;
        // ���� ���̺��� �ִ� �� ���ڸ� ����
        currentEnemyCount = currentWave.maxEnemyCount;
        //���� ���̺� ����
        StartCoroutine("SpawnEnemy");
    }
    private IEnumerator SpawnEnemy()
    {
        //���� ���̺꿡�� ������ �� ����
        int spawnEnemyCount = 0;

        enemys = new GameObject[currentWave.maxEnemyCount];
        Vector3 position = aggregationPoint.position;
        for (int i = 0; i < currentWave.maxEnemyCount; i++)
        {
            // ���̺꿡 �����ϴ� ���� ������ ���� ������ �� ������ ���� �����ϵ��� �����ϰ�, �� ������Ʈ ����

            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            //�� ������Ʈ ����
            enemys[i] = Instantiate(currentWave.enemyPrefabs[enemyIndex], transform);
            enemys[i].transform.position = position;
        }
        //while (true)
        //���� ���̺꿡�� �����Ǿ�� �ϴ� ���� ���ڸ�ŭ ���� �����ϰ� �ڷ�ƾ ����
        while (spawnEnemyCount< currentWave.maxEnemyCount)
        {
            //GameObject clone = Instantiate(enemyPrefab);       
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

            //���� ���̺꿡�� ������ ���� ���� +1
            spawnEnemyCount++;

            //yield return new WaitForSeconds(spawnTime);         //spawnTime �ð� ���� ���
            // �� ���̺긶�� spawnTime�� �ٸ� �� �ֱ� ������ ���� ���̺�(currentWave)�� spawnTime ���
            yield return new WaitForSeconds(currentWave.spawnTime);
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
        //���� ����� �� ���� ���� ���̺��� ���� �� ���� ���� (UIǥ�ÿ�)
        currentEnemyCount--;
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
