using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using M4ControlsDBMaker;

namespace M4ControlsParser
{
    /// <summary>
    /// Summary description for ObjectsReader.
    /// </summary>
    public class ObjectsReader
	{
        private string mERP = string.Empty;
        private DBManager mDB = null;

        private CommonFunctions cf = new CommonFunctions();

        private static string ADDLINK = "AddLink";
        private static string ADDCOLUMN = "AddColumn";
        private static string ONATTACHDATA = "OnAttachData";
        private static string VOID = "void";
        private static string RUNTIME_CLASS = "RUNTIME_CLASS";
        private static string CLASS = "class";
        private static string NEW = "new";
        private static string BUILD_DATA_CONTROL_LINKS = "BuildDataControlLinks";
        private static string CUSTOMIZE = "Customize";
        private static string ADDTILE = "AddTile";
        private static string ADDFILTERTILE = "AddFilterTile";
        private static string ADDTILEDIALOG = "AddTileDialog";
        private static string ADDACTIONTILE = "AddActionTile";
        private static string INCLUDE = "#include";
        private static string IMPLEMENT_DYNCREATE = "IMPLEMENT_DYNCREATE";

        public static event EventHandler<string> CurrentClass;
        public static event EventHandler<string> CurrentIDC;

        private List<_CLASS> mClasses = new List<_CLASS>();
        private List<_CONTROL> mControls = new List<_CONTROL>();
        private List<string> mLabel = new List<string>();
        
        private Hashtable mFunctions = new Hashtable();
        private Hashtable mHotKeyLinks = new Hashtable();
        private Hashtable mDBTs = new Hashtable();
        private Hashtable mDBTNamespaces = new Hashtable();
        private Hashtable mBodyEditIDCs = new Hashtable();


        public ObjectsReader(DBManager aDB = null, string aERP = "")
		{
            if (aDB != null && !string.IsNullOrEmpty(aERP))
            {
                mDB = aDB;
                mERP = aERP;
                Initialize(aDB, aERP);
            }
        }

        #region load (public methods)

        public void Initialize(DBManager aDB, string aERP)
        {
            mDB = aDB;
            mERP = aERP;

            mControls.Clear();
            mHotKeyLinks.Clear();
            mDBTs.Clear();
            mDBTNamespaces.Clear();
        }

        public bool LoadModule(string aModule)
        {
            if (string.IsNullOrEmpty(mERP) || string.IsNullOrEmpty(aModule))
                return false;

            string r = Path.Combine(mERP, aModule);

            if (!Directory.Exists(r))
                return false;

            int aCurrentFileNumber = 0;
            foreach (string d in Directory.GetDirectories(r))
            {
                foreach (string f in Directory.GetFiles(d, "*.cpp"))
                    aCurrentFileNumber++;
            }

 
            aCurrentFileNumber = 0;
            foreach (string d in Directory.GetDirectories(r))
            {
                foreach (string f in Directory.GetFiles(d, "*.cpp"))
                {
                    if (!LoadFile(aModule, f))
                        return false;
                }
            }
            System.Diagnostics.Debug.WriteLine("===============================================================");
            return true;
        }

        public bool LoadFile(string aModule, string aFile)
        {
            if (string.IsNullOrEmpty(mERP) || string.IsNullOrEmpty(aModule) || string.IsNullOrEmpty(aFile))
                return false;

            if (!File.Exists(aFile))
                return false;

            return LoadText(aModule, aFile, File.ReadAllText(aFile, Encoding.Default));
        }

        public bool LoadText(string aModule, string aFile, string aText)
        {
            if (string.IsNullOrEmpty(mERP) || string.IsNullOrEmpty(aModule) || string.IsNullOrEmpty(aFile) || string.IsNullOrEmpty(aText))
                return false;

            if (!File.Exists(aFile))
                return false;

            System.Diagnostics.Debug.WriteLine("\tFILE: " + aFile);
            aText = cf.Clean(aText);

            mClasses.Clear();
            mFunctions.Clear();
            mBodyEditIDCs.Clear();
            SearchClasses(aModule, aFile, aText, ADDLINK);
            SearchClasses(aModule, aFile, aText, ADDCOLUMN);
            SearchClassesAddLabel(aText);
            SearchAddLinks(aModule, aFile, aText);
            SearchAddColumns(aModule, aFile, aText);
            CheckBodyedit();
            return true;
        }

        public void UpdateDB()
        {
            foreach (_CONTROL cl in mControls)
            {
                mDB.ControlsInsert(cl);                
            }
            foreach (string IDC in mLabel)
            {
                mDB.LabelInsert(IDC);
            }            
        }

        #endregion

        #region classes
        private void SearchClasses(string aModule, string aFile, string aText, string aTag)
        {
            int pTag = aText.IndexOf(aTag);
            int pSeparator = -1;
            int pReturnType = -1;
            int pCurlyBracketOpened = -1;
            int pCurlyBracketClosed = -1;
            int pSemicolon = -1;
            int pStartTag = -1;

            while (pTag != -1)
            {
                pCurlyBracketClosed = -1;

                if (cf.IsValid(aText, aTag, pTag))
                {
                    pSeparator = -1;
                    pReturnType = -1;
                    pCurlyBracketOpened = aText.LastIndexOf("{", pTag);
                    pCurlyBracketClosed = -1;
                    pSemicolon = aText.LastIndexOf(";", pTag);

                    if (pCurlyBracketOpened > pSemicolon)
                        pStartTag = pCurlyBracketOpened;
                    else
                        pStartTag = pSemicolon;

                    if (!aText.Substring(pStartTag, pTag - pStartTag).Contains(".") && !aText.Substring(pStartTag, pTag - pStartTag).Contains(">"))
                    {
                        pReturnType = aText.LastIndexOf(VOID, pTag);
                        if (pReturnType != -1)
                        {
                            pSeparator = aText.IndexOf("::", pReturnType);
                            if (pSeparator != -1)
                            {
                                pStartTag = aText.IndexOf("{", pTag);
                                pCurlyBracketClosed = aText.IndexOf("}", pTag);
                                while (pStartTag > -1 && pStartTag < pCurlyBracketClosed)
                                {
                                    pStartTag = aText.IndexOf("{", pCurlyBracketClosed + 1);
                                    pCurlyBracketClosed = aText.IndexOf("}", pCurlyBracketClosed + 1);
                                }

                                if (pCurlyBracketClosed != -1)
                                {
                                    _CLASS cs = new _CLASS();

                                    cs._module = aModule;
                                    cs._filename = aFile;
                                    cs._start = pCurlyBracketOpened;
                                    cs._end = pCurlyBracketClosed;
                                    cs._isaddlink = (aTag == ADDLINK);
                                    cs._name = aText.Substring(pReturnType + 4, pSeparator - pReturnType - 4).Trim();
                                    //string classe = aText.Substring(pReturnType + 4, pSeparator - pReturnType - 4).Trim();
                                   // cs._name = findNameSpaceIDD(cs._name, aText);
                                    if (ADDLINK == aTag)
                                    {
                                        cs._containerclass = string.Empty;
                                        cs._idc = string.Empty;
                                        cs._idd = SearchClassIDD(aFile, aText, cs._name);
                                     
                                    }
                                    else 
                                    {
                                        var cc  = SearchBodyEditContainerClass(aFile, aText, cs._name);
                                        cs._containerclass = cc.Item1;
                                        cs._idc = cc.Item2;
                                        cs._idd = string.IsNullOrWhiteSpace(cc.Item3) ? SearchClassIDDInLine(aFile, aText.Replace(" ", ""), cs._containerclass) :cc.Item3;
                                        cs._text = cc.Item4;
                                        if (string.IsNullOrEmpty(cs._idd))
                                            cs._idd = SearchBodyEditContainerClassInHeader(aFile, cs._containerclass);
                                    }
                                    var tile = SearchTile(aFile, aText, cs._isaddlink ? cs._name : cs._containerclass);
                                    cs._tilestyle = tile.Item1;
                                    cs._tiletext = tile.Item2;
                                    cs._tilesize = tile.Item3;

                                    mClasses.Add(cs);
                                }
                            }
                        }
                    }
                }
                if (pCurlyBracketClosed > 0)
                    pTag = aText.IndexOf(aTag, pCurlyBracketClosed);
                else
                    pTag = aText.IndexOf(aTag, pTag + 1);
            }
        }
        #endregion


