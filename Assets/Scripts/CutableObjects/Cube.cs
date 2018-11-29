using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Valve.VR.InteractionSystem;

namespace AnythingCut
{
    public class Cube : CutObject, ICutable
    {
        enum SplitDirection
        {
            Left,
            Right,
        }
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Cut();
            }
        }
        public override void Cut()
        {
            if (!CanCut())
                return;

            NowCutLevel++;

            // Instantiate
            var obj = Instantiate(gameObject);
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            obj.GetComponent<CutObject>().NowCutLevel = NowCutLevel;

            // ReName
            gameObject.name = "Cube Level : " + NowCutLevel;
            obj.name = "Cube Level : " + NowCutLevel;
            
            // new mesh
            var mesh = gameObject.GetComponent<MeshFilter>().mesh;
            obj.GetComponent<MeshFilter>().mesh = CopyMesh(mesh);

            // mesh adjust
            AdjustmentMesh(gameObject, SplitDirection.Left);
            AdjustmentMesh(obj, SplitDirection.Right);
            //collider adjust
            AdjustmentCollider(gameObject, SplitDirection.Left);
            AdjustmentCollider(obj, SplitDirection.Right);
        }

        public void TakeCut()
        {
            Cut();
        }

        private void AdjustmentMesh(GameObject meshGameObject, SplitDirection splitDirection)
        {
            var mesh = meshGameObject.GetComponent<MeshFilter>().mesh;
            if (mesh == null)
                return;

            var vertices = mesh.vertices;
            
            var xMax = vertices.Max(v => v.x);
            var xMin = vertices.Min(v => v.x);
            var centerPosition = (xMin + xMax) / 2f;

            for (var i = 0; i < vertices.Length; i++)
            {
                switch (splitDirection)
                {
                    case SplitDirection.Left:
                        if (vertices[i].x >= centerPosition)
                        {
                            vertices[i].x = centerPosition;
                        }
                        break;
                    case SplitDirection.Right:
                        if (vertices[i].x <= centerPosition)
                        {
                            vertices[i].x = centerPosition;
                        }
                        break;
                }
            }

            mesh.vertices = vertices;
            mesh.RecalculateBounds();
        }

        private void AdjustmentCollider(GameObject colliderGameObject, SplitDirection splitDirection)
        {
            var collider = colliderGameObject.GetComponent<BoxCollider>();
            if (collider == null)
                return;

            // ReSize
            var colSize = collider.size;
            colSize.x /= 2f;
            collider.size = colSize;

            // ReCenter
            var mesh = colliderGameObject.GetComponent<MeshFilter>().mesh;

            var vertices = mesh.vertices;

            var xMax = vertices.Max(v => v.x);
            var xMin = vertices.Min(v => v.x);
            var centerPosition = (xMin + xMax) / 2f;

            var colCenter = collider.center;
            colCenter.x = centerPosition;
            collider.center = colCenter;
        }

        private static Mesh CopyMesh(Mesh mesh)
        {
            var copyMesh = new Mesh
            {
                vertices = mesh.vertices,
                triangles = mesh.triangles,
                uv = mesh.uv,
                normals = mesh.normals,
                colors = mesh.colors,
                tangents = mesh.tangents
            };
            return copyMesh;
        }
    }
}
