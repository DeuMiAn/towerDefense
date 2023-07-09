using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum WeaponType {Cannon =0,Laser}
public enum WeaponState { SearchTarget = 0, TryAttackCannon,TryAttackLaser  }


public class TowerWeapon : MonoBehaviour
{
    //Header(string) Inspector View에 표시되는 변수들을 용도별로 구분하기위해 사용
    //string에 작성된 내용은 굵은 글씨로 표시됨
    [Header("Commons")]
    [SerializeField]
    private TowerTemplate towerTemplate;        // 타워정보(공력,공속 등등)
    [SerializeField]
    private Transform spawnPoint;               // 발사체 생성위치
    [SerializeField]
    private WeaponType weaponType;              //무기 속성 설정

    [Header("Cannon")]
    [SerializeField]
    private GameObject projectilPrefab;        //발사체 프리팹

    [Header("Laser")]
    [SerializeField]
    private LineRenderer lineRenderer;        //레이저 선
    [SerializeField]
    private Transform hitEffect;            //타격효과
    [SerializeField]
    private LayerMask targetLayer;              //광선에 부딪히는 레이어 설정


    /*[SerializeField]
    private float attackRate = 0.5f;            //공격 속도
    [SerializeField]
    private float attackRange = 2.0f;           //공격범위
    [SerializeField]
    private float attackDamage = 1f;           //공격력*/
    private int level = 0;                                          //타워레벨
    private WeaponState weaponState = WeaponState.SearchTarget;     //타워 무기의 상태
    private Enemy attackTarget = null;                          //공격 대상
    private SpriteRenderer spriteRenderer;                          //타워 오브젝트 이미지 변경용
    private EnemySpawner enemySpawner;                              // 게임에 존재하는 적 정보 획득용
    private PlayerGold playerGold;                                  // 플레이어의 골드 정보 획득 및 설정
    private Tile ownerTile;                                         // 현재 타워가 배치되어 있는 타일


    public Sprite TowerSprite => towerTemplate.weapon[level].sprite;
    public float Damage => towerTemplate.weapon[level].damage;
    public float Rate => towerTemplate.weapon[level].rate;
    public float Range => towerTemplate.weapon[level].range;
    /*public float Damage => attackDamage;
    public float Rate => attackRate;
    public float Range => attackRange;*/
    public int Level => level + 1;
    public int MaxLevel => towerTemplate.weapon.Length;

