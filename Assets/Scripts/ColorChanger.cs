using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    #region Components
    [SerializeField] private List<ParticleSystem> colorParticles;
    #endregion
    #region Variables
    [SerializeField] private ColorPalette palette;
    [SerializeField] private ColorFromPalette colorToSet;
    private bool isChanged = false;
    #endregion
    private void Start()
    {
        SetColorParticles();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isChanged || !other.TryGetComponent(out Skateboard skateboard) || !skateboard.IsUsingByPlayer) return;

        isChanged = true;
        EventManager.TriggerEvent(EventKeys.OnColorChanged,new object[] { colorToSet });
    }

    private void SetColorParticles()
    {
        for(int i = 0;i < colorParticles.Count; i++) 
        {
            var main = colorParticles[i].main;
            main.startColor = palette.colors[(int)colorToSet];
        }
    }
}
