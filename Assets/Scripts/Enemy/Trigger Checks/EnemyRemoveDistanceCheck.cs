using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRemoveDistanceCheck : MonoBehaviour
{
    private Enemy _enemy;


    void Awake()
    {
        _enemy = GetComponentInParent<Enemy>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            _enemy.SetRemoveDistanceBool(false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            _enemy.SetRemoveDistanceBool(true);
        }
    }
}
