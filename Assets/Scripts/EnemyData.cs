using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class EnemyData : ScriptableObject
{
    public float moveSpeed;
    public float health;
    public bool hitboxDealsDamage;
    public bool isSentient; // Is the enemy aggressive towards the player?

}


[CreateAssetMenu(fileName = "Boss", menuName = "Scriptable Objects/Boss")]
public class BossData : EnemyData
{
    // List of boss's weak points, and a bool indicating if it's enabled
    public Dictionary<Collider, bool> WeakPoints;
}