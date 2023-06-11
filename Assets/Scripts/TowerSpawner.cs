using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private EnemySpawner enemySpawner; //���� �ʿ� �����ϴ� �� ����Ʈ ������ ��� ���ؼ�..

    public void SpawnTower(Transform tileTransform)
    {
        Tile tile = tileTransform.GetComponent<Tile>();

        //Ÿ�� �Ǽ� ���� ���� Ȯ��
        //1. ���� Ÿ���� ��ġ�� �̹� Ÿ���� �ż��Ǿ� ������ Ÿ�� �Ǽ� X
        if(tile.IsBuildTower==true)
        {
            return;
        }
        //Ÿ���� �Ǽ��Ǿ� �������� ����
        tile.IsBuildTower = true;
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
