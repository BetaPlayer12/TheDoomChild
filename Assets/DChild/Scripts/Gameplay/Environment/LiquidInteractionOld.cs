using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is gotten from Water Area Script of the 2D GameKit

namespace DChildDebug
{
    [ExecuteInEditMode]
    public class LiquidInteraction : MonoBehaviour
    {
        protected struct WaterColumn
        {
            public float currentHeight;
            public float baseHeight;
            public float velocity;
            public float xPosition;
            public int vertexIndex;
        }

        const float NEIGHBOUR_TRANSFER = 0.001f;

        [SerializeField, BoxGroup("Mesh Creation"), MinValue(1), OnValueChanged("OnEditorRecomposition")]
        private int pointPerUnits = 5;
        [SerializeField, BoxGroup("Mesh Creation"), OnValueChanged("OnEditorRecomposition", true)]
        private Vector2 size = new Vector2(6f, 2f);

        [SerializeField, BoxGroup("Liquid")]
        private float dampening = 0.93f;
        [SerializeField, BoxGroup("Liquid")]
        private float tension = 0.025f;
        [SerializeField, BoxGroup("Liquid")]
        private float neighbourTransfer = 0.03f;

        protected MeshFilter m_Filter;
        protected Mesh m_Mesh;

        protected WaterColumn[] m_Columns;
        protected float m_Width;
        protected Vector2 m_LowerCorner;

        protected Vector3[] meshVertices;

        private void OnEnable()
        {
            GetReferences();
            RecomputeMesh();
            meshVertices = m_Mesh.vertices;
        }

        public void GetReferences()
        {
            m_Filter = GetComponent<MeshFilter>();
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < m_Columns.Length; ++i)
            {
                //float ratio = ((float)i) / m_columns.Length;

                float leftDelta = 0;
                if (i > 0)
                    leftDelta = neighbourTransfer * (m_Columns[i - 1].currentHeight - m_Columns[i].currentHeight);

                float rightDelta = 0;
                if (i < m_Columns.Length - 1)
                    rightDelta = neighbourTransfer * (m_Columns[i + 1].currentHeight - m_Columns[i].currentHeight);

                float force = leftDelta;
                force += rightDelta;
                force += tension * (m_Columns[i].baseHeight - m_Columns[i].currentHeight);

                m_Columns[i].velocity = dampening * m_Columns[i].velocity + force;

                m_Columns[i].currentHeight += m_Columns[i].velocity;
            }

            for (int i = 0; i < m_Columns.Length; ++i)
            {
                meshVertices[m_Columns[i].vertexIndex].y = m_Columns[i].currentHeight;
            }

            m_Mesh.vertices = meshVertices;

            m_Mesh.UploadMeshData(false);
        }

