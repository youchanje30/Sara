using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class BSpread : BFunction
{
    [Header("뿌리기 지연시간")]
    [SerializeField] float delayTime;

    [Header("기본 정보")]
    float startAngle;
    [SerializeField] float defaultAngle;
    [SerializeField] float fixAngle;
    [SerializeField] float nextShootTime;
    [SerializeField] float nextAngle;
    [SerializeField] int bulletNum;
    [SerializeField] float[] shootForce;
    [SerializeField] bool isRepeat;
    [SerializeField] GameObject[] spreadBullet;

    
    [SerializeField] bool isStopAfterSpread;
    [SerializeField] bool isRemoveAfterSpread;


    [Header("각도 지정 여부")]
    [SerializeField] bool isAngleRange;
    [SerializeField] float maxAngle;
    [SerializeField] float minAngle;
    float recentAngle;

    List<BulletBase> bullets = new List<BulletBase>();
    
    public override void Init()
    {
        StartCoroutine(Spread());
        Vector2 startVec = GetComponent<BulletBase>().startVec;
        startAngle = Mathf.Atan2(startVec.y, startVec.x) * Mathf.Rad2Deg;
    }

    public override void FunctionUpdate()
    {

    }
    IEnumerator Spread()
    {
        recentAngle = 0f;
        float _nextAngle = nextAngle;
        int i = 0;
        int _bulletNum = bulletNum;

        yield return new WaitForSeconds(delayTime);

        while (_bulletNum > 0)
        {
            GameObject Bullet = PoolManager.instance.GetBullet1(PoolManager.instance.GetNumToString(spreadBullet[i % spreadBullet.Length].name));
            Bullet.transform.SetParent(transform);
            Bullet.transform.position = transform.position;

            recentAngle += _nextAngle;
            if(isAngleRange)
            {
                if(recentAngle > maxAngle)
                {
                    recentAngle = maxAngle;
                    _nextAngle *= -1;
                }
                else if(recentAngle < minAngle)
                {
                    recentAngle = minAngle;
                    _nextAngle *= -1;
                }
            }

            float angle = (recentAngle + defaultAngle + startAngle + fixAngle) * Mathf.Deg2Rad;

            Vector2 vec = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
            foreach (BFunction item in Bullet.GetComponents<BFunction>())
                item.Shoot(vec, shootForce[i % shootForce.Length]);
            
            BulletBase bulletBase = Bullet.GetComponent<BulletBase>();

            bulletBase.ownBullet = GetComponent<BulletBase>().ownBullet;
            bulletBase.startVec = vec;
            bulletBase.damage = GetComponent<BulletBase>().damage;
            bulletBase.Init();

            yield return new WaitForSeconds(nextShootTime);
            
            
            if(!isRepeat)
                _bulletNum --;
        }

        if(isStopAfterSpread)
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        if(isRemoveAfterSpread)
        {
            gameObject.SetActive(false);
        }
            
    }

    public override void Shoot(Vector2 vec, float force)
    {
        GetComponent<Rigidbody2D>().AddForce(vec * force, ForceMode2D.Impulse);
    }


    void OnDisable()
    {
        DBG.DebugerNN.Debug("Disabled " + transform.name);
    }
}
