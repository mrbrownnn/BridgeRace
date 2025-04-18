using System.Collections;
using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;

namespace _GAME.Scripts.Core.Object_Pooling.Brick
{
    public enum BRICKSTATUS
    {
        OnGround = 1,
        OnPlayer = 2,
        Invoke = 3,
    }

    public class IEBrick : MonoBehaviour
    {
        #region OnInit
        private void Initialize()
        {
            // Initialization logic for the brick
        }

        private void BrickOnGround()
        {
            Initialize();
            if (GetComponent<IEBrick>() == null)
            {
                // Logic for when the brick is on the ground
            }
            else
            {
                // Logic for when the brick is not on the ground
            }
        }
        

        private void BrickOnPlayer()
        {
            GameObject brickObject = gameObject; // Assuming this script is attached to the brick GameObject
            if (brickObject.CompareTag("BrickOnGround") && brickObject.CompareTag("Player"))
            {
                Initialize();
            }
        }
        #endregion
    }
}