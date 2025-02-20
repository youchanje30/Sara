using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemyPistol : Gun<Enemy>
{
    public override GunData data
    { 
        get { return _data; }
        set { _data = value; }
    }
    [SerializeField] GunData _data;
    int spawnIndex = 0;

    public override void Shoot(Enemy entity)
    {
        GameObject Bullet = PoolManager.instance.GetBullet1(PoolManager.instance.GetNumToString(data.bullet[spawnIndex++ % data.bullet.Length].name));
        Bullet.transform.position = entity.shootTrans.transform.position;
        
        BulletBase bulletBase = Bullet.GetComponent<BulletBase>();

        float zAngle = entity.transform.rotation.eulerAngles.z;
        Vector2 startVec = Quaternion.Euler(0, 0, zAngle) * Vector2.right;
        float directionAngle = Mathf.Atan2(startVec.y, startVec.x) * Mathf.Rad2Deg;
        
        directionAngle += data.fixAngle * Random.Range(-1f, 1f);
        directionAngle *= Mathf.Deg2Rad;

        Vector2 direction = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle)).normalized;

        bulletBase.ownBullet = OwnBullet.Enemy;
        bulletBase.startVec = direction;
        bulletBase.damage = data.damage;
        foreach (BFunction item in Bullet.GetComponents<BFunction>())
            item.Shoot(direction, data.shootForce);
        bulletBase.Init();
    }
}
