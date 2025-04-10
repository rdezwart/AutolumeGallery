using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the events and resets for all associated <see cref="PanelController"/> objects.
/// </summary>
public class PaintingManager : MonoBehaviour
{
    public List<PanelController> panels = new();
    private readonly List<PanelController> _floating = new();

    private void Start()
    {
        foreach (var panel in panels)
        {
            panel.panelInserted.AddListener(PanelInserted);
            panel.panelRemoved.AddListener(PanelRemoved);
            panel.panelGrabbed.AddListener(PanelGrabbed);
        }
    }

    /// <summary>
    /// Event listener for when a <c>PanelController</c> is inserted into a <c>XRSocketInteractor</c>.
    /// </summary>
    /// <param name="panel">The <c>PanelController</c> invoking this event.</param>
    private void PanelInserted(PanelController panel)
    {
        // Debug.Log("Panel Inserted", panel);
        if (!_floating.Contains(panel)) return;

        var index = _floating.IndexOf(panel);
        for (var i = index + 1; i < _floating.Count; i++)
        {
            var currentPanel = _floating[i];
            currentPanel.transform.Translate(0, 0, 0.15f);
        }

        _floating.Remove(panel);
    }

    /// <summary>
    /// Event listener for when a <c>PanelController</c> is removed from a <c>XRSocketInteractor</c>.
    /// </summary>
    /// <param name="panel">The <c>PanelController</c> invoking this event.</param>
    private void PanelRemoved(PanelController panel)
    {
        // Debug.Log("Panel Removed", panel);

        _floating.Add(panel);
        var index = _floating.IndexOf(panel);

        panel.transform.Translate(0, 0, -0.06f);
        panel.transform.Translate(0, 0, -0.15f * (index + 1));
    }

    /// <summary>
    /// Event listener for when a <c>PanelController</c> is grabbed by the player.
    /// </summary>
    /// <remarks>Currently unused.</remarks>
    /// <param name="panel">The <c>PanelController</c> invoking this event.</param>
    private void PanelGrabbed(PanelController panel)
    {
        // Debug.Log("Panel Grabbed");

        // This code is for rearranging the indexes of the tiles as they're picked up.
        // Didn't end up being necessary, found a simpler solution with the others, but might as well keep it.

        // var index = floating.IndexOf(panel);
        // for (var i = index; i < floating.Count; i++)
        // {
        //     var p = floating[i];
        //     
        //     if (i == index)
        //     {
        //         var change = floating.Count - 1 - index;
        //         p.distance += change;
        //         Debug.Log($"Moving forward {change}", p);
        //         // p.transform.Translate(0, 0, 0.2f * change);
        //     }
        //     else if (i > index)
        //     {
        //         p.distance--;
        //     }
        // }
    }

    public void ResetAllPanels()
    {
        _floating.Clear();
        foreach (var panel in panels)
        {
            panel.ResetPanel();
        }
    }
}