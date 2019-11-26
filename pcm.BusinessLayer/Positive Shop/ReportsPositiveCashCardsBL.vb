Imports pcm.DataLayer
Public Class ReportsPositiveCashCardsBL

    Private ReadOnly _dLayer As ReportsPositiveCashCardsDL

    Public Sub New(ByVal CompanyCode As String)
        _dLayer = New ReportsPositiveCashCardsDL(CompanyCode)
    End Sub

    Public Sub New()
        _dLayer = New ReportsPositiveCashCardsDL
    End Sub


    Public Function GetCashCardAccounts(ByVal SearchString As String) As DataTable

        Return _dLayer.GetCustomerAccounts(SearchString)

    End Function

    Public Function ReturnTransactionListing(ByVal StartDate As String, ByVal EndDate As String, _
                                             Optional ByVal AccountNumber As String = "") As DataTable

        Return _dLayer.ReturnTransactionListing(StartDate, EndDate, AccountNumber)

    End Function

    Public Function ReturnCustomerDetails(ByVal AccountNumber As String) As DataTable

        Return _dLayer.ReturnCustomerDetails(AccountNumber)

    End Function

    Public Function ReturnCashCardSummary(ByVal DateCreated As String, ByVal LastTransactionDate As String) As DataTable

        Return _dLayer.ReturnCashCardSummary(DateCreated, LastTransactionDate)

    End Function

    Public Function ReturnCashCard(ByVal DateFrom As String, ByVal DateTo As String,
                                   ByVal AllAccounts As Boolean) As DataTable

        Return _dLayer.ReturnCashCard(DateFrom, DateTo, AllAccounts)

    End Function

    Public Function ReturnCashCardSummaryLineItems(ByVal DateCreated As String, ByVal LastTransactionDate As String) As DataTable

        Return _dLayer.ReturnCashCardSummaryLineItems(DateCreated, LastTransactionDate)

    End Function

End Class
