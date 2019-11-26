﻿Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer

Public Class WebServiceLogs
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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.WebServiceLogs) Then
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

    Protected Sub dxGrid_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        dxGrid.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        Dim _BLayer As New ReportsPositiveWebServiceLogsBL()

        Dim data As DataTable = _BLayer.GetWebServiceLogs(txtFromDate.Text, txtToDate.Text)

        'gridView.KeyFieldName = "account_number" 'data.PrimaryKey(0).ColumnName
        gridView.DataSource = data

        dxGrid.EndUpdate()

    End Sub

    Private Sub ReportsUsers_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub cmdRun_Click(sender As Object, e As EventArgs) Handles cmdRun.Click
        dxGrid.DataBind()

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


End Class