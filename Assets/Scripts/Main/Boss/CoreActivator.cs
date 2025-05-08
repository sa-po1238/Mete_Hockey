using UnityEngine;

public class CoreActivator : MonoBehaviour
{
    [SerializeField] private GameObject coreCover; // コアを覆っている"ふた"
    [SerializeField] private int totalBarriers = 3; // 必要なバリアの数
    private int destroyedBarrierCount = 0;
    public bool isCoreActive = false; // コアが露出していればtrue

    // このスクリプトが有効になると呼ばれる
    private void OnEnable()
    {
        // バリアが破壊されたときのイベントを購読
        BossBarrierManager.OnBarrierDestroyed += OnBarrierDestroyed;
    }

    // このスクリプトが無効になると呼ばれる
    private void OnDisable()
    {
        // バリアが破壊されたときのイベントを購読解除 重複やエラーを防ぐ
        BossBarrierManager.OnBarrierDestroyed -= OnBarrierDestroyed;
    }

    // バリアが破壊されたときに呼ばれるメソッド
    private void OnBarrierDestroyed(BossBarrierManager barrier)
    {
        destroyedBarrierCount++;

        // 全てのバリアが壊れるとこっち
        if (destroyedBarrierCount >= totalBarriers)
        {
            if (coreCover != null)
            {
                coreCover.SetActive(false); // ふたを非表示にして、コアが見えるようにする
                isCoreActive = true; // コアが露出している状態にする
                GetComponent<SphereCollider>().enabled = true;
            }
        }
    }
    public void ResetCoreActivation()
    {
        destroyedBarrierCount = 0; // バリアのカウントをリセット
        isCoreActive = false; // コアが露出していない状態にする
        GetComponent<SphereCollider>().enabled = false;
        if (coreCover != null)
        {
            coreCover.SetActive(true); // ふたを表示して、コアを覆う
        }
    }
}
