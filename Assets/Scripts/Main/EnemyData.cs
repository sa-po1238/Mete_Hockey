using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public int enemyNumber;   // 敵番号（0始まり）    
    public float enemySpeed = 10f; //敵のスピード
    public float enemyHP = 20f; //敵のHP
    public float enemyDamage = 20f; //敵の攻撃力、エネミーショットのダメージ
}
