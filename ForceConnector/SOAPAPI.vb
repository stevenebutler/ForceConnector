Option Explicit On
Option Strict Off

Imports ForceConnector.Partner

Module SOAPAPI

    Dim soapClient As Partner.SoapClient
    Dim soapSessionHeader As Partner.SessionHeader
    Dim callOptions As Partner.CallOptions
    Dim packageVersions() As Partner.PackageVersion

    Dim metaClient As MiniMETA.MetadataPortTypeClient
    Dim metaSessionHeader As MiniMETA.SessionHeader
    Dim allOrNoneHeader As MiniMETA.AllOrNoneHeader

    Public Function getSObjectList() As Partner.DescribeGlobalSObjectResult()
        Dim dgr As Partner.DescribeGlobalResult = New DescribeGlobalResult()
        Dim limitInfo() As Partner.LimitInfo

        Try
            If setSoapBinding() Then
                limitInfo = soapClient.describeGlobal(soapSessionHeader, callOptions, packageVersions, dgr)
                Return dgr.sobjects
            End If

        Catch ex As Exception
            Throw New Exception("getSObjectList Exception!" & vbCrLf & ex.Message)
        End Try
        Return Nothing
    End Function

    Public Function DescribeSObject(ByVal objname As String, ByVal baseLang As String) As Partner.DescribeSObjectResult
        Dim dsr As Partner.DescribeSObjectResult = New DescribeSObjectResult()
        Dim limitInfo() As Partner.LimitInfo
        Try
            If setSoapBinding() Then
                Dim localeOptions As Partner.LocaleOptions = New Partner.LocaleOptions()
                localeOptions.language = baseLang
                limitInfo = soapClient.describeSObject(soapSessionHeader, callOptions, packageVersions, localeOptions, objname, dsr)
                Return dsr
            End If
        Catch ex As Exception
            Throw New Exception("getSObjectList Exception!" & vbCrLf & ex.Message)
        End Try
        Return Nothing
    End Function




    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '' Common Functions Block
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    Function setSoapBinding() As Boolean
        If Not checkSession() Then
            If Not LoginToSalesforce() Then GoTo done
        End If

        If checkSession() Then
            If soapClient Is Nothing Then
                soapClient = ThisAddIn.soapClient
            End If
            If soapSessionHeader Is Nothing Then
                soapSessionHeader = ThisAddIn.soapSessionHeader
            End If
            callOptions = New Partner.CallOptions
            packageVersions = {New Partner.PackageVersion()}
            Return True
        End If
done:
        Return False
    End Function


End Module
