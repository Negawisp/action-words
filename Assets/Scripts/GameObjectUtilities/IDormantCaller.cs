using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Marks this class as a one that can make IDormantable objects dormantable.
/// 
/// Implementation tracks all the IDormantables it made dormant and is able to react to
/// "not dormant anymore" notifications from them.
/// </summary>
public interface IDormantCaller
{
    void MakeDormant(IDormantable dormantable);
    void NotDormantNotify(IDormantable dormantable);
}
