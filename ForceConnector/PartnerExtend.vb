Option Explicit On
Option Strict Off

Imports System.Web.Script.Serialization
Imports System.Xml

Namespace Partner
    Partial Public Class sObject
        Public Function getObject() As Object
            Dim fields As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
            For Each field As XmlElement In anyField
                If field.Attributes.Count = 0 Then
                    fields.Add(field.LocalName, field.InnerText)
                Else
                    fields.Add(field.LocalName, getObject(field.ChildNodes))
                End If
            Next
            Return fields
        End Function

        Private Function getObject(ByVal childList As XmlNodeList) As Object
            Dim childs As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
            For Each child As XmlNode In childList
                If child.Attributes.Count = 0 Then
                    childs.Add(child.LocalName, child.InnerText)
                Else
                    childs.Add(child.LocalName, getObject(child.ChildNodes))
                End If
            Next
            Return childs
        End Function

        Public Function setObject(ByVal objname As String, ByVal entity As Dictionary(Of String, Object)) As sObject
            Dim obj As sObject = New sObject()
            obj.type = objname
            Dim fields(entity.Count) As XmlElement
            Dim doc As XmlDocument = New XmlDocument()
            For i As Integer = 0 To entity.Count Step 1
                Dim key As String = entity.Keys(i)
                fields(i) = doc.CreateElement(key)
                fields(i).InnerText = entity.Item(key).ToString
            Next
            obj.anyField = fields
            Return obj
        End Function

        Public Function getField(ByVal fieldName As String) As Object
            For i As Integer = 0 To anyField.Length Step 1
                Dim entity As XmlElement = anyField(i)
                If entity.LocalName.ToLower = fieldName.ToLower Then
                    If entity.Attributes.Count = 0 Then
                        Return entity.InnerText
                    Else
                        Return convert(entity.ChildNodes)
                    End If
                End If
            Next
            Return Nothing
        End Function

        Private Function convert(ByVal entity As XmlNodeList) As sObject
            Dim obj As sObject = New sObject()
            obj.type = entity(0).InnerText
            Dim fields(entity.Count - 1) As XmlElement
            Dim doc As XmlDocument = New XmlDocument()
            For i As Integer = 1 To entity.Count Step 1
                fields(i - 1) = doc.CreateElement(entity(i).LocalName)
                fields(i - 1).InnerText = entity(i).InnerText
                fields(i - 1).Prefix = entity(i).Prefix
            Next
            obj.anyField = fields
            Return obj
        End Function
    End Class
End Namespace
