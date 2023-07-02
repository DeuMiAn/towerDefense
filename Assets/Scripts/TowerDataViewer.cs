using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image imageTower;
    [SerializeField]
    private TextMeshProUGUI textDamage;
    [SerializeField]
    private TextMeshProUGUI textRate;
    [SerializeField]
    private TextMeshProUGUI textRange;
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private TowerAttackRange towerAttackRange;
    [SerializeField]
    private Button buttonUpgrade;


    private TowerWeapon currentTower;

    private void Awake()
    {
        OffPanel();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }

    public void OnPanel(Transform towerWeapon)
    {
        //����ؾ��ϴ� Ÿ�� ������ �޾ƿͼ� ����
        currentTower=towerWeapon.GetComponent<TowerWeapon>();

        //Ÿ�� ���� Panel On
        gameObject.SetActive(true);

        //Ÿ������ ����
        UpdateTowerData();

        //Ÿ�� ������Ʈ �ֺ��� ǥ�õǴ� Ÿ�� ���� ���� Sprite On
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }
    public void OffPanel()
    {
        //Ÿ�� ���� panel off
        gameObject.SetActive(false);
        //Ÿ�� ���ݹ��� Sprite Off
        towerAttackRange.OffAttackRange();

    }
    private void UpdateTowerData()
    {
        imageTower.sprite = currentTower.TowerSprite;
        textDamage.text = "Damage : " + currentTower.Damage;
        textRate.text = "Rate : " + currentTower.Rate;
        textRange.text = "Range : " + currentTower.Range;
        textLevel.text = "Level : " + currentTower.Level;

        //���׷��̵尡 �Ұ��������� ��ư ��Ȱ��ȭ
        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;
    }

    public void OnClickEventTowerUpgrade()
    {
        //Ÿ�� ���׷��̵� �õ�(����:true, ����:false)
        bool isSuccess = currentTower.Upgrade();

        if (isSuccess == true)
        {
            //Ÿ�� ���׷��̵� �� ���� ���� 
            UpdateTowerData();
            //Ÿ�� �ֺ� ���ݹ����� ����
            towerAttackRange.OnAttackRange(currentTower.transform.position,currentTower.Range);
        }
        else
        {
            //Ÿ�� ���׷��̵� ��� ����
        }
    }
}