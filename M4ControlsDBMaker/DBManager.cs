using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M4ControlsDBMaker
{
    public class DBManager
    {
        private string mDBName = string.Empty;
        public DBManager(string aDBName)
        {
            mDBName = aDBName;
        }

        public bool ExistDB()
        {
            return SQLServerManagement.ExistDB();
        }

        public bool Create()
        {
           return string.IsNullOrEmpty(mDBName) ? false : SQLServerManagement.CreateDB(mDBName);
        }

        public bool Delete(bool deleteFild )
        {
            return SQLServerManagement.ClearDB(deleteFild);
        }


        public bool Open()
        {
            return string.IsNullOrEmpty(mDBName) ? false : SQLServerManagement.OpenDB(mDBName);
        }

        // FIELDS

        public bool IsFieldsEmpty()
        {
            return string.IsNullOrEmpty(mDBName) ? true : TableM4Fields.IsEmpty();
        }

        public bool FieldsDelete()
        {
            return string.IsNullOrEmpty(mDBName) ? false : TableM4Fields.Delete() >= 0;
        }

        public bool FieldsInsert(string aTable, string aField, string aFieldNamespace)
        {
            return string.IsNullOrEmpty(mDBName) || string.IsNullOrEmpty(aTable) || string.IsNullOrEmpty(aField) || string.IsNullOrEmpty(aFieldNamespace) ? false : TableM4Fields.Insert(aTable, aField, aFieldNamespace) >= 0;
        }

        public string GetFieldNamespace(string aTable, string aField)
        {
            if (string.IsNullOrEmpty(mDBName) || string.IsNullOrEmpty(aTable) || string.IsNullOrEmpty(aField))
                return string.Empty;

            string n = TableM4Fields.GetNamespace(aTable, aField);
            if (!string.IsNullOrEmpty(n))
                return n;

            string t = string.Empty;
            int i = 0;
            while (string.IsNullOrEmpty(n) & i < 10)
            {
                t = GetParentTableName(aTable);
                if (string.IsNullOrEmpty(t))
                    return string.Empty;
                n = TableM4Fields.GetNamespace(t, aField);
                i++;
            }
            return n;
        }

        // TABLES

        public bool IsTablesEmpty()
        {
            return string.IsNullOrEmpty(mDBName) ? true : TableM4Tables.IsEmpty();
        }

        public bool TablesDelete()
        {
            return string.IsNullOrEmpty(mDBName) ? false : TableM4Tables.Delete() >= 0;
        }

        public bool TablesInsert(string aParentTable, string aTable)
        {
            return string.IsNullOrEmpty(mDBName) || string.IsNullOrEmpty(aParentTable) || string.IsNullOrEmpty(aTable) ? false : TableM4Tables.Insert(aParentTable, aTable) >= 0;
        }

        public string GetParentTableName(string aTable)
        {
            return string.IsNullOrEmpty(mDBName) || string.IsNullOrEmpty(aTable) ? string.Empty : TableM4Tables.GetParentTableName(aTable);
        }

        // CONTROLS CLASSES

        public bool IsControlsClassesEmpty()
        {
            return string.IsNullOrEmpty(mDBName) ? true : TableM4ControlsClasses.IsEmpty();
        }

        public bool ControlsClassesDelete()
        {
            return string.IsNullOrEmpty(mDBName) ? false : TableM4ControlsClasses.Delete() >= 0;
        }

        public bool ControlsClassesInsert(string aJsonName, string aControlClass)
        {
            return string.IsNullOrEmpty(mDBName) || string.IsNullOrEmpty(aJsonName) || string.IsNullOrEmpty(aControlClass) ? false : TableM4ControlsClasses.Insert(aJsonName, aControlClass) >= 0;
        }

        public string GetJsonName(string aControlClass)
        {
            return string.IsNullOrEmpty(mDBName) || string.IsNullOrEmpty(aControlClass) ? string.Empty : TableM4ControlsClasses.GetControlJsonName(aControlClass);
        }

        // CONTROLS

        public bool  UpdateControl(_CONTROL control)
        {
            
            if (string.IsNullOrEmpty(mDBName))
                return false;
            TableM4Controls.Update(control);
            return true;
        }

        public bool IsControlsEmpty(string aModule, string aFilename = "")
        {
            return string.IsNullOrEmpty(mDBName) || string.IsNullOrEmpty(aModule) ? true : TableM4Controls.IsEmpty(aModule, aFilename);
        }

        public bool ControlsDelete(string aModule, string aFilename = "")
        {
            TableM4Label.Delete();
            return string.IsNullOrEmpty(mDBName) || string.IsNullOrEmpty(aModule) ? false : TableM4Controls.Delete(aModule, aFilename) >= 0;
        }

        public bool ControlsInsert(_CONTROL aControl)
        {
            return string.IsNullOrEmpty(mDBName) ? false : TableM4Controls.Insert(aControl) >= 0;
        }

        public List<_CONTROL> GetControls(string aModule = "", string aFilename = "")
        {
            return string.IsNullOrEmpty(mDBName) ? null : TableM4Controls.GetControls(aModule, aFilename);
        }

        public void LabelInsert(string cl)
        {
            TableM4Label.Insert(cl);
        }

        public bool LabelExist(string idc)
        {
            return TableM4Label.exist(idc);
        }

    }
}
