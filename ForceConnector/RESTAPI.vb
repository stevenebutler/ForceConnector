Option Explicit On
Option Strict Off

Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Web.Script.Serialization

Module RESTAPI
    Public Version As String = ThisAddIn.api.ToString("F1")

    Private ReadOnly DescribeSObjectsUrl = "{0}/services/data/v{1}/sobjects/"
    Private ReadOnly SOjectDescribeUrl = "{0}/services/data/v{1}/sobjects/{2}/describe/"
    Private ReadOnly QueryRecordsUrl = "{0}/services/data/v{1}/query/?q={2}"
    Private ReadOnly QueryRecordsAllUrl = "{0}/services/data/v{1}/queryAll/?q={2}"
    Private ReadOnly RetrieveRecordsUrl = "{0}/services/data/v{1}/composite/sobjects/{2}" ' POST with upto 2000 records, GET with upto 800 records
    Private ReadOnly CreateRecordsUrl = "{0}/services/data/v{1}/composite/sobjects" ' POST with Content-Type 'application/json', upto 200 records
    Private ReadOnly UpdateRecordsUrl = "{0}/services/data/v{1}/composite/sobjects" ' PATCH with Content-Type 'application/json', upto 200 records
    Private ReadOnly UpsertRecordsUrl = "{0}/services/data/v{1}/composite/sobjects/{2}/{3}" ' POST(?)/PATCH with Content-Type 'application/json', upto 200 records
    Private ReadOnly DeleteRecordsUrl = "{0}/services/data/v{1}/composite/sobjects?ids={2}&allOrNone={3}" ' DELETE, upto 200 records

    Public Function getSObjectList() As RESTful.DescribeGlobalSObjectResult()
        Dim dgr As RESTful.DescribeGlobalResult

        Try
            dgr = RESTAPI.DescribeSObjects()
        Catch ex As Exception
            Throw New Exception("getSObjectList Exception!" & vbCrLf & ex.Message)
        End Try

        Return dgr.sobjects
    End Function

    Public Function getConnectionInfo() As RESTful.ConnectionInfo
        Dim describeResult As RESTful.ConnectionInfo = New RESTful.ConnectionInfo()
        Dim jss = New JavaScriptSerializer()

        Dim serviceUrl As String = ThisAddIn.id & "?version=latest"
        Dim json As String = CallREST("GET", serviceUrl)

        Try
            describeResult = jss.Deserialize(Of RESTful.ConnectionInfo)(json)
        Catch ex As Exception
            MsgBox(ex.Message, Title:="getConnectionInfo Deserialize Exception")
            'Throw New Exception("getConnectionInfo Deserialize Exception" & vbCrLf & ex.Message)
        End Try

        Return describeResult
    End Function

    Public Function DescribeSObjects() As RESTful.DescribeGlobalResult
        Dim describeResult As RESTful.DescribeGlobalResult
        Dim jss As JavaScriptSerializer = New JavaScriptSerializer()

        Dim serviceUrl As String = String.Format(DescribeSObjectsUrl, ThisAddIn.instanceUrl, Version)
        Dim json As String = CallREST("GET", serviceUrl)

        Try
            describeResult = jss.Deserialize(Of RESTful.DescribeGlobalResult)(json)
        Catch ex As Exception
            'MsgBox(ex.Message, Title:="DescribeSObjects Deserialize Exception")
            Throw New Exception("DescribeSObjects Deserialize Exception" & vbCrLf & ex.Message)
        End Try

        Return describeResult
    End Function

    Public Function DescribeSObject(ByVal objectName As String) As RESTful.DescribeSObjectResult
        Dim describeResult As RESTful.DescribeSObjectResult
        Dim jss As JavaScriptSerializer = New JavaScriptSerializer()

        Dim serviceUrl As String = String.Format(SOjectDescribeUrl, ThisAddIn.instanceUrl, Version, objectName)
        Dim json As String = CallREST("GET", serviceUrl)

        Try
            describeResult = jss.Deserialize(Of RESTful.DescribeSObjectResult)(json)
        Catch ex As Exception
            'MsgBox(ex.Message, Title:="DescribeSObject Deserialize Exception")
            Throw New Exception("DescribeSObject Deserialize Exception" & vbCrLf & ex.Message)
        End Try

        Return describeResult
    End Function

    Public Function Query(ByVal queryString As String) As RESTful.QueryResult
        Dim queryResult As RESTful.QueryResult
        Dim jss As JavaScriptSerializer = New JavaScriptSerializer()

        Dim headers As Dictionary(Of String, String) = New Dictionary(Of String, String)
        headers.Add("Sforce-Query-Options", "batchSize=50")
        Dim serviceUrl As String = String.Format(QueryRecordsUrl, ThisAddIn.instanceUrl, Version, queryString)
        Dim json As String = CallREST("GET", serviceUrl, headers:=headers)

        Try
            queryResult = jss.Deserialize(Of RESTful.QueryResult)(json)

        Catch ex As Exception
            'MsgBox(ex.Message, Title:="Query Deserialize Exception")
            Throw New Exception("Query Deserialize Exception" & vbCrLf & ex.Message)
        End Try

        Return queryResult
    End Function

    Public Function QueryMore(ByVal nextQueryUrl As String) As RESTful.QueryResult
        Dim queryResult As RESTful.QueryResult
        Dim jss As JavaScriptSerializer = New JavaScriptSerializer()

        Dim headers As Dictionary(Of String, String) = New Dictionary(Of String, String)
        headers.Add("Sforce-Query-Options", "batchSize=50")
        Dim moreUrl As String = ThisAddIn.instanceUrl & nextQueryUrl
        Dim json As String = CallREST("GET", moreUrl, headers:=headers)

        Try
            queryResult = jss.Deserialize(Of RESTful.QueryResult)(json)
        Catch ex As Exception
            'MsgBox(ex.Message, Title:="QueryMore Deserialize Exception")
            Throw New Exception("QueryMore Deserialize Exception" & vbCrLf & ex.Message)
        End Try

        Return queryResult
    End Function

    Public Function RetrieveRecords(ByVal objectName As String, ByVal ids As String(), ByVal fields As String()) As Object()
        Dim recordSet As Object()
        Dim jss As JavaScriptSerializer = New JavaScriptSerializer()

        Dim objectBody = New Dictionary(Of String, String()) From {
            {"ids", ids},
            {"fields", fields}
        }
        Dim serviceUrl As String = String.Format(RetrieveRecordsUrl, ThisAddIn.instanceUrl, Version, objectName)
        Dim stringBody As String = jss.Serialize(objectBody)
        Dim json As String = CallREST("POST", serviceUrl, stringBody)

        Try
            recordSet = jss.Deserialize(Of Object())(json)
        Catch ex As Exception
            'MsgBox(ex.Message, Title:="RetrieveRecords Deserialize Exception")
            Throw New Exception("RetrieveRecords Deserialize Exception" & vbCrLf & ex.Message)
        End Try

        Return recordSet
    End Function

    Public Function CreateRecords(ByVal records As Object(), Optional allOrNone As Boolean = True) As RESTful.SaveResult()
        Dim saveResults() As RESTful.SaveResult
        Dim jss As JavaScriptSerializer = New JavaScriptSerializer()

        Dim headers As Dictionary(Of String, String) = New Dictionary(Of String, String)
        If Not RegQueryBoolValue(AUTOASSIGNRULE) Then
            headers.Add("Sforce-Auto-Assign", "False")
        End If
        Dim serviceUrl As String = String.Format(CreateRecordsUrl, ThisAddIn.instanceUrl, Version)
        Dim recordset As RESTful.RecordSet = New RESTful.RecordSet()
        recordset.allOrNone = allOrNone
        recordset.records = records
        Dim json As String = CallREST("POST", serviceUrl, jss.Serialize(recordset), headers)

        Try
            saveResults = jss.Deserialize(Of RESTful.SaveResult())(json)
        Catch ex As Exception
            'MsgBox(ex.Message, Title:="CreateRecords Deserialize Exception")
            Throw New Exception("CreateRecords Deserialize Exception" & vbCrLf & ex.Message)
        End Try

        Return saveResults
    End Function

    Public Function UpdateRecords(ByVal records As Object(), Optional allOrNone As Boolean = True) As RESTful.SaveResult()
        Dim saveResults As RESTful.SaveResult()
        Dim jss As JavaScriptSerializer = New JavaScriptSerializer()

        Dim headers As Dictionary(Of String, String) = New Dictionary(Of String, String)
        If Not RegQueryBoolValue(AUTOASSIGNRULE) Then
            headers.Add("Sforce-Auto-Assign", "False")
        End If
        Dim serviceUrl As String = String.Format(UpdateRecordsUrl, ThisAddIn.instanceUrl, Version)
        Dim recordset As RESTful.RecordSet = New RESTful.RecordSet()
        recordset.allOrNone = allOrNone
        recordset.records = records
        Dim json As String = CallREST("PATCH", serviceUrl, jss.Serialize(recordset), headers)

        Try
            saveResults = jss.Deserialize(Of RESTful.SaveResult())(json)
        Catch ex As Exception
            'MsgBox(ex.Message, Title:="UpdateRecords Deserialize Exception")
            Throw New Exception("UpdateRecords Deserialize Exception" & vbCrLf & ex.Message)
        End Try

        Return saveResults
    End Function

    Public Function UpsertRecords(ByVal objectName As String, ByVal extId As String, ByVal records As Object(),
                                  Optional allOrNone As Boolean = True) As RESTful.UpsertResult()
        Dim upsertResults As RESTful.UpsertResult()
        Dim jss As JavaScriptSerializer = New JavaScriptSerializer()

        Dim headers As Dictionary(Of String, String) = New Dictionary(Of String, String)
        If Not RegQueryBoolValue(AUTOASSIGNRULE) Then
            headers.Add("Sforce-Auto-Assign", "False")
        End If
        Dim serviceUrl As String = String.Format(UpsertRecordsUrl, ThisAddIn.instanceUrl, Version, objectName, extId)
        Dim recordset As RESTful.RecordSet = New RESTful.RecordSet()
        recordset.allOrNone = allOrNone
        recordset.records = records
        Dim json As String = CallREST("PATCH", serviceUrl, jss.Serialize(recordset), headers)

        Try
            upsertResults = jss.Deserialize(Of RESTful.UpsertResult())(json)
        Catch ex As Exception
            'MsgBox(ex.Message, Title:="UpsertRecords Deserialize Exception")
            Throw New Exception("UpsertRecords Deserialize Exception" & vbCrLf & ex.Message)
        End Try

        Return upsertResults
    End Function

    Public Function DeleteRecords(ByVal objectName As String, ByVal delIds As String(),
                                  Optional allOrNone As Boolean = True) As RESTful.DeleteResult()
        Dim deleteResults As RESTful.DeleteResult()
        Dim jss As JavaScriptSerializer = New JavaScriptSerializer()

        Dim aon As String = IIf(allOrNone, "true", "false")
        Dim ids As String = String.Join(",", delIds)
        Dim serviceUrl As String = String.Format(DeleteRecordsUrl, ThisAddIn.instanceUrl, Version, ids, aon)
        Dim json As String = CallREST("DELETE", serviceUrl, "")

        Try
            deleteResults = jss.Deserialize(Of RESTful.DeleteResult())(json)
        Catch ex As Exception
            'MsgBox(ex.Message, Title:="DeleteRecords Deserialize Exception")
            Throw New Exception("DeleteRecords Deserialize Exception" & vbCrLf & ex.Message)
        End Try

        Return deleteResults
    End Function

    Private Function CallREST(ByVal method As String, ByVal url As String, Optional body As String = "",
                              Optional headers As Dictionary(Of String, String) = Nothing) As String
        Dim jsonstring As String = ""
        Dim client As HttpClient = New HttpClient()
        Dim request As HttpRequestMessage = New HttpRequestMessage(New HttpMethod(method.ToUpper()), url)
        Dim response As HttpResponseMessage = New HttpResponseMessage()

        Try
            client.Timeout = New TimeSpan(0, 0, 120)
            request.Headers.Authorization = New AuthenticationHeaderValue("Bearer", ThisAddIn.accessToken)

            If headers IsNot Nothing Then
                If headers.Count > 0 Then
                    Dim keys As Dictionary(Of String, String).KeyCollection = headers.Keys
                    For Each key As String In keys
                        Dim value As String = headers(key)
                        request.Headers.Add(key, value)
                    Next
                End If
            End If

            If method = "POST" Or method = "PATCH" Then
                Dim buffer As Byte() = Encoding.UTF8.GetBytes(body)
                Dim contentBody = New ByteArrayContent(buffer)
                contentBody.Headers.ContentType = New MediaTypeHeaderValue("application/json")
                request.Content = contentBody
            End If
        Catch ex As Exception
            Throw New Exception("CallREST Preperation Exception!! " & vbCrLf & ex.Message)
        End Try

        Try
            response = client.SendAsync(request).Result()

            If response.IsSuccessStatusCode Then
                Try
                    jsonstring = response.Content.ReadAsStringAsync().Result()
                Catch ex As Exception
                    Throw New Exception("ReadAsStringAsync Exception!!" & vbCrLf & ex.Message)
                    'MsgBox(ex.Message, Title:="Handle CallREST Response Exception")
                End Try
            Else
                'MsgBox(response.StatusCode & " (" & response.ReasonPhrase & ")", Title:="CallREST Failed!")
                Throw New Exception("SendAsync Error!! " & vbCrLf & response.StatusCode & " (" & response.ReasonPhrase & ")")
            End If
        Catch ex As Exception
            'MsgBox(ex.Message, Title:="CallREST Exception")
            Throw New Exception("CallREST Exception!! " & vbCrLf & ex.Message)
        End Try

        Return jsonstring
    End Function

    Public Function CallREST2(ByVal method As String, ByVal url As String, Optional body As String = "",
                              Optional headers As Dictionary(Of String, String) = Nothing) As String
        Dim jsonstring As String = ""
        Dim client As HttpClient = New HttpClient()
        Dim response As HttpResponseMessage = New HttpResponseMessage()

        Try
            client.BaseAddress = New Uri(url)
            client.DefaultRequestHeaders.Accept.Clear()
            client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json")
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " & ThisAddIn.accessToken)
            client.Timeout = New TimeSpan(0, 0, 120)

            If headers IsNot Nothing Then
                Dim keys As Dictionary(Of String, String).KeyCollection = headers.Keys
                For Each key As String In keys
                    Dim value As String = headers(key)
                    client.DefaultRequestHeaders.TryAddWithoutValidation(key, value)
                Next
            End If
        Catch ex As Exception
            Throw New Exception("CallREST2 Preperation Exception!! " & vbCrLf & ex.Message)
        End Try

        Try
            If method = "GET" Then
                response = client.GetAsync(url).Result()
            ElseIf method = "POST" Or method = "PATCH" Then
                'Dim content As StringContent = New StringContent(body, Encoding.UTF8, "application/json")
                Dim bytearray As Byte() = Encoding.UTF8.GetBytes(body)
                Dim content As ByteArrayContent = New ByteArrayContent(bytearray)

                content.Headers.ContentType = New Headers.MediaTypeHeaderValue("application/json")

                If method = "PATCH" Then url = url & "?_HttpMethod=PATCH"
                response = client.PostAsync(url, content).Result()
            ElseIf method = "DELETE" Then
                response = client.DeleteAsync(url).Result()
            End If

            If response.IsSuccessStatusCode Then

                Try
                    jsonstring = response.Content.ReadAsStringAsync().Result()
                Catch ex As Exception
                    Throw New Exception("ReadAsStringAsync Exception!!" & vbCrLf & ex.Message)
                    'MsgBox(ex.Message, Title:="Handle Response Exception")
                End Try
            Else
                Throw New Exception("SendAsync Error!! " & vbCrLf & response.StatusCode & " (" & response.ReasonPhrase & ")")
                'MsgBox(response.StatusCode & " (" & response.ReasonPhrase & ")", Title:="CallREST2 Failed!")
            End If

            'Console.ReadKey()
        Catch ex As Exception
            'MsgBox(ex.Message, Title:="CallREST Exception")
            Throw New Exception("CallREST2 Exception!! " & vbCrLf & ex.Message)
        End Try

        Return jsonstring
    End Function

