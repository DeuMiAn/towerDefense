using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum WeaponType {Cannon =0,Laser,Slow,Buff,}
public enum WeaponState { SearchTarget = 0, TryAttackCannon,TryAttackLaser  }


public class TowerWeapon : MonoBehaviour
{
    //Header(string) Inspector View�� ǥ�õǴ� �������� �뵵���� �����ϱ����� ���
    //string�� �ۼ��� ������ ���� �۾��� ǥ�õ�
    [Header("Commons")]
    [SerializeField]
    private TowerTemplate towerTemplate;        // Ÿ������(����,���� ���)
    [SerializeField]
    private Transform spawnPoint;               // �߻�ü ������ġ
    [SerializeField]
    private WeaponType weaponType;              //���� �Ӽ� ����

    [Header("Cannon")]
    [SerializeField]
    private GameObject projectilPrefab;        //�߻�ü ������

    [Header("Laser")]
    [SerializeField]
    private LineRenderer lineRenderer;        //������ ��
    [SerializeField]
    private Transform hitEffect;            //Ÿ��ȿ��
    [SerializeField]
    private LayerMask targetLayer;              //������ �ε����� ���̾� ����


    /*[SerializeField]
    private float attackRate = 0.5f;            //���� �ӵ�
    [SerializeField]
    private float attackRange = 2.0f;           //���ݹ���
    [SerializeField]
    private float attackDamage = 1f;           //���ݷ�*/
    private int level = 0;                                          // Ÿ������
    private WeaponState weaponState = WeaponState.SearchTarget;     // Ÿ�� ������ ����
    private Enemy attackTarget = null;                              // ���� ���
    private SpriteRenderer spriteRenderer;                          // Ÿ�� ������Ʈ �̹��� �����
    private TowerSpawner towerSpawner;
    private EnemySpawner enemySpawner;                              // ���ӿ� �����ϴ� �� ���� ȹ���
    private PlayerGold playerGold;                                  // �÷��̾��� ��� ���� ȹ�� �� ����
    private Tile ownerTile;                                         // ���� Ÿ���� ��ġ�Ǿ� �ִ� Ÿ��

    private float addedDamage;                                      // ������ ���� �߰��� ������
    private int buffLevel;                                          // ������ �޴��� ���� ����(0:����X. 1~3: �޴� ���� ����)


    public Sprite TowerSprite => towerTemplate.weapon[level].sprite;
    public float Damage => towerTemplate.weapon[level].damage;
    public float Rate => towerTemplate.weapon[level].rate;
    public float Range => towerTemplate.weapon[level].range;
    public int UpgradeCost => Level < MaxLevel ? towerTemplate.weapon[level + 1].cost : 0;
    public int SellCost => towerTemplate.weapon[level].sell;
    /*public float Damage => attackDamage;
    public float Rate => attackRate;
    public float Range => attackRange;*/
    public int Level => level + 1;
    public int MaxLevel => towerTemplate.weapon.Length;
    public float Slow => towerTemplate.weapon[level].slow;
    public float Buff => towerTemplate.weapon[level].buff;
    public WeaponType WeaponType => weaponType;
    public float AddedDamage
    {
        set => addedDamage = Mathf.Max(0, value);
        get => addedDamage;
    }
    public int BuffLevel
    {
        set => buffLevel = Mathf.Max(0, value);
        get => buffLevel;
    }

    public void Setup(TowerSpawner towerSpawner,EnemySpawner enemySpawner, PlayerGold playerGold, Tile ownerTile)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.towerSpawner = towerSpawner;
        this.enemySpawner = enemySpawner;
        this.playerGold = playerGold;
        this.ownerTile=ownerTile;
        //���� ���¸� WeaponState.SerchTarget���� ����
        //ChangeState(WeaponState.SearchTarget);
        
        //���� �Ӽ��� ĳ��, ������ �ϋ�
        if(weaponType==WeaponType.Cannon || weaponType==WeaponType.Laser)
        {
            //���� ���¸� WeaponState.SearchTarget���� ����
            ChangeState(WeaponState.SearchTarget);

        }
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

