using UnityEngine;

public class MeshEvaluator : MonoBehaviour
{
    [SerializeField] Color vertexColor = Color.white;
    [SerializeField] Color normalColor = Color.green;
    [SerializeField] Color tangentColor = Color.blue;
    [SerializeField] float vertexRadius = .1f;
    [SerializeField] float normalLength = .5f;
    [SerializeField] float tangentLength = .25f;

    [Header("Readonly values")]
    [SerializeField] int vertexCount;
    [SerializeField] int triangleCount;

    private Mesh mesh;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertexCount = mesh.vertexCount;
        triangleCount = mesh.triangles.Length;
    }

    private void OnDrawGizmosSelected()
    {
        if (!mesh) return;
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            var vertex = mesh.vertices[i];
            var normal = mesh.normals[i];
            var tangent4 = mesh.tangents[i];
            var tangent = new Vector3(tangent4.x, tangent4.y, tangent4.z);

            //if (tangent4.w != 1) Debug.LogWarning($"Tangent {i} w = {tangent4.w}");

            Gizmos.color = vertexColor;
            Gizmos.DrawSphere(vertex, vertexRadius);

            Gizmos.color = normalColor;
            Gizmos.DrawLine(vertex, vertex + normal * normalLength);

            Gizmos.color = tangentColor;
            Gizmos.DrawLine(vertex, vertex + tangent * tangentLength);
        }
    }
}
