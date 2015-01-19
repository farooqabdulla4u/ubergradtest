Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Web


Public Class Umail_class
    Shared ConString As String = ConfigurationManager.ConnectionStrings("GyaaneDbString").ConnectionString
    Public Shared Function Get_Contacted_universities(ByVal student_id As Integer) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("get_student_contacted_campus", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@student_id", SqlDbType.BigInt)
                .Parameters("@student_id").Direction = ParameterDirection.Input
                .Parameters("@student_id").Value = student_id

                .Parameters.Add("@ret_msg", SqlDbType.VarChar, 20)
                .Parameters("@ret_msg").Direction = ParameterDirection.Output


            End With
            sqldata.SelectCommand = sqlcmd
            sqldata.Fill(ds)

            With ds.Tables(0)
                For i As Integer = 0 To .Rows.Count - 1
                    If i = 0 Then
                        restr = "{""id"":""" & .Rows.Item(i)("id") & """, ""name"":""" & .Rows.Item(i)("name") & """,""status"":""" & .Rows.Item(i)("status") & """}"
                    Else
                        restr += ",{""id"":""" & .Rows.Item(i)("id") & """, ""name"":""" & .Rows.Item(i)("name") & """,""status"":""" & .Rows.Item(i)("status") & """}"
                    End If
                Next
            End With


            restr = "{""items"":[" & restr & "],""ret_msg"":""" & sqlcmd.Parameters("@ret_msg").Value() & """}"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr

    End Function

    Public Shared Function Send_Composed_Umail(ByVal student_id As String, ByVal cids As String, ByVal subject As String, ByVal content As String, ByVal sent_to As String, ByVal mode As String, ByVal umail_id As String) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("student_send_composed_umail", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                If student_id <> "" Then
                    .Parameters.Add("@student_id", SqlDbType.Int)
                    .Parameters("@student_id").Direction = ParameterDirection.Input
                    .Parameters("@student_id").Value = student_id
                End If

                If cids <> "" Then
                    .Parameters.Add("@camp_id", SqlDbType.VarChar, 1000)
                    .Parameters("@camp_id").Direction = ParameterDirection.Input
                    .Parameters("@camp_id").Value = cids
                End If

                If subject <> "" Then
                    .Parameters.Add("@subject", SqlDbType.VarChar, 20)
                    .Parameters("@subject").Direction = ParameterDirection.Input
                    .Parameters("@subject").Value = subject
                End If

                If content <> "" Then
                    .Parameters.Add("@message", SqlDbType.VarChar, 8000)
                    .Parameters("@message").Direction = ParameterDirection.Input
                    .Parameters("@message").Value = content
                End If

                If sent_to <> "" Then
                    .Parameters.Add("@sent_to", SqlDbType.VarChar, 30)
                    .Parameters("@sent_to").Direction = ParameterDirection.Input
                    .Parameters("@sent_to").Value = sent_to
                End If
                If mode <> "" Then
                    .Parameters.Add("@mode", SqlDbType.Int)
                    .Parameters("@mode").Direction = ParameterDirection.Input
                    .Parameters("@mode").Value = mode
                End If
                If umail_id <> "" Then
                    .Parameters.Add("@uml_id", SqlDbType.VarChar, 50)
                    .Parameters("@uml_id").Direction = ParameterDirection.Input
                    .Parameters("@uml_id").Value = umail_id
                End If

                sqlcon.Open()
                sqlcmd.ExecuteNonQuery()

            End With

            restr = "success"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr

    End Function
    Public Shared Function get_student_umails(ByVal student_id As Integer, ByVal mail_type As Integer, ByVal s_string As String, ByVal page_number As Integer, ByVal page_size As Integer) As String
        Dim sqlcmd As SqlCommand, sqlcon As SqlConnection = Nothing, retstr As String = "", da As New SqlDataAdapter, ds As New DataSet
        Dim mails As String = ""
        Try
            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("get_student_umails_test", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@student_id", SqlDbType.BigInt)
                .Parameters("@student_id").Direction = ParameterDirection.Input
                .Parameters("@student_id").Value = student_id

                '.Parameters.Add("@university_admin_id", SqlDbType.BigInt)
                '.Parameters("@university_admin_id").Direction = ParameterDirection.Input
                '.Parameters("@university_admin_id").Value = university_admin_id

                .Parameters.Add("@mail_type", SqlDbType.Int)
                .Parameters("@mail_type").Direction = ParameterDirection.Input
                .Parameters("@mail_type").Value = mail_type

                If (s_string <> "") Then
                    .Parameters.Add("@search_string", SqlDbType.VarChar, 50)
                    .Parameters("@search_string").Direction = ParameterDirection.Input
                    .Parameters("@search_string").Value = s_string
                End If

                If page_number > 0 Then
                    .Parameters.Add("@page_number", SqlDbType.Int)
                    .Parameters("@page_number").Direction = ParameterDirection.Input
                    .Parameters("@page_number").Value = page_number
                End If

                If page_size > 0 Then
                    .Parameters.Add("@page_size", SqlDbType.Int)
                    .Parameters("@page_size").Direction = ParameterDirection.Input
                    .Parameters("@page_size").Value = page_size
                End If

                .Parameters.Add("@count", SqlDbType.Int)
                .Parameters("@count").Direction = ParameterDirection.Output

                .Parameters.Add("@notification", SqlDbType.VarChar, 20)
                .Parameters("@notification").Direction = ParameterDirection.Output

            End With
            da.SelectCommand = sqlcmd
            da.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                With ds.Tables(0)
                    For i As Integer = 0 To .Rows.Count - 1
                        If i = 0 Then
                            mails = mails & "{""uml_id"":""" & .Rows(i)("uml_id") & """,""subject"":""" & .Rows(i)("subject") & """,""message"":""" & .Rows(i)("message") & """,""date"":""" & .Rows(i)("inserted_ts") & """,""campus_id"":""" & .Rows(i)("campus_id") & """,""campus_name"":""" & .Rows(i)("campus_name") & """,""city"":""" & .Rows(i)("city") & """,""state"":""" & .Rows(i)("state") & """,""count"":""" & .Rows(i)("count") & """,""sender"":""" & .Rows(i)("sender") & """,""last"":""" & .Rows(i)("last") & """,""read_status"":""" & .Rows(i)("read_status") & """,""student_imp_status"":""" & .Rows(i)("imp_status") & """,""date1"":""" & .Rows(i)("date") & """,""time1"":""" & .Rows(i)("time") & """,""subject_category"":""" & .Rows(i)("subject_category").ToString() & """,""cat_name"":""" & .Rows(i)("cat_name").ToString() & """,""draft_status"":""" & .Rows(i)("draft_status").ToString() & """}"
                        Else
                            mails = mails & ",{""uml_id"":""" & .Rows(i)("uml_id") & """,""subject"":""" & .Rows(i)("subject") & """,""message"":""" & .Rows(i)("message") & """,""date"":""" & .Rows(i)("inserted_ts") & """,""campus_id"":""" & .Rows(i)("campus_id") & """,""campus_name"":""" & .Rows(i)("campus_name") & """,""city"":""" & .Rows(i)("city") & """,""state"":""" & .Rows(i)("state") & """,""count"":""" & .Rows(i)("count") & """,""sender"":""" & .Rows(i)("sender") & """,""last"":""" & .Rows(i)("last") & """,""read_status"":""" & .Rows(i)("read_status") & """,""student_imp_status"":""" & .Rows(i)("imp_status") & """,""date1"":""" & .Rows(i)("date") & """,""time1"":""" & .Rows(i)("time") & """,""subject_category"":""" & .Rows(i)("subject_category").ToString() & """,""cat_name"":""" & .Rows(i)("cat_name").ToString() & """,""draft_status"":""" & .Rows(i)("draft_status").ToString() & """}"
                        End If
                    Next
                End With

            End If
            retstr = "{""items"":[" & mails & "],""count"":""" & sqlcmd.Parameters("@count").Value.ToString() & """,""notification"":""" & sqlcmd.Parameters("@notification").Value.ToString() & """}"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            sqlcon.Close()
        End Try
        Return retstr
    End Function
    Public Shared Function get_individual_student_umail(ByVal uml_id As Integer) As String
        Dim sqlcmd As SqlCommand, sqlcon As SqlConnection = Nothing, retstr As String = "", da As New SqlDataAdapter, ds As New DataSet
        Dim umails As New StringBuilder, campusinfo As String = ""
        Try
            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("individual_student_umail", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@uml_id", SqlDbType.BigInt)
                .Parameters("@uml_id").Direction = ParameterDirection.Input
                .Parameters("@uml_id").Value = uml_id

                .Parameters.Add("@subject_category_name", SqlDbType.VarChar, 20)
                .Parameters("@subject_category_name").Direction = ParameterDirection.Output

                .Parameters.Add("@draft_message", SqlDbType.VarChar, 8000)
                .Parameters("@draft_message").Direction = ParameterDirection.Output

                .Parameters.Add("@student_id", SqlDbType.BigInt)
                .Parameters("@student_id").Direction = ParameterDirection.Input
                .Parameters("@student_id").Value = SessionUtil.getSession("user_id")

                .Parameters.Add("@notification", SqlDbType.VarChar, 20)
                .Parameters("@notification").Direction = ParameterDirection.Output

            End With
            da.SelectCommand = sqlcmd
            da.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                With ds.Tables(0)
                    For i As Integer = 0 To .Rows.Count - 1
                        If i = 0 Then
                            umails.Append("{""uml_id"":""" & .Rows(i)("uml_id") & """,""subject"":""" & .Rows(i)("subject") & """,""subject_id"":""" & .Rows(i)("Subject_Id") & """,""message"":""" & .Rows(i)("message") & """,""date"":""" & .Rows(i)("inserted_ts") & """,""sender"":""" & .Rows(i)("sender") & """,""date1"":""" & .Rows(i)("date") & """,""time1"":""" & .Rows(i)("time") & """,""imp_status"":""" & .Rows(i)("student_important_status") & """}")
                        Else
                            umails.Append(",{""uml_id"":""" & .Rows(i)("uml_id") & """,""subject"":""" & .Rows(i)("subject") & """,""subject_id"":""" & .Rows(i)("Subject_id") & """,""message"":""" & .Rows(i)("message") & """,""date"":""" & .Rows(i)("inserted_ts") & """,""sender"":""" & .Rows(i)("sender") & """,""date1"":""" & .Rows(i)("date") & """,""time1"":""" & .Rows(i)("time") & """,""imp_status"":""" & .Rows(i)("student_important_status") & """}")
                        End If
                    Next
                End With
            End If
            If ds.Tables(1).Rows.Count > 0 Then
                With ds.Tables(1)
                    For i As Integer = 0 To .Rows.Count - 1
                        If i = 0 Then
                            '  studentinfo = studentinfo & """id"":""" & .Rows(i)("id") & """,""name"":""" & .Rows(i)("name") & """,""pic"":""" & .Rows(i)("pic") & """,""country"":""" & .Rows(i)("country") & """,""state"":""" & .Rows(i)("state") & """,""city"":""" & .Rows(i)("city") & """,""address"":""" & .Rows(i)("address") & """,""profile_status"":""" & .Rows(i)("profile_status") & """"
                            campusinfo = campusinfo & """id"":""" & .Rows(i)("id") & """,""name"":""" & .Rows(i)("name") & """,""pic"":""" & .Rows(i)("pic") & """,""state"":""" & .Rows(i)("state") & """,""city"":""" & .Rows(i)("city") & """,""Address"":""" & .Rows(i)("address") & """"
                        End If
                    Next
                End With

            End If
            If ds.Tables(2).Rows.Count > 0 Then
                With ds.Tables(2)
                    For i As Integer = 0 To .Rows.Count - 1
                        If i = 0 Then
                            If campusinfo = "" Then
                                'studentinfo = studentinfo & """degree"":""" & .Rows(i)("Degree_Name") & """,""stream"":""" & .Rows(i)("stream") & """,""Completion_Year"":""" & .Rows(i)("Completion_Year") & """,""University_Name"":""" & .Rows(i)("University_Name") & """,""College_Name"":""" & .Rows(i)("College_Name") & """"
                                campusinfo = campusinfo & """campus_established"":""" & .Rows(i)("campus_established") & """,""campus_acceptance_rate"":""" & .Rows(i)("campus_acceptance_rate") & """,""campus_male_female_ratio"":""" & .Rows(i)("campus_male_female_ratio") & """,""campus_setting"":""" & .Rows(i)("campus_setting") & """"
                            Else
                                'studentinfo = studentinfo & ",""degree"":""" & .Rows(i)("Degree_Name") & """,""stream"":""" & .Rows(i)("stream") & """,""Completion_Year"":""" & .Rows(i)("Completion_Year") & """,""University_Name"":""" & .Rows(i)("University_Name") & """,""College_Name"":""" & .Rows(i)("College_Name") & """"
                                campusinfo = campusinfo & ",""campus_established"":""" & .Rows(i)("campus_established") & """,""campus_acceptance_rate"":""" & .Rows(i)("campus_acceptance_rate") & """,""campus_male_female_ratio"":""" & .Rows(i)("campus_male_female_ratio") & """,""campus_setting"":""" & .Rows(i)("campus_setting") & """"
                            End If

                        End If
                    Next
                End With

            End If
            'If ds.Tables(3).Rows.Count > 0 Then
            '    With ds.Tables(3)
            '        For i As Integer = 0 To .Rows.Count - 1
            '            If i = 0 Then
            '                If studentinfo = "" Then
            '                    studentinfo = studentinfo & ",""GPA_Score"":""" & .Rows(i)("GPA_Score") & """,""GPA_Max"":""" & .Rows(i)("GPA_Max") & """,""GMAT_Score"":""" & .Rows(i)("GMAT_Score") & """,""GRE_Score"":""" & .Rows(i)("GRE_Score") & """,""TOEFL_Score"":""" & .Rows(i)("TOEFL_Score") & """,""IELTS_Score"":""" & .Rows(i)("IELTS_Score") & """"
            '                Else
            '                    studentinfo = studentinfo & ",""GPA_Score"":""" & .Rows(i)("GPA_Score") & """,""GPA_Max"":""" & .Rows(i)("GPA_Max") & """,""GMAT_Score"":""" & .Rows(i)("GMAT_Score") & """,""GRE_Score"":""" & .Rows(i)("GRE_Score") & """,""TOEFL_Score"":""" & .Rows(i)("TOEFL_Score") & """,""IELTS_Score"":""" & .Rows(i)("IELTS_Score") & """"
            '                End If

            '            End If
            '        Next
            '    End With

            'End If
            If campusinfo <> "" Then
                retstr = "{""notification"":""" & sqlcmd.Parameters("@notification").Value.ToString() & """,""subject_category_id"":""" & sqlcmd.Parameters("@subject_category_name").Value() & """,""draft_msg"":""" & sqlcmd.Parameters("@draft_message").Value() & """,""umails"":[" + umails.ToString() + "]," & campusinfo & "}"
            Else
                retstr = "{""notification"":""" & sqlcmd.Parameters("@notification").Value.ToString() & """,""subject_category_id"":""" & sqlcmd.Parameters("@subject_category_name").Value() & """,""draft_msg"":""" & sqlcmd.Parameters("@draft_message").Value() & """,""umails"":[" + umails.ToString() + "]}"
            End If

            '   retstr = "{""umails"":[" + umails.ToString() + "]," & campusinfo & "}"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            sqlcon.Close()
        End Try
        Return retstr
    End Function
    Public Shared Function student_send_email(ByVal sid As String, ByVal content As String, ByVal uml_id As String) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("student_send_email", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                'If campus_id <> "" Then
                '    .Parameters.Add("@campus_id", SqlDbType.Int)
                '    .Parameters("@campus_id").Direction = ParameterDirection.Input
                '    .Parameters("@campus_id").Value = campus_id
                'End If

                'If user_id <> "" Then
                '    .Parameters.Add("@user_id", SqlDbType.Int)
                '    .Parameters("@user_id").Direction = ParameterDirection.Input
                '    .Parameters("@user_id").Value = user_id
                'End If

                If sid <> "" Then
                    .Parameters.Add("@stu_id", SqlDbType.Int)
                    .Parameters("@stu_id").Direction = ParameterDirection.Input
                    .Parameters("@stu_id").Value = sid
                End If

                'If subject <> "" Then
                '    .Parameters.Add("@subject", SqlDbType.VarChar, 1000)
                '    .Parameters("@subject").Direction = ParameterDirection.Input
                '    .Parameters("@subject").Value = subject
                'End If

                If content <> "" Then
                    .Parameters.Add("@message", SqlDbType.VarChar, 8000)
                    .Parameters("@message").Direction = ParameterDirection.Input
                    .Parameters("@message").Value = content
                End If

                If uml_id <> "" Then
                    .Parameters.Add("@uml_id", SqlDbType.Int)
                    .Parameters("@uml_id").Direction = ParameterDirection.Input
                    .Parameters("@uml_id").Value = uml_id
                End If


                sqlcon.Open()
                sqlcmd.ExecuteNonQuery()

            End With

            restr = "success"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr

    End Function
    Public Shared Function student_inbox_delete_umail(ByVal uml_ids As String, ByVal mtype As Integer) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("student_inbox_delete_umail", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                If uml_ids <> "" Then
                    .Parameters.Add("@uml_ids", SqlDbType.VarChar, 100)
                    .Parameters("@uml_ids").Direction = ParameterDirection.Input
                    .Parameters("@uml_ids").Value = uml_ids.Trim(",")

                    .Parameters.Add("@mtype", SqlDbType.Int)
                    .Parameters("@mtype").Direction = ParameterDirection.Input
                    .Parameters("@mtype").Value = mtype

                End If

               

                    sqlcon.Open()
                    sqlcmd.ExecuteNonQuery()
            End With

            restr = "success"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr

    End Function
    Public Shared Function get_subjects() As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("get_subjects", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                sqlcon.Open()
                sqlcmd.ExecuteNonQuery()
            End With

            sqldata.SelectCommand = sqlcmd
            sqldata.Fill(ds)

            With ds.Tables(0)
                For i As Integer = 0 To .Rows.Count - 1
                    If i = 0 Then
                        restr = "{""id"":""" & .Rows.Item(i)("Subject_Id") & """, ""subject"":""" & .Rows.Item(i)("subject") & """}"
                    Else
                        restr += ",{""id"":""" & .Rows.Item(i)("Subject_Id") & """, ""subject"":""" & .Rows.Item(i)("subject") & """}"
                    End If
                Next
            End With

            restr = "{""items"":[" & restr & "]}"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr
    End Function
    Public Shared Function Get_bookmarked_campus(ByVal student_id As Integer) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("Get_Bookmarked_Campus", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@student_id", SqlDbType.BigInt)
                .Parameters("@student_id").Direction = ParameterDirection.Input
                .Parameters("@student_id").Value = student_id

                .Parameters.Add("@ret_msg", SqlDbType.VarChar, 20)
                .Parameters("@ret_msg").Direction = ParameterDirection.Output


            End With
            sqldata.SelectCommand = sqlcmd
            sqldata.Fill(ds)

            With ds.Tables(0)
                For i As Integer = 0 To .Rows.Count - 1
                    If i = 0 Then
                        restr = "{""id"":""" & .Rows.Item(i)("campus_id") & """, ""name"":""" & .Rows.Item(i)("campus_name") & """}"
                    Else
                        restr += ",{""id"":""" & .Rows.Item(i)("campus_id") & """, ""name"":""" & .Rows.Item(i)("campus_name") & """}"
                    End If
                Next
            End With


            restr = "{""items"":[" & restr & "],""ret_msg"":""" & sqlcmd.Parameters("@ret_msg").Value() & """}"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr

    End Function
    Public Shared Function update_univ_inbox_read_status(ByVal uml_id As Integer) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("update_student_umail_read_status", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@uml_id", SqlDbType.BigInt)
                .Parameters("@uml_id").Direction = ParameterDirection.Input
                .Parameters("@uml_id").Value = uml_id

                .Parameters.Add("@ret_msg", SqlDbType.VarChar, 20)
                .Parameters("@ret_msg").Direction = ParameterDirection.Output
            End With
            sqldata.SelectCommand = sqlcmd
            sqldata.Fill(ds)

            'sqlcon.Open()
            '.ExecuteNonQuery()
            'sqlcon.Close()



            restr = "{""ret_msg"":""" & sqlcmd.Parameters("@ret_msg").Value() & """}"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            sqlcon.Close()
        End Try
        Return restr
    End Function
    Public Shared Function student_imp_mark(ByVal uml_id As Integer, ByVal status As Integer) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("change_student_imp_status", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@uml_id", SqlDbType.BigInt)
                .Parameters("@uml_id").Direction = ParameterDirection.Input
                .Parameters("@uml_id").Value = uml_id

                .Parameters.Add("@status", SqlDbType.BigInt)
                .Parameters("@status").Direction = ParameterDirection.Input
                .Parameters("@status").Value = status

                .Parameters.Add("@ret_msg", SqlDbType.VarChar, 20)
                .Parameters("@ret_msg").Direction = ParameterDirection.Output


                sqlcon.Open()
                .ExecuteNonQuery()
                sqlcon.Close()
            End With
            restr = "{""ret_msg"":""" & sqlcmd.Parameters("@ret_msg").Value() & """}"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            sqlcon.Close()
        End Try
        Return restr
    End Function
    Public Shared Function student_send_reply(ByVal uml_id As Integer, ByVal content As String) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("student_reply_mail", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@uml_id", SqlDbType.BigInt)
                .Parameters("@uml_id").Direction = ParameterDirection.Input
                .Parameters("@uml_id").Value = uml_id

                .Parameters.Add("@content", SqlDbType.VarChar, 5000)
                .Parameters("@content").Direction = ParameterDirection.Input
                .Parameters("@content").Value = content

                .Parameters.Add("@ret_msg", SqlDbType.VarChar, 20)
                .Parameters("@ret_msg").Direction = ParameterDirection.Output


                sqlcon.Open()
                .ExecuteNonQuery()
                sqlcon.Close()
            End With
            restr = "{""ret_msg"":""" & sqlcmd.Parameters("@ret_msg").Value() & """}"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            sqlcon.Close()
        End Try
        Return restr
    End Function
    Public Shared Function student_Send_Composed_Umail(ByVal student_id As String, ByVal cids As String, ByVal subject As String, ByVal content As String) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("student_composed_umail_new", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                If student_id <> "" Then
                    .Parameters.Add("@student_id", SqlDbType.VarChar, 100)
                    .Parameters("@student_id").Direction = ParameterDirection.Input
                    .Parameters("@student_id").Value = student_id
                End If

                If cids <> "" Then
                    .Parameters.Add("@camp_id", SqlDbType.VarChar, 100)
                    .Parameters("@camp_id").Direction = ParameterDirection.Input
                    .Parameters("@camp_id").Value = cids
                End If

                If subject <> "" Then
                    .Parameters.Add("@subject", SqlDbType.VarChar, 100)
                    .Parameters("@subject").Direction = ParameterDirection.Input
                    .Parameters("@subject").Value = subject
                End If

                If content <> "" Then
                    .Parameters.Add("@message", SqlDbType.VarChar, 8000)
                    .Parameters("@message").Direction = ParameterDirection.Input
                    .Parameters("@message").Value = content
                End If

                .Parameters.Add("@ret_msg", SqlDbType.VarChar, 20)
                .Parameters("@ret_msg").Direction = ParameterDirection.Output

                 sqlcon.Open()
                .ExecuteNonQuery()
                sqlcon.Close()
            End With
            restr = "{""ret_msg"":""" & sqlcmd.Parameters("@ret_msg").Value() & """}"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            sqlcon.Close()
        End Try
        Return restr
    End Function
    Public Shared Function student_Umail_Actions(ByVal uml_ids As String, ByVal status As String, ByVal mtype As String) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("student_umail_actions", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                If uml_ids <> "" Then
                    .Parameters.Add("@uml_ids", SqlDbType.VarChar, 100)
                    .Parameters("@uml_ids").Direction = ParameterDirection.Input
                    .Parameters("@uml_ids").Value = uml_ids.Trim(",")

                    .Parameters.Add("@status", SqlDbType.VarChar, 20)
                    .Parameters("@status").Direction = ParameterDirection.Input
                    .Parameters("@status").Value = status

                    .Parameters.Add("@mtype", SqlDbType.VarChar, 20)
                    .Parameters("@mtype").Direction = ParameterDirection.Input
                    .Parameters("@mtype").Value = mtype

                    .Parameters.Add("@ret_msg", SqlDbType.VarChar, 20)
                    .Parameters("@ret_msg").Direction = ParameterDirection.Output

                End If
                sqlcon.Open()
                sqlcmd.ExecuteNonQuery()
            End With

            restr = "success"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr

    End Function
    Public Shared Function Get_student_bookmarked_universities(ByVal student_id As Integer) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("student_get_bookmarks", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@student_id", SqlDbType.BigInt)
                .Parameters("@student_id").Direction = ParameterDirection.Input
                .Parameters("@student_id").Value = student_id

                .Parameters.Add("@count", SqlDbType.Int)
                .Parameters("@count").Direction = ParameterDirection.Output


            End With
            sqldata.SelectCommand = sqlcmd
            sqldata.Fill(ds)

            With ds.Tables(0)
                For i As Integer = 0 To .Rows.Count - 1
                    If i = 0 Then
                        restr = "{""id"":""" & .Rows.Item(i)("campus_id") & """, ""name"":""" & .Rows.Item(i)("campus_name") & """,""name_string"":""" & .Rows.Item(i)("campus_name").replace(" ", "-") & """,""logo"":""" & .Rows.Item(i)("logo") & """}"
                    Else
                        restr += ",{""id"":""" & .Rows.Item(i)("campus_id") & """, ""name"":""" & .Rows.Item(i)("campus_name") & """,""name_string"":""" & .Rows.Item(i)("campus_name").replace(" ", "-") & """,""logo"":""" & .Rows.Item(i)("logo") & """}"
                    End If
                Next
            End With


            restr = "{""items"":[" & restr & "],""count"":""" & sqlcmd.Parameters("@count").Value() & """}"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr

    End Function
    Public Shared Function delete_bookmarks(ByVal student_id As Integer, ByVal campus_id As Integer) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("delete_student_bookmarks", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@student_id", SqlDbType.BigInt)
                .Parameters("@student_id").Direction = ParameterDirection.Input
                .Parameters("@student_id").Value = student_id

                .Parameters.Add("@campus_id", SqlDbType.BigInt)
                .Parameters("@campus_id").Direction = ParameterDirection.Input
                .Parameters("@campus_id").Value = campus_id

                .Parameters.Add("@ret_msg", SqlDbType.VarChar, 20)
                .Parameters("@ret_msg").Direction = ParameterDirection.Output

                sqlcon.Open()
                sqlcmd.ExecuteNonQuery()
            End With

            restr = "{""ret_msg"":""" & sqlcmd.Parameters("@ret_msg").Value() & """}"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr
    End Function
    Public Shared Function Student_resuggest_request(ByVal content As String, ByVal uml_id As Integer, ByVal status As String) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("Student_Resuggest", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@uml_id", SqlDbType.BigInt)
                .Parameters("@uml_id").Direction = ParameterDirection.Input
                .Parameters("@uml_id").Value = uml_id

                .Parameters.Add("@content", SqlDbType.VarChar, 5000)
                .Parameters("@content").Direction = ParameterDirection.Input
                .Parameters("@content").Value = content

                .Parameters.Add("@status", SqlDbType.VarChar, 200)
                .Parameters("@status").Direction = ParameterDirection.Input
                .Parameters("@status").Value = status

                .Parameters.Add("@ret_msg", SqlDbType.VarChar, 20)
                .Parameters("@ret_msg").Direction = ParameterDirection.Output

                .Parameters.Add("@student_id", SqlDbType.VarChar, 50)
                .Parameters("@student_id").Direction = ParameterDirection.Input
                .Parameters("@student_id").Value = SessionUtil.getSession("user_id")

                sqlcon.Open()
                sqlcmd.ExecuteNonQuery()
            End With

            restr = "{""ret_msg"":""" & sqlcmd.Parameters("@ret_msg").Value() & """}"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr
    End Function
    Public Shared Function Get_student_suggested_list() As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("student_suggested_list", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@student_id", SqlDbType.BigInt)
                .Parameters("@student_id").Direction = ParameterDirection.Input
                .Parameters("@student_id").Value = 13133

                .Parameters.Add("@ret_msg", SqlDbType.VarChar, 20)
                .Parameters("@ret_msg").Direction = ParameterDirection.Output


            End With
            sqldata.SelectCommand = sqlcmd
            sqldata.Fill(ds)

            With ds.Tables(0)
                For i As Integer = 0 To .Rows.Count - 1
                    If i = 0 Then
                        restr = "{""id"":""" & .Rows.Item(i)("suggested_id") & """, ""name"":""" & .Rows.Item(i)("campus_name") & """}"
                    Else
                        restr += ",{""id"":""" & .Rows.Item(i)("suggested_id") & """, ""name"":""" & .Rows.Item(i)("campus_name") & """}"
                    End If
                Next
            End With


            restr = "{""items"":[" & restr & "],""ret_msg"":""" & sqlcmd.Parameters("@ret_msg").Value() & """}"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr
    End Function
    Public Shared Function Student_Request_to_apply(ByVal sug_ids As String, ByVal uml_id As Integer) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("student_request_to_apply", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@uml_id", SqlDbType.BigInt)
                .Parameters("@uml_id").Direction = ParameterDirection.Input
                .Parameters("@uml_id").Value = uml_id

                .Parameters.Add("@sug_ids", SqlDbType.VarChar, 5000)
                .Parameters("@sug_ids").Direction = ParameterDirection.Input
                .Parameters("@sug_ids").Value = sug_ids
                
                .Parameters.Add("@student_id", SqlDbType.VarChar, 50)
                .Parameters("@student_id").Direction = ParameterDirection.Input
                .Parameters("@student_id").Value = SessionUtil.getSession("user_id")

                .Parameters.Add("@ret_msg", SqlDbType.VarChar, 20)
                .Parameters("@ret_msg").Direction = ParameterDirection.Output

                sqlcon.Open()
                sqlcmd.ExecuteNonQuery()
            End With

            restr = "{""ret_msg"":""" & sqlcmd.Parameters("@ret_msg").Value() & """}"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr
    End Function
    Public Shared Function book_mark_univ(ByVal campus_id As String, ByVal campus_name As String, ByVal course As String, ByVal program As String) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("student_book_mark_univ", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@campus_id", SqlDbType.VarChar, 40)
                .Parameters("@campus_id").Direction = ParameterDirection.Input
                .Parameters("@campus_id").Value = campus_id

                .Parameters.Add("@course", SqlDbType.VarChar, 100)
                .Parameters("@course").Direction = ParameterDirection.Input
                .Parameters("@course").Value = course

                .Parameters.Add("@program", SqlDbType.VarChar, 100)
                .Parameters("@program").Direction = ParameterDirection.Input
                .Parameters("@program").Value = program

                .Parameters.Add("@student_id", SqlDbType.VarChar, 50)
                .Parameters("@student_id").Direction = ParameterDirection.Input
                .Parameters("@student_id").Value = SessionUtil.getSession("user_id")

                .Parameters.Add("@ret_msg", SqlDbType.VarChar, 20)
                .Parameters("@ret_msg").Direction = ParameterDirection.Output

                sqlcon.Open()
                sqlcmd.ExecuteNonQuery()
            End With

            restr = "{""ret_msg"":""" & sqlcmd.Parameters("@ret_msg").Value() & """}"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr
    End Function
End Class
