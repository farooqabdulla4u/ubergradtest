Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports Microsoft.SqlServer.Server
Imports DevOne.Security.Cryptography.BCrypt



Partial Public Class userpwd_hash    
    Public Shared Function userpwd_hash(ByVal user_pwd As String) As String
        Dim hashpwd As String = ""
        Dim salt As String = BCryptHelper.GenerateSalt()
        ' hashpwd = PasswordHash.CreateHash(user_pwd.ToString(), salt.ToString())
        hashpwd = BCryptHelper.HashPassword(user_pwd, salt)
        Return hashpwd
    End Function
    Public Shared Function validatepwd(ByVal userpwd As String, ByVal correctHash As String) As String
        Dim result As String = BCryptHelper.CheckPassword(userpwd, correctHash)
        Return result
    End Function

End Class
