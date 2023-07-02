using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] //
public class TowerTemplate : ScriptableObject
{
    public GameObject towerPrefab;      //Ÿ�� ������ ���� ������
    public Weapon[] weapon;             //������ Ÿ��(����) ����

    [System.Serializable]
    public struct Weapon //���� ����ü
    {
        public Sprite sprite;       // �������� Ÿ�� �̹��� (UI)
        public float damage;        // ���ݷ�
        public float rate;          // ���� �ӵ�
        public float range;         // ���� ����
        public int cost;            // �ʿ� ��� (0���� : �Ǽ�, 1~���� : ���׷��̵�)
    }
}

// Ŭ���� ���ο� ����ü ������ �ܺο��� ����ü ���� ���� ����
// ���� ����� ������ �ڵ帣���� �������� ���ϵ��� ��� private������
// ��� ������ ���� ������ ������Ƽ�� ����