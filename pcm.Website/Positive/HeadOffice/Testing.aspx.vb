Imports DevExpress.Web
Imports DevExpress.Web.ASPxTreeList
Imports Entities
Imports pcm.BusinessLayer
Imports DevExpress.XtraPrinting
Imports DevExpress.Export

Public Class Testing
    Inherits System.Web.UI.Page
    Dim employee As EmployeeBusinessLayer = New EmployeeBusinessLayer
    Dim tablerow As New DataTable()
    Dim dt As DataTable = employee.getdata_testing()
    Dim employee_number As Integer

    Private Sub hud_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        DevExpress.Web.ASPxWebControl.GlobalTheme = "Office2010Blue"
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            DateofBirth.MinDate = "01-01-2000"
            DateofBirth.MaxDate = Date.Now()
            DateofBirth.Value = Date.Now()
        End If
        ASPxGridView1.DataSource = dt
        ASPxGridView1.DataBind()
    End Sub
    Protected Sub ASPxCallback1_Callback(sender As Object, e As CallbackEventArgsBase) Handles ASPxCallbackPanel1.Callback
        If hdWhichButton.Value = "addRow" Then
            addRow()
        End If

    End Sub
    Public Sub clear()
        EmployeeNumber.Value = ""
        FirstName.Text = ""
        LastName.Text = ""
        Active.Value = False
        EmailAddress.Text = ""
        Male.Value = False
        Female.Value = False

    End Sub
    Public Sub addRow()

        Dim Gender As String = Nothing
        If (Male.Checked) Then
            Gender = "Male"
        Else
            Gender = "Female"
        End If
        Dim test As New Testing2
        Dim baseresponse As New BaseResponse
        test.Employee_Number = EmployeeNumber.Value()
        test.First_Name = FirstName.Text()
        test.Last_Name = LastName.Text()
        test.Date_of_Birth = DateofBirth.Value()
        test.Active = Active.Value()
        test.Email_Address = EmailAddress.Text()
        test.Gender = Gender
        baseresponse = employee.Save(test)
        dxPopUpError.ShowOnPageLoad = False
        clear()

    End Sub

    Public Sub deleterow()
        Dim baseresponse As New BaseResponse

        employee_number = ASPxGridView1.EditingRowVisibleIndex
        baseresponse = employee.delete_testing(employee_number)
    End Sub

    Protected Sub ASPxGridView1_CustomButtonCallback(sender As Object, e As ASPxGridViewCustomButtonCallbackEventArgs) Handles ASPxGridView1.CustomButtonCallback
        Try
            If (e.ButtonID = "delete") Then
                Dim baseresponse As New BaseResponse
                Dim index As Integer
                index = e.VisibleIndex
                employee_number = ASPxGridView1.GetRowValues(index, "employee_number")
                'dxPopUpError.HeaderText = "Confirm"
                'lblError.Text = "Do you really want to delete?"
                'dxPopUpError.ShowOnPageLoad = True
                baseresponse = employee.delete_testing(employee_number)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub ASPxGridView1_RowUpdating(sender As Object, e As Data.ASPxDataUpdatingEventArgs) Handles ASPxGridView1.RowUpdating
        Dim test As New Testing2
        Dim baseresponse As New BaseResponse
        test.Employee_Number = e.NewValues("employee_number")
        test.First_Name = e.NewValues("first_name")
        test.Last_Name = e.NewValues("last_name")
        test.Active = e.NewValues("active")
        test.Date_of_Birth = Format(e.NewValues("date_of_birth"), "yyyy-MM-dd")
        test.Email_Address = e.NewValues("email_address")
        test.Gender = e.NewValues("gender")
        baseresponse = employee.update_testing(test)
        'dxPopUpError.HeaderText = "Confirm"
        'lblError.Text = "Do you really want to update?"
        'dxPopUpError.ShowOnPageLoad = True
    End Sub

    Protected Sub ASPxUploadControl1_FileUploadComplete(sender As Object, e As FileUploadCompleteEventArgs) Handles ASPxUploadControl1.FileUploadComplete
        Try
            Dim uploadFolder As String = Server.MapPath("~/Uploaded/")
            Dim filename As String = e.UploadedFile.FileName
            Dim exists As String = uploadFolder + filename
            If System.IO.File.Exists(exists) Then
                e.IsValid = False
            Else
                e.UploadedFile.SaveAs(uploadFolder + filename)
                e.IsValid = True
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

End Class
