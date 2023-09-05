using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D explosiveSensor;
    [SerializeField]
    private GameObject explosiveEffect;

    private float splash_damage=0;
    private float splash_range = 0;


    public void onExplosive(float splash_damage,float splash_range)
    {
        print("폭발작동");
        this.splash_damage = splash_damage;
        this.splash_range = splash_range;
        GameObject clone = Instantiate(explosiveEffect, gameObject.transform.position, Quaternion.identity);
        ParticleSystem particleSystem = clone.GetComponent<ParticleSystem>();
        particleSystem.transform.localScale = new Vector3(splash_range, splash_range, splash_range);
        particleSystem.Play();
        explosiveSensor.radius = splash_range / 2;
        explosiveSensor.enabled = true;
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
        {
            return;
        }
        /*Destroy(gameObject);*/
        collision.GetComponent<EnemyHP>().TakeDamage(splash_damage);//적 체력을 damage만큼 감소
    }
  

}
