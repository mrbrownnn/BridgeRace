namespace _GAME.Scripts
{
    using UnityEngine;
    using System.Collections;
    using _GAME.Scripts.Enum;

    public class Stair : MonoBehaviour
    {
        [SerializeField] private Renderer stairRenderer;
        private ColorType _colorType = ColorType.BLUE; // Default is BLUE

        public ColorType ColorType
        {
            get { return _colorType; }
        }

        public void SetColor(ColorType colorType, Material material)
        {
            _colorType = colorType;
            if (stairRenderer != null && material != null)
            {
                stairRenderer.material = material;
            }
        }
    }
}