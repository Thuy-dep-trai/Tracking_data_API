using System.Data;

namespace Tracking_data.Hepler
{
    public static  class Extension
    {
        public static List<Dictionary<string, object>> ToDictionaryList(this DataTable table)
        {
            var list = new List<Dictionary<string, object>>();
            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }
            return list;
        }
    }
}
