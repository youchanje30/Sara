using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Pistol : Gun<Player>
{
    public override GunData data
    { 
        get { return _data; }
        set { _data = value; }
    }
    [SerializeField] GunData _data;
    int spawnIndex = 0;
    bool canShoot = true;

    public override void Shoot(Player entity)
    {
        if(!canShoot) return;
        canShoot = false;

        GameObject Bullet = PoolManager.instance.GetBullet1(PoolManager.instance.GetNumToString(data.bullet[spawnIndex++ % data.bullet.Length].name));
        Bullet.transform.position = entity.shootTrans.transform.position;

        BulletBase bulletBase = Bullet.GetComponent<BulletBase>();
        
        float zAngle = entity.transform.rotation.eulerAngles.z;
        Vector2 direction = Quaternion.Euler(0, 0, zAngle) * Vector2.right;
        float directionAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        directionAngle += data.fixAngle * Random.Range(-1f, 1f);
        directionAngle *= Mathf.Deg2Rad;

        direction = new Vector2(Mathf.Cos(directionAngle), Mathf.Sin(directionAngle)).normalized;

        bulletBase.damage = data.damage;
        bulletBase.ownBullet = OwnBullet.Player;
        bulletBase.startVec = direction;
        foreach (BFunction item in Bullet.GetComponents<BFunction>())
            item.Shoot(direction, data.shootForce);
        bulletBase.Init();

        Invoke(nameof(Reload), data.reshootCoolDown);
    }

    void Reload()
    {
        canShoot = true;
    }
}
