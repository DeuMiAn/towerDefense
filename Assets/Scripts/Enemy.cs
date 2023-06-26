using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum EnemyDestroyType { Kill=0,Arrive}
public class Enemy : MonoBehaviour
{
    private int wayPointCount;             //�̵� ��� ����
    private Transform[] wayPoints;         //�̵� ��� ����
    private Transform aggregationPoint;         //���� ��ġ ����

    private int currentIndex = 0;          //���� ��ǥ���� ���ؽ�
    private Movement2D movement2D;         //������Ʈ �̵� ����
    private bool isReady=true;         //������Ʈ �̵� ����
    [SerializeField]
    private bool isDie = false;         //������Ʈ �̵� ����
    private EnemySpawner enemySpawner; // ���� ������������ ���� �ʰ� EnemySpawner�� �˷��� ������
    [SerializeField]
    private int gold = 10;              //�� ��� �� ȹ�� ������ ���


    public void Setup( EnemySpawner enemySpawner, Transform[] wayPoints, Transform aggregationPoint)
    {
        isReady = false;
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;

        // �� �̵� ��� Waypoints ���� ����
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount]; //Transform wayPointCount��ŭ �迭ũ�����
        this.wayPoints = wayPoints;
        this.aggregationPoint = aggregationPoint;

        //���� ��ġ�� ù��° wayPoint ��ġ�� ����
        transform.position= wayPoints[currentIndex].position;

        //�� �̵�/��ǥ���� ���� �ڷ�ƾ �Լ�����
        StartCoroutine("OnMove");

    }
    private IEnumerator OnMove()
    {
        //���� �̵� ���� ����
        NextMoveTo();

        while (!isReady)
        {
            //�� ������Ʈ ȸ��
            transform.Rotate(Vector3.forward * 10);

            //���� ������ġ�� ��ǥ��ġ�� �Ÿ��� 0.02 *moveMent2DMoveSpeed���� ������ if ���ǹ� ����
            //Tip movement2D.MoveSpeed�� �����ִ� ������ �ӵ��� ������ �� �����ӿ� 0.02���� ũ�� �����̱� ������
            // if ���ǹ��� �ɸ��� �ʰ� ��θ� Ż���ϴ� ������Ʈ�� �߻��Ҽ� ����
            if(Vector3.Distance(transform.position, wayPoints[currentIndex].position)<0.06f*movement2D.MoveSpeed)
            {
                //���� �̵� ���� ����
                NextMoveTo();
            }

            yield return null;
        }
    }

    private void NextMoveTo()
    {
        if (isReady) return;
        //���� �̵��� wayPoints�� ���� �ִٸ�
        if(currentIndex < wayPointCount-1) {
            //���� ��ġ�� ��Ȯ�ϰ� ��ǥ ��ġ�� ����
            transform.position = wayPoints[currentIndex].position;
            //�̵� ���� ���� => ������ǥ���� (wayPoints)
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position-transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else
        {
            //��ǥ������ �����ؼ� ����ҋ��� ������
            gold = 0;
            //�� ������Ʈ ����
            // Destroy(gameObject);
            OnDie(EnemyDestroyType.Arrive);
        }
    }
    public bool IsReady => isReady;
    public bool IsDie => isDie;


    public void OnDie(EnemyDestroyType type)
    {
        isReady = false;
        isDie = true;
        currentIndex = 0;
        movement2D.setPosition(aggregationPoint.position);
        enemySpawner.DestroyEnemy(type,this,gold);
    }


}
