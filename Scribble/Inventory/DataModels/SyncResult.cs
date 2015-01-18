namespace InventoriesSample.DataModels
{
    /// <summary>
    /// Contains all game data needed in the Program, used for complete synchronization between the cliant and the server.
    /// </summary>
    public class SyncResult
    {
        /// <summary>
        /// Units in the unit inventory.
        /// </summary>
        public Unit[] Units { get; set; }
    }
}
