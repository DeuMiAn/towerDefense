using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField] 
    private int towerBuildGold=50;      //Ÿ�� �Ǽ��� ���Ǵ� ���
    [SerializeField]
    private EnemySpawner enemySpawner; //���� �ʿ� �����ϴ� �� ����Ʈ ������ ��� ���ؼ�..
    [SerializeField]
    private PlayerGold playerGold;       //Ÿ�� �Ǽ� �� ��� ���Ҹ� ����

    public void SpawnTower(Transform tileTransform)
    {

        //Ÿ�� �Ǽ� ���� ����Ȯ��
        //1. Ÿ���� �Ǽ��� ����ŭ ���� ������ Ÿ�� �Ǽ�X
        if (towerBuildGold > playerGold.CurrentGold)
        {
            return;
        }
        Tile tile = tileTransform.GetComponent<Tile>();

        //Ÿ�� �Ǽ� ���� ���� Ȯ��
        //1. ���� Ÿ���� ��ġ�� �̹� Ÿ���� �ż��Ǿ� ������ Ÿ�� �Ǽ� X
        if(tile.IsBuildTower==true)
        {
            return;
        }
        //Ÿ���� �Ǽ��Ǿ� �������� ����
        tile.IsBuildTower = true;
        //Ÿ�� �Ǽ��� �ʿ��� ��常ŭ ����
        playerGold.CurrentGold -= towerBuildGold;
        //������ Ÿ���� ��ġ�� Ÿ�� �Ǽ�
        GameObject clone=Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
        //Ÿ�� ���⿡ enemySpawner ���� ����
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
