Imports pcm.DataLayer
Imports Entities

Public Class ContactBusinessLayer
    Private _dlDebtors As New DebtorsDataLayer
    Private _dlContactLog As New ContactDataLayer
    Private _dlLogging As New LoggingDataLayer

    Private Function WRequest(URL As String, method As String, POSTdata As String) As String
        Dim responseData As String = ""
        Try
            Dim hwrequest As Net.HttpWebRequest = Net.WebRequest.Create(URL)
            hwrequest.Accept = "*/*"
            hwrequest.AllowAutoRedirect = True
            hwrequest.UserAgent = "http_requester/0.1"
            hwrequest.Timeout = 60000

            hwrequest.Method = method
            If hwrequest.Method = "POST" Then
                hwrequest.ContentType = "application/x-www-form-urlencoded"
                Dim encoding As New ASCIIEncoding() 'Use UTF8Encoding for XML requests
                Dim postByteArray() As Byte = encoding.GetBytes(POSTdata)
                hwrequest.ContentLength = postByteArray.Length
                Dim postStream As IO.Stream = hwrequest.GetRequestStream()
                postStream.Write(postByteArray, 0, postByteArray.Length)
                postStream.Close()
            End If
            Dim hwresponse As Net.HttpWebResponse = hwrequest.GetResponse()
            If hwresponse.StatusCode = Net.HttpStatusCode.OK Then
                Dim responseStream As IO.StreamReader = _
                  New IO.StreamReader(hwresponse.GetResponseStream())
                responseData = responseStream.ReadToEnd()
            End If
            hwresponse.Close()
        Catch e As Exception
            responseData = "An error occurred: " & e.Message
        End Try
        Return responseData
    End Function

    Public Sub SendSMS(ByVal AccountNumber As String, ByVal ContactNumber As String, _
                       ByVal message As String, ByVal Username As String, Optional ByVal MessageType As String = "")

        _dlDebtors.SendSMS(AccountNumber, ContactNumber, message, Username, MessageType)

    End Sub

    Public Function ContactUpdate(ByVal Username As String, NewLog As CollectionCallResult, ByVal IPAddress As String) As Boolean

        Dim _LogRecord As New CollectionCallLog

        Dim ServedTime As Date = CDate(NewLog.TimeRecordServed)
        Dim TimeNow As Date = Format(Now, "yyyy-MM-dd HH:mm:ss")

        _LogRecord.AccountNumber = NewLog.AccountNumber
        _LogRecord.ActionType = "Call"
        _LogRecord.IPAddress = IPAddress
        _LogRecord.LengthOfAction = DateDiff(DateInterval.Second, ServedTime, TimeNow)
        _LogRecord.PTPAmount = Format(Val(NewLog.PTPAmount), "0.00")
        _LogRecord.UserComment = NewLog.ContactNotes
        _LogRecord.UserName = Username
        _LogRecord.PTPDate = NewLog.PTPDate
        _LogRecord.CollectionsPeriod = NewLog.CollectionsPeriod

        'Can be blank if not updated
        'Update regardless of call result
        If NewLog.PreferredLanguage <> "" Then
            If Not _dlDebtors.UpdateDebtorPreferredLanguage(NewLog.AccountNumber, NewLog.PreferredLanguage) Then
                Return False
            End If

            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Updated Preferred Language", "", NewLog.PreferredLanguage) Then
                Return False
            End If
        End If

        'Incorrect number
        If NewLog.ContactResult = "Number does not exist / Incorrect number" Then
            _LogRecord.ActionResult = NewLog.ContactResult & " Level 0"

            'Update debtor to not send SMS
            If Not _dlDebtors.UpdateDebtorSMSStatus(NewLog.AccountNumber, True) Then
                Return False
            End If

            'Update debtor change log
            '                                                                       012345678901234567890123456789012345678901234567890123456789
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked as Invalid Cellphone Number", "", "true") Then
                Return False
            End If

            'Update debtor, mark as under contact investigation
            If Not _dlDebtors.UpdateDebtorContactInvestigation(NewLog.AccountNumber, True) Then
                Return False
            End If

            'Update debtor change log
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as Contact Investigation", "", "true") Then
                Return False
            End If

            'Insert call record for debtor contact history
            '                      012345678901234567890123456789012345678901234567890123456789
            NewLog.ContactNotes = "Marked Customer As Contact Investigation and Do Not Send SMS - " & NewLog.ContactNotes
            If Not _dlContactLog.InsertContactHistoryRecord(Username, NewLog) Then
                Return False
            End If

            'Insert user record
            If Not _dlLogging.WriteToLog(_LogRecord) Then
                Return False
            End If

            'Set next contact time as null
            If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "0") = 0 Then
                Return False
            End If

            Return True
        End If

        'Under Investigation
        If NewLog.ContactResult = "Under Investigation" Then
            _LogRecord.ActionResult = NewLog.ContactResult & " Level 0"

            'Update debtor mark as under investigation
            If Not _dlDebtors.UpdateDebtorUnderInvestigation(NewLog.AccountNumber, True) Then
                Return False
            End If

            'Update debtor change log
            '                                                                       012345678901234567890123456789012345678901234567890123456789
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as Under Investigation", "", "true") Then
                Return False
            End If

            'Insert call record for debtor contact history
            '                      012345678901234567890123456789012345678901234567890123456789
            NewLog.ContactNotes = "Marked Customer As Under Investigation - " & NewLog.ContactNotes
            If Not _dlContactLog.InsertContactHistoryRecord(Username, NewLog) Then
                Return False
            End If

            'Insert user record
            If Not _dlLogging.WriteToLog(_LogRecord) Then
                Return False
            End If

            'Set next contact time as null
            If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "0") = 0 Then
                Return False
            End If

            Return True
        End If

        'No answer / Engaged / Call Back Later 
        If NewLog.ContactResult = "No answer / Engaged / Call Back Later" Then
            'No answer / Engaged / Call Back Later 
            'Selecting this option will cause the system to serve this customer again in a future random time (26-36 hours)
            Dim message As String
            If NewLog.CollectionsPeriod = "30" Then
                message = "We tried to call to remind you that a payment is due at the end of the month on your Rage account. Pls ignore if you have already made payment."
            Else
                message = "We tried to call to discuss your Rage account which is overdue. Pls call 0116086800 to make a payment arrangement. Pls ignore if you have already made payment."
            End If
            _dlDebtors.SendSMS(NewLog.AccountNumber, NewLog.ContactNumber, message, Username)

                'Just add them to the queue
                NewLog.CurrentContactLevel = 1

                _LogRecord.ActionResult = NewLog.ContactResult & " Level 1"

                Dim randomNumber As Integer = CInt(Math.Floor((13) * Rnd())) + 24

                If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "1", TimeNow.AddHours(randomNumber)) = 0 Then
                    Return False
                End If

                'Insert call record for debtor contact history
                NewLog.ContactNotes = "Next Contact : " & TimeNow.AddHours(randomNumber) & " - " & NewLog.ContactNotes
                If Not _dlContactLog.InsertContactHistoryRecord(Username, NewLog) Then
                    Return False
                End If

                'Insert user record
                If Not _dlLogging.WriteToLog(_LogRecord) Then
                    Return False
                End If

                Return True
            End If

            'Straight to Voice mail
            If NewLog.ContactResult = "Straight to Voice mail" Then
            'Straight to Voice mail
            'Selecting this option will cause the system to serve this customer again in 3 hours -> 24 hours -> 72 hours (“snoozed”) 
            'after which the account will be marked as “Contact Investigation”

            '2015-03-10 Update: Voicemail must be called 3 times

            '2014-03-18 Update: Customers will now only be "snoozed" twice as it is taking too long to get through all the debtors
            'New schedule is 21 hours -> 76 hours

            ''Using PTP as contact level 5
            'If Val(NewLog.CurrentContactLevel) > 3 Then
            '    NewLog.CurrentContactLevel = 0
            'End If

            Dim message As String
            If NewLog.CollectionsPeriod = "30" Then
                message = "We tried to call to remind you that a payment is due at the end of the month on your Rage account. Pls ignore if you have already made payment."
            Else
                message = "We tried to call to discuss your Rage account which is overdue. Pls call 0116086800 to make a payment arrangement. Pls ignore if you have already made payment."
            End If

            _dlDebtors.SendSMS(NewLog.AccountNumber, NewLog.ContactNumber, message, Username)

            If NewLog.CurrentContactLevel = "4" Then
                NewLog.CurrentContactLevel = 0

                _LogRecord.ActionResult = NewLog.ContactResult & " Level 0"

                '2014-07-04: No longer marking as dont_send as we still want to send statements / overdue messages
                'This was the 3rd call with no result
                'Update debtor to not send SMS
                'If Not _dlDebtors.UpdateDebtorSMSStatus(NewLog.AccountNumber, True) Then
                '    Return False
                'End If

                ''Update debtor change log
                ''                                                                       012345678901234567890123456789012345678901234567890123456789
                'If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked as Invalid Cellphone Number", "", "true") Then
                '    Return False
                'End If

                'Update debtor, mark as under contact investigation
                If Not _dlDebtors.UpdateDebtorContactInvestigation(NewLog.AccountNumber, True) Then
                    Return False
                End If

                'Update debtor change log
                If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as Contact Investigation", "", "true") Then
                    Return False
                End If

                'Insert call record for debtor contact history
                NewLog.ContactNotes = "Marked Customer As Contact Investigation and Do Not Send SMS - " & NewLog.ContactNotes
                If Not _dlContactLog.InsertContactHistoryRecord(Username, NewLog) Then
                    Return False
                End If

                'Set next contact time as null
                If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "0", "") = 0 Then
                    Return False
                End If

                'Insert user record
                If Not _dlLogging.WriteToLog(_LogRecord) Then
                    Return False
                End If

                Return True
            End If


            Select Case Val(NewLog.CurrentContactLevel)
                Case 0
                    _LogRecord.ActionResult = NewLog.ContactResult & " Level 2"

                    'Set next contact time
                    If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "2", TimeNow.AddHours(21)) = 0 Then
                        Return False
                    End If

                    'Insert call record for debtor contact history
                    NewLog.ContactNotes = "Next Contact : " & TimeNow.AddHours(21) & " - " & NewLog.ContactNotes
                    If Not _dlContactLog.InsertContactHistoryRecord(Username, NewLog) Then
                        Return False
                    End If

                Case 2
                    _LogRecord.ActionResult = NewLog.ContactResult & " Level 3"

                    If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "3", TimeNow.AddHours(76)) = 0 Then
                        Return False
                    End If

                    'Insert call record for debtor contact history
                    NewLog.ContactNotes = "Next Contact : " & TimeNow.AddHours(76) & " - " & NewLog.ContactNotes
                    If Not _dlContactLog.InsertContactHistoryRecord(Username, NewLog) Then
                        Return False
                    End If
                Case 3
                    _LogRecord.ActionResult = NewLog.ContactResult & " Level 4"

                    If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "4", TimeNow.AddHours(172)) = 0 Then
                        Return False
                    End If

                    'Insert call record for debtor contact history
                    NewLog.ContactNotes = "Next Contact : " & TimeNow.AddHours(172) & " - " & NewLog.ContactNotes
                    If Not _dlContactLog.InsertContactHistoryRecord(Username, NewLog) Then
                        Return False
                    End If
            End Select

            'Insert user record
            If Not _dlLogging.WriteToLog(_LogRecord) Then
                Return False
            End If

            Return True
        End If

        If NewLog.ContactResult = "PTP" Then
            _LogRecord.ActionResult = NewLog.ContactResult & " Level 5"

            'Insert call record for debtor contact history
            NewLog.ContactNotes = NewLog.ContactNotes
            If Not _dlContactLog.InsertContactHistoryRecord(Username, NewLog) Then
                Return False
            End If

            'Insert user record
            If Not _dlLogging.WriteToLog(_LogRecord) Then
                Return False
            End If

            'Need to call customer day after PTP if still in arrears
            Dim PTPDate As Date
            PTPDate = CDate(NewLog.PTPDate)
            'Using PTP as contact level 5
            If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "5", PTPDate.AddDays(1)) = 0 Then
                Return False
            End If

            Return True

        End If

        If NewLog.ContactResult = "Refuses to Pay" Then
            _LogRecord.ActionResult = NewLog.ContactResult & " Level 0"

            'Update debtor mark as under investigation
            If Not _dlDebtors.UpdateDebtorUnderInvestigation(NewLog.AccountNumber, True) Then
                Return False
            End If

            'Update debtor change log
            '                                                                       012345678901234567890123456789012345678901234567890123456789
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as Under Investigation", "", "true") Then
                Return False
            End If

            'Update debtor as legal
            If Not _dlDebtors.UpdateDebtorLegalStatus(NewLog.AccountNumber, True) Then
                Return False
            End If

            'Update debtor change log
            '                                                                       012345678901234567890123456789012345678901234567890123456789
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Customer Refuses to Pay. Marked Account as Legal", "", "true") Then
                Return False
            End If

            'Insert call record
            '                      012345678901234567890123456789012345678901234567890123456789
            NewLog.ContactNotes = "Marked Customer As Legal - " & NewLog.ContactNotes
            If Not _dlContactLog.InsertContactHistoryRecord(Username, NewLog) Then
                Return False
            End If

            'Insert user record
            If Not _dlLogging.WriteToLog(_LogRecord) Then
                Return False
            End If

            'Set next contact time as null
            If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "0", "") = 0 Then
                Return False
            End If

            Return True

        End If

        Return False

    End Function

    Public Function UpdateBureauData(ByVal Username As String, ByVal AccountNumber As String) As Boolean

        If Not _dlDebtors.AddToDebtorChangeLog(Username, AccountNumber, "Retrieved New Bureau Data", "", "") Then
            Return False
        End If

        Return _dlDebtors.UpdateBureauData(AccountNumber)

    End Function

    Public Function ContactInvestigationUpdate(ByVal Username As String, NewLog As ContactInvestigationResult, ByVal IPAddress As String) As Boolean

        Dim _NewUserLog As New CollectionCallLog

        Dim ServedTime As Date = CDate(NewLog.TimeRecordServed)
        Dim TimeNow As Date = Format(Now, "yyyy-MM-dd HH:mm:ss")

        NewLog.ActionType = "CI Call"
        NewLog.IPAddress = IPAddress
        NewLog.LengthOfAction = DateDiff(DateInterval.Second, ServedTime, TimeNow)
        NewLog.UserName = Username

        'Insert changes into change log
        If Trim(NewLog.AltNumber) <> Trim(NewLog.OriginalAltNumber) Then
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Changed Fax Number", NewLog.AltNumber, NewLog.OriginalAltNumber) Then
                Return False
            End If
        End If

        If Trim(NewLog.ContactNumber) <> Trim(NewLog.OriginalContactNumber) Then
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Changed Cellphone Number", NewLog.OriginalContactNumber, NewLog.ContactNumber) Then
                Return False
            End If
        End If

        If Trim(NewLog.HomeNumber1) <> Trim(NewLog.OriginalHomeNumber1) Then
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Changed Home Telephone Number", NewLog.OriginalHomeNumber1, NewLog.HomeNumber1) Then
                Return False
            End If
        End If

        If Trim(NewLog.HomeNumber2) <> Trim(NewLog.OriginalHomeNumber2) Then
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Changed Home Telephone Number 2", NewLog.OriginalHomeNumber2, NewLog.HomeNumber2) Then
                Return False
            End If
        End If

        If Trim(NewLog.NextOfKin) <> Trim(NewLog.OriginalNextOfKin) Then
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Changed Next of Kin", NewLog.OriginalNextOfKin, NewLog.NextOfKin) Then
                Return False
            End If
        End If

        If Trim(NewLog.NextOfKinNumber) <> Trim(NewLog.OriginalNextOfKinNumber) Then
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Changed Next of Kin Contact Number", NewLog.OriginalNextOfKinNumber, NewLog.NextOfKinNumber) Then
                Return False
            End If
        End If

        If Trim(NewLog.SpouseContactNumber) <> Trim(NewLog.OriginalSpouseContactNumber) Then
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Changed Spouse Contact Number", NewLog.OriginalSpouseContactNumber, NewLog.SpouseContactNumber) Then
                Return False
            End If
        End If

        If Trim(NewLog.WorkNumber) <> Trim(NewLog.OriginalWorkNumber) Then
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Changed Work Number", NewLog.OriginalWorkNumber, NewLog.WorkNumber) Then
                Return False
            End If
        End If

        'Can be blank if not updated
        'Update regardless of call result
        If NewLog.PreferredLanguage <> "" Then
            If Not _dlDebtors.UpdateDebtorPreferredLanguage(NewLog.AccountNumber, NewLog.PreferredLanguage) Then
                Return False
            End If

            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Updated Preferred Language", "", NewLog.PreferredLanguage) Then
                Return False
            End If
        End If

        'Update the debtor record with all changes
        _dlDebtors.UpdateDebtorContactNumberStatus(NewLog)

        'Changed to this position on 2014-11-13
        'Insert call record for debtor contact history
        If Not _dlContactLog.InsertContactInvestigationHistoryRecord(Username, NewLog) Then
            Return False
        End If

        'Changed to this position on 2014-11-13
        'Insert user record
        _NewUserLog.AccountNumber = NewLog.AccountNumber
        _NewUserLog.ActionResult = NewLog.ActionResult
        _NewUserLog.ActionType = "CI Call"
        _NewUserLog.CollectionsPeriod = ""
        _NewUserLog.IPAddress = NewLog.IPAddress
        _NewUserLog.LengthOfAction = NewLog.LengthOfAction
        _NewUserLog.PTPAmount = NewLog.PTPAmount
        _NewUserLog.PTPDate = NewLog.PTPDate
        _NewUserLog.UserComment = NewLog.UserComment
        _NewUserLog.UserName = Username

        If Not _dlLogging.WriteToLog(_NewUserLog) Then
            Return False
        End If

        'Incorrect number
        If NewLog.ActionResult = "Unable to obtain valid contact number" Then

            '2014-07-04: No longer marking as dont_send as we still want to try send statements / overdue messages
            ''Update debtor to not send SMS / invalid contact number
            'If Not _dlDebtors.UpdateDebtorSMSStatus(NewLog.AccountNumber, True) Then
            '    Return False
            'End If

            ''Update debtor change log
            'If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked Do Not Send SMS as True", "", "true") Then
            '    Return False
            'End If

            '1.
            'Update debtor mark as no longer under contact investigation
            If Not _dlDebtors.UpdateDebtorContactInvestigation(NewLog.AccountNumber, False) Then
                Return False
            End If

            '2.
            'Update debtor change log
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as no longer under Contact Investigation", "", "true") Then
                Return False
            End If

            '3.
            'Mark customer as Under Investigation so that the account doesn't keep coming up
            'Update debtor mark as under investigation
            If Not _dlDebtors.UpdateDebtorUnderInvestigation(NewLog.AccountNumber, True) Then
                Return False
            End If

            '4.
            'Update debtor change log
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked Under Investigation as True", "", "true") Then
                Return False
            End If

            '5.
            'Set next contact time as null
            If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "0", "") = 0 Then
                Return False
            End If

            Return True
        End If

        'Under Investigation
        If NewLog.ActionResult = "Under Investigation" Then

            '1.
            'Update debtor mark as no longer under contact investigation
            If Not _dlDebtors.UpdateDebtorContactInvestigation(NewLog.AccountNumber, False) Then
                Return False
            End If

            '2.
            'Update debtor change log
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as no longer under Contact Investigation", "", "true") Then
                Return False
            End If

            '3.
            'Update debtor mark as under investigation
            If Not _dlDebtors.UpdateDebtorUnderInvestigation(NewLog.AccountNumber, True) Then
                Return False
            End If

            '4.
            'Update debtor change log
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as Under Investigation", "", "true") Then
                Return False
            End If

            '5.
            'Set next contact time as null
            If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "0", "") = 0 Then
                Return False
            End If

            Return True
        End If

        If NewLog.ActionResult = "PTP" Then

            '1.
            'Update debtor mark as no longer under contact investigation
            If Not _dlDebtors.UpdateDebtorContactInvestigation(NewLog.AccountNumber, False) Then
                Return False
            End If

            '2.
            'Update debtor change log
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as no longer under Contact Investigation", "", "true") Then
                Return False
            End If

            'Need to call customer day after PTP if still in arrears
            Dim PTPDate As Date
            PTPDate = CDate(NewLog.PTPDate)
            'Using PTP as contact level 5
            If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "5", PTPDate.AddDays(1)) = 0 Then
                Return False
            End If

            Return True

        End If

        If NewLog.ActionResult = "Refuses to Pay" Then

            '1.
            'Update debtor mark as no longer under contact investigation
            If Not _dlDebtors.UpdateDebtorContactInvestigation(NewLog.AccountNumber, False) Then
                Return False
            End If

            '2.
            'Update debtor change log
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as no longer under Contact Investigation", "", "true") Then
                Return False
            End If

            'Update debtor to send SMS / valid contact number
            If Not _dlDebtors.UpdateDebtorSMSStatus(NewLog.AccountNumber, False) Then
                Return False
            End If

            'Update debtor change log
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked Do Not Send SMS as False", "", "true") Then
                Return False
            End If

            'Update debtor mark as under investigation
            If Not _dlDebtors.UpdateDebtorUnderInvestigation(NewLog.AccountNumber, True) Then
                Return False
            End If

            'Update debtor change log
            '                                                                       012345678901234567890123456789012345678901234567890123456789
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as Under Investigation", "", "true") Then
                Return False
            End If

            'Update debtor as legal
            If Not _dlDebtors.UpdateDebtorLegalStatus(NewLog.AccountNumber, True) Then
                Return False
            End If

            'Update debtor change log
            '                                                                       012345678901234567890123456789012345678901234567890123456789
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Customer Refuses to Pay. Marked Account as Legal", "", "true") Then
                Return False
            End If

            'Set next contact time as null
            If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "0", "") = 0 Then
                Return False
            End If

            Return True

        End If

        'Other call results
        'Primary Number updated successfully
        'Original Primary Number was correct

        '1.
        'Update debtor mark as no longer under contact investigation
        If Not _dlDebtors.UpdateDebtorContactInvestigation(NewLog.AccountNumber, False) Then
            Return False
        End If

        '2.
        'Update debtor change log
        If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as no longer under Contact Investigation", "", "true") Then
            Return False
        End If

        'Set to send SMS now that number has been confirmed
        'Update debtor to send SMS / valid contact number
        If Not _dlDebtors.UpdateDebtorSMSStatus(NewLog.AccountNumber, False) Then
            Return False
        End If

        'Update debtor change log
        If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked Do Not Send SMS as False", "", "true") Then
            Return False
        End If

        'Set next contact time as null
        If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "0", "") = 0 Then
            Return False
        End If

        Return True

    End Function

    Public Function InvestigationUpdate(ByVal Username As String, NewLog As ContactInvestigationResult, ByVal IPAddress As String) As Boolean

        Dim _NewUserLog As New CollectionCallLog

        Dim ServedTime As Date = CDate(NewLog.TimeRecordServed)
        Dim TimeNow As Date = Format(Now, "yyyy-MM-dd HH:mm:ss")

        NewLog.ActionType = "Inv Call"
        NewLog.IPAddress = IPAddress
        NewLog.LengthOfAction = DateDiff(DateInterval.Second, ServedTime, TimeNow)
        NewLog.UserName = Username

        'Insert changes into change log
        If Trim(NewLog.AltNumber) <> Trim(NewLog.OriginalAltNumber) Then
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Changed Fax Number", NewLog.AltNumber, NewLog.OriginalAltNumber) Then
                Return False
            End If
        End If

        If Trim(NewLog.ContactNumber) <> Trim(NewLog.OriginalContactNumber) Then
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Changed Cellphone Number", NewLog.OriginalContactNumber, NewLog.ContactNumber) Then
                Return False
            End If
        End If

        If Trim(NewLog.HomeNumber1) <> Trim(NewLog.OriginalHomeNumber1) Then
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Changed Home Telephone Number", NewLog.OriginalHomeNumber1, NewLog.HomeNumber1) Then
                Return False
            End If
        End If

        If Trim(NewLog.HomeNumber2) <> Trim(NewLog.OriginalHomeNumber2) Then
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Changed Home Telephone Number 2", NewLog.OriginalHomeNumber2, NewLog.HomeNumber2) Then
                Return False
            End If
        End If

        If Trim(NewLog.NextOfKin) <> Trim(NewLog.OriginalNextOfKin) Then
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Changed Next of Kin", NewLog.OriginalNextOfKin, NewLog.NextOfKin) Then
                Return False
            End If
        End If

        If Trim(NewLog.NextOfKinNumber) <> Trim(NewLog.OriginalNextOfKinNumber) Then
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Changed Next of Kin Contact Number", NewLog.OriginalNextOfKinNumber, NewLog.NextOfKinNumber) Then
                Return False
            End If
        End If

        If Trim(NewLog.SpouseContactNumber) <> Trim(NewLog.OriginalSpouseContactNumber) Then
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Changed Spouse Contact Number", NewLog.OriginalSpouseContactNumber, NewLog.SpouseContactNumber) Then
                Return False
            End If
        End If

        If Trim(NewLog.WorkNumber) <> Trim(NewLog.OriginalWorkNumber) Then
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Changed Work Number", NewLog.OriginalWorkNumber, NewLog.WorkNumber) Then
                Return False
            End If
        End If

        'Can be blank if not updated
        'Update regardless of call result
        If NewLog.PreferredLanguage <> "" Then
            If Not _dlDebtors.UpdateDebtorPreferredLanguage(NewLog.AccountNumber, NewLog.PreferredLanguage) Then
                Return False
            End If

            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Updated Preferred Language", "", NewLog.PreferredLanguage) Then
                Return False
            End If
        End If

        'Update the debtor record with all changes
        _dlDebtors.UpdateDebtorContactNumberStatus(NewLog)

        'Incorrect number
        If NewLog.ActionResult = "Unable to obtain valid contact number" Then

            'Update debtor mark as no longer under contact investigation
            If Not _dlDebtors.UpdateDebtorContactInvestigation(NewLog.AccountNumber, False) Then
                Return False
            End If

            'Update debtor change log
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as Not under Contact Investigation", "", "true") Then
                Return False
            End If

            'Update debtor to send SMS / valid contact number
            If Not _dlDebtors.UpdateDebtorSMSStatus(NewLog.AccountNumber, True) Then
                Return False
            End If

            'Update debtor change log
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked Do Not Send SMS as True", "", "true") Then
                Return False
            End If

            'Update debtor mark as not under investigation
            If Not _dlDebtors.UpdateDebtorUnderInvestigation(NewLog.AccountNumber, False) Then
                Return False
            End If

            'Update debtor change log
            '                                                                       012345678901234567890123456789012345678901234567890123456789
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as Not Under Investigation", "", "true") Then
                Return False
            End If

            'Update debtor as legal
            If Not _dlDebtors.UpdateDebtorLegalStatus(NewLog.AccountNumber, True) Then
                Return False
            End If

            'Update debtor change log
            '                                                                       012345678901234567890123456789012345678901234567890123456789
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Customer Absconded. Marked Account as Legal", "", "true") Then
                Return False
            End If


            'Insert call record for debtor contact history
            '                      012345678901234567890123456789012345678901234567890123456789
            If Not _dlContactLog.InsertInvestigationHistoryRecord(Username, NewLog) Then
                Return False
            End If

            'Insert user record
            _NewUserLog.AccountNumber = NewLog.AccountNumber
            _NewUserLog.ActionResult = NewLog.ActionResult
            _NewUserLog.ActionType = "Inv Call"
            _NewUserLog.CollectionsPeriod = ""
            _NewUserLog.IPAddress = NewLog.IPAddress
            _NewUserLog.LengthOfAction = NewLog.LengthOfAction
            _NewUserLog.PTPAmount = 0
            _NewUserLog.PTPDate = ""
            _NewUserLog.UserComment = NewLog.UserComment
            _NewUserLog.UserName = Username

            If Not _dlLogging.WriteToLog(_NewUserLog) Then
                Return False
            End If

            'Set next contact time as null
            If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "6", "") = 0 Then
                Return False
            End If

            Return True
        End If

        If NewLog.ActionResult = "PTP" Then

            'Update debtor mark as no longer under contact investigation
            If Not _dlDebtors.UpdateDebtorContactInvestigation(NewLog.AccountNumber, False) Then
                Return False
            End If

            'Update debtor change log
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as no longer under Contact Investigation", "", "true") Then
                Return False
            End If

            'Update debtor to send SMS / valid contact number
            If Not _dlDebtors.UpdateDebtorSMSStatus(NewLog.AccountNumber, False) Then
                Return False
            End If

            'Update debtor change log
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked Do Not Send SMS as False", "", "true") Then
                Return False
            End If

            'Update debtor mark as not under investigation
            If Not _dlDebtors.UpdateDebtorUnderInvestigation(NewLog.AccountNumber, False) Then
                Return False
            End If

            'Update debtor change log
            '                                                                       012345678901234567890123456789012345678901234567890123456789
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as Not Under Investigation", "", "true") Then
                Return False
            End If

            'Insert call record for debtor contact history
            '                      012345678901234567890123456789012345678901234567890123456789
            If Not _dlContactLog.InsertInvestigationHistoryRecord(Username, NewLog) Then
                Return False
            End If

            'Insert user record
            _NewUserLog.AccountNumber = NewLog.AccountNumber
            _NewUserLog.ActionResult = NewLog.ActionResult
            _NewUserLog.ActionType = "Inv Call"
            _NewUserLog.CollectionsPeriod = ""
            _NewUserLog.IPAddress = NewLog.IPAddress
            _NewUserLog.LengthOfAction = NewLog.LengthOfAction
            _NewUserLog.PTPAmount = 0
            _NewUserLog.PTPDate = ""
            _NewUserLog.UserComment = NewLog.UserComment
            _NewUserLog.UserName = Username

            If Not _dlLogging.WriteToLog(_NewUserLog) Then
                Return False
            End If

            'Need to call customer day after PTP if still in arrears
            Dim PTPDate As Date
            PTPDate = CDate(NewLog.PTPDate)
            'Using PTP as contact level 5
            If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "5", PTPDate.AddDays(1)) = 0 Then
                Return False
            End If

            Return True

        End If

        If NewLog.ActionResult = "Refuses to Pay" Then

            'Update debtor mark as no longer under contact investigation
            If Not _dlDebtors.UpdateDebtorContactInvestigation(NewLog.AccountNumber, False) Then
                Return False
            End If

            'Update debtor change log
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as Not under Contact Investigation", "", "false") Then
                Return False
            End If

            'Update debtor to send SMS / valid contact number
            'Might as well send them statements
            If Not _dlDebtors.UpdateDebtorSMSStatus(NewLog.AccountNumber, False) Then
                Return False
            End If

            'Update debtor change log
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked Do Not Send SMS as False", "", "true") Then
                Return False
            End If

            'Update debtor mark as not under investigation
            If Not _dlDebtors.UpdateDebtorUnderInvestigation(NewLog.AccountNumber, False) Then
                Return False
            End If

            'Update debtor change log
            '                                                                       012345678901234567890123456789012345678901234567890123456789
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as Not Under Investigation", "", "false") Then
                Return False
            End If

            'Update debtor as legal
            If Not _dlDebtors.UpdateDebtorLegalStatus(NewLog.AccountNumber, True) Then
                Return False
            End If

            'Update debtor change log
            '                                                                       012345678901234567890123456789012345678901234567890123456789
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Customer Refuses to Pay. Marked Account as Legal", "", "true") Then
                Return False
            End If

            'Insert call record for debtor contact history
            '                      012345678901234567890123456789012345678901234567890123456789
            If Not _dlContactLog.InsertInvestigationHistoryRecord(Username, NewLog) Then
                Return False
            End If

            'Insert user record
            _NewUserLog.AccountNumber = NewLog.AccountNumber
            _NewUserLog.ActionResult = NewLog.ActionResult
            _NewUserLog.ActionType = "Inv Call"
            _NewUserLog.CollectionsPeriod = ""
            _NewUserLog.IPAddress = NewLog.IPAddress
            _NewUserLog.LengthOfAction = NewLog.LengthOfAction
            _NewUserLog.PTPAmount = 0
            _NewUserLog.PTPDate = ""
            _NewUserLog.UserComment = NewLog.UserComment
            _NewUserLog.UserName = Username

            If Not _dlLogging.WriteToLog(_NewUserLog) Then
                Return False
            End If

            'Level 6 = do not contact again
            If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "6", "") = 0 Then
                Return False
            End If

            Return True

        End If

        If NewLog.ActionResult = "Debt Review" Then

            'Update debtor mark as no longer under contact investigation
            If Not _dlDebtors.UpdateDebtorContactInvestigation(NewLog.AccountNumber, False) Then
                Return False
            End If

            'Update debtor change log
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as no longer under Contact Investigation", "", "false") Then
                Return False
            End If

            'Update debtor to send SMS / valid contact number
            'No point sending them statement as they have their own payment plans
            If Not _dlDebtors.UpdateDebtorSMSStatus(NewLog.AccountNumber, True) Then
                Return False
            End If

            'Update debtor change log
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked Do Not Send SMS as True", "", "true") Then
                Return False
            End If

            'Update debtor mark as under investigation = false
            If Not _dlDebtors.UpdateDebtorUnderInvestigation(NewLog.AccountNumber, False) Then
                Return False
            End If

            'Update debtor change log
            '                                                                       012345678901234567890123456789012345678901234567890123456789
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as Not Under Investigation", "", "false") Then
                Return False
            End If

            'Update debtor as legal = false
            If Not _dlDebtors.UpdateDebtorLegalStatus(NewLog.AccountNumber, False) Then
                Return False
            End If

            'Update debtor status to DEBT REVIEW
            If Not _dlDebtors.UpdateDebtorDebtReviewStatus(NewLog.AccountNumber) Then
                Return False
            End If

            'Update debtor change log
            '                                                                       012345678901234567890123456789012345678901234567890123456789
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Customer under Debt Review. Marked Account as Not Legal", "", "true") Then
                Return False
            End If

            'Insert call record for debtor contact history
            '                      012345678901234567890123456789012345678901234567890123456789
            If Not _dlContactLog.InsertInvestigationHistoryRecord(Username, NewLog) Then
                Return False
            End If

            'Insert user record
            _NewUserLog.AccountNumber = NewLog.AccountNumber
            _NewUserLog.ActionResult = NewLog.ActionResult
            _NewUserLog.ActionType = "Inv Call"
            _NewUserLog.CollectionsPeriod = ""
            _NewUserLog.IPAddress = NewLog.IPAddress
            _NewUserLog.LengthOfAction = NewLog.LengthOfAction
            _NewUserLog.PTPAmount = 0
            _NewUserLog.PTPDate = ""
            _NewUserLog.UserComment = NewLog.UserComment
            _NewUserLog.UserName = Username

            If Not _dlLogging.WriteToLog(_NewUserLog) Then
                Return False
            End If

            'Level 6 = do not contact again
            If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "6", "") = 0 Then
                Return False
            End If

            Return True

        End If

        If NewLog.ActionResult = "Customer Absconded" Then
            'No need to keep showing this account over and over

            'Update debtor mark as no longer under contact investigation
            If Not _dlDebtors.UpdateDebtorContactInvestigation(NewLog.AccountNumber, False) Then
                Return False
            End If

            'Update debtor change log
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as Not under Contact Investigation", "", "false") Then
                Return False
            End If

            'Update debtor to send SMS / valid contact number
            If Not _dlDebtors.UpdateDebtorSMSStatus(NewLog.AccountNumber, True) Then
                Return False
            End If

            'Update debtor change log
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked Do Not Send SMS", "", "true") Then
                Return False
            End If

            'Update debtor mark as under investigation = false
            If Not _dlDebtors.UpdateDebtorUnderInvestigation(NewLog.AccountNumber, False) Then
                Return False
            End If

            'Update debtor change log
            '                                                                       012345678901234567890123456789012345678901234567890123456789
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as Not Under Investigation", "", "false") Then
                Return False
            End If

            'Update debtor as legal = True
            If Not _dlDebtors.UpdateDebtorLegalStatus(NewLog.AccountNumber, True) Then
                Return False
            End If

            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Customer Absconded. Set account status to LEGAL", "", "LEGAL") Then
                Return False
            End If

            'Insert call record for debtor contact history
            '                      012345678901234567890123456789012345678901234567890123456789
            If Not _dlContactLog.InsertInvestigationHistoryRecord(Username, NewLog) Then
                Return False
            End If

            'Insert user record
            _NewUserLog.AccountNumber = NewLog.AccountNumber
            _NewUserLog.ActionResult = NewLog.ActionResult
            _NewUserLog.ActionType = "Inv Call"
            _NewUserLog.CollectionsPeriod = ""
            _NewUserLog.IPAddress = NewLog.IPAddress
            _NewUserLog.LengthOfAction = NewLog.LengthOfAction
            _NewUserLog.PTPAmount = 0
            _NewUserLog.PTPDate = ""
            _NewUserLog.UserComment = NewLog.UserComment
            _NewUserLog.UserName = Username

            If Not _dlLogging.WriteToLog(_NewUserLog) Then
                Return False
            End If

            'Level 6 = don't contact
            If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "6", "") = 0 Then
                Return False
            End If

            Return True

        End If

        If NewLog.ActionResult = "Fraud" Then
            'Update debtor mark as no longer under contact investigation
            If Not _dlDebtors.UpdateDebtorContactInvestigation(NewLog.AccountNumber, False) Then
                Return False
            End If

            'Update debtor change log
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as Not under Contact Investigation", "", "false") Then
                Return False
            End If

            'Update debtor to send SMS / valid contact number
            If Not _dlDebtors.UpdateDebtorSMSStatus(NewLog.AccountNumber, True) Then
                Return False
            End If

            'Update debtor change log
            'No point sending SMS to the person who was defrauded
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked Do Not Send SMS True", "", "true") Then
                Return False
            End If

            'Update debtor mark as under investigation = false
            If Not _dlDebtors.UpdateDebtorUnderInvestigation(NewLog.AccountNumber, False) Then
                Return False
            End If

            'Update debtor change log
            '                                                                       012345678901234567890123456789012345678901234567890123456789
            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as Not Under Investigation", "", "false") Then
                Return False
            End If

            'Update debtor as fraud = true
            If Not _dlDebtors.UpdateDebtorFraudStatus(NewLog.AccountNumber, True) Then
                Return False
            End If

            If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as FRAUD", "", "ACTIVE") Then
                Return False
            End If

            'Insert call record for debtor contact history
            '                      012345678901234567890123456789012345678901234567890123456789
            If Not _dlContactLog.InsertInvestigationHistoryRecord(Username, NewLog) Then
                Return False
            End If

            'Insert user record
            _NewUserLog.AccountNumber = NewLog.AccountNumber
            _NewUserLog.ActionResult = NewLog.ActionResult
            _NewUserLog.ActionType = "Inv Call"
            _NewUserLog.CollectionsPeriod = ""
            _NewUserLog.IPAddress = NewLog.IPAddress
            _NewUserLog.LengthOfAction = NewLog.LengthOfAction
            _NewUserLog.PTPAmount = 0
            _NewUserLog.PTPDate = ""
            _NewUserLog.UserComment = NewLog.UserComment
            _NewUserLog.UserName = Username

            If Not _dlLogging.WriteToLog(_NewUserLog) Then
                Return False
            End If

            'Level 6 = don't contact
            If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "6", "") = 0 Then
                Return False
            End If

            Return True

        End If

        '///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        'Other call results (Primary Number updated successfully / Original Primary Number was correct / Issue Resolved)
        '///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        'Update debtor mark as under investigation = false
        If Not _dlDebtors.UpdateDebtorUnderInvestigation(NewLog.AccountNumber, False) Then
            Return False
        End If

        'Update debtor change log
        '                                                                       012345678901234567890123456789012345678901234567890123456789
        If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as Not Under Investigation", "", "false") Then
            Return False
        End If

        'Update debtor mark as no longer under contact investigation
        If Not _dlDebtors.UpdateDebtorContactInvestigation(NewLog.AccountNumber, False) Then
            Return False
        End If

        'Update debtor change log
        If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as no longer under Contact Investigation", "", "false") Then
            Return False
        End If

        'Update debtor to send SMS / valid contact number
        If Not _dlDebtors.UpdateDebtorSMSStatus(NewLog.AccountNumber, False) Then
            Return False
        End If

        'Update debtor change log
        If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked Do Not Send SMS as False", "", "false") Then
            Return False
        End If

        'Update debtor as legal = false
        If Not _dlDebtors.UpdateDebtorLegalStatus(NewLog.AccountNumber, False) Then
            Return False
        End If

        If Not _dlDebtors.AddToDebtorChangeLog(Username, NewLog.AccountNumber, "Marked account as Not LEGAL", "", "ACTIVE") Then
            Return False
        End If

        'Insert call record for debtor contact history
        If Not _dlContactLog.InsertInvestigationHistoryRecord(Username, NewLog) Then
            Return False
        End If

        'Insert user record

        _NewUserLog.AccountNumber = NewLog.AccountNumber
        _NewUserLog.ActionResult = NewLog.ActionResult
        _NewUserLog.ActionType = "Inv Call"
        _NewUserLog.CollectionsPeriod = ""
        _NewUserLog.IPAddress = NewLog.IPAddress
        _NewUserLog.LengthOfAction = NewLog.LengthOfAction
        _NewUserLog.PTPAmount = 0
        _NewUserLog.PTPDate = ""
        _NewUserLog.UserComment = NewLog.UserComment
        _NewUserLog.UserName = Username

        If Not _dlLogging.WriteToLog(_NewUserLog) Then
            Return False
        End If

        'Set next contact time as null
        If _dlDebtors.SetNextContactDateTime(NewLog.AccountNumber, "0", "") = 0 Then
            Return False
        End If

        Return True

    End Function

End Class
