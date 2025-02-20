using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum OwnBullet { Player, Enemy }

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Bullet : MonoBehaviour, IBullet
{
    public abstract float damage { get; set; }
    public abstract OwnBullet ownBullet { get; set; }

    public abstract BFunction[] functions { get; set; }

    public abstract void Init( );
}
