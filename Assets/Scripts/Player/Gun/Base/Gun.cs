using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun<T> : MonoBehaviour
{
    public abstract GunData      data { get; set;}

    public abstract void Shoot(T entity);
}