        #region addlink
        private void SearchAddLinks(string aModule, string aFile, string aText)
        {
            int pTag = -1;
            int pBracketOpened = -1;
            int pBracketClosed = -1;
            //int pQuotationMarksOpened = -1;
            //int pQuotationMarksClosed = -1;
            int pSemicolon = -1;
            int pComma1 = -1;
            int pComma2 = -1;
            int pSeparator = -1;
            int pRuntimeClass = -1;
            int pTemp = -1;
            string aTemp = string.Empty;

            foreach (_CLASS cs in mClasses)
            {
                if (!cs._isaddlink)
                    continue;

                if (CurrentClass != null)
                    CurrentClass(null, cs._name);

                System.Diagnostics.Debug.WriteLine("\t\tCLASS: " + cs._name);

                pTag = aText.IndexOf(ADDLINK, cs._start);

                while (pTag != -1 && pTag < cs._end)
                {
                    pBracketOpened = aText.IndexOf("(", pTag);
                    if (pBracketOpened != -1)
                    {
                        _CONTROL cl = new _CONTROL();

                        cl._generatejson = true;
                        cl._module = aModule;
                        cl._filename = aFile;
                        cl._classidd = cs._idd;
                        cl._class = cs._name;
                        cl._tilesize = cs._tilesize;
                        cl._tiletext = cs._tiletext;
                        cl._tilestyle = cs._tilestyle;
                        cl._bodyeditidc = string.Empty;
                        cl._bodyedittext = string.Empty;
                        cl._idc = string.Empty;
                        cl._namespace = string.Empty;
                        cl._fieldnamespace = string.Empty;
                        cl._text = string.Empty;
                        cl._combotype = string.Empty;
                        cl._recordpointer = string.Empty;
                        cl._dbtpointer = string.Empty;
                        cl._dbtnamespace = string.Empty;
                        cl._recordclass = string.Empty;
                        cl._field = string.Empty;
                        cl._runtimeclass = string.Empty;
                        cl._hkl = string.Empty;
                        cl._button = string.Empty;
                        cl._isaddlink = true;

                        pSemicolon = aText.IndexOf(";", pTag + 1);
                        pComma1 = aText.IndexOf(",", pTag + 1);
                        // Idc
                        cl._idc = aText.Substring(pBracketOpened + 1, pComma1 - pBracketOpened - 1).Trim();                       

                        if (CurrentIDC != null)
                            CurrentIDC(null, cl._idc);

                        // Namespace
                        pComma2 = aText.IndexOf(",", pComma1 + 1);
                        //pQuotationMarksOpened = aText.IndexOf("\"", pComma1 + 1);
                        //pQuotationMarksClosed = aText.IndexOf("\"", pQuotationMarksOpened + 1);
                        if (string.IsNullOrWhiteSpace(cl._namespace = FindIDD(cs._name, aText)))
                            if (string.IsNullOrWhiteSpace(cl._namespace = FindIDD(cs._name, aText, @"_NS_VIEW")))
                                cl._namespace = cs._name;

                        //if (pQuotationMarksOpened != -1 && pQuotationMarksClosed != -1 && pQuotationMarksClosed < pComma2)
                        //    cl._namespace = aText.Substring(pQuotationMarksOpened + 1, pQuotationMarksClosed - pQuotationMarksOpened - 1).Trim();
                        //else if (string.IsNullOrWhiteSpace(cl._namespace = FindIDD(cs._name, aText)))
                        //    if (string.IsNullOrWhiteSpace(cl._namespace = FindIDD(cs._name, aText, @"_NS_VIEW")))
                        //        cl._namespace=cs._name;

                            //cl._namespace = findNameSpaceIDD(, aText);
                            // Function & Field
                        if (!string.IsNullOrEmpty(cl._namespace))
                        {
                            pComma1 = pComma2;
                            pComma2 = aText.IndexOf(",", pComma1 + 1);
                        }
                        
                        if (pComma1 != -1 && pComma2 != -1 && pComma2 < pSemicolon)
                        {
                            int startAddLink = aText.IndexOf('(', aText.LastIndexOf("AddLink", pSemicolon));

                            string addlink = aText.Substring(startAddLink, pSemicolon - startAddLink);
                            if (!string.IsNullOrWhiteSpace(addlink))
                            {
                                string macro = "";
                                if (addlink.Contains("RDC"))
                                    macro = "RDC";
                                else if (addlink.Contains("SDC"))
                                    macro = "SDC";

                                // se trovo NULL allora non e' stata usata la macro RDC/SDC
                                if (string.IsNullOrWhiteSpace(macro))// aText.Substring(pComma1, pComma2 - pComma1).Contains("NULL")
                                {
                                    pComma1 = pComma2;
                                    pComma2 = aText.IndexOf(",", pComma1 + 1);

                                    if (pComma1 != -1 && pComma2 != -1 && pComma2 < pSemicolon)
                                    {
                                        aTemp = cf.Trim(aText.Substring(pComma1 + 1, pComma2 - pComma1 - 1).Trim(), "&", "(", ")");
                                        pTemp = aTemp.IndexOf(".");
                                        if (pTemp != -1 && pTemp < pSemicolon)
                                        {
                                            cl._recordpointer = aTemp.Substring(0, pTemp).Trim();
                                            cl._recordclass = SearchRecord(aFile, aText, cs._name, cl._recordpointer, pBracketOpened);
                                            cl._field = aTemp.Substring(pTemp + 1).Trim();
                                            cl._dbtpointer = SearchDBT(aFile, aText, cl._class, cl._recordclass);
                                            cl._dbtnamespace = SearchDBTNamespace(aFile, aText, cl._dbtpointer);
                                            cl._fieldnamespace = mDB.GetFieldNamespace(cl._recordclass, cl._field);
                                        }
                                        else
                                        {
                                            pTemp = aTemp.IndexOf("->");
                                            if (pTemp != -1 && pTemp < pSemicolon)
                                            {
                                                cl._recordpointer = aTemp.Substring(0, pTemp).Trim();
                                                cl._recordclass = SearchRecord(aFile, aText, cs._name, cl._recordpointer, pBracketOpened);
                                                cl._field = aTemp.Substring(pTemp + 2).Trim();
                                                cl._dbtpointer = SearchDBT(aFile, aText, cl._class, cl._recordclass);
                                                cl._dbtnamespace = SearchDBTNamespace(aFile, aText, cl._dbtpointer);
                                                cl._fieldnamespace = mDB.GetFieldNamespace(cl._recordclass, cl._field);
                                            }
                                        }
                                    }
                                }
                                else if (macro == "RDC")
                                {
                                    pBracketOpened = aText.IndexOf("(", aText.IndexOf(macro, startAddLink));
                                    pBracketClosed = aText.IndexOf(")", pBracketOpened);
                                    pTemp = aText.IndexOf("(", pBracketOpened + 1);
                                    if (pTemp != -1 && pTemp < pBracketClosed)
                                        pBracketClosed = aText.IndexOf(")", pBracketClosed + 1);

                                    aTemp = cf.Trim(aText.Substring(pBracketOpened + 1, pBracketClosed - pBracketOpened - 1).Trim(), "&", "(", ")");
                                    pComma1 = aTemp.IndexOf(",");

                                    if (pComma1 == -1)
                                        cl._field = aTemp.Trim();
                                    else
                                    {
                                        cl._recordpointer = aTemp.Substring(0, pComma1).Trim();
                                        cl._recordclass = SearchRecord(aFile, aText, cs._name, cl._recordpointer, pBracketOpened);
                                        cl._field = aTemp.Substring(pComma1 + 1).Trim();
                                        cl._dbtpointer = SearchDBT(aFile, aText, cl._class, cl._recordclass);
                                        cl._dbtnamespace = SearchDBTNamespace(aFile, aText, cl._dbtpointer);
                                        cl._fieldnamespace = mDB.GetFieldNamespace(cl._recordclass, cl._field);
                                    }
                                }
                                else if (macro == "SDC")
                                {
                                    macro = "SDC(m_pHKL";
                                    aTemp = aText.Substring(startAddLink, pSemicolon - startAddLink);
                                    if (aTemp.Contains(macro))// se c'è un hot link nell sdc
                                    {
                                        pBracketOpened = aTemp.IndexOf(macro);
                                        pBracketClosed = aTemp.IndexOf("->", pBracketOpened + macro.Length);
                                        cl._dbtnamespace = aTemp.Substring(pBracketOpened + macro.Length - 3, pBracketClosed - pBracketOpened - macro.Length + 3);
                                        pBracketOpened = aTemp.IndexOf("->", pBracketClosed + 2) + 2;
                                        pBracketClosed = aTemp.IndexOf(")", pBracketOpened);
                                        cl._field = aTemp.Substring(pBracketOpened, pBracketClosed - pBracketOpened);
                                        cl._field = cl._field.Substring(cl._field.IndexOf('_') + 1);
                                    }
                                    else
                                    {
                                        macro = "SDC(";
                                        pBracketOpened = aTemp.IndexOf(macro) + macro.Length;
                                        pBracketClosed = aTemp.IndexOf(")", pBracketOpened);
                                        cl._field = aTemp.Substring(pBracketOpened, pBracketClosed - pBracketOpened);
                                        cl._fieldnamespace = cl._field.Substring(cl._field.IndexOf('_') + 1);
                                    }
                                }

                                // Runtime Class
                                //pComma1 = pComma2;
                                //pComma2 = aText.IndexOf(",", pComma1 + 1);
                                pRuntimeClass = aText.IndexOf(RUNTIME_CLASS, startAddLink);
                                if (pRuntimeClass != -1)
                                {
                                    pBracketOpened = aText.IndexOf("(", pRuntimeClass + 1);
                                    pBracketClosed = aText.IndexOf(")", pBracketOpened);
                                    cl._runtimeclass = aText.Substring(pBracketOpened + 1, pBracketClosed - pBracketOpened - 1).Trim();
                                }

                                // Hotlink & Button
                                //int temp = addlink.IndexOf("RUNTIME_CLASS");
                                //int openSeparetor = -1;
                                //if (temp != -1)
                                //    openSeparetor = addlink.IndexOf(',', temp);
                                //int closeSeparetor;

                                //if (openSeparetor > 0)
                                //{
                                //    if (addlink.Substring(openSeparetor + 1).Contains(","))
                                //        closeSeparetor = addlink.IndexOf(',', openSeparetor + 1);
                                //    else
                                //        closeSeparetor = addlink.IndexOf(')', openSeparetor + 1);
                                //    macro = "RUNTIME_CLASS";
                                //    if (addlink.IndexOf(macro, temp + macro.Length) == -1 && addlink.IndexOf("NULL", temp + macro.Length) == -1)
                                //    {
                                //        macro = "m_p";
                                //        cl._hkl = addlink.Substring(openSeparetor + 1, closeSeparetor - openSeparetor - 1).Trim();
                                //        if (cl._hkl.Contains(macro))
                                //            cl._hkl = cl._hkl.Substring(cl._hkl.IndexOf(macro) + macro.Length);
                                //        macro = "NO_BUTTON";
                                //        if (addlink.Contains(macro))
                                //            cl._button = macro;
                                //        else if (addlink.Contains("BTN_DEFAULT"))
                                //            cl._button = "BTN_DEFAULT";
                                //    }
                                //}

                                pComma1 = aText.IndexOf(",", pBracketClosed + 1);
                                if (pComma1 != -1 && pComma1 < pSemicolon)
                                {
                                    pComma2 = aText.IndexOf(",", pComma1 + 1);
                                    if (pComma2 == -1 || pComma2 > pSemicolon)
                                        pComma2 = pSemicolon;

                                    pSeparator = aText.IndexOf("->", pComma1 + 1);
                                    if (pSeparator == -1)
                                    {
                                        var hkl = cf.HKL(aText.Substring(pComma1 + 1, pComma2 - pComma1 - 1).Trim());
                                        cl._hkl = cf.Trim(SearchHKL(aFile, aText, cs._name, hkl.Item1), ")", "(");
                                        cl._button = hkl.Item2;
                                    }
                                    else if (pSeparator < pSemicolon)
                                    {
                                        var hkl = cf.HKL(aText.Substring(pSeparator + 2, pComma2 - pSeparator - 2).Trim());
                                        cl._hkl = cf.Trim(SearchHKL(aFile, aText, cs._name, hkl.Item1), ")", "(");
                                        cl._button = hkl.Item2;
                                    }
                                    else
                                    {
                                        //codice che mette il nome classe
                                        var hkl = cf.HKL(aText.Substring(pComma1 + 1, pComma2 - pComma1 - 1).Trim());
                                        cl._hkl = cf.Trim(SearchHKL(aFile, aText, cs._name, hkl.Item1), ")", "(");
                                        cl._button = hkl.Item2;

                                    }
                                }
                                
                                // SetRange SetCtrlSize SetCtrlMaxLen
                                int next = aText.IndexOf(ADDLINK, pSemicolon);
                                int graffa = aText.IndexOf("}", pSemicolon);
                                if (next != -1 && graffa > next)
                                {
                                    string text = aText.Substring(pSemicolon, next - pSemicolon).Replace(" ", "");
                                    SetRange(text, out cl._minValue, out cl._maxValue);
                                    SetCtrlSize(text, out cl._chars, out cl._rows);
                                    if (string.IsNullOrWhiteSpace(cl._chars))
                                        SetCtrlMaxLen(text, out cl._chars);
                                }
                            }
                            else
                                System.Diagnostics.Debug.WriteLine("\tError:  " + cl._idc);
                        }
                        mControls.Add(cl);
                        System.Diagnostics.Debug.WriteLine("\t\t\t            IDC: " + cl._idc);
                        System.Diagnostics.Debug.WriteLine("\t\t\t           Type: AddLink");
                        System.Diagnostics.Debug.WriteLine("\t\t\t      Namespace: " + cl._namespace);
                        System.Diagnostics.Debug.WriteLine("\t\t\t            DBT: " + cl._dbtpointer);
                        System.Diagnostics.Debug.WriteLine("\t\t\t  DBT Namespace: " + cl._dbtnamespace);
                        System.Diagnostics.Debug.WriteLine("\t\t\t       Function: " + cl._recordpointer);
                        System.Diagnostics.Debug.WriteLine("\t\t\t         Record: " + cl._recordclass);
                        System.Diagnostics.Debug.WriteLine("\t\t\tField Namespace: " + cl._fieldnamespace);
                        System.Diagnostics.Debug.WriteLine("\t\t\t          Field: " + cl._field);
                        System.Diagnostics.Debug.WriteLine("\t\t\t  Runtime Class: " + cl._runtimeclass);
                        System.Diagnostics.Debug.WriteLine("\t\t\t     HotKeyLink: " + cl._hkl);
                        System.Diagnostics.Debug.WriteLine("\t\t\t         Button: " + cl._button);
                        System.Diagnostics.Debug.WriteLine("");
                    }
                    pTag = aText.IndexOf(ADDLINK, pTag + 1);
                }
            }
        }
        #endregion

