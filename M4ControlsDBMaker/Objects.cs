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

namespace M4ControlsDBMaker
{
    public class _CLASS
    {
        public string _module;
        public string _filename;
        public string _name;
        public string _idd;
        public string _idc;
        public string _text;
        public string _containerclass;
        public string _tiletext;
        public string _tilesize;
        public int _tilestyle;
        public int _start;
        public int _end;
        public bool _isaddlink;
    }
    public class _CONTROL
    {
        public bool _generatejson;
        public string _module;
        public string _filename;
        public string _classidd;
        public string _class;
        public string _tiletext;
        public string _tilesize;
        public int _tilestyle;
        public string _bodyeditidc;
        public string _bodyedittext;
        public string _namespace;
        public string _dbtpointer;
        public string _dbtnamespace;
        public string _recordpointer;
        public string _recordclass;
        public string _field;
        public string _fieldnamespace;
        public string _text;
        public string _combotype;
        public string _idc;
        public string _runtimeclass;
        public string _hkl;
        public string _button;
        public bool _isaddlink;

        public string _order;
        public bool _hidden;
        public bool _noChange_Grayed;
        public bool _grayed;

        public string _minValue;
        public string _maxValue;
        public string _chars;
        public string _rows;
    }

    public class _FILD
    {
        public string _TableName;
        public string _FieldName;
        public string _FieldNamespace;
    }

}