using UnityEngine;
using UnityEngine.UI;

namespace QDuck.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class UGUIEmptyRaycast : MaskableGraphic
    {
        protected UGUIEmptyRaycast()
        {
            useLegacyMeshGeneration = false;
        }
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    }
}