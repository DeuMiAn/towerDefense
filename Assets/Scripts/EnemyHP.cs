using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP;        //�ִ�ü��
    private float currentHP;    //���� ü��   
    private bool isDie=false;   //���� ��� �����̸� isDie�� true�� ����
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;

    public float MaxHP=>maxHP; public float CurrentHP=>currentHP;

    private void Awake()
    {
        currentHP = maxHP; //���� ü���� �ִ� ü�°� ���� ���� 
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    public void TakeDamage(float damage)
    {
        //���� ü���� damage��ŭ �����ؼ� ���� ��Ȳ�� �� ���� Ÿ���� ������ ���ÿ� ������
        //enemy.OnDIe() �Լ��� ���� �� ����� �� �ִ�
        // ���� ���� ���°� ��� �����̸� �Ʒ� �ڵ带 �������� �ʴ´�
        if(isDie==true) { return; } 

        //���� ü���� damage��ŭ ����
        currentHP -= damage;

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        //ü���� 0 ����= �� ĳ���� ���
        if (currentHP <= 0)
        {
            isDie = true;
            //�� ĳ���� ���
            enemy.OnDie();
        }

    }

    private IEnumerator HitAlphaAnimation()
    {
        //���� ���� ������ color ������ ����
        Color color=spriteRenderer.color;

        //���� ������ 40%��
        color.a = 0.4f;
        spriteRenderer.color = color;

        //0.05�� ���� ���
        yield return new WaitForSeconds(0.05f);

        //���� ������ 100% �� ����
        color.a = 1.0f;
        spriteRenderer.color=color;
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
