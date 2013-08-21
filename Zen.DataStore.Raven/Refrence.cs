using System;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Zen.DataStore
{

    /// <summary>
    /// ����������������� ������ �� ������
    /// </summary>
    /// <typeparam name="TRefObject">��� �������</typeparam>
    [Serializable]
    public class Refrence<TRefObject> : IRefrence where TRefObject : class, IHasStringId 
    {
        /// <summary>
        /// �� �� ������� ���������
        /// </summary>
        [DisplayName("������������� �������")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage="������� ������������� �������")]
        public string Id { get; set; }

        /// <summary>
        /// ������ �� ������� ���������
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
            set
            {
                if (value != null)
                {
                    Id = value.Id;
                    if (Repository != null && Repository.Find(Id) == null)
                    {
                        Repository.Store(value);
                        Repository.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// ����������� ��������
        /// </summary>
        [JsonIgnore]
        //[Raven.Imports.Newtonsoft.Json.JsonIgnore]
        public virtual IRepository<TRefObject> Repository { get; set; }
    }
}