using UnityEngine;

public class TutorialEnemyC : MonoBehaviour
{
    [SerializeField] TutorialWaveC tutorialWaveC; // Inspectorで割り当てる
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] private int flagIndex = 0; // どのフラグを立てるか（0ならwaveCFlags[0]）
    private bool flagChanged = false; // 1回だけ呼ぶためのフラグ

    void Start()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
    }
    void Update()
    {
        bool tutorialFlag = tutorialWaveC.GetFlagC(0);
        if (!tutorialFlag)
        {
            enemyManager.UpdateEnemyHP(100); // 敵のHPを100に固定
        }
        // 敵のX座標が0以下になったら0に固定
        if (transform.position.x <= 0)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }

        // 1回だけ ChangeFlagC(0) を呼ぶようにする
        if (this.gameObject.tag == "EnemyShot" && !flagChanged)
        {
            tutorialWaveC.ChangeFlagC(0);
            flagChanged = true; // 2回目以降は無視
        }
    }

    void OnDestroy()
    {
        // チュートリアルwaveCに通知してフラグをオン
        if (tutorialWaveC != null)
        {
            tutorialWaveC.IsWave0Failed();
        }
    }
}
