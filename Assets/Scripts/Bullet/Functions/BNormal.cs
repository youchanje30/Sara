using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BNormal : BFunction
{
    [SerializeField] float distance;

    public override void FunctionUpdate()
    {
        
    }

    public override void Init()
    {
        Vector2 startVec = GetComponent<BulletBase>().startVec;
        Vector2 startPos = (Vector2)transform.position + startVec.normalized * distance;
        
        transform.position = startPos;

        gameObject.transform.SetParent(PoolManager.instance.transform);

        // Shoot(startVec, f)
    }

    public override void Shoot(Vector2 vec, float force)
    {
        GetComponent<Rigidbody2D>().AddForce(vec * force, ForceMode2D.Impulse);        
    }
}
