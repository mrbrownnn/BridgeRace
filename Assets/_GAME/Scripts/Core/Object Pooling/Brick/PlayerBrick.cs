using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
namespace _GAME.Scripts.Core.Object_Pooling.Brick
{
    public class PlayerBrick : MonoBehaviour
    {
        [SerializeField] private List<GameObject> bricks;
        private void Start()
        {
            bricks = new List<GameObject>();
        }
    }
}