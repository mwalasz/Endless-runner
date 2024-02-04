using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public static class UnityEngineUtility 
    {
        #region Screen methods

        public static Vector2 InvertScreenPosition(Vector2 position, bool invertX = true, bool invertY = true)
        {
            if (invertX)
            {
                position.x  = Screen.width - position.x;
            }
            if (invertY)
            {
                position.y  = Screen.height - position.y;
            }

            return position;
        }

        #endregion
    }
}