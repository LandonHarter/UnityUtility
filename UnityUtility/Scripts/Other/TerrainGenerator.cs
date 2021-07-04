using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainGenerator : MonoBehaviour
{

    [Range(1, 255)] public int size = 20;

    [Space]

    [Header("Noise Options")]
    [Range(-9999999, 9999999)] public int seed = 0;
    public float scale = 32;
    public float amplitude = 8;
    public Vector2 offset = Vector2.zero;

    private MeshFilter filter;
    [HideInInspector] public bool generated = false;
    private float minHeight;
    private float maxHeight;

    void Awake()
    {
        if (!generated)
        {
            GenerateMesh();
        }
    }

    void Update()
    {
        if (generated) GenerateMesh();
    }

    public void GenerateMesh()
    {
        filter = GetComponent<MeshFilter>();

        Mesh terrain = new Mesh();
        terrain.name = "Terrain";

        Vector3[] vertices = new Vector3[(size + 1) * (size + 1)];
        int[] indices = new int[size * size * 6];
        Vector2[] uvs = new Vector2[vertices.Length];
        Color[] color = new Color[vertices.Length];

        for (int i = 0, x = 0; x <= size; x++)
        {
            for (int z = 0; z <= size; z++)
            {
                vertices[i] = new Vector3(x, GenerateNoise(x, z), z);

                if (vertices[i].y > maxHeight) maxHeight = vertices[i].y;
                if (vertices[i].y < minHeight) minHeight = vertices[i].y;

                i++;
            }
        }

        int vert = 0;
        int tris = 0;
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                indices[tris + 5] = vert + 0;
                indices[tris + 4] = vert + size + 1;
                indices[tris + 3] = vert + 1;
                indices[tris + 2] = vert + 1;
                indices[tris + 1] = vert + size + 1;
                indices[tris + 0] = vert + size + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        for (int i = 0, x = 0; x <= size; x++)
        {
            for (int z = 0; z <= size; z++)
            {
                uvs[i] = new Vector2((float)x / size, (float)z / size);

                i++;
            }
        }

        terrain.vertices = vertices;
        terrain.triangles = indices;

        terrain.colors = Array.Empty<Color>();
        terrain.uv = uvs;

        terrain.RecalculateNormals();
        filter.sharedMesh = terrain;

        generated = true;
    }

    public void ClearMesh()
    {
        filter = GetComponent<MeshFilter>();
        filter.sharedMesh.Clear();
        filter.sharedMesh.RecalculateNormals();

        generated = false;
    }

    private float GenerateNoise(int x, int z)
    {
        float height = Mathf.PerlinNoise((x + offset.x + (seed * 3)) / scale, (z + offset.y + (seed * 3)) / scale) * amplitude;
        return height;
    }
}
