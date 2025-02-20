using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSpin : BFunction
{
    [Header("�⺻ ���")]
    Transform target;
    public float angle;
    public float distance;
    public float spinSpeed;
    [Space(10f)]

    [Header("ȸ�� �Ÿ� ���� ����")]
    [SerializeField] bool canIncreaseDistance;
    [SerializeField] float increaseDistancePerSec;
    [Space(10f)]

    [Header("ȸ�� �ӵ� ���� ����")]
    [SerializeField] bool canIncreaseSpeed;
    [SerializeField] float increaseSpeedPerSec;
    [Space(10f)]

    [Header("ȸ�� ���� ����")]
    [SerializeField] bool canStopSpin;
    [SerializeField] float stopTime;
    float curTime = 0f;

    public override void FunctionUpdate()
    {
        if(canStopSpin)
        {
            if(stopTime <= curTime)
                return;
            curTime += Time.deltaTime;
        }
        

        Vector3 pos = new Vector3(distance * Mathf.Cos(angle * Mathf.Deg2Rad), distance * Mathf.Sin(angle * Mathf.Deg2Rad), 0);
        pos += target.position;
        transform.position = pos;

        angle += spinSpeed * Time.deltaTime;

        if(canIncreaseDistance)
            distance += Time.deltaTime * increaseDistancePerSec;

        if(canIncreaseSpeed)
            spinSpeed += Time.deltaTime * increaseSpeedPerSec;
    }

    public override void Init()
    {
        curTime = 0f;
        target = transform.parent.transform;
        gameObject.transform.SetParent(PoolManager.instance.transform);
        
        Vector2 startVec = GetComponent<BulletBase>().startVec;
        
        float angleInRadians = Mathf.Atan2(startVec.y, startVec.x);
        angle = angleInRadians * Mathf.Rad2Deg;

        
    }
    
    public override void Shoot(Vector2 vec, float force)
    {
        float angleInRadians = Mathf.Atan2(vec.y, vec.x);
        angle = angleInRadians * Mathf.Rad2Deg;
    }
}
