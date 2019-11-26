Imports DevExpress.Web
Imports pcm.BusinessLayer
Imports Entities
Imports Newtonsoft.Json


Public Class EmployeeClocking
    Inherits System.Web.UI.Page
    Dim _blErrorLogging As New ErrorLogBL
    Dim _BLayer As New StockcodesHOBL
    Dim GetEmployees As New ReportsBusinessLayer
    Dim RG As New Entities.CommonFunctions.clsCommon



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not HttpContext.Current.IsDebuggingEnabled Then
            If Session("username") = "" Then
                If Not IsCallback Then
                    Response.Redirect("~/Intranet/Default.aspx")
                Else
                    ASPxWebControl.RedirectOnCallback("~/Intranet/Default.aspx")
                End If
            Else
                If Not CheckScreenAccess.CheckAccess(Session("reporting_permission_sequence"), Screens.Reporting.EmployeeClocking) Then
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
    Private Sub hud_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub

    Protected Sub ASPxCallback1_Callback(ByVal sender As Object, ByVal e As CallbackEventArgsBase)
        If hdWhichButton.Value = "Save" Then
            Save()
        End If

        If hdWhichButton.Value = "SearchEmployees" Then
            SearchEmployees()
        End If

        If hdWhichButton.Value = "SelectEmployee" Then
            SelectEmployee()
        End If
    End Sub
    Protected Sub SelectEmployee()
        Dim arrArray() As String
        arrArray = Split(lstSearch.Value, " - ")

        txtEmployee.Text = arrArray(0)

        txtSearch.Text = ""
        LookupMain.ShowOnPageLoad = False


    End Sub

    Protected Sub SearchEmployees()

        Dim clockingEmplyees As DataTable
        Try

            clockingEmplyees = GetEmployees.GetClockingEmployees(txtSearch.Text)
            If clockingEmplyees.Rows.Count > 0 Then
                lstSearch.Items.Clear()
                For Each drSCs As DataRow In clockingEmplyees.Rows
                    lstSearch.Items.Add(drSCs.Item("employee_number") & " - " & drSCs.Item("first_name") & " " & drSCs.Item("last_name"))
                Next

            Else
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "No Employees found"
                dxPopUpError.ShowOnPageLoad = True

            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try

    End Sub
    Public Sub Save()

        If txtEmail.Text = "" Then
            lblError.Text = "No email address was entered."
            dxPopUpError.ShowOnPageLoad = True
            Exit Sub
        End If

        Dim ReturnString As String = ""

        Dim employeereport As New Entities.EmployeeClocking.SaveEmployeeReport
        employeereport.EmployeeNumber = RG.Apos(txtEmployee.Text)
        employeereport.Totals = chkTotals.Checked
        employeereport.All = chkAll.Checked

        Dim json As String = JsonConvert.SerializeObject(employeereport)

        Try

            ReturnString = _BLayer.RunEmployeeReport(txtEmail.Text, txtFromDate.Text, txtToDate.Text, json)

            If ReturnString <> "Success" Then
                dxPopUpError.HeaderText = "Error"
                lblError.Text = "Something went wrong"
                dxPopUpError.ShowOnPageLoad = True

            Else
                ClearForm()
                dxPopUpError.HeaderText = "Success"
                lblError.Text = "Your report is running! It will be delivered to the specified email address(es) once it has completed."
                dxPopUpError.ShowOnPageLoad = True
            End If

        Catch ex As Exception
            _blErrorLogging.ErrorLogging(ex)
        End Try

    End Sub

    Public Sub ClearForm()
        txtEmail.Text = String.Empty
        txtEmployee.Text = String.Empty
        chkTotals.Checked = False
        chkAll.Checked = False
        txtFromDate.Text = Format(Now, "yyyy-MM-dd")
        txtToDate.Text = Format(Now, "yyyy-MM-dd")
    End Sub
End Class