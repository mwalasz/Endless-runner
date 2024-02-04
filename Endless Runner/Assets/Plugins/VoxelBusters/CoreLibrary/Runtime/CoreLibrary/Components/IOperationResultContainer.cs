namespace VoxelBusters.CoreLibrary
{
    public interface IOperationResultContainer<TData>
    {
        #region Methods

        bool IsError();

        Error GetError();

        TData GetResult();

        string GetResultAsText();

        #endregion
    }
}