        #region addLabel

        private void SearchClassesAddLabel(string aText)
        {
            string ADDLABEL = "AddLabelLinkWithLine";
            int end;
            int init = aText.IndexOf(ADDLABEL);
            if (init > 0)
            {
                init = aText.IndexOf("(",init);
                end = aText.IndexOf(")", init);
                mLabel.Add( aText.Substring(init + 1, end - init - 1));
            } 
           
        }
        #endregion

        #region addcolumn
        private void SearchAddColumns(string aModule, string aFile, string aText)
        {
            int pTag = -1;
            int pBracketOpened = -1;
            int pBracketClosed = -1;
            int pQuotationMarksOpened = -1;
            int pQuotationMarksClosed = -1;
            int pSemicolon = -1;
            int pComma1 = -1;
            int pComma2 = -1;
            int pSeparator = -1;
            int pRuntimeClass = -1;
            int nOrder = -1;

            foreach (_CLASS cs in mClasses)
            {
                if (cs._isaddlink)
                    continue;

                if (CurrentClass != null)
                    CurrentClass(null, cs._name);
                
                pTag = aText.IndexOf(ADDCOLUMN, cs._start);

                while (pTag != -1 && pTag < cs._end)
                {
                    pBracketOpened = aText.IndexOf("(", pTag);
                    if (pBracketOpened != -1)
                    {
                        _CONTROL cl = new _CONTROL();

                        cl._generatejson = true;
                        cl._module = aModule;
                        cl._filename = aFile;
                        cl._classidd = cs._idd;
                        cl._class = cs._name;
                        cl._tilesize = cs._tilesize;
                        cl._tiletext = cs._tiletext;
                        cl._tilestyle = cs._tilestyle;
                        cl._bodyeditidc = cs._idc;
                        cl._bodyedittext = cs._text;
                        cl._idc = string.Empty;
                        cl._namespace = string.Empty;
                        cl._fieldnamespace = string.Empty;
                        cl._text = string.Empty;
                        cl._combotype = string.Empty;
                        cl._recordpointer = string.Empty;
                        cl._dbtpointer = string.Empty;
                        cl._dbtnamespace = string.Empty;
                        cl._recordclass = string.Empty;
                        cl._field = string.Empty;
                        cl._runtimeclass = string.Empty;
                        cl._hkl = string.Empty;
                        cl._button = string.Empty;
                        cl._isaddlink = false;
                        nOrder++;
                        cl._order = nOrder.ToString().PadLeft(5, '0');

                        if (!mBodyEditIDCs.Contains(cl._bodyeditidc))
                            mBodyEditIDCs.Add(cl._bodyeditidc, string.Empty);

                        // Namespace
                        pQuotationMarksOpened = aText.IndexOf("\"", pBracketOpened + 1);
                        pQuotationMarksClosed = aText.IndexOf("\"", pQuotationMarksOpened + 1);
                        cl._namespace = aText.Substring(pQuotationMarksOpened + 1, pQuotationMarksClosed - pQuotationMarksOpened - 1).Trim();

                        // Text
                        pComma1 = aText.IndexOf(",", pQuotationMarksClosed + 1);
                        pComma2 = aText.IndexOf(",", pComma1 + 1);
                        pQuotationMarksOpened = aText.IndexOf("\"", pComma1 + 1);
                        pQuotationMarksClosed = aText.IndexOf("\"", pQuotationMarksOpened + 1);
                        if (pQuotationMarksOpened > pComma2)
                            pQuotationMarksClosed = pComma1;
                        else
                            cl._text = aText.Substring(pQuotationMarksOpened + 1, pQuotationMarksClosed - pQuotationMarksOpened - 1).Trim();

                        // Style
                        pComma1 = aText.IndexOf(",", pQuotationMarksClosed + 1);
                        pComma2 = aText.IndexOf(",", pComma1 + 1);
                        cl._combotype = aText.Substring(pComma1 + 1, pComma2 - pComma1 - 1).Trim();

                        // idc
                        pComma1 = pComma2;
                        pComma2 = aText.IndexOf(",", pComma1 + 1);
                        cl._idc = aText.Substring(pComma1 + 1, pComma2 - pComma1 - 1).Trim();

                        if (CurrentIDC != null)
                            CurrentIDC(null, cl._idc);
                        // Function & Field
                        pComma1 = pComma2;
                        pComma2 = aText.IndexOf(",", pComma1 + 1);
                        pSeparator = aText.IndexOf("->", pComma1 + 1);
                        if (pSeparator == -1 || pSeparator > pComma2)
                            cl._field = aText.Substring(pComma1 + 1, pComma2 - pComma1 - 1).Trim();
                        else
                        {
                            pBracketOpened = aText.IndexOf("(", pComma1 + 1);
                            pBracketClosed = aText.LastIndexOf(")", pComma2);
                            if (pBracketOpened == -1 && pBracketClosed == -1 || pBracketOpened > pComma1 || pBracketClosed > pComma2)
                            {
                                pBracketOpened = pComma1;
                                pBracketClosed = pComma2;
                            }
                            cl._recordpointer = cf.Trim(aText.Substring(pBracketOpened + 2, pSeparator - pBracketOpened - 2).Trim(), "&", "(");
                            cl._recordclass = SearchRecord(aFile, aText, cs._name, cl._recordpointer, pBracketOpened);
                            cl._field = aText.Substring(pSeparator + 2, pBracketClosed - pSeparator - 2).Trim();
                            if (cl._field.EndsWith(")"))
                                cl._field = cl._field.Substring(0, cl._field.Length - 1);
                            cl._dbtpointer = SearchDBT(aFile, aText, cl._class, cl._recordclass);
                            cl._dbtnamespace = SearchDBTNamespace(aFile, aText, cl._dbtpointer);
                            cl._fieldnamespace = mDB.GetFieldNamespace(cl._recordclass, cl._field);
                        }

                        // Runtime Class
                        pRuntimeClass = aText.IndexOf(RUNTIME_CLASS, pComma2 + 1);
                        pBracketOpened = aText.IndexOf("(", pRuntimeClass + 1);
                        pBracketClosed = aText.IndexOf(")", pBracketOpened);
                        cl._runtimeclass = aText.Substring(pBracketOpened + 1, pBracketClosed - pBracketOpened - 1).Trim();

                        // Hotlink & Button
                        pComma1 = aText.IndexOf(",", pBracketClosed + 1);
                        pSemicolon = aText.IndexOf(";", pBracketClosed + 1);
                        if (pComma1 != -1 && pComma1 < pSemicolon)
                        {
                            pSeparator = aText.IndexOf("->", pComma1 + 1);
                            if (pSeparator < pSemicolon)
                            {
                                var hkl = cf.HKL(aText.Substring(pSeparator + 2, pSemicolon - pSeparator - 2).Trim());
                                cl._hkl = cf.Trim(SearchHKL(aFile, aText, cs._name, hkl.Item1), ")", "(");
                                cl._button = hkl.Item2;
                            }
                            else
                            {
                                var hkl = cf.HKL(aText.Substring(pComma1, pSemicolon - pComma1 - 1).Trim());
                                cl._hkl = cf.Trim(SearchHKL(aFile, aText, cs._name, hkl.Item1), ")", "(");
                                cl._button = hkl.Item2;
                            }
                        }
                        //modificatori grafici
                        //"hidden": true,"noChangeGrayed": true,"grayed" :  true    
                        string func = "pColInfo->SetStatus(";
                        string setStatus;
                        int tagSetStatus, tagSemiColon;
                        if ((tagSetStatus = aText.IndexOf(func, pSemicolon)) != -1)
                        {
                            if (( tagSemiColon = aText.IndexOf(";", pSemicolon + 1)) > tagSetStatus)
                            {
                                setStatus = aText.Substring(tagSetStatus + func.Length,  tagSemiColon- tagSetStatus - func.Length);
                                cl._hidden = setStatus.Contains("STATUS_HIDDEN");
                                cl._grayed = setStatus.Contains("STATUS_GRAYED");
                                cl._noChange_Grayed = setStatus.Contains("STATUS_NOCHANGE_GRAYED"); 
                            }                            
                        }
                        //int next =aText.IndexOf(ADDCOLUMN, pSemicolon);
                        //string text = aText.Substring(pSemicolon, next - pSemicolon);
                        //SetRange(text, out cl._minValue, out cl._maxValue);




                        mControls.Add(cl);
                        System.Diagnostics.Debug.WriteLine("\t\t\t             IDC: " + cl._idc);
                        System.Diagnostics.Debug.WriteLine("\t\t\t            Type: AddColumn");
                        System.Diagnostics.Debug.WriteLine("\t\t\t       Namespace: " + cl._namespace);
                        System.Diagnostics.Debug.WriteLine("\t\t\t             DBT: " + cl._dbtpointer);
                        System.Diagnostics.Debug.WriteLine("\t\t\t   DBT Namespace: " + cl._dbtnamespace);
                        System.Diagnostics.Debug.WriteLine("\t\t\t        Function: " + cl._recordpointer);
                        System.Diagnostics.Debug.WriteLine("\t\t\t          Record: " + cl._recordclass);
                        System.Diagnostics.Debug.WriteLine("\t\t\t Field Namespace: " + cl._fieldnamespace);
                        System.Diagnostics.Debug.WriteLine("\t\t\t           Field: " + cl._field);
                        System.Diagnostics.Debug.WriteLine("\t\t\t   Runtime Class: " + cl._runtimeclass);
                        System.Diagnostics.Debug.WriteLine("\t\t\t      HotKeyLink: " + cl._hkl);
                        System.Diagnostics.Debug.WriteLine("\t\t\t          hidden: " + cl._hidden.ToString());
                        System.Diagnostics.Debug.WriteLine("\t\t\t          grayed: " + cl._grayed.ToString());
                        System.Diagnostics.Debug.WriteLine("\t\t\tNo Change Grayed: " + cl._noChange_Grayed.ToString());
                        System.Diagnostics.Debug.WriteLine("");
                    }
                    pTag = aText.IndexOf(ADDCOLUMN, pTag + 1);
                }
            }
        }
        #endregion

