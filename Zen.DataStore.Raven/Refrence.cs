using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Zen.DataStore
{
    /// <summary>
    ///     ����������������� ������ �� ������
    /// </summary>
    /// <typeparam name="TRefObject">��� �������</typeparam>
    [Serializable]
    public class Refrence<TRefObject> : IRefrence where TRefObject : class, IHasStringId
    {
        public Refrence()
        {
        }

        public Refrence(IRepository<TRefObject> repository)
        {
            Repository = repository;
        }

        /// <summary>
        ///     ������ �� ������� ���������
        /// </summary>
        [JsonIgnore]
        //[Raven.Imports.Newtonsoft.Json.JsonIgnore]
        public virtual TRefObject Object
        {
            get
            {
                return Repository != null && !RefrenceHacks.SkipRefrences
                           ? Repository.Find(Id)
                           : default(TRefObject);
            }
            set {
                Id = value != null ? value.Id : null;
            }
        }

        /// <summary>
        ///     ����������� ��������
        /// </summary>
        [JsonIgnore]
        //[Raven.Imports.Newtonsoft.Json.JsonIgnore]
        public virtual IRepository<TRefObject> Repository { get; set; }

        /// <summary>
        ///     �� �� ������� ���������
        /// </summary>
        [DisplayName("������������� �������")]
        [Required(ErrorMessage = "������� ������������� �������")]
        public string Id { get; set; }
    }
}