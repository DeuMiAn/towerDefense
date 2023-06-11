using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //타일에 타워가 건설되어 있는지 검사하는 변수로
    //자동 구현 프로퍼티로 구현했음
    //아래코드처럼 작성하면 get,set기본형이 자동 작성됨 올 ㅋ 굿
    public bool IsBuildTower { set; get; }

    private void Awake()
    {
        IsBuildTower = false;
    }
}
