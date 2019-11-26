Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities


Public Class graph_account_sales
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim getAccountSalesResponse As New GetAccountSalesResponse
    Dim _NewReport As New ReportsBusinessLayer
    Dim runningTotal As Long = 0
    Dim runningPayments As Long = 0
    Dim TotalStores As Long = 0
    Dim dt As DataTable



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
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.AccountSales) Then
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

            lblFromDate.Visible = False
            lblToDate.Visible = False
            txtFromDate.Visible = False
            txtToDate.Visible = False
            chartAccountSales.Visible = False
            Panel1.Visible = False


            cboDateRange.Items.Clear()
            cboDateRange.Items.Add("Today")
            cboDateRange.Items.Add("Yesterday")
            cboDateRange.Items.Add("This Month")
            cboDateRange.Items.Add("Custom Date Range")

            'Try
            '    GetAccountSales()
            'Catch ex As Exception
            '    _blErrorLogging.ErrorLogging(ex)
            'End Try

        End If
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)


        If hdWhichButton.Value = "DateRange" Then
                DateRangeIndexChanged()
            End If

        If hdWhichButton.Value = "cmdRun" Then
            GetAccountSales()
        End If

    End Sub

    Private Sub Page_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub GetAccountSales()

        Dim tmpYesterday As String
        tmpYesterday = Now.Date.AddDays(-1).ToString("yyyy-MM-dd")

        'tmpYesterday = Now.Year.ToString("yyyy") & "-" & Now.Month.ToString("MM") & -
        'tmpYesterday.ToString("yyyy-MM-dd")
        'tmpYesterday = Mid$(tmpYesterday, 7, 4) & "-" & Mid$(tmpYesterday, 4, 2) & "-" & Mid$(tmpYesterday, 1, 2)

        Dim ThisMonth As String = Format(Now, "yyyy-MM") & "-01"

        Dim StartDate As String = Format(Now, "yyyy-MM-dd") 'Default to today
        Dim EndDate As String = Format(Now, "yyyy-MM-dd") 'Default to today

        If cboDateRange.Text = "Yesterday" Then
            StartDate = tmpYesterday
            EndDate = tmpYesterday
        End If
        If cboDateRange.Text = "This Month" Then
            StartDate = ThisMonth
            EndDate = Format(Now, "yyyy-MM-dd")
        End If

        If cboDateRange.Text = "Custom Date Range" Then
            StartDate = txtFromDate.Text
            EndDate = txtToDate.Text
        End If

        Try
            getAccountSalesResponse = _NewReport.GetAccountSales(StartDate, EndDate)
            If getAccountSalesResponse.dt IsNot Nothing AndAlso getAccountSalesResponse.dt.Rows.Count > 0 Then
                TotalStores = getAccountSalesResponse.TotalStores
                runningTotal = getAccountSalesResponse.RunningTotal
                runningPayments = getAccountSalesResponse.RunningPayments

                chartAccountSales.Visible = True
                Panel1.Visible = True
                If TotalStores > 5 Then
                    chartAccountSales.Width = TotalStores * 30
                End If
                dt = getAccountSalesResponse.dt

                chartAccountSales.DataSource = dt
                chartAccountSales.DataBind()

                'Fix the grid
                chartAccountSales.ChartAreas("ChartArea1").AxisX.Interval = 1 'Set the vertical labels
                Dim gd As New DataVisualization.Charting.Grid
                gd.LineWidth = 0
                chartAccountSales.ChartAreas("ChartArea1").AxisX.MajorGrid = gd 'Get rid of the vertical lines
                chartAccountSales.Series("Series1").IsValueShownAsLabel = True 'Show the value as a label
                chartAccountSales.Series("Series2").IsValueShownAsLabel = True 'Show the value as a label

                'lblTotal.Text = "Total: " & runningTotal
                If chkPayments.Checked = False Then
                    chartAccountSales.Series.RemoveAt(1)
                    'lblPayments.Text = ""
                    chartAccountSales.Titles("Title3").Text = ""

                Else
                    'lblPayments.Text = "Payments: " & runningPayments
                    chartAccountSales.Titles("Title3").Text = "Payments: " & runningPayments

                End If

                chartAccountSales.Titles("Title1").Text = "Date Range: " & txtFromDate.Text & " to " & txtToDate.Text
                chartAccountSales.Titles("Title2").Text = "Total: " & runningTotal
            End If

            If cboDateRange.Text = "Custom Date Range" Then
                lblFromDate.Visible = True
                lblToDate.Visible = True
                txtFromDate.Visible = True
                txtToDate.Visible = True
            End If


            _NewReport.InsertUserRecord(Session("username"), Session("is_pcm_admin"), Session("ipaddress"), "Run Graph", "graph_segments", "")
        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try
    End Sub

    Public Sub DateRangeIndexChanged()
        If cboDateRange.Text = "Custom Date Range" Then
            lblFromDate.Visible = True
            lblToDate.Visible = True
            txtFromDate.Visible = True
            txtToDate.Visible = True
            txtFromDate.Text = Format(Now, "yyyy-MM-dd")
            txtToDate.Text = Format(Now, "yyyy-MM-dd")
            Panel1.Visible = False
            Exit Sub
        Else
            lblFromDate.Visible = False
            lblToDate.Visible = False
            txtFromDate.Visible = False
            txtToDate.Visible = False
            Panel1.Visible = False
        End If
    End Sub

End Class