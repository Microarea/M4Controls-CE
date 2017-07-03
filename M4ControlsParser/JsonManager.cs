using M4ControlsDBMaker;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M4ControlsParser
{
    public class JsonManager
    {
        public IEnumerable<JToken> mItemObjects = null; 
        private JObject mTokenJson = null;
        private JArray mColumnsArray = null;
        private DBManager mDB = null;
        private List<string> mErrors = new List<string>();

        public JsonManager(DBManager aDB)
        {
            mDB = aDB;
        }

        public bool LoadJson(string aIDD, string aNamespace, string aTileSize, int aTileStyle, string aTileText, string aSourceFilename, out string aJsonFilename)
        {
            mErrors.Clear();
            string aRootFolder = Path.GetDirectoryName(aSourceFilename);

            // Se non esiste nella cartella del file la cartella ModuleObjects va ancora su di un livello
            // (dovrebbe essre la situazione standard, vedi Mago)
            if (!Directory.Exists(Path.Combine(aRootFolder, "ModuleObjects")))
                aRootFolder = Path.GetDirectoryName(aRootFolder);

            string aFileName = aIDD + ".tbjson";

            aJsonFilename = SearchJsonFile(aRootFolder, aFileName);
            if (string.IsNullOrEmpty(aJsonFilename))
                return false;

            string j = File.ReadAllText(aJsonFilename);
            if (string.IsNullOrEmpty(j))
                return false;

            mTokenJson = JObject.Parse(j);

            SetProperty("name", aNamespace);
            SetProperty("size", GetTitleSize(aTileSize));
            SetProperty("text", aTileText/*Format(aTileText)*/);
            mTokenJson.Remove("rcId");
            if (aTileStyle > 0)
                SetProperty("tileStyle", aTileStyle);
            SetProperty("type", "Tile");


            mItemObjects = AllChildren(mTokenJson)
                        .First(c => c.Type == JTokenType.Array && c.Path.Contains("items"))
                        .Children<JObject>();
            SetLabel();
            return true;
        }

        public void SetLabel()
        {

            foreach(JToken token in mItemObjects)
            {
                if (mDB.LabelExist(token["id"].ToString()))
                {
                    token["controlClass"] = "LabelStatic";
                    token["linePosition"] = 1;
                }
            }
        }

        public string GetJson()
        {
            return mTokenJson.ToString();
        }

        public List<string> GetErrors()
        {
            return mErrors;
        }

        public bool CreateAddLinkBinding(string aIDC, string aNameSpace, string aFieldNameSpace, string aRuntimeClass, string aDBTNamespace, string aMinValue, string aMaxValue, string aChars, string aRows, string aHotlink = "", string aButton = "")
        {
            if (string.IsNullOrEmpty(aIDC))
                return false;

            string aDataSource = "";

            if (!string.IsNullOrEmpty(aDBTNamespace) && !string.IsNullOrEmpty(aFieldNameSpace))
                aDataSource = aDBTNamespace + "." + aFieldNameSpace;
            else if (!string.IsNullOrEmpty(aFieldNameSpace))
                aDataSource = aFieldNameSpace;
            else
                aDataSource = aDBTNamespace;

            if (aDataSource == ".")
                return false;

            if (string.IsNullOrEmpty(aDataSource))
                if(aRuntimeClass != "LabelStatic")
                    aDataSource = "**PLACE HOLDER**";

            string aControlClass = GetControlClass(aRuntimeClass);

            if (string.IsNullOrEmpty(aControlClass))
                mErrors.Add(aIDC + "\t" + aNameSpace + "\t" + aFieldNameSpace + "\t-->\tControl class " + aRuntimeClass + " not found");

            int aButtonId = 1;
            if (!string.IsNullOrEmpty(aButton))
                aButtonId = GetButtonId(aButton);
            if (aButtonId == -1)
                mErrors.Add(aIDC + "\t" + aNameSpace + "\t" + aFieldNameSpace + "\t-->\tButton " + aButton + " not found");
            
            foreach (JObject item in mItemObjects)
            {
                #region particolarità
                int var;
                JObject Const;
                if (item["id"].ToString() == aIDC)
                {
                    if (int.TryParse(aMinValue, out var))
                        mTokenJson["minValue"] = var;
                    else if (!string.IsNullOrWhiteSpace(aMinValue))
                    {
                        Const = new JObject();
                        Const["const"] = aMinValue;
                        item["minValue"] = Const;
                    }
                    if (int.TryParse(aMaxValue, out var))
                        item["maxValue"] = var;
                    else if (!string.IsNullOrWhiteSpace(aMinValue))
                    {
                        Const = new JObject();
                        Const["const"] = aMaxValue;
                        item["maxValue"] = Const;
                    }
                    if (int.TryParse(aChars, out var))
                        item["chars"] = var;
                    else if (!string.IsNullOrWhiteSpace(aChars))
                    {
                        Const = new JObject();
                        Const["const"] = aChars;
                        item["chars"] = Const;
                    }
                    if (int.TryParse(aRows, out var))
                        item["minValue"] = var;
                    else if (!string.IsNullOrWhiteSpace(aRows))
                    {
                        Const = new JObject();
                        Const["const"] = aMinValue;
                        item["minValue"] = Const;
                    }
                }
                #endregion

                foreach (JProperty property in item.Properties())
                {
                    if (property.Value.ToString() == aIDC)
                    {
                        JObject jo = new JObject();
                        item["controlClass"] = aControlClass;
                        if (aControlClass== "LabelStatic")
                        {
                            item["linePosition"] = 1;//top
                        }
                        else
                        {
                            jo["datasource"] = aDataSource;
                            if (!string.IsNullOrEmpty(aHotlink))
                                jo["hotLink"] = aHotlink;
                            if (aButtonId == 0)
                                jo["buttonId"] = aButtonId;
                            item["binding"] = jo;
                        }

                        string type = item["type"].ToString();

                        if (type ==  "10" && aControlClass == "CheckBox")
                        {
                            item["controlClass"] = "RadioButton";
                        }

                        return true;
                    }
                }
            }
            return false;
        }

        public bool CreateBodyEditBinding(string aIDC, string aDBTNamespace, string aBodyEditText)
        {
            if (string.IsNullOrEmpty(aIDC) || string.IsNullOrEmpty(aDBTNamespace))
                return false;

            foreach (JObject item in mItemObjects)
            {
                foreach (JProperty property in item.Properties())
                {
                    if (property.Value.ToString() == aIDC)
                    {
                        item["controlClass"] = "BodyEdit";
                        item["name"] = aBodyEditText;
                        item["type"] = "BodyEdit";

                        JObject jo = new JObject();
                        jo["datasource"] = aDBTNamespace;
                        item["binding"] = jo;

                        mColumnsArray = new JArray();
                        item["items"] = mColumnsArray;

                        return true;
                    }
                }
            }
            return false;
        }

        public bool CreateAddColumn(string aIDC, string aNameSpace, string aFieldNameSpace, string aRuntimeClass, string aComboType, string aText, bool bHidden, bool bGrayed, bool bNoChange_Grayed, string aMinValue, string aMaxValue, string aChars, string aRows, string aHotlink = "", string aButton = "")
        {
            if (mColumnsArray == null)
                return false;

            if (string.IsNullOrEmpty(aIDC) || string.IsNullOrEmpty(aFieldNameSpace))
                return false;

            int aButtonId = 1;
            if (!string.IsNullOrEmpty(aButton))
                aButtonId = GetButtonId(aButton);
            if (aButtonId == -1)
                mErrors.Add(aIDC + "\t" + aNameSpace + "\t" + aFieldNameSpace + "\t-->\tButton " + aButton + " not found");

            string aControlClass = GetControlClass(aRuntimeClass);
            if (string.IsNullOrEmpty(aControlClass))
                mErrors.Add(aIDC + "\t" + aNameSpace + "\t" + aFieldNameSpace + "\t-->\tControl class " + aRuntimeClass + " not found");

            int aCombo = 0;
            if (!string.IsNullOrEmpty(aComboType))
                aCombo = GetComboType(aComboType);
            if (aCombo == -1)
                mErrors.Add(aIDC + "\t" + aNameSpace + "\t" + aFieldNameSpace + "\t-->\tCombo type " + aComboType + " not found");

            JObject jo = new JObject();
            string aOkText = aText;
            aOkText = aOkText.Replace("\\n", "\n");//toglie doppio backslash
            
            jo["type"] = "ColTitle";
            jo["controlClass"] = aControlClass;
            jo["id"] = aIDC;
            jo["text"] = aOkText;
            jo["name"] = aNameSpace;
            if (bHidden == true)
                jo["hidden"] = bHidden;
            if (bGrayed == true)
                jo["grayed"] = bGrayed;
            if (bNoChange_Grayed == true)
                jo["noChangeGrayed"] = bNoChange_Grayed;
            if (aCombo > 0)
                jo["comboType"] = aCombo;

            #region particolarità
            int var;
            JObject Const;
            if (jo["id"].ToString() == aIDC)
            {
                if (int.TryParse(aMinValue, out var))
                    mTokenJson["minValue"] = var;
                else if (!string.IsNullOrWhiteSpace(aMinValue))
                {
                    Const = new JObject();
                    Const["const"] = aMinValue;
                    jo["minValue"] = Const;
                }
                if (int.TryParse(aMaxValue, out var))
                    jo["maxValue"] = var;
                else if (!string.IsNullOrWhiteSpace(aMinValue))
                {
                    Const = new JObject();
                    Const["const"] = aMaxValue;
                    jo["maxValue"] = Const;
                }
                if (int.TryParse(aChars, out var))
                    jo["chars"] = var;
                else if (!string.IsNullOrWhiteSpace(aChars))
                {
                    Const = new JObject();
                    Const["const"] = aChars;
                    jo["chars"] = Const;
                }
                if (int.TryParse(aRows, out var))
                    jo["minValue"] = var;
                else if (!string.IsNullOrWhiteSpace(aRows))
                {
                    Const = new JObject();
                    Const["const"] = aMinValue;
                    jo["minValue"] = Const;
                }
            }
            #endregion

            JObject jods = new JObject();
            jods["datasource"] = aFieldNameSpace;
            if (!string.IsNullOrEmpty(aHotlink))
                jods["hotLink"] = aHotlink;
            if (aButtonId == 0)
                jods["buttonId"] = aButtonId;
            jo["binding"] = jods;

            

            mColumnsArray.Add(jo);
            return true;
        }

        private int GetButtonId(string aButton)
        {
            switch (aButton)
            {
                case "NO_BUTTON":
                    return 0;
                case "BTN_DEFAULT":
                    return 1;
                default:
                    return -1;
            }
        }

        private int GetTitleSize(string aTileSize)
        {
            switch (aTileSize)
            {
                case "TILE_MICRO":
                    return 0;
                case "TILE_MINI":
                    return 1;
                case "TILE_LARGE":
                    return 3;
                case "TILE_WIDE":
                    return 4;
                case "TILE_AUTOFILL":
                    return 5;
                case "TILE_P":
                    return 5;
                default: // TILE_STANDARD
                    return 2;
            }
        }

        private int GetComboType(string aComboType)
        {
            switch (aComboType)
            {
                case "CBS_DROPDOWN":
                    return 1;
                case "CBS_DROPDOWNLIST":
                    return 2;
                default: 
                    return -1;
            }
        }

        private string GetControlClass(string aRuntimeClass)
        {
            switch (aRuntimeClass)
            {
                // String
                case "CStrEdit":
                    return "StringEdit";
                case "CAddressEdit":
                    return "AddressEdit";
                case "CBrowsePathEdit":
                    return "BrowsePathEdit";
                case "CNamespaceEdit":
                    return "NamespaceEdit";
                case "CLinkEdit":
                    return "LinkEdit";
                case "CPhoneEdit":
                    return "PhoneEdit";
                case "CEmailAddressEdit":
                    return "EmailAddressEdit";
                case "CIdentifierEdit":
                    return "IdentifierEdit";
                case "CShowFileTextStatic":
                    return "FileTextStatic";
                case "CStrStatic":
                    return "StringStatic";
                case "CLabelStatic":
                    return "LabelStatic";
                case "CPictureStatic":
                    return "PictureStatic";
                case "CNSBitmap":
                    return "NamespaceBitmap";
                case "CParsedWebCtrl":
                    return "WebControl";
                case "CIdentifierCombo":
                    return "IdentifierCombo";
                case "CXmlCombo":
                    return "XmlCombo";
                case "CStrCombo":
                    return "StringComboDropDown";
                case "CStrListBox":
                    return "StringListBox";
                case "CRadioCombo":
                    return "RadioCombo";

                // Integer
                case "CIntEdit":
                    return "IntegerEdit";
                case "CIntStatic":
                    return "IntegerStatic";
                case "CIntCombo":
                    return "IntegerComboDropDown";
                case "CIntListBox":
                    return "IntListBox";

                // Long
                case "CLongEdit":
                    return "LongEdit";
                case "CLongCombo":
                    return "LongComboDropDown";
                case "CLongStatic":
                    return "LongStatic";
                case "CLongListBox":
                    return "LongListBox";

                // Date
                case "CDateEdit":
                    return "DateEdit";
                case "CDateSpinEdit":
                    return "DateSpinEdit";
                case "CDateCombo":
                    return "DateComboDropDown";
                case "CDateStatic":
                    return "DateStatic";

                // ElapsedTime
                case "CElapsedTimeEdit":
                    return "ElapsedTimeEdit";
                case "CElapsedTimeStatic":
                    return "ElapsedTimeStatic";

                // Double
                case "CDoubleEdit":
                    return "DoubleEdit";
                case "CDoubleCombo":
                    return "DoubleComboDropDown";
                case "CDoubleStatic":
                    return "DoubleStatic";
                case "CDoubleListBox":
                    return "DoubleListBox";
                case "CTBLinearGaugeCtrl":
                    return "TBLinearGaugeCtrl";
                case "CTBCircularGaugeCtrl":
                    return "TBCircularGaugeCtrl";

                // Money
                case "CMoneyEdit":
                    return "MoneyEdit";
                case "CMoneyCombo":
                    return "MoneyComboDropDown";
                case "CMoneyStatic":
                    return "MoneyStatic";

                // Quantity
                case "CQuantityEdit":
                    return "QuantityEdit";
                case "CQuantityCombo":
                    return "QuantityComboDropDown";
                case "CQuantityStatic":
                    return "QuantityStatic";

                // DateTime
                case "CDateTimeEdit":
                    return "DateTimeEdit";
                case "CDateTimeStatic":
                    return "DateTimeStatic";
                case "CDateListBox":
                    return "DateListBox";

                // Time
                case "CTimeEdit":
                    return "TimeEdit";
                case "CTimeStatic":
                    return "TimeStatic";

                // Percent
                case "CPercEdit":
                    return "PercentEdit";
                case "CPercCombo":
                    return "PercentComboDropDown";
                case "CPercStatic":
                    return "PercentStatic";

                // Bool
                case "CBoolEdit":
                    return "BoolEdit";
                case "CPushButton":
                    return "Button";
                case "CBoolButton":
                    return "CheckBox";
                case "CBoolButtonStatic":
                    return "CheckBoxStatic";
                case "CBoolCombo":
                    return "BoolCombo";
                case "CBoolStatic":
                    return "BoolStatic";
                case "CBoolListBox":
                    return "BoolListBox";

                // Guid
                case "CGuidEdit":
                    return "UUidEdit";
                case "CGuidStatic":
                    return "UUidStatic";

                // Enum
                case "CEnumCombo":
                    return "EnumCombo";
                case "CEnumStatic":
                    return "EnumStatic";
                case "CEnumListBox":
                    return "EnumListBox";
                case "CEnumButton":
                    return "EnumButton";

                // Text
                case "CTextEdit":
                    return "TextEdit";
                case "CTextStatic":
                    return "TextStatic";

                // Null
                case "CStatic":
                    return "Label";
                case "CTreeViewAdvCtrl":
                    return "TreeView";
                case "CTBPropertyGrid":
                    return "PropertyGrid";
                case "CHeaderStrip":
                    return "HeaderStrip";
                default:
                {
                    string n = mDB.GetJsonName(aRuntimeClass);
                    if (string.IsNullOrEmpty(n))
                        return "*** NOT FOUND ***";
                    else
                        return n;
                }
            }
        }


        private void SetProperty(string aTag, string aValue)
        {
            if (string.IsNullOrEmpty(aValue))
                return;

            var t = mTokenJson[aTag];
            if (t == null)
                mTokenJson.Add(aTag, aValue.Trim());
            else
                mTokenJson[aTag] = aValue.Trim();
        }

        private void SetProperty(string aTag, int aValue)
        {
            var t = mTokenJson[aTag];
            if (t == null)
                mTokenJson.Add(aTag, aValue);
            else
                mTokenJson[aTag] = aValue;
        }

        private string Format(string aText)
        {
            string s = string.Empty;
            foreach (char c in aText)
            {
                if (c == Char.ToUpper(c))
                    s += " " + c;
                else
                    s += c;
            }
            return s;
        }

        private string SearchJsonFile(string aRootFolder, string aFileName)
        {
            string r = Path.Combine(aRootFolder, "ModuleObjects");
            string jsf = string.Empty;
            string f = string.Empty;

            foreach (string d in Directory.GetDirectories(r))
            {
                jsf = Path.Combine(d, "JsonForms");
                f = Path.Combine(jsf, aFileName);
                if (File.Exists(f))
                    return f;
            }

            r = Path.Combine(aRootFolder, "JsonForms");

            foreach (string d in Directory.GetDirectories(r))
            {
                f = Path.Combine(d, aFileName);
                if (File.Exists(f))
                    return f;
            }
            return string.Empty;
        }

        private static IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;
                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }
    }
}

