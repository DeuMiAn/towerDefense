using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowMousePosition : MonoBehaviour
{

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera=Camera.main;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ȭ���� ���콺 ���ݸ� �������� ���� ���� ���� ��ǥ�� ���Ѵ�.
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        transform.position=mainCamera.ScreenToWorldPoint(position);
        // z ��ġ�� 0���� ����
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        
    }

    /*
     * 
     * �ش� ��ũ��Ʈ�� ������ �ִ� ���� ������Ʈ�� ���콺 ��ġ�� �ѾƴٴѴ�
     * 
     */
}
