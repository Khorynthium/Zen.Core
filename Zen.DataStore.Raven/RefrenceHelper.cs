using System.Linq;
using System.Text;
using log4net;

namespace Zen.DataStore
{
    public static class RefrenceHelpers
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (RefrenceHelpers));
        /// <summary>
        /// ���������� �� ������ �������� ��������� ��� ��� ������
        /// </summary>
        /// <typeparam name="T">��� ��������</typeparam>
        /// <param name="obj">��������</param>
        /// <param name="skip">�� ��������� ���� TRUE</param>
        /// <returns>��������</returns>
        public static T SkipRefrences<T>(this T obj,bool skip) where T : IHasStringId
        {
            var sb = new StringBuilder();
            sb.AppendLine("��������� �������� ������ � " + skip);
            //TODO: ���������� �� ������������ ���������
            var type = typeof (T);
            foreach (var prop in type.GetProperties().Where(p=>p.PropertyType==typeof(Refrence<>)))
            {
                //���������� ��� ������
                var pType = prop.PropertyType;

                //������ �������� ������ ����������� ����
                var pVal = prop.GetValue(obj);

                //�������� ����������� �������� �������� ������
                var loadProp = pType.GetProperty("SkipLoad");
                sb.AppendLine("����������� ��� �������� " + prop.Name);
                loadProp.SetValue(pVal, skip);
            }
            Log.Debug(sb);
            return obj;
        }

        /// <summary>
        /// ���������� �������� ������
        /// </summary>
        /// <typeparam name="T">��� ��������</typeparam>
        /// <param name="obj">��������</param>
        /// <returns>��������</returns>
        public static T SkipRefrences<T>(this T obj) where T : IHasStringId
        {
            return SkipRefrences(obj, true);
        }

        /// <summary>
        /// ��������� ������
        /// </summary>
        /// <typeparam name="T">��� ��������</typeparam>
        /// <param name="obj">��������</param>
        /// <returns>��������</returns>
        public static T LoadRefrences<T>(this T obj) where T : IHasStringId
        {
            return SkipRefrences(obj, false);
        }
    }
}