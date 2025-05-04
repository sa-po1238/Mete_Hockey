using UnityEngine;

public class TutorialEnemyB : MonoBehaviour
{
    [SerializeField] private TutorialWaveB tutorialWaveB; // Inspectorで割り当てる
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] private int flagIndex = 0; // どのフラグを立てるか（0ならWaveBFlags[0]）

    void Start()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
    }
    void Update()
    {
        bool tutorialFlag = tutorialWaveB.GetFlagB(1);
        if (!tutorialFlag)
        {
            enemyManager.UpdateEnemyHP(100); // 敵のHPを100に固定
        }
        // 敵のX座標が0以下になったら0に固定
        if (transform.position.x <= 0)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
    }

    void OnDestroy()
    {
        // チュートリアルWaveBに通知してフラグをオン
        if (tutorialWaveB != null)
        {
            tutorialWaveB.ChangeFlagB(1);
            Debug.Log($"敵が破壊されました。TutorialWaveBのフラグ {2} をONにしました。");
        }
    }
}
