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
        //출력해야하는 타워 정보를 받아와서 저장
        currentTower=towerWeapon.GetComponent<TowerWeapon>();

        //타워 정보 Panel On
        gameObject.SetActive(true);

        //타워정보 갱신
        UpdateTowerData();

        //타워 오브젝트 주변에 표시되는 타워 공격 범위 Sprite On
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }
    public void OffPanel()
    {
        //타워 정보 panel off
        gameObject.SetActive(false);
        //타워 공격범위 Sprite Off
        towerAttackRange.OffAttackRange();

    }
    private void UpdateTowerData()
    {
        imageTower.sprite = currentTower.TowerSprite;
        textDamage.text = "Damage : " + currentTower.Damage;
        textRate.text = "Rate : " + currentTower.Rate;
        textRange.text = "Range : " + currentTower.Range;
        textLevel.text = "Level : " + currentTower.Level;

        //업그레이드가 불가능해지면 버튼 비활성화
        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;
    }

    public void OnClickEventTowerUpgrade()
    {
        //타워 업그레이드 시도(성공:true, 실패:false)
        bool isSuccess = currentTower.Upgrade();

        if (isSuccess == true)
        {
            //타워 업그레이드 완 정보 갱신 
            UpdateTowerData();
            //타워 주변 공격범위도 갱신
            towerAttackRange.OnAttackRange(currentTower.transform.position,currentTower.Range);
        }
        else
        {
            //타워 업그레이드 비용 부족
        }
    }
}
