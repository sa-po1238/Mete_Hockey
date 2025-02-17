using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "ScriptableObjects/EnemyDatabase", order = 2)]
public class EnemyDatabase : ScriptableObject
{
    public List<EnemyData> enemies;
}
