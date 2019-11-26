Imports DevExpress.Web
Imports DevExpress.Web.ASPxTreeList
Imports Entities
Imports DevExpress.XtraPrinting
Imports DevExpress.Export
Imports pcm.BusinessLayer

Public Class WebForm1
    Inherits System.Web.UI.Page
    Dim _testing As New Testing1_BusinessLayer
    Dim srch As New Search
    Dim _test As New Testing3
    Dim stDate As String
    Dim edDate As String
    Dim actv As Boolean
    Dim prc As String
    Dim qty As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub ASPxCallbackPanel1_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles ASPxCallbackPanel1.Callback

        If hdWhichButton.Value = "specialname" Then
            getSpecialName()
        End If
        If hdWhichButton.Value = "AddData" Then
            AddData()
        End If
        If hdWhichButton.Value = "Rowvalue" Then
            specialName()
        End If
        If hdWhichButton.Value = "clear" Then
            clear()
        End If

    End Sub
    ''' <summary>
    ''' This Function is used to Call the Databinding function of AspxGridview2
    ''' </summary>
    ''' Created Date=16-07-2019
    ''' Created By=Kamal
    ''' Modified Date=
    ''' Modified By=
    Public Sub getSpecialName()
        ASPxGridView2.DataBind()
    End Sub
    ''' <summary>
    ''' This Function is used to Call the Databinding function of AspxGridview2
    ''' </summary>
    ''' Created Date=16-07-2019
    ''' Created By=Kamal
    ''' Modified Date=
    ''' Modified By=
    Public Sub AddData()
        ASPxGridView1.DataBind()
    End Sub
    ''' <summary>
    ''' This Function is used to Bind the result of search  with Aspxgridview2
    ''' </summary>
    ''' Created Date=16-07-2019
    ''' Created By=Kamal
    ''' Modified Date=
    ''' Modified By=
    Protected Sub ASPxGridView2_DataBinding(sender As Object, e As EventArgs) Handles ASPxGridView2.DataBinding
        Dim GetSpecialName As DataTable
        srch.Search_Type = cboSpecial.Value()
        srch.Search_Detail = ASPxTextBox2.Text()
        srch.Limit = ASPxTextBox3.Value()
        GetSpecialName = _testing.Search(srch)
        If cboSpecial.Value = "sp" Then
            ASPxGridView2.KeyFieldName = "special_name"
        End If
        If cboSpecial.Value = "mc" Then
            ASPxGridView2.KeyFieldName = "master_code"
        End If
        If cboSpecial.Value = "gc" Then
            ASPxGridView2.KeyFieldName = "generated_code"
        End If
        ASPxGridView2.DataSource = GetSpecialName

    End Sub
    ''' <summary>
    ''' This Function is used to Bind the Mastercode and Generetedcode with Aspxgridview1
    ''' </summary>
    ''' Created Date=16-07-2019
    ''' Created By=Kamal
    ''' Modified Date=
    ''' Modified By=
    Protected Sub ASPxGridView1_DataBinding(sender As Object, e As EventArgs) Handles ASPxGridView1.DataBinding
        Dim Data As DataTable
        _test.Special_Name = ASPxTextBox1.Text()
        Data = _testing.AddData(_test)
        stDate = Data.Rows(0).Item("start_date")
        edDate = Data.Rows(0).Item("end_date")
        actv = Data.Rows(0).Item("is_active")
        prc = Data.Rows(0).Item("price")
        qty = Data.Rows(0).Item("qty")
        ASPxGridView1.DataSource = Data
    End Sub
    'Protected Sub ASPxGridView2_FocusedRowChanged(sender As Object, e As EventArgs) Handles ASPxGridView2.FocusedRowChanged
    '    Dim rowvalue As String = ASPxGridView2.GetRowValues(ASPxGridView2.FocusedRowIndex, "special_name")
    '    ASPxTextBox1.Text() = rowvalue
    '    ASPxPopupControl1.ShowOnPageLoad = False
    'End Sub
    Private Sub specialName()
        Dim name = New List(Of Object)
        If cboSpecial.Value = "sp" Then
            name = ASPxGridView2.GetSelectedFieldValues("special_name")
            ASPxTextBox1.Text = Convert.ToString(name(0))
            ASPxGridView1.DataBind()
            ASPxDateEdit1.Value = stDate
            ASPxDateEdit2.Value = edDate
            ASPxCheckBox1.Value = actv
            ASPxTextBox4.Text = prc
            ASPxTextBox6.Text = qty
        End If
        If cboSpecial.Value = "mc" Then
            name = ASPxGridView2.GetSelectedFieldValues("master_code")
            ASPxTextBox5.Text = Convert.ToString(name(0))

        End If
        If cboSpecial.Value = "gc" Then
            name = ASPxGridView2.GetSelectedFieldValues("generated_code")
            ASPxTextBox5.Text = Convert.ToString(name(0))

        End If
        ASPxGridView2.Columns.Clear()
    End Sub

    Private Sub clear()
        ASPxTextBox1.Text = ""
        ASPxDateEdit1.Value = ""
        ASPxDateEdit2.Value = ""
        ASPxCheckBox1.Value = False
        ASPxTextBox4.Text = ""
        ASPxTextBox5.Text = ""
        ASPxTextBox6.Text = ""
        ASPxGridView1.Columns.Clear()
    End Sub
End Class