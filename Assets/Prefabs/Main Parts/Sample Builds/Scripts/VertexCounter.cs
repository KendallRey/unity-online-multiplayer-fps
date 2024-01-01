using UnityEngine;

public class VertexCounter : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] Transform ParentTransform;
    private int VertexCount = 0;
    void Start()
    {
        if (ParentTransform == null) ParentTransform = transform;
        CountVerts();
    }
    
    private void CountVerts()
    {
        var MFInChildren = ParentTransform.GetComponentsInChildren<MeshFilter>();
        foreach( MeshFilter mf in MFInChildren)
        {
            VertexCount += mf.mesh.vertexCount;
        }
        Debug.Log("Mesh Count: " + MFInChildren.Length.ToString());
        Debug.Log("Vertex Count: " + VertexCount.ToString());
    }
}
