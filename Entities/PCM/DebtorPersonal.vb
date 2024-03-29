﻿Public Class DebtorsMaintainenceRequest
    Public Property PersonalDetails As DebtorsPersonalDetails
    Public Property AddressDetails As DebtorsAddressDetails
    Public Property EmploymentDetails As DebtorsEmploymentDetails
    Public Property BankingDetails As DebtorsBankingDetails
    Public Property CommonDetails As DebtorsCommonDetails
    Public Property UserPermissions As UserPermissionDetails
End Class

Public Class DebtorsPersonalDetails
    Public Property IDNumber As String
    Public Property AccountNumber As String
    Public Property Title As String
    Public Property FirstName As String
    Public Property LastName As String
    Public Property MiddleName As String
    Public Property DOB As String
    Public Property Gender As String
    Public Property HomeNumber1 As String
    Public Property CellularNumber As String
    Public Property DontSend As Boolean
    Public Property Email As String
    Public Property SendPromos As Boolean
    Public Property ContactNumber As String
    Public Property StatementDelivery As String
    Public Property CardProtection As Boolean
    Public Property AutoIncrease As Boolean
    Public Property AgeAnalysis As Boolean
    Public Property BranchName As String
    Public Property PreferredLanguage As String
    Public Property CurrentStatus As String
    Public Property CreditLimit As String
    Public Property CardNumber1 As String
    Public Property ITCRating As String
End Class

Public Class DebtorsAddressDetails
    Public Property cboHindi As String
    Public Property Lop As String
    Public Property Loc As String

    '******** Current Residential *********
    Public Property tA_0 As String
    Public Property tA_1 As String
    Public Property tA_2 As String
    Public Property tA_3 As String
    Public Property cboProv1 As String
    Public Property tA_4 As String

    '******** Previous Residential *********
    Public Property tA_5 As String
    Public Property tA_6 As String
    Public Property tA_7 As String
    Public Property tA_8 As String
    Public Property cboProv2 As String
    Public Property tA_9 As String

    '******** Next of kin Residential *********
    Public Property tA_10 As String
    Public Property tA_11 As String
    Public Property tA_12 As String
    Public Property tA_13 As String
    Public Property cboProv3 As String
    Public Property tA_14 As String

    '******** Postal *********
    Public Property tA_15 As String
    Public Property tA_16 As String
    Public Property tA_17 As String
    Public Property tA_18 As String

End Class

Public Class DebtorsEmploymentDetails
    Public Property tE_0 As String
    Public Property tE_1 As String
    Public Property tE_2 As String
    Public Property tE_3 As String
    Public Property tE_4 As String
    Public Property tE_5 As String
    Public Property tE_6 As String
    Public Property tE_7 As String
    Public Property tE_8 As String
    Public Property tE_9 As String
    Public Property tE_10 As String
    Public Property tE_11 As String
    Public Property tE_12 As String
    Public Property tE_13 As String
    Public Property Work1 As String
    Public Property Work3 As String
    Public Property Work2 As String
    Public Property WorkFax1 As String
    Public Property chkRageEmployee As Boolean
    Public Property ClockNumber As String
    Public Property LOS As String
    Public Property LOS2 As String

End Class

Public Class DebtorsBankingDetails
    Public Property cboTOA As String
    Public Property cboBName As String
    Public Property tB_0 As String
    Public Property tB_1 As String
    Public Property tB_2 As String
    Public Property cboCC1Type As String
    Public Property cboCC2Type As String
    Public Property tB_3 As String
    Public Property tB_4 As String
    Public Property cboPType As String

End Class

Public Class DebtorsCommonDetails
    Public Property OldNotes As String
    Public Property NewNotes As String
    Public Property User As String
    Public Property Password As String
    Public Property Updated As Boolean
    Public Property ThinFileCall As Boolean
    Public Property ButtonValue As String
End Class

Public Class UserPermissionDetails
    Public Property isLevel0 As Boolean
    Public Property isLevel1 As Boolean
    Public Property isLevel2 As Boolean
    Public Property UserIsSupervisor As Boolean
    Public Property lStatus As String
    Public Property lCLimit As String
    Public Property lThinFileCall As Boolean
    Public Property lBranchCode As String
    Public Property lLanguage As String
    Public Property lIsUpdated As Boolean
End Class




'************** Response *****************

Public Class BaseResponse
    Public Property Success As Boolean
    Public Property Message As String
    Public Property QuestionnaireID As String
End Class


Public Class DebtorDetailsResponse
    Inherits BaseResponse
    Public Property GetSelectedDebtorsResponse As DataTable
End Class

Public Class DebtorsReporting

    Public Property AgeAnalysis As List(Of Debtor_AgeAnalysis)
    Public Property ContactHistory As List(Of Debtor_ContactHistory)
    Public Property ChangeHistory As List(Of DebtorChangeLog)
    Public Property Transactions As List(Of Transactions)
    Public Property PaymentPlans As List(Of Debtor_PaymentPlan)
    Public Property ClosingBalances As List(Of Debtor_ClosingBalances)
End Class
