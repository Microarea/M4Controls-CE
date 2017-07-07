/*
M4-Controls CE - Tool di completamento delle descrizioni Json a partire dai source C++ 
Copyright (C) 2017 Microarea s.p.a.

This program is free software: you can redistribute it and/or modify it under the 
terms of the GNU General Public License as published by the Free Software Foundation, 
either version 3 of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful, 
but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE. 

See the GNU General Public License for more details.
*/

//using Finisar.Sql;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace M4ControlsDBMaker
{
    internal class TableM4Controls
    {
        private static string create =
        #region Query
            @"CREATE TABLE  dbo.[Controls] (
                                        [Module] VARCHAR(64) NOT NULL,
                                        [Filename] VARCHAR(256) NOT NULL,
                                        [Class] VARCHAR(64) NOT NULL,
                                        [IDC] VARCHAR(64) NOT NULL,
                                        [GenerateJson] BIT NULL,
                                        [ClassIDD] VARCHAR(64) NULL,
                                        [TileText] VARCHAR(64) NULL,
                                        [TileSize] VARCHAR(64) NULL,
                                        [TileStyle] INTEGER NULL,
                                        [BodyEditIDC] VARCHAR(64) NULL,
                                        [BodyEditText] VARCHAR(64) NULL,
                                        [Namespace] VARCHAR(64) NULL,
                                        [DBTPointer] VARCHAR(64) NULL,
                                        [DBTNamespace] VARCHAR(64) NULL,
                                        [RecordPointer] VARCHAR(64) NULL,
                                        [RecordClass] VARCHAR(64) NULL,
                                        [Field] VARCHAR(64) NULL,
                                        [FieldNamespace] VARCHAR(64) NULL,
                                        [Text] VARCHAR(64) NULL,
                                        [ComboType] VARCHAR(64) NULL,
                                        [RuntimeClass] VARCHAR(64) NULL,
                                        [HotKeyLink] VARCHAR(64) NULL,
                                        [Button] VARCHAR(64) NULL,
                                        [IsAddLink] BIT NULL,
                                        [Order] VARCHAR(64) NULL,
                                        [Hidden] BIT NULL,
                                        [NoChange_Grayed] BIT NULL,
                                        [Grayed]  BIT NULL,
                                        [MinValue]  VARCHAR(64) NULL,
                                        [MaxValue]  VARCHAR(64) NULL,
                                        [Chars]  VARCHAR(64) NULL,
                                        [Rows]  VARCHAR(64) NULL,
                                        PRIMARY KEY ([Module], [Filename], [Class], [IDC])
                                    );";
        #endregion
        private static string update =
        #region Query
            @"UPDATE[Controls]
                                            SET[GenerateJson] = @generatejson,
                                            [ClassIDD] = @classidd, 
                                            [TileText] = @tiletext,
                                            [TileSize] = @tilesize,
                                            [TileStyle] = @tilestyle,
                                            [BodyEditIDC] = @bodyeditidc,
                                            [BodyEditText] = @bodyedittext,                                                    
                                            [DBTPointer] = @dbtpointer, 
                                            [DBTNamespace] = @dbtnamespace,
                                            [Namespace] = @namespace,
                                            [RecordPointer] = @recordpointer,
                                            [RecordClass] = @recordclass,
                                            [Field] = @field, 
                                            [FieldNamespace] = @fieldnamespace,
                                            [Text] = @text,
                                            [ComboType] = @combotype,
                                            [RuntimeClass] = @runtimeclass,
                                            [HotKeyLink] = @hkl,
                                            [Button] = @button,
                                            [IsAddLink] = @isaddlink,
                                            [Order] = @Order,
                                            [Hidden] = @hidden,
                                            [NoChange_Grayed] = @noChange_Grayed,
                                            [Grayed] = @grayed,
                                            [MinValue] = @minValue,
                                            [MaxValue] = @maxValue,
                                            [Chars] = @chars,
                                            [Rows] = @rows                                            
                                            WHERE @module like[Module]
                                                    and @filename like [Filename]
                                                    and @class like [Class]
                                                    and @idc like [IDC];";
        #endregion
        private static string insert =
        #region Query
            @"INSERT INTO [Controls] (
						[Module],
                        [Filename],
						[Class],
						[IDC],
						[GenerateJson],
						[ClassIDD],
						[TileText],
						[TileSize],
						[TileStyle],
	                    [BodyEditIDC],
						[BodyEditText],
						[Namespace],
						[DBTPointer],
						[DBTNamespace],
						[RecordPointer],
						[RecordClass],
						[Field],
						[FieldNamespace],
						[Text],
						[ComboType],
						[RuntimeClass],
						[HotKeyLink],
						[Button],
						[IsAddLink],
						[order],
						[Hidden],
						[NoChange_Grayed],
						[Grayed],
                        [MinValue],
                        [MaxValue],
                        [Chars],
                        [Rows]
                ) VALUES ( 		
	                    @module,@filename,@class,@idc,@generatejson,@classidd,@tiletext,@tilesize,@tilestyle,@bodyeditidc,		
	                    @bodyedittext,@namespace,@dbtpointer,@dbtnamespace,@recordpointer,@recordclass,@field,@fieldnamespace,
	                    @text,@combotype,@runtimeclass,@hkl, @button, @isaddlink, @order, @hidden, @noChange_Grayed, @grayed,
                        @minValue,@maxValue,@chars,@rows)";
        #endregion
        
        public static int Update(_CONTROL cl)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            #region Assignment
            param.Add(new SqlParameter("module",cl._module.Trim()));
            param.Add(new SqlParameter("idc", cl._idc.Trim()));
            param.Add(new SqlParameter("filename", cl._filename.Trim()));
            param.Add(new SqlParameter("class", cl._class.Trim()));
            param.Add(new SqlParameter("classidd", !string.IsNullOrWhiteSpace(cl._classidd) ? cl._classidd.Trim() : ""));
            param.Add(new SqlParameter("tiletext", !string.IsNullOrWhiteSpace(cl._tiletext) ? cl._tiletext.Trim() : ""));
            param.Add(new SqlParameter("tilesize", !string.IsNullOrWhiteSpace(cl._tilesize) ? cl._tilesize.Trim() : ""));
            param.Add(new SqlParameter("bodyeditidc", !string.IsNullOrWhiteSpace(cl._bodyeditidc) ? cl._bodyeditidc.Trim() : ""));
            param.Add(new SqlParameter("bodyedittext", !string.IsNullOrWhiteSpace(cl._bodyedittext) ? cl._bodyedittext.Trim() : ""));
            param.Add(new SqlParameter("namespace", !string.IsNullOrWhiteSpace(cl._namespace) ? cl._namespace.Trim() : ""));
            param.Add(new SqlParameter("dbtpointer", !string.IsNullOrWhiteSpace(cl._dbtpointer) ? cl._dbtpointer.Trim() : ""));
            param.Add(new SqlParameter("dbtnamespace", !string.IsNullOrWhiteSpace(cl._dbtnamespace) ? cl._dbtnamespace.Trim() : ""));
            param.Add(new SqlParameter("recordpointer", !string.IsNullOrWhiteSpace(cl._recordpointer) ? cl._recordpointer.Trim() : ""));
            param.Add(new SqlParameter("recordclass", !string.IsNullOrWhiteSpace(cl._recordclass) ? cl._recordclass.Trim() : ""));
            param.Add(new SqlParameter("field", !string.IsNullOrWhiteSpace(cl._field) ? cl._field.Trim() : ""));
            param.Add(new SqlParameter("fieldnamespace", !string.IsNullOrWhiteSpace(cl._fieldnamespace) ? cl._fieldnamespace.Trim() : ""));
            param.Add(new SqlParameter("text", !string.IsNullOrWhiteSpace(cl._text) ? cl._text.Trim() : ""));
            param.Add(new SqlParameter("combotype", !string.IsNullOrWhiteSpace(cl._combotype) ? cl._combotype.Trim() : ""));
            param.Add(new SqlParameter("runtimeclass", !string.IsNullOrWhiteSpace(cl._runtimeclass) ? cl._runtimeclass.Trim() : ""));
            param.Add(new SqlParameter("hkl", !string.IsNullOrWhiteSpace(cl._hkl) ? cl._hkl.Trim() : ""));
            param.Add(new SqlParameter("button", !string.IsNullOrWhiteSpace(cl._button) ? cl._button.Trim() : ""));
            param.Add(new SqlParameter("generatejson", cl._generatejson ? 1 : 0));
            param.Add(new SqlParameter("tilestyle", cl._tilestyle));
            param.Add(new SqlParameter("isaddlink", cl._isaddlink ? 1 : 0));
            param.Add(new SqlParameter("order", !string.IsNullOrWhiteSpace(cl._order) ? cl._order.Trim() : ""));
            param.Add(new SqlParameter("hidden", cl._hidden ? 1 : 0));
            param.Add(new SqlParameter("noChange_Grayed", cl._noChange_Grayed ? 1 : 0));
            param.Add(new SqlParameter("grayed", cl._grayed ? 1 : 0));
            param.Add(new SqlParameter("minValue", !string.IsNullOrWhiteSpace(cl._minValue) ? cl._minValue.Trim() : ""));
            param.Add(new SqlParameter("maxValue", !string.IsNullOrWhiteSpace(cl._maxValue) ? cl._maxValue.Trim() : ""));
            param.Add(new SqlParameter("chars", !string.IsNullOrWhiteSpace(cl._chars) ? cl._chars.Trim() : ""));
            param.Add(new SqlParameter("rows", !string.IsNullOrWhiteSpace(cl._rows) ? cl._rows.Trim() : ""));
            #endregion

            System.Diagnostics.Debug.WriteLine(("CONTROLS TABLE UPDATE:"+ cl._module.Trim()) + "," +cl._filename.Trim() + ","+ cl._class.Trim() + ", " + cl._idc.Trim());

            return SQLServerManagement.ExecuteNonQuery(update, param);
        }
        
        public static bool Create()
        {
            return SQLServerManagement.ExecuteNonQuery(create) >= 0;
        }

        public static bool IsEmpty(string aModule, string aFilename = "")
        {
            bool e = true;
            List<SqlParameter> paramiters = new List<SqlParameter>();
            if (string.IsNullOrEmpty(aFilename))
            {
                
                paramiters.Add(new SqlParameter("Module", aModule));

                string query = "SELECT [IDC] FROM [Controls] WHERE [Module] = @Module";

                if (SQLServerManagement.ExecuteReader(query, paramiters) != null) 
                    e = !SQLServerManagement.Read();
                SQLServerManagement.ReaderClose();
            }
            else
            {
                paramiters.Add(new SqlParameter("Module", aModule));
                paramiters.Add(new SqlParameter("Filename", aFilename));

                string query = "SELECT [IDC] FROM [Controls] WHERE [Module] = @Module AND [Filename] = @Filename";

                if (SQLServerManagement.ExecuteReader(query, paramiters) != null)
                    e = !SQLServerManagement.Read();
                SQLServerManagement.ReaderClose();
            }
            return e;
        }

        public static List<_CONTROL> GetControls(string aModule = "", string aFilename = "")
        {
            List<_CONTROL> lc = new List<_CONTROL>();
            List<SqlParameter> param = new List<SqlParameter>();

            if (string.IsNullOrEmpty(aModule))
            {
                //string query = "SELECT * FROM [Controls] order by [order]";
                string query = "SELECT * FROM [Controls] order by [Filename], [order]";

                if (SQLServerManagement.ExecuteReader(query, null) != null)
                {
                    while (SQLServerManagement.Read())
                        lc.Add(GetValue());
                }
                SQLServerManagement.ReaderClose();
            }
            else if (string.IsNullOrEmpty(aFilename))
            {
                param.Add(new SqlParameter("ControlClass", aModule));
                param.Add(new SqlParameter("Filename", aFilename));

                string query = "SELECT * FROM [Controls] WHERE [TableName] = @module";

                if (SQLServerManagement.ExecuteReader(query, param) != null)
                {
                    while (SQLServerManagement.Read())
                        lc.Add(GetValue());
                }
                SQLServerManagement.ReaderClose();
            }
            else
            {                
                param.Add(new SqlParameter("ControlClass", aModule));
                param.Add(new SqlParameter("Filename", aFilename));

                string query = "SELECT * FROM [Controls] WHERE [TableName] = @ControlClass AND [FieldName] = @Filename";

                if (SQLServerManagement.ExecuteReader(query, param) != null)
                {
                    while (SQLServerManagement.Read())
                        lc.Add(GetValue());
                }
                SQLServerManagement.ReaderClose();
            }
            return lc;
        }

        public static _CONTROL GetValue()
        {
            _CONTROL cl = new _CONTROL();

            cl._module = SQLServerManagement.GetValue<string>("Module");
            cl._filename = SQLServerManagement.GetValue<string>("Filename");
            cl._class = SQLServerManagement.GetValue<string>("Class");
            cl._idc = SQLServerManagement.GetValue<string>("IDC");
            cl._generatejson = SQLServerManagement.GetValue<bool>("GenerateJson");
            cl._classidd = SQLServerManagement.GetValue<string>("ClassIDD");
            cl._tiletext = SQLServerManagement.GetValue<string>("TileText");
            cl._tilesize = SQLServerManagement.GetValue<string>("TileSize");
            cl._tilestyle = SQLServerManagement.GetValue<int>("TileStyle");
            cl._bodyeditidc = SQLServerManagement.GetValue<string>("BodyEditIDC");
            cl._bodyedittext = SQLServerManagement.GetValue<string>("BodyEditText");
            cl._namespace = SQLServerManagement.GetValue<string>("Namespace");
            cl._dbtpointer = SQLServerManagement.GetValue<string>("DBTPointer");
            cl._dbtnamespace = SQLServerManagement.GetValue<string>("DBTNamespace");
            cl._recordpointer = SQLServerManagement.GetValue<string>("RecordPointer");
            cl._recordclass = SQLServerManagement.GetValue<string>("RecordClass");
            cl._field = SQLServerManagement.GetValue<string>("Field");
            cl._fieldnamespace = SQLServerManagement.GetValue<string>("FieldNamespace");
            cl._text = SQLServerManagement.GetValue<string>("Text");
            cl._combotype = SQLServerManagement.GetValue<string>("ComboType");
            cl._runtimeclass = SQLServerManagement.GetValue<string>("RuntimeClass");
            cl._hkl = SQLServerManagement.GetValue<string>("HotKeyLink");
            cl._button = SQLServerManagement.GetValue<string>("Button");
            cl._isaddlink = SQLServerManagement.GetValue<bool>("IsAddLink");
            cl._order = SQLServerManagement.GetValue<string>("Order");
            cl._hidden = SQLServerManagement.GetValue<bool>("Hidden");
            cl._noChange_Grayed= SQLServerManagement.GetValue<bool>("NoChange_Grayed");
            cl._grayed = SQLServerManagement.GetValue<bool>("Grayed");
            cl._minValue = SQLServerManagement.GetValue<string> ("MinValue");
            cl._maxValue = SQLServerManagement.GetValue<string> ("MaxValue");
            cl._chars = SQLServerManagement.GetValue<string>    ("Chars");
            cl._rows = SQLServerManagement.GetValue<string>     ("Rows");

            return cl;
        }

        public static int Insert(_CONTROL cl)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            #region Assignment
            param.Add(new SqlParameter("module", cl._module.Trim()));
            param.Add(new SqlParameter("idc", cl._idc.Trim()));
            param.Add(new SqlParameter("filename", cl._filename.Trim()));
            param.Add(new SqlParameter("class", cl._class.Trim()));
            param.Add(new SqlParameter("classidd", !string.IsNullOrWhiteSpace(cl._classidd) ? cl._classidd.Trim() : ""));
            param.Add(new SqlParameter("tiletext", !string.IsNullOrWhiteSpace(cl._tiletext) ? cl._tiletext.Trim() : ""));
            param.Add(new SqlParameter("tilesize", !string.IsNullOrWhiteSpace(cl._tilesize) ? cl._tilesize.Trim() : ""));
            param.Add(new SqlParameter("bodyeditidc", !string.IsNullOrWhiteSpace(cl._bodyeditidc) ? cl._bodyeditidc.Trim() : ""));
            param.Add(new SqlParameter("bodyedittext", !string.IsNullOrWhiteSpace(cl._bodyedittext) ? cl._bodyedittext.Trim() : ""));
            param.Add(new SqlParameter("namespace", !string.IsNullOrWhiteSpace(cl._namespace) ? cl._namespace.Trim() : ""));
            param.Add(new SqlParameter("dbtpointer", !string.IsNullOrWhiteSpace(cl._dbtpointer) ? cl._dbtpointer.Trim() : ""));
            param.Add(new SqlParameter("dbtnamespace", !string.IsNullOrWhiteSpace(cl._dbtnamespace) ? cl._dbtnamespace.Trim() : ""));
            param.Add(new SqlParameter("recordpointer", !string.IsNullOrWhiteSpace(cl._recordpointer) ? cl._recordpointer.Trim() : ""));
            param.Add(new SqlParameter("recordclass", !string.IsNullOrWhiteSpace(cl._recordclass) ? cl._recordclass.Trim() : ""));
            param.Add(new SqlParameter("field", !string.IsNullOrWhiteSpace(cl._field) ? cl._field.Trim() : ""));
            param.Add(new SqlParameter("fieldnamespace", !string.IsNullOrWhiteSpace(cl._fieldnamespace) ? cl._fieldnamespace.Trim() : ""));
            param.Add(new SqlParameter("text", !string.IsNullOrWhiteSpace(cl._text) ? cl._text.Trim() : ""));
            param.Add(new SqlParameter("combotype", !string.IsNullOrWhiteSpace(cl._combotype) ? cl._combotype.Trim() : ""));
            param.Add(new SqlParameter("runtimeclass", !string.IsNullOrWhiteSpace(cl._runtimeclass) ? cl._runtimeclass.Trim() : ""));
            param.Add(new SqlParameter("hkl", !string.IsNullOrWhiteSpace(cl._hkl) ? cl._hkl.Trim() : ""));
            param.Add(new SqlParameter("button", !string.IsNullOrWhiteSpace(cl._button) ? cl._button.Trim() : ""));
            param.Add(new SqlParameter("generatejson", cl._generatejson ? 1 : 0));
            param.Add(new SqlParameter("tilestyle", cl._tilestyle));
            param.Add(new SqlParameter("isaddlink", cl._isaddlink ? 1 : 0));
            param.Add(new SqlParameter("order", !string.IsNullOrWhiteSpace(cl._order) ? cl._order.Trim() : ""));
            param.Add(new SqlParameter("hidden", cl._hidden ? 1 : 0));
            param.Add(new SqlParameter("noChange_Grayed", cl._noChange_Grayed ? 1 : 0));
            param.Add(new SqlParameter("grayed", cl._grayed ? 1 : 0));            
            param.Add(new SqlParameter("minValue", !string.IsNullOrWhiteSpace(cl._minValue) ? cl._minValue.Trim() : ""));
            param.Add(new SqlParameter("maxValue", !string.IsNullOrWhiteSpace(cl._maxValue) ? cl._maxValue.Trim() : ""));
            param.Add(new SqlParameter("chars", !string.IsNullOrWhiteSpace(cl._chars) ? cl._chars.Trim() : ""));
            param.Add(new SqlParameter("rows", !string.IsNullOrWhiteSpace(cl._rows) ? cl._rows.Trim() : ""));
            #endregion

            System.Diagnostics.Debug.WriteLine("CONTROLS TABLE: " + cl._module + " " + cl._idc + " " + cl._filename + " " + cl._class);
            string query = string.Format(insert);
            return SQLServerManagement.ExecuteNonQuery(query, param);
        }

        public static int Delete(string aModule, string aFilename ="")
        {
            List<SqlParameter> param = new List<SqlParameter>(); 
            if (string.IsNullOrEmpty(aFilename))
            {
                param.Add(new SqlParameter("module", aModule.Trim()));
                param.Add(new SqlParameter("filename", aFilename.Trim())); 
                string query = "DELETE FROM [Controls] WHERE [Module] = @module";
                return SQLServerManagement.ExecuteNonQuery(query, param);
            }
            else
            {
                param.Add(new SqlParameter("module", aModule.Trim()));
                param.Add(new SqlParameter("filename", aFilename.Trim()));
                string query = "DELETE FROM [Controls] WHERE [Module] = @module AND [Filename] = @filename";
                return SQLServerManagement.ExecuteNonQuery(query, param);
            }
        }
    }
}