Imports System.Collections.Generic
Imports System.Web

Public NotInheritable Class CookieUtil
    Private Sub New()
    End Sub

    Public Shared Function GetCookieValue(ByVal cookieName As String) As String
        Dim cookieVal As String = String.Empty
        cookieVal = HttpContext.Current.Request.Cookies(cookieName).Value
        Return cookieVal
    End Function

    Public Shared Sub CreateCookie(ByVal cookieName As String, ByVal value As String, ByVal expirationDays As System.Nullable(Of Integer))
        Dim Cookie As New HttpCookie(cookieName, value)
        If expirationDays.HasValue Then
            Cookie.Expires = DateTime.Now.AddDays(expirationDays.Value)
        End If
        HttpContext.Current.Response.Cookies.Add(Cookie)
    End Sub

    Public Shared Sub DeleteCookie(ByVal cookieName As String)
        Dim Cookie As HttpCookie = HttpContext.Current.Request.Cookies(cookieName)
        If Cookie IsNot Nothing Then
            Cookie.Expires = DateTime.Now.AddDays(-2)
            HttpContext.Current.Response.Cookies.Add(Cookie)
        End If
    End Sub

    Public Shared Function CookieExists(ByVal cookieName As String) As Boolean
        Dim exists As Boolean = False
        Dim cookie As HttpCookie = HttpContext.Current.Request.Cookies(cookieName)
        If cookie IsNot Nothing Then
            exists = True
        End If
        Return exists
    End Function

    Public Shared Function GetAllCookies() As Dictionary(Of String, String)
        Dim cookies As New Dictionary(Of String, String)()
        For Each key As String In HttpContext.Current.Request.Cookies.AllKeys
            cookies.Add(key, HttpContext.Current.Request.Cookies(key).Value)
        Next
        Return cookies
    End Function

    Public Shared Sub DeleteAllCookies()
        Dim aCookie As HttpCookie
        Dim i As Integer
        Dim cookieName As String
        Dim limit As Integer = HttpContext.Current.Request.Cookies.Count - 1
        For i = 0 To limit
            cookieName = HttpContext.Current.Request.Cookies(i).Name
            DeleteCookie(cookieName)
        Next
    End Sub


    Public Shared Sub SetEncryptedCookie(ByVal key As String, _
           ByVal value As String)
        'Convert parts
        'key = CryptoUtil.Encrypt(key)
        value = CryptoUtil.Encrypt(value)

        SetCookie(key, value)
    End Sub


    'SetCookie - key & value only
    Public Shared Sub SetCookie(ByVal key As String, ByVal value As String)
        'Encode Part
        key = HttpContext.Current.Server.UrlEncode(key)
        value = HttpContext.Current.Server.UrlEncode(value)

        Dim cookie As HttpCookie
        cookie = New HttpCookie(key, value)
        SetCookie(cookie)
    End Sub

    'SetCookie - HttpCookie only
    'final step to set the cookie to the response clause
    Public Shared Sub SetCookie(ByVal cookie As HttpCookie)
        HttpContext.Current.Response.Cookies.Set(cookie)
    End Sub

    'GET COOKIE FUNCTIONS     

    Public Shared Function GetEncryptedCookieValue(ByVal key As String) As String
        'get value 
        Dim value As String
        value = GetCookieValue_session(key)
        'decrypt value
        value = CryptoUtil.Decrypt(value)
        Return value
    End Function

    Public Shared Function GetCookie(ByVal key As String) As HttpCookie
        'encode key for retrieval
        key = HttpContext.Current.Server.UrlEncode(key)
        Return HttpContext.Current.Request.Cookies.Get(key)
    End Function

    Public Shared Function GetCookieValue_session(ByVal key As String) As String
        Try
            'get value 
            Dim value As String
            value = GetCookie(key).Value
            'decode stored value
            value = HttpContext.Current.Server.UrlDecode(value)
            Return value
        Catch
        End Try
    End Function
End Class

