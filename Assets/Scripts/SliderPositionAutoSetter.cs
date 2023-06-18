using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField]
    private Vector3 distace = Vector3.down * 20.0f;
    //private Transform tartgetTransform;
    private Enemy tartget;

    private RectTransform rectTransform;

    public void Setup(Enemy target)
    {
        //Slider UI�� �Ѿƴٴ� target ����
        tartget = target;
        //RectTransform ������Ʈ ���� ������
        rectTransform = GetComponent<RectTransform>();

    }

    private void LateUpdate()
    {
        //���� �ı��Ǿ� �Ѿƴٴ� ����� ������� Slider UI�� ����
        if (tartget == null|| tartget.IsDie)
        {
            Destroy(gameObject);
            return;
        }
        //������Ʈ�� ��ġ�� ���ŵ� ���Ŀ� Slider UI�� �Բ� ��ġ�� �����ϵ��� �ϱ� ����
        //LaterUpdate()���� ȣ���Ѵ�

        //������Ʈ�� ���� ��ǥ�� �������� ȭ�鿡���� ��ǥ ���� ����
        Vector3 screenPosition=Camera.main.WorldToScreenPoint(tartget.transform.position);
        //ȭ�鳻���� ��ǥ + distance��ŭ ������ ��ġ�� Slider UI�� ��ġ�� ����
        rectTransform.position = screenPosition + distace;
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
