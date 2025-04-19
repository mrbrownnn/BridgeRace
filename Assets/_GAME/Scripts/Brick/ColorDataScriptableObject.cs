namespace _GAME.Scripts
{
    using UnityEngine;
    using System.Collections.Generic;
    using _GAME.Scripts.Enum;

    [CreateAssetMenu(fileName = "ColorData", menuName = "Bridge Race/Color Data")]
    public class ColorData : ScriptableObject
    {
        public List<Material> colorMaterials = new List<Material>();
    
        public Material GetMaterialByColorType(ColorType colorType)
        {
            int index = (int)colorType;
            if (index >= 0 && index < colorMaterials.Count)
            {
                return colorMaterials[index];
            }
            return null;
        }
    }
}