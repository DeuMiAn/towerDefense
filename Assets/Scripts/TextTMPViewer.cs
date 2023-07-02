using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textPlayerHP;   //text- TextMeshPro UI[플레이어 체력]
    [SerializeField]
    private TextMeshProUGUI textPlayerGold; //text- TextMeshPro UI[플레이어 체력]
    [SerializeField]
    private TextMeshProUGUI textWave;       //text- TextMeshPro UI[현재 웨이브 / 총 웨이브]
    [SerializeField]
    private TextMeshProUGUI textEnemyCount;       //text- TextMeshPro UI[현재 웨이브 / 총 웨이브]
    [SerializeField]
    private PlayerHP playerHP;              //플레이어 체력 정보
    [SerializeField]
    private PlayerGold playerGold;              //플레이어 체력 정보
    [SerializeField]
    private WaveSystem waveSystem;          //웨이브 정보
    [SerializeField]
    private EnemySpawner enemySpawner;          //적 정보



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textPlayerHP.text = playerHP.CurrentHP + "/" + playerHP.MaxHP;
        textPlayerGold.text = playerGold.CurrentGold.ToString();
        textWave.text = waveSystem.CurrentWave + "/" + waveSystem.MaxWave;
        textEnemyCount.text = enemySpawner.CurrentEnemyCount + "/" + enemySpawner.MaxEnemyCount;
    }
}
