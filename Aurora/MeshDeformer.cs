using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour
{

    Mesh deformingMesh;
    Vector3[] originalVertices, displacedVertices;
    Vector3[] vertexVelocities;

    public float springForce = 10f;
    public float damping = 5f;

    private float minMag;
    private Vector3 minVector3;

    // Use this for initialization
    void Start()
    {
        /*deformingMesh = GetComponent<MeshFilter>().mesh;
		originalVertices = deformingMesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];
		for (int i = 0; i < originalVertices.Length; i++) {
			displacedVertices[i] = originalVertices[i];
		}
		vertexVelocities = new Vector3[originalVertices.Length];*/
    }

    public void StoreMesh()
    {
        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = deformingMesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
        for (int i = 0; i < originalVertices.Length; i++)
        {
            displacedVertices[i] = originalVertices[i];
        }
        vertexVelocities = new Vector3[originalVertices.Length];
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            UpdateVertex(i);
        }
        deformingMesh.vertices = displacedVertices;
        deformingMesh.RecalculateNormals();
    }

    void UpdateVertex(int i)
    {
        Vector3 velocity = vertexVelocities[i];
        Vector3 displacement = displacedVertices[i] - originalVertices[i];
        velocity -= displacement * springForce * Time.deltaTime;
        velocity *= 1f - damping * Time.deltaTime;
        vertexVelocities[i] = velocity;
        displacedVertices[i] += velocity * Time.deltaTime;
    }

    public void AddDeformingForce(Vector3 point, float velocity)
    {

        point = transform.InverseTransformPoint(point);
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            AddForceToVertex(i, point, velocity);
        }
    }

    private void AddForceToVertex(int i, Vector3 point, float velocity)
    {
        Vector3 pointToVertex = displacedVertices[i] - point;
        Vector3 pointToOriginalVertex = originalVertices[i] - point;
        velocity = velocity / (1f + 10000 * pointToVertex.sqrMagnitude);
        velocity /= Time.deltaTime;
        vertexVelocities[i] += pointToVertex.normalized * velocity;
    }
}

