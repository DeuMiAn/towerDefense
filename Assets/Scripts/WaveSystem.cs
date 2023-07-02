using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;               //���� ���������� ��� ���̺� ����
    [SerializeField]
    private EnemySpawner enemySpawner;
    private int currentWaveIndex = -1; // ���� ���̺� ���ؽ�

    //���̺� ���� ����� ���� Get ������Ƽ (���� ���̺�, �� ���̺�)
    public int CurrentWave => currentWaveIndex + 1;     //������ 0�̱� ������ +1
    public int MaxWave => waves.Length;


    public void StartWave()
    {
        //���� �ʿ� ���� ����, wave�� ���� ������
        if (enemySpawner.EnemyList.Count == 0 && currentWaveIndex < waves.Length - 1)
        {
            // ���ؽ��� ������ -1�̱� ������ ���̺� ���ؽ� ������ ���� ���� ��
            currentWaveIndex++;
            // EnemySpawner�� StartWave() �Լ� ȣ��. ���� ���̺� ���� ����
            enemySpawner.StartWave(waves[currentWaveIndex]);

        }
    }

    [System.Serializable]                   //����ü, Ŭ������ ����ȭ �ϴ� ���
    public struct Wave
    {
        public float spawnTime;             // ���� ���̺� �� ���� �ֱ�
        public int maxEnemyCount;           // ���� ���̺� �� ���� ����
        public GameObject[] enemyPrefabs;   // ���� ���̺� �� ���� ����
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
