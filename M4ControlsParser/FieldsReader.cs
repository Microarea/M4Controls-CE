using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M4ControlsDBMaker;

namespace M4ControlsParser
{
   public  class FieldsReader
    {
        private string mERP = string.Empty;
        private CommonFunctions cf = new CommonFunctions();
        private Hashtable mFieldNamespaces = new Hashtable();
        private Hashtable mParentTables = new Hashtable();
        private DBManager mDB = null;

        private static string VOID = "void";
        private static string BEGIN_BIND_DATA = "BEGIN_BIND_DATA";
        private static string END_BIND_DATA = "END_BIND_DATA";
        private static string AFXISACTIVATED = "AfxIsActivated";
        private static string IMPLEMENT_DYNCREATE = "IMPLEMENT_DYNCREATE";

        public FieldsReader(DBManager aDB, string aERP)
        {
            mDB = aDB;
            mERP = aERP;
        }

        public void Search()
        {
            if (string.IsNullOrEmpty(mERP))
                return;

            mFieldNamespaces.Clear();
            mParentTables.Clear();
            mDB.FieldsDelete();

            foreach (string m in Directory.GetDirectories(mERP))
            {
                foreach (string d in Directory.GetDirectories(m))
                {
                    foreach (string f in Directory.GetFiles(d, "*.cpp"))
                        LoadFields(File.ReadAllText(f, Encoding.Default));
                }
            }
        }

        private void LoadFields(string aText)
        {
            string t = cf.Clean(aText);

            int pBeginBindData = t.IndexOf(BEGIN_BIND_DATA);
            if (pBeginBindData == -1)
                return;
            int pEndBindData = t.IndexOf(END_BIND_DATA, pBeginBindData);
            if (pEndBindData == -1)
                return;

            int pSeparator = t.IndexOf("++", pBeginBindData);
            if (pSeparator != -1 && pSeparator < pEndBindData)
                pEndBindData = pSeparator;

            while (pBeginBindData != -1 && pEndBindData != -1 && pBeginBindData < pEndBindData)
            {
                LoadFields(t, pBeginBindData, pEndBindData);
                pBeginBindData = t.IndexOf(BEGIN_BIND_DATA, pEndBindData);
                if (pBeginBindData > -1)
                {
                    pEndBindData = t.IndexOf(END_BIND_DATA, pBeginBindData);
                    pSeparator = t.IndexOf("++", pBeginBindData);
                    if (pSeparator != -1 && pSeparator < pEndBindData)
                        pEndBindData = pSeparator;
                }
            }
        }

        private void LoadFields(string aText, int pBegin, int pEnd)
        {
            int pVoid = aText.LastIndexOf(VOID, pBegin);
            if (pVoid == -1)
                return;

            int pSeparator = aText.IndexOf("::", pVoid);
            if (pSeparator == -1 || pSeparator > pBegin)
                return;

            string aTable = aText.Substring(pVoid + 4, pSeparator - pVoid - 4).Trim();

            int pOpenBracket = -1;
            int pCloseBracket = -1;
            int pComma = -1;
            string aParentTable = string.Empty;
            string aCheckTable = string.Empty;

            int pImplmenentDyncreate = aText.LastIndexOf(IMPLEMENT_DYNCREATE, pVoid);

            if (pImplmenentDyncreate != -1)
            {
                pOpenBracket = aText.IndexOf("(", pImplmenentDyncreate);
                if (pOpenBracket != -1 && pOpenBracket < pVoid)
                {
                    pComma = aText.IndexOf(",", pOpenBracket);
                    if (pComma != -1 && pOpenBracket < pVoid)
                    {
                        pCloseBracket = aText.IndexOf(")", pOpenBracket);
                        if (pCloseBracket != -1 && pCloseBracket < pVoid)
                        {
                            aCheckTable = aText.Substring(pOpenBracket + 1, pComma - pOpenBracket - 1).Trim();
                            if (aCheckTable == aTable)
                            {
                                aParentTable = aText.Substring(pComma + 1, pCloseBracket - pComma - 1).Trim();
                                AddParentTable(aParentTable, aTable);
                            }
                        }
                    }
                }
            }

            string aField = string.Empty;
            string aFieldNamespace = string.Empty;

            pComma = -1;
            pCloseBracket = -1;

            int pOpenQuotationMarks = aText.IndexOf("\"", pBegin);
            int pCloseQuotationMarks = aText.IndexOf("\"", pOpenQuotationMarks + 1);
            int pLastCloseQuotationMarks = pCloseQuotationMarks;
            int pAfxIsActivated = -1;
            int pTemp = -1;

            while (pOpenQuotationMarks != -1 && pCloseQuotationMarks != -1 && pOpenQuotationMarks < pEnd)
            {
                pAfxIsActivated = aText.LastIndexOf(AFXISACTIVATED, pOpenQuotationMarks);

                if (pAfxIsActivated != -1 && pAfxIsActivated > pLastCloseQuotationMarks)
                {
                    pOpenQuotationMarks = aText.IndexOf("\"", pCloseQuotationMarks + 1);
                    pCloseQuotationMarks = aText.IndexOf("\"", pOpenQuotationMarks + 1);
                }

                aFieldNamespace = aText.Substring(pOpenQuotationMarks + 1, pCloseQuotationMarks - pOpenQuotationMarks - 1);
                pComma = aText.IndexOf(",", pCloseQuotationMarks);
                if (pComma > -1)
                {
                    pCloseBracket = aText.IndexOf(")", pComma);
                    if (pComma > -1)
                    {
                        pTemp = aText.IndexOf(",", pComma + 1);
                        if (pTemp != -1 && pTemp < pCloseBracket)
                            pCloseBracket = pTemp;

                        AddField(aTable, aText.Substring(pComma + 1, pCloseBracket - pComma - 1), aFieldNamespace);
                    }
                }
                pLastCloseQuotationMarks = pCloseQuotationMarks;
                pOpenQuotationMarks = aText.IndexOf("\"", pCloseQuotationMarks + 1);
                pCloseQuotationMarks = aText.IndexOf("\"", pOpenQuotationMarks + 1);
            }
        }

        private void AddField(string aTable, string aField, string aFieldNamespace)
        {
            if (mDB == null)
                return;

            System.Diagnostics.Debug.WriteLine("\tFIELD:\t" + aTable + "\t" + aField + "\t" + aFieldNamespace);

            if (!mFieldNamespaces.Contains(aTable + "::" + aField))
            {
                mFieldNamespaces.Add(aTable + "::" + aField, aFieldNamespace);
                mDB.FieldsInsert(aTable, aField, aFieldNamespace);
            }
        }

        private void AddParentTable(string aParentTable, string aTable)
        {
            if (mDB == null)
                return;

            if (aParentTable == "SqlRecord" || aParentTable == "SqlRecordSlavable" || aParentTable == "SqlVirtualRecord")
                return;

            System.Diagnostics.Debug.WriteLine("\tPARENT TABLE:\t" + aParentTable + "\t" + aTable);

            if (!mParentTables.Contains(aParentTable + "::" + aTable))
            {
                mParentTables.Add(aParentTable + "::" + aTable, string.Empty);
                mDB.TablesInsert(aParentTable, aTable);
            }
        }
    }
}
