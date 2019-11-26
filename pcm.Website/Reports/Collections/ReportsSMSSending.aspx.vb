﻿Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer

Public Class ReportsSMSSending
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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.SMSSending) Then
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

            cboType.Items.Add("All")
            cboType.Items.Add("60 days Overdue")
            cboType.Items.Add("90 days Overdue")
            cboType.Items.Add("120 days Overdue")
            cboType.Items.Add("150 days Overdue")
            cboType.Items.Add("Birthday SMS")
            cboType.Items.Add("Card Reminder")
            cboType.Items.Add("Cash Card Birthday")
            cboType.Items.Add("PTP")
            cboType.Items.Add("Reminder To Buy")
            cboType.Items.Add("Statement")

            cboType.Text = "All"

            Session.Remove("grid_data")

        End If

    End Sub

    Private Sub ReportsSMSSending_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Private Function GetData() As DataTable

        If Not IsNothing(Session("grid_data")) Then
            Return Session("grid_data")
        End If

        Dim _BLayer As New ReportsBusinessLayer(Session("current_company"))

        Dim data As DataTable = _BLayer.ReturnSMSReportData(cboType.Text, txtFromDate.Text)

        Session("grid_data") = data

        Return data

    End Function


    Protected Sub dxGrid_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
        dxGrid.BeginUpdate()

        Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        gridView.DataSource = GetData()

        dxGrid.EndUpdate()

    End Sub

    Protected Sub cmdRun_Click(sender As Object, e As EventArgs) Handles cmdRun.Click

        Session.Remove("grid_data")

        'dxGrid.Columns.Clear()

        dxGrid.DataBind()

    End Sub

    'Protected Sub cmdExportPDF_Click(sender As Object, e As EventArgs) Handles cmdExportPDF.Click
    '    Exporter.WritePdfToResponse()
    'End Sub

    'Protected Sub cmdExportExcel_Click(sender As Object, e As EventArgs) Handles cmdExportExcel.Click
    '    Exporter.WriteXlsxToResponse()
    'End Sub

    'Protected Sub cmdExportCSV_Click(sender As Object, e As EventArgs) Handles cmdExportCSV.Click
    '    Exporter.WriteCsvToResponse()
    'End Sub

End Class