using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialoguePanel
{
    /// <summary>
    /// Sets gameObject active and launches animation of appearing of the panel
    /// </summary>
    void Show();

    /// <summary>
    /// Animates disappearing of the panel and sets gameObject inactive.
    /// </summary>
    void Hide();

    /// <summary>
    /// Sets gameObject active and makes the panel fully appear instantly without animation.
    /// </summary>
    void ShowInstantly();

    /// <summary>
    /// Sets gameObject inactive and makes it ready to "Appear()" again.
    /// </summary>
    void HideInstantly();
}
