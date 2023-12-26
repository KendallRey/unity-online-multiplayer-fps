
using UnityEngine;

public class LayerManager : PlayerView
{
    [SerializeField] string LayerName;
    [SerializeField] GameObject[] objectsToApplyLayer;
    private void Start()
    {
        if(!view.IsMine) return;
        foreach(GameObject obj in objectsToApplyLayer)
        {
            obj.layer = LayerMask.NameToLayer(LayerName);
        }
    }

}
