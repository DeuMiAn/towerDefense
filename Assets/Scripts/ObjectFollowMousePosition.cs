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
        //화면의 마우스 좌쵸를 기준으로 게임 월드 상의 좌표를 구한다.
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        transform.position=mainCamera.ScreenToWorldPoint(position);
        // z 위치를 0으로 설정
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        
    }

    /*
     * 
     * 해당 스크립트를 가지고 있는 게임 오브젝트가 마우스 위치를 쫓아다닌다
     * 
     */
}
