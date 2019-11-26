Public Class Maintenance

End Class

Public Class EmployeeNoteRequest
    Public Property EmployeeName As String
    Public Property EmployeeNumber As String
    Public Property TypeOfReport As String
    Public Property Warning As String
    Public Property Rating As String
    Public Property Note As String
    Public Property BranchCode As String
    Public Property WarningExpiryDate As String

End Class
Public Class Branches

    Public Property ListOfBranches As List(Of Branch)
    Public Property return_message As String

End Class

Public Class Branch
    Public Property branch_code As String
    Public Property branch_name As String
    Public Property address_line_1 As String
    Public Property address_line_2 As String
    Public Property address_line_3 As String
    Public Property address_line_4 As String
    Public Property address_line_5 As String
    Public Property telephone_number As String
    Public Property fax_number As String
    Public Property email_address As String
    Public Property pricelevel As String
    Public Property tax_number As String
    Public Property is_blocked As Boolean
    Public Property is_head_office As Boolean
    Public Property branch_type As String
    Public Property merchant_number As String
    Public Property updated As String
    Public Property return_message As String
    Public Property target As String


End Class

Public Class Testing2
    Public Property Employee_Number As Integer
    Public Property First_Name As String
    Public Property Last_Name As String
    Public Property Active As Boolean
    Public Property Date_of_Birth As String
    Public Property Email_Address As String
    Public Property Gender As String
End Class

Public Class Testing3
    Public Property Special_Name As String
    Public Property Start_Date As DateTime
    Public Property End_Date As DateTime
    Public Property Active As Boolean
    Public Property Price As Integer
    Public Property Code As String
    Public Property Qty As Integer
End Class
Public Class Search
    Public Property Search_Type As String
    Public Property Search_Detail As String
    Public Property Limit As Integer
End Class
