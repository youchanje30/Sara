using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BWaitShoot : BFunction
{

    [SerializeField] float stopTime;
    [SerializeField] float waitTime;
    [SerializeField] float shootForce;
    
    [SerializeField] Transform target;
    
    public override void FunctionUpdate()
    {
        
    }

    public override void Init()
    {
        StartCoroutine(WaitShoot());
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    IEnumerator WaitShoot()
    {
        yield return new WaitForSeconds(stopTime);
        
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        yield return new WaitForSeconds(waitTime);

        Vector2 vec = ((Vector2)target.position - (Vector2)transform.position).normalized;

        // GetComponent<Rigidbody2D>().AddForce(vec * shootForce, ForceMode2D.Impulse);
        Shoot(vec, shootForce);
        
    }

    
    public override void Shoot(Vector2 vec, float force)
    {
        GetComponent<Rigidbody2D>().AddForce(vec * force, ForceMode2D.Impulse);
    }
}
