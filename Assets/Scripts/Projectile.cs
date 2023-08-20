using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjecctileType { Bullet = 0, ExplosiveBomb }
public class Projectile : MonoBehaviour
{
    private Movement2D movement2D;
    private Enemy target;
    private float damage;
    private float splash_range;  // 스플래시 범위
    private float splash_damage; // 스플래시 공격력

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
            //발사체를 tartget의 위치로 이동
            Vector3 direction =(target.transform.position-transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else //여러 이유로 tartget이 사라지면
        {
            // 발사체 오브젝트 삭제
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return; //적이 아닌 대상과 부딪히면
        if (collision.transform != target.transform) return; //현재 tartget인 적이 아닐 때
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
            Destroy(gameObject); //발사체 오브젝트 삭제

        }
        collision.GetComponent<EnemyHP>().TakeDamage(damage);//적 체력을 damage만큼 감소
        //collision.GetComponent<Enemy>().OnDie(); //적 사망 함수 호출
    }
}
