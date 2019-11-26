Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer
Imports DevExpress.XtraPrinting
Imports DevExpress.XtraPrintingLinks
Imports System.Web.UI
Imports System.IO

Public Class BadDebt
    Inherits System.Web.UI.Page

    Private Sub hud_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.BadDebt) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            Dim _NewReport As New ReportsBusinessLayer

            cboPeriod.DataSource = _NewReport.GetInternalPeriods()
            cboPeriod.DataBind()

        End If

    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)


        If hdWhichButton.Value = "Ok" Then

            '=====================================================================================================
            'LOGGING
            '=====================================================================================================
            Dim _Logging As UsersBL = New UsersBL
            Dim _Log As New PCMUserLog
            _Log.AccountNumber = ""
            _Log.ActionType = "Run Report"
            _Log.IPAddress = Session("LoggingIPAddress")
            _Log.SearchCriteria = cboPeriod.Text
            _Log.UserComment = ""
            _Log.UserName = Session("username")
            _Log.WebPage = "Bad Debt Report"

            _Logging.WriteToLogPCM(_Log)
            '=====================================================================================================

            Dim SelectedPeriod As String() = cboPeriod.Text.Split(" - ")

            Dim _NewReport As New ReportsBusinessLayer

            Session("_BadDebt") = _NewReport.GetBadDebtData(SelectedPeriod(0))
            If Session("_BadDebt") IsNot Nothing Then
                grdBadDebt.DataBind()
            Else
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "An error occured fetching the data for the Bad Debt"
                dxPopUpError.ShowOnPageLoad = True
            End If

            Session("_BadDebtRecovered") = _NewReport.GetBadDebtRecoveredData(SelectedPeriod(0))
            If Session("_BadDebtRecovered") IsNot Nothing Then
                grdBadDebtRecovered.DataBind()
            Else
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "An error occured fetching the data for the Bad Debt Recovered"
                dxPopUpError.ShowOnPageLoad = True
            End If

            Session("_Reduction150") = _NewReport.GetReductionIn150(SelectedPeriod(0))
            If Session("_Reduction150") IsNot Nothing Then
                grdReduction.DataBind()
            Else
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "An error occured fetching the data for the Reduction in 150 Days"
                dxPopUpError.ShowOnPageLoad = True
            End If
        End If



    End Sub

    Protected Sub grdBadDebt_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = Session("_BadDebt")
        ''gridView.KeyFieldName = "completed_survey_id" 'data.PrimaryKey(0).ColumnName
        gridView.DataSource = data


    End Sub
    Protected Sub grdBadDebtRecovered_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = Session("_BadDebtRecovered")
        ''gridView.KeyFieldName = "completed_survey_id" 'data.PrimaryKey(0).ColumnName
        gridView.DataSource = data


    End Sub

    Protected Sub grdReduction_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = Session("_Reduction150")
        ''gridView.KeyFieldName = "completed_survey_id" 'data.PrimaryKey(0).ColumnName
        gridView.DataSource = data


    End Sub

    Private Sub BadDebt_Init(sender As Object, e As EventArgs) Handles Me.Init
        Page.Server.ScriptTimeout = 300
    End Sub

    Private Sub WriteToResponse(ByVal fileName As String, ByVal saveAsFile As Boolean, ByVal fileFormat As String, ByVal stream As MemoryStream)
        If Page Is Nothing OrElse Page.Response Is Nothing Then
            Return
        End If
        Dim disposition As String = If(saveAsFile, "attachment", "inline")
        Page.Response.Clear()
        Page.Response.Buffer = False
        Page.Response.AppendHeader("Content-Type", String.Format("application/{0}", fileFormat))
        Page.Response.AppendHeader("Content-Transfer-Encoding", "binary")
        Page.Response.AppendHeader("Content-Disposition", String.Format("{0}; filename={1}.{2}", disposition, fileName, fileFormat))
        Page.Response.BinaryWrite(stream.GetBuffer())
        Page.Response.End()
    End Sub

    Protected Sub cmdExportCSVBadDebt_Click(sender As Object, e As EventArgs) Handles cmdExportCSVBadDebt.Click
        Dim ps As New PrintingSystem()

        Dim link As New PrintableComponentLink(ps)
        link.Component = GridExporter1

        Dim compositeLink As New CompositeLink(ps)
        compositeLink.Links.AddRange(New Object() {link})
        compositeLink.CreateDocument()
        Using stream As New MemoryStream()
            compositeLink.PrintingSystem.ExportToCsv(stream)
            WriteToResponse("bad_debt.csv", True, "csv", stream)
        End Using


        ps.Dispose()

    End Sub

    Protected Sub cmdExportCSVBadDebtRecovered_Click(sender As Object, e As EventArgs) Handles cmdExportCSVBadDebtRecovered.Click
        Dim ps As New PrintingSystem()

        Dim link As New PrintableComponentLink(ps)
        link.Component = GridExporter2

        Dim compositeLink As New CompositeLink(ps)
        compositeLink.Links.AddRange(New Object() {link})
        compositeLink.CreateDocument()
        Using stream As New MemoryStream()
            compositeLink.PrintingSystem.ExportToCsv(stream)
            WriteToResponse("bad_debt_recovered.csv", True, "csv", stream)
        End Using


        ps.Dispose()

    End Sub

    Protected Sub cmdExportCSVReductionIn150_Click(sender As Object, e As EventArgs) Handles cmdExportCSVReductionIn150.Click
        Dim ps As New PrintingSystem()

        Dim link As New PrintableComponentLink(ps)
        link.Component = GridExporter3

        Dim compositeLink As New CompositeLink(ps)
        compositeLink.Links.AddRange(New Object() {link})
        compositeLink.CreateDocument()
        Using stream As New MemoryStream()
            compositeLink.PrintingSystem.ExportToCsv(stream)
            WriteToResponse("reduction150.csv", True, "csv", stream)
        End Using


        ps.Dispose()

    End Sub
End Class