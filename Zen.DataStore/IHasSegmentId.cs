namespace Zen.DataStore
{
    /// <summary>
    /// ������ ��� �������� � ��������
    /// </summary>
    public interface IHasSegmentId
    {
        /// <summary>
        /// �� ��������
        /// </summary>
        string SegmentId { get; set; }
    }
}