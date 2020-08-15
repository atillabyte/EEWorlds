namespace EEWorlds
{
    public enum WorldFormat
    {
        /// <summary>
        /// TSON format written by miou.
        /// </summary>
        TSON,

        /// <summary>
        /// JSON format written by miou. This format is legacy, deprecated in favor of TSON.
        /// </summary>
        JSON,

        /// <summary>
        /// EELVL format written by LukeM.
        /// </summary>
        EELVL,

        /// <summary>
        /// EELEVEL format written by Cyph1e. Used in EEditor, which is maintained by capasha.
        /// </summary>
        EELEVEL
    }
}