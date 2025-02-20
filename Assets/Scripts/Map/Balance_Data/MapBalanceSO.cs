using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapBalanceParameters_", menuName = "PCG/MapBalanceData")]
public class MapBalanceSO : ScriptableObject
{
    // 몬스터
    public int spawnMonsterNum;
    [Range(0f, 1f)]
    public float spawnMonsterRate;

    // 오브젝트
    public int spawnObjectNum;
    [Range(0f, 1f)]
    public float spawnObjectRate;

}
