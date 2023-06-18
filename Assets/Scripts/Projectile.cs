using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Movement2D movement2D;
    private Enemy target;

    public void Setup(Enemy target)
    {
        movement2D=GetComponent<Movement2D>();
        this.target = target;
    }
    // Update is called once per frame
    void Update()
    {
        if (!target.IsDie)
        {
            //�߻�ü�� tartget�� ��ġ�� �̵�
            Vector3 direction =(target.transform.position-transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else //���� ������ tartget�� �������
        {
            // �߻�ü ������Ʈ ����
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return; //���� �ƴ� ���� �ε�����
        if (collision.transform != target.transform) return; //���� tartget�� ���� �ƴ� ��
        collision.GetComponent<Enemy>().OnDie(); //�� ��� �Լ� ȣ��
        Destroy(gameObject); //�߻�ü ������Ʈ ����
    }
}
