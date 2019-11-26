Imports Entities
Public Class Testing1_DataLayer
    Inherits DataAccessLayerBase
    ''' <summary>
    ''' This Function is used to Get Special Name,Master Code and Generated Code From Special_master,Stockcodes_master Tables and Returns in  DataTable
    ''' </summary>
    ''' Created Date:16-07-2019
    ''' Created By:Kamal 
    Public Function Search(srch As Search) As DataTable

        Dim GetSpecialName As DataTable
        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPositiveRead")

        If srch.Search_Type = "sp" Then
            tmpSQL = "SELECT special_name,start_date,end_date FROM specials_master WHERE special_name LIKE '" & (srch.Search_Detail) & "%'
                      ORDER BY special_name ASC LIMIT " & srch.Limit
        End If
        If srch.Search_Type = "mc" Then
            tmpSQL = "SELECT master_code,description,sku_number FROM (SELECT DISTINCT ON (master_code) * FROM stockcodes_master WHERE master_code LIKE '" & (srch.Search_Detail) & "%') 
                     stockcodes_master ORDER BY master_code ASC LIMIT " & srch.Limit
        End If
        If srch.Search_Type = "gc" Then
            tmpSQL = "SELECT generated_code,description,sku_number FROM stockcodes_master WHERE generated_code LIKE '" & (srch.Search_Detail) & "%' 
                     ORDER BY generated_code ASC LIMIT " & srch.Limit
        End If

        Try
            Ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(Ds) Then
                GetSpecialName = Ds.Tables(0)
            Else
                Return Nothing
            End If

        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
        End Try
        Return GetSpecialName

    End Function
    ''' <summary>
    ''' This Function is used to Get Start_date,End_date,Active,Price and special_name from stockcodes_master,Special_master and Specials_line Tables and return in DataTable
    ''' </summary>
    ''' Created Date=16-07-2019
    ''' Created By=Kamal
    ''' Modified Date=
    ''' Modified By=
    ''' <returns>Data</returns>
    Public Function AddData(_test As Testing3) As DataTable
        Dim Data As DataTable
        Dim objDBRead As New dlNpgSQL("PostgreConnectionStringPositiveRead")
        tmpSQL = "SELECT m.start_date,m.end_date,m.is_active,m.price,m.update_date,m.special_name,l.master_code,l.qty, " &
                 "(SELECT description FROM stockcodes_master WHERE master_code = l.master_code LIMIT 1) AS description " &
                 "FROM specials_master m " &
                 "INNER JOIN specials_line_items l ON m.special_id = l.specials_link_id " &
                 "WHERE special_name = '" & (_test.Special_Name) & "'"
        Try
            Ds = objDBRead.GetDataSet(tmpSQL)
            If objDBRead.isR(Ds) Then
                Data = Ds.Tables(0)
            Else
                Return Nothing
            End If

        Catch ex As Exception
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDBRead IsNot Nothing) Then
                objDBRead.CloseConnection()
            End If
        End Try
        Return Data

    End Function

End Class
