Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts

Public Class SmsApi
    ' Private objProxy1 As WebProxy = Nothing

    Public Shared Function SendSMS(ByVal User As String, ByVal password As String, ByVal Mobile_Number As String, ByVal Message As String, Optional ByVal SID As String = "UbrGrd", Optional ByVal MType As String = "N", Optional ByVal DR As String = "N") As String

        Dim stringpost As System.Object = "User=" & User & "&passwd=" & password & "&mobilenumber=" & Mobile_Number & "&message=" & Message & "&SID=" & SID & "&MTYPE=" & MType & "&DR=" & DR

        Dim functionReturnValue As String = Nothing
        functionReturnValue = ""

        Dim objWebRequest As HttpWebRequest = Nothing
        Dim objWebResponse As HttpWebResponse = Nothing
        Dim objStreamWriter As StreamWriter = Nothing
        Dim objStreamReader As StreamReader = Nothing

        Try
            Dim stringResult As String = Nothing

            objWebRequest = DirectCast(WebRequest.Create("http://www.smscountry.com/SMSCwebservice_bulk.aspx?"), HttpWebRequest)
            objWebRequest.Method = "POST"

            REM If (objProxy1 IsNot Nothing) Then
            REM objWebRequest.Proxy = objProxy1
            REM End If

            ' Use below code if you want to SETUP PROXY. 
            'Parameters to pass: 1. ProxyAddress 2. Port 
            'You can find both the parameters in Connection settings of your internet explorer.

            'Dim myProxy As New WebProxy("YOUR PROXY", PROXPORT)
            'myProxy.BypassProxyOnLocal = True
            'wrGETURL.Proxy = myProxy

            objWebRequest.ContentType = "application/x-www-form-urlencoded"

            objStreamWriter = New StreamWriter(objWebRequest.GetRequestStream())
            objStreamWriter.Write(stringpost)
            objStreamWriter.Flush()
            objStreamWriter.Close()

            objWebResponse = DirectCast(objWebRequest.GetResponse(), HttpWebResponse)
            objStreamReader = New StreamReader(objWebResponse.GetResponseStream())

            stringResult = objStreamReader.ReadToEnd()

            objStreamReader.Close()
            Return (stringResult)
        Catch ex As Exception
            Return (ex.Message)
        Finally
            If (objStreamWriter IsNot Nothing) Then
                objStreamWriter.Close()
            End If
            If (objStreamReader IsNot Nothing) Then
                objStreamReader.Close()
            End If
            objWebRequest = Nothing
            objWebResponse = Nothing
            ' objProxy1 = Nothing
        End Try
    End Function


    Sub main()
        Dim obj As New SmsApi
        ' Dim strPostResponse As String = obj.SendSMS("UserName", "Password", "919XXXXXXXXX", "Message", )
        'Response.Write("Server Response " & strPostResponse)
    End Sub
End Class


