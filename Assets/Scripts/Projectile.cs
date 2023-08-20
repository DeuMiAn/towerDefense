using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjecctileType { Bullet = 0, ExplosiveBomb }
public class Projectile : MonoBehaviour
{
    private Movement2D movement2D;
    private Enemy target;
    private float damage;
    private float splash_range;  // ���÷��� ����
    private float splash_damage; // ���÷��� ���ݷ�

    [SerializeField] private ProjecctileType type;

    public void Setup(Enemy target,float damage=0, float splash_damage=0, float splash_range=0)
    {
        movement2D=GetComponent<Movement2D>();
        this.target = target;
        this.damage = damage;
        this.splash_damage= splash_damage;
        this.splash_range= splash_range;
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
        if (type== ProjecctileType.ExplosiveBomb)
        {
           ParticleSystem particleSystem = GetComponentInChildren<ParticleSystem>();
           Explosive explosive = GetComponentInChildren<Explosive>();

           print(splash_range);
           particleSystem.transform.localScale =new Vector3(splash_range, splash_range, splash_range);
           gameObject.GetComponentInChildren<ParticleSystem>().Play();

            explosive.onSensorOn();

        }
        else
        {
            Destroy(gameObject); //�߻�ü ������Ʈ ����

        }
        collision.GetComponent<EnemyHP>().TakeDamage(damage);//�� ü���� damage��ŭ ����
        //collision.GetComponent<Enemy>().OnDie(); //�� ��� �Լ� ȣ��
    }
}
