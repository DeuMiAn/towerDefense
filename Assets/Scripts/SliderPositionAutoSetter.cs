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
        //Slider UI가 쫓아다닐 target 설정
        tartget = target;
        //RectTransform 컴포넌트 정보 얻어오기
        rectTransform = GetComponent<RectTransform>();

    }

    private void LateUpdate()
    {
        //적이 파괴되어 쫓아다닐 대상이 사라지면 Slider UI도 삭제
        if (tartget == null|| tartget.IsDie)
        {
            Destroy(gameObject);
            return;
        }
        //오브젝트의 위치가 갱신된 이후에 Slider UI도 함께 위치를 설정하도록 하기 위해
        //LaterUpdate()에서 호출한다

        //오브젝트의 월드 좌표를 기준으로 화면에서의 촤표 값을 구함
        Vector3 screenPosition=Camera.main.WorldToScreenPoint(tartget.transform.position);
        //화면내에서 좌표 + distance만큼 떨어진 위치를 Slider UI의 위치로 설정
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
