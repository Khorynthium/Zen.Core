namespace Zen.DataStore
{
    /// <summary>
    ///     ������ � �������� ���������������
    /// </summary>
    public interface IHasIntId : IHasSegmentId
    {
        /// <summary>
        ///     �� ������
        /// </summary>
        int Id { get; set; }
    }
}