        #region bodyedit
        private void CheckBodyedit()
        {
            foreach (_CONTROL cl in mControls)
            {
                if (cl._isaddlink && mBodyEditIDCs.Contains(cl._idc))
                    cl._generatejson = false;
            }
        }
        #endregion

        #region Class IDD
        private string SearchClassIDD(string aFile, string aText, string aClass, bool bClass = true)
        {
            if (string.IsNullOrEmpty(aClass))
                return string.Empty;


            string t = aText.Replace(" ", "");
            string c = string.Empty;
            if (bClass)
                c = aClass + "::" + aClass;
            else
                c = aClass + "()";
            string idd = "";
            int pCostrunctor = t.IndexOf(c);
            if (pCostrunctor == -1)
                idd = bClass ? SearchClassIDDInLine(aFile, t, aClass) : string.Empty;
            if (!string.IsNullOrWhiteSpace(idd))
                return idd;
            if (pCostrunctor == -1)
                return bClass ? SearchClassIDDInHeader(aFile, aClass) : string.Empty;

            int pCurlyBracketOpened = t.IndexOf("{", pCostrunctor);
            if (pCurlyBracketOpened == -1)
                return bClass ? SearchClassIDDInHeader(aFile, aClass) : string.Empty;

            int pComma = t.LastIndexOf(",", pCurlyBracketOpened);
            if (pComma == -1)
                return bClass ? SearchClassIDDInHeader(aFile, aClass) : string.Empty;

            int pBracketClosed = t.LastIndexOf(")", pCurlyBracketOpened);
            if (pBracketClosed == -1)
                return bClass ? SearchClassIDDInHeader(aFile, aClass) : string.Empty;

            return t.Substring(pComma + 1, pBracketClosed - pComma - 1).Trim();
        }
        private string SearchClassIDDInLine(string aFile,string aText , string aClass)
        {
            string idd = "", costruttore = aClass + "():CTileDialog(";
            if (string.IsNullOrEmpty(aClass))
                return string.Empty;
            int start = aText.IndexOf(costruttore);
            if (start == -1)
                return string.Empty;
            int end = aText.IndexOf("{}", start + costruttore.Length);
            idd = aText.Substring(start + costruttore.Length, end - start - costruttore.Length);
            start = idd.IndexOf(',');
            end = idd.Length;
            if (start != -1)
                idd = idd.Substring(start + 1, end - 2 - start);
            return idd;
        }

