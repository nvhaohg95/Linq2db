using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Linq2dbContext.DAL;
using Newtonsoft.Json;

namespace Linq2dbContext.Common
{
    public static class StringHelper
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>Returns a "random" alpha-numeric string of specified length and characters.</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or
        ///                                     illegal values.</exception>
        /// <param name="length">  the length of the random string.</param>
        /// <param name="pickfrom">the string of characters to pick randomly from.</param>
        /// <returns>The generate random string.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static string GenerateRandomString(int length, string pickfrom)
        {
            if (string.IsNullOrEmpty(pickfrom))
            {
                throw new ArgumentException("pickfrom is null or empty.", "pickfrom");
            }

            var r = new Random();
            string result = string.Empty;
            int picklen = pickfrom.Length - 1;

            for (int i = 0; i < length; i++)
            {
                int index = r.Next(picklen);
                result = result + pickfrom.Substring(index, 1);
            }

            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Remove all char by regex.</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <param name="text"> The text.</param>
        /// <param name="regex">The regex.</param>
        /// <returns>System.String.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static string RemoveCharByRegEx(string text, string regex)
        {
            Regex matchNonAlphaAndDigit = new Regex(regex);
            return matchNonAlphaAndDigit.Replace(text, "");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Truncates a string with the specified limits and adds (...) to the end if truncated.</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <param name="input">input string.</param>
        /// <param name="limit">max size of string.</param>
        /// <returns>truncated string.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static string Truncate(string input, int limit)
        {
            string output = input;

            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            // Check if the string is longer than the allowed amount
            // otherwise do nothing
            if (output.Length > limit && limit > 0)
            {
                // cut the string down to the maximum number of characters
                output = output.Substring(0, limit);

                // Check if the space right after the truncate point 
                // was a space. if not, we are in the middle of a word and 
                // need to cut out the rest of it
                if (input.Substring(output.Length, 1) != " ")
                {
                    int lastSpace = output.LastIndexOf(" ");

                    // if we found a space then, cut back to that space
                    if (lastSpace != -1)
                    {
                        output = output.Substring(0, lastSpace);
                    }
                }

                // Finally, add the "..."
                output += "...";
            }

            return output;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Truncates a string with the specified limits by adding (...) to the middle.</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <param name="input">input string.</param>
        /// <param name="limit">max size of string.</param>
        /// <returns>truncated string.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static string TruncateMiddle(string input, int limit)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            string output = input;
            const string middle = "...";

            // Check if the string is longer than the allowed amount
            // otherwise do nothing
            if (output.Length > limit && limit > 0)
            {
                // figure out how much to make it fit...
                int left = limit / 2 - middle.Length / 2;
                int right = limit - left - middle.Length / 2;

                if (left + right + middle.Length < limit)
                {
                    right++;
                }
                else if (left + right + middle.Length > limit)
                {
                    right--;
                }

                // cut the left side
                output = input.Substring(0, left);

                // add the middle
                output += middle;

                // add the right side...
                output += input.Substring(input.Length - right, right);
            }

            return output;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>When the string is trimmed, is it <see langword="null" /> or empty?</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <param name="str">The string.</param>
        /// <returns>The is <see langword="null" /> or empty trimmed.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static bool IsNullOrEmptyTrimmed(this string str)
        {
            //return str == null || String.IsNullOrEmpty(str.Trim());
            return string.IsNullOrWhiteSpace(str);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>The process text.</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <param name="text">The text.</param>
        /// <returns>The process text.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static string ProcessText(string text)
        {
            return ProcessText(text, true);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>The process text.</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <param name="text">   The text.</param>
        /// <param name="nullify">The nullify.</param>
        /// <returns>The process text.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static string ProcessText(string text, bool nullify)
        {
            return ProcessText(text, nullify, true);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>The process text.</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <param name="text">   The text.</param>
        /// <param name="nullify">The nullify.</param>
        /// <param name="trim">   The trim.</param>
        /// <returns>The process text.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static string ProcessText(string text, bool nullify, bool trim)
        {
            if (trim && !string.IsNullOrEmpty(text))
            {
                text = text.Trim();
            }

            if (nullify && text.IsNullOrEmptyTrimmed())
            {
                text = null;
            }

            return text;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Converts a string to a list using delimiter.</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <param name="str">      starting string.</param>
        /// <param name="delimiter">value that delineates the string.</param>
        /// <returns>list of strings.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static List<string> StringToList(this string str, char delimiter)
        {
            return str.StringToList(delimiter, new List<string>());
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Converts a string to a list using delimiter.</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
        ///                                         illegal values.</exception>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="str">      starting string.</param>
        /// <param name="delimiter">value that delineates the string.</param>
        /// <param name="exclude">  items to exclude from list.</param>
        /// <returns>list of strings.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static List<string> StringToList(this string str, char delimiter, List<string> exclude)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("str is null or empty.", "str");
            }

            if (exclude == null)
            {
                throw new ArgumentNullException("exclude", "exclude is null.");
            }

            var list = str.Split(delimiter).ToList();

            list.RemoveAll(exclude.Contains);
            list.Remove(delimiter + "");

            return list;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Creates a string from a string list.</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="strList">  The string list.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <returns>System.String.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static string ListToString(this List<string> strList, string delimiter)
        {
            if (strList == null)
            {
                throw new ArgumentNullException("strList", "strList is null.");
            }

            StringBuilder sb = new StringBuilder();

            strList.ForEach(
              x =>
              {
                  if (sb.Length > 0)
                  {
                      // append delimiter if this isn't the first string
                      sb.Append(delimiter);
                  }

                  // append string...
                  sb.Append(x);
              });

            return sb + "";
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Removes empty strings from the list.</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="inputList">The input list.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static List<string> RemoveEmptyStrings(this List<string> inputList)
        {
            if (inputList == null)
            {
                throw new ArgumentNullException("inputList", "inputList is null.");
            }

            return inputList.Where(x => !x.IsNullOrEmptyTrimmed()).ToList();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Removes strings that are smaller then <paramref name="minSize" /></summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        /// <param name="inputList">The input list.</param>
        /// <param name="minSize">  The minimum size.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static List<string> RemoveSmallStrings(this List<string> inputList, int minSize)
        {
            if (inputList == null)
            {
                throw new ArgumentNullException("inputList", "inputList is null.");
            }

            return inputList.Where(x => x.Length >= minSize).ToList();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Removes multiple whitespace characters from a string.</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <param name="text">The text.</param>
        /// <returns>The remove multiple whitespace.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static string RemoveMultipleWhitespace(string text)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(text))
            {
                return result;
            }

            var r = new Regex(@"\s+");
            return r.Replace(text, @" ");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Removes multiple single quote ' characters from a string.</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <param name="text">The text.</param>
        /// <returns>The remove multiple single quotes.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static string RemoveMultipleSingleQuotes(string text)
        {
            string result = string.Empty;
            if (string.IsNullOrEmpty(text))
            {
                return result;
            }

            var r = new Regex(@"\'");
            return r.Replace(text, @"'");
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Converts a String to a MemoryStream.</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <param name="str">The string.</param>
        /// <returns>MemoryStream.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static MemoryStream StringToStream(string str)
        {
            byte[] byteArray = Encoding.ASCII.GetBytes(str);
            return new MemoryStream(byteArray);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Converts a Stream to a String.</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <param name="theStream">The stream.</param>
        /// <returns>The stream to string.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static string StreamToString(Stream theStream)
        {
            var reader = new StreamReader(theStream);
            return reader.ReadToEnd();
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Splits the trim.</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <param name="sInput">   The s input.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>System.String[].</returns>
        ///-------------------------------------------------------------------------------------------------

        public static string[] SplitTrim(this string sInput, params char[] separator)
        {
            if (sInput.IsNullOrEmptyTrimmed())
                return new string[] { };
            sInput = RemoveCharByRegEx(sInput, " ");
            return sInput.Split(separator);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Gets the name of the control type.</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <param name="inputType">Type of the input.</param>
        /// <returns>System.String.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static string GetControlTypeName(int inputType)
        {
            string name = string.Empty;
            if (inputType == 0)
                name = "TextBox";
            else if (inputType == 1)
                name = "TextBox Multiline";
            else if (inputType == 2)
                name = "RadioButton";
            else if (inputType == 3)
                name = "CheckBox";

            return name;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Trims the null.</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <param name="str">The string.</param>
        /// <returns>System.String.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static string TrimNull(this string str)
        {
            string result = string.Empty;
            if (str.IsNullOrEmptyTrimmed() == false)
                result = str.Trim();
            return result;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>Converts to stringorempty.</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <param name="obj">The object.</param>
        /// <returns>System.String.</returns>
        ///-------------------------------------------------------------------------------------------------

        public static string ToStringOrEmpty(this object obj)
        {
            string res = string.Empty;
            if (obj != null)
                res = obj + "";
            return res;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Unicode string to ASCII string.</summary>
        /// <remarks>Tqhoan, 4/28/2022.</remarks>
        /// <param name="text">The Unicode string.</param>
        /// <returns>ASCII string.</returns>
        /// -------------------------------------------------------------------------------------------------

        public static string NonUnicode(this string text)
        {
            return text.Unidecode();
        }

        /// <summary>
        /// Replace with format: [FieldName], @for-<Datas>{Text [FieldName] Text [FieldName1]}@
        /// </summary>
        /// <param name="text"></param>
        /// <param name="data">Thie data is Dictionary<string, object> or entity model</param>
        /// <example>
        /// <code language="cs">
        /// var dt = new Dictionary<string, object>();
        /// dt.Add("Status", "Tinh Trang");
        /// dt.Add("Name", "Ten A");
        /// dt.Add("Datas", new[] { new { FieldName = "F1", VOld = "", VNew = "123" }, new { FieldName = "F2", VOld = "111", VNew = "222" } });
        /// dt.Add("Datas1", new[] { new { FieldName1 = "F11", VOld1 = "", VNew1 = "11" }, new { FieldName1 = "F12", VOld1 = "11", VNew1 = "22" }, new { FieldName1 = "F13", VOld1 = "13", VNew1 = "" } });
        /// string str = "<h2>Status [Status]</h2><table><tr><th>Company</th><th>Contact</th><th>Country</th></tr>"
        ///    + "@for-Datas{<tr><td>[FieldName]</td><td>[VOld]</td><td>[VNew]</td></tr>}@"
        ///    + "</table>"
        ///    + "<table><tr><th>Company</th><th>Contact</th><th>Country</th></tr>"
        ///    + "@for-Datas1{<tr><td>[FieldName1]</td><td>[VOld1]</td><td>[VNew1]</td></tr>}@"
        ///    + "</table><p>A basic HTML table [Name]</p>";
        /// var result1 = str.Replace(dt);
        /// </code>
        /// </example>
        /// <returns>Text replaced</returns>
        public static string Replace(this string text, object data)
        {
            if (data == null) return text;

            Dictionary<string, object> entityData = null;
            if (!(data is Dictionary<string, object>))
            {
                var js = JsonConvert.SerializeObject(data);
                entityData = JsonConvert.DeserializeObject<Dictionary<string, object>>(js);
            }
            else
            {
                entityData = (Dictionary<string, object>)data;
            }

            string Rpl(string txt, Dictionary<string, object> data)
            {
                var ms = Regex.Matches(txt, @"\[(?<key>.*?)\]");
                Parallel.ForEach(ms, m =>
                {
                    var k = m.Groups["key"].Value;
                    if (k == "EmailSignature")
                    {
                        //var companySetting = CacheServices.GetCompanySetting().Result;
                        //txt = companySetting != null && companySetting.Count > 0 ? companySetting[0].Signature : "";
                    }
                    else
                    {
                        if (data.ContainsKey(k))
                        {
                            var f = data[k];
                            txt = txt.Replace(m.Value, f + "");
                        }
                    }

                });

                return txt;
            }

            var matches = Regex.Matches(text, @"@for-(?<key>.*?){(?<value>.*?)}@");
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    var sKey = m.Groups["key"].Value;
                    var sValue = m.Groups["value"].Value;
                    var sResult = "";
                    if (!string.IsNullOrEmpty(sKey) && !string.IsNullOrEmpty(sValue) && entityData.ContainsKey(sKey) && entityData[sKey] != null)
                    {
                        var d = entityData[sKey];
                        List<Dictionary<string, object>> jsDatas = null;
                        if (!(d is List<Dictionary<string, object>>))
                        {
                            jsDatas = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(JsonConvert.SerializeObject(d));
                        }
                        else
                        {
                            jsDatas = (List<Dictionary<string, object>>)d;
                        }

                        foreach (var dts in jsDatas)
                        {
                            var sR = sValue;
                            sR = Rpl(sR, dts);

                            sResult += sR;
                        }
                    }

                    text = text.Replace(m.Groups[0].ToString(), sResult);
                }
            }

            return Rpl(text, entityData);
        }

        /// <summary>
        /// Replace with format: [FieldName], @for-<Datas>{Text [FieldName] Text [FieldName1]}@
        /// </summary>
        /// <param name="text"></param>
        /// <param name="data">Thie data is Dictionary<string, object> or entity model</param>
        /// <example>
        /// <code language="cs">
        /// var dt = new Dictionary<string, object>();
        /// dt.Add("Status", "Tinh Trang");
        /// dt.Add("Name", "Ten A");
        /// dt.Add("Datas", new[] { new { FieldName = "F1", VOld = "", VNew = "123" }, new { FieldName = "F2", VOld = "111", VNew = "222" } });
        /// dt.Add("Datas1", new[] { new { FieldName = "F11", VOld = "", VNew = "11" }, new { FieldName = "F12", VOld = "11", VNew = "22" }, new { FieldName = "F13", VOld = "13", VNew = "" } });
        /// string str = "<div><h2>Status<span codx-data='[Status]'>[Tình Trạng]</span></h2><table><tr><th>Company</th><th>Contact</th><th>Country</th></tr>"
        ///    + "<tr codx-data='for-Datas'><td><span codx-data='[FieldName]'>[Tên Trường]</span></td><td><span codx-data='[VOld]'>[Gia trị cũ]</span></td><td><span codx-data='[VNew]'>[Gia trị mới]</span></td></tr>
        ///    + "</table>"
        ///    + "<table><tr><th>Company</th><th>Contact</th><th>Country</th></tr>"
        ///    + "<tr codx-data='for-Datas1'><td><span codx-data='[FieldName]'>[Tên Trường]</span></td><td><span codx-data='[VOld]'>[Gia trị cũ]</span></td><td><span codx-data='[VNew]'>[Gia trị mới]</span></td></tr>"
        ///    + "</table><p>A basic HTML table<span codx-data='[Name]'>[Tên]</span></p></div>";
        /// var result1 = str.ReplaceXML(dt);
        /// </code>
        /// </example>
        /// <returns>Text replaced</returns>
        //public static string ReplaceXML(this string text, object data)
        //{
        //    if (data == null) return text;

        //    Dictionary<string, object> entityData = null;
        //    if (!(data is Dictionary<string, object>))
        //    {
        //        var js = LVJsonHelper.Serializer(data);
        //        entityData = LVJsonHelper.Deserializer<Dictionary<string, object>>(js);
        //    }
        //    else
        //    {
        //        entityData = (Dictionary<string, object>)data;
        //    }

        //    void Rpl(IEnumerable<XAttribute> dtAttr, Dictionary<string, object> data, string sKeyData = "", int index = -1)
        //    {
        //        foreach (var attr in dtAttr)
        //        {

        //            var pr = attr.Value.ToString();
        //            var parent = attr.Parent;
        //            if (pr.Contains("|vll:") || pr.Contains("|cbx:"))
        //            {
        //                if (!string.IsNullOrEmpty(sKeyData) && index > -1)
        //                {
        //                    parent.SetAttributeValue("codx-index", index);
        //                    parent.SetAttributeValue("codx-for-data", sKeyData);
        //                }
        //            }
        //            else
        //            {
        //                if (pr.Contains('[') && pr.Contains(']'))
        //                {
        //                    //var fld = sNItem.Replace('[').Replace(']');
        //                    var startChar = pr.IndexOf("[");
        //                    var closeChar = pr.IndexOf("]");
        //                    var k = pr.Substring(startChar + 1, closeChar - startChar - 1);
        //                    if (!string.IsNullOrEmpty(k))
        //                    {
        //                        if (data.ContainsKey(k))
        //                        {
        //                            var f = data[k];

        //                            parent.Value = f + "";
        //                            if (!string.IsNullOrEmpty(sKeyData) && index > -1)
        //                            {
        //                                parent.SetAttributeValue("codx-index", index);
        //                                parent.SetAttributeValue("codx-for-data", sKeyData);
        //                            }

        //                            //parent.Attribute("codx-data").Remove();
        //                            //text = text.Replace(pr, f + "");
        //                        }
        //                    }
        //                }
        //            }

        //        }
        //        //return "";
        //    }

        //    var document = XDocument.Parse(text);

        //    var attrFor = document.Descendants().Attributes().Where(e => e.Name == "codx-data" && e.Value.StartsWith("for-"));

        //    if (attrFor.Count() > 0)
        //    {
        //        foreach (var item in attrFor)
        //        {
        //            var parent = item.Parent;
        //            var attrValue = item.Value.ToString().Split('-');
        //            if (attrValue != null && attrValue.Length > 0 && entityData.ContainsKey(attrValue[1].ToString()))
        //            {
        //                var dtGroup = entityData[attrValue[1].ToString()];
        //                List<Dictionary<string, object>> jsDatas = null;
        //                if (!(dtGroup is List<Dictionary<string, object>>))
        //                {
        //                    jsDatas = LVJsonHelper.Deserializer<List<Dictionary<string, object>>>(LVJsonHelper.Serializer(dtGroup));
        //                }
        //                else
        //                {
        //                    jsDatas = (List<Dictionary<string, object>>)dtGroup;
        //                }

        //                int i = 0;
        //                foreach (var dts in jsDatas)
        //                {                          
        //                    var ele = parent;
        //                    if(i > 0)
        //                    {
        //                        ele = new XElement(parent);
        //                        item.Parent.Parent.Add(ele);
        //                        ele.Attribute("codx-data").Remove();
        //                    }                           
        //                    var dtAttr = ele.Descendants().Attributes("codx-data");
        //                    Rpl(dtAttr, dts, attrValue[1].ToString(), i);                          
        //                    i++;
        //                    //sResult += sR;
        //                }
        //                //parent.Remove();

        //            }

        //        }
        //    }
        //    var attrUnFor = document.Descendants().Attributes().Where(e => e.Name == "codx-data" && !e.Value.StartsWith("for-"));           
        //    //var dt = document.Descendants().Attributes("data");
        //    Rpl(attrUnFor, entityData);

        //    return document.ToString();
        //}
    }
}
