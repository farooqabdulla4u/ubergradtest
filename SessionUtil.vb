Option Compare Text
Imports Microsoft.VisualBasic

Public Class SessionUtil
    Public Shared Function getSession(ByVal name As String) As String
        Dim ckvalue As String = "", values(10) As String, returnvalue As String = ""
        Try
            ckvalue = CookieUtil.GetEncryptedCookieValue("gyaane")

            If String.IsNullOrEmpty(ckvalue) = False Then
                values = ckvalue.Split(";")
                If values.Length < 1 OrElse CInt(values(1)) <= 0 Then Return ""
            End If

            Select Case name
                Case "user_id"
                    returnvalue = values(1)
                Case "user_name"
                    returnvalue = values(2)
                Case "user_email"
                    returnvalue = values(3)
                Case "login_type"
                    returnvalue = values(4)
            End Select
			'Just to test changes on the github 
            Return returnvalue
        Catch ex As Exception
            Return ex.Message
        Finally
            values = Nothing
        End Try
    End Function

    Public Shared Sub removecookie(ByVal id As String)
        Dim rCookie As New HttpCookie(id)
        rCookie.Value = ""
        rCookie.Expires = DateTime.Now.AddDays(-1)
        CookieUtil.SetCookie(rCookie)
    End Sub
End Class
