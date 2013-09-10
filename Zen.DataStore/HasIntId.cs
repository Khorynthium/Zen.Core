using System;

namespace Zen.DataStore
{
    [Serializable]
    public abstract class HasIntId : IHasIntId
    {
        /// <summary>
        ///     �� ������
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        ///     �� ��������
        /// </summary>
        public virtual string SegmentId { get; set; }
    }
}