using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGun
{
    /// <summary>
    /// 발사된 총알의 데미지 값
    /// </summary>
    float damage            { get; set; } 


    /// <summary>
    /// 다시 총을 발사하기 까지의 총의 쿨타임
    /// </summary>
    float reshootCoolDown   { get; set; }

    /// <summary>
    /// 총알 힘
    /// </summary>
    float shootForce        { get; set; }
    
    /// <summary>
    /// 총에서 사용되는 총알
    /// </summary>
    GameObject[] bullet       { get; set; }

}
