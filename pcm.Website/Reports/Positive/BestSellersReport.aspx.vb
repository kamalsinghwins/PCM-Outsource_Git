Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer
Imports DevExpress.XtraPrinting
Imports DevExpress.Export

Public Class BestSellersReport
    Inherits System.Web.UI.Page
    Dim _NewReport As New ReportsBusinessLayer
    Dim _blErrorLogging As New ErrorLogBL


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim url As String = Request.Url.AbsoluteUri

        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Maintenance.SendMarketingSMS) Then
                    If Not IsCallback Then
                        Response.Redirect("~/Intranet/Welcome.aspx")
                    Else
                        ASPxWebControl.RedirectOnCallback("~/Intranet/Welcome.aspx")
                    End If
                End If
            End If
        End If

        If Not IsPostBack Then
            txtFromDate.Text = Format(Now, "yyyy-MM-dd")
            txtToDate.Text = Format(Now, "yyyy-MM-dd")

        End If
    End Sub
    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        If hdWhichButton.Value = "cmdRun" Then
            gvMaster.DataBind()
        End If


    End Sub
    Private Function GetMasterData() As DataTable
        Dim data As DataSet = GetData()
        If data IsNot Nothing Then
            Return data.Tables("Master")
        Else
            Return Nothing
        End If
    End Function

    Private Function GetDetailData(ByVal masterRowKey As Object) As DataView
        Dim data As DataSet = GetData()
        If data IsNot Nothing Then
            Dim detail As DataTable = data.Tables("Detail")
            If data.Relations("MasterDetail") IsNot Nothing Then
                Dim columnName As String = data.Relations("MasterDetail").ParentColumns(0).ColumnName
                Return New DataView(detail, String.Format("[{0}] = '{1}'", columnName, masterRowKey), String.Empty, DataViewRowState.CurrentRows)
            Else
                Return Nothing
            End If

        Else
                Return Nothing
            End If
    End Function
    Private Function GetData() As DataSet
        Dim trim As String = String.Empty
        Dim data As DataSet
        Dim StartDate As String
        Dim EndDate As String

        StartDate = txtFromDate.Text
        EndDate = txtToDate.Text

        If chkMastercode.Checked = True Then
            trim = trim & "TRIM(both ' '  from UPPER(l.master_code)) as stockcode, "
        Else
            trim = trim & "TRIM(both ' '  from UPPER(l.generated_code)) as stockcode, "
        End If

        Try

            data = _NewReport.GetBestSellersReport(StartDate, EndDate, trim)


            Session("data") = data


            _NewReport.InsertUserRecord(Session("username"), Session("is_pcm_admin"), Session("ipaddress"), "Run Graph", "grd_best_sellers", "")

        Catch ex As Exception

            _blErrorLogging.ErrorLogging(ex)

        End Try

        Return data

    End Function
    Private Sub BestSellersReport_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub gvMaster_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        gvMaster.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)
        Dim data As DataTable = GetMasterData()
        gridView.KeyFieldName = "stockcode"
        gridView.DataSource = data
        gvMaster.EndUpdate()

    End Sub

    Protected Sub gvDetail_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        Dim dataView As DataView = GetDetailData(gridView.GetMasterRowKeyValue())
        gridView.KeyFieldName = "guid"
        gridView.DataSource = dataView
    End Sub
    Protected Sub cmdExportPDF_Click(sender As Object, e As EventArgs) Handles cmdExportPDF.Click
        Exporter.WritePdfToResponse()
    End Sub

    Protected Sub cmdExportExcel_Click(sender As Object, e As EventArgs) Handles cmdExportExcel.Click
        Exporter.WriteXlsxToResponse(New XlsxExportOptionsEx() With {.ExportType = ExportType.WYSIWYG})

    End Sub

    Protected Sub cmdExportCSV_Click(sender As Object, e As EventArgs) Handles cmdExportCSV.Click
        Exporter.WriteCsvToResponse(New CsvExportOptionsEx() With {.ExportType = ExportType.WYSIWYG})
    End Sub

End Class