    public void OnBuffAroundTower()
    {
        //���� �ʿ� ��ġ�� "Tower" �±׸� ���� ��� ������Ʈ Ž��
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        for(int i=0; i<towers.Length; i++)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();

            //�̹� ������ �ް� �ְ�, ���� ���� Ÿ���� �������� ���� �����̸� �н�
            if (weapon.BuffLevel > Level)
            {
                continue;
            }

            //���� ���� Ÿ���� �ٸ� Ÿ���� �Ÿ��� �ǻ��ؼ� ���� �ȿ� Ÿ���� ������
            if (Vector3.Distance(weapon.transform.position, transform.position) <= towerTemplate.weapon[level].range)
            {
                //������ ������ ĳ��, ������ Ÿ���̸�
                if(weapon.WeaponType==WeaponType.Cannon || weapon.WeaponType==WeaponType.Laser)
                {
                    //������ ���� ���ݷ� ����
                    weapon.AddedDamage=weapon.Damage * (towerTemplate.weapon[level].buff);
                    //Ÿ���� �ް� �ִ� ���� ���� ����
                    weapon.BuffLevel = level;
                }
            }
        }
    }
    private IEnumerator SearchTarget()
    {
        while (true)
        {
            /*//���� ������ �ִ� ���� ã�� ���� ���� �Ÿ��� �ִ��� ũ�� ����
            float closestDistSqr = Mathf.Infinity;
            // EnemySpawner�� EnemyList�� �ִ� ���� �ʿ� �����ϴ� ��� �� �˻�
            for (int i = 0;i<enemySpawner.EnemyList.Count;i++)
            {
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
                // ���� �˻����� ������ �Ÿ��� ���ݹ��� ���� �ְ�, ������� �˻��� ������ �Ÿ��� ������
                if (distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    attackTarget = enemySpawner.EnemyList[i];
                }

            }*/
            //���� Ÿ���� ���� ������ �ִ� ���� ���(��) Ž��
            attackTarget = FindClosestAttackTarget();
            if(attackTarget!=null && attackTarget.IsDie == false)
            {
                if (weaponType == WeaponType.Cannon)
                {
                    ChangeState(WeaponState.TryAttackCannon);
                }else if (weaponType == WeaponType.Laser)
                {
                    ChangeState(WeaponState.TryAttackLaser);
                }

            }
            yield return null;
             
        }
    }

    //private IEnumerator AttackToTarget()
    private IEnumerator TryAttackCannon()
    {
        while (true)
        {
            /*//1. tartget�� �ִ��� �˻� (�ٸ� �߻�ü�� ���� ����, Goal �������� �̵��� ������)
            if (attackTarget == null|| attackTarget.IsDie)
            {
                ChangeState(WeaponState.SearchTarget); break;
            }
            //2. target�� ���� ���� �ȿ� �ִ��� �˻�(���� ������ ����� ���ο� �� Ž��)
            float distance =Vector3.Distance(attackTarget.transform.position, transform.position);
            if (distance > towerTemplate.weapon[level].range)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget); break;
            }*/

            //target�� �����ϴ°� �������� �˻�
            if (IsPossibleToAttackTarget() == false)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            //3. attackRage �ð���ŭ ���
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);
            //4. ���� (�߻�ü ����)
            SpawnProjectile();
        }
    }
    private IEnumerator TryAttackLaser()
    {
        EnableLaser();
        while (true)
        {

            //target�� �����ϴ°� �������� �˻�
            if (IsPossibleToAttackTarget() == false)
            {
                //������, ������ Ÿ�� ȿ�� ��Ȱ��ȭ
                DisableLaser();
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            //������ ����
            SpawnLaser();
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);
        }
    }
    private Enemy FindClosestAttackTarget()
    {
        //���� ������ �ִ� ���� ã�� ���� ���� �Ÿ��� �ִ��� ũ�� ����
        float closestDistSqr = Mathf.Infinity;
        //EnemySpawner�� EnemyList�� �ִ� ���� �ʿ� �����ϴ� ��� �� �˻�
        for(int i=0; i < enemySpawner.EnemyList.Count; ++i)
        {
            float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
            //���� �˻����� ������ �Ÿ��� ���ݹ��� ���� �ְ�, ������� �˻��� ������ �Ÿ��� ������
            if (distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
            {
                closestDistSqr = distance;
                attackTarget = enemySpawner.EnemyList[i];
            }
        }

        return attackTarget;
    }

    private bool IsPossibleToAttackTarget()
    {
        //tartget�� �ִ��� �˻�(�ٸ� �߻�ü�� ���� ����, Goal �������� �̵��� ���� ��)
        if(attackTarget==null)
        {
            return false;
        }

        //target�� ���� ���� �ȿ� �ִ��� �˻� (���� ������ ����� ���ο� �� Ž��)
        float distance = Vector3.Distance(attackTarget.transform.position, transform.position);
        if (distance > towerTemplate.weapon[level].range)
        {
            attackTarget = null;
            return false;
        }
        return true;
    }


    private void SpawnProjectile()
    {
        GameObject clone=Instantiate(projectilPrefab,spawnPoint.position,Quaternion.identity);

        //���ݷ� = Ÿ�� �⺻ ���ݷ� + ������ ���� �߰��� ���ݷ�
        float damage = towerTemplate.weapon[level].damage + AddedDamage;
        clone.GetComponent<Projectile>().Setup(attackTarget, damage);
    }
    private void EnableLaser()
    {
        lineRenderer.gameObject.SetActive(true);
        hitEffect.gameObject.SetActive(true);
    }
    private void DisableLaser()
    {
        lineRenderer.gameObject.SetActive(false);
        hitEffect.gameObject.SetActive(false);
    }
    private void SpawnLaser()
    {
        Vector3 direction =attackTarget.transform.position-spawnPoint.position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(spawnPoint.position, direction, 
            towerTemplate.weapon[level].range,targetLayer);   

        //���� �������� �������� ���¸� ���� ���� ���� attackTartget �� ������ ������Ʈ�� ����
        for(int i=0;i<hit.Length;++i)
        {
            if (hit[i].transform == attackTarget.transform)
            {
                //���� ����
                lineRenderer.SetPosition(0, spawnPoint.position);
                //���� ��ǥ����
                lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);
                //Ÿ�� ȿ�� ��ġ ����
                hitEffect.position = hit[i].point;
                //�� ü�� ����(1�ʿ� damage��ŭ ����)
                //���ݷ�=Ÿ�� �⺻ ���ݷ� +������ ���� �߰��� ���ݷ�
                float damage= towerTemplate.weapon[level].damage+AddedDamage;
                attackTarget.GetComponent<EnemyHP>().TakeDamage(damage* Time.deltaTime);

            }
        }
    }

    public bool Upgrade()
    {
        //Ÿ�� ���׷��̵忡 �ʿ��� ��尡 �������?
        if (playerGold.CurrentGold < towerTemplate.weapon[level].cost)
        {
            return false;
        }

        //Ÿ�� ���� ����
        level++;
        //Ÿ�� ���� ����(Sprite)
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;
        //��� ����
        playerGold.CurrentGold -= towerTemplate.weapon[level].cost;

        if (weaponType == WeaponType.Laser)
        {
            //������ ���� ������ ���� ����
            lineRenderer.startWidth = 0.05f + level * 0.05f;
            lineRenderer.endWidth = 0.05f;
        }

        //Ÿ���� ���׷��̵� �ɶ� ��� ���� Ÿ���� ���� ȿ�� ����
        //���� Ÿ���� ���� Ÿ���� ���, ���� Ÿ���� ���� Ÿ���� ���
        towerSpawner.OnBuffAllBuffTowers();
        return true;
    }
    public void Sell()
    {
        //��� ����
        playerGold.CurrentGold += towerTemplate.weapon[level].sell;
        //���� ���Ͽ� �ٽ� Ÿ�� �Ǽ��� �����ϵ��� ����
        ownerTile.IsBuildTower = false;
        //Ÿ�� �ı�
        Destroy(gameObject);

    }
}
