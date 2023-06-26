using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textPlayerHP; //text- TextMeshPro UI[�÷��̾� ü��]
    [SerializeField]
    private TextMeshProUGUI textPlayerGold; //text- TextMeshPro UI[�÷��̾� ü��]
    [SerializeField]
    private PlayerHP playerHP;              //�÷��̾� ü�� ����
    [SerializeField]
    private PlayerGold playerGold;              //�÷��̾� ü�� ����



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textPlayerHP.text = playerHP.CurrentHP + "/" + playerHP.MaxHP;
        textPlayerGold.text = playerGold.CurrentGold.ToString();
    }
}
