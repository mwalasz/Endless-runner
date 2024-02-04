using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public enum AsyncOperationStatus
    {
        NotStarted = 0,

        InProgress,

        Succeeded,

        Failed,
    }
}