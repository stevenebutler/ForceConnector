using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Script.Serialization;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace ForceConnector
{
    static class RESTAPI
    {
        public static string Version = ThisAddIn.api.ToString("F1");
        private readonly static string DescribeSObjectsUrl = "{0}/services/data/v{1}/sobjects/";
        private readonly static string SOjectDescribeUrl = "{0}/services/data/v{1}/sobjects/{2}/describe/";
        private readonly static string QueryRecordsUrl = "{0}/services/data/v{1}/query/?q={2}";
        private readonly static string QueryRecordsAllUrl = "{0}/services/data/v{1}/queryAll/?q={2}";
        private readonly static string RetrieveRecordsUrl = "{0}/services/data/v{1}/composite/sobjects/{2}"; // POST with upto 2000 records, GET with upto 800 records
        private readonly static string CreateRecordsUrl = "{0}/services/data/v{1}/composite/sobjects"; // POST with Content-Type 'application/json', upto 200 records
        private readonly static string UpdateRecordsUrl = "{0}/services/data/v{1}/composite/sobjects"; // PATCH with Content-Type 'application/json', upto 200 records
        private readonly static string UpsertRecordsUrl = "{0}/services/data/v{1}/composite/sobjects/{2}/{3}"; // POST(?)/PATCH with Content-Type 'application/json', upto 200 records
        private readonly static string DeleteRecordsUrl = "{0}/services/data/v{1}/composite/sobjects?ids={2}&allOrNone={3}"; // DELETE, upto 200 records

        public static RESTful.DescribeGlobalSObjectResult[] getSObjectList()
        {
            RESTful.DescribeGlobalResult dgr;
            try
            {
                dgr = DescribeSObjects();
            }
            catch (Exception ex)
            {
                throw new Exception("getSObjectList Exception!" + Constants.vbCrLf + ex.Message);
            }

            return dgr.sobjects;
        }

        public static RESTful.ConnectionInfo getConnectionInfo()
        {
            var describeResult = new RESTful.ConnectionInfo();
            var jss = new JavaScriptSerializer();
            string serviceUrl = ThisAddIn.id + "?version=latest";
            string json = CallREST("GET", serviceUrl);
            try
            {
                describeResult = jss.Deserialize<RESTful.ConnectionInfo>(json);
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message, Title: "getConnectionInfo Deserialize Exception");
                // Throw New Exception("getConnectionInfo Deserialize Exception" & vbCrLf & ex.Message)
            }

            return describeResult;
        }

        public static RESTful.DescribeGlobalResult DescribeSObjects()
        {
            RESTful.DescribeGlobalResult describeResult;
            var jss = new JavaScriptSerializer();
            string serviceUrl = Conversions.ToString(string.Format(DescribeSObjectsUrl, ThisAddIn.instanceUrl, Version));
            string json = CallREST("GET", serviceUrl);
            try
            {
                describeResult = jss.Deserialize<RESTful.DescribeGlobalResult>(json);
            }
            catch (Exception ex)
            {
                // MsgBox(ex.Message, Title:="DescribeSObjects Deserialize Exception")
                throw new Exception("DescribeSObjects Deserialize Exception" + Constants.vbCrLf + ex.Message);
            }

            return describeResult;
        }

        public static RESTful.DescribeSObjectResult DescribeSObject(string objectName)
        {
            RESTful.DescribeSObjectResult describeResult;
            var jss = new JavaScriptSerializer();
            string serviceUrl = Conversions.ToString(string.Format(SOjectDescribeUrl, ThisAddIn.instanceUrl, Version, objectName));
            string json = CallREST("GET", serviceUrl);
            try
            {
                describeResult = jss.Deserialize<RESTful.DescribeSObjectResult>(json);
            }
            catch (Exception ex)
            {
                // MsgBox(ex.Message, Title:="DescribeSObject Deserialize Exception")
                throw new Exception("DescribeSObject Deserialize Exception" + Constants.vbCrLf + ex.Message);
            }

            return describeResult;
        }

        public static RESTful.QueryResult Query(string queryString)
        {
            RESTful.QueryResult queryResult;
            var jss = new JavaScriptSerializer();
            var headers = new Dictionary<string, string>();
            headers.Add("Sforce-Query-Options", "batchSize=50");
            string serviceUrl = Conversions.ToString(string.Format(QueryRecordsUrl, ThisAddIn.instanceUrl, Version, queryString));
            string json = CallREST("GET", serviceUrl, headers: headers);
            try
            {
                queryResult = jss.Deserialize<RESTful.QueryResult>(json);
            }
            catch (Exception ex)
            {
                // MsgBox(ex.Message, Title:="Query Deserialize Exception")
                throw new Exception("Query Deserialize Exception" + Constants.vbCrLf + ex.Message);
            }

            return queryResult;
        }

        public static RESTful.QueryResult QueryMore(string nextQueryUrl)
        {
            RESTful.QueryResult queryResult;
            var jss = new JavaScriptSerializer();
            var headers = new Dictionary<string, string>();
            headers.Add("Sforce-Query-Options", "batchSize=50");
            string moreUrl = ThisAddIn.instanceUrl + nextQueryUrl;
            string json = CallREST("GET", moreUrl, headers: headers);
            try
            {
                queryResult = jss.Deserialize<RESTful.QueryResult>(json);
            }
            catch (Exception ex)
            {
                // MsgBox(ex.Message, Title:="QueryMore Deserialize Exception")
                throw new Exception("QueryMore Deserialize Exception" + Constants.vbCrLf + ex.Message);
            }

            return queryResult;
        }

        public static object[] RetrieveRecords(string objectName, string[] ids, string[] fields)
        {
            object[] recordSet;
            var jss = new JavaScriptSerializer();
            var objectBody = new Dictionary<string, string[]>() { { "ids", ids }, { "fields", fields } };
            string serviceUrl = Conversions.ToString(string.Format(RetrieveRecordsUrl, ThisAddIn.instanceUrl, Version, objectName));
            string stringBody = jss.Serialize(objectBody);
            string json = CallREST("POST", serviceUrl, stringBody);
            try
            {
                recordSet = jss.Deserialize<object[]>(json);
            }
            catch (Exception ex)
            {
                // MsgBox(ex.Message, Title:="RetrieveRecords Deserialize Exception")
                throw new Exception("RetrieveRecords Deserialize Exception" + Constants.vbCrLf + ex.Message);
            }

            return recordSet;
        }

        public static RESTful.SaveResult[] CreateRecords(object[] records, bool allOrNone = true)
        {
            RESTful.SaveResult[] saveResults;
            var jss = new JavaScriptSerializer();
            var headers = new Dictionary<string, string>();
            if (!RegDB.RegQueryBoolValue(ForceConnector.AUTOASSIGNRULE))
            {
                headers.Add("Sforce-Auto-Assign", "False");
            }

            string serviceUrl = Conversions.ToString(string.Format(CreateRecordsUrl, ThisAddIn.instanceUrl, Version));
            var recordset = new RESTful.RecordSet();
            recordset.allOrNone = allOrNone;
            recordset.records = records;
            string json = CallREST("POST", serviceUrl, jss.Serialize(recordset), headers);
            try
            {
                saveResults = jss.Deserialize<RESTful.SaveResult[]>(json);
            }
            catch (Exception ex)
            {
                // MsgBox(ex.Message, Title:="CreateRecords Deserialize Exception")
                throw new Exception("CreateRecords Deserialize Exception" + Constants.vbCrLf + ex.Message);
            }

            return saveResults;
        }

        public static RESTful.SaveResult[] UpdateRecords(object[] records, bool allOrNone = true)
        {
            RESTful.SaveResult[] saveResults;
            var jss = new JavaScriptSerializer();
            var headers = new Dictionary<string, string>();
            if (!RegDB.RegQueryBoolValue(ForceConnector.AUTOASSIGNRULE))
            {
                headers.Add("Sforce-Auto-Assign", "False");
            }

            string serviceUrl = Conversions.ToString(string.Format(UpdateRecordsUrl, ThisAddIn.instanceUrl, Version));
            var recordset = new RESTful.RecordSet();
            recordset.allOrNone = allOrNone;
            recordset.records = records;
            string json = CallREST("PATCH", serviceUrl, jss.Serialize(recordset), headers);
            try
            {
                saveResults = jss.Deserialize<RESTful.SaveResult[]>(json);
            }
            catch (Exception ex)
            {
                // MsgBox(ex.Message, Title:="UpdateRecords Deserialize Exception")
                throw new Exception("UpdateRecords Deserialize Exception" + Constants.vbCrLf + ex.Message);
            }

            return saveResults;
        }

        public static RESTful.UpsertResult[] UpsertRecords(string objectName, string extId, object[] records, bool allOrNone = true)
        {
            RESTful.UpsertResult[] upsertResults;
            var jss = new JavaScriptSerializer();
            var headers = new Dictionary<string, string>();
            if (!RegDB.RegQueryBoolValue(ForceConnector.AUTOASSIGNRULE))
            {
                headers.Add("Sforce-Auto-Assign", "False");
            }

            string serviceUrl = Conversions.ToString(string.Format(UpsertRecordsUrl, ThisAddIn.instanceUrl, Version, objectName, extId));
            var recordset = new RESTful.RecordSet();
            recordset.allOrNone = allOrNone;
            recordset.records = records;
            string json = CallREST("PATCH", serviceUrl, jss.Serialize(recordset), headers);
            try
            {
                upsertResults = jss.Deserialize<RESTful.UpsertResult[]>(json);
            }
            catch (Exception ex)
            {
                // MsgBox(ex.Message, Title:="UpsertRecords Deserialize Exception")
                throw new Exception("UpsertRecords Deserialize Exception" + Constants.vbCrLf + ex.Message);
            }

            return upsertResults;
        }

        public static RESTful.DeleteResult[] DeleteRecords(string objectName, string[] delIds, bool allOrNone = true)
        {
            RESTful.DeleteResult[] deleteResults;
            var jss = new JavaScriptSerializer();
            string aon = Conversions.ToString(Interaction.IIf(allOrNone, "true", "false"));
            string ids = string.Join(",", delIds);
            string serviceUrl = Conversions.ToString(string.Format(DeleteRecordsUrl, ThisAddIn.instanceUrl, Version, ids, aon));
            string json = CallREST("DELETE", serviceUrl, "");
            try
            {
                deleteResults = jss.Deserialize<RESTful.DeleteResult[]>(json);
            }
            catch (Exception ex)
            {
                // MsgBox(ex.Message, Title:="DeleteRecords Deserialize Exception")
                throw new Exception("DeleteRecords Deserialize Exception" + Constants.vbCrLf + ex.Message);
            }

            return deleteResults;
        }

        private static string CallREST(string method, string url, string body = "", Dictionary<string, string> headers = null)
        {
            string jsonstring = "";
            var client = new HttpClient();
            var request = new HttpRequestMessage(new HttpMethod(method.ToUpper()), url);
            var response = new HttpResponseMessage();
            try
            {
                client.Timeout = new TimeSpan(0, 0, 120);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ThisAddIn.accessToken);
                if (headers is object)
                {
                    if (headers.Count > 0)
                    {
                        var keys = headers.Keys;
                        foreach (string key in keys)
                        {
                            string value = headers[key];
                            request.Headers.Add(key, value);
                        }
                    }
                }

                if (method == "POST" | method == "PATCH")
                {
                    var buffer = Encoding.UTF8.GetBytes(body);
                    var contentBody = new ByteArrayContent(buffer);
                    contentBody.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    request.Content = contentBody;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CallREST Preperation Exception!! " + Constants.vbCrLf + ex.Message);
            }

            try
            {
                response = client.SendAsync(request).Result;
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        jsonstring = response.Content.ReadAsStringAsync().Result;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ReadAsStringAsync Exception!!" + Constants.vbCrLf + ex.Message);
                        // MsgBox(ex.Message, Title:="Handle CallREST Response Exception")
                    }
                }
                else
                {
                    // MsgBox(response.StatusCode & " (" & response.ReasonPhrase & ")", Title:="CallREST Failed!")
                    throw new Exception("SendAsync Error!! " + Constants.vbCrLf + ((int)response.StatusCode).ToString() + " (" + response.ReasonPhrase + ")");
                }
            }
            catch (Exception ex)
            {
                // MsgBox(ex.Message, Title:="CallREST Exception")
                throw new Exception("CallREST Exception!! " + Constants.vbCrLf + ex.Message);
            }

            return jsonstring;
        }

        public static string CallREST2(string method, string url, string body = "", Dictionary<string, string> headers = null)
        {
            string jsonstring = "";
            var client = new HttpClient();
            var response = new HttpResponseMessage();
            try
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + ThisAddIn.accessToken);
                client.Timeout = new TimeSpan(0, 0, 120);
                if (headers is object)
                {
                    var keys = headers.Keys;
                    foreach (string key in keys)
                    {
                        string value = headers[key];
                        client.DefaultRequestHeaders.TryAddWithoutValidation(key, value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CallREST2 Preperation Exception!! " + Constants.vbCrLf + ex.Message);
            }

            try
            {
                if (method == "GET")
                {
                    response = client.GetAsync(url).Result;
                }
                else if (method == "POST" | method == "PATCH")
                {
                    // Dim content As StringContent = New StringContent(body, Encoding.UTF8, "application/json")
                    var bytearray = Encoding.UTF8.GetBytes(body);
                    var content = new ByteArrayContent(bytearray);
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    if (method == "PATCH")
                        url = url + "?_HttpMethod=PATCH";
                    response = client.PostAsync(url, content).Result;
                }
                else if (method == "DELETE")
                {
                    response = client.DeleteAsync(url).Result;
                }

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        jsonstring = response.Content.ReadAsStringAsync().Result;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ReadAsStringAsync Exception!!" + Constants.vbCrLf + ex.Message);
                        // MsgBox(ex.Message, Title:="Handle Response Exception")
                    }
                }
                else
                {
                    throw new Exception("SendAsync Error!! " + Constants.vbCrLf + ((int)response.StatusCode).ToString() + " (" + response.ReasonPhrase + ")");
                    // MsgBox(response.StatusCode & " (" & response.ReasonPhrase & ")", Title:="CallREST2 Failed!")
                }
            }

            // Console.ReadKey()
            catch (Exception ex)
            {
                // MsgBox(ex.Message, Title:="CallREST Exception")
                throw new Exception("CallREST2 Exception!! " + Constants.vbCrLf + ex.Message);
            }

            return jsonstring;
        }
    }

    public class RESTful
    {
        // RESTful
        // Helper class for interfacing by RESTful API
        // @Created By : MinGyoon Woo, 2018-06-05
        // @Modified By : MinGyoon Woo, 2020-08-25

        public class Attributes
        {
            public string type { get; set; }
            public string url { get; set; }

            public Attributes(string type, string url = "")
            {
                this.type = type;
                if (!ReferenceEquals(url, ""))
                    this.url = url;
            }
        }

        public class ActionOverride
        {
            public string formFactor { get; set; }
            public bool isAvailableInTouch { get; set; }
            public string name { get; set; }
            public string pageId { get; set; }
            public string url { get; set; }
        }

        public class ChildRelationship
        {
            public bool cascadeDelete { get; set; }
            public string childSObject { get; set; }
            public bool deprecatedAndHidden { get; set; }
            public string field { get; set; }
            public string[] junctionIdListNames { get; set; }
            public string[] junctionReferenceTo { get; set; }
            public string relationshipName { get; set; }
            public bool restrictedDelete { get; set; }
        }

        public class DeleteResult
        {
            public SalesforceError[] errors { get; set; }
            public string id { get; set; }
            public bool success { get; set; }
        }

        public class DescribeGlobalResult
        {
            public string encoding { get; set; }
            public int maxBatchSize { get; set; }
            public DescribeGlobalSObjectResult[] sobjects { get; set; }
        }

        public class DescribeGlobalSObjectResult
        {
            public bool activateable { get; set; }
            public bool custom { get; set; }
            public bool customSetting { get; set; }
            public bool createable { get; set; }
            public bool deletable { get; set; }
            public bool deprecatedAndHidden { get; set; }
            public bool feedEnabled { get; set; }
            public string keyPrefix { get; set; }
            public string label { get; set; }
            public string labelPlural { get; set; }
            public bool layoutable { get; set; }
            public bool mergeable { get; set; }
            public bool mruEnabled { get; set; }
            public string name { get; set; }
            public bool queryable { get; set; }
            public bool replicateable { get; set; }
            public bool retrieveable { get; set; }
            public bool searchable { get; set; }
            public bool triggerable { get; set; }
            public bool undeletable { get; set; }
            public bool updateable { get; set; }
            public ServiceUrl urls { get; set; }
        }

        public class DescribeSObjectResult
        {
            public ActionOverride[] actionOverrides { get; set; }
            public bool activateable { get; set; }
            public ChildRelationship[] childRelationships { get; set; }
            public bool compactLayoutable { get; set; }
            public bool createable { get; set; }
            public bool custom { get; set; }
            public bool customSetting { get; set; }
            public bool deepCloneable { get; set; }
            public string defaultImplementation { get; set; }
            public bool deletable { get; set; }
            public bool deprecatedAndHidden { get; set; }
            public string extendedBy { get; set; }
            public string extendsInterfaces { get; set; }
            public bool feedEnabled { get; set; }
            public Field[] fields { get; set; }
            public bool hasSubtypes { get; set; }
            public string implementedBy { get; set; }
            public string implementsInterfaces { get; set; }
            public bool isInterface { get; set; }
            public bool isSubtype { get; set; }
            public string keyPrefix { get; set; }
            public string label { get; set; }
            public string labelPlural { get; set; }
            public bool layoutable { get; set; }
            public object listviewable { get; set; }
            public object lookupLayoutable { get; set; }
            public bool mergeable { get; set; }
            public bool mruEnabled { get; set; }
            public string name { get; set; }
            public NamedLayoutInfo[] namedLayoutInfos { get; set; }
            public string networkScopeFieldName { get; set; }
            public bool queryable { get; set; }
            public RecordTypeInfo[] recordTypeInfos { get; set; }
            public bool replicateable { get; set; }
            public bool retrieveable { get; set; }
            public bool searchable { get; set; }
            public string sobjectDescribeOption { get; set; }
            public ScopeInfo[] supportedScopes { get; set; }
            public bool triggerable { get; set; }
            public bool undeletable { get; set; }
            public bool updateable { get; set; }
            public ServiceUrl urls { get; set; }
        }

        public class Field
        {
            public bool aggregatable { get; set; }
            public bool aiPredictionField { get; set; }
            public bool autoNumber { get; set; }
            public int byteLength { get; set; }
            public bool calculated { get; set; }
            public string calculatedFormula { get; set; }
            public bool cascadeDelete { get; set; }
            public bool caseSensitive { get; set; }
            public string compoundFieldName { get; set; }
            public string controllerName { get; set; }
            public bool createable { get; set; }
            public bool custom { get; set; }
            public object defaultValue { get; set; }
            public string defaultValueFormula { get; set; }
            public bool defaultedOnCreate { get; set; }
            public bool dependentPicklist { get; set; }
            public bool deprecatedAndHidden { get; set; }
            public int digits { get; set; }
            public bool displayLocationInDecimal { get; set; }
            public bool encrypted { get; set; }
            public bool externalId { get; set; }
            public string extraTypeInfo { get; set; }
            public bool filterabl { get; set; }
            public FilteredLookupInfo filteredLookupInfo { get; set; }
            public bool formulaTreatNullNumberAsZero { get; set; }
            public bool groupable { get; set; }
            public bool highScaleNumber { get; set; }
            public bool htmlFormatted { get; set; }
            public bool idLookup { get; set; }
            public string inlineHelpText { get; set; }
            public string label { get; set; }
            public int length { get; set; }
            public string mask { get; set; }
            public string maskType { get; set; }
            public string name { get; set; }
            public bool nameField { get; set; }
            public bool namePointing { get; set; }
            public bool nillable { get; set; }
            public bool permissionable { get; set; }
            public PicklistEntry[] picklistValues { get; set; }
            public bool polymorphicForeignKey { get; set; }
            public int precision { get; set; }
            public bool queryByDistance { get; set; }
            public string referenceTargetField { get; set; }
            public string[] referenceTo { get; set; }
            public string relationshipName { get; set; }
            public object relationshipOrder { get; set; } // Real type is Integer, but Integer does not accept NULL value.
            public bool restrictedDelete { get; set; }
            public bool restrictedPicklist { get; set; }
            public int scale { get; set; }
            public bool searchPrefilterable { get; set; }
            public string soapType { get; set; }
            public bool sortable { get; set; }
            public string type { get; set; }
            public bool unique { get; set; }
            public bool updateable { get; set; }
            public bool writeRequiresMasterRead { get; set; }
        }

        public class FilteredLookupInfo
        {
            public string[] controllingFields { get; set; }
            public bool dependent { get; set; }
            public bool optionalFilter { get; set; }
        }

        public class NamedLayoutInfo
        {
            public string name { get; set; }
        }

        public class PicklistEntry
        {
            public bool active { get; set; }
            public bool defaultValue { get; set; }
            public string label { get; set; }
            public string validFor { get; set; }
            public string value { get; set; }
        }

        public class QueryResult
        {
            public bool done { get; set; }
            public string nextRecordsUrl { get; set; }
            public IDictionary[] records { get; set; }
            public int totalSize { get; set; }
        }

        public class RecordSet
        {
            public bool allOrNone { get; set; }
            public object[] records { get; set; }
        }

        public class RecordTypeInfo
        {
            public bool active { get; set; }
            public bool available { get; set; }
            public bool defaultRecordTypeMapping { get; set; }
            public string developerName { get; set; }
            public bool master { get; set; }
            public string name { get; set; }
            public string recordTypeId { get; set; }
            public Dictionary<string, string> urls { get; set; }
        }

        public class SaveResult
        {
            public SalesforceError[] errors { get; set; }
            public string id { get; set; }
            public bool success { get; set; }
        }

        public class ScopeInfo
        {
            public string label { get; set; }
            public string name { get; set; }
        }

        public class UpsertResult
        {
            public bool created { get; set; }
            public SalesforceError[] errors { get; set; }
            public string id { get; set; }
            public bool success { get; set; }
        }

        /// <summary>
    /// RESTful Only Class
    /// </summary>
        public class ConnectionInfo
        {
            public string id { get; set; }
            public bool asserted_user { get; set; }
            public string user_id { get; set; }
            public string organization_id { get; set; }
            public string username { get; set; }
            public string nick_name { get; set; }
            public string display_name { get; set; }
            public string email { get; set; }
            public bool email_verified { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string addr_street { get; set; }
            public string addr_city { get; set; }
            public string addr_state { get; set; }
            public string addr_country { get; set; }
            public string addr_zip { get; set; }
            public string mobile_phone { get; set; }
            public bool mobile_phone_verified { get; set; }
            public bool is_lighting_login_user { get; set; }
            public bool active { get; set; }
            public string user_type { get; set; }
            public string timezone { get; set; }
            public string language { get; set; }
            public string locale { get; set; }
            public string utcOffset { get; set; }
            public string last_modified_date { get; set; }
            public bool is_app_installed { get; set; }
            public ConnectionStatus status { get; set; }
            public PhotoInfo photos { get; set; }
            public UrlInfo urls { get; set; }
        }

        public class ConnectionStatus
        {
            public string created_date { get; set; }
            public string body { get; set; }
        }

        public class ObjectType
        {
            public string type { get; set; }

            public ObjectType(string type)
            {
                this.type = type;
            }
        }

        public class PhotoInfo
        {
            public string picture { get; set; }
            public string thumbnail { get; set; }
        }

        public class SalesforceError
        {
            public string[] fields { get; set; }
            public string message { get; set; }
            public object statusCode { get; set; }
        }

        public class ServiceUrl
        {
            public string compactLayouts { get; set; }
            public string approvalLayouts { get; set; }
            public string uiDetailTemplate { get; set; }
            public string uiEditTemplate { get; set; }
            public string defaultValues { get; set; }
            public string listviews { get; set; }
            public string uiNewRecord { get; set; }
            public string quickActions { get; set; }
            public string layouts { get; set; }
            public string sobject { get; set; }
            public string describe { get; set; }
            public string rowTemplate { get; set; }
        }

        public class UrlInfo
        {
            public string enterprise { get; set; }
            public string metadata { get; set; }
            public string partner { get; set; }
            public string rest { get; set; }
            public string sobjects { get; set; }
            public string search { get; set; }
            public string query { get; set; }
            public string recent { get; set; }
            public string profile { get; set; }
            public string feeds { get; set; }
            public string groups { get; set; }
            public string users { get; set; }
            public string feed_items { get; set; }
            public string feed_elements { get; set; }
            public string tooling_soap { get; set; }
            public string tooling_rest { get; set; }
            public string custom_domain { get; set; }
        }
    }
}