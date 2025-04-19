namespace _GAME.Scripts
{
    using UnityEngine;
    using _GAME.Scripts.Enum;

    public class GroundBrick : MonoBehaviour
    {
        [SerializeField] private Renderer brickRenderer;
        private ColorType _colorType;
        private bool _isCollected = false;

        public ColorType ColorType
        {
            get { return _colorType; }
        }

        public bool IsCollected
        {
            get { return _isCollected; }
            set { _isCollected = value; }
        }

        public void Init(ColorType colorType, Material material)
        {
            _colorType = colorType;
            ChangeColor(material);
            _isCollected = false;
            gameObject.SetActive(true);
        }

        public void ChangeColor(Material material)
        {
            if (brickRenderer != null && material != null)
            {
                brickRenderer.material = material;
            }
        }

        public void Collect()
        {
            _isCollected = true;
            gameObject.SetActive(false);
        }
    }
}