Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Public Class VintageReport
    Inherits System.Web.UI.Page
    Dim _AgeAnalysis As PCMReportingBusinessLayer = New PCMReportingBusinessLayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            InitialiseValues()
        End If

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.VintageReport) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        Else
            Session("username") = "DANIEL"
        End If

    End Sub
    Private Sub hud_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Public Sub InitialiseValues()
        cboYear.Items.Add("2010")
        cboYear.Items.Add("2011")
        cboYear.Items.Add("2012")
        cboYear.Items.Add("2013")
        cboYear.Items.Add("2014")
        cboYear.Items.Add("2015")
        cboYear.Items.Add("2016")
        cboYear.Items.Add("2017")
        cboYear.Items.Add("2018")
        cboYear.Items.Add("2019")
        cboYear.Items.Add("2020")

        cboMonth.Items.Add("January")
        cboMonth.Items.Add("February")
        cboMonth.Items.Add("March")
        cboMonth.Items.Add("April")
        cboMonth.Items.Add("May")
        cboMonth.Items.Add("June")
        cboMonth.Items.Add("July")
        cboMonth.Items.Add("August")
        cboMonth.Items.Add("September")
        cboMonth.Items.Add("October")
        cboMonth.Items.Add("November")
        cboMonth.Items.Add("December")

        cboStatus.Items.Clear()
        cboStatus.Items.Add("A,L,D,DR,C,F,WO,B")
        cboStatus.Items.Add("ALL")
        cboStatus.Items.Add("ACTIVE")
        cboStatus.Items.Add("BLOCKED")
        cboStatus.Items.Add("DEBT REVIEW")
        cboStatus.Items.Add("DECEASED")
        cboStatus.Items.Add("DECLINED")
        cboStatus.Items.Add("FRAUD")
        cboStatus.Items.Add("LEGAL")
        cboStatus.Items.Add("PENDING")
        cboStatus.Items.Add("SUSPENDED")
        cboStatus.Items.Add("WRITE-OFF")

    End Sub

    Private Sub chkThick_Click()
        If chkthick.Value = 1 Then
            txtScore.Text = ""
            txtScore.Enabled = False
        Else
            txtScore.Enabled = True
        End If
    End Sub

    Protected Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Logging As UsersBL = New UsersBL
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Run Report"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = "Month: " & cboMonth.Text & " Year: " & cboYear.Text
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Vintage Report"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================

        Dim reportRequest As New ReportRequest
        Dim reportResponse As New ReportResponse
        reportRequest.CboMonth = cboMonth.Text
        reportRequest.CboYear = cboYear.Text
        reportRequest.CboStatus = cboStatus.Text
        reportRequest.FileName = txtFile.Text
        reportRequest.Score = txtScore.Text
        reportRequest.CheckThickFilesOnly = chkthick.Checked
        reportRequest.CheckMaleOnly = chkMale.Checked
        reportRequest.CheckIncludeAllPeriods = chkAllPeriods.Checked
        reportRequest.CheckZeroes = chkZeroes.Checked

        reportResponse = _AgeAnalysis.GetReport(reportRequest)
        If reportResponse.Success = True Then
            Response.Clear()
            Response.Buffer = True
            Response.AddHeader("content-disposition", "attachment;filename=" & reportRequest.FileName & ".csv")
            Response.Charset = ""
            Response.ContentType = "application/text"
            Response.Output.Write(reportResponse.CSV)
            Response.Flush()
            Response.End()
        Else
            lblError.Text = reportResponse.Message
            dxPopUpError.ShowOnPageLoad = True
        End If

    End Sub
End Class