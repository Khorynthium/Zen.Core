using System;

namespace Zen.DataStore
{
    /// <summary>
    ///     ������ � GUID ���������������
    /// </summary>
    public interface IHasGuidId : IHasStringId
    {
        /// <summary>
        ///     ���� ������
        /// </summary>
        Guid Guid { get; set; }
    }
}