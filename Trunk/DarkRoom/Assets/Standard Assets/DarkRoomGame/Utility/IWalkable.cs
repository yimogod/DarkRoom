namespace DarkRoom.Game
{
	public interface IWalkableGrid
	{
		bool IsWalkable(int col, int row);
	    void SetWalkable(int col, int row, bool value);
	}

    public interface IWalkableNode
    {
        /// <summary>
        /// The row.
        /// </summary>
        int Row { get; set; }

        /// <summary>
        /// The col.
        /// </summary>
        int Col { get; set; }

        /// <summary>
        /// The walkable.
        /// </summary>
        bool Walkable { get; set; }
    }
}