        private string SearchClassIDDInHeader(string aFile, string aClass)
        {
            if (string.IsNullOrEmpty(aClass))
                return string.Empty;

            string aHeaderFile = aFile.Replace(".cpp", ".h");

            if (!File.Exists(aHeaderFile))
                return string.Empty;

            string aText = File.ReadAllText(aHeaderFile, Encoding.Default);
            aText = cf.Clean(aText);
            aText = aText.Replace(" ", "");
            return SearchClassIDD(aFile, aText, aClass, false);
        }

        private Tuple<string, string, string, string> SearchBodyEditContainerClass(string aFile, string aText, string aClass)
        {
            if (string.IsNullOrEmpty(aClass) || string.IsNullOrEmpty(aClass))
                return new Tuple<string, string, string, string>(string.Empty, string.Empty, string.Empty, string.Empty);

            string aBodyEditFolder = Path.GetDirectoryName(aFile);
            var b = SearchBodyEditContainerClass(aText, aClass);
            if (!string.IsNullOrEmpty(b.Item1) && !string.IsNullOrEmpty(b.Item2))
                return b;

            List<string> ls = cf.FilesContent(aBodyEditFolder, "*.cpp", false, RUNTIME_CLASS, aClass);
            foreach (string s in ls)
            {
                b = SearchBodyEditContainerClass(s, aClass);
                if (!string.IsNullOrEmpty(b.Item1) && !string.IsNullOrEmpty(b.Item2))
                    return b;
            }
            return new Tuple<string, string, string, string>(string.Empty, string.Empty, string.Empty, string.Empty);
        }

        private Tuple<string, string, string, string> SearchBodyEditContainerClass(string aText, string aClass)
        {
            //       string t = aText.Replace(" ", "");
                   string t = aText;
            string c = RUNTIME_CLASS + "(" + aClass + ")";

            int pRuntimeClass = t.IndexOf(c);
            if (pRuntimeClass == -1)
                return new Tuple<string, string, string, string>(string.Empty, string.Empty, string.Empty, string.Empty);

            int pStartText = aText.IndexOf("\"", pRuntimeClass);
            if (pStartText == -1)
                return new Tuple<string, string, string, string>(string.Empty, string.Empty, string.Empty, string.Empty);

            int pEndText = aText.IndexOf("\"", pStartText + 1);
            if (pEndText == -1)
                return new Tuple<string, string, string, string>(string.Empty, string.Empty, string.Empty, string.Empty);

            string aBodyEditText = aText.Substring(pStartText + 1, pEndText - pStartText - 1).Trim();

            int pBuildDataControlLinks = t.LastIndexOf(BUILD_DATA_CONTROL_LINKS, pRuntimeClass);
            if (pBuildDataControlLinks == -1)
                return new Tuple<string, string, string, string>(string.Empty, string.Empty, string.Empty, aBodyEditText);

            int pVoid = t.LastIndexOf(VOID, pBuildDataControlLinks);
            if (pVoid == -1)
                return new Tuple<string, string, string, string>(string.Empty, string.Empty, string.Empty, aBodyEditText);

            int pSeparator = t.IndexOf("::", pVoid);
            if (pSeparator == -1 || pSeparator > pBuildDataControlLinks)
                return new Tuple<string, string, string, string>(string.Empty, string.Empty, string.Empty, aBodyEditText);

            string aContainerClass = t.Substring(pVoid + 4, pSeparator - pVoid - 4).Trim();

            int pAddLink = t.LastIndexOf(ADDLINK, pRuntimeClass);
            if (pAddLink == -1 || pAddLink < pBuildDataControlLinks)
                return new Tuple<string, string, string, string>(aContainerClass, string.Empty, string.Empty, aBodyEditText);

            int pBracketOpened = t.IndexOf("(", pAddLink);
            if (pAddLink == -1 || pAddLink > pRuntimeClass)
                return new Tuple<string, string, string, string>(aContainerClass, string.Empty, string.Empty, aBodyEditText);

            int pComma = t.IndexOf(",", pBracketOpened);
            if (pBracketOpened == -1 || pComma > pRuntimeClass)
                return new Tuple<string, string, string, string>(aContainerClass, string.Empty, string.Empty, aBodyEditText);

            string aIDC = t.Substring(pBracketOpened + 1, pComma - pBracketOpened - 1).Trim();

            c = aContainerClass + "::" + aContainerClass;
            int pConstructor = t.IndexOf(c);
            if (pConstructor == -1)
                return new Tuple<string, string, string, string>(aContainerClass, aIDC, string.Empty, aBodyEditText);

            int pCurlyBracketOpened = t.IndexOf("{", pConstructor);
            if (pCurlyBracketOpened == -1)
                return new Tuple<string, string, string, string>(aContainerClass, aIDC, string.Empty, aBodyEditText);

            int pBracketClosed = t.LastIndexOf(")", pCurlyBracketOpened);
            if (pBracketClosed == -1 || pBracketClosed < pConstructor)
                return new Tuple<string, string, string, string>(aContainerClass, aIDC, string.Empty, aBodyEditText);

            pComma = t.LastIndexOf(",", pBracketClosed);
            if (pComma == -1 || pComma < pConstructor)
                return new Tuple<string, string, string, string>(aContainerClass, aIDC, string.Empty, aBodyEditText);

            string aIDD = t.Substring(pComma + 1, pBracketClosed - pComma - 1).Trim();

            return new Tuple<string, string, string, string>(aContainerClass, aIDC, aIDD, aBodyEditText);
        }

        private string SearchBodyEditContainerClassInHeader(string aFile, string aClass)
        {
            if (string.IsNullOrEmpty(aClass))
                return string.Empty;

            string d = string.Empty;

            List<string> ls = cf.FilesContent(Path.GetDirectoryName(aFile), "*.h", true, aClass);
            foreach (string s in ls)
            {
                d = SearchBodyEditContainerClassInHeader2(s, aClass);
                if (!string.IsNullOrEmpty(d))
                    return d;
            }
            return d;
        }
        
        private string SearchBodyEditContainerClassInHeader2(string aText, string aClass)
        {
            string c = aClass + "()";

            int pClass = aText.IndexOf(c);
            if (pClass == -1)
                return string.Empty;

            int pCurlyBracketOpened = aText.IndexOf("{", pClass);
            if (pCurlyBracketOpened == -1)
                return string.Empty;

            int pBracketClosed = aText.LastIndexOf(")", pCurlyBracketOpened);
            if (pBracketClosed == -1 || pBracketClosed < pClass)
                return string.Empty;

            int pComma = aText.LastIndexOf(",", pBracketClosed);
            if (pComma == -1 || pComma < pClass)
                return string.Empty;

            return aText.Substring(pComma + 1, pBracketClosed - pComma - 1).Trim();
        }
        #endregion

        #region Tile 
        private Tuple<int, string, string> SearchTile(string aFile, string aText, string aClass)
        {
            if (string.IsNullOrEmpty(aClass))
                return new Tuple<int, string, string>(0, string.Empty, string.Empty);

            string aTileFolder = Path.GetDirectoryName(aFile);
            
            var t = SearchTile(aText, aClass);
            if (!string.IsNullOrEmpty(t.Item3))
                return t;

            List<string> ls = cf.FilesContent(aTileFolder, "*.cpp", false, RUNTIME_CLASS, aClass);
            foreach (string s in ls)
            {
                t = SearchTile(s, aClass);
                if (!string.IsNullOrEmpty(t.Item3))
                    return t;
            }
            return t;
        }
        private Tuple<int, string, string> SearchTile(string aText, string aClass)
        {
            int aTileStyle = 0;
            string aTileText = string.Empty;
            string aTileSize = string.Empty;

            if (string.IsNullOrEmpty(aClass))
                return new Tuple<int, string, string>(0, string.Empty, string.Empty);

            string t = aText;//.Replace(" ", "");
            string rc = RUNTIME_CLASS + "(" + aClass + ")";

            int pRuntimeClass = t.IndexOf(rc);
            if (pRuntimeClass == -1)
                return new Tuple<int, string, string>(0, string.Empty, string.Empty);

            int pCustomize = t.LastIndexOf(CUSTOMIZE, pRuntimeClass);
            if (pCustomize == -1)
                return new Tuple<int, string, string>(0, string.Empty, string.Empty);

            int pTemp = t.LastIndexOf(";", pRuntimeClass);
            if (pTemp > pCustomize)
                pCustomize = pTemp;

            int pAddTile = t.LastIndexOf(ADDTILE, pRuntimeClass);
            int separetor = t.LastIndexOf(';', pRuntimeClass);
            if (pAddTile == -1 || pAddTile < separetor)
            {
                pAddTile = t.LastIndexOf(ADDFILTERTILE, pRuntimeClass);
                aTileStyle = 2;
            }
            if (pAddTile == -1 || pAddTile < separetor)
            {
                pAddTile = t.LastIndexOf(ADDTILEDIALOG, pRuntimeClass);
                aTileStyle = 2;
            }
            if (pAddTile == -1 || pAddTile < separetor)
            {
                pAddTile = t.LastIndexOf(ADDACTIONTILE, pRuntimeClass);
                aTileStyle = 2;//non so se è giusto
            }

            if (pAddTile == -1 || pAddTile < separetor)
                return new Tuple<int, string, string>(0, string.Empty, string.Empty);

            int pQuotationMarksOpened = t.IndexOf("\"", pAddTile);
            if (pQuotationMarksOpened == -1)
                return new Tuple<int, string, string>(0, string.Empty, string.Empty);

            int pQuotationMarksClosed = t.IndexOf("\"", pQuotationMarksOpened + 1);
            if (pQuotationMarksClosed == -1)
                return new Tuple<int, string, string>(0, string.Empty, string.Empty);

            aTileText = t.Substring(pQuotationMarksOpened + 1, pQuotationMarksClosed - pQuotationMarksOpened - 1).Trim();

            int pComma = t.IndexOf(",", pQuotationMarksClosed);
            if (pComma == -1)
                return new Tuple<int, string, string>(0, string.Empty, string.Empty);

            int pBracketClosed = t.IndexOf(")", pComma);
            if (pBracketClosed == -1)
                return new Tuple<int, string, string>(0, string.Empty, string.Empty);

            pTemp = t.IndexOf(",", pComma + 1);
            if (pTemp != -1 && pTemp < pBracketClosed)
                pBracketClosed = pTemp;

            aTileSize = t.Substring(pComma + 1, pBracketClosed - pComma - 1).Trim();

            if (aTileSize.StartsWith("TileDialogSize::"))
                aTileSize = aTileSize.Substring(16);
            return new Tuple<int, string, string>(aTileStyle, aTileText, aTileSize);
        }
        #endregion

        #region record
        private string SearchRecord(string aFile, string aText, string aClass, string aFunction, int aStart)
        {
            if (string.IsNullOrEmpty(aClass) || string.IsNullOrEmpty(aFunction))
                return string.Empty;

            string aKey = aClass + "::" + aFunction;

            if (mFunctions.Contains(aKey))
                return mFunctions[aKey].ToString();

            string r = string.Empty;
            string f = string.Empty;
            int pStartRecord = -1;
            int pStartFunction = -1;
            int pTemp = -1;

            if (aFunction.Contains("("))
            {
                pStartFunction = aText.LastIndexOf(aKey);
                if (pStartFunction == -1)
                {
                    f = aFunction.Replace("(", "");
                    f = f.Replace(")", "");
                    // prima faccio ancora la ricerca nel cpp
                    r = SearchRecordInHeader(aFile, aText, aClass, f);
                    if (string.IsNullOrEmpty(r))
                        r = SearchRecordInHeader(aFile, aClass, f);
                }
                else
                {
                    pStartRecord = aText.LastIndexOf("}", pStartFunction);
                    pTemp = aText.LastIndexOf(";", pStartFunction);
                    if (pTemp > pStartRecord)
                        pStartRecord = pTemp;
                    pTemp = aText.LastIndexOf(")", pStartFunction);
                    if (pTemp > pStartRecord)
                        pStartRecord = pTemp;
                    r = aText.Substring(pStartRecord + 1, pStartFunction - pStartRecord - 1);
                }
            }
            else
            {
                pStartFunction = cf.PreviousValid(aText, aFunction, aStart);
                if (pStartFunction == -1)
                    return string.Empty;
                pStartRecord = aText.LastIndexOf("{", pStartFunction);
                pTemp = aText.LastIndexOf(";", pStartFunction);
                if (pTemp > pStartRecord)
                    pStartRecord = pTemp;
                r = aText.Substring(pStartRecord + 1, pStartFunction - pStartRecord - 1);
            }
            r = r.Replace("*", "").Trim();
            mFunctions.Add(aKey, r);
            return r;
        }

        private string SearchRecordInHeader(string aFile, string aClass, string aFunction)
        {
            if (string.IsNullOrEmpty(aClass) || string.IsNullOrEmpty(aFunction))
                return string.Empty;

            string aHeaderFile = aFile.Replace(".cpp", ".h");

            if (!File.Exists(aHeaderFile))
                return string.Empty;

            string aText = File.ReadAllText(aHeaderFile, Encoding.Default);
            aText = cf.Clean(aText);
            string r = SearchRecordInHeader(aFile, aText, aClass, aFunction);

            if (r.Contains("public:"))
            { 
                r = r.Replace("public:", "");
                r = r.Trim();
            }

            if (string.IsNullOrEmpty(r))
            {
                var ff = cf.FindSingleFile(Path.GetDirectoryName(aFile), "*.h", aFunction);
                r = SearchRecordInHeader(ff.Item1, ff.Item2, aClass, aFunction);
                if (string.IsNullOrEmpty(r))
                    r = SearchRecordInHeader(ff.Item1, ff.Item2, aClass, aFunction, false);
            }
            return r;
        }

        private string SearchRecordInHeader(string aFile, string aText, string aClass, string aFunction, bool bClass = true)
        {
            if (string.IsNullOrEmpty(aFile) || string.IsNullOrEmpty(aText) || string.IsNullOrEmpty(aClass) || string.IsNullOrEmpty(aFunction))
                return string.Empty;

            bool isHeader = Path.GetExtension(aFile) == ".h";

            int pClassStart = -1;
            int pClassEnd = -1;
            if (bClass)
            {
                pClassStart = cf.NextValid(aText, aClass + ":", 0, string.Empty, "::");
                if (pClassStart == -1)
                    pClassStart = cf.NextValid(aText, aClass + " :", 0, string.Empty, "::");
                if (pClassStart == -1)
                    return string.Empty;

                pClassEnd = cf.NextValid(aText, CLASS, pClassStart);
                if (pClassEnd == -1)
                    if (isHeader)
                        pClassEnd = cf.NextValid(aText, INCLUDE, pClassStart);
                    else
                        pClassEnd = cf.NextValid(aText, IMPLEMENT_DYNCREATE, pClassStart);
                if (pClassEnd == -1)
                    return string.Empty;
            }
            else
                pClassStart = 0;

            int pStartFunction = cf.NextValid(aText, aFunction, pClassStart);
            if (pStartFunction == -1 || (pStartFunction > pClassEnd && pClassEnd != -1))
                return string.Empty;

            int pStartRecord = aText.LastIndexOf(";", pStartFunction);
            int pTemp = -1;
            if (isHeader)
            {
                pTemp = aText.LastIndexOf(":", pStartFunction);
                if (pTemp > pStartRecord)
                    pStartRecord = pTemp;
            }
            pTemp = aText.LastIndexOf("}", pStartFunction);
            if (pTemp > pStartRecord)
                pStartRecord = pTemp;
            if (pStartRecord < pClassStart)
                return string.Empty;

            string aRecord = aText.Substring(pStartRecord + 2, pStartFunction - pStartRecord - 2).Trim();

            if (aRecord.Contains("::"))
            {
                aRecord = aRecord.Replace("::", "");
                aRecord = aRecord.Replace(aClass, "");
            }
            aRecord = aRecord.Replace("*", "");
            return aRecord;
        }
        #endregion

        #region hotkeylink
        private string SearchHKL(string aFile, string aText, string aClass, string aHKL)
        {
            aHKL = aHKL.Trim();

            if (string.IsNullOrEmpty(aClass) || string.IsNullOrEmpty(aHKL) || aHKL == "NULL")
                return string.Empty;

            // primo tentativo: hkl definito nel corrente source
            string aHKLFolder = Path.GetDirectoryName(aFile);
            string hklClass = SearchHKL(aText, aClass, aHKL);
            if (!string.IsNullOrEmpty(hklClass))
                return hklClass;

            //secondo tentativo: cerca per naming convention
            string docFile = cf.SearchDocFile(aFile);
            if (!string.IsNullOrEmpty(docFile))
            {
                string content = cf.FileContent(docFile, false, ONATTACHDATA, aHKL);
                if (!string.IsNullOrEmpty(content))
                {
                    hklClass = SearchHKL(content, aClass, aHKL);
                    if (!string.IsNullOrEmpty(hklClass))
                        return hklClass;
                }
            }

            // terzo tentativo: cerco negli altri cpp generici del folder
            List<string> contents = cf.FilesContent(aHKLFolder, "*.cpp", false, ONATTACHDATA, aHKL);
            foreach (string content in contents)
            {
                hklClass = SearchHKL(content, aClass, aHKL);
                if (!string.IsNullOrEmpty(hklClass))
                    return hklClass;
            }
            return hklClass;
        }

        private string SearchHKL(string aText, string aClass, string aHKL)
        {
            if (string.IsNullOrEmpty(aClass) || string.IsNullOrEmpty(aHKL))
                return string.Empty;

            string aKey = aClass + "::" + aHKL;
            if (mHotKeyLinks.Contains(aKey))
                return mHotKeyLinks[aKey].ToString();

            int pAttachData = cf.NextValid(aText, ONATTACHDATA);
            if (pAttachData == -1)
                return string.Empty;

            int pCurlyBracketClosed = aText.IndexOf("}", pAttachData);
            if (pCurlyBracketClosed == -1)
                return string.Empty;

            int pHKL = cf.NextValid(aText, aHKL, pAttachData);
            if (pHKL == -1 || pHKL > pCurlyBracketClosed)
                return string.Empty;

            int pSemicolon = aText.IndexOf(";", pHKL);
            if (pSemicolon == -1 || pSemicolon > pCurlyBracketClosed)
                return string.Empty;

            int pNew = cf.NextValid(aText, NEW, pHKL);
            if (pNew == -1 || pNew > pCurlyBracketClosed)
                return string.Empty;

            string h = aText.Substring(pNew + 3, pSemicolon - pNew - 3).Trim();
            mHotKeyLinks.Add(aKey, h);
            return h;
        }
        #endregion

        #region DBT
        private string SearchDBT(string aFile, string aText, string aClass, string aRecord)
        {
            if (string.IsNullOrEmpty(aClass) || string.IsNullOrEmpty(aRecord))
                return string.Empty;

            string aDBTFolder = Path.GetDirectoryName(aFile);
            string aDBTFile = Path.GetFileName(aFile);

            string d = SearchDBT(aText, aClass, aRecord);
            if (!string.IsNullOrEmpty(d))
                return d;

            // cerca per naming convention
            string docFile = cf.SearchDocFile(aFile);
            if (!string.IsNullOrEmpty(docFile))
            {
                string content = cf.FileContent(docFile, false, ONATTACHDATA, aRecord);
                if (!string.IsNullOrEmpty(content))
                {
                    d = SearchDBT(content, aClass, aRecord);
                    if (!string.IsNullOrEmpty(d))
                        return d;
                }
            }

            List<string> contents = cf.FilesContent(aDBTFolder, "*.cpp", false, ONATTACHDATA, aRecord);
            foreach (string content in contents)
            {
                d = SearchDBT(content, aClass, aRecord);
                if (!string.IsNullOrEmpty(d))
                    return d;
            }
            return d;
        }

        private string SearchDBT(string aText, string aClass, string aRecord)
        {
            if (string.IsNullOrEmpty(aClass) || string.IsNullOrEmpty(aRecord))
                return string.Empty;

            string aKey = aClass + "::" + aRecord;
            if (mDBTs.Contains(aKey))
                return mDBTs[aKey].ToString();

            int pAttachData = cf.NextValid(aText, ONATTACHDATA);
            if (pAttachData == -1)
                return string.Empty;

            int pCurlyBracketClosed = CommonFunctions.IndexSubstingBracket(aText, aText.IndexOf("{", pAttachData));
            if (pCurlyBracketClosed == -1)
                return string.Empty;

            int pRecord = cf.NextValid(aText, aRecord, pAttachData);
            if (pRecord == -1 || pRecord > pCurlyBracketClosed)
                return string.Empty;

            int pNew = cf.PreviousValid(aText, NEW, pRecord);
            if (pNew == -1 || pNew < pAttachData)
                return string.Empty;

            int pRuntimeClass = cf.NextValid(aText, RUNTIME_CLASS, pNew);
            if (pRuntimeClass == -1 || pRuntimeClass > pRecord)
                return string.Empty;

            string d = aText.Substring(pNew + 3, pRuntimeClass - pNew - 4).Trim();
            mDBTs.Add(aKey, d);
            return d;
        }

        private string SearchDBTNamespace(string aFile, string aText, string aDBT)
        {
            if (string.IsNullOrEmpty(aDBT))
                return string.Empty;

            if (string.IsNullOrEmpty(aDBT))
                return string.Empty;

            string aDBTFolder = Path.GetDirectoryName(aFile);
            string aDBTFile = Path.GetFileName(aFile);

            string d = SearchDBTNamespace(aText, aDBT);
            if (!string.IsNullOrEmpty(d))
                return d;

            // cerca per naming convention
            string docFile = cf.SearchDocFile(aFile);
            if (!string.IsNullOrEmpty(docFile))
            {
                string s = cf.FileContent(docFile, true, aDBT + "::" + aDBT);
                if (!string.IsNullOrEmpty(s))
                {
                    d = SearchDBTNamespace(s, aDBT);
                    if (!string.IsNullOrEmpty(d))
                        return d;
                }
            }

            List<string> ls = cf.FilesContent(aDBTFolder, "*.cpp", true, aDBT + "::" + aDBT); 
            foreach (string s in ls)
            {
                d = SearchDBTNamespace(s, aDBT);
                if (!string.IsNullOrEmpty(d))
                    return d;
            }
            return d;
        }

        private string SearchDBTNamespace(string aText, string aDBT)
        {
            if (string.IsNullOrEmpty(aDBT))
                return string.Empty;

            if (mDBTNamespaces.Contains(aDBT))
                return mDBTNamespaces[aDBT].ToString();

            int pConstructor = aText.IndexOf(aDBT + "::" + aDBT);
            if (pConstructor == -1)
                return string.Empty;

            int pCurlyBracketClosed = aText.IndexOf("}", pConstructor);
            if (pCurlyBracketClosed == -1)
                return string.Empty;

            int pQuotationMarksOpened = aText.IndexOf("\"", pConstructor);
            if (pQuotationMarksOpened == -1 || pQuotationMarksOpened > pCurlyBracketClosed)
                return string.Empty;

            int pQuotationMarksClosed = aText.IndexOf("\"", pQuotationMarksOpened + 1);
            if (pQuotationMarksClosed == -1 || pQuotationMarksClosed > pCurlyBracketClosed)
                return string.Empty;

            string d = aText.Substring(pQuotationMarksOpened + 1, pQuotationMarksClosed - pQuotationMarksOpened - 1).Trim();
            mDBTNamespaces.Add(aDBT, d);
            return d;
        }

        #endregion

        #region controls
        public int GetControlsCount()
        {
            return mControls.Count;
        }

        public _CONTROL GetControlModule(int aIndex)
        {
            return mControls[aIndex];
        }
        #endregion.
        
        // SetRange SetCtrlSize SetCtrlMaxLen
        #region Set
        public void SetRange(string text ,out string minValue, out string maxValue)
        {
            minValue = "";
            maxValue = "";
            int tagComa,point;
            string func = "SetRange(";
            if (text.Contains(func))
            {
                point = text.IndexOf(func) + func.Length;
                if (text.Contains(","))
                {
                    minValue = text.Substring(point, (tagComa = text.IndexOf(",", point)) - point);
                    maxValue = text.Substring(tagComa+1, text.IndexOf(")", tagComa) - tagComa-1);                     
                }
                else
                    minValue = text.Substring(point, (tagComa = text.IndexOf(")", point)) - point-1);
            }

        }

        public void SetCtrlSize(string text, out string chars, out string rows)
        {
            chars = "";
            rows = "";
            int tagComa, point;
            string func = "SetCtrlSize(";
            if (text.Contains(func))
            {
                point = text.IndexOf(func) + func.Length;
                if (text.Contains(","))
                {
                    chars = text.Substring(point, (tagComa = text.IndexOf(",", point)) - point);
                    rows = text.Substring(tagComa + 1, text.IndexOf(")") - tagComa - 1);
                }
                else
                    chars = text.Substring(point, (tagComa = text.IndexOf(")", point)) - point - 1);
            }
        }
        public void SetCtrlMaxLen(string text, out string chars)
        {
            chars = "";
            int tagComa, point;
            string func = "SetCtrlSize(";
            if (text.Contains(func))
            {
                point = text.IndexOf(func) + func.Length;
                chars = text.Substring(point, (tagComa = text.IndexOf(")", point)) - point);
            }
        }


        #endregion

        #region utilitis
        public string findNameSpaceIDD(string className,string aText)
        {
            string nameSpace="";
            int init = aText.IndexOf(className + @"::" + className);
            string x = aText.Substring(init);
            if (init != -1)
            {
                init = x.IndexOf(@"_NS_TILEDLG");//, aText.IndexOf(cl._classidd + @"::" + cl._classidd)
                if (init != -1)
                {
                    int st = x.IndexOf("(\"", init) + 2;
                    int end = x.IndexOf("),", st) - 1;
                    nameSpace = x.Substring(st, end - st);
                }
            }
            return nameSpace;
        }
        protected string FindIDD(string className ,string aText, string macro = @"_NS_TILEDLG")
        {
            try
            {
                int start = aText.IndexOf(string.Format("{0}::{0}", className));
                start = aText.IndexOf(macro, start);
                start = aText.IndexOf("(\"", start);
                int end = aText.IndexOf("\")", start);
                if (start > end)
                    return "";
                macro =aText.Substring(start+2, end - start-2);
            }
            catch (Exception)
            {
                macro = "";
            }
            
            return macro;
        }
        #endregion
    }
}