    public void Setup(EnemySpawner enemySpawner, PlayerGold playerGold, Tile ownerTile)
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        this.enemySpawner = enemySpawner;
        this.playerGold = playerGold;
        this.ownerTile=ownerTile;
        //최초 상태를 WeaponState.SerchTarget으로 설정
        ChangeState(WeaponState.SearchTarget);
    }

    public void ChangeState(WeaponState newState)
    {
        //이전에 재생중이던 상태 종료
        StopCoroutine(weaponState.ToString());
        //상태 변경
        weaponState = newState;
        // 새로운 상태 재생
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
        //원점으로부터의 거리와 수평축으로부터의 각도를 이용해 위치를 구하는 극 좌표계 이용
        // 각도=arctan(y/x)
        // x,y 변위값 구하기
        float dx= attackTarget.transform.position.x-transform.position.x;
        float dy= attackTarget.transform.position.y-transform.position.y;
        //x,y 변위값을 바탕으로 각도 구하기
        // 각도가 radian 단위이기 때문에 Mathf.Rad2Deg를 곱해 도 단위를 구함
        float degree = Mathf.Atan2(dy,dx)*Mathf.Rad2Deg;
        transform.rotation=Quaternion.Euler(0,0,degree);
    }
    private IEnumerator SearchTarget()
    {
        while (true)
        {
            /*//제일 가까이 있는 적을 찾기 위해 최초 거리를 최대한 크게 설정
            float closestDistSqr = Mathf.Infinity;
            // EnemySpawner의 EnemyList에 있는 현재 맵에 존재하는 모든 적 검사
            for (int i = 0;i<enemySpawner.EnemyList.Count;i++)
            {
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
                // 현재 검사중인 적과의 거리가 공격범위 내에 있고, 현재까지 검사한 적보다 거리가 가까우면
                if (distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    attackTarget = enemySpawner.EnemyList[i];
                }

            }*/
            //현재 타워에 가장 가까이 있는 공격 대상(적) 탐색
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
            /*//1. tartget이 있는지 검사 (다른 발사체에 의해 제거, Goal 지점까지 이동해 삭제등)
            if (attackTarget == null|| attackTarget.IsDie)
            {
                ChangeState(WeaponState.SearchTarget); break;
            }
            //2. target이 공격 범위 안에 있는지 검사(공격 범위를 벗어나면 새로운 적 탐색)
            float distance =Vector3.Distance(attackTarget.transform.position, transform.position);
            if (distance > towerTemplate.weapon[level].range)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget); break;
            }*/

            //target을 공격하는게 가능한지 검사
            if (IsPossibleToAttackTarget() == false)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            //3. attackRage 시간만큼 대기
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);
            //4. 공격 (발사체 생성)
            SpawnProjectile();
        }
    }
    private IEnumerator TryAttackLaser()
    {
        EnableLaser();
        while (true)
        {

            //target을 공격하는게 가능한지 검사
            if (IsPossibleToAttackTarget() == false)
            {
                //레이저, 레이저 타격 효과 비활성화
                DisableLaser();
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            //레이저 공격
            SpawnLaser();
            yield return new WaitForSeconds(towerTemplate.weapon[level].rate);
        }
    }
    private Enemy FindClosestAttackTarget()
    {
        //제일 가까이 있는 적을 찾기 위해 최초 거리를 최대한 크게 설정
        float closestDistSqr = Mathf.Infinity;
        //EnemySpawner의 EnemyList에 있는 현재 맵에 존재하는 모든 적 검사
        for(int i=0; i < enemySpawner.EnemyList.Count; ++i)
        {
            float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
            //현재 검사중인 적과의 거리가 공격범위 내에 있고, 현재까지 검사한 적보다 거리가 가까우면
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
        //tartget이 있는지 검사(다른 발사체에 의해 제거, Goal 지점까지 이동해 삭제 등)
        if(attackTarget==null)
        {
            return false;
        }

        //target이 공격 범위 안에 있는지 검사 (공격 범위를 벗어나면 새로운 적 탐색)
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
        clone.GetComponent<Projectile>().Setup(attackTarget, towerTemplate.weapon[level].damage);
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

        //같은 방향으로 여러개의 광좌를 쏴서 그중 현재 attackTartget 과 동일한 오브젝트를 검출
        for(int i=0;i<hit.Length;++i)
        {
            if (hit[i].transform == attackTarget.transform)
            {
                //선의 시작
                lineRenderer.SetPosition(0, spawnPoint.position);
                //선의 목표지점
                lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);
                //타격 효과 위치 설정
                hitEffect.position = hit[i].point;
                //적 체력 감소(1초에 damage만큼 감소)
                attackTarget.GetComponent<EnemyHP>().TakeDamage(towerTemplate.weapon[level].damage * Time.deltaTime);

            }
        }
    }

    public bool Upgrade()
    {
        //타워 업그레이드에 필요한 골드가 충분한지?
        if (playerGold.CurrentGold < towerTemplate.weapon[level].cost)
        {
            return false;
        }

        //타워 레벨 증가
        level++;
        //타워 외형 변경(Sprite)
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;
        //골드 차감
        playerGold.CurrentGold -= towerTemplate.weapon[level].cost;

        if (weaponType == WeaponType.Laser)
        {
            //레벨에 따라 레이저 굵기 설정
            lineRenderer.startWidth = 0.05f + level * 0.05f;
            lineRenderer.endWidth = 0.05f;
        }
        return true;
    }
    public void Sell()
    {
        //골드 증가
        playerGold.CurrentGold += towerTemplate.weapon[level].sell;
        //현태 자일에 다시 타워 건설이 가능하도록 설정
        ownerTile.IsBuildTower = false;
        //타워 파괴
        Destroy(gameObject);

    }
}
