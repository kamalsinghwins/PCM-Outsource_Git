Imports Entities

Public Class ContactDataLayer

    Dim objDB As New dlNpgSQL("PostgreConnectionStringPCMWrite")
    Dim ds As DataSet
    Dim tmpSQL As String
    Dim RG As New Utilities.clsUtil

    Public Function InsertContactHistoryRecord(ByRef UserName As String, ByVal NewLogRecord As CollectionCallResult) As Boolean

        If NewLogRecord.PTPDate = "" Then
            tmpSQL = "INSERT INTO debtor_contact_history (guid,timestamp_of_contact,account_number,type_of_contact,result_of_action,ptp_amount," & _
                 "action_notes,username) VALUES (" & _
                 "'" & Guid.NewGuid.ToString & "',to_timestamp('" & Format(Now, "yyyy-MM-dd hh:mm:ss tt") & "','YYYY-MM-DD HH:MI:SS AM'),'" & NewLogRecord.AccountNumber & "'," & _
                 "'Collections','" & NewLogRecord.ContactResult & "','" & RG.Num(NewLogRecord.PTPAmount) & "','" & Mid$(RG.Apos(NewLogRecord.ContactNotes), 1, 495) & "'," & _
                 "'" & UserName & "')"
        Else
            tmpSQL = "INSERT INTO debtor_contact_history (guid,timestamp_of_contact,account_number,type_of_contact,result_of_action,ptp_amount," & _
                 "action_notes,username,ptp_date) VALUES (" & _
                 "'" & Guid.NewGuid.ToString & "',to_timestamp('" & Format(Now, "yyyy-MM-dd hh:mm:ss tt") & "','YYYY-MM-DD HH:MI:SS AM'),'" & NewLogRecord.AccountNumber & "'," & _
                 "'Collections','" & NewLogRecord.ContactResult & "','" & RG.Num(NewLogRecord.PTPAmount) & "','" & Mid$(RG.Apos(NewLogRecord.ContactNotes), 1, 495) & "'," & _
                 "'" & UserName & "','" & NewLogRecord.PTPDate & "')"
        End If


        Try
            objDB.ExecuteQuery(tmpSQL)

        Catch ex As Exception
            Return False

            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try

        Return True


    End Function

    Public Function InsertContactInvestigationHistoryRecord(ByRef UserName As String, ByVal NewLogRecord As ContactInvestigationResult) As Boolean

        tmpSQL = "INSERT INTO debtor_contact_history (guid,timestamp_of_contact,account_number,type_of_contact,result_of_action," & _
                 "action_notes,username) VALUES (" & _
                 "'" & Guid.NewGuid.ToString & "',to_timestamp('" & Format(Now, "yyyy-MM-dd hh:mm:ss tt") & "','YYYY-MM-DD HH:MI:SS AM'),'" & NewLogRecord.AccountNumber & "'," & _
                 "'CI Call','" & NewLogRecord.ActionResult & "','" & Mid$(RG.Apos(NewLogRecord.UserComment), 1, 495) & "'," & _
                 "'" & UserName & "')"

        Try
            objDB.ExecuteQuery(tmpSQL)

        Catch ex As Exception
            Return False

            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try

        Return True


    End Function

    Public Function InsertInvestigationHistoryRecord(ByRef UserName As String, ByVal NewLogRecord As ContactInvestigationResult) As Boolean

        tmpSQL = "INSERT INTO debtor_contact_history (guid,timestamp_of_contact,account_number,type_of_contact,result_of_action," & _
                 "action_notes,username) VALUES (" & _
                 "'" & Guid.NewGuid.ToString & "',to_timestamp('" & Format(Now, "yyyy-MM-dd hh:mm:ss tt") & "','YYYY-MM-DD HH:MI:SS AM'),'" & NewLogRecord.AccountNumber & "'," & _
                 "'INV Call','" & NewLogRecord.ActionResult & "','" & Mid$(RG.Apos(NewLogRecord.UserComment), 1, 495) & "'," & _
                 "'" & UserName & "')"

        Try
            objDB.ExecuteQuery(tmpSQL)

        Catch ex As Exception
            Return False

            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        Finally
            If (objDB IsNot Nothing) Then
                objDB.CloseConnection()
            End If
        End Try

        Return True


    End Function
End Class
