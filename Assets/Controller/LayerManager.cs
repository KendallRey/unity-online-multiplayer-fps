
using UnityEngine;

public class LayerManager : PlayerView
{
    const string WEAPONS_LAYER = "Weapons";
    [SerializeField] GameObject[] objectsToApplyLayer;

    private void Start()
    {
        if(!view.IsMine) return;
        foreach(GameObject obj in objectsToApplyLayer)
        {
            obj.layer = LayerMask.NameToLayer(WEAPONS_LAYER);
        }
    }

}
