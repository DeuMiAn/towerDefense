using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private Transform hitTransform = null;      //마우스 클릭 선택한 오브젝트 임시저장


    private void Awake()
    {
        //Maincamera 태그를 가지고 있는 오브젝트 탐색후 CAMERA 컴포넌트 정보 전달
        //GameObject.FindeGameObjectWithTag("mainCamera").GetComponent<Camera>(); 와 동일
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //마우스가 UI에 머물러 있을 때는 아래 코드들이 실행되지 않도록함
        if (EventSystem.current.IsPointerOverGameObject() == true)
        {
            return;
        }
        //마우스 왼쪽 버튼을 눌렀을때
        if (Input.GetMouseButtonDown(0))
        {
            //카메라 위치에서 화면의 마우스 위치를 관통하는 광선 ㅐㅇ성
            //ray.origin : 광선의 시작위치(=카메라 위치)
            //ray.direction : 광선의 진행방향
            ray=mainCamera.ScreenPointToRay(Input.mousePosition);

            //2D 모니터를 통해 3D 월드의 오브젝트를 마우스로 선택하는 방법쿠
            //광선에 부딪히는 오브젝트를 검출해서 hit에 저장
            if (Physics.Raycast(ray,out hit, Mathf.Infinity))
            {
                hitTransform=hit.transform;
                //광선에 부딛힌 오브젝트의 태그가 Tile 이면
                if (hit.transform.CompareTag("Tile"))
                {
                    //타워를 생헝하는 sawnTower() 호출
                    towerSpawner.SpawnTower(hit.transform);
                }
                // 타워를 선택하면 해당 타워 정보를 출력하는 타워 정보창 ON
                else if(hit.transform.CompareTag("Tower"))
                {
                    towerDataViewer.OnPanel(hit.transform);
                }
            }
        }
        else if(Input.GetMouseButtonUp(0)) 
        {
            //마우스를 눌렀을 때 선택한 오브젝트가 없거나 선택한 오브젝트가 타워가 아니면
            if(hitTransform==null || hitTransform.CompareTag("Tower") == false)
            {
                //타워 정보 패널을 비활성화 한다
                towerDataViewer.OffPanel();
            }
            hitTransform=null;
        }
    }
}
