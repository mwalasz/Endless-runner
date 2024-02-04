using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    /// <summary>
    /// An enumeration for the available calendars.
    /// </summary>
    public enum Calendar
    {
        // <summary> Identifier for the Buddhist calendar. </summary>
        Buddhist = 1,

        // <summary> Identifier for the Chinese calendar. </summary>
        Chinese,

        // <summary> Identifier for the Coptic calendar. </summary>
        Coptic,

        // <summary> Identifier for the Ethiopic (Amete Alem) calendar. </summary>
        EthiopicAmeteAlem,

        // <summary> Identifier for the Ethiopic (Amete Mihret) calendar. </summary>
        EthiopicAmeteMihret,

        // <summary> Identifier for the Gregorian calendar. </summary>
        Gregorian,

        // <summary> Identifier for the Hebrew calendar. </summary>
        Hebrew,

        // <summary> Identifier for the Indian calendar. </summary>
        Indian,

        // <summary> Identifier for the Islamic calendar. </summary>
        Islamic,

        // <summary> Identifier for the Islamic civil calendar. </summary>
        IslamicCivil,

        // <summary> Identifier for the tabular Islamic calendar. </summary>
        IslamicTabular,

        // <summary> Identifier for the Islamic Umm al-Qura calendar, as used in Saudi Arabia. </summary>
        IslamicUmmAlQura,

        // <summary> Identifier for the ISO8601 calendar. </summary>
        Iso8601,

        // <summary> Identifier for the Japanese calendar. </summary>
        Japanese,

        // <summary> Identifier for the Persian calendar. </summary>
        Persian,

        // <summary> Identifier for the Republic of China (Taiwan) calendar. </summary>
        RepublicOfChina,
    }
}