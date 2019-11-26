Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities

Public Class EmployeeDetails
    Inherits System.Web.UI.Page
    Public Shared employee_number As String
    Dim _Employee As EmployeeBusinessLayer = New EmployeeBusinessLayer
    Dim _blErrorLogging As New ErrorLogBL



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim url As String = Request.Url.AbsoluteUri

            If Not url.Contains("localhost") Then
                If Session("username") = "" Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Default.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                    End If
                Else
                    If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.HREmployeeReviewReport) Then
                        If Not IsCallback Then
                            Response.Redirect("~/Intranet/Welcome.aspx")
                        Else
                            ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                        End If
                    End If
                End If
            End If

            employee_number = Request.QueryString("id")
            If employee_number = "" Then
                Response.Redirect("~/Reports/HR/EmployeeReview.aspx")
            End If

            GetData()
        End If

    End Sub

    Private Sub GetData()
        GetReviews()
        GetQuestionnaire()
        GetEmployeeDetails()
        BindEmployeesDropdown()
    End Sub

    Protected Sub EmployeeQuestionnaire_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Private Sub GetReviews()
        Dim _ReviewList As DataSet
        _ReviewList = _Employee.GetEmployeeReviews(employee_number)
        If _ReviewList IsNot Nothing AndAlso _ReviewList.Tables(0).Rows.Count > 0 Then
            gvReview.DataSource = _ReviewList.Tables(0)
            gvReview.DataBind()
        End If

    End Sub

    Private Function GetQuestionnaire()
        Dim _SurveysSummary As DataSet
        _SurveysSummary = _Employee.GetSurveysSummary(employee_number)
        Session("_EmployeeQuestionnaireDS") = _SurveysSummary
        gvMaster.DataBind()
        Return Session("_EmployeeQuestionnaireDS")
    End Function

    Protected Sub gvMaster_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        gvMaster.BeginUpdate()
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataSet = Session("_EmployeeQuestionnaireDS")
        gridView.KeyFieldName = "completed_survey_id" 'data.PrimaryKey(0).ColumnName
        gridView.DataSource = data
        gvMaster.EndUpdate()

    End Sub

    Protected Sub gvDetail_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim _SurveyDetailsSummary As DataSet
        Try
            Dim completed_survey_id As String = gridView.GetMasterRowKeyValue()
            _SurveyDetailsSummary = _Employee.GetSurveyDetailsSummary(completed_survey_id)
            Dim data As DataSet = _SurveyDetailsSummary
            gridView.DataSource = data
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub
    Private Sub GetEmployeeDetails()
        Dim _dt As DataTable
        _dt = _Employee.GetEmployees(employee_number)
        If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then
            EmployeeNumber.Text = Convert.ToString(_dt.Rows(0)("employee_number"))
            EmployeeIDNumber.Text = Convert.ToString(_dt.Rows(0)("id_number"))
            EmployeeFN.Text = Convert.ToString(_dt.Rows(0)("first_name"))
            EmployeeLN.Text = Convert.ToString(_dt.Rows(0)("last_name"))
            EmployeeEmail.Text = Convert.ToString(_dt.Rows(0)("email_address"))
            EmployeeCell.Text = Convert.ToString(_dt.Rows(0)("cellphone"))
            EmployeeBankAccount.Text = Convert.ToString(_dt.Rows(0)("bank_account_number"))
            EmployeeBranchCode.Text = Convert.ToString(_dt.Rows(0)("bank_branch_code"))
        End If

    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)

    End Sub
    Private Sub BindEmployeesDropdown()
        Dim _dt As DataTable
        _dt = _Employee.GetEmployees()
        If _dt IsNot Nothing AndAlso _dt.Rows.Count > 0 Then

            For i As Integer = 0 To _dt.Rows.Count - 1
                cboEmployees.Items.Add(_dt(i)("first_name") & " " & _dt(i)("last_name") & " (" & _dt(i)("employee_number") & ")")
            Next
        End If
    End Sub

End Class