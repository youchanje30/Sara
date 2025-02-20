using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSpinParent : BFunction
{

    float startAngle;
    [SerializeField] float defaultAngle;
    [SerializeField] float distance;
    [SerializeField] float spinSpeed;
    [SerializeField] int bulletNum;
    List<BFunction> bullets;

    [SerializeField] GameObject[] spinBullets;

    public override void FunctionUpdate()
    {
        foreach (BFunction bullet in bullets)
        {
            bullet.FunctionUpdate();
        }
    }

    public override void Init()
    {
        Vector2 startVec = GetComponent<BulletBase>().startVec;
        startAngle = Mathf.Atan2(startVec.y, startVec.x) * Mathf.Rad2Deg;

        float angle = 360f / bulletNum;
        bullets = new List<BFunction>();
        for (int i = 0; i < bulletNum; i++)
        {

            // GameObject Bullet = Instantiate(spinBullet, transform);
            // GameObject Bullet = PoolManager.instance.GetBulletString(spinBullets[i % spinBullets.Length].name);
            GameObject Bullet = PoolManager.instance.GetBullet1(PoolManager.instance.GetNumToString(spinBullets[i % spinBullets.Length].name));
            Bullet.transform.SetParent(transform);
            bullets.Add(Bullet.GetComponent<BSpin>());

            BSpin SpinBullet = Bullet.GetComponent<BSpin>();
            SpinBullet.angle = i * angle + defaultAngle + startAngle;
            SpinBullet.spinSpeed = spinSpeed;
            SpinBullet.distance = distance;
        }
    }
    
    public override void Shoot(Vector2 vec, float force)
    {
        GetComponent<Rigidbody2D>().AddForce(vec * force, ForceMode2D.Impulse);
    }
}
