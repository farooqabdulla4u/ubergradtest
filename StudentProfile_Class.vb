Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Web
Imports Newtonsoft.Json.Linq
Public Class StudentProfile_Class
    Shared ConString As String = ConfigurationManager.ConnectionStrings("GyaaneDbString").ConnectionString
    Public Shared Function Get_Language_List(ByVal search_lang_term As String) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = "", item1 As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("Get_Languages_Known_ub_old", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@lang", SqlDbType.VarChar, 50)
                .Parameters("@lang").Value = search_lang_term

            End With
            sqldata.SelectCommand = sqlcmd
            sqldata.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                With ds.Tables(0)
                    For i As Integer = 0 To .Rows.Count - 1
                        If i = 0 Then
                            item1 = item1 & "{""id"":""" & .Rows(i)("Language_id") & """,""name"":""" & .Rows(i)("Language_Name") & """}"
                        Else
                            item1 = item1 & ",{""id"":""" & .Rows(i)("Language_id") & """,""name"":""" & .Rows(i)("Language_Name") & """}"
                        End If
                    Next i
                End With
            End If
            restr = "[" + item1 + "]"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr

    End Function
    Public Shared Function Check_Mobile(ByVal stu_id As Integer, ByVal mobile As String) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("user_mobile_check", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure
                sqlcmd.Parameters.Add("@end_user_id", SqlDbType.Int)
                sqlcmd.Parameters("@end_user_id").Value = stu_id
                sqlcmd.Parameters.Add("@mobile", SqlDbType.VarChar, 15)
                sqlcmd.Parameters("@mobile").Value = mobile
                sqlcmd.Parameters.Add("@ret_val", SqlDbType.Int).Direction = ParameterDirection.Output

            End With
            sqldata.SelectCommand = sqlcmd
            sqldata.Fill(ds)

            restr = "{""ret_val"":""" & sqlcmd.Parameters("@ret_val").Value & """}"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr

    End Function
    Public Shared Function Submit_User_Details(ByVal name As String, ByVal email As String, ByVal mobile As String, ByVal state As String,
                                                         ByVal city As String, ByVal gender As String, ByVal dob As String, ByVal cob As String,
                                                         ByVal f_name As String, ByVal m_name As String, ByVal citizenship As String,
                                                         ByVal m_addr As String, ByVal p_addr As String, ByVal lang As String, ByVal bachelor_degree As String,
                                                         ByVal present_stream As String, ByVal college As String,
                                                         ByVal backlogs As String, ByVal cgpa_percent As String, ByVal gpa_scale As String,
                                                         ByVal publications As String, ByVal verbal_gre As String,
                                                         ByVal analytical_gre As String, ByVal quants_gre As String, ByVal tot_gre As String, ByVal gre_retake As String,
                                                         ByVal est_gre As String, ByVal est_date_gre As String, ByVal tot_gmat As String, ByVal gmat_retake As String,
                                                         ByVal est_gmat As String, ByVal est_date_gmat As String, ByVal reading_toefl As String, ByVal writing_toefl As String,
                                                         ByVal listening_toefl As String, ByVal speaking_toefl As String, ByVal tot_toefl As String, ByVal toefl_retake As String,
                                                         ByVal est_toefl As String, ByVal est_date_toefl As String, ByVal reading_ielts As String, ByVal writing_ielts As String,
                                                         ByVal listening_ielts As String, ByVal speaking_ielts As String, ByVal tot_ielts As String, ByVal ielts_retake As String,
                                                         ByVal est_ielts As String, ByVal est_date_ielts As String, ByVal gre_taken As String, ByVal gmat_taken As String, ByVal toefl_taken As String,
                                                         ByVal ielts_taken As String, ByVal country As String, ByVal prefered_program As String, ByVal stream As String,
                                                         ByVal research_interest As String, ByVal travel_yr As String, ByVal min_budget As String, ByVal max_budget As String,
                                                         ByVal univ_list As String, ByVal state_list As String, ByVal wrk_exp As String,
                                                         ByVal achv As String, ByVal graduation_from_date As String, ByVal graduation_to_date As String,
                                                         ByVal i_name As String, ByVal i_from_date As String, ByVal i_to_date As String, ByVal i_board As String,
                                                         ByVal i_score As String, ByVal s_name As String, ByVal s_from_date As String, ByVal s_to_date As String,
                                                         ByVal s_board As String, ByVal s_score As String, ByVal tab_value As String
                                                        ) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""

        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("User_Profile_Details_New", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@stu_id", SqlDbType.Int)
                .Parameters("@stu_id").Value = SessionUtil.getSession("user_id")
                .Parameters.Add("@stu_name", SqlDbType.VarChar, 30)
                .Parameters("@stu_name").Value = name
                .Parameters.Add("@stu_email", SqlDbType.VarChar, 30)
                .Parameters("@stu_email").Value = email
                .Parameters.Add("@stu_mobile", SqlDbType.VarChar, 20)
                .Parameters("@stu_mobile").Value = mobile
                If String.IsNullOrEmpty(state) = False Then
                    .Parameters.Add("@stu_state_id", SqlDbType.VarChar, 10)
                    .Parameters("@stu_state_id").Value = state
                End If
                If String.IsNullOrEmpty(city) = False Then
                    .Parameters.Add("@stu_city_id", SqlDbType.VarChar, 10)
                    .Parameters("@stu_city_id").Value = city
                End If
                If String.IsNullOrEmpty(gender) = False Then
                    .Parameters.Add("@gender", SqlDbType.VarChar, 10)
                    .Parameters("@gender").Value = gender
                End If
                If String.IsNullOrEmpty(dob) = False Then
                    .Parameters.Add("@dob", SqlDbType.VarChar, 30)
                    .Parameters("@dob").Value = dob
                End If
                If String.IsNullOrEmpty(cob) = False Then
                    .Parameters.Add("@cob", SqlDbType.VarChar, 30)
                    .Parameters("@cob").Value = cob
                End If
                If String.IsNullOrEmpty(f_name) = False Then
                    .Parameters.Add("@f_name", SqlDbType.VarChar, 100)
                    .Parameters("@f_name").Value = f_name
                End If
                If String.IsNullOrEmpty(m_name) = False Then
                    .Parameters.Add("@m_name", SqlDbType.VarChar, 100)
                    .Parameters("@m_name").Value = m_name
                End If
                If String.IsNullOrEmpty(citizenship) = False Then
                    .Parameters.Add("@citizenship", SqlDbType.VarChar, 100)
                    .Parameters("@citizenship").Value = citizenship
                End If
                If String.IsNullOrEmpty(m_addr) = False Then
                    .Parameters.Add("@m_addr", SqlDbType.VarChar, 255)
                    .Parameters("@m_addr").Value = m_addr
                End If
                If String.IsNullOrEmpty(p_addr) = False Then
                    .Parameters.Add("@p_addr", SqlDbType.VarChar, 255)
                    .Parameters("@p_addr").Value = p_addr
                End If
                If String.IsNullOrEmpty(lang) = False Then
                    .Parameters.Add("@lang", SqlDbType.VarChar, 8000)
                    .Parameters("@lang").Value = lang
                End If
                If String.IsNullOrEmpty(bachelor_degree) = False Then
                    .Parameters.Add("@stu_degree", SqlDbType.VarChar, 10)
                    .Parameters("@stu_degree").Value = bachelor_degree
                End If
                'If String.IsNullOrEmpty(cmpl_yr) = False Then
                '    .Parameters.Add("@stu_cmpl_year", SqlDbType.VarChar, 10)
                '    .Parameters("@stu_cmpl_year").Value = cmpl_yr
                'End If
                If String.IsNullOrEmpty(present_stream) = False Then
                    .Parameters.Add("@stu_stream", SqlDbType.VarChar, 10)
                    .Parameters("@stu_stream").Value = present_stream
                End If
                If String.IsNullOrEmpty(college) = False Then
                    .Parameters.Add("@stu_clg_name", SqlDbType.VarChar, 50)
                    .Parameters("@stu_clg_name").Value = college
                End If
                If String.IsNullOrEmpty(backlogs) = False Then
                    .Parameters.Add("@stu_backlogs", SqlDbType.VarChar, 10)
                    .Parameters("@stu_backlogs").Value = backlogs
                End If
                If String.IsNullOrEmpty(cgpa_percent) = False Then
                    .Parameters.Add("@stu_cgpa_percent", SqlDbType.VarChar, 20)
                    .Parameters("@stu_cgpa_percent").Value = cgpa_percent
                End If
                If String.IsNullOrEmpty(gpa_scale) = False Then
                    .Parameters.Add("@stu_gpa_scale", SqlDbType.VarChar, 20)
                    .Parameters("@stu_gpa_scale").Value = gpa_scale
                End If
                .Parameters.Add("@stu_publications", SqlDbType.VarChar, 8000)
                .Parameters("@stu_publications").Value = publications

                If String.IsNullOrEmpty(verbal_gre) = False Then
                    .Parameters.Add("@stu_gre_verbal", SqlDbType.VarChar, 10)
                    .Parameters("@stu_gre_verbal").Value = verbal_gre
                End If
                If String.IsNullOrEmpty(analytical_gre) = False Then
                    .Parameters.Add("@stu_gre_analytical", SqlDbType.VarChar, 10)
                    .Parameters("@stu_gre_analytical").Value = analytical_gre
                End If
                If String.IsNullOrEmpty(quants_gre) = False Then
                    .Parameters.Add("@stu_gre_quants", SqlDbType.VarChar, 10)
                    .Parameters("@stu_gre_quants").Value = quants_gre
                End If
                If String.IsNullOrEmpty(tot_gre) = False Then
                    .Parameters.Add("@stu_gre_tot", SqlDbType.VarChar, 10)
                    .Parameters("@stu_gre_tot").Value = tot_gre
                End If
                If String.IsNullOrEmpty(gre_retake) = False Then
                    .Parameters.Add("@stu_gre_retake", SqlDbType.VarChar, 30)
                    .Parameters("@stu_gre_retake").Value = gre_retake
                End If
                If String.IsNullOrEmpty(est_gre) = False Then
                    .Parameters.Add("@stu_gre_est_score", SqlDbType.VarChar, 10)
                    .Parameters("@stu_gre_est_score").Value = est_gre
                End If
                If String.IsNullOrEmpty(est_date_gre) = False Then
                    .Parameters.Add("@stu_gre_est_date", SqlDbType.VarChar, 30)
                    .Parameters("@stu_gre_est_date").Value = est_date_gre
                End If
                If String.IsNullOrEmpty(tot_gmat) = False Then
                    .Parameters.Add("@stu_gmat_tot", SqlDbType.VarChar, 10)
                    .Parameters("@stu_gmat_tot").Value = tot_gmat
                End If
                If String.IsNullOrEmpty(gmat_retake) = False Then
                    .Parameters.Add("@stu_gmat_retake", SqlDbType.VarChar, 30)
                    .Parameters("@stu_gmat_retake").Value = gmat_retake
                End If
                If String.IsNullOrEmpty(est_gmat) = False Then
                    .Parameters.Add("@stu_gmat_est_score", SqlDbType.VarChar, 10)
                    .Parameters("@stu_gmat_est_score").Value = est_gmat
                End If
                If String.IsNullOrEmpty(est_date_gmat) = False Then
                    .Parameters.Add("@stu_gmat_est_date", SqlDbType.VarChar, 30)
                    .Parameters("@stu_gmat_est_date").Value = est_date_gmat
                End If
                If String.IsNullOrEmpty(reading_toefl) = False Then
                    .Parameters.Add("@stu_toefl_reading", SqlDbType.VarChar, 10)
                    .Parameters("@stu_toefl_reading").Value = reading_toefl
                End If
                If String.IsNullOrEmpty(writing_toefl) = False Then
                    .Parameters.Add("@stu_toefl_writing", SqlDbType.VarChar, 10)
                    .Parameters("@stu_toefl_writing").Value = writing_toefl
                End If
                If String.IsNullOrEmpty(listening_toefl) = False Then
                    .Parameters.Add("@stu_toefl_listening", SqlDbType.VarChar, 10)
                    .Parameters("@stu_toefl_listening").Value = listening_toefl
                End If
                If String.IsNullOrEmpty(speaking_toefl) = False Then
                    .Parameters.Add("@stu_toefl_speaking", SqlDbType.VarChar, 10)
                    .Parameters("@stu_toefl_speaking").Value = speaking_toefl
                End If
                If String.IsNullOrEmpty(tot_toefl) = False Then
                    .Parameters.Add("@stu_toefl_tot", SqlDbType.VarChar, 10)
                    .Parameters("@stu_toefl_tot").Value = tot_toefl
                End If
                If String.IsNullOrEmpty(toefl_retake) = False Then
                    .Parameters.Add("@stu_toefl_retake", SqlDbType.VarChar, 30)
                    .Parameters("@stu_toefl_retake").Value = toefl_retake
                End If
                If String.IsNullOrEmpty(est_toefl) = False Then
                    .Parameters.Add("@stu_toefl_est_score", SqlDbType.VarChar, 10)
                    .Parameters("@stu_toefl_est_score").Value = est_toefl
                End If
                If String.IsNullOrEmpty(est_date_toefl) = False Then
                    .Parameters.Add("@stu_toefl_est_date", SqlDbType.VarChar, 30)
                    .Parameters("@stu_toefl_est_date").Value = est_date_toefl
                End If

                If String.IsNullOrEmpty(reading_ielts) = False Then
                    .Parameters.Add("@stu_ielts_reading", SqlDbType.VarChar, 10)
                    .Parameters("@stu_ielts_reading").Value = reading_ielts
                End If
                If String.IsNullOrEmpty(writing_ielts) = False Then
                    .Parameters.Add("@stu_ielts_writing", SqlDbType.VarChar, 10)
                    .Parameters("@stu_ielts_writing").Value = writing_ielts
                End If
                If String.IsNullOrEmpty(listening_ielts) = False Then
                    .Parameters.Add("@stu_ielts_listening", SqlDbType.VarChar, 10)
                    .Parameters("@stu_ielts_listening").Value = listening_ielts
                End If
                If String.IsNullOrEmpty(speaking_ielts) = False Then
                    .Parameters.Add("@stu_ielts_speaking", SqlDbType.VarChar, 10)
                    .Parameters("@stu_ielts_speaking").Value = speaking_ielts
                End If
                If String.IsNullOrEmpty(tot_ielts) = False Then
                    .Parameters.Add("@stu_ielts_tot", SqlDbType.VarChar, 10)
                    .Parameters("@stu_ielts_tot").Value = tot_ielts
                End If
                If String.IsNullOrEmpty(ielts_retake) = False Then
                    .Parameters.Add("@stu_ielts_retake", SqlDbType.VarChar, 30)
                    .Parameters("@stu_ielts_retake").Value = ielts_retake
                End If
                If String.IsNullOrEmpty(est_ielts) = False Then
                    .Parameters.Add("@stu_ielts_est_score", SqlDbType.VarChar, 10)
                    .Parameters("@stu_ielts_est_score").Value = est_ielts
                End If
                If String.IsNullOrEmpty(est_date_ielts) = False Then
                    .Parameters.Add("@stu_ielts_est_date", SqlDbType.VarChar, 30)
                    .Parameters("@stu_ielts_est_date").Value = est_date_ielts
                End If

                If String.IsNullOrEmpty(gre_taken) = False Then
                    .Parameters.Add("@gre_taken_date", SqlDbType.VarChar, 30)
                    .Parameters("@gre_taken_date").Value = gre_taken
                End If
                If String.IsNullOrEmpty(gmat_taken) = False Then
                    .Parameters.Add("@gmat_taken_date", SqlDbType.VarChar, 30)
                    .Parameters("@gmat_taken_date").Value = gmat_taken
                End If
                If String.IsNullOrEmpty(toefl_taken) = False Then
                    .Parameters.Add("@toefl_taken_date", SqlDbType.VarChar, 30)
                    .Parameters("@toefl_taken_date").Value = toefl_taken
                End If
                If String.IsNullOrEmpty(ielts_taken) = False Then
                    .Parameters.Add("@ielts_taken_date", SqlDbType.VarChar, 30)
                    .Parameters("@ielts_taken_date").Value = ielts_taken
                End If
                If String.IsNullOrEmpty(country) = False Then
                    .Parameters.Add("@stu_country_id", SqlDbType.VarChar, 10)
                    .Parameters("@stu_country_id").Value = country
                End If
                If String.IsNullOrEmpty(prefered_program) = False Then
                    .Parameters.Add("@stu_degree_level", SqlDbType.VarChar, 25)
                    .Parameters("@stu_degree_level").Value = prefered_program
                End If
                If String.IsNullOrEmpty(stream) = False Then
                    .Parameters.Add("@stu_major", SqlDbType.VarChar, 50)
                    .Parameters("@stu_major").Value = stream
                End If
                .Parameters.Add("@stu_research_interest", SqlDbType.VarChar, 50)
                .Parameters("@stu_research_interest").Value = research_interest
                .Parameters.Add("@stu_intake", SqlDbType.VarChar, 50)
                .Parameters("@stu_intake").Value = travel_yr
                If String.IsNullOrEmpty(min_budget) = False Then
                    .Parameters.Add("@stu_budget_min", SqlDbType.VarChar, 15)
                    .Parameters("@stu_budget_min").Value = min_budget
                End If
                If String.IsNullOrEmpty(max_budget) = False Then
                    .Parameters.Add("@stu_budget_max", SqlDbType.VarChar, 15)
                    .Parameters("@stu_budget_max").Value = max_budget
                End If
                Dim u_list As JArray = JArray.Parse(univ_list)
                Dim univ_tab As DataTable = New DataTable
                univ_tab.Columns.Add("univ_name", GetType(String))
                univ_tab.Columns.Add("univ_id", GetType(Integer))
                For i = 0 To u_list.Count - 1
                    univ_tab.Rows.Add(u_list(i).SelectToken("univ_name").ToString(), u_list(i).SelectToken("univ_id").ToString())
                Next
                .Parameters.Add("@universities_list", SqlDbType.Structured).Value = univ_tab



                Dim s_list As JArray = JArray.Parse(state_list)
                Dim state_tab As DataTable = New DataTable
                state_tab.Columns.Add("state_name", GetType(String))
                state_tab.Columns.Add("state_id", GetType(Integer))
                For i = 0 To s_list.Count - 1
                    state_tab.Rows.Add(s_list(i).SelectToken("state_name").ToString(), s_list(i).SelectToken("state_id").ToString())
                Next
                .Parameters.Add("@states_list", SqlDbType.Structured).Value = state_tab

                Dim wrk_exp_list As JArray = JArray.Parse(wrk_exp)
                Dim wrk_exp_tab_new As DataTable = New DataTable
                wrk_exp_tab_new.Columns.Add("cmpny_id", GetType(Integer))
                wrk_exp_tab_new.Columns.Add("cmpny_name", GetType(String))
                wrk_exp_tab_new.Columns.Add("duration", GetType(String))
                wrk_exp_tab_new.Columns.Add("role", GetType(String))
                wrk_exp_tab_new.Columns.Add("role_desc", GetType(String))
                For i = 0 To wrk_exp_list.Count - 1
                    If String.IsNullOrEmpty(wrk_exp_list(i).SelectToken("cmpny_id").ToString()) Then
                        wrk_exp_tab_new.Rows.Add(0, wrk_exp_list(i).SelectToken("cmpny_name").ToString(), wrk_exp_list(i).SelectToken("duration").ToString(), wrk_exp_list(i).SelectToken("role").ToString(), wrk_exp_list(i).SelectToken("role_desc").ToString())
                    Else
                        wrk_exp_tab_new.Rows.Add(wrk_exp_list(i).SelectToken("cmpny_id").ToString(), wrk_exp_list(i).SelectToken("cmpny_name").ToString(), wrk_exp_list(i).SelectToken("duration").ToString(), wrk_exp_list(i).SelectToken("role").ToString(), wrk_exp_list(i).SelectToken("role_desc").ToString())
                    End If

                Next
                .Parameters.Add("@wrk_exp_new", SqlDbType.Structured).Value = wrk_exp_tab_new

                Dim achv_list As JArray = JArray.Parse(achv)
                Dim achv_tab As DataTable = New DataTable
                achv_tab.Columns.Add("achvid", GetType(Integer))
                achv_tab.Columns.Add("desc", GetType(String))
                achv_tab.Columns.Add("duration", GetType(String))
                achv_tab.Columns.Add("position", GetType(String))
                For i = 0 To achv_list.Count - 1
                    If String.IsNullOrEmpty(achv_list(i).SelectToken("achv_id").ToString()) Then
                        achv_tab.Rows.Add(0, achv_list(i).SelectToken("achv_desc").ToString(), achv_list(i).SelectToken("achv_duration").ToString(), achv_list(i).SelectToken("achv_position").ToString())
                    Else
                        achv_tab.Rows.Add(achv_list(i).SelectToken("achv_id").ToString(), achv_list(i).SelectToken("achv_desc").ToString(), achv_list(i).SelectToken("achv_duration").ToString(), achv_list(i).SelectToken("achv_position").ToString())
                    End If

                Next
                .Parameters.Add("@achievements", SqlDbType.Structured).Value = achv_tab


                If String.IsNullOrEmpty(graduation_from_date) = False Then
                    .Parameters.Add("@graduation_from_date", SqlDbType.Date)
                    .Parameters("@graduation_from_date").Value = CDate(graduation_from_date)
                End If
                If String.IsNullOrEmpty(graduation_to_date) = False Then
                    .Parameters.Add("@graduation_to_date", SqlDbType.Date)
                    .Parameters("@graduation_to_date").Value = CDate(graduation_to_date)
                End If
                .Parameters.Add("@i_name", SqlDbType.VarChar, 100)
                .Parameters("@i_name").Value = i_name
                If String.IsNullOrEmpty(i_from_date) = False Then
                    .Parameters.Add("@i_from_date", SqlDbType.Date)
                    .Parameters("@i_from_date").Value = CDate(i_from_date)
                End If
                If String.IsNullOrEmpty(i_to_date) = False Then
                    .Parameters.Add("@i_to_date", SqlDbType.Date)
                    .Parameters("@i_to_date").Value = CDate(i_to_date)
                End If

                .Parameters.Add("@i_board", SqlDbType.VarChar, 10)
                .Parameters("@i_board").Value = i_board
                .Parameters.Add("@i_score", SqlDbType.VarChar, 10)
                .Parameters("@i_score").Value = i_score

                .Parameters.Add("@s_name", SqlDbType.VarChar, 100)
                .Parameters("@s_name").Value = s_name
                If String.IsNullOrEmpty(s_from_date) = False Then
                    .Parameters.Add("@s_from_date", SqlDbType.Date)
                    .Parameters("@s_from_date").Value = CDate(s_from_date)
                End If
                If String.IsNullOrEmpty(s_to_date) = False Then
                    .Parameters.Add("@s_to_date", SqlDbType.Date)
                    .Parameters("@s_to_date").Value = CDate(s_to_date)
                End If

                .Parameters.Add("@s_board", SqlDbType.VarChar, 10)
                .Parameters("@s_board").Value = s_board
                .Parameters.Add("@s_score", SqlDbType.VarChar, 10)
                .Parameters("@s_score").Value = s_score

                If String.IsNullOrEmpty(tab_value) = False Then
                    .Parameters.Add("@tab_value", SqlDbType.VarChar, 10)
                    .Parameters("@tab_value").Value = tab_value
                End If
                sqlcmd.Parameters.Add("@ret_mob_val", SqlDbType.Int).Direction = ParameterDirection.Output
            End With


            sqldata.SelectCommand = sqlcmd
            sqldata.Fill(ds)


            restr = sqlcmd.Parameters("@ret_mob_val").Value
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr

    End Function
    Public Shared Function Get_Cities(ByVal state_id As Integer) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("Get_Cities", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@state_id", SqlDbType.Int)
                .Parameters("@state_id").Value = state_id

            End With
            sqldata.SelectCommand = sqlcmd
            sqldata.Fill(ds)

            With ds.Tables(0)
                For i As Integer = 0 To .Rows.Count - 1
                    If i = 0 Then
                        restr = "{""city_id"":""" & .Rows.Item(i)("id") & """, ""city_name"":""" & .Rows.Item(i)("city_name") & """}"
                    Else
                        restr += ",{""city_id"":""" & .Rows.Item(i)("id") & """, ""city_name"":""" & .Rows.Item(i)("city_name") & """}"
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
    Public Shared Function Submit_Student_Details(ByVal name As String, ByVal email As String, ByVal mobile As String, ByVal state As String,
                                                         ByVal city As String, ByVal univ_list As String, ByVal state_list As String, ByVal bachelor_degree As String,
                                                         ByVal cmpl_yr As String, ByVal present_stream As String, ByVal college As String, ByVal backlogs As String,
                                                         ByVal country As String, ByVal prefered_program As String, ByVal stream As String,
                                                         ByVal research_interest As String, ByVal travel_yr As String, ByVal min_budget As String, ByVal max_budget As String,
                                                         ByVal exp_year As String, ByVal exp_month As String, ByVal publications As String, ByVal extra_cur As String,
                                                         ByVal co_cur As String, ByVal cgpa_percent As String, ByVal gpa_scale As String, ByVal verbal_gre As String,
                                                         ByVal analytical_gre As String, ByVal quants_gre As String, ByVal tot_gre As String, ByVal gre_retake As String,
                                                         ByVal est_gre As String, ByVal est_date_gre As String, ByVal tot_gmat As String, ByVal gmat_retake As String,
                                                         ByVal est_gmat As String, ByVal est_date_gmat As String, ByVal reading_toefl As String, ByVal writing_toefl As String,
                                                         ByVal listening_toefl As String, ByVal speaking_toefl As String, ByVal tot_toefl As String, ByVal toefl_retake As String,
                                                         ByVal est_toefl As String, ByVal est_date_toefl As String, ByVal reading_ielts As String, ByVal writing_ielts As String,
                                                         ByVal listening_ielts As String, ByVal speaking_ielts As String, ByVal tot_ielts As String, ByVal ielts_retake As String,
                                                         ByVal est_ielts As String, ByVal est_date_ielts As String, ByVal wrk_exp As String, ByVal gender As String,
                                                         ByVal dob As String, ByVal cob As String, ByVal f_name As String, ByVal m_name As String, ByVal citizenship As String,
                                                         ByVal m_addr As String, ByVal p_addr As String, ByVal lang As String, ByVal bachelor_arr As String,
                                                         ByVal sponsor As String, ByVal gre_taken As String, ByVal gmat_taken As String, ByVal toefl_taken As String,
                                                         ByVal ielts_taken As String
                                                         ) As String

        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""

        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("User_Profile_Details", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@stu_id", SqlDbType.Int)
                .Parameters("@stu_id").Value = SessionUtil.getSession("user_id")
                .Parameters.Add("@stu_name", SqlDbType.VarChar, 30)
                .Parameters("@stu_name").Value = name
                .Parameters.Add("@stu_email", SqlDbType.VarChar, 30)
                .Parameters("@stu_email").Value = email
                .Parameters.Add("@stu_mobile", SqlDbType.VarChar, 20)
                .Parameters("@stu_mobile").Value = mobile
                If String.IsNullOrEmpty(gender) = False Then
                    .Parameters.Add("@gender", SqlDbType.VarChar, 10)
                    .Parameters("@gender").Value = gender
                End If
                If String.IsNullOrEmpty(dob) = False Then
                    .Parameters.Add("@dob", SqlDbType.VarChar, 30)
                    .Parameters("@dob").Value = dob
                End If
                If String.IsNullOrEmpty(cob) = False Then
                    .Parameters.Add("@cob", SqlDbType.VarChar, 30)
                    .Parameters("@cob").Value = cob
                End If
                If String.IsNullOrEmpty(f_name) = False Then
                    .Parameters.Add("@f_name", SqlDbType.VarChar, 100)
                    .Parameters("@f_name").Value = f_name
                End If
                If String.IsNullOrEmpty(m_name) = False Then
                    .Parameters.Add("@m_name", SqlDbType.VarChar, 100)
                    .Parameters("@m_name").Value = m_name
                End If
                If String.IsNullOrEmpty(citizenship) = False Then
                    .Parameters.Add("@citizenship", SqlDbType.VarChar, 100)
                    .Parameters("@citizenship").Value = citizenship
                End If
                If String.IsNullOrEmpty(m_addr) = False Then
                    .Parameters.Add("@m_addr", SqlDbType.VarChar, 255)
                    .Parameters("@m_addr").Value = m_addr
                End If
                If String.IsNullOrEmpty(p_addr) = False Then
                    .Parameters.Add("@p_addr", SqlDbType.VarChar, 255)
                    .Parameters("@p_addr").Value = p_addr
                End If
                If String.IsNullOrEmpty(lang) = False Then
                    .Parameters.Add("@lang", SqlDbType.VarChar, 150)
                    .Parameters("@lang").Value = lang
                End If
                If String.IsNullOrEmpty(state) = False Then
                    .Parameters.Add("@stu_state_id", SqlDbType.VarChar, 10)
                    .Parameters("@stu_state_id").Value = state
                End If
                If String.IsNullOrEmpty(city) = False Then
                    .Parameters.Add("@stu_city_id", SqlDbType.VarChar, 10)
                    .Parameters("@stu_city_id").Value = city
                End If
                If String.IsNullOrEmpty(bachelor_degree) = False Then
                    .Parameters.Add("@stu_degree", SqlDbType.VarChar, 10)
                    .Parameters("@stu_degree").Value = bachelor_degree
                End If
                If String.IsNullOrEmpty(cmpl_yr) = False Then
                    .Parameters.Add("@stu_cmpl_year", SqlDbType.VarChar, 10)
                    .Parameters("@stu_cmpl_year").Value = cmpl_yr
                End If
                If String.IsNullOrEmpty(present_stream) = False Then
                    .Parameters.Add("@stu_stream", SqlDbType.VarChar, 10)
                    .Parameters("@stu_stream").Value = present_stream
                End If
                If String.IsNullOrEmpty(college) = False Then
                    .Parameters.Add("@stu_clg_name", SqlDbType.VarChar, 50)
                    .Parameters("@stu_clg_name").Value = college
                End If
                If String.IsNullOrEmpty(backlogs) = False Then
                    .Parameters.Add("@stu_backlogs", SqlDbType.VarChar, 10)
                    .Parameters("@stu_backlogs").Value = backlogs
                End If
                If String.IsNullOrEmpty(country) = False Then
                    .Parameters.Add("@stu_country_id", SqlDbType.VarChar, 10)
                    .Parameters("@stu_country_id").Value = country
                End If
                If String.IsNullOrEmpty(prefered_program) = False Then
                    .Parameters.Add("@stu_degree_level", SqlDbType.VarChar, 25)
                    .Parameters("@stu_degree_level").Value = prefered_program
                End If
                If String.IsNullOrEmpty(stream) = False Then
                    .Parameters.Add("@stu_major", SqlDbType.VarChar, 50)
                    .Parameters("@stu_major").Value = stream
                End If
                .Parameters.Add("@stu_research_interest", SqlDbType.VarChar, 50)
                .Parameters("@stu_research_interest").Value = research_interest
                .Parameters.Add("@stu_intake", SqlDbType.VarChar, 50)
                .Parameters("@stu_intake").Value = travel_yr
                If String.IsNullOrEmpty(min_budget) = False Then
                    .Parameters.Add("@stu_budget_min", SqlDbType.VarChar, 15)
                    .Parameters("@stu_budget_min").Value = min_budget
                End If
                If String.IsNullOrEmpty(max_budget) = False Then
                    .Parameters.Add("@stu_budget_max", SqlDbType.VarChar, 15)
                    .Parameters("@stu_budget_max").Value = max_budget
                End If
                .Parameters.Add("@stu_wrk_exp_years", SqlDbType.VarChar, 15)
                .Parameters("@stu_wrk_exp_years").Value = exp_year
                .Parameters.Add("@stu_wrk_exp_months", SqlDbType.VarChar, 15)
                .Parameters("@stu_wrk_exp_months").Value = exp_month
                .Parameters.Add("@stu_publications", SqlDbType.VarChar, 8000)
                .Parameters("@stu_publications").Value = publications
                .Parameters.Add("@stu_extra_cur", SqlDbType.VarChar, 8000)
                .Parameters("@stu_extra_cur").Value = extra_cur
                .Parameters.Add("@stu_co_cur", SqlDbType.VarChar, 8000)
                .Parameters("@stu_co_cur").Value = co_cur
                If String.IsNullOrEmpty(cgpa_percent) = False Then
                    .Parameters.Add("@stu_cgpa_percent", SqlDbType.VarChar, 20)
                    .Parameters("@stu_cgpa_percent").Value = cgpa_percent
                End If
                If String.IsNullOrEmpty(gpa_scale) = False Then
                    .Parameters.Add("@stu_gpa_scale", SqlDbType.VarChar, 20)
                    .Parameters("@stu_gpa_scale").Value = gpa_scale
                End If
                If String.IsNullOrEmpty(verbal_gre) = False Then
                    .Parameters.Add("@stu_gre_verbal", SqlDbType.VarChar, 10)
                    .Parameters("@stu_gre_verbal").Value = verbal_gre
                End If
                If String.IsNullOrEmpty(analytical_gre) = False Then
                    .Parameters.Add("@stu_gre_analytical", SqlDbType.VarChar, 10)
                    .Parameters("@stu_gre_analytical").Value = analytical_gre
                End If
                If String.IsNullOrEmpty(quants_gre) = False Then
                    .Parameters.Add("@stu_gre_quants", SqlDbType.VarChar, 10)
                    .Parameters("@stu_gre_quants").Value = quants_gre
                End If
                If String.IsNullOrEmpty(tot_gre) = False Then
                    .Parameters.Add("@stu_gre_tot", SqlDbType.VarChar, 10)
                    .Parameters("@stu_gre_tot").Value = tot_gre
                End If
                .Parameters.Add("@stu_gre_retake", SqlDbType.VarChar, 30)
                .Parameters("@stu_gre_retake").Value = gre_retake
                If String.IsNullOrEmpty(est_gre) = False Then
                    .Parameters.Add("@stu_gre_est_score", SqlDbType.VarChar, 10)
                    .Parameters("@stu_gre_est_score").Value = est_gre
                End If
                .Parameters.Add("@stu_gre_est_date", SqlDbType.VarChar, 30)
                .Parameters("@stu_gre_est_date").Value = est_date_gre
                If String.IsNullOrEmpty(tot_gmat) = False Then
                    .Parameters.Add("@stu_gmat_tot", SqlDbType.VarChar, 10)
                    .Parameters("@stu_gmat_tot").Value = tot_gmat
                End If
                .Parameters.Add("@stu_gmat_retake", SqlDbType.VarChar, 30)
                .Parameters("@stu_gmat_retake").Value = gmat_retake
                If String.IsNullOrEmpty(est_gmat) = False Then
                    .Parameters.Add("@stu_gmat_est_score", SqlDbType.VarChar, 10)
                    .Parameters("@stu_gmat_est_score").Value = est_gmat
                End If
                .Parameters.Add("@stu_gmat_est_date", SqlDbType.VarChar, 30)
                .Parameters("@stu_gmat_est_date").Value = est_date_gmat

                If String.IsNullOrEmpty(reading_toefl) = False Then
                    .Parameters.Add("@stu_toefl_reading", SqlDbType.VarChar, 10)
                    .Parameters("@stu_toefl_reading").Value = reading_toefl
                End If
                If String.IsNullOrEmpty(writing_toefl) = False Then
                    .Parameters.Add("@stu_toefl_writing", SqlDbType.VarChar, 10)
                    .Parameters("@stu_toefl_writing").Value = writing_toefl
                End If
                If String.IsNullOrEmpty(listening_toefl) = False Then
                    .Parameters.Add("@stu_toefl_listening", SqlDbType.VarChar, 10)
                    .Parameters("@stu_toefl_listening").Value = listening_toefl
                End If
                If String.IsNullOrEmpty(speaking_toefl) = False Then
                    .Parameters.Add("@stu_toefl_speaking", SqlDbType.VarChar, 10)
                    .Parameters("@stu_toefl_speaking").Value = speaking_toefl
                End If
                If String.IsNullOrEmpty(tot_toefl) = False Then
                    .Parameters.Add("@stu_toefl_tot", SqlDbType.VarChar, 10)
                    .Parameters("@stu_toefl_tot").Value = tot_toefl
                End If
                .Parameters.Add("@stu_toefl_retake", SqlDbType.VarChar, 30)
                .Parameters("@stu_toefl_retake").Value = toefl_retake
                If String.IsNullOrEmpty(est_toefl) = False Then
                    .Parameters.Add("@stu_toefl_est_score", SqlDbType.VarChar, 10)
                    .Parameters("@stu_toefl_est_score").Value = est_toefl
                End If
                .Parameters.Add("@stu_toefl_est_date", SqlDbType.VarChar, 30)
                .Parameters("@stu_toefl_est_date").Value = est_date_toefl

                If String.IsNullOrEmpty(reading_ielts) = False Then
                    .Parameters.Add("@stu_ielts_reading", SqlDbType.VarChar, 10)
                    .Parameters("@stu_ielts_reading").Value = reading_ielts
                End If
                If String.IsNullOrEmpty(writing_ielts) = False Then
                    .Parameters.Add("@stu_ielts_writing", SqlDbType.VarChar, 10)
                    .Parameters("@stu_ielts_writing").Value = writing_ielts
                End If
                If String.IsNullOrEmpty(listening_ielts) = False Then
                    .Parameters.Add("@stu_ielts_listening", SqlDbType.VarChar, 10)
                    .Parameters("@stu_ielts_listening").Value = listening_ielts
                End If
                If String.IsNullOrEmpty(speaking_ielts) = False Then
                    .Parameters.Add("@stu_ielts_speaking", SqlDbType.VarChar, 10)
                    .Parameters("@stu_ielts_speaking").Value = speaking_ielts
                End If
                If String.IsNullOrEmpty(tot_ielts) = False Then
                    .Parameters.Add("@stu_ielts_tot", SqlDbType.VarChar, 10)
                    .Parameters("@stu_ielts_tot").Value = tot_ielts
                End If
                .Parameters.Add("@stu_ielts_retake", SqlDbType.VarChar, 30)
                .Parameters("@stu_ielts_retake").Value = ielts_retake
                If String.IsNullOrEmpty(est_ielts) = False Then
                    .Parameters.Add("@stu_ielts_est_score", SqlDbType.VarChar, 10)
                    .Parameters("@stu_ielts_est_score").Value = est_ielts
                End If
                .Parameters.Add("@stu_ielts_est_date", SqlDbType.VarChar, 30)
                .Parameters("@stu_ielts_est_date").Value = est_date_ielts

                If String.IsNullOrEmpty(gre_taken) = False Then
                    .Parameters.Add("@gre_taken_date", SqlDbType.VarChar, 30)
                    .Parameters("@gre_taken_date").Value = gre_taken
                End If
                If String.IsNullOrEmpty(gmat_taken) = False Then
                    .Parameters.Add("@gmat_taken_date", SqlDbType.VarChar, 30)
                    .Parameters("@gmat_taken_date").Value = gmat_taken
                End If
                If String.IsNullOrEmpty(toefl_taken) = False Then
                    .Parameters.Add("@toefl_taken_date", SqlDbType.VarChar, 30)
                    .Parameters("@toefl_taken_date").Value = toefl_taken
                End If
                If String.IsNullOrEmpty(ielts_taken) = False Then
                    .Parameters.Add("@ielts_taken_date", SqlDbType.VarChar, 30)
                    .Parameters("@ielts_taken_date").Value = ielts_taken
                End If

                Dim u_list As JArray = JArray.Parse(univ_list)
                Dim univ_tab As DataTable = New DataTable
                univ_tab.Columns.Add("univ_name", GetType(String))
                univ_tab.Columns.Add("univ_id", GetType(Integer))
                For i = 0 To u_list.Count - 1
                    univ_tab.Rows.Add(u_list(i).SelectToken("univ_name").ToString(), u_list(i).SelectToken("univ_id").ToString())
                Next
                .Parameters.Add("@universities_list", SqlDbType.Structured).Value = univ_tab



                Dim s_list As JArray = JArray.Parse(state_list)
                Dim state_tab As DataTable = New DataTable
                state_tab.Columns.Add("state_name", GetType(String))
                state_tab.Columns.Add("state_id", GetType(Integer))
                For i = 0 To s_list.Count - 1
                    state_tab.Rows.Add(s_list(i).SelectToken("state_name").ToString(), s_list(i).SelectToken("state_id").ToString())
                Next
                .Parameters.Add("@states_list", SqlDbType.Structured).Value = state_tab

                'Dim extra_achv_list As JArray = JArray.Parse(extra_achv)
                'Dim extra_achv_tab As DataTable = New DataTable
                'extra_achv_tab.Columns.Add("extra_desc", GetType(String))
                'extra_achv_tab.Columns.Add("extra_duration", GetType(String))
                'extra_achv_tab.Columns.Add("extra_position", GetType(String))
                'For i = 0 To extra_achv_list.Count - 1
                '    extra_achv_tab.Rows.Add(extra_achv_list(i).SelectToken("extra_desc").ToString(), extra_achv_list(i).SelectToken("extra_duration").ToString(), extra_achv_list(i).SelectToken("extra_position").ToString())
                'Next
                '.Parameters.Add("@extra_achv", SqlDbType.Structured).Value = extra_achv_tab

                Dim wrk_exp_list As JArray = JArray.Parse(wrk_exp)
                Dim wrk_exp_tab_new As DataTable = New DataTable
                wrk_exp_tab_new.Columns.Add("cmpny_name", GetType(String))
                wrk_exp_tab_new.Columns.Add("duration", GetType(String))
                'wrk_exp_tab_new.Columns.Add("exp_yrs", GetType(String))
                wrk_exp_tab_new.Columns.Add("role", GetType(String))
                wrk_exp_tab_new.Columns.Add("role_desc", GetType(String))
                For i = 0 To wrk_exp_list.Count - 1
                    wrk_exp_tab_new.Rows.Add(wrk_exp_list(i).SelectToken("cmpny_name").ToString(), wrk_exp_list(i).SelectToken("duration").ToString(), wrk_exp_list(i).SelectToken("role").ToString(), wrk_exp_list(i).SelectToken("role_desc").ToString())
                Next
                .Parameters.Add("@wrk_exp_new", SqlDbType.Structured).Value = wrk_exp_tab_new



                Dim sponsor_list As JArray = JArray.Parse(sponsor)
                Dim sponsor_tab As DataTable = New DataTable
                sponsor_tab.Columns.Add("relation", GetType(String))
                sponsor_tab.Columns.Add("name", GetType(String))
                sponsor_tab.Columns.Add("percent", GetType(String))
                For i = 0 To sponsor_list.Count - 1
                    sponsor_tab.Rows.Add(sponsor_list(i).SelectToken("relation").ToString(), sponsor_list(i).SelectToken("name").ToString(), sponsor_list(i).SelectToken("percent").ToString())
                Next
                .Parameters.Add("@sponsor", SqlDbType.Structured).Value = sponsor_tab

                ' HttpContext.Current.Response.Write("hiiiiiiiiii")
                'HttpContext.Current.Response.End()

                Dim academic_list As JArray = JArray.Parse(bachelor_arr)
                Dim academic_tab As DataTable = New DataTable
                academic_tab.Columns.Add("degree", GetType(Integer))
                academic_tab.Columns.Add("degree_level", GetType(String))
                academic_tab.Columns.Add("major", GetType(String))
                academic_tab.Columns.Add("univ", GetType(String))
                academic_tab.Columns.Add("atnd_dates", GetType(String))
                academic_tab.Columns.Add("grad_yr", GetType(String))
                academic_tab.Columns.Add("grade", GetType(String))
                For i = 0 To academic_list.Count - 1
                    If String.IsNullOrEmpty(academic_list(i).SelectToken("degree").ToString()) = False Then
                        academic_tab.Rows.Add(academic_list(i).SelectToken("degree").ToString(), academic_list(i).SelectToken("degree_level").ToString(), academic_list(i).SelectToken("major").ToString(), academic_list(i).SelectToken("univ").ToString(), academic_list(i).SelectToken("atnd_dates").ToString(), academic_list(i).SelectToken("grad_date").ToString(), academic_list(i).SelectToken("grade").ToString())
                    End If
                Next
                .Parameters.Add("@academics", SqlDbType.Structured).Value = academic_tab



                sqlcmd.Parameters.Add("@ret_mob_val", SqlDbType.Int).Direction = ParameterDirection.Output
            End With
            sqldata.SelectCommand = sqlcmd
            sqldata.Fill(ds)

            restr = sqlcmd.Parameters("@ret_mob_val").Value

        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
            'HttpContext.Current.Response.End()
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr

    End Function
    Public Shared Function Get_Universities_List(ByVal search_univ_term As String) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = "", item1 As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("Get_universities_for_interested", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@univ_name", SqlDbType.VarChar, 50)
                .Parameters("@univ_name").Value = search_univ_term

            End With
            sqldata.SelectCommand = sqlcmd
            sqldata.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                With ds.Tables(0)
                    For i As Integer = 0 To .Rows.Count - 1
                        If i = 0 Then
                            item1 = item1 & "{""id"":""" & .Rows(i)("University_Id") & """,""name"":""" & .Rows(i)("University_Name") & """}"
                        Else
                            item1 = item1 & ",{""id"":""" & .Rows(i)("University_Id") & """,""name"":""" & .Rows(i)("University_Name") & """}"
                        End If
                    Next i
                End With
            End If
            restr = "[" + item1 + "]"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr

    End Function
    Public Shared Function Get_States_List(ByVal search_state_term As String) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = "", item1 As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("Get_states_for_interested", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add("@state_name", SqlDbType.VarChar, 50)
                .Parameters("@state_name").Value = search_state_term

            End With
            sqldata.SelectCommand = sqlcmd
            sqldata.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                With ds.Tables(0)
                    For i As Integer = 0 To .Rows.Count - 1
                        If i = 0 Then
                            item1 = item1 & "{""id"":""" & .Rows(i)("id") & """,""name"":""" & .Rows(i)("state_name") & """}"
                        Else
                            item1 = item1 & ",{""id"":""" & .Rows(i)("id") & """,""name"":""" & .Rows(i)("state_name") & """}"
                        End If
                    Next i
                End With
            End If
            restr = "[" + item1 + "]"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr

    End Function
    Public Shared Function Get_Currency_Conversions(ByVal cntry_val As String, ByVal min_val As String, ByVal max_val As String) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("Get_Currency_Conversions", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@min_value", SqlDbType.VarChar, 20)
                .Parameters("@min_value").Value = min_val
                .Parameters.Add("@max_value", SqlDbType.VarChar, 20)
                .Parameters("@max_value").Value = max_val
                .Parameters.Add("@cntry_val", SqlDbType.VarChar, 20)
                .Parameters("@cntry_val").Value = cntry_val

                .Parameters.Add("@currency_min", SqlDbType.Int, 50).Direction = ParameterDirection.Output
                .Parameters.Add("@currency_max", SqlDbType.Int, 50).Direction = ParameterDirection.Output
                .Parameters.Add("@currency_name", SqlDbType.VarChar, 20).Direction = ParameterDirection.Output

            End With
            sqldata.SelectCommand = sqlcmd
            sqldata.Fill(ds)


            restr = "{""currency_min"":""" & sqlcmd.Parameters("@currency_min").Value & """,""currency_max"":""" & sqlcmd.Parameters("@currency_max").Value & """,""currency_name"":""" & sqlcmd.Parameters("@currency_name").Value & """}"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr

    End Function
    Public Shared Function Student_Mobile_Check(ByVal mobile As String) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("Student_Mobile_Check", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@mobile", SqlDbType.VarChar, 15)
                .Parameters("@mobile").Value = mobile
                .Parameters.Add("@stu_id", SqlDbType.Int)
                .Parameters("@stu_id").Value = 4798
                .Parameters.Add("@ret_val", SqlDbType.Int).Direction = ParameterDirection.Output
            End With
            sqldata.SelectCommand = sqlcmd
            sqldata.Fill(ds)


            restr = "{""ret_val"":""" & sqlcmd.Parameters("@ret_val").Value & """}"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr

    End Function
    Public Shared Function Student_Email_Check(ByVal email As String) As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("Student_Email_Check", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@email", SqlDbType.VarChar, 50)
                .Parameters("@email").Value = email
                .Parameters.Add("@stu_id", SqlDbType.Int)
                .Parameters("@stu_id").Value = 4798
                .Parameters.Add("@ret_val", SqlDbType.Int).Direction = ParameterDirection.Output
            End With
            sqldata.SelectCommand = sqlcmd
            sqldata.Fill(ds)


            restr = "{""ret_val"":""" & sqlcmd.Parameters("@ret_val").Value & """}"
        Catch ex As Exception
            HttpContext.Current.Response.Write(ex.ToString())
        Finally
            sqlcmd = Nothing
            If sqlcon IsNot Nothing Then sqlcon.Dispose()
            sqlcon = Nothing
        End Try
        Return restr

    End Function
    Public Shared Function Get_States() As String
        Dim sqlcon As SqlConnection = Nothing, sqlcmd As SqlCommand = Nothing, sqldata As New SqlDataAdapter, ds As New DataSet
        Dim restr As String = "", states As String = "", bachelor_stream As String = "", bachelor_degree As String = "", majors As String = "",
            country As String = "", univ_list As String = "", states_list As String = "", academic_degree_levels As String = "",
            languages_list As String = ""
        Try

            sqlcon = New SqlConnection(ConString)
            sqlcmd = New SqlCommand("Get_States_Initial", sqlcon)
            With sqlcmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add("@stu_id", SqlDbType.Int)
                .Parameters("@stu_id").Value = SessionUtil.getSession("user_id")
            End With
            sqldata.SelectCommand = sqlcmd
            sqldata.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                With ds.Tables(0)
                    For i As Integer = 0 To .Rows.Count - 1
                        If i = 0 Then
                            states = states & "{""id"":""" & .Rows(i)("id") & """,""name"":""" & .Rows(i)("state_name") & """}"
                        Else
                            states = states & ",{""id"":""" & .Rows(i)("id") & """,""name"":""" & .Rows(i)("state_name") & """}"
                        End If
                    Next i
                End With
            End If

            If ds.Tables(1).Rows.Count > 0 Then
                With ds.Tables(1)
                    For i As Integer = 0 To .Rows.Count - 1
                        If i = 0 Then
                            bachelor_degree = bachelor_degree & "{""id"":""" & .Rows(i)("bachelor_degree_id") & """,""name"":""" & .Rows(i)("degree_name") & """}"
                        Else
                            bachelor_degree = bachelor_degree & ",{""id"":""" & .Rows(i)("bachelor_degree_id") & """,""name"":""" & .Rows(i)("degree_name") & """}"
                        End If
                    Next i
                End With
            End If

            If ds.Tables(2).Rows.Count > 0 Then
                With ds.Tables(2)
                    For i As Integer = 0 To .Rows.Count - 1
                        If i = 0 Then
                            bachelor_stream = bachelor_stream & "{""id"":""" & .Rows(i)("Student_Department_ID") & """,""name"":""" & .Rows(i)("Student_Department_Name") & """}"
                        Else
                            bachelor_stream = bachelor_stream & ",{""id"":""" & .Rows(i)("Student_Department_ID") & """,""name"":""" & .Rows(i)("Student_Department_Name") & """}"
                        End If
                    Next i
                End With
            End If

            If ds.Tables(3).Rows.Count > 0 Then
                With ds.Tables(3)
                    For i As Integer = 0 To .Rows.Count - 1
                        If i = 0 Then
                            majors = majors & "{""id"":""" & .Rows(i)("id") & """,""name"":""" & .Rows(i)("name") & """}"
                        Else
                            majors = majors & ",{""id"":""" & .Rows(i)("id") & """,""name"":""" & .Rows(i)("name") & """}"
                        End If
                    Next i
                End With
            End If

            If ds.Tables(4).Rows.Count > 0 Then
                With ds.Tables(4)
                    For i As Integer = 0 To .Rows.Count - 1
                        If i = 0 Then
                            country = country & "{""id"":""" & .Rows(i)("country_id") & """,""name"":""" & .Rows(i)("country_name") & """}"
                        Else
                            country = country & ",{""id"":""" & .Rows(i)("country_id") & """,""name"":""" & .Rows(i)("country_name") & """}"
                        End If
                    Next i
                End With
            End If

            If ds.Tables(5).Rows.Count > 0 Then
                With ds.Tables(5)
                    For i As Integer = 0 To .Rows.Count - 1
                        If i = 0 Then
                            univ_list = univ_list & "{""id"":""" & .Rows(i)("University_Id") & """,""name"":""" & .Rows(i)("University_Name") & """}"
                        Else
                            univ_list = univ_list & ",{""id"":""" & .Rows(i)("University_Id") & """,""name"":""" & .Rows(i)("University_Name") & """}"
                        End If
                    Next i
                End With
            End If

            If ds.Tables(6).Rows.Count > 0 Then
                With ds.Tables(6)
                    For i As Integer = 0 To .Rows.Count - 1
                        If i = 0 Then
                            states_list = states_list & "{""id"":""" & .Rows(i)("State_id") & """,""name"":""" & .Rows(i)("State_name") & """}"
                        Else
                            states_list = states_list & ",{""id"":""" & .Rows(i)("State_id") & """,""name"":""" & .Rows(i)("State_name") & """}"
                        End If
                    Next i
                End With
            End If

            If ds.Tables(7).Rows.Count > 0 Then
                With ds.Tables(7)
                    For i As Integer = 0 To .Rows.Count - 1
                        If i = 0 Then
                            academic_degree_levels = academic_degree_levels & "{""id"":""" & .Rows(i)("student_degree_id") & """,""name"":""" & .Rows(i)("student_degree_name") & """}"
                        Else
                            academic_degree_levels = academic_degree_levels & ",{""id"":""" & .Rows(i)("student_degree_id") & """,""name"":""" & .Rows(i)("student_degree_name") & """}"
                        End If
                    Next i
                End With
            End If


            'If ds.Tables(8).Rows.Count > 0 Then
            '    With ds.Tables(8)
            '        For i As Integer = 0 To .Rows.Count - 1
            '            If i = 0 Then
            '                languages_list = languages_list & "{""id"":""" & .Rows(i)("language_id") & """,""name"":""" & .Rows(i)("language_name") & """}"
            '            Else
            '                languages_list = languages_list & ",{""id"":""" & .Rows(i)("language_id") & """,""name"":""" & .Rows(i)("language_name") & """}"
            '            End If
            '        Next i
            '    End With
            'End If
            'restr = "{""items"":[" & states & "],""items1"":[" & bachelor_degree & "],""items2"":[" & bachelor_stream & "],""items3"":[" & majors & "],""items4"":[" & country & "],""items5"":[" & univ_list & "],""items6"":[" & states_list & "],""items7"":[" & academic_degree_levels & "],""items8"":[" & languages_list & "]}"
            restr = "{""items"":[" & states & "],""items1"":[" & bachelor_degree & "],""items2"":[" & bachelor_stream & "],""items3"":[" & majors & "],""items4"":[" & country & "],""items5"":[" & univ_list & "],""items6"":[" & states_list & "],""items7"":[" & academic_degree_levels & "]}"
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
