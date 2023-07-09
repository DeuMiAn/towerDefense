using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] //
public class TowerTemplate : ScriptableObject
{
    public GameObject towerPrefab;      //타워 생성을 위한 프리팹
    public GameObject followTowerPrefab; //임시 타워 프리팹
    public Weapon[] weapon;             //레벨별 타워(무기) 정보

    [System.Serializable]
    public struct Weapon //무기 구조체
    {
        public Sprite sprite;       // 보여지는 타워 이미지 (UI)
        public float damage;        // 공격력
        public float rate;          // 공격 속도
        public float range;         // 공격 범위
        public int cost;            // 필요 골드 (0레벨 : 건설, 1~레벨 : 업그레이드)
        public int sell;            // 타워 판매 시 획득 골드
    }
}

// 클래스 내부에 구조체 생성시 외부에서 구조체 변수 선언 못함
// 권장 방법은 변수를 코드르에서 조작하지 못하도록 모두 private설정후
// 모든 변수에 접근 가능한 프로퍼티를 제작
