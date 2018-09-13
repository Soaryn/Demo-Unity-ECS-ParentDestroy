using System;
using Unity.Rendering;
using UnityEngine;

namespace EntityExample {
    [CreateAssetMenu(menuName = "Demo/Settings")]
    public class SettingsHandler : ScriptableObject {
        public MeshInstanceRenderer NodeRenderer;
        public MeshInstanceRenderer LeafRenderer;

        [Range(1, 100)]
        public int NodeCount;

        [Range(1, 100)]
        public int LeafCount;
    }
}