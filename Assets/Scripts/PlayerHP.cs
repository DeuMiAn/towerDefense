using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private Image imageScreen;// 전체화면 덮은 빨간색이미지
    [SerializeField]
    private float maxHP = 20;       //최대 체력
    private float currentHP;        //현재 체력

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP; 

    private void Awake()
    {
        currentHP = maxHP;          //현재체력=최대체력

    }

    public void TakeDamage(float damage)
    {
        //damage!!!
        currentHP -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        //체력0은 죽음
        if(currentHP <= 0) { }
    }
    private IEnumerator HitAlphaAnimation()
    {
        //전체화면크기로 배치된 이미지 색상을 color변수에저장
        //이미지 투명도를 40%로 설정
        Color color=imageScreen.color;
        color.a = 0.4f;
        imageScreen.color = color;  

        //투명도가 0%가 될때까지 감소
        while(color.a >= 0.0f)
        {
            color.a -= Time.deltaTime;
            imageScreen.color=color;

            yield return null;

        }
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
