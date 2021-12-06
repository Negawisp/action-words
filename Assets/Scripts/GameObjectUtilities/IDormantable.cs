using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// Marks that an object can be put dormant by IDormantCaller class and woken up by sertain
/// circumstances.
/// May or may not notify the IDormantCaller of being woken up.
/// </summary>
public interface IDormantable
{
    /// <summary>
    /// Forces the instance to "go dormant" (falling asleep):
    /// 
    /// The definition of going dormant is up to an implementation,
    /// this may, for instance, be the calling of "setActive(false)",
    /// or creating an only-clickable-once panel, clicking which
    /// a "DormantWakeUp()" method will be called.
    /// 
    /// </summary>
    /// <param name="dormantCaller">A caller-instance of this method.</param>
    void GoDormant(IDormantCaller dormantCaller);

    /// <summary>
    /// Is called to change the state of an instance to "not dormant".
    /// 
    /// The definition is up to an implementation:
    /// this may be as an undoing of effects of "GoDormant(...)" method,
    /// as anything absolutely irrelative.
    /// 
    /// </summary>
    void DormantWakeUp(bool notifyCaller);
}
