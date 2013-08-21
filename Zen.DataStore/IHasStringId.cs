namespace Zen.DataStore
{
    /// <summary>
    /// ������ �� ��������� ���������������
    /// </summary>
    public interface IHasStringId : IHasSegmentId
    {
        /// <summary>
        /// �� ������
        /// </summary>
        string Id { get; set; }
    }
}