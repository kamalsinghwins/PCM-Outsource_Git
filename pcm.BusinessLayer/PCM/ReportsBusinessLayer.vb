Imports pcm.DataLayer
Imports Entities


Public Class ReportsBusinessLayer
    Dim _DLayer As ReportsDataLayer
    Dim NewReport As New DataTable
   
    Public Sub New(ByVal CompanyCode As String)
        _DLayer = New ReportsDataLayer(CompanyCode)
    End Sub

    Public Sub New()
        _DLayer = New ReportsDataLayer()
    End Sub

    Public Function ReturnWebLog(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        NewReport = _DLayer.ReturnBureauLog(StartDate, EndDate)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function GetSalesPayments(ByVal OpenedFrom As String, ByVal OpenedTo As String,
                                     ByVal SalesFrom As String, ByVal SalesTo As String,
                                     ByVal PaymentsFrom As String, ByVal PaymentsTo As String) As DataTable

        NewReport = _DLayer.GetSalesPayments(OpenedFrom, OpenedTo, SalesFrom, SalesTo, PaymentsFrom, PaymentsTo)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If
    End Function

    Public Function ReturnAgingSummary(ByVal MinimumAmount As String) As DataTable

        NewReport = _DLayer.GetAgingSummary(MinimumAmount)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function GetInternalPeriods() As DataTable

        NewReport = _DLayer.GetInternalPeriods()

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function GetBadDebtData(ByVal SelectedPeriod As String) As DataTable

        NewReport = _DLayer.GetBadDebtData(SelectedPeriod)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function GetBadDebtRecoveredData(ByVal SelectedPeriod As String) As DataTable

        NewReport = _DLayer.GetBadDebtRecoveredData(SelectedPeriod)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function GetReductionIn150(ByVal SelectedPeriod As String) As DataTable

        NewReport = _DLayer.GetReductionIn150(SelectedPeriod)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function


    Public Function BadDebtByStoreTransactions(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        NewReport = _DLayer.BadDebtByStoreTransactions(StartDate, EndDate)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function BadDebtByStoreMaster(ByVal StartDate As String, ByVal EndDate As String,
                                     ByVal Period As String, ByVal BadDebtAmount As Double) As DataTable

        NewReport = _DLayer.BadDebtByStoreMaster(StartDate, EndDate, Period, BadDebtAmount)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function


    Public Function GetAccountsToBeCalled(ByVal MinimumAmount As String) As DataTable

        NewReport = _DLayer.GetAccountsToBeCalled(MinimumAmount)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function


    Public Function ReturnIncomingSMS(ByVal StartDate As String, ByVal ToDate As String) As DataTable

        NewReport = _DLayer.GetIncomingSMS(StartDate, ToDate)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function ReturnLastUpdateData() As DataTable

        NewReport = _DLayer.GetLastUpdateData()

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function ReturnAdminReportData(ByVal Status As String, ByVal PTPDate As String) As DataTable

        If Status = "PTP" Then
            NewReport = _DLayer.ReturnPTPListing(PTPDate)
        Else
            NewReport = _DLayer.ReturnInvestigationListing(Status)
        End If

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function ReturnSMSReportData(ByVal SMSType As String, ByVal SendDate As String) As DataTable

        NewReport = _DLayer.ReturnSMSReportData(SMSType, SendDate)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function ReturnPaymentsVSContactsReportData(ByVal FromDate As String, ByVal ToDate As String) As DataTable

        NewReport = _DLayer.ReturnPaymentsVSContactsReportData(FromDate, ToDate)

        If IsNothing(NewReport) Then
            Return Nothing
        Else
            Return NewReport
        End If

    End Function

    Public Function GetLimitIncreaseReport(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return _DLayer.GetLimitIncreaseReport(StartDate, EndDate)

    End Function
    Public Function GetErrors(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return _DLayer.GetErrors(StartDate, EndDate)

    End Function

    Public Function GetCurrentTurnover(ByVal OrderBy As String) As CurrentTurnoverResponse
        Return _DLayer.GetCurrentTurnover(OrderBy)
    End Function

    Public Function GenerateData(ByVal OrderBy As String, ByVal StartDate As String, ByVal EndDate As String) As CurrentTurnoverResponse
        Return _DLayer.GenerateData(OrderBy, StartDate, EndDate)
    End Function

    Public Function GetTransactions(ByVal StartDate As String, ByVal EndDate As String) As GetTransactionsResponse
        Return _DLayer.GetTransactions(StartDate, EndDate)
    End Function
    Public Function GetBranchDetails() As DataTable
        Return _DLayer.GetBranchDetails()
    End Function

    Public Function GetSegments(StartDate As String, EndDate As String, DateRange As String, Segment As String, AllBranches As Boolean, Optional ByVal strBranch As String = "") As GetSegmentsResponse
        Return _DLayer.GetSegments(StartDate, EndDate, DateRange, Segment, AllBranches, strBranch)
    End Function

    Public Sub InsertUserRecord(ByVal Username As String, ByVal isManager As String,
                                ByVal IPAddress As String, ByVal TypeOfAction As String,
                                ByVal ReportName As String, ByVal ReportOptions As String)
        _DLayer.InsertUserRecord(Username, isManager, IPAddress, TypeOfAction, ReportName, ReportOptions)
    End Sub

    Public Function GetSentEmployeeDetails() As DataTable

        Return _DLayer.GetSentEmployeeDetails()

    End Function

    Public Function GetSentSMSLog(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Return _DLayer.GetSentSMSLog(StartDate, EndDate)

    End Function

    Public Function GetBestSellersReport(ByVal StartDate As String, ByVal EndDate As String, ByVal Trim As String) As DataSet

        Return _DLayer.GetBestSellersReport(StartDate, EndDate, Trim)

    End Function

    Public Function GetCashTransactions(ByVal StartDate As String, ByVal EndDate As String) As GetTransactionsResponse
        Return _DLayer.GetCashTransactions(StartDate, EndDate)
    End Function

    Public Function GetAccountSales(StartDate As String, EndDate As String) As GetAccountSalesResponse
        Return _DLayer.GetAccountSales(StartDate, EndDate)
    End Function

    Public Function GetMasterEmployeesPerStore(Permanent As Boolean, Casual As Boolean, All As Boolean, ByVal StartDate As String, ByVal EndDate As String) As DataSet
        Return _DLayer.GetMasterEmployeesPerStore(Permanent, Casual, All, StartDate, EndDate)

    End Function

    Public Function GetClockingEmployees(ByVal EmployeeNumber As String) As DataTable
        Return _DLayer.GetClockingEmployees(EmployeeNumber)
    End Function

    Public Function GetReports_Test() As DataSet
        Return _DLayer.GetReports_Test()
    End Function
End Class

