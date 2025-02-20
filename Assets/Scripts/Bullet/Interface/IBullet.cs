using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet
{
    /// <summary>
    /// 발사되는 총알의 데미지 값
    /// </summary>
    float damage { get; set; }

}
