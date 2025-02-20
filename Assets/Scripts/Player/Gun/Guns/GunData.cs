using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Gun Data", menuName = "ScriptableObject/Gun Data")]
public class GunData : ScriptableObject, IGun
{
    public float damage
    { 
        get { return _damage; }
        set { _damage = value; }
    }
    public float reshootCoolDown
    { 
        get { return _reshootCoolDown; }
        set { _reshootCoolDown = value; }
    }
    public float shootForce
    {
        get { return _shootForce; }
        set { _shootForce = value; }
    }
    public GameObject[] bullet 
    { 
        get { return _bullet; }
        set { _bullet = value; }
    }
    public float fixAngle
    {
        get { return _fixAngle; }
        set { _fixAngle = value; }
    }

    [SerializeField] float _damage;
    [SerializeField] float _reshootCoolDown;
    [SerializeField] float _shootForce;
    [SerializeField] float _fixAngle;
    [SerializeField] GameObject[] _bullet;

}
