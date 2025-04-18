using System.Collections;
using System.Collections.Generic;
using _GAME.Scripts.Core.Object_Pooling.Brick;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    void BrickManager(){
         List<GameObject> currentBrick = new List<GameObject>();
          // Logic to manage the brick pool
    }
}
public enum State
    {
        IDLE = 1,
        RUN = 2,
        COLLISION = 3,
    }