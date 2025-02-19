using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float enemyStarttime = 2.0f; //最初の敵が沸くまでの時間
    [SerializeField] float enemyCooltime = 2.0f; // 敵が沸く間隔



    void Start()
    {
        InvokeRepeating("GenEnemy", enemyStarttime, enemyCooltime);
    }

    void GenEnemy()
    {
        Instantiate(enemyPrefab, new Vector3(12, 0, -4 + 8 * Random.value), Quaternion.identity);
    }
}
