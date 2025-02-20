using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerCheckable
{
    bool isWithoutRemoveDistance { get; set; }
    bool isWithinStrikingDistance { get; set; }

    void SetRemoveDistanceBool(bool isWithoutRemoveDistance);
    void SetStrikingDistanceBool(bool isWithinStrikingDistance);
}
