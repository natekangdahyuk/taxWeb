using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Utilities;
using System.Data;

namespace Common.Lib
{
    public class cJSON
    {
        #region Serialize
        static public string _Serialize<T>(T _obj)
        {
            string Temp = null;

            try
            {
                Temp = JsonConvert.SerializeObject(_obj);
            }
            catch (Exception e)
            {
            }

            return Temp;
        }
        #endregion

        #region DeSerialize
        static public T _DeSerialize<T>(string _string)
        {
            T Temp = default(T);

            try
            {
                Temp = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(_string);
            }
            catch (Exception e)
            {

            }

            return Temp;
        }
        #endregion

        #region DataSet To Json
        public static string GetJSONString(DataTable Dt)
        {

            string[] StrDc = new string[Dt.Columns.Count];
            string HeadStr = string.Empty;
            for (int i = 0; i < Dt.Columns.Count; i++)
            {
                StrDc[i] = Dt.Columns[i].Caption;
                HeadStr += "\"" + StrDc[i] + "\" : \"" + StrDc[i] + i.ToString() + "¾" + "\",";
            }

            HeadStr = HeadStr.Substring(0, HeadStr.Length - 1);
            StringBuilder Sb = new StringBuilder();
            Sb.Append("{\"" + Dt.TableName + "\" : [");
            for (int i = 0; i < Dt.Rows.Count; i++)
            {


                string TempStr = HeadStr;
                Sb.Append("{");
                for (int j = 0; j < Dt.Columns.Count; j++)
                {
                    TempStr = TempStr.Replace(Dt.Columns[j] + j.ToString() + "¾", Dt.Rows[i][j].ToString());
                }
                Sb.Append(TempStr + "},");

            }
            Sb = new StringBuilder(Sb.ToString().Substring(0, Sb.ToString().Length - 1));
            Sb.Append("]};");
            return Sb.ToString();
        }
        #endregion               
        
        public static string FromCODAPI(DataTable dt, string Status_Code, string Status_Msg)
        {
            string rowDelimiter = "";


            //StringBuilder result = new StringBuilder("{\"result\":0,\" + dt.TableName + "\":[");
            StringBuilder result = new StringBuilder("{\"Status_Code\":\"" + Status_Code + "\",\"Status_Msg\":\"" + Status_Msg + "\",\"" + dt.TableName + "\":[");

            foreach (DataRow row in dt.Rows)
            {
                result.Append(rowDelimiter);
                result.Append(FromDataRow(row));
                rowDelimiter = ",";
            }
            result.Append("]}");

            return result.ToString();
        }

        public static string FromDataTable(DataTable dt, int dtResult)
        {
            string rowDelimiter = "";


            //StringBuilder result = new StringBuilder("{\"result\":0,\" + dt.TableName + "\":[");
            StringBuilder result = new StringBuilder("{\"result\":" + dtResult + ",\"" + dt.TableName + "\":[");

            foreach (DataRow row in dt.Rows)
            {
                result.Append(rowDelimiter);
                result.Append(FromDataRow(row));
                rowDelimiter = ",";
            }
            result.Append("]}");

            return result.ToString();
        }


        public static string FromDataRow(DataRow row)
        {
            DataColumnCollection cols = row.Table.Columns;
            string colDelimiter = "";

            StringBuilder result = new StringBuilder("{");
            for (int i = 0; i < cols.Count; i++)
            { // use index rather than foreach, so we can use the index for both the row and cols collection
                result.Append(colDelimiter).Append("\"")
                      .Append(cols[i].ColumnName).Append("\":")
                      .Append(JSONValueFromDataRowObject(row[i], cols[i].DataType));

                colDelimiter = ",";
            }
            result.Append("}");
            return result.ToString();
        }

        private static Type[] numeric = new Type[] {typeof(byte), typeof(decimal), typeof(double),
                                     typeof(Int16), typeof(Int32), typeof(SByte), typeof(Single),
                                     typeof(UInt16), typeof(UInt32), typeof(UInt64)};

        // I don't want to rebuild this value for every date cell in the table
        private static long EpochTicks = new DateTime(1970, 1, 1).Ticks;

        private static string JSONValueFromDataRowObject(object value, Type DataType)
        {
            // null
            if (value == DBNull.Value) return "null";

            // numeric
            if (Array.IndexOf(numeric, DataType) > -1)
                return value.ToString(); // TODO: eventually want to use a stricter format. Specifically: separate integral types from floating types and use the "R" (round-trip) format specifier

            // boolean
            if (DataType == typeof(bool))
                return ((bool)value) ? "true" : "false";

            // date -- see http://weblogs.asp.net/bleroy/archive/2008/01/18/dates-and-json.aspx
            if (DataType == typeof(DateTime))
                return "\"\\/Date(" + new TimeSpan(((DateTime)value).ToUniversalTime().Ticks - EpochTicks).TotalMilliseconds.ToString() + ")\\/\"";

            // TODO: add Timespan support
            // TODO: add Byte[] support

            //TODO: this would be _much_ faster with a state machine
            //TODO: way to select between double or single quote literal encoding
            //TODO: account for database strings that may have single \r or \n line breaks
            // string/char  
            return "\"" + value.ToString().Replace(@"\", @"\\").Replace(Environment.NewLine, @"\n").Replace("\"", @"\""") + "\"";
        }

    }
}
