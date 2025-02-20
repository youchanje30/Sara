using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : Bullet
{
    public override float damage { get; set; }
    public override OwnBullet ownBullet { get; set; }
    public override BFunction[] functions
    {
        get { return bulletFunctions; }
        set { bulletFunctions = value; } 
    }
    [SerializeField] BFunction[] bulletFunctions;
    public Vector2 startVec;

    public override void Init()
    {
        foreach (BFunction function in functions)
        {
            function.Init();
        }
    }

    void Update()
    {   
        foreach (BFunction function in functions)
        {
            function.FunctionUpdate();
        }
    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy") && ownBullet == OwnBullet.Player)
        {
            other.GetComponent<Enemy>().Damage(damage);
            gameObject.SetActive(false);
        }
        else if(other.CompareTag("Player") && ownBullet == OwnBullet.Enemy)
        {
            other.GetComponent<Player>().GetDamage(damage);
            gameObject.SetActive(false);
        }
        else if(other.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        PoolManager.instance.SetBullet(PoolManager.instance.GetNumToString(gameObject.name), gameObject);

    }
}
