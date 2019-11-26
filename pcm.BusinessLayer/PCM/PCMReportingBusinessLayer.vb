Imports Entities
Imports pcm.DataLayer
Public Class PCMReportingBusinessLayer
    Private _dlPCMReportingDataLayer As New PCMReportingDataLayer

    Public Function GetDetails(_getDetailsRequest As GetDetailsRequest) As GetDetailsResponse

        Dim getDetailsResponse As New GetDetailsResponse

        If _getDetailsRequest.ActiveOnly = False And _getDetailsRequest.OtherStatus = False Then
            GetDetailsResponse.Success = False
            GetDetailsResponse.Message = "Please select a Status option."
            Return GetDetailsResponse
        End If

        If _getDetailsRequest.OtherStatus = True And _getDetailsRequest.Other = "" Then
            GetDetailsResponse.Message = "Please select an option for Other Status"
            GetDetailsResponse.Success = False
            Return GetDetailsResponse
        End If

        If _getDetailsRequest.CheckCurrentUse = True Then
            If _getDetailsRequest.CboCurrent = "" Or Val(_getDetailsRequest.WhereCurrent) <= 0 Then
                GetDetailsResponse.Message = "Please select corresponding option and fill value"
                GetDetailsResponse.Success = False
                Return GetDetailsResponse
            End If
        End If

        If _getDetailsRequest.CheckUse30 = True Then
            If _getDetailsRequest.Cbo30Days = "" Or Val(_getDetailsRequest.Where30Days) <= 0 Then
                GetDetailsResponse.Message = "Please select corresponding option and fill value"
                GetDetailsResponse.Success = False
                Return GetDetailsResponse
            End If
        End If

        If _getDetailsRequest.CheckUse60 = True Then
            If _getDetailsRequest.Cbo60Days = "" Or Val(_getDetailsRequest.Where60Days) <= 0 Then
                GetDetailsResponse.Message = "Please select corresponding option and fill value"
                GetDetailsResponse.Success = False
                Return GetDetailsResponse
            End If
        End If

        If _getDetailsRequest.CheckUse90 = True Then
            If _getDetailsRequest.Cbo90Days = "" Or Val(_getDetailsRequest.Where90Days) <= 0 Then
                GetDetailsResponse.Message = "Please select corresponding option and fill value"
                GetDetailsResponse.Success = False
                Return GetDetailsResponse
            End If
        End If

        If _getDetailsRequest.CheckUse120 = True Then
            If _getDetailsRequest.Cbo120Days = "" Or Val(_getDetailsRequest.Where120Days) <= 0 Then
                GetDetailsResponse.Message = "Please select corresponding option and fill value"
                GetDetailsResponse.Success = False
                Return GetDetailsResponse
            End If
        End If

        If _getDetailsRequest.CheckUse150 = True Then
            If _getDetailsRequest.Cbo150Days = "" Or Val(_getDetailsRequest.Where150Days) <= 0 Then
                GetDetailsResponse.Message = "Please select corresponding option and fill value"
                GetDetailsResponse.Success = False
                Return GetDetailsResponse
            End If
        End If

        If _getDetailsRequest.CheckUsetotal = True Then
            If _getDetailsRequest.CboTotal = "" Or Val(_getDetailsRequest.Wheretotal) <= 0 Then
                GetDetailsResponse.Message = "Please select corresponding option and fill value"
                GetDetailsResponse.Success = False
                Return GetDetailsResponse
            End If
        End If

        If _getDetailsRequest.AccountsOpenedBetween = True Then
            If _getDetailsRequest.StartDate = "" Or _getDetailsRequest.EndDate = "" Then
                getDetailsResponse.Message = "Please select start date and end date"
                getDetailsResponse.Success = False
                Return getDetailsResponse
            End If
        End If

        If _getDetailsRequest.LastTransaction = True Then
            If _getDetailsRequest.LastDateTransaction = "" Then
                getDetailsResponse.Message = "Please select last transaction date"
                getDetailsResponse.Success = False
                Return getDetailsResponse
            End If
        End If

        Dim _getDetailsResponse As GetDetailsResponse
        _getDetailsResponse = _dlPCMReportingDataLayer.GetDetails(_getDetailsRequest)
        Return _getDetailsResponse

    End Function
    Public Function GetQuery(_queryRequest As QueryRequest) As QueryResponse
        Dim _queryResponse As QueryResponse
        _queryResponse = _dlPCMReportingDataLayer.GetQuery(_queryRequest)
        Return _queryResponse
    End Function

    Public Function GetEMailAddresses() As String

        Return _dlPCMReportingDataLayer.GetEMailAddresses

    End Function

    Public Function GetCellphoneNumbers() As String

        Return _dlPCMReportingDataLayer.GetCellphoneNumbers

    End Function

    Public Function GetCellphonesByBranch(ByVal BranchCode As String) As String

        Return _dlPCMReportingDataLayer.GetCellphonesByBranch(BranchCode)

    End Function

    Public Function GetAllCellphoneNumbers() As String

        Return _dlPCMReportingDataLayer.GetAllCellphoneNumbers

    End Function

    Public Function GetAgeAnalysisDetails(_getAgeAnalysisDetailRequest As GetAgeAnalysisDetailRequest) As GetAgeAnalysisDetailResponse
        Dim _ageAnalysisDetailResponse As GetAgeAnalysisDetailResponse
        _ageAnalysisDetailResponse = _dlPCMReportingDataLayer.GetAgeAnalysisDetails(_getAgeAnalysisDetailRequest)
        Return _ageAnalysisDetailResponse
    End Function

    Public Function GetPeriods() As GetPeriodResponse
        Dim _periodResponse As GetPeriodResponse
        _periodResponse = _dlPCMReportingDataLayer.GetReport()
        Return _periodResponse
    End Function

    Public Function GetReport(_reportRequest As ReportRequest) As ReportResponse
        Dim _reportResponse As ReportResponse
        _reportResponse = _dlPCMReportingDataLayer.GetReport(_reportRequest)
        Return _reportResponse
    End Function

    Public Function GetCardDetails(_giftCardDetailsRequest As GiftCardDetailsRequest) As GiftCardDetailsResponse
        Dim _giftCardDetailsResponse As GiftCardDetailsResponse
        _giftCardDetailsResponse = _dlPCMReportingDataLayer.GetCardDetails(_giftCardDetailsRequest)
        Return _giftCardDetailsResponse
    End Function

    Public Function GetTransactionListDetails(ByVal TransactionListRequest As TransactionListRequest) As TransactionListResponse
        Dim _transactionListResponse As New TransactionListResponse
        _transactionListResponse = _dlPCMReportingDataLayer.GetTransactionListDetails(TransactionListRequest)
        Return _transactionListResponse
    End Function

    Public Function CardIssued(ByVal NewAccountRequest As NewAccountRequest) As CardIssuedResponse
        Dim _cardIssuedResponse As New CardIssuedResponse
        _cardIssuedResponse = _dlPCMReportingDataLayer.CardIssued(NewAccountRequest)
        Return _cardIssuedResponse
    End Function

    Public Function GetQuery(ByVal NewAccountRequest As NewAccountRequest) As GetQueryResponse
        Dim _queryResponse As New GetQueryResponse
        _queryResponse = _dlPCMReportingDataLayer.GetQuery(NewAccountRequest)
        Return _queryResponse
    End Function

    Public Function GetPaymentTransactions(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return _dlPCMReportingDataLayer.GetPaymentTransactions(StartDate, EndDate)

    End Function

    Public Function GetNumbersForSMS(ByVal Type As String, ByVal BranchCode As String) As DataTable

        Return _dlPCMReportingDataLayer.GetNumbersForSMS(Type, BranchCode)

    End Function

End Class
