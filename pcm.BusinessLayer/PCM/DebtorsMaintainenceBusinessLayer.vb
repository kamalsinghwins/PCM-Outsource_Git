﻿Imports Entities
Imports pcm.DataLayer

Public Class DebtorsMaintainenceBusinessLayer
    Private _dlDebtorsMaintainence As New DebtorsMaintainenceDataLayer
    Private _baseResponse As New BaseResponse

    Public Function SavePersonalDetails(_DebtorsMaintainenceRequest As DebtorsMaintainenceRequest,
                                        ByVal UserName As String) As BaseResponse


        'Save Debtor Personal Details
        _baseResponse = _dlDebtorsMaintainence.SavePersonalDetails(_DebtorsMaintainenceRequest, UserName)

        Return _baseResponse
    End Function

    Public Function GetBranchList(Optional ByVal BranchCode As String = "") As DataSet
        Dim _branchListDS As DataSet

        'Get Branch List
        _branchListDS = _dlDebtorsMaintainence.GetBranchList(BranchCode)
        If _branchListDS Is Nothing Then
            Return Nothing
        End If
        Return _branchListDS
    End Function


    Public Function ValidID(IdNumber As String) As Boolean
        Dim ValidIDResponse As Boolean

        'Save Debtor Personal Details
        ValidIDResponse = _dlDebtorsMaintainence.ValidID(IdNumber)

        Return ValidIDResponse
    End Function

    Public Function GetDebtors(Field As String, Criteria As String, IsRageEmployeesOnly As Boolean, ByVal IsLast100 As Boolean) As DataTable
        Dim GetDebtorsResponse As DataTable

        GetDebtorsResponse = _dlDebtorsMaintainence.GetDebtors(Field, Criteria, IsRageEmployeesOnly, IsLast100)

        Return GetDebtorsResponse
    End Function

    Public Function GetSelectedDebtorsDetails(strAccNum As String) As DebtorDetailsResponse
        Dim _debtorDetailsResponse As New DebtorDetailsResponse

        _debtorDetailsResponse = _dlDebtorsMaintainence.GetSelectedDebtorsDetails(strAccNum)

        Return _debtorDetailsResponse
    End Function

    Public Function GetReportingList(AccountNumber As String) As DataSet
        Dim _ReportingListDS As DataSet

        _ReportingListDS = _dlDebtorsMaintainence.GetReportingList(AccountNumber)

        If _ReportingListDS Is Nothing Then
            Return Nothing
        End If

        Return _ReportingListDS
    End Function

    Public Function GetDebtorXmlData(AccountNumber As String) As String
        Dim XmlData As String = _dlDebtorsMaintainence.GetDebtorXmlData(AccountNumber)
        Return XmlData
    End Function


    Public Function GetDebtorsReportingData(ByVal AccountNumber As String) As DebtorsReporting

        Dim _NewDebtorData As DebtorsReporting

        _NewDebtorData = _dlDebtorsMaintainence.GetDebtorsReportingData(AccountNumber)

        If _NewDebtorData Is Nothing Then
            Return Nothing
        End If

        Return _NewDebtorData

    End Function


End Class
