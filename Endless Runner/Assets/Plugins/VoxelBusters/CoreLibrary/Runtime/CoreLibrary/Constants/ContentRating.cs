using UnityEngine;
using System.Collections;

namespace VoxelBusters.CoreLibrary
{
    public enum ContentRating
    {
        Unspecified = 0,

        /// <summary> Content suitable for general audiences, including families. </summary>
        GeneralAudience,

        /// <summary> Content suitable only for mature audiences. </summary>
        MatureAudience,

        /// <summary> Content suitable for most audiences with parental guidance. </summary>
        ParentalGuidance,

        /// <summary> Content suitable for teen and older audiences. </summary>
        TeensAndOlder,
    }  
}