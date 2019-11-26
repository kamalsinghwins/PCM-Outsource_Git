Imports Npgsql
Imports Microsoft.VisualBasic
Imports System.Data
Imports Entities
Imports System.IO

Public Class ShopDownloadsDL
    Inherits DataAccessLayerBase

    Dim ds2 As DataSet

    Dim objDBWrite As dlNpgSQL
    Dim objDBRead As dlNpgSQL

    Dim connection As Npgsql.NpgsqlConnection = Nothing
    Dim connectionPositive As Npgsql.NpgsqlConnection = Nothing

    Dim _database As String

    'Public Sub New(ByVal CompanyCode As String)

    '    'If Not HttpContext.Current.IsDebuggingEnabled Then
    '    'Not debugging
    '    objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite")
    '    _database = CompanyCode
    '    objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead")
    '    connection = Me.DataBase("PostgreConnectionStringPositiveRead")
    '    'Else
    '    '    '_database = "pos_demo"
    '    '    objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWriteTesting", CompanyCode)
    '    '    objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveReadTesting", CompanyCode)
    '    '    connection = Me.DataBase("PostgreConnectionStringPositiveReadTesting", CompanyCode)
    '    'End If

    'End Sub

    Public Sub New()
        _database = ConfigurationManager.AppSettings("CurrentDatabase")
        objDBWrite = New dlNpgSQL("PostgreConnectionStringPositiveWrite")
        objDBRead = New dlNpgSQL("PostgreConnectionStringPositiveRead")
        connection = Me.DataBase("PostgreConnectionStringPCMRead")
        connectionPositive = Me.DataBase("PostgreConnectionStringPositiveRead")

    End Sub

    Public Function GetTillNumbers(ByVal LastInsertDate As String,
                                   ByVal BranchCode As String, ByVal ServerPath As String,
                                   ByVal sIPAddress As String) As List(Of TillNumbers)

        tmpSQL = "SELECT * FROM till_numbers WHERE inserted >= '" & LastInsertDate & "'"

        Dim _TillNumbers As New List(Of TillNumbers)

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                DoWebServiceLog(BranchCode, "GetTillNumbers", LastInsertDate, "", ds.Tables(0).Rows.Count)
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _till As New TillNumbers
                    _till.branch_code = dr("branch_code") & ""
                    _till.till_number = dr("till_number") & ""

                    _TillNumbers.Add(_till)
                Next
            End If

        Catch ex As Exception
            Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",GetTillNumbers," & ex.Message & "," & tmpSQL & ",database," & _database & ",branch_code," & BranchCode)
            End Using

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '         "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & BranchCode & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If

            Return Nothing
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return _TillNumbers


    End Function

    Public Function GetBranches(ByVal LastInsertDate As String,
                                ByVal LastUpdateDate As String,
                                ByVal BranchCode As String, ByVal ServerPath As String,
                                ByVal sIPAddress As String) As List(Of Branch)

        tmpSQL = "SELECT * FROM branch_details " &
                 "WHERE (inserted >= '" & LastInsertDate & "' OR updated >= '" & LastUpdateDate & "') " &
                 "AND is_blocked = False "

        Dim _Branches As New List(Of Branch)

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                DoWebServiceLog(BranchCode, "GetBranches", LastInsertDate, LastUpdateDate, ds.Tables(0).Rows.Count)
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _branch As New Branch
                    _branch.address_line_1 = dr("address_line_1") & ""
                    _branch.address_line_2 = dr("address_line_2") & ""
                    _branch.address_line_3 = dr("address_line_3") & ""
                    _branch.address_line_4 = dr("address_line_4") & ""
                    _branch.address_line_5 = dr("address_line_5") & ""
                    _branch.branch_code = dr("branch_code") & ""
                    _branch.branch_name = dr("branch_name") & ""
                    _branch.branch_type = dr("branch_type") & ""
                    _branch.email_address = dr("email_address") & ""
                    _branch.fax_number = dr("fax_number") & ""
                    _branch.is_blocked = dr("is_blocked") & ""
                    _branch.merchant_number = dr("merchant_number") & ""
                    _branch.is_head_office = dr("is_head_office") & ""
                    _branch.pricelevel = dr("pricelevel") & ""
                    _branch.tax_number = dr("tax_number") & ""
                    _branch.telephone_number = dr("telephone_number") & ""
                    _branch.updated = dr("updated") & ""

                    _Branches.Add(_branch)
                Next
            End If

        Catch ex As Exception
            Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",GetBranches," & ex.Message & "," & tmpSQL & ",database," & _database & ",branch_code," & BranchCode)
            End Using

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '         "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & BranchCode & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If

            Return Nothing
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return _Branches

    End Function

    Private Sub DoWebServiceLog(ByVal BranchCode As String, ByVal WebService As String,
                                ByVal FromDate As String, ByVal ToDate As String, ByVal ReturnRecords As String)

        tmpSQL = "INSERT INTO web_service_logs (branch_code,web_service,from_date,to_date,records_returned) VALUES " &
                 "('" & RG.Apos(BranchCode) & "','" & WebService & "','" & FromDate & "','" & ToDate & "','" & ReturnRecords & "')"
        objDBWrite.ExecuteQuery(tmpSQL)

        If (objDBWrite IsNot Nothing) Then
            objDBWrite.CloseConnection()
        End If

    End Sub

    Public Function GetColourGrids(ByVal LastInsertDate As String, ByVal LastUpdateDate As String,
                                   ByVal BranchCode As String, ByVal ServerPath As String,
                                   ByVal sIPAddress As String) As List(Of ColourGrid)

        tmpSQL = "SELECT * FROM colour_grids WHERE (inserted >= '" & LastInsertDate & "' OR updated >= '" & LastUpdateDate & "')"

        Dim _ColourGrids As New List(Of ColourGrid)

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                DoWebServiceLog(BranchCode, "GetColourGrids", LastUpdateDate, "", ds.Tables(0).Rows.Count)
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _ColourGrid As New ColourGrid
                    _ColourGrid.colour_code = dr("colour_code") & ""
                    _ColourGrid.colour_description = dr("colour_description") & ""

                    _ColourGrids.Add(_ColourGrid)
                Next
            End If

        Catch ex As Exception
            Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",GetColourGrids," & ex.Message & "," & tmpSQL & ",database," & _database & ",branch_code," & BranchCode)
            End Using

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '         "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & BranchCode & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If

            Return Nothing
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return _ColourGrids

    End Function

    Public Function GetTaxGroups(ByVal BranchCode As String, ByVal ServerPath As String,
                                 ByVal LastUpdateDate As String,
                                 ByVal sIPAddress As String) As List(Of TaxGroup)

        tmpSQL = "SELECT * FROM tax_groups WHERE updated >= '" & LastUpdateDate & "'"

        Dim _TaxGroups As New List(Of TaxGroup)

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                DoWebServiceLog(BranchCode, "GetTaxGroups", LastUpdateDate, "", ds.Tables(0).Rows.Count)
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _TaxGroup As New TaxGroup
                    _TaxGroup.tax_group = dr("tax_group") & ""
                    _TaxGroup.tax_value = dr("tax_value") & ""
                    _TaxGroup.tax_description = dr("tax_description") & ""

                    _TaxGroups.Add(_TaxGroup)
                Next
            End If

        Catch ex As Exception
            Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",GetTaxGroups," & ex.Message & "," & tmpSQL & ",database," & _database & ",branch_code," & BranchCode)
            End Using

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '         "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & BranchCode & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If

            Return Nothing
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return _TaxGroups

    End Function

    Public Function GetNewSpecials(ByVal BranchCode As String, ByVal ServerPath As String,
                                   ByVal LastUpdateDate As String,
                                   ByVal sIPAddress As String) As List(Of Special)

        tmpSQL = "SELECT m.special_id,m.start_date,m.end_date,m.is_active,m.price,m.special_name " &
                 "FROM specials_master m " &
                 "WHERE update_date >= '" & LastUpdateDate & "'"

        Dim _Specials As New List(Of Special)

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                DoWebServiceLog(BranchCode, "GetNewSpecials", LastUpdateDate, "", ds.Tables(0).Rows.Count)
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _Special As New Special
                    _Special.SpecialID = dr("special_id")
                    _Special.SpecialName = dr("special_name")
                    _Special.StartDate = dr("start_date")
                    _Special.EndDate = dr("end_date")
                    _Special.is_Active = dr("is_active")
                    _Special.Price = dr("price")

                    Dim _SpecialLines As New List(Of SpecialLineItems)

                    tmpSQL = "SELECT master_code,qty FROM specials_line_items WHERE specials_link_id = '" & dr("special_id") & "'"
                    ds2 = objDBRead.GetDataSet(tmpSQL)
                    If objDBRead.isR(ds2) Then
                        For Each dr2 As DataRow In ds2.Tables(0).Rows
                            Dim _SpecialLine As New SpecialLineItems
                            _SpecialLine.Mastercode = dr2("master_code") & ""
                            _SpecialLine.Qty = dr2("qty") & ""
                            _SpecialLines.Add(_SpecialLine)
                        Next
                    End If

                    'If _SpecialLines.Count > 0 Then
                    _Special.ListOfItems = _SpecialLines
                    'End If

                    _Specials.Add(_Special)

                Next
            End If

        Catch ex As Exception
            Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",GetNewSpecials," & ex.Message & "," & tmpSQL & ",database," & _database & ",branch_code," & BranchCode)
            End Using

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '         "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & BranchCode & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If

            Return Nothing
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return _Specials

    End Function

    Public Function GetUsers(ByVal LastInsertDate As String, ByVal LastUpdateDate As String,
                             ByVal BranchCode As String, ByVal ServerPath As String,
                             ByVal sIPAddress As String) As List(Of UserPermissions)

        tmpSQL = "SELECT * FROM user_permissions WHERE (inserted >= '" & LastInsertDate & "' OR updated >= '" & LastUpdateDate & "')"

        Dim _UserPermissions As New List(Of UserPermissions)

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                DoWebServiceLog(BranchCode, "GetUsers", LastUpdateDate, "", ds.Tables(0).Rows.Count)
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _UserPermission As New UserPermissions
                    _UserPermission.user_name = dr("user_name") & ""
                    _UserPermission.user_password = dr("user_password") & ""
                    _UserPermission.branch_code = dr("branch_code") & ""
                    _UserPermission.is_head_office_user = dr("is_head_office_user") & ""
                    _UserPermission.is_locked_to_branch = dr("is_locked_to_branch") & ""
                    _UserPermission.pos_sequence = dr("pos_sequence") & ""
                    _UserPermission.maintenance_sequence = dr("maintenance_sequence") & ""
                    _UserPermission.transaction_sequence = dr("transaction_sequence") & ""

                    _UserPermissions.Add(_UserPermission)
                Next
            End If

        Catch ex As Exception
            Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",GetUsers," & ex.Message & "," & tmpSQL & ",database," & _database & ",branch_code," & BranchCode)
            End Using

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '         "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & BranchCode & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If

            Return Nothing
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return _UserPermissions

    End Function

    Public Function GetStockOnHand(ByVal LastUpdateDate As String,
                                   ByVal BranchCode As String, ByVal ServerPath As String,
                                   ByVal sIPAddress As String) As List(Of StockOnHand)

        tmpSQL = "SELECT * FROM stock_on_hand WHERE branch_code = '" & RG.Apos(BranchCode) & "' AND updated >= '" & LastUpdateDate & "'"

        Dim _StockOnHands As New List(Of StockOnHand)

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                DoWebServiceLog(BranchCode, "GetStockOnHand", LastUpdateDate, "", ds.Tables(0).Rows.Count)
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _StockOnHand As New StockOnHand
                    _StockOnHand.generated_code = dr("generated_code") & ""
                    _StockOnHand.qty_on_hand = dr("qty_on_hand") & ""

                    _StockOnHands.Add(_StockOnHand)
                Next
            End If

        Catch ex As Exception
            Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",GetStockOnHand," & ex.Message & "," & tmpSQL & ",database," & _database & ",branch_code," & BranchCode)
            End Using

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '         "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & BranchCode & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If

            Return Nothing
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return _StockOnHands

    End Function

    Public Function GetAllStockOnHand(ByVal BranchCode As String, ByVal ServerPath As String,
                                      ByVal sIPAddress As String) As List(Of StockOnHand)

        tmpSQL = "SELECT * FROM stock_on_hand WHERE branch_code = '" & RG.Apos(BranchCode) & "' AND qty_on_hand <> 0 ORDER BY generated_code ASC"

        Dim _StockOnHands As New List(Of StockOnHand)

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                DoWebServiceLog(BranchCode, "GetAllStockOnHand", "", "", ds.Tables(0).Rows.Count)
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _StockOnHand As New StockOnHand
                    _StockOnHand.generated_code = dr("generated_code") & ""
                    _StockOnHand.qty_on_hand = dr("qty_on_hand") & ""

                    _StockOnHands.Add(_StockOnHand)
                Next
            End If

        Catch ex As Exception
            Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",GetStockOnHand," & ex.Message & "," & tmpSQL & ",database," & _database & ",branch_code," & BranchCode)
            End Using

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '         "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & BranchCode & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If

            Return Nothing
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return _StockOnHands

    End Function

    Public Function GetEmployeeClockNumbers(ByVal SearchText As String) As DataTable
        Dim xdata As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            connectionPositive.Open()
            command.Connection = connectionPositive
            command.CommandType = CommandType.Text

            tmpSQL = "SELECT * " &
                     "FROM employee_details " &
                     "WHERE employee_number ILIKE '" & RG.Apos(SearchText) & "%' " &
                     "ORDER BY employee_number LIMIT 10"

            Dim reader As New NpgsqlDataAdapter(tmpSQL, connectionPositive)
            reader.Fill(xdata)

        Catch ex As Exception
            If (connection IsNot Nothing) Then
                connectionPositive.Close()
            End If
            Return Nothing
        Finally
            If (connectionPositive IsNot Nothing) Then
                connectionPositive.Close()
            End If

        End Try

        Return xdata

    End Function

    Public Function GetEmployeeDetails(ByVal LastUpdateDate As String,
                                       ByVal BranchCode As String, ByVal ServerPath As String,
                                       ByVal sIPAddress As String) As List(Of EmployeeDetail)

        tmpSQL = "SELECT * FROM employee_details WHERE date_last_updated >= '" & LastUpdateDate & "'  
                  AND is_active = true ORDER BY date_last_updated DESC LIMIT 10000"

        Dim _EmployeeDetails As New List(Of EmployeeDetail)

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                DoWebServiceLog(BranchCode, "GetEmployeeDetails", LastUpdateDate, "", ds.Tables(0).Rows.Count)
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _EmployeeDetail As New EmployeeDetail
                    _EmployeeDetail.employee_number = dr("employee_number") & ""
                    _EmployeeDetail.first_name = dr("first_name") & ""
                    _EmployeeDetail.last_name = dr("last_name") & ""
                    _EmployeeDetail.is_active = dr("is_active") & ""

                    _EmployeeDetails.Add(_EmployeeDetail)
                Next
            End If

        Catch ex As Exception
            Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",GetEmployeeDetails," & ex.Message & "," & tmpSQL & ",database," & _database & ",branch_code," & BranchCode)
            End Using

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '         "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & BranchCode & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If

            Return Nothing
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return _EmployeeDetails

    End Function

    Public Function GetSizeGrids(ByVal LastInsertDate As String, ByVal LastUpdateDate As String,
                                 ByVal BranchCode As String, ByVal ServerPath As String,
                                 ByVal sIPAddress As String) As List(Of SizeGrid)

        tmpSQL = "SELECT * FROM size_grids WHERE (inserted >= '" & LastInsertDate & "' OR updated >= '" & LastUpdateDate & "')"

        Dim _SizeGrids As New List(Of SizeGrid)

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                DoWebServiceLog(BranchCode, "GetSizeGrids", LastUpdateDate, "", ds.Tables(0).Rows.Count)
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _SizeGrid As New SizeGrid
                    _SizeGrid.grid_number = dr("grid_number") & ""
                    _SizeGrid.grid_description = dr("grid_description") & ""
                    _SizeGrid.s1 = dr("s1") & ""
                    _SizeGrid.s2 = dr("s2") & ""
                    _SizeGrid.s3 = dr("s3") & ""
                    _SizeGrid.s4 = dr("s4") & ""
                    _SizeGrid.s5 = dr("s5") & ""
                    _SizeGrid.s6 = dr("s6") & ""
                    _SizeGrid.s7 = dr("s7") & ""
                    _SizeGrid.s8 = dr("s8") & ""
                    _SizeGrid.s9 = dr("s9") & ""
                    _SizeGrid.s10 = dr("s10") & ""
                    _SizeGrid.s11 = dr("s11") & ""
                    _SizeGrid.s12 = dr("s12") & ""
                    _SizeGrid.s13 = dr("s13") & ""
                    _SizeGrid.s14 = dr("s14") & ""
                    _SizeGrid.s15 = dr("s15") & ""
                    _SizeGrid.s16 = dr("s16") & ""
                    _SizeGrid.s17 = dr("s17") & ""
                    _SizeGrid.s18 = dr("s18") & ""
                    _SizeGrid.s19 = dr("s19") & ""
                    _SizeGrid.s20 = dr("s20") & ""
                    _SizeGrid.s21 = dr("s21") & ""
                    _SizeGrid.s22 = dr("s22") & ""
                    _SizeGrid.s23 = dr("s23") & ""
                    _SizeGrid.s24 = dr("s24") & ""
                    _SizeGrid.s25 = dr("s25") & ""
                    _SizeGrid.s26 = dr("s26") & ""
                    _SizeGrid.s27 = dr("s27") & ""
                    _SizeGrid.s28 = dr("s28") & ""
                    _SizeGrid.s29 = dr("s29") & ""
                    _SizeGrid.s30 = dr("s30") & ""

                    _SizeGrids.Add(_SizeGrid)
                Next
            End If

        Catch ex As Exception
            Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",GetSizeGrids," & ex.Message & "," & tmpSQL & ",database," & _database & ",branch_code," & BranchCode)
            End Using

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '         "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & BranchCode & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If

            Return Nothing
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return _SizeGrids

    End Function

    Public Function GetCompanyDetails(ByVal ServerPath As String,
                                      ByVal LastUpdateDate As String,
                                      ByVal BranchCode As String,
                                      ByVal sIPAddress As String) As CompanyDetails

        tmpSQL = "SELECT * FROM company_details WHERE updated >= '" & LastUpdateDate & "'"

        Dim _CompanyDetails As New CompanyDetails

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                DoWebServiceLog(BranchCode, "GetCompanyDetails", LastUpdateDate, "", ds.Tables(0).Rows.Count)
                For Each dr As DataRow In ds.Tables(0).Rows

                    _CompanyDetails.company_code = dr("company_code") & ""
                    _CompanyDetails.company_name = dr("company_name") & ""
                    _CompanyDetails.allow_negative_stock = dr("allow_negative_stock") & ""
                    _CompanyDetails.currency_symbol = dr("currency_symbol") & ""
                    _CompanyDetails.round_down_to_closest = dr("round_down_to_closest") & ""
                    _CompanyDetails.maximum_discount_per_row = dr("maximum_discount_per_row") & ""
                    _CompanyDetails.default_purchase_tax = dr("default_purchase_tax") & ""
                    _CompanyDetails.default_sales_tax = dr("default_sales_tax") & ""
                    _CompanyDetails.points_to_cash = dr("points_to_cash") & ""
                    _CompanyDetails.cash_to_points = dr("cash_to_points") & ""
                    _CompanyDetails.image_server = dr("image_server") & ""

                Next
            End If

        Catch ex As Exception
            Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",GetCompanyDetails," & ex.Message & "," & tmpSQL & ",database," & _database & ",branch_code," & BranchCode)
            End Using

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '         "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & BranchCode & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If

            Return Nothing
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return _CompanyDetails

    End Function

    Public Function NeedsRestart(ByVal ServerPath As String) As String

        Dim RebootCode As String = String.Empty

        tmpSQL = "SELECT * FROM restart_code WHERE is_pos = False"

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    RebootCode = dr("restart_code")
                Next
            End If

        Catch ex As Exception
            Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",NeedsReboot," & ex.Message & "," & tmpSQL & ",database," & _database)
            End Using

            Return Nothing
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return RebootCode

    End Function

    Public Function NeedsRestartPOS(ByVal ServerPath As String) As String

        Dim RebootCode As String = String.Empty

        tmpSQL = "SELECT * FROM restart_code WHERE is_pos = True"

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    RebootCode = dr("restart_code")
                Next
            End If

        Catch ex As Exception
            Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",NeedsReboot," & ex.Message & "," & tmpSQL & ",database," & _database)
            End Using

            Return Nothing
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return RebootCode

    End Function


    Public Function GetStockcodes(ByVal LastInsertDate As String, ByVal LastUpdateDate As String,
                                  ByVal isFirstRun As Boolean, ByVal ServerPath As String,
                                  ByVal BranchCode As String) As List(Of Stockcodes)

        Dim SixMonthsAgo As String
        SixMonthsAgo = Format(Now.Date.AddDays(-180), "yyyy-MM-dd")
        Dim TwelveMonthsAgo As String
        TwelveMonthsAgo = Format(Now.Date.AddDays(-365), "yyyy-MM-dd")

        If isFirstRun = True Then
            tmpSQL = "SELECT stockcodes_categories.generated_code,stockcodes_categories.master_code,stockcodes_categories.category_1," &
                     "stockcodes_categories.category_2,stockcodes_categories.category_3,stockcodes_master.sku_number," &
                     "stockcodes_master.is_service_item,stockcodes_master.description,stockcodes_master.purchase_tax_group," &
                     "stockcodes_master.sales_tax_group,stockcodes_master.supplier,stockcodes_master.suppliers_code," &
                     "stockcodes_master.is_not_discountable,stockcodes_master.is_blocked,stockcodes_master.minimum_stock_level," &
                     "stockcodes_master.item_size,stockcodes_master.item_colour,stockcodes_master.size_matrix,stockcodes_master.colour_matrix," &
                     "stockcodes_prices.cost_price,stockcodes_prices.estimated_cost,stockcodes_prices.selling_price_1,stockcodes_prices.selling_price_2," &
                     "stockcodes_prices.selling_price_3,stockcodes_dates.updated,stockcodes_dates.created " &
                     "FROM stockcodes_categories " &
                     "LEFT OUTER JOIN stockcodes_master ON stockcodes_categories.generated_code = stockcodes_master.generated_code " &
                     "LEFT OUTER JOIN stockcodes_prices ON stockcodes_categories.generated_code = stockcodes_prices.generated_code " &
                     "LEFT OUTER JOIN stockcodes_dates ON stockcodes_categories.generated_code = stockcodes_dates.generated_code " &
                     "WHERE is_blocked = False " &
                     "AND (stockcodes_dates.created >= '" & SixMonthsAgo & "' OR stockcodes_dates.updated >= '" & SixMonthsAgo & "')" &
                     "ORDER BY stockcodes_master.generated_code "
        Else
            tmpSQL = "SELECT stockcodes_categories.generated_code,stockcodes_categories.master_code,stockcodes_categories.category_1," &
                     "stockcodes_categories.category_2,stockcodes_categories.category_3,stockcodes_master.sku_number," &
                     "stockcodes_master.is_service_item,stockcodes_master.description,stockcodes_master.purchase_tax_group," &
                     "stockcodes_master.sales_tax_group,stockcodes_master.supplier,stockcodes_master.suppliers_code," &
                     "stockcodes_master.is_not_discountable,stockcodes_master.is_blocked,stockcodes_master.minimum_stock_level," &
                     "stockcodes_master.item_size,stockcodes_master.item_colour,stockcodes_master.size_matrix,stockcodes_master.colour_matrix," &
                     "stockcodes_prices.cost_price,stockcodes_prices.estimated_cost,stockcodes_prices.selling_price_1,stockcodes_prices.selling_price_2," &
                     "stockcodes_prices.selling_price_3,stockcodes_dates.updated,stockcodes_dates.created " &
                     "FROM stockcodes_categories " &
                     "LEFT OUTER JOIN stockcodes_master ON stockcodes_categories.generated_code = stockcodes_master.generated_code " &
                     "LEFT OUTER JOIN stockcodes_prices ON stockcodes_categories.generated_code = stockcodes_prices.generated_code " &
                     "LEFT OUTER JOIN stockcodes_dates ON stockcodes_categories.generated_code = stockcodes_dates.generated_code " &
                     "WHERE (stockcodes_dates.created >= '" & LastInsertDate & "' OR stockcodes_dates.updated >= '" & LastUpdateDate & "')" &
                     "AND is_blocked = False " &
                     "ORDER BY stockcodes_master.generated_code " &
                     "LIMIT 30000"
        End If

        Dim _Stockcodes As New List(Of Stockcodes)

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                DoWebServiceLog(BranchCode, "GetStockcodes", LastUpdateDate, "", ds.Tables(0).Rows.Count)
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _stockcode As New Stockcodes
                    _stockcode.sku_number = dr("sku_number") & ""
                    _stockcode.category_1 = dr("category_1") & ""
                    _stockcode.category_2 = dr("category_2") & ""
                    _stockcode.category_3 = dr("category_3") & ""
                    _stockcode.cost_price = dr("cost_price") & ""
                    _stockcode.estimated_cost = dr("estimated_cost") & ""
                    _stockcode.description = dr("description") & ""
                    _stockcode.generated_code = dr("generated_code") & ""
                    _stockcode.is_blocked = dr("is_blocked") & ""
                    _stockcode.is_not_discountable = dr("is_not_discountable") & ""
                    _stockcode.is_service_item = dr("is_service_item") & ""
                    _stockcode.item_colour = dr("item_colour") & ""
                    _stockcode.item_size = dr("item_size") & ""
                    _stockcode.master_code = dr("master_code") & ""
                    _stockcode.minimum_stock_level = dr("minimum_stock_level") & ""
                    _stockcode.purchase_tax_group = dr("purchase_tax_group") & ""
                    _stockcode.sales_tax_group = dr("sales_tax_group") & ""
                    _stockcode.selling_price_1 = dr("selling_price_1") & ""
                    _stockcode.selling_price_2 = dr("selling_price_2") & ""
                    _stockcode.selling_price_3 = dr("selling_price_3") & ""
                    _stockcode.size_matrix = dr("size_matrix") & ""
                    _stockcode.colour_matrix = dr("colour_matrix") & ""
                    _stockcode.supplier = dr("supplier") & ""
                    _stockcode.suppliers_code = dr("suppliers_code") & ""
                    _stockcode.created = dr("created") & ""
                    _stockcode.updated = dr("updated") & ""

                    _Stockcodes.Add(_stockcode)
                Next
            End If

        Catch ex As Exception
            Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",GetStockcodes," & ex.Message & "," & tmpSQL & ",database," & _database & ",branch_code," & BranchCode)
            End Using

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '         "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & BranchCode & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If

            Return Nothing
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return _Stockcodes

    End Function

    Public Function GetSomeStockcodes(ByVal ServerPath As String) As List(Of Stockcodes)

        Dim SixMonthsAgo As String
        SixMonthsAgo = Format(Now.Date.AddDays(-180), "yyyy-MM-dd")
        Dim TwelveMonthsAgo As String
        TwelveMonthsAgo = Format(Now.Date.AddDays(-365), "yyyy-MM-dd")

        tmpSQL = "SELECT stockcodes_categories.generated_code,stockcodes_categories.master_code,stockcodes_categories.category_1," &
                 "stockcodes_categories.category_2,stockcodes_categories.category_3,stockcodes_master.sku_number," &
                 "stockcodes_master.is_service_item,stockcodes_master.description,stockcodes_master.purchase_tax_group," &
                 "stockcodes_master.sales_tax_group,stockcodes_master.supplier,stockcodes_master.suppliers_code," &
                 "stockcodes_master.is_not_discountable,stockcodes_master.is_blocked,stockcodes_master.minimum_stock_level," &
                 "stockcodes_master.item_size,stockcodes_master.item_colour,stockcodes_master.size_matrix,stockcodes_master.colour_matrix," &
                 "stockcodes_prices.cost_price,stockcodes_prices.estimated_cost,stockcodes_prices.selling_price_1,stockcodes_prices.selling_price_2," &
                 "stockcodes_prices.selling_price_3,stockcodes_dates.updated,stockcodes_dates.created " &
                 "FROM stockcodes_categories " &
                 "LEFT OUTER JOIN stockcodes_master ON stockcodes_categories.generated_code = stockcodes_master.generated_code " &
                 "LEFT OUTER JOIN stockcodes_prices ON stockcodes_categories.generated_code = stockcodes_prices.generated_code " &
                 "LEFT OUTER JOIN stockcodes_dates ON stockcodes_categories.generated_code = stockcodes_dates.generated_code " &
                 "WHERE is_blocked = False " &
                 "AND (stockcodes_dates.created >= '" & SixMonthsAgo & "' OR stockcodes_dates.updated >= '" & SixMonthsAgo & "')" &
                 "ORDER BY stockcodes_master.generated_code LIMIT 35000"



        Dim _Stockcodes As New List(Of Stockcodes)

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _stockcode As New Stockcodes
                    _stockcode.sku_number = dr("sku_number") & ""
                    _stockcode.category_1 = ""
                    _stockcode.category_2 = dr("category_2") & ""
                    _stockcode.category_3 = dr("category_3") & ""
                    _stockcode.cost_price = ""
                    _stockcode.estimated_cost = ""
                    _stockcode.description = dr("description") & ""
                    _stockcode.generated_code = dr("generated_code") & ""
                    _stockcode.is_blocked = dr("is_blocked") & ""
                    _stockcode.is_not_discountable = dr("is_not_discountable") & ""
                    _stockcode.is_service_item = dr("is_service_item") & ""
                    _stockcode.item_colour = dr("item_colour") & ""
                    _stockcode.item_size = dr("item_size") & ""
                    _stockcode.master_code = dr("master_code") & ""
                    _stockcode.minimum_stock_level = dr("minimum_stock_level") & ""
                    _stockcode.purchase_tax_group = dr("purchase_tax_group") & ""
                    _stockcode.sales_tax_group = dr("sales_tax_group") & ""
                    _stockcode.selling_price_1 = dr("selling_price_1") & ""
                    _stockcode.selling_price_2 = dr("selling_price_2") & ""
                    _stockcode.selling_price_3 = dr("selling_price_3") & ""
                    _stockcode.size_matrix = dr("size_matrix") & ""
                    _stockcode.colour_matrix = dr("colour_matrix") & ""
                    _stockcode.supplier = dr("supplier") & ""
                    _stockcode.suppliers_code = dr("suppliers_code") & ""
                    _stockcode.created = dr("created") & ""
                    _stockcode.updated = dr("updated") & ""

                    _Stockcodes.Add(_stockcode)
                Next
            End If

        Catch ex As Exception
            Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",GetStockcodes," & ex.Message & "," & tmpSQL & ",database," & _database)
            End Using

            'tmpSQL = "INSERT INTO upload_errors (guid, sql_statement, branch_code, error_description) VALUES " &
            '         "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & BranchCode & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If

            Return Nothing
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return _Stockcodes

    End Function

    Public Function GetBackdatedStockcodes(ByVal DateFrom As String, ByVal DateTo As String,
                                           ByVal ServerPath As String,
                                           ByVal BranchCode As String) As List(Of Stockcodes)

        Dim SixMonthsAgo As String
        SixMonthsAgo = Format(Now.Date.AddDays(-180), "yyyy-MM-dd")
        Dim TwelveMonthsAgo As String
        TwelveMonthsAgo = Format(Now.Date.AddDays(-365), "yyyy-MM-dd")

        tmpSQL = "SELECT stockcodes_categories.generated_code,stockcodes_categories.master_code,stockcodes_categories.category_1," &
                 "stockcodes_categories.category_2,stockcodes_categories.category_3,stockcodes_master.sku_number," &
                 "stockcodes_master.is_service_item,stockcodes_master.description,stockcodes_master.purchase_tax_group," &
                 "stockcodes_master.sales_tax_group,stockcodes_master.supplier,stockcodes_master.suppliers_code," &
                 "stockcodes_master.is_not_discountable,stockcodes_master.is_blocked,stockcodes_master.minimum_stock_level," &
                 "stockcodes_master.item_size,stockcodes_master.item_colour,stockcodes_master.size_matrix,stockcodes_master.colour_matrix," &
                 "stockcodes_prices.cost_price,stockcodes_prices.estimated_cost,stockcodes_prices.selling_price_1,stockcodes_prices.selling_price_2," &
                 "stockcodes_prices.selling_price_3,stockcodes_dates.updated,stockcodes_dates.created " &
                 "FROM stockcodes_categories " &
                 "LEFT OUTER JOIN stockcodes_master ON stockcodes_categories.generated_code = stockcodes_master.generated_code " &
                 "LEFT OUTER JOIN stockcodes_prices ON stockcodes_categories.generated_code = stockcodes_prices.generated_code " &
                 "LEFT OUTER JOIN stockcodes_dates ON stockcodes_categories.generated_code = stockcodes_dates.generated_code " &
                 "WHERE (stockcodes_dates.created BETWEEN '" & DateFrom & "' AND '" & DateTo & "' " &
                 "OR stockcodes_dates.updated BETWEEN '" & DateFrom & "' AND '" & DateTo & "')" &
                 "AND is_blocked = False " &
                 "ORDER BY stockcodes_master.generated_code"

        Dim _Stockcodes As New List(Of Stockcodes)

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                DoWebServiceLog(BranchCode, "GetBackdatedStockcodes", DateFrom, DateTo, ds.Tables(0).Rows.Count)
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _stockcode As New Stockcodes
                    _stockcode.sku_number = dr("sku_number") & ""
                    _stockcode.category_1 = dr("category_1") & ""
                    _stockcode.category_2 = dr("category_2") & ""
                    _stockcode.category_3 = dr("category_3") & ""
                    _stockcode.cost_price = dr("cost_price") & ""
                    _stockcode.estimated_cost = dr("estimated_cost") & ""
                    _stockcode.description = dr("description") & ""
                    _stockcode.generated_code = dr("generated_code") & ""
                    _stockcode.is_blocked = dr("is_blocked") & ""
                    _stockcode.is_not_discountable = dr("is_not_discountable") & ""
                    _stockcode.is_service_item = dr("is_service_item") & ""
                    _stockcode.item_colour = dr("item_colour") & ""
                    _stockcode.item_size = dr("item_size") & ""
                    _stockcode.master_code = dr("master_code") & ""
                    _stockcode.minimum_stock_level = dr("minimum_stock_level") & ""
                    _stockcode.purchase_tax_group = dr("purchase_tax_group") & ""
                    _stockcode.sales_tax_group = dr("sales_tax_group") & ""
                    _stockcode.selling_price_1 = dr("selling_price_1") & ""
                    _stockcode.selling_price_2 = dr("selling_price_2") & ""
                    _stockcode.selling_price_3 = dr("selling_price_3") & ""
                    _stockcode.size_matrix = dr("size_matrix") & ""
                    _stockcode.colour_matrix = dr("colour_matrix") & ""
                    _stockcode.supplier = dr("supplier") & ""
                    _stockcode.suppliers_code = dr("suppliers_code") & ""
                    _stockcode.created = dr("created") & ""
                    _stockcode.updated = dr("updated") & ""

                    _Stockcodes.Add(_stockcode)
                Next
            End If

        Catch ex As Exception
            Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",GetStockcodes," & ex.Message & "," & tmpSQL & ",database," & _database & ",branch_code," & BranchCode)
            End Using

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '         "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & BranchCode & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If

            Return Nothing
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return _Stockcodes

    End Function

    Public Function GetSingleStockcode(ByVal MasterCodeOrBlank As String,
                                       ByVal GeneratedcodeOrBlank As String,
                                       ByVal ServerPath As String,
                                       ByVal BranchCode As String) As List(Of Stockcodes)


        tmpSQL = "SELECT stockcodes_categories.generated_code,stockcodes_categories.master_code,stockcodes_categories.category_1," &
                 "stockcodes_categories.category_2,stockcodes_categories.category_3,stockcodes_master.sku_number," &
                 "stockcodes_master.is_service_item,stockcodes_master.description,stockcodes_master.purchase_tax_group," &
                 "stockcodes_master.sales_tax_group,stockcodes_master.supplier,stockcodes_master.suppliers_code," &
                 "stockcodes_master.is_not_discountable,stockcodes_master.is_blocked,stockcodes_master.minimum_stock_level," &
                 "stockcodes_master.item_size,stockcodes_master.item_colour,stockcodes_master.size_matrix,stockcodes_master.colour_matrix," &
                 "stockcodes_prices.cost_price,stockcodes_prices.estimated_cost,stockcodes_prices.selling_price_1,stockcodes_prices.selling_price_2," &
                 "stockcodes_prices.selling_price_3,stockcodes_dates.updated,stockcodes_dates.created " &
                 "FROM stockcodes_categories " &
                 "LEFT OUTER JOIN stockcodes_master ON stockcodes_categories.generated_code = stockcodes_master.generated_code " &
                 "LEFT OUTER JOIN stockcodes_prices ON stockcodes_categories.generated_code = stockcodes_prices.generated_code " &
                 "LEFT OUTER JOIN stockcodes_dates ON stockcodes_categories.generated_code = stockcodes_dates.generated_code "

        If MasterCodeOrBlank <> "" Then
            tmpSQL &= "WHERE stockcodes_master.master_code = '" & RG.Apos(MasterCodeOrBlank) & "' " &
                      "ORDER BY stockcodes_master.generated_code"
        End If

        If GeneratedcodeOrBlank <> "" Then
            tmpSQL &= "WHERE stockcodes_master.generated_code = '" & RG.Apos(MasterCodeOrBlank) & "' " &
                      "ORDER BY stockcodes_master.generated_code"
        End If


        Dim _Stockcodes As New List(Of Stockcodes)

        Try
            ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(ds) Then
                DoWebServiceLog(BranchCode, "GetSingleStockcode", "", "", ds.Tables(0).Rows.Count)
                For Each dr As DataRow In ds.Tables(0).Rows
                    Dim _stockcode As New Stockcodes
                    _stockcode.sku_number = dr("sku_number") & ""
                    _stockcode.category_1 = dr("category_1") & ""
                    _stockcode.category_2 = dr("category_2") & ""
                    _stockcode.category_3 = dr("category_3") & ""
                    _stockcode.cost_price = dr("cost_price") & ""
                    _stockcode.estimated_cost = dr("estimated_cost") & ""
                    _stockcode.description = dr("description") & ""
                    _stockcode.generated_code = dr("generated_code") & ""
                    _stockcode.is_blocked = dr("is_blocked") & ""
                    _stockcode.is_not_discountable = dr("is_not_discountable") & ""
                    _stockcode.is_service_item = dr("is_service_item") & ""
                    _stockcode.item_colour = dr("item_colour") & ""
                    _stockcode.item_size = dr("item_size") & ""
                    _stockcode.master_code = dr("master_code") & ""
                    _stockcode.minimum_stock_level = dr("minimum_stock_level") & ""
                    _stockcode.purchase_tax_group = dr("purchase_tax_group") & ""
                    _stockcode.sales_tax_group = dr("sales_tax_group") & ""
                    _stockcode.selling_price_1 = dr("selling_price_1") & ""
                    _stockcode.selling_price_2 = dr("selling_price_2") & ""
                    _stockcode.selling_price_3 = dr("selling_price_3") & ""
                    _stockcode.size_matrix = dr("size_matrix") & ""
                    _stockcode.colour_matrix = dr("colour_matrix") & ""
                    _stockcode.supplier = dr("supplier") & ""
                    _stockcode.suppliers_code = dr("suppliers_code") & ""
                    _stockcode.created = dr("created") & ""
                    _stockcode.updated = dr("updated") & ""

                    _Stockcodes.Add(_stockcode)
                Next
            End If

        Catch ex As Exception
            Using tmpStreamWriter As New StreamWriter(ServerPath, True)
                tmpStreamWriter.WriteLine(Format(Now, "yyyy-MM-dd HH:mm:ss") & ",GetSingleStockcode," & ex.Message & "," & tmpSQL & ",database," & _database & ",branch_code," & BranchCode)
            End Using

            'tmpSQL = "INSERT INTO upload_errors (guid,sql_statement,branch_code,error_description) VALUES " &
            '         "('" & Guid.NewGuid.ToString & "','" & RG.Apos(tmpSQL) & "','" & BranchCode & "','" & RG.Apos(ex.Message) & "')"
            'objDBWrite.ExecuteQuery(tmpSQL)

            'If (objDBWrite IsNot Nothing) Then
            '    objDBWrite.CloseConnection()
            'End If

            Return Nothing
        End Try

        If (objDBRead IsNot Nothing) Then
            objDBRead.CloseConnection()
        End If

        Return _Stockcodes

    End Function

End Class
