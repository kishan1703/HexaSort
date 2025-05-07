using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class JellyMesh : MonoBehaviour
{
    private Mesh originalMesh;
    private Mesh clonedMesh;
    private Vector3[] originalVertices;
    private Vector3[] displacedVertices;
    private Vector3[] vertexVelocities;

    public float stiffness = 10f;
    public float damping = 0.75f;
    public float mass = 1f;

    void Start()
    {
        originalMesh = GetComponent<MeshFilter>().mesh;
        clonedMesh = Instantiate(originalMesh);
        GetComponent<MeshFilter>().mesh = clonedMesh;

        originalVertices = originalMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        vertexVelocities = new Vector3[originalVertices.Length];

        for (int i = 0; i < originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i];
        }
    }

    void Update()
    {
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            Vector3 velocity = vertexVelocities[i];
            Vector3 displacement = displacedVertices[i] - originalVertices[i];
            Vector3 force = -stiffness * displacement - damping * velocity;
            Vector3 acceleration = force / mass;
            velocity += acceleration * Time.deltaTime;
            displacedVertices[i] += velocity * Time.deltaTime;
            vertexVelocities[i] = velocity;
        }

        clonedMesh.vertices = displacedVertices;
        clonedMesh.RecalculateNormals();
    }
}
