﻿Imports DevExpress.Web
Imports Entities
Imports pcm.BusinessLayer

Public Class ReportsPaymentsAndContacts
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Increase page timeout to 10 minutes
        Page.Server.ScriptTimeout = 600

        Dim url As String = Request.Url.AbsoluteUri

        If Not url.Contains("localhost") Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.PaymentsVSContacts) Then
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

            Session.Remove("grid_data")

        End If

    End Sub

    Private Sub ReportsPaymentsAndContacts_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Private Function GetData() As DataTable

        If Not IsNothing(Session("grid_data")) Then
            Return Session("grid_data")
        End If

        Dim _BLayer As New ReportsBusinessLayer(Session("current_company"))

        Dim data As DataTable = _BLayer.ReturnPaymentsVSContactsReportData(txtFromDate.Text, txtToDate.Text)

        Session("grid_data") = data

        Return data

    End Function


    Protected Sub dxGrid_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        dxGrid.DataSource = GetData()

        'dxGrid.BeginUpdate()

        'Dim gridView As ASPxGridView = TryCast(sender, ASPxGridView)

        'gridView.DataSource = GetData()

        'dxGrid.EndUpdate()

    End Sub

    Protected Sub cmdRun_Click(sender As Object, e As EventArgs) Handles cmdRun.Click

        Session.Remove("grid_data")

        'dxGrid.Columns.Clear()

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