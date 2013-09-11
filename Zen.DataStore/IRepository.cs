using System;
using System.Collections.Generic;
using System.Linq;

namespace Zen.DataStore
{
    /// <summary>
    ///     ����������� ��������
    /// </summary>
    /// <typeparam name="TEntity">��� �������</typeparam>
    public interface IRepository<TEntity> : IDisposable
    {
        /// <summary>
        ///     �������� ������
        /// </summary>
        IQueryable<TEntity> Query { get; }

        /// <summary>
        ///     ����� ������ �� �� ���������� ��
        /// </summary>
        /// <param name="id">�� �������</param>
        /// <returns>������ �� ��</returns>
        TEntity Find(string id);

        IQueryable<TEntity> Find(IEnumerable<string> ids);

        /// <summary>
        ///     ��������� ������ � ��
        /// </summary>
        /// <param name="entity">������</param>
        void Store(TEntity entity);

        void StoreBulk(IEnumerable<TEntity> entities);

        /// <summary>
        ///     ������� ������ �� ��
        /// </summary>
        /// <param name="entity">������</param>
        void Delete(TEntity entity);

        /// <summary>
        ///     ��������� ��������� ������
        /// </summary>
        void SaveChanges();

        void DeleteAttach(string key);


        /// <summary>
        ///     ���������
        /// </summary>
        /// <param name="entity">��������</param>
        void Detach(TEntity entity);

        void DeleteById(string id);

        IEnumerable<TEntity> GetAll();
    }
}