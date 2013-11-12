using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Raven.Client;
using JSIgnore = Raven.Imports.Newtonsoft.Json.JsonIgnoreAttribute;

namespace Zen.DataStore
{
    /// <summary>
    ///     ����������������� ������ �� ������
    /// </summary>
    /// <typeparam name="TRefObject">��� �������</typeparam>
    [Serializable]
    public class Refrence<TRefObject> : IRefrence where TRefObject : class, IHasStringId
    {
        private readonly Func<IDocumentSession> _sessionFactory;

        public Refrence()
        {
            _sessionFactory = AppCore.Instance.Resolve<Func<IDocumentSession>>();
            RepositoryFactory = AppCore.Instance.Resolve<Func<IDocumentSession, IRepository<TRefObject>>>();
        }

        public Refrence(Func<IDocumentSession, IRepository<TRefObject>> repository,
            Func<IDocumentSession> sessionFactory)
        {
            _sessionFactory = sessionFactory;
            RepositoryFactory = repository;
        }


        private TRefObject refObject = default(TRefObject);

        /// <summary>
        ///     ������ �� ������� ���������
        /// </summary>
        [JsonIgnore]
        [JSIgnore]
        public virtual TRefObject Object
        {
            get
            {
                if (RefrenceHacks.SkipRefrences) return default(TRefObject);
                using (var sess=GetRefrenceSession())
                {
                    if (refObject == default(TRefObject) && sess.Repository != null)
                    {
                        refObject = sess.Repository.Find(Id);
                    }

                    return refObject;
                }
            }
            set {
                Id = value != null ? value.Id : null;
            }
        }

        /// <summary>
        ///     ����������� ��������
        /// </summary>
        [JsonIgnore]
        [JSIgnore]
        protected Func<IDocumentSession, IRepository<TRefObject>> RepositoryFactory { get; set; }


        public RefrenceSession<TRefObject> GetRefrenceSession()
        {
            //����������� �������� ������ ��� ������� ��� ���������� ���������
            IDocumentSession session;
            using (var rootSession = _sessionFactory())
            {
                session = rootSession.Advanced.DocumentStore.OpenSession();
                return new RefrenceSession<TRefObject>(RepositoryFactory(session), session);
            }
        }

        /// <summary>
        ///     �� �� ������� ���������
        /// </summary>
        [DisplayName("������������� �������")]
        [Required(ErrorMessage = "������� ������������� �������")]
        public string Id { get; set; }
    }
}