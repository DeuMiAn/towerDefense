using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;     //적 프리팹
    [SerializeField]
    private GameObject enemyHPSliderPrefab;     //적 체력을 나타내는 Slider UI 프리팹
    [SerializeField]
    private Transform canvasTransform;     //UI를 표현하는 Canvas 오브젝트의 Transform
    [SerializeField]                    
    private int createNum;            //적 생성 수
    [SerializeField]
    private float spawnTime;            //적 생성 주기
    [SerializeField]
    private Transform[] wayPoints;      // 현재 스테이지의 이동 경로
    [SerializeField]
    private Transform aggregationPoint; 

    private List<Enemy> enemyList;// 현재 맵에 존재하는 모든 적의 정보

    //적의 생성과 삭제는 EnemySpawner에서 하기때문에 set은 필요없음
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
        //적 생성 코루틴 함수 호출
        StartCoroutine("SpawnEnemy");
    }
    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            //GameObject clone = Instantiate(enemyPrefab);        //적 오브젝트 생성
            Enemy readyEnemy = enemys.Select(e => e.GetComponent<Enemy>()).FirstOrDefault(e => e != null && e.IsReady);
            if (readyEnemy == null) // readyEnemy 가 null 이 아니라면 준비된 적이 없는것 대기
            {
                break;
            }
            GameObject clone = readyEnemy.gameObject;

            Enemy enemy =clone.GetComponent<Enemy>();            //방금 생성된 적의 Enemy 컴포넌트

            enemy.Setup(this,wayPoints, aggregationPoint);                             //wayPoint 정보를 매개변수로 Setup() 호출
            enemyList.Add(enemy);

            SpawnEnemyHPSlider(clone);                               // 적 체력을 나타내는 Slider UI 생성 및 설정

            yield return new WaitForSeconds(spawnTime);         //spawnTime 시간 동안 대기
        }
    }
    public void DestroyEnemy(Enemy enemy)
    {
        // 리스트에서 사망하는 적 정보 삭제
        enemyList.Remove(enemy);
        //적 오브젝트 삭제
        //enemy.OnDie();
    }
    private void SpawnEnemyHPSlider(GameObject enemy)
    {
        //적 체력을 나타내는 Slider UI생성
        GameObject sliderClone = Instantiate(enemyHPSliderPrefab);
        //Slider UI 오브젝트를 parent("Canvas" 오브젝트)의 자식으로 설정
        //UI 는 캔버스의 자식오브젝트로 설정되어 있어야 화면에 보인다 
        sliderClone.transform.SetParent(canvasTransform);
        //계층 설정으로 바뀐 크기를 다시(1,1,1)fh tjfwjd
        sliderClone.transform.localScale=Vector3.one;

        //Slider UI가 쫓아다닐 대상을 본인으로 설정
        sliderClone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.GetComponent<Enemy>());
        // Slider UI에 자신의 체력 정보를 표시하도록 설정
        sliderClone.GetComponent<EnemyHPViewer>().Setup(enemy.GetComponent<EnemyHP>());
    }
}