        public void RecomputeMesh()
        {
            //we recreate the mesh as we the previous one could come from prefab (and so every object would the same when they each need there...)
            //ref countign should take care of leaking, (and if it's a prefabed mesh, the prefab keep its mesh)
            m_Mesh = new Mesh();
            m_Mesh.name = "LiquidMesh";
            m_Filter.sharedMesh = m_Mesh;

            m_LowerCorner = -(size * 0.5f);

            m_Width = size.x;

            int count = Mathf.CeilToInt(size.x * (pointPerUnits - 1)) + 1;

            m_Columns = new WaterColumn[count + 1];

            float step = size.x / count;

            Vector3[] pts = new Vector3[(count + 1) * 2];
            Vector3[] normal = new Vector3[(count + 1) * 2];
            Vector2[] uvs = new Vector2[(count + 1) * 2];
            Vector2[] uvs2 = new Vector2[(count + 1) * 2];
            int[] indices = new int[6 * count];

            for (int i = 0; i <= count; ++i)
            {
                pts[i * 2 + 0].Set(m_LowerCorner.x + step * i, m_LowerCorner.y, 0);
                pts[i * 2 + 1].Set(m_LowerCorner.x + step * i, m_LowerCorner.y + size.y, 0);

                normal[i * 2 + 0].Set(0, 0, 1);
                normal[i * 2 + 1].Set(0, 0, 1);

                uvs[i * 2 + 0].Set(((float)i) / count, 0);
                uvs[i * 2 + 1].Set(((float)i) / count, 1);

                //Set the 2nd uv set to local position, allow for coherent tiling of normal map
                uvs2[i * 2 + 0].Set(pts[i * 2 + 0].x, pts[i * 2 + 0].y);
                uvs2[i * 2 + 1].Set(pts[i * 2 + 1].x, pts[i * 2 + 1].y);

                if (i > 0)
                {
                    int arrayIdx = (i - 1) * 6;
                    int startingIdx = (i - 1) * 2;

                    indices[arrayIdx + 0] = startingIdx;
                    indices[arrayIdx + 1] = startingIdx + 1;
                    indices[arrayIdx + 2] = startingIdx + 3;

                    indices[arrayIdx + 3] = startingIdx;
                    indices[arrayIdx + 4] = startingIdx + 3;
                    indices[arrayIdx + 5] = startingIdx + 2;
                }

                m_Columns[i] = new WaterColumn();
                m_Columns[i].xPosition = pts[i * 2].x;
                m_Columns[i].baseHeight = pts[i * 2 + 1].y;
                m_Columns[i].velocity = 0;
                m_Columns[i].vertexIndex = i * 2 + 1;
                m_Columns[i].currentHeight = m_Columns[i].baseHeight;
            }

            m_Mesh.Clear();

            m_Mesh.vertices = pts;
            m_Mesh.normals = normal;
            m_Mesh.uv = uvs;
            m_Mesh.uv2 = uvs2;
            m_Mesh.triangles = indices;

            meshVertices = m_Mesh.vertices;

            m_Mesh.UploadMeshData(false);
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.isTrigger == false)
            {
                Rigidbody2D rb = collision.GetComponentInParent<Rigidbody2D>();
                if (rb == null || rb.bodyType == RigidbodyType2D.Static)
                    return; //we don't care about static rigidbody, they can't "fall" in water

                Bounds bounds = collision.bounds;

                List<int> touchedColumnIndices = new List<int>();
                float divisionWith = m_Width / m_Columns.Length;

                Vector3 localMin = transform.InverseTransformPoint(bounds.min);
                Vector3 localMax = transform.InverseTransformPoint(bounds.max);

                // find all our springs within the bounds
                var xMin = localMin.x;
                var xMax = localMax.x;

                for (var i = 0; i < m_Columns.Length; i++)
                {
                    if (m_Columns[i].xPosition > xMin && m_Columns[i].xPosition < xMax)
                        touchedColumnIndices.Add(i);
                }

                // if we have no hits we should loop back through and find the 2 closest verts and use them
                if (touchedColumnIndices.Count == 0)
                {
                    for (var i = 0; i < m_Columns.Length; i++)
                    {
                        // widen our search to included divisitionWidth padding on each side so we definitely get a couple hits
                        if (m_Columns[i].xPosition + divisionWith > xMin && m_Columns[i].xPosition - divisionWith < xMax)
                            touchedColumnIndices.Add(i);
                    }
                }

                float testForce = 0.2f;
                for (int i = 0; i < touchedColumnIndices.Count; ++i)
                {
                    int idx = touchedColumnIndices[i];
                    m_Columns[idx].velocity -= testForce;
                }
            }
        }

#if UNITY_EDITOR
        private void OnEditorRecomposition()
        {
            GetReferences();
            RecomputeMesh();
            meshVertices = m_Mesh.vertices;
            if (TryGetComponent(out BoxCollider2D collider))
            {
                var colliderSize = collider.size;
                colliderSize.Set(size.x, size.y);
                collider.size = colliderSize;
            }
            else
            {
                gameObject.AddComponent<BoxCollider2D>();
            }
        }
#endif
    }
}