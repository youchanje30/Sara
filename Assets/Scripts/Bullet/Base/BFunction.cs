using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BFunction : MonoBehaviour
{
    public abstract void Init();
    
    public abstract void FunctionUpdate();
    public abstract void Shoot(Vector2 vec, float force);
}
