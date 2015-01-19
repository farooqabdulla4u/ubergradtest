'Imports Microsoft.VisualBasic
'Imports System.Data
'Imports System.Data.SqlClient


'Public Class UniversityClass

'    Public Shared ConStr As String = ConfigurationManager.ConnectionStrings("GyaaneDbString").ConnectionString
'    Public Shared Function University_SignUp(ByVal name As String, ByVal email As String, ByVal weburl As String) As String
'        Dim sqlcmd As New SqlCommand, sqlcon As New SqlConnection(ConStr), sqldata As New SqlDataAdapter, ds As New DataSet, items As String = ""
'        Dim randomclass As New Random()        
'        Try
'            sqlcmd = New SqlCommand("University_SignUp", sqlcon)
'            With sqlcmd
'                .CommandType = CommandType.StoredProcedure

'                .Parameters.Add("@university_name", SqlDbType.VarChar, 255).Direction = ParameterDirection.Input
'                .Parameters("@university_name").Value = name

'                .Parameters.Add("@university_email", SqlDbType.VarChar, 255).Direction = ParameterDirection.Input
'                .Parameters("@university_email").Value = email

'                .Parameters.Add("@university_weburl", SqlDbType.VarChar, 255).Direction = ParameterDirection.Input
'                .Parameters("@university_weburl").Value = weburl

'                .Parameters.Add("@university_password", SqlDbType.VarChar, 255).Direction = ParameterDirection.Input
'                .Parameters("@university_password").Value = userpwd_hash.userpwd_hash("ubergrad", 12345678)

'                .Parameters.Add("@ret_msg", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output

'                sqlcon.Open()
'                .ExecuteNonQuery()
'                sqlcon.Close()

'                items = "{""ret_msg"":""" & .Parameters("@ret_msg").Value & """}"
'            End With
'        Catch ex As Exception
'            items = "{""ret_msg"":""" & ex.Message & """}"
'        End Try
'        Return items
'    End Function
'    Public Shared Function LoginCheck(ByVal email As String, ByVal password As String) As String
'        Dim ret_str As String = "", sessionstr As String = "", sessiontype As String = "website_login", password_check As String = ""
'        Dim sqlcmd As New SqlCommand, sqlcon As New SqlConnection(ConStr)
'        Dim randomclass As New Random()
'        ' Dim hashpwd As String = ""
'        ' hashpwd = userpwd_hash.validatepwd("naresh", "1000:vDZBttI8aYE=:G1DFFeROltX7RSHGEaCMXWvQyXWF079K")
'        'Return hashpwd
'        'hashpwd = PasswordHash.CreateHash("naresh".ToString(), salt.ToString())
'        Try
'            sqlcmd = New SqlCommand("Login_Check", sqlcon)
'            With sqlcmd
'                .CommandType = CommandType.StoredProcedure

'                .Parameters.Add("@Username", SqlDbType.VarChar, 500).Direction = ParameterDirection.Input
'                .Parameters("@Username").Value = email

'                .Parameters.Add("@Password_Str", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output                

'                .Parameters.Add("@Ret_Val", SqlDbType.Int).Direction = ParameterDirection.Output
'                .Parameters.Add("@Ret_Message", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
'                .Parameters.Add("@u_id", SqlDbType.Int).Direction = ParameterDirection.Output
'				 .Parameters.Add("@logo", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output
'                .Parameters.Add("@u_name", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output

'                sqlcon.Open()
'                .ExecuteNonQuery()
'            End With
'            If sqlcmd.Parameters("@Ret_Message").Value = "OK" Then                
'                If userpwd_hash.validatepwd(password, sqlcmd.Parameters("@Password_Str").Value) Then                
'                    sessionstr = Now.TimeOfDay.ToString() & ";"
'                    sessionstr = sessionstr & sqlcmd.Parameters("@u_id").Value & ";" & sqlcmd.Parameters("@u_name").Value & ";" & email & ";" & sessiontype & ";" & sqlcmd.Parameters("@logo").Value & ""
'                    CookieUtil.SetEncryptedCookie("gyaane_id", sessionstr)
'                    password_check = "success"
'                Else
'                    password_check = "fail"
'                    sqlcmd.Parameters("@Ret_Message").Value = "Wrong Password"
'                End If
'            End If
'            ret_str = "{""ret_val"":""" & sqlcmd.Parameters("@Ret_Val").Value & """, ""ret_msg"":""" & sqlcmd.Parameters("@Ret_Message").Value & """, ""password_check"":""" & password_check & """}"
'            'If sqlcmd.Parameters("@Ret_Val").Value = 0 Then
'            'End If
'        Catch ex As Exception
'            ret_str = "{""ret_val"":""3"", ""ret_msg"":""" & ex.Message & """}"
'        Finally
'            sqlcon.Close()
'            sqlcmd = Nothing
'        End Try
'        Return ret_str
'    End Function
'End Class
