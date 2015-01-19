Imports System
Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.IO
Imports System.Diagnostics
Imports System.Configuration
Imports System.Data
Imports DevOne.Security.Cryptography.BCrypt
Imports System.Text
Imports System.Text.RegularExpressions

Public Class user_class
    Public Shared constr As String = ConfigurationManager.ConnectionStrings("GyaaneDbString").ConnectionString
    Public Shared Function login_check_merge(ByVal email As String, ByVal password As String, ByVal mobile_num As String) As String
        Dim ret_str As String = "", sessionstr As String = "", sessiontype As String = "normal", password_check As String = "", confirm_status As String = ""
        Dim sqlcmd As New SqlCommand
        Dim sqlcon As New SqlConnection(constr)
        Dim ret_val As Integer, password_str As String, user_id As Integer, user_name, mobile As String
        mobile = ""
        Try
            sqlcmd = New SqlCommand("User_Login_Check_merge", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@email", SqlDbType.VarChar, 500)
                .Parameters("@email").Value = email

                .Parameters.Add("@password", SqlDbType.VarChar, 500)
                .Parameters("@password").Value = password

                .Parameters.Add("@mobile_num", SqlDbType.VarChar, 500)
                .Parameters("@mobile_num").Value = mobile_num

                .Parameters.Add("@Password_Str", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output
                .Parameters.Add("@Ret_Val", SqlDbType.Int).Direction = ParameterDirection.Output
                .Parameters.Add("@user_id", SqlDbType.Int).Direction = ParameterDirection.Output
                .Parameters.Add("@user_name", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output
                .Parameters.Add("@mobile", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output

                sqlcon.Open()
                .ExecuteNonQuery()
                sqlcon.Close()
                ret_val = .Parameters("@Ret_Val").Value
                If ret_val <> 0 Then
                    password_str = (.Parameters("@Password_Str").Value).ToString()
                    user_id = .Parameters("@user_id").Value
                    user_name = .Parameters("@user_name").Value
                    mobile = sqlcmd.Parameters("@mobile").Value
                End If
            End With
			
            If ret_val = 1 Then
                If userpwd_hash.validatepwd(password, password_str) Then
                    sqlcmd = New SqlCommand("update_status_mobile", sqlcon)
                    With sqlcmd
                        .CommandType = CommandType.StoredProcedure
                        .Parameters.Add("@user_id", SqlDbType.VarChar, 500)
                        .Parameters("@user_id").Value = user_id
                        sqlcon.Open()
                        .ExecuteNonQuery()
                        sqlcon.Close()
                    End With
                    sessionstr = Now.TimeOfDay.ToString() & ";"
                    sessionstr = sessionstr & user_id & ";" & user_name & ";" & email & ";" & sessiontype & ""
                    CookieUtil.SetEncryptedCookie("gyaane", sessionstr)
                    password_check = "success"
                Else
                    password_check = "Invalid password"
                End If
            ElseIf ret_val = 0 Then
                password_check = "Invalid email address"
            ElseIf ret_val = 2 Then
                password_check = "success"
                confirm_status = "no"
            ElseIf ret_val = 4 Then
                Try
                    Dim password1 As String = GetPassword()
                    sqlcmd = New SqlCommand("diff_account_mobile_Password", sqlcon)
                    With sqlcmd
                        .CommandType = CommandType.StoredProcedure

                        .Parameters.Add("@mobile", SqlDbType.VarChar, 15).Direction = ParameterDirection.Input
                        .Parameters("@mobile").Value = mobile
                        .Parameters.Add("@password", SqlDbType.VarChar, 250)
                        .Parameters("@password").Value = userpwd_hash.userpwd_hash(password1)
                        .Parameters.Add("@ret_msg", SqlDbType.VarChar, 10).Direction = ParameterDirection.Output

                        sqlcon.Open()
                        .ExecuteNonQuery()
                        sqlcon.Close()
                    End With
                    If sqlcmd.Parameters("@ret_msg").Value = "ok" Then
                        SmsApi.SendSMS("gyaane", "gyaane123", "91" & mobile & "", "Your OTP password is " & password1 & " and You can login with your Mobile Number.", "", "", "")
                        password_check = "We sent a Password to your mobile."
                        confirm_status = "yes"
                    End If
                Catch ex As Exception
                Finally
                    sqlcmd = Nothing
                End Try
            End If
            ret_str = "{""ret_val"":""" & ret_val & """, ""password_check"":""" & password_check & """, ""confirm_status"":""" & confirm_status & """,""mobile"":""" & mobile & """}"
        Catch ex As Exception
            ret_str = "{""ret_val"":""4"", ""ret_msg"":""" & ex.ToString() & """}"
        Finally
            sqlcon.Close()
            sqlcmd = Nothing
        End Try
        Return ret_str
    End Function

    Public Shared Function login_check(ByVal email As String, ByVal password As String) As String
        Dim ret_str As String = "", sessionstr As String = "", sessiontype As String = "normal", password_check As String = "", confirm_status As String = ""
        Dim sqlcmd As New SqlCommand
        Dim sqlcon As New SqlConnection(constr)
        Dim ret_val As Integer, password_str As String, user_id As Integer, user_name As String
        Try
            sqlcmd = New SqlCommand("User_Login_Check_naresh", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@email", SqlDbType.VarChar, 500)
                .Parameters("@email").Value = email

                .Parameters.Add("@Password_Str", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output
                .Parameters.Add("@Ret_Val", SqlDbType.Int).Direction = ParameterDirection.Output
                .Parameters.Add("@user_id", SqlDbType.Int).Direction = ParameterDirection.Output
                .Parameters.Add("@user_name", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output
                .Parameters.Add("@mobile", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output

                sqlcon.Open()
                .ExecuteNonQuery()
                ret_val = .Parameters("@Ret_Val").Value
                password_str = (.Parameters("@Password_Str").Value).ToString()
                user_id = .Parameters("@user_id").Value
                user_name = .Parameters("@user_name").Value
            End With
            If ret_val = 1 Then
                If userpwd_hash.validatepwd(password, password_str) Then
                    sessionstr = Now.TimeOfDay.ToString() & ";"
                    sessionstr = sessionstr & user_id & ";" & user_name & ";" & email & ";" & sessiontype & ""
                    CookieUtil.SetEncryptedCookie("gyaane", sessionstr)
                    password_check = "success"
                Else
                    password_check = "Invalid password"
                End If
            ElseIf ret_val = 0 Then
                password_check = "Invalid email address"
            ElseIf ret_val = 2 Then
                password_check = "success"
                confirm_status = "no"
            End If
            ret_str = "{""ret_val"":""" & ret_val & """, ""password_check"":""" & password_check & """, ""confirm_status"":""" & confirm_status & """,""mobile"":""" & sqlcmd.Parameters("@mobile").Value & """}"
        Catch ex As Exception
            ret_str = "{""ret_val"":""3"", ""ret_msg"":""" & ex.ToString() & """}"
        Finally
            sqlcon.Close()
            sqlcmd = Nothing
        End Try
        Return ret_str
    End Function
    Public Shared Function Forgot_Password_Change(ByVal pwd As String, ByVal unique_id As String) As String
        Dim sqlcmd As New SqlCommand, con As New SqlConnection(constr), res As String = ""

        Try
            sqlcmd = New SqlCommand("Forgot_Password", con)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@unique_id", SqlDbType.VarChar, 100).Direction = ParameterDirection.Input
                .Parameters("@unique_id").Value = unique_id

                .Parameters.Add("@password", SqlDbType.VarChar, 255).Direction = ParameterDirection.Input
                .Parameters("@password").Value = userpwd_hash.userpwd_hash(pwd)

                .Parameters.Add("@ret_msg", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output

            End With
            con.Open()
            sqlcmd.ExecuteNonQuery()
            con.Close()
            res = sqlcmd.Parameters("@ret_msg").Value
        Catch ex As Exception
            res = ex.Message
        Finally
            sqlcmd = Nothing
        End Try
        Return res
    End Function
    Public Shared Function login_check_mobile(ByVal mobile As String, ByVal password As String) As String
        Dim ret_str As String = "", sessionstr As String = "", sessiontype As String = "normal", password_check As String = "", confirm_status As String = ""
        Dim sqlcmd As New SqlCommand
        Dim sqlcon As New SqlConnection(constr)
        Dim ret_val As Integer, password_str As String, user_id As Integer, user_name As String, ret_msg As String = ""
        Dim email As String = ""
        Try
            sqlcmd = New SqlCommand("User_Login_Check_mobile", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@mobile", SqlDbType.VarChar, 500)
                .Parameters("@mobile").Value = mobile

                .Parameters.Add("@otp_paasword", SqlDbType.VarChar, 500)
                .Parameters("@otp_paasword").Value = password

                .Parameters.Add("@Password_Str", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output
                .Parameters.Add("@Ret_Message", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output
                .Parameters.Add("@Ret_Val", SqlDbType.Int).Direction = ParameterDirection.Output
                .Parameters.Add("@user_id", SqlDbType.Int).Direction = ParameterDirection.Output
                .Parameters.Add("@user_name", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output
                .Parameters.Add("@email", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output

                sqlcon.Open()
                .ExecuteNonQuery()
                sqlcon.Close()
                ret_val = .Parameters("@Ret_Val").Value
                If ret_val <> 0 Then
                    ret_msg = .Parameters("@Ret_Message").Value
                    password_str = (.Parameters("@Password_Str").Value).ToString()
                    user_id = .Parameters("@user_id").Value
                    user_name = .Parameters("@user_name").Value
                    email = (.Parameters("@email").Value).ToString()
                End If
            End With
			
            If ret_val = 1 Then
			if password_str = "facebook" or password_str = "google_oauth2" or password_str = "yahoo" then
			  ret_val = 3
			  password_check = "Please Confirm your Mobile number for Mobile login"
                confirm_status = "no"
			else
                If userpwd_hash.validatepwd(password, password_str) Then
                    sessionstr = Now.TimeOfDay.ToString() & ";"
                    sessionstr = sessionstr & user_id & ";" & user_name & ";" & email & ";" & sessiontype & ""
                    CookieUtil.SetEncryptedCookie("gyaane", sessionstr)
                    password_check = "success"
                Else
                    password_check = "Invalid password"
                End If
				end if
            ElseIf ret_val = 0 Then
                password_check = "Invalid Mobile"
            ElseIf ret_val = 2 Or ret_val = 5 Then
                password_check = "success"
                confirm_status = "no"
            ElseIf ret_val = 3 Then
                password_check = "Please Confirm your Mobile number for Mobile login"
                confirm_status = "no"
            ElseIf ret_val = 4 Then
                Try
                    Dim password1 As String = GetPassword()
                    sqlcmd = New SqlCommand("diff_account_mobile_Password", sqlcon)
                    With sqlcmd
                        .CommandType = CommandType.StoredProcedure

                        .Parameters.Add("@mobile", SqlDbType.VarChar, 15).Direction = ParameterDirection.Input
                        .Parameters("@mobile").Value = mobile
                        .Parameters.Add("@password", SqlDbType.VarChar, 250)
                        .Parameters("@password").Value = userpwd_hash.userpwd_hash(password1)
                        .Parameters.Add("@ret_msg", SqlDbType.VarChar, 10).Direction = ParameterDirection.Output

                        sqlcon.Open()
                        .ExecuteNonQuery()
                        sqlcon.Close()
                    End With
                    If sqlcmd.Parameters("@ret_msg").Value = "ok" Then
                        SmsApi.SendSMS("gyaane", "gyaane123", "91" & mobile & "", "Your OTP password is " & password1 & " and You can login with your Mobile Number.", "", "", "")
                        password_check = "We sent a Password to your mobile."
                        confirm_status = "yes"
                    End If
                Catch ex As Exception

                End Try
            End If
            ret_str = "{""ret_val"":""" & ret_val & """, ""password_check"":""" & password_check & """, ""confirm_status"":""" & confirm_status & """,""mobile"":""" & mobile & """,""email"":""" & email & """,""user_id"":""" & user_id & """}"
        Catch ex As Exception
            ret_str = "{""ret_val"":""4"", ""ret_msg"":""" & ex.ToString() & """}"
        Finally
            sqlcon.Close()
            sqlcmd = Nothing
        End Try
        Return ret_str
    End Function
    Public Shared Function RandomNumber(ByVal min As Integer, ByVal max As Integer) As Integer
        Dim random As New Random()
        Return random.Next(min, max)
    End Function 'RandomNumber

    Public Shared Function RandomString(ByVal size As Integer, ByVal lowerCase As Boolean) As String
        Dim builder As New StringBuilder()
        Dim random As New Random()
        Dim ch As Char
        Dim i As Integer
        For i = 0 To size - 1
            ch = Convert.ToChar(Convert.ToInt32((26 * random.NextDouble() + 65)))
            builder.Append(ch)
        Next i
        If lowerCase Then
            Return builder.ToString().ToLower()
        End If
        Return builder.ToString()
    End Function 'RandomString

    Public Shared Function GetPassword() As String
        Dim builder As New StringBuilder()
        builder.Append(RandomNumber(100000, 999999))
        Return builder.ToString()
    End Function
    Public Shared Function CountWords(ByVal value As String) As Integer
        ' Count matches.
        Dim collection As MatchCollection = Regex.Matches(value, "\S+")
        Return collection.Count
    End Function
End Class
