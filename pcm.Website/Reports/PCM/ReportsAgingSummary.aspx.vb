Imports pcm.BusinessLayer
Imports DevExpress.Web
Imports Entities

Public Class ReportsAgingSummary
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim url As String = Request.Url.AbsoluteUri

        If Not url.Contains("localhost") Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.AgingSummary) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

    End Sub

    Protected Sub cmdRun_Click(sender As Object, e As EventArgs)

        '=====================================================================================================
        'LOGGING
        '=====================================================================================================
        Dim _Logging As UsersBL = New UsersBL
        Dim _Log As New PCMUserLog
        _Log.AccountNumber = ""
        _Log.ActionType = "Run Report"
        _Log.IPAddress = Session("LoggingIPAddress")
        _Log.SearchCriteria = "Amount: " & txtAmount.Text
        _Log.UserComment = ""
        _Log.UserName = Session("username")
        _Log.WebPage = "Aging Summary Report"

        _Logging.WriteToLogPCM(_Log)
        '=====================================================================================================
        'dxGrid.DataBind()
        'dxGridCalls.DataBind()


    End Sub

    Protected Sub dxGrid_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        Dim _NewReport As New ReportsBusinessLayer
        'If dxGrid.DataSource = "" Then
        '    cmdExportPDF.Visible = False
        '    cmdExportExcel.Visible = False
        '    cmdExportCSV.Visible = False
        'End If
        dxGrid.DataSource = _NewReport.ReturnAgingSummary(txtAmount.Text)

    End Sub

    Protected Sub dxGrid_DataBinding_Calls(ByVal sender As Object, ByVal e As EventArgs)

        Dim _NewReport As New ReportsBusinessLayer

        dxGridCalls.DataSource = _NewReport.GetAccountsToBeCalled(txtAmount.Text)

    End Sub

    Protected Sub cmdExportPDF_Click(sender As Object, e As EventArgs) Handles cmdExportPDF.Click
        Exporter.WritePdfToResponse()
    End Sub

    Protected Sub cmdExportExcel_Click(sender As Object, e As EventArgs) Handles cmdExportExcel.Click
        Exporter.WriteXlsxToResponse()
    End Sub

    Protected Sub cmdExportCSV_Click(sender As Object, e As EventArgs) Handles cmdExportCSV.Click
        Exporter.WriteCsvToResponse()
    End Sub

    Private Sub ReportsAgingSummary_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub ASPxCallbackPanel1_Callback(sender As Object, e As CallbackEventArgsBase) Handles ASPxCallbackPanel1.Callback

    End Sub
End Class