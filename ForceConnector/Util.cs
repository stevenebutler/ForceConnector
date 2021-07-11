using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    static class Util
    {
        public static bool checkSession()
        {
            if (!string.IsNullOrEmpty(ThisAddIn.accessToken) && !string.IsNullOrEmpty(ThisAddIn.id))
            {
                try
                {
                    //var result = RESTAPI.getConnectionInfo();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }

        public static bool IsRequired(RESTful.Field fld)
        {
            return fld.name != "Id" && !fld.nillable && !fld.defaultedOnCreate && fld.createable;
        }

        public static bool IsNameField(RESTful.Field fld)
        {
            bool IsNameFieldRet = default;
            IsNameFieldRet = fld.nameField && !IsRequired(fld) && !fld.custom && fld.updateable;
            return IsNameFieldRet;
        }

        public static bool IsStandard(RESTful.Field fld)
        {
            bool IsStandardRet = default;
            IsStandardRet = !IsNameField(fld) && !IsRequired(fld) && !fld.custom && fld.updateable;
            return IsStandardRet;
        }

        public static bool IsCustom(RESTful.Field fld)
        {
            bool IsCustomRet = default;
            IsCustomRet = fld.custom && fld.updateable && !IsRequired(fld);
            return IsCustomRet;
        }

        public static bool IsReadOnly(RESTful.Field fld)
        {
            bool IsReadOnlyRet = default;
            IsReadOnlyRet = fld.name != "Id" && !fld.updateable && !IsRequired(fld);
            return IsReadOnlyRet;
        }

        public static bool IsHyperlink(RESTful.Field fld, string val)
        {
            if (fld.type != "string")
                return false;
            if (Strings.Left(val, 5) == "_HL1_")
                return true;
            return false;
        }

        public static bool IsObject(IDictionary obj)
        {
            return obj.Contains("attributes");
        }

        public static void AddHyperlink(Range cel, object val) // trim down the value
        {
            string link = Strings.Right(Conversions.ToString(val), Strings.Len(val) - 5);
            link = Strings.Left(link, Strings.Len(link) - 5);
            int p = Strings.InStr(link, "_HL2_");
            if (p > 0)
            {
                cel.Value = Strings.Mid(link, p + 5);
                ForceConnector.worksheet.Hyperlinks.Add(cel, Strings.Left(link, p - 1));
            }
        }
        // 
        public static void displayUserName(string uname)
        {
            Globals.Ribbons.ForceRibbon.ribbonForceConnector.Label = ThisAddIn.ribbonBoxName + " (" + uname + ")";
        }

        public static void ErrorBox(string msg)
        {
            TopMostMessageBox.Show("Error", msg, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // 
        // we may need to trim the range down to valid ID's ?
        // 
        // trim top and bottom of this range to try to capture the region of valid
        // object id's
        // if we are given a range like "A:A" we can be smart by removing the
        // top invalid items and triming the blank cells at the tail of the range
        public static Range build_ref_range(string str)
        {
            try
            {
                Range r = ForceConnector.worksheet.get_Range(str); // if this is not a valid range description send a msg
                Range t = null;
                foreach (Range c in r)
                {
                    if (c is null || Information.IsError(c))
                    {
                        return null;
                    }
                    switch (Strings.Len(c.get_Value()))
                    {
                        case 15:
                        case 18:
                            {
                                // sometimes a text string like 'opportunity id' will be just
                                // 15 or 18 long, to avoid adding this, check that the string we
                                // are looking at has some numeric chars and is not all alpha.
                                if (Conversions.ToBoolean(LikeOperator.LikeObject(c.get_Value(), "*[0-9][0-9]*", CompareMethod.Binary)))    // two adjacent numbers
                                {
                                    if (t == null)
                                        t = c; // special case first time thru
                                    t = (Range)ForceConnector.excelApp.Union(t, c); // normal case, extend the range down
                                }

                                break;
                            }
                    }
                }

                // check that the range is made of one area...
                if (t.Areas.Count > 1)
                {
                    Interaction.MsgBox("Range " + t.get_Address(1) + " is made of more than one area");
                }
                return t;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // 
        // slightly different than above for query strings
        // SFDC_escape_q
        public static string escapeQueryString(string s)
        {
            int InI;
            // "&|!()[]^""~*?:'" should really deal with all of these, just lazy i guess
            var loopTo = Strings.Len(s);
            for (InI = 1; InI <= loopTo; InI += 1)
            {
                // Debug.Print Mid(s, InI, 1): Debug.Print Asc(Mid(s, InI, 1))
                switch (Strings.Asc(Strings.Mid(s, InI, 1)))
                {
                    case 39: // this is the tick ->'<-
                        {
                            s = Strings.Left(s, InI - 1) + '\\' + '\'' + Strings.Right(s, Strings.Len(s) - InI);
                            InI = InI + 1;
                            break;
                        }
                }
            }

            return Strings.Trim(s);
        }

        // 
        // adjust the format of the value for types as expected by API
        // sfQueryValueFormat
        public static string QueryValueFormat(string typ, object vlu, object obVal)
        {

            switch (typ)
            {
                case "datetime":
                case "date":
                    {
                        string dateFormat = (typ == "date") ? "yyyy-MM-dd" : "yyyy-MM-ddTHH:mm:ss.000Z";
                        // 
                        // 5.12 allow strings like
                        // today, today - 1 , today - 150, today + 30
                        // to be translated into vba dates for the query...
                        // 
                        if (Conversions.ToBoolean(Strings.InStr(Strings.LCase(Conversions.ToString(vlu)), "today")))
                        {
                            DateTime today;
                            today = new DateTime();

                            int incr;
                            incr = 0;
                            if (Conversions.ToString(vlu).Contains("-"))
                            {
                                var daychange = Strings.Split(Conversions.ToString(vlu), "-");
                                incr = Conversions.ToInteger(Operators.SubtractObject(0, Conversion.Int(daychange[1])));
                            }

                            if (Conversions.ToString(vlu).Contains("+"))
                            {
                                var daychange = Strings.Split(Conversions.ToString(vlu), "+");
                                incr = Conversions.ToInteger(Conversion.Int(daychange[1]));
                            }

                            vlu = DateAndTime.DateAdd("d", incr, today);
                            return Strings.Format(vlu, dateFormat);
                        } // 5.12 end

                        return Strings.Format(obVal, dateFormat);
                    }

                case "double":
                case "currency":
                case "percent":  // add percent per Scot S. 5.67
                    {
                        if (Conversions.ToBoolean(Strings.InStr(Conversions.ToString(vlu), ".")))
                        {
                            return Conversion.Val(vlu).ToString(); // if the double has a decimal already, dont need to add .0
                        }
                        else
                        {
                            return Conversion.Val(vlu) + ".0";
                        }
                    }

                case "boolean":
                    {
                        return Conversions.ToString(Interaction.IIf(Conversions.ToBoolean(Operators.OrObject(Conversion.Val(vlu), Operators.ConditionalCompareObjectEqual("true", Strings.LCase(Conversions.ToString(vlu)), false))), "TRUE", "FALSE"));
                    }

                case "int": // 6.11 by scot stony
                    {
                        return Conversions.ToString(vlu); // all which look like string, including but not limited to
                    }

                default:
                    {
                        return Conversions.ToString(Operators.ConcatenateObject(Operators.ConcatenateObject("'", vlu), "'")); // string, picklist, id, reference, textarea, combobox email
                    }
            }
        }

        public static Dictionary<string, RESTful.Field> getFieldMap(RESTful.Field[] field)
        {
            var fields = new Dictionary<string, RESTful.Field>();
            foreach (RESTful.Field fld in field)
                fields.Add(fld.name, fld);
            return fields;
        }
        /// <summary>
        /// Add fields by label, but if a label appears twice, we remove it entirely from the 
        /// map, as it's ambiguous and we need to know which version of the field it was from
        /// the comment property as a fallback. If there's no comment, the query will fall short.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static Dictionary<string, RESTful.Field> getFieldLabelMap(RESTful.Field[] field)
        {
            var fields = new Dictionary<string, RESTful.Field>(StringComparer.OrdinalIgnoreCase);
            var blackList = new HashSet<string>();
            foreach (RESTful.Field fld in field)
            {
                if (!blackList.Contains(fld.label))
                {
                    if (fields.ContainsKey(fld.label))
                    {
                        blackList.Add(fld.label);
                        fields.Remove(fld.label);
                    }
                    else
                    {
                        fields.Add(fld.label, fld);
                    }
                }
            }
            return fields;
        }
        public static string getAPINameFromCell(Range cell)
        {
            return getAPIName(cell.Comment.Text());
        }

        public static string getAPIName(string commentText)
        {
            int idx = commentText.IndexOf(Microsoft.VisualBasic.Constants.vbCrLf) == -1 ? commentText.Length : commentText.IndexOf(Microsoft.VisualBasic.Constants.vbCrLf);
            return commentText.Substring(10, idx - 10);
        }

        public static string typeToFormat(string sfType)
        {
            string typeToFormatRet = default;
            typeToFormatRet = "General"; // default
            switch (sfType ?? "")
            {
                case "date":
                case "datetime": // re-written for 5.66
                    {
                        typeToFormatRet = "yyyy-MM-dd";
                        if (sfType == "datetime")
                        {
                            typeToFormatRet = typeToFormatRet + " HH:mm:ss"; // 5.15
                        }

                        break;
                    }

                case "string":
                case "picklist":
                case "phone": // , "textarea"
                    {
                        typeToFormatRet = "@";
                        break;
                    }

                case "currency":
                    {
                        typeToFormatRet = "$#,##0_);($#,##0)"; // format as currency, no cents (added in 5.15)
                        break;
                    }
            }

            return typeToFormatRet;
        }

        public static bool IsMissing(object cond)
        {
            if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(cond, true, false)))
                return true;
            else
                return false;
        }

        // 
        // Converts a 15 character ID to an 18 character, case-insensitive one ...
        // got this one from sforce community
        // thanks go to Scot Stoney
        // 
        public static string FixID(string InID)
        {
            string FixIDRet = default;
            FixIDRet = "";
            if (Strings.Len(InID) == 18)
            {
                FixIDRet = InID;
                return FixIDRet;
            }

            string InChars;
            int InI;
            string InUpper;
            int InCnt;
            InChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ012345";
            InUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            InCnt = 0;
            for (InI = 15; InI >= 1; InI -= 1)
            {
                InCnt = 2 * InCnt + Math.Sign(Strings.InStr(1, InUpper, Strings.Mid(InID, InI, 1), Microsoft.VisualBasic.Constants.vbBinaryCompare));
                if (InI % 5 == 1)
                {
                    FixIDRet = Strings.Mid(InChars, InCnt + 1, 1) + FixIDRet;
                    InCnt = 0;
                }
            }

            FixIDRet = InID + FixIDRet;
            return FixIDRet;
        }

        // 
        // look at the field type and variant type, cast the into a return value
        // 
        public static object toVBtype(Range cel, RESTful.Field field)
        {
            object toVBtypeRet = default;
            object val = cel.Value;
            // empty cell is null - special case
            if (string.IsNullOrEmpty(Conversions.ToString(val)))
            {
                return null;
            }

            switch (field.type ?? "")
            {
                case "int":
                    {
                        int i;
                        i = (int)Math.Round(Conversion.Int(Conversion.Val(val)));
                        toVBtypeRet = i;
                        break;
                    }

                case "percent":
                    {
                        toVBtypeRet = (object)Conversion.Val(val); // normal case
                        break;
                    }

                case "double":
                case "currency":
                    {
                        // val() does not use i18n conventions, use CDbl instead, 6.08
                        
                        toVBtypeRet = Conversions.ToDouble(val);  // normal case
                                                                                      // 6.01 truncate to the number of digits, Field3 likes it's numbers formated
                        if (field.scale == 0)
                        {
                            toVBtypeRet = Conversion.Int(toVBtypeRet);
                        }
                        else // If (field.Scale > 0) Then
                        {
                            int z = Strings.InStr(Conversions.ToString(val), Conversions.ToString(ThisAddIn.excelApp.International[XlApplicationInternational.xlDecimalSeparator]));
                            if (z > 0)  // need to remove any extra decimal places
                            {
                                toVBtypeRet = Conversions.ToDouble(Strings.Left(Conversions.ToString(val), z + field.scale));
                            }
                        }

                        break;
                    }

                case "datetime":
                case "date":
                    {
                        if (val is DateTime dt)
                        {
                            string typeToFormat = "yyyy-MM-dd";
                            if (field.type == "datetime")
                            {
                                typeToFormat = "s";
                            }
                            toVBtypeRet = dt.ToString(typeToFormat);
                        }
                        else
                        {
                            toVBtypeRet = val;
                        }

                        break;
                    }

                case "boolean":
                    {
                        toVBtypeRet = val;
                        break;
                    }

                case "reference":
                    {
                        // deal with user names in a reference id field here 5.29
                        // and record types, and others that ref_id can deal with 5.34
                        // need to map a name into the actual ID prior to passing to update
                        // ref_id routine will return the passed in value if we don't map
                        // the ReferenceTo type provided (User,Group,Profile... etc) as a fallback
                        toVBtypeRet = val;
                        if (field.referenceTo.Length > 0)
                        {
                            toVBtypeRet = Util.NameToId(Conversions.ToString(val), field.referenceTo[0]);
                        }
                        if (!(Strings.Len(toVBtypeRet) == 15 || Strings.Len(toVBtypeRet) == 18))
                        {
                            throw new Exception($"Invalid Id format for {field.name} has value {val} translated to {toVBtypeRet}");
                        } // all other types (so far),  work with this "string" type

                        break;
                    }

                default:
                    {
                        toVBtypeRet = "" + val.ToString();
                        break;
                    }
            }

            return toVBtypeRet;
        }

        public static void ScrollAtBottom(ref Window win, long outrow)
        {
            long sr;
            sr = outrow - win.VisibleRange.Rows.Count + 2L;
            if (sr < 1L)
                sr = 1L;
            win.ScrollRow = (int)sr;
        }

        // given a users name or string name of a reference type
        // return the id or any other case or
        // if it's not a reference at all, ref_to will be nul, make that case
        // return the name_string, this is working in 5.46
        public static string NameToId(string name_string, string objectName)
        {
            if (!RegDB.RegQueryBoolValue(ForceConnector.USE_REFERENCE))
                return name_string;
            string[] names;
            IDictionary[] records;

            switch (Strings.Len(name_string))
            {
                case 15:
                case 18: // length matches, and two adjacent numbers, looks like an ID 5.51
                    {
                        // kick out now or we may turn a real ID into a name string
                        if (LikeOperator.LikeString(name_string, "*[0-9][0-9]*", CompareMethod.Binary))
                            return name_string;
                        break;
                    }
            };

            switch (objectName ?? "")
            {
                case "User":
                    {
                        if (ThisAddIn.UserNames.ContainsKey(name_string))
                        {
                            return ThisAddIn.UserNames[name_string];
                        }
                        if (ThisAddIn.UserNames.Count == 0)
                        {
                            records = QueryAll("SELECT Name, Id FROM User");
                            foreach (IDictionary record in records)
                            {
                                var refId = record["Id"];
                                ThisAddIn.UserNames.Add(Conversions.ToString(refId), Conversions.ToString(record["Name"]));
                                ThisAddIn.UserNames.Add(name_string, Conversions.ToString(refId));

                            }


                            if (ThisAddIn.UserNames.ContainsKey(name_string))
                            {
                                return ThisAddIn.UserNames[name_string];
                            }
                            else
                            {
                                return name_string;
                            }
                        }
                        else
                        {
                            return name_string;
                        }


                    }

                // use of this type of reference should be controled by an option
                // it could cause a serious performance problem on long queries
                // and is not going to return unique strings if there are dups in the database
                // should check for dups!!! and then return the ID passed in rather than guessing.
                // as it does now TODO
                case "RecordType":
                    {
                        if (ThisAddIn.RecordTypes.ContainsKey(name_string))
                        {
                            return ThisAddIn.RecordTypes[name_string];
                        }

                        records = QueryAll("SELECT Id, Name FROM RecordType WHERE Name = '" + name_string + "'");
                        foreach (var record in records)
                        {
                            var refId = record["Id"];
                            ThisAddIn.RecordTypes.Add(Conversions.ToString(refId), name_string);
                            ThisAddIn.RecordTypes.Add(name_string, Conversions.ToString(refId));
                        }

                        if (ThisAddIn.RecordTypes.ContainsKey(name_string))
                        {
                            return ThisAddIn.RecordTypes[name_string];
                        }
                        else
                        {
                            return name_string;
                        }


                    }

                case "Profile":
                    {
                        if (ThisAddIn.Profiles.ContainsKey(name_string))
                        {
                            return ThisAddIn.Profiles[name_string];
                        }

                        records = QueryAll("SELECT Id, Name FROM Profile WHERE Name = '" + name_string + "'");
                        foreach (var record in records)
                        {
                            var refId = record["Id"];
                            ThisAddIn.Profiles.Add(Conversions.ToString(refId), name_string);
                            ThisAddIn.Profiles.Add(name_string, Conversions.ToString(refId));
                        }

                        if (ThisAddIn.Profiles.ContainsKey(name_string))
                        {
                            return ThisAddIn.Profiles[name_string];
                        }
                        else
                        {
                            return name_string;
                        }


                    }

                case "Group":
                    {
                        if (ThisAddIn.Groups.ContainsKey(name_string))
                        {
                            return ThisAddIn.Groups[name_string];
                        }

                        records = QueryAll("SELECT Id, Name FROM Group WHERE Name = '" + name_string + "'");
                        foreach (var record in records)
                        {
                            var refId = record["Id"];

                            ThisAddIn.Groups.Add(Conversions.ToString(refId), name_string);
                            ThisAddIn.Groups.Add(name_string, Conversions.ToString(refId));

                        }

                        if (ThisAddIn.Groups.ContainsKey(name_string))
                        {
                            return ThisAddIn.Groups[name_string];
                        }
                        else
                        {
                            return name_string;
                        }


                    }

                case "UserRole":
                    {
                        if (ThisAddIn.Roles.ContainsKey(name_string))
                        {
                            return ThisAddIn.Roles[name_string];
                        }

                        records = QueryAll("SELECT Id, Name FROM UserRole WHERE Name = '" + name_string + "'");
                        foreach (var record in records)
                        {
                            var refId = record["Id"];

                            ThisAddIn.Roles.Add(Conversions.ToString(refId), name_string);
                            ThisAddIn.Roles.Add(name_string, Conversions.ToString(refId));

                        }

                        if (ThisAddIn.Roles.ContainsKey(name_string))
                        {
                            return ThisAddIn.Roles[name_string];
                        }
                        else
                        {
                            return name_string;
                            // and we arrive here for not a ref_to at all in 5.46
                        } // 5.37 don't know how to map this type, so restore the value passed in


                    }

                default:
                    {
                        return name_string; // assume it was correct and we got called by mistake.
                    }
            }
        }

        // 
        // lookup ID and return the string name and add the name to a dict
        // works on user id's and record types currently, can be extended
        // for roles, profiles, groups, etc.
        // 
        public static string IdToName(string objectid)
        {
            if (!RegDB.RegQueryBoolValue(ForceConnector.USE_REFERENCE))
                return objectid;
            object[] records;
            var keyPrefixes = new[] { "005", "012", "00e", "00G", "00E" };
            string prefix = Strings.Left(objectid, 3);
            if (string.IsNullOrEmpty(objectid))
                return ""; // we get here for Converted Account Id in leads which are not converted...
            if (!keyPrefixes.Contains(prefix))
                return objectid;
            ;

            switch (prefix ?? "")
            {
                case "005": // User
                    {
                        // would be nice to look up and return the org info here...
                        // since this is located int the session object we can pull it
                        if (ThisAddIn.UserNames.ContainsKey(objectid))
                        {
                            return ThisAddIn.UserNames[objectid];
                        }

                        records = QueryAll("SELECT FirstName, LastName, Id FROM User WHERE Id = '" + objectid + "' ");
                        foreach (var currentRecord in records)
                        {
                            if (currentRecord is IDictionary record)
                            {
                                Debug.Assert(Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(objectid, record["Id"], false))); // better be true
                                var refName = Operators.ConcatenateObject(Operators.ConcatenateObject(record["FirstName"], " "), record["LastName"]);
                                ThisAddIn.UserNames.Add(Conversions.ToString(record["Id"]), Conversions.ToString(refName));
                                ThisAddIn.UserNames.Add(Conversions.ToString(refName), Conversions.ToString(record["Id"]));
                            }
                        }

                        if (ThisAddIn.UserNames.ContainsKey(objectid))
                        {
                            return ThisAddIn.UserNames[objectid];
                        }
                        else
                        {
                            return objectid;
                        }


                    }

                case "012": // RecordType
                    {
                        if (ThisAddIn.RecordTypes.ContainsKey(objectid))
                        {
                            return ThisAddIn.RecordTypes[objectid];
                        }

                        records = QueryAll("SELECT Id, Name FROM RecordType WHERE Id = '" + objectid + "'");
                        foreach (var currentRecord1 in records)
                        {
                            if (currentRecord1 is IDictionary record)
                            {

                                var refName = record["Name"];

                                ThisAddIn.RecordTypes.Add(Conversions.ToString(record["Id"]), Conversions.ToString(refName));
                                ThisAddIn.RecordTypes.Add(Conversions.ToString(refName), Conversions.ToString(record["Id"]));
                            }
                        }

                        if (ThisAddIn.RecordTypes.ContainsKey(objectid))
                        {
                            return ThisAddIn.RecordTypes[objectid];
                        }
                        else
                        {
                            return objectid;
                        }


                    }

                case "00e": // Profile
                    {
                        if (ThisAddIn.Profiles.ContainsKey(objectid))
                        {
                            return ThisAddIn.Profiles[objectid];
                        }

                        records = QueryAll("SELECT Id, Name FROM Profile WHERE Id = '" + objectid + "'");
                        foreach (var currentRecord2 in records)
                        {
                            if (currentRecord2 is IDictionary record)
                            {

                                var refName = record["Name"];

                                ThisAddIn.Profiles.Add(Conversions.ToString(record["Id"]), Conversions.ToString(refName));
                                ThisAddIn.Profiles.Add(Conversions.ToString(refName), Conversions.ToString(record["Id"]));
                            }
                        }

                        if (ThisAddIn.Profiles.ContainsKey(objectid))
                        {
                            return ThisAddIn.Profiles[objectid];
                        }
                        else
                        {
                            return objectid;
                        }


                    }

                case "00G": // Group
                    {
                        if (ThisAddIn.Groups.ContainsKey(objectid))
                        {
                            return ThisAddIn.Groups[objectid];
                        }

                        records = QueryAll("SELECT Id, Name FROM Group WHERE Id = '" + objectid + "'");
                        foreach (var currentRecord3 in records)
                        {
                            if (currentRecord3 is IDictionary record)
                            {

                                var refName = record["Name"];


                                // 5.40 sometimes the group name is empty stash the id instead of an empty name
                                if (Operators.ConditionalCompareObjectEqual(record["Name"], "", false))
                                    return Conversions.ToString(record["Id"]);
                                ThisAddIn.Groups.Add(Conversions.ToString(record["Id"]), Conversions.ToString(refName));
                                ThisAddIn.Groups.Add(Conversions.ToString(refName), Conversions.ToString(record["Id"]));
                            }
                        }

                        if (ThisAddIn.Groups.ContainsKey(objectid))
                        {
                            return ThisAddIn.Groups[objectid];
                        }
                        else
                        {
                            return objectid;
                        }


                    }

                case "00E": // UserRole
                    {
                        if (ThisAddIn.Roles.ContainsKey(objectid))
                        {
                            return ThisAddIn.Roles[objectid];
                        }

                        records = QueryAll("SELECT Id, Name FROM UserRole WHERE Id = '" + objectid + "'");
                        foreach (var currentRecord4 in records)
                        {
                            if (currentRecord4 is IDictionary record)
                            {

                                var refName = record["Name"];

                                ThisAddIn.Roles.Add(Conversions.ToString(record["Id"]), Conversions.ToString(refName));
                                ThisAddIn.Roles.Add(Conversions.ToString(refName), Conversions.ToString(record["Id"]));
                            }
                        }

                        if (ThisAddIn.Roles.ContainsKey(objectid))
                        {
                            return ThisAddIn.Roles[objectid];
                        }
                        else
                        {
                            return objectid;
                        }


                    }

                default:
                    {
                        return objectid; // too small, throw it back
                    }
            }
        }

        public static IDictionary[] QueryAll(string q)
        {
            var qrs = RESTAPI.Query(q);
            var records = new List<IDictionary>();
            try
            {
                if (qrs.totalSize > 0)
                {
                    records.AddRange(qrs.records);
                }

                while (!qrs.done)
                {

                    qrs = RESTAPI.QueryMore(qrs.nextRecordsUrl);
                    if (qrs.totalSize > 0)
                    {
                        records.AddRange(qrs.records);
                    }

                }
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message, Title: "Salesforce.QueryAll Error");
            }
            return records.ToArray();
        }
    }
}