End Module


Public Class RESTful
    ' RESTful
    ' Helper class for interfacing by RESTful API
    ' @Created By : MinGyoon Woo, 2018-06-05
    ' @Modified By : MinGyoon Woo, 2020-08-25

    Public Class Attributes
        Public Property type() As String
        Public Property url() As String

        Public Sub New(ByVal type As String, Optional url As String = "")
            Me.type = type
            If url IsNot "" Then Me.url = url
        End Sub
    End Class

    Public Class ActionOverride
        Public Property formFactor() As String
        Public Property isAvailableInTouch() As Boolean
        Public Property name() As String
        Public Property pageId() As String
        Public Property url() As String
    End Class

    Public Class ChildRelationship
        Public Property cascadeDelete() As Boolean
        Public Property childSObject() As String
        Public Property deprecatedAndHidden() As Boolean
        Public Property field() As String
        Public Property junctionIdListNames() As String()
        Public Property junctionReferenceTo() As String()
        Public Property relationshipName() As String
        Public Property restrictedDelete() As Boolean
    End Class

    Public Class DeleteResult
        Public Property errors As SalesforceError()
        Public Property id As String
        Public Property success As Boolean
    End Class

    Public Class DescribeGlobalResult
        Public Property encoding() As String
        Public Property maxBatchSize() As Integer
        Public Property sobjects() As DescribeGlobalSObjectResult()
    End Class

    Public Class DescribeGlobalSObjectResult
        Public Property activateable() As Boolean
        Public Property custom() As Boolean
        Public Property customSetting() As Boolean
        Public Property createable() As Boolean
        Public Property deletable() As Boolean
        Public Property deprecatedAndHidden() As Boolean
        Public Property feedEnabled() As Boolean
        Public Property keyPrefix() As String
        Public Property label() As String
        Public Property labelPlural() As String
        Public Property layoutable() As Boolean
        Public Property mergeable() As Boolean
        Public Property mruEnabled() As Boolean
        Public Property name() As String
        Public Property queryable() As Boolean
        Public Property replicateable() As Boolean
        Public Property retrieveable() As Boolean
        Public Property searchable() As Boolean
        Public Property triggerable() As Boolean
        Public Property undeletable() As Boolean
        Public Property updateable() As Boolean
        Public Property urls() As ServiceUrl
    End Class

    Public Class DescribeSObjectResult
        Public Property actionOverrides() As ActionOverride()
        Public Property activateable() As Boolean
        Public Property childRelationships() As ChildRelationship()
        Public Property compactLayoutable() As Boolean
        Public Property createable() As Boolean
        Public Property custom() As Boolean
        Public Property customSetting() As Boolean
        Public Property deepCloneable() As Boolean
        Public Property defaultImplementation() As String
        Public Property deletable() As Boolean
        Public Property deprecatedAndHidden() As Boolean
        Public Property extendedBy() As String
        Public Property extendsInterfaces() As String
        Public Property feedEnabled() As Boolean
        Public Property fields() As Field()
        Public Property hasSubtypes() As Boolean
        Public Property implementedBy() As String
        Public Property implementsInterfaces() As String
        Public Property isInterface() As Boolean
        Public Property isSubtype() As Boolean
        Public Property keyPrefix() As String
        Public Property label() As String
        Public Property labelPlural() As String
        Public Property layoutable() As Boolean
        Public Property listviewable() As Object
        Public Property lookupLayoutable() As Object
        Public Property mergeable() As Boolean
        Public Property mruEnabled() As Boolean
        Public Property name() As String
        Public Property namedLayoutInfos As NamedLayoutInfo()
        Public Property networkScopeFieldName As String
        Public Property queryable() As Boolean
        Public Property recordTypeInfos As RecordTypeInfo()
        Public Property replicateable() As Boolean
        Public Property retrieveable() As Boolean
        Public Property searchable() As Boolean
        Public Property sobjectDescribeOption() As String
        Public Property supportedScopes() As ScopeInfo()
        Public Property triggerable() As Boolean
        Public Property undeletable() As Boolean
        Public Property updateable() As Boolean
        Public Property urls() As ServiceUrl
    End Class

    Public Class Field
        Public Property aggregatable() As Boolean
        Public Property aiPredictionField() As Boolean
        Public Property autoNumber() As Boolean
        Public Property byteLength() As Integer
        Public Property calculated() As Boolean
        Public Property calculatedFormula() As String
        Public Property cascadeDelete() As Boolean
        Public Property caseSensitive() As Boolean
        Public Property compoundFieldName() As String
        Public Property controllerName() As String
        Public Property createable() As Boolean
        Public Property custom() As Boolean
        Public Property defaultValue() As Object
        Public Property defaultValueFormula() As String
        Public Property defaultedOnCreate() As Boolean
        Public Property dependentPicklist() As Boolean
        Public Property deprecatedAndHidden() As Boolean
        Public Property digits() As Integer
        Public Property displayLocationInDecimal() As Boolean
        Public Property encrypted() As Boolean
        Public Property externalId() As Boolean
        Public Property extraTypeInfo() As String
        Public Property filterabl() As Boolean
        Public Property filteredLookupInfo() As FilteredLookupInfo
        Public Property formulaTreatNullNumberAsZero() As Boolean
        Public Property groupable() As Boolean
        Public Property highScaleNumber() As Boolean
        Public Property htmlFormatted() As Boolean
        Public Property idLookup() As Boolean
        Public Property inlineHelpText() As String
        Public Property label() As String
        Public Property length() As Integer
        Public Property mask() As String
        Public Property maskType() As String
        Public Property name() As String
        Public Property nameField() As Boolean
        Public Property namePointing() As Boolean
        Public Property nillable() As Boolean
        Public Property permissionable() As Boolean
        Public Property picklistValues() As PicklistEntry()
        Public Property polymorphicForeignKey() As Boolean
        Public Property precision() As Integer
        Public Property queryByDistance() As Boolean
        Public Property referenceTargetField() As String
        Public Property referenceTo() As String()
        Public Property relationshipName() As String
        Public Property relationshipOrder() As Object ' Real type is Integer, but Integer does not accept NULL value.
        Public Property restrictedDelete() As Boolean
        Public Property restrictedPicklist() As Boolean
        Public Property scale() As Integer
        Public Property searchPrefilterable() As Boolean
        Public Property soapType() As String
        Public Property sortable() As Boolean
        Public Property type() As String
        Public Property unique() As Boolean
        Public Property updateable() As Boolean
        Public Property writeRequiresMasterRead() As Boolean
    End Class

    Public Class FilteredLookupInfo
        Public Property controllingFields() As String()
        Public Property dependent() As Boolean
        Public Property optionalFilter() As Boolean
    End Class

    Public Class NamedLayoutInfo
        Public Property name() As String
    End Class

    Public Class PicklistEntry
        Public Property active() As Boolean
        Public Property defaultValue() As Boolean
        Public Property label() As String
        Public Property validFor() As String
        Public Property value() As String
    End Class

    Public Class QueryResult
        Public Property done() As Boolean
        Public Property nextRecordsUrl() As String
        Public Property records() As Object()
        Public Property totalSize() As Integer
    End Class

    Public Class RecordSet
        Public Property allOrNone() As Boolean
        Public Property records() As Object()
    End Class

    Public Class RecordTypeInfo
        Public Property active() As Boolean
        Public Property available() As Boolean
        Public Property defaultRecordTypeMapping() As Boolean
        Public Property developerName() As String
        Public Property master() As Boolean
        Public Property name() As String
        Public Property recordTypeId() As String
        Public Property urls() As Dictionary(Of String, String)
    End Class

    Public Class SaveResult
        Public Property errors As SalesforceError()
        Public Property id As String
        Public Property success As Boolean
    End Class

    Public Class ScopeInfo
        Public Property label() As String
        Public Property name() As String
    End Class

    Public Class UpsertResult
        Public Property created As Boolean
        Public Property errors As SalesforceError()
        Public Property id As String
        Public Property success As Boolean
    End Class

    ''' <summary>
    ''' RESTful Only Class
    ''' </summary>
    ''' 
    Public Class ConnectionInfo
        Public Property id() As String
        Public Property asserted_user() As Boolean
        Public Property user_id() As String
        Public Property organization_id() As String
        Public Property username() As String
        Public Property nick_name() As String
        Public Property display_name() As String
        Public Property email() As String
        Public Property email_verified() As Boolean
        Public Property first_name() As String
        Public Property last_name() As String
        Public Property addr_street() As String
        Public Property addr_city() As String
        Public Property addr_state() As String
        Public Property addr_country() As String
        Public Property addr_zip() As String
        Public Property mobile_phone() As String
        Public Property mobile_phone_verified() As Boolean
        Public Property is_lighting_login_user() As Boolean
        Public Property active() As Boolean
        Public Property user_type() As String
        Public Property timezone() As String
        Public Property language() As String
        Public Property locale() As String
        Public Property utcOffset() As String
        Public Property last_modified_date() As String
        Public Property is_app_installed() As Boolean
        Public Property status() As ConnectionStatus
        Public Property photos() As PhotoInfo
        Public Property urls() As UrlInfo
    End Class

    Public Class ConnectionStatus
        Public Property created_date() As String
        Public Property body() As String
    End Class

    Public Class ObjectType
        Public Property type() As String

        Public Sub New(ByVal type As String)
            Me.type = type
        End Sub
    End Class

    Public Class PhotoInfo
        Public Property picture() As String
        Public Property thumbnail() As String

    End Class

    Public Class SalesforceError
        Public Property fields As String()
        Public Property message As String
        Public Property statusCode As Object
    End Class

    Public Class ServiceUrl
        Public Property compactLayouts() As String
        Public Property approvalLayouts() As String
        Public Property uiDetailTemplate() As String
        Public Property uiEditTemplate() As String
        Public Property defaultValues() As String
        Public Property listviews() As String
        Public Property uiNewRecord() As String
        Public Property quickActions() As String
        Public Property layouts() As String
        Public Property sobject() As String
        Public Property describe() As String
        Public Property rowTemplate() As String
    End Class

    Public Class UrlInfo
        Public Property enterprise() As String
        Public Property metadata() As String
        Public Property partner() As String
        Public Property rest() As String
        Public Property sobjects() As String
        Public Property search() As String
        Public Property query() As String
        Public Property recent() As String
        Public Property profile() As String
        Public Property feeds() As String
        Public Property groups() As String
        Public Property users() As String
        Public Property feed_items() As String
        Public Property feed_elements() As String
        Public Property tooling_soap() As String
        Public Property tooling_rest() As String
        Public Property custom_domain() As String
    End Class
End Class

