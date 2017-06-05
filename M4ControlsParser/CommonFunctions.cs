using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M4ControlsParser
{
    public class CommonFunctions
    {
        static Dictionary<string, string> mCleanValues = new Dictionary<string, string>()
        {
            { "\t", " " },
            { "\r", " " },
            { "\n", " " },
            { "pDoc->", "" },
            { "pServerDoc->", "" },
            { "pClientDoc->", "" },
            { "GetDocument()->", "" },
            { "GetServerDoc()->", "" },
            { "GetClientDoc()->", "" }
        };

        static Dictionary<string, string> mHKLCleanValues = new Dictionary<string, string>()
        {
            { "NO_BUTTON", "" },
            { "BTN_DEFAULT", "" },
            { ",", "" },
            { ")", "" }
        };

        public Tuple<string, string> FindFile(string aFolder, string aPattern, string aTextToSearch1, string aTextToSearch2 = "")
        {
            string aText = string.Empty;

            if (Directory.Exists(aFolder))
            {
                foreach (string f in Directory.GetFiles(aFolder, aPattern))
                {
                    aText = File.ReadAllText(f, Encoding.Default);
                    if (aText.Contains(aTextToSearch1) && (string.IsNullOrEmpty(aTextToSearch2) || aText.Contains(aTextToSearch2)))
                    {
                        // faccio cosi' perche' la pulizia dei commenti a priori e' troppo lenta
                        aText = Clean(aText);
                        if (aText.Contains(aTextToSearch1) && (string.IsNullOrEmpty(aTextToSearch2) || aText.Contains(aTextToSearch2)))
                            return new Tuple<string, string>(f, aText);
                    }
                }
            }
            return new Tuple<string, string>(string.Empty, string.Empty);
        }

        public List<string> FindFiles(string aFolder, string aPattern, bool bNoSpaces, string aTextToSearch1, string aTextToSearch2 = "")
        {
            List<string> lt = new List<string>();
            string aText = string.Empty;

            if (Directory.Exists(aFolder))
            {
                foreach (string f in Directory.GetFiles(aFolder, aPattern))
                {
                    aText = File.ReadAllText(f, Encoding.Default);
                    if (aText.Contains(aTextToSearch1) && (string.IsNullOrEmpty(aTextToSearch2) || aText.Contains(aTextToSearch2)))
                    {
                        // faccio cosi' perche' la pulizia dei commenti a priori e' troppo lenta
                        aText = Clean(aText);
                        if (bNoSpaces)
                            aText = aText.Replace(" ", "");
                        if (aText.Contains(aTextToSearch1) && (string.IsNullOrEmpty(aTextToSearch2) || aText.Contains(aTextToSearch2)))
                            lt.Add(aText);
                    }
                }
            }
            return lt;
        }

        public string Clean(string aText)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            int pStartComment = aText.IndexOf(@"//");
            int pEndComment = -1;

            while (pStartComment != -1)
            {
                pEndComment = aText.IndexOf("\n", pStartComment);
                if (pEndComment != -1)
                    aText = aText.Remove(pStartComment, pEndComment - pStartComment);
                else
                    aText = aText.Substring(0, pStartComment);
                pStartComment = aText.IndexOf(@"//");
            }

            pStartComment = aText.IndexOf(@"/*");
            pEndComment = -1;

            while (pStartComment != -1)
            {
                pEndComment = aText.IndexOf(@"*/", pStartComment);
                if (pEndComment != 1)
                    aText = aText.Remove(pStartComment, pEndComment - pStartComment + 2);
                pStartComment = aText.IndexOf(@"/*");
            }

            aText = StringBuilderReplace(new StringBuilder(aText, aText.Length * 2));
            stopwatch.Stop();
            System.Diagnostics.Debug.WriteLine("---------------------- TIME: " + stopwatch.ElapsedMilliseconds);

            return aText;
        }

        private static string StringBuilderReplace(StringBuilder data)
        {
            foreach (string k in mCleanValues.Keys)
            {
                data.Replace(k, mCleanValues[k]);
            }
            return data.ToString();
        }

        public string Trim(string aText, string aTag1 = "", string aTag2 = "", string aTag3 = "")
        {
            if (string.IsNullOrEmpty(aText))
                return aText;

            Dictionary<string, string> aTrimValues = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(aTag1))
                aTrimValues.Add(aTag1, "");
            if (!string.IsNullOrEmpty(aTag2))
                aTrimValues.Add(aTag2, "");
            if (!string.IsNullOrEmpty(aTag3))
                aTrimValues.Add(aTag3, "");
            return StringBuilderTrim(new StringBuilder(aText, aText.Length * 2), aTrimValues);
        }

        private static string StringBuilderTrim(StringBuilder data, Dictionary<string, string> aTrimValues)
        {
            foreach (string k in aTrimValues.Keys)
            {
                data.Replace(k, aTrimValues[k], 0, 1);
                data.Replace(k, aTrimValues[k], data.Length - 1, 1);
            }
            return data.ToString();
        }

        public bool IsValid(string aText, string aTag, int aTagPosition, string NotStarting = "", string NotEnding = "")
        {
            int l = aTag.Trim().Length;
            int t = aTagPosition + l;
            if (t >= aText.Length || t <= 0)
                return true;

            bool st = false;
            bool ed = false;
            string s = string.Empty;
            string e = string.Empty;

            if (aTagPosition == 0)
                st = true;
            else
            {
                s = aText.Substring(aTagPosition - 1, 1);
                st = string.IsNullOrEmpty(s) || s == "\n" || s == "\r" || s == "\t" || s == " " || s == "(" || s == ")" || s == "=" || s == ";" || s == ":" || s == "." || s == ">";
            }

            if (aTagPosition == t)
                ed = true;
            else
            {
                e = aText.Substring(t, 1);
                ed = string.IsNullOrEmpty(e) || e == "\n" || e == "\r" || e == "\t" || e == " " || e == "(" || e == ")" || e == "=" || e == ";" || e == ":" || e == "." || e == ">";
            }

            if (!string.IsNullOrEmpty(NotStarting) && NotStarting.Length < t)
            {
                s = aText.Substring(t - l - NotStarting.Length, NotStarting.Length);
                st = st && !s.StartsWith(NotStarting);
            }

            if (!string.IsNullOrEmpty(NotEnding) && NotEnding.Length < t)
            {
                e = aText.Substring(t - NotEnding.Length + 1, NotEnding.Length);
                ed = ed && !e.EndsWith(NotEnding);
            }

            return st && ed;
        }

        public int PreviousValid(string aText, string aTag, int aStart = 0, string NotStarting = "", string NotEnding = "")
        {
            aTag = aTag.Trim();
            int pTagPosition = aText.LastIndexOf(aTag, aStart);
            while (pTagPosition != -1 && !IsValid(aText, aTag, pTagPosition, NotStarting, NotEnding))
                pTagPosition = aText.LastIndexOf(aTag, pTagPosition - 1);
            return pTagPosition;
        }

        public int NextValid(string aText, string aTag, int aStart = 0, string NotStarting = "", string NotEnding = "")
        {
            aTag = aTag.Trim();
            int pTagPosition = aText.IndexOf(aTag, aStart);
            while (pTagPosition != -1 && !IsValid(aText, aTag, pTagPosition, NotStarting, NotEnding))
                pTagPosition = aText.IndexOf(aTag, pTagPosition + 1);
            return pTagPosition;
        }

        public Tuple<string, string> HKL(string aText)
        {
            string button = string.Empty;

            if (aText.Contains("NO_BUTTON"))
                button = "NO_BUTTON";
            else if (aText.Contains("BTN_DEFAULT"))
                button = "BTN_DEFAULT";
            return new Tuple<string, string>(StringBuilderHKL(new StringBuilder(aText, aText.Length * 2)), button);
        }

        private static string StringBuilderHKL(StringBuilder data)
        {
            foreach (string k in mHKLCleanValues.Keys)
            {
                data.Replace(k, mHKLCleanValues[k]);
                data.Replace(k, mHKLCleanValues[k]);
            }
            return data.ToString();
        }
        
        public static string SubstingBracket(string aText,int openBracket)
        {
            string text = aText.Substring(openBracket);
            openBracket = 0;
            int controlBracket= openBracket+1;
            int closeBracket= openBracket+1;
            do{
                closeBracket = text.IndexOf(')', closeBracket + 1);
                controlBracket = text.IndexOf('(', controlBracket + 1);
                if (controlBracket > closeBracket)
                    return text.Substring(openBracket, closeBracket - openBracket +1);
            } while (controlBracket < closeBracket);
            return "";
        }

        public static int IndexSubstingBracket(string aText, int openBracket)
        {
            int controlBracket = openBracket + 1;
            int closeBracket = openBracket + 1;
            do
            {
                closeBracket = aText.IndexOf('}', closeBracket + 1);
                controlBracket = aText.IndexOf('{', controlBracket + 1);
                if (controlBracket > closeBracket)
                    return closeBracket;
            } while (controlBracket < closeBracket);
            return -1;
        }
    }
}
