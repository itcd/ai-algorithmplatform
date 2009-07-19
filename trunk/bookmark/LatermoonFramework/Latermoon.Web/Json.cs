using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Latermoon.Web
{
    /// <summary>
    /// 提供一组方法将对象换成JSON文本
    /// JSON 即 JavaScript Object Natation，它是一种轻量级的数据交换格式，非常适合于服务器与 JavaScript 的交互。
    /// </summary>
    public static class JSON
    {
        public static string ArrayToJson(object[] array)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (object elem in array)
            {
                sb.Append(ToJson(elem));
                sb.Append(",");
            }
            if (array.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1); // Remove last ","
            }
            sb.Append("]");
            return sb.ToString();
        }

        public static string HashtableToJson(Hashtable hashtable)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('{');
            foreach (DictionaryEntry item in hashtable)
            {
                sb.Append('\"');
                sb.Append(item.Key);
                sb.Append("\":");
                sb.Append(ToJson(item.Value));
                sb.Append(",");
            }
            if (sb.Length > 1)
            {
                sb.Remove(sb.Length - 1, 1); //Remove last ","
            }
            sb.Append('}');
            return sb.ToString();
        }

        /// <summary>
        /// 将对象转换成JSON格式文本
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJson(object value)
        {
            if (value == null)
            {
                return "null";
            }

            if (value is Boolean)
            {
                return BooleanToJson((bool)value);
            }
            if (value is string)
            {
                return StringToJson((string)value);
            }
            if (value is DateTime)
            {
                return DateTimeToJson((DateTime)value);
            }
            if (value.GetType().IsPrimitive)
            {
                return NumberToJson((ValueType)value);
            }
            if (value is Array)
            {
                return ArrayToJson((object[])value);
            }
            if (value is Hashtable)
            {
                return HashtableToJson((Hashtable)value);
            }
            //如果是其它类对象，则调用ToString作为字符串返回
            return StringToJson(value.ToString());
        }

        public static string StringToJson(string value)
        {
            if (value == null)
            {
                return ToJson(null);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append('\"');
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '/':
                        sb.Append("\\/");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            sb.Append('\"');
            return sb.ToString();
        }

        public static string NumberToJson(ValueType value)
        {
            return value.ToString();
        }

        public static string BooleanToJson(bool value)
        {
            return value ? "true" : "false";
        }

        /// <summary>
        /// 转换日期到GMT格式文本，如：Mon, 01 Jan 2001 00:00:00 GMT
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string DateTimeToJson(DateTime value)
        {
            return "\"" + value.ToString("r") + "\"";
        }
    }
}

    /// http://www.ietf.org/rfc/rfc4627.txt
    /// http://www.ibm.com/developerworks/cn/web/wa-lo-json/?S_TACT=105AGX52&S_CMP=tec-csdn#main