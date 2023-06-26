using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private Image imageScreen;// ��üȭ�� ���� �������̹���
    [SerializeField]
    private float maxHP = 20;       //�ִ� ü��
    private float currentHP;        //���� ü��

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP; 

    private void Awake()
    {
        currentHP = maxHP;          //����ü��=�ִ�ü��

    }

    public void TakeDamage(float damage)
    {
        //damage!!!
        currentHP -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        //ü��0�� ����
        if(currentHP <= 0) { }
    }
    private IEnumerator HitAlphaAnimation()
    {
        //��üȭ��ũ��� ��ġ�� �̹��� ������ color����������
        //�̹��� ������ 40%�� ����
        Color color=imageScreen.color;
        color.a = 0.4f;
        imageScreen.color = color;  

        //������ 0%�� �ɶ����� ����
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
