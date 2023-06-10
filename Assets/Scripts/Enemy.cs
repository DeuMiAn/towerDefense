using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int wayPointCount;             //�̵� ��� ����
    private Transform[] wayPoints;         //�̵� ��� ����
    private int currentIndex = 0;          //���� ��ǥ���� ���ؽ�
    private Movement2D movement2D;         //������Ʈ �̵� ����

    public void Setup(Transform[] wayPoints)
    {
        movement2D = GetComponent<Movement2D>();

        // �� �̵� ��� Waypoints ���� ����
        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount]; //Transform wayPointCount��ŭ �迭ũ�����
        this.wayPoints = wayPoints;

        //���� ��ġ�� ù��° wayPoint ��ġ�� ����
        transform.position= wayPoints[currentIndex].position;

        //�� �̵�/��ǥ���� ���� �ڷ�ƾ �Լ�����
        StartCoroutine("OnMove");

    }
    private IEnumerator OnMove()
    {
        //���� �̵� ���� ����
        NextMoveTo();

        while (true)
        {
            //�� ������Ʈ ȸ��
            transform.Rotate(Vector3.forward * 10);

            //���� ������ġ�� ��ǥ��ġ�� �Ÿ��� 0.02 *moveMent2DMoveSpeed���� ������ if ���ǹ� ����
            //Tip movement2D.MoveSpeed�� �����ִ� ������ �ӵ��� ������ �� �����ӿ� 0.02���� ũ�� �����̱� ������
            // if ���ǹ��� �ɸ��� �ʰ� ��θ� Ż���ϴ� ������Ʈ�� �߻��Ҽ� ����
            if(Vector3.Distance(transform.position, wayPoints[currentIndex].position)<0.02f*movement2D.MoveSpeed)
            {
                //���� �̵� ���� ����
                NextMoveTo();
            }

            yield return null;
        }
    }

    private void NextMoveTo()
    {
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
            //�� ������Ʈ ����
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
