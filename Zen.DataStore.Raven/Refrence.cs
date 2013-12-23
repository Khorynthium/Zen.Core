using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Raven.Client;
using log4net;
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
        private Func<IDocumentSession> _sessionFactory;       
        public Refrence()
        {
            _sessionFactory = null;
            RepositoryFactory = null;
            SkipLoad = RefrenceHacks.SkipRefrencesByDefault;
        }

        public Refrence(Func<IDocumentSession, IRepository<TRefObject>> repository,
            Func<IDocumentSession> sessionFactory)
        {
            _sessionFactory = sessionFactory;
            RepositoryFactory = repository;
            SkipLoad = RefrenceHacks.SkipRefrencesByDefault;
        }


        private TRefObject _refObject;
        private string _id;

        /// <summary>
        /// �� �������� �������� ������
        /// </summary>
        public bool SkipLoad { get; set; }

        /// <summary>
        ///     ������ �� ������� ���������
        /// </summary>
        [JsonIgnore]
        [JSIgnore]
        public virtual TRefObject Object
        {
            get
            {
                if (SkipLoad || RefrenceHacks.SkipRefrences)
                    return default(TRefObject);
                IAppScope scope = null;
                try
                {
                    //��� �� ���������� ������������� ������� ���������� ������
                    if (_sessionFactory == null || RepositoryFactory == null)
                    {
                        scope = AppCore.Instance.BeginScope();
                        _sessionFactory = scope.Resolve<Func<IDocumentSession>>();
                        RepositoryFactory = scope.Resolve<Func<IDocumentSession, IRepository<TRefObject>>>();
                    }

                    using (var sess = GetRefrenceSession())
                    {
                        if (_refObject == default(TRefObject) && sess.Repository != null)
                        {
                            _refObject = sess.Repository.Find(Id);
                        }

                        return _refObject;
                    }
                }
                finally
                {
                    //���� ���� ������� ���������� ������
                    if (scope != null)
                    {
                        scope.Dispose();
                        _sessionFactory = null;
                        RepositoryFactory = null;
                    }
                }
            }
            set {
                Id = value != null ? value.Id : null;
                
                _refObject = default(TRefObject);
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
            using (var rootSession = _sessionFactory())
            {
                IDocumentSession session = rootSession.Advanced.DocumentStore.OpenSession();
                return new RefrenceSession<TRefObject>(RepositoryFactory(session), session);
            }
        }

        /// <summary>
        ///     �� �� ������� ���������
        /// </summary>
        [DisplayName("������������� �������")]
        [Required(ErrorMessage = "������� ������������� �������")]
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                _refObject = default(TRefObject);
            }
        }
    }
}