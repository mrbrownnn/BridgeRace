namespace _GAME.Scripts
{
    using UnityEngine;
    using _GAME.Scripts.Enum;

    public class PlayerBrick : MonoBehaviour
    {
        [SerializeField] private Renderer brickRenderer;
        private ColorType _colorType;

        public ColorType ColorType
        {
            get { return _colorType; }
        }

        public void SetColor(ColorType colorType, Material material)
        {
            _colorType = colorType;
            if (brickRenderer != null && material != null)
            {
                brickRenderer.material = material;
            }
        }
    }

}