using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum WeaponState {SearchTarget =0, AttackToTarget}

public class TowerWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilPrefab;          //�߻�ü ������
    [SerializeField]
    private Transform spawnPoint;               // �߻�ü ���� ��ġ
    [SerializeField]
    private float attackRate = 0.5f;            //���� �ӵ�
    [SerializeField]
    private float attackRange = 2.0f;           //���ݹ���
    [SerializeField]
    private float attackDamage = 1f;           //���ݷ�
    private int level = 0;                                          //Ÿ������
    private WeaponState weaponState = WeaponState.SearchTarget;     //Ÿ�� ������ ����
    private Enemy attackTarget = null;                          //���� ���
    private EnemySpawner enemySpawner;                              // ���ӿ� �����ϴ� �� ���� ȹ���

    public float Damage => attackDamage;
    public float Rate => attackRate;
    public float Range => attackRange;
    public int Level => level + 1;

    public void Setup(EnemySpawner enemySpawner)
    {
        this.enemySpawner = enemySpawner;
        //���� ���¸� WeaponState.SerchTarget���� ����
        ChangeState(WeaponState.SearchTarget);
    }

    public void ChangeState(WeaponState newState)
    {
        //������ ������̴� ���� ����
        StopCoroutine(weaponState.ToString());
        //���� ����
        weaponState = newState;
        // ���ο� ���� ���
        StartCoroutine(weaponState.ToString() );
    }

    // Update is called once per frame
    private void Update()
    {
        if(attackTarget != null&& attackTarget.IsDie==false)
        {
            RotateToTarget();
        }
        
    }

    private void RotateToTarget()
    {
        //�������κ����� �Ÿ��� ���������κ����� ������ �̿��� ��ġ�� ���ϴ� �� ��ǥ�� �̿�
        // ����=arctan(y/x)
        // x,y ������ ���ϱ�
        float dx= attackTarget.transform.position.x-transform.position.x;
        float dy= attackTarget.transform.position.y-transform.position.y;
        //x,y �������� �������� ���� ���ϱ�
        // ������ radian �����̱� ������ Mathf.Rad2Deg�� ���� �� ������ ����
        float degree = Mathf.Atan2(dy,dx)*Mathf.Rad2Deg;
        transform.rotation=Quaternion.Euler(0,0,degree);
    }
    private IEnumerator SearchTarget()
    {
        while (true)
        {
            //���� ������ �ִ� ���� ã�� ���� ���� �Ÿ��� �ִ��� ũ�� ����
            float closestDistSqr = Mathf.Infinity;
            // EnemySpawner�� EnemyList�� �ִ� ���� �ʿ� �����ϴ� ��� �� �˻�
            for (int i = 0;i<enemySpawner.EnemyList.Count;i++)
            {
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
                // ���� �˻����� ������ �Ÿ��� ���ݹ��� ���� �ְ�, ������� �˻��� ������ �Ÿ��� ������
                if (distance <= attackRange && distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    attackTarget = enemySpawner.EnemyList[i];
                }

            }
            if(attackTarget!=null && attackTarget.IsDie == false)
            {
                ChangeState(WeaponState.AttackToTarget);
            }
            yield return null;
             
        }
    }

    private IEnumerator AttackToTarget()
    {
        while(true)
        {
            //1. tartget�� �ִ��� �˻� (�ٸ� �߻�ü�� ���� ����, Goal �������� �̵��� ������)
            if (attackTarget == null|| attackTarget.IsDie)
            {
                ChangeState(WeaponState.SearchTarget); break;
            }
            //2. target�� ���� ���� �ȿ� �ִ��� �˻�(���� ������ ����� ���ο� �� Ž��)
            float distance =Vector3.Distance(attackTarget.transform.position, transform.position);
            if (distance > attackRange)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget); break;
            }

            //3. attackRage �ð���ŭ ���
            yield return new WaitForSeconds(attackRate);
            //4. ���� (�߻�ü ����)
            SpawnProjectile();
        }
    }

    private void SpawnProjectile()
    {
        GameObject clone=Instantiate(projectilPrefab,spawnPoint.position,Quaternion.identity);
        clone.GetComponent<Projectile>().Setup(attackTarget,attackDamage);
    }
}
