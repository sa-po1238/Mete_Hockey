using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHPManager : MonoBehaviour
{
    [SerializeField] float playerHP = 100f; //プレイヤーのHP
    private float currentPlayerHP; //プレイヤーの現在のHP
    [SerializeField] private PlayerHPManager playerHPManager;
    [SerializeField] private PlayerHPGauge playerHPGauge;
    [SerializeField] private GameObject playerFace; //プレイヤーの顔
    private Animator playerFaceAnimator; //プレイヤーの顔のアニメーター

    private void Awake()
    {
        currentPlayerHP = playerHP;

        playerFaceAnimator = playerFace.GetComponent<Animator>();
    }
    public void TakeDamage(float damage)
    {
        currentPlayerHP -= damage;
        Debug.Log($"HP:{currentPlayerHP}");
        // HPが30以下のアニメーション
        playerFaceAnimator.SetFloat("HP", currentPlayerHP);

        // ダメージを受けたときのアニメーション
        playerFaceAnimator.SetTrigger("isPlayerDamaged");

        if (currentPlayerHP <= 0)
        {
            Debug.Log("GameOver");
            AudioManager.instance_AudioManager.PlaySE(13);
            WaitForSeconds(1f);
            SceneManager.LoadScene("GameOverResult");
        }
    }

    IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    // 外から最大HPや現在HPを取るための関数
    public float GetMaxPlayerHP()
    {
        return playerHP;
    }

    public float GetCurrentPlayerHP()
    {
        return currentPlayerHP;
    }
}
