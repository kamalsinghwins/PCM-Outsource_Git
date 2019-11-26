Imports Npgsql
Imports Entities
Imports System.Globalization

Public Class ReportsDataLayer

    Inherits DataAccessLayerBase

    Dim pos_connection As Npgsql.NpgsqlConnection = Nothing
    Dim pcm_connection As Npgsql.NpgsqlConnection = Nothing

    Dim objDBWritePCM As dlNpgSQL
    Dim objDBReadPCM As dlNpgSQL
    Dim objDBReadPOS As dlNpgSQL

    Dim where As String
    Private currentTurnoverResponse As New CurrentTurnoverResponse
    Private getSegmentsResponse As New GetSegmentsResponse
    Private getTransactionsResponse As New GetTransactionsResponse
    Private getAccountSalesResponse As New GetAccountSalesResponse


    Public Sub New(Optional ByVal CompanyCode As String = "")

        pcm_connection = Me.DataBase("PostgreConnectionStringPCMRead")
        pos_connection = Me.DataBase("PostgreConnectionStringPositiveRead")

        objDBWritePCM = New dlNpgSQL("PostgreConnectionStringPCMWrite")
        objDBReadPCM = New dlNpgSQL("PostgreConnectionStringPCMRead")
        objDBReadPOS = New dlNpgSQL("PostgreConnectionStringPositiveRead")

    End Sub

    Public Sub New()
        pcm_connection = Me.DataBase("PostgreConnectionStringPCMRead")

    End Sub


    Public Function BadDebtByStoreMaster(ByVal StartDate As String, ByVal EndDate As String,
                                         ByVal Period As String, ByVal BadDebtAmount As Double) As DataTable

        Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xDataPos As New DataTable

        Dim OneYearAgo As String
        OneYearAgo = Format(Now.Date.AddMonths(-12), "yyyy-MM-dd")

        Try

            Dim command As New NpgsqlCommand()
            connection = Me.DataBase("PostgreConnectionStringPositiveRead")
            connection.Open()
            command.Connection = connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT branch_code,branch_name FROM branch_details WHERE inserted >= '" & OneYearAgo & "';"

            Dim reader As New NpgsqlDataAdapter(strQuery, connection)
            reader.Fill(xDataPos)

        Catch ex As Exception
            Throw ex
        Finally
            If (connection IsNot Nothing) Then
                connection.Close()
            End If

        End Try

        If xDataPos.Rows.Count > 0 Then
            For i As Integer = 0 To xDataPos.Rows.Count - 1

                tmpSQL = "SELECT branch_code FROM branch_details WHERE branch_code = '" & xDataPos.Rows(i)("branch_code") & "'"

                Try
                    Ds = objDBReadPCM.GetDataSet(tmpSQL)
                    If Not objDBReadPCM.isR(Ds) Then
                        tmpSQL = "INSERT INTO branch_details (branch_code,branch_name) VALUES ('" & xDataPos.Rows(i)("branch_code") & "','" & RG.Apos(xDataPos.Rows(i)("branch_name")) & "')"
                        Try
                            objDBWritePCM.ExecuteQuery(tmpSQL)
                        Catch ex As Exception
                            objDBWritePCM.CloseConnection()
                            Return Nothing
                        End Try
                    End If
                Catch ex As Exception
                    If (objDBReadPCM IsNot Nothing) Then
                        objDBReadPCM.CloseConnection()
                    End If
                    Return Nothing
                End Try
            Next
        End If

        If (objDBReadPOS IsNot Nothing) Then
            objDBReadPOS.CloseConnection()
        End If

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT " &
                                     "debtor_first_purchase.first_purchase AS branch_code," &
                                     "SUM(financial_balances.total_spent) AS total_spent," &
                                     "COALESCE(Sum(case when p" & Period & " > " & BadDebtAmount & " THEN total end),0) AS bad_debt," &
                                     "COUNT(financial_balances.account_number) AS total_opened," &
                                     "COUNT(case when p" & Period & " > " & BadDebtAmount & " THEN financial_balances.account_number end) AS total_bad_accounts," &
                                     "((COUNT(case when p" & Period & " > " & BadDebtAmount & " THEN financial_balances.account_number end)::numeric(16,2) / COUNT(financial_balances.account_number)::numeric(16,2)) *100)::numeric(16,2) AS bad_people_percentage," &
                                     "((COALESCE(SUM(case when p" & Period & " > " & BadDebtAmount & " THEN total end)::numeric(16,2),0) / COALESCE(SUM(financial_balances.total_spent)::numeric(16,2),0)) *100)::numeric(16,2) AS bad_debt_percentage," &
                                     "MAX(branch_details.branch_name) AS branch_name " &
                                     "FROM debtor_dates " &
                                     "INNER JOIN debtor_first_purchase ON debtor_dates.account_number = debtor_first_purchase.account_number " &
                                     "INNER JOIN financial_balances ON debtor_first_purchase.account_number = financial_balances.account_number " &
                                     "INNER JOIN branch_details ON debtor_first_purchase.first_purchase = branch_details.branch_code " &
                                     "WHERE " &
                                     "debtor_dates.date_of_creation BETWEEN '" & StartDate & "' AND '" & EndDate & "'" &
                                     "AND total_spent > 0 AND (first_purchase NOT LIKE 'HO%' AND first_purchase <> '') " &
                                     "GROUP BY " &
                                     "debtor_first_purchase.first_purchase " &
                                     "ORDER BY debtor_first_purchase.first_purchase"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function BadDebtByStoreTransactions(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT " &
                                     "debtor_personal.account_number," &
                                     "debtor_personal.id_number," &
                                     "debtor_personal.itc_rating," &
                                     "debtor_first_purchase.first_purchase AS branch_code," &
                                     "financial_balances.total," &
                                     "financial_balances.current_balance," &
                                     "financial_balances.p30," &
                                     "financial_balances.p60," &
                                     "financial_balances.p90," &
                                     "financial_balances.p120," &
                                     "financial_balances.p150," &
                                     "financial_balances.total_spent," &
                                     "financial_balances.credit_limit," &
                                     "debtor_dates.date_of_creation," &
                                     "CASE WHEN CAST(itc_rating AS numeric) > 6 THEN 'THICK' ELSE 'THIN' END as file_type " &
                                     "FROM debtor_personal " &
                                     "INNER JOIN debtor_dates ON debtor_personal.account_number = debtor_dates.account_number " &
                                     "INNER JOIN debtor_first_purchase ON debtor_personal.account_number = debtor_first_purchase.account_number " &
                                     "INNER JOIN financial_balances ON debtor_personal.account_number = financial_balances.account_number " &
                                     "WHERE debtor_dates.date_of_creation BETWEEN '" & StartDate & "' AND '" & EndDate & "' " &
                                     "AND total_spent > 0 AND (first_purchase NOT LIKE 'HO%' AND first_purchase <> '')"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData
    End Function

    Public Function GetBadDebtData(ByVal SelectedPeriod As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text


            Dim strQuery As String = "SELECT " &
                                         "b2.account_number, " &
                                         "b2.total, " &
                                         "b2.current_balance, " &
                                         "b2.p30, " &
                                         "b2.p60, " &
                                         "b2.p90, " &
                                         "b2.p120, " &
                                         "b2.p150, " &
                                         "b2.purchase_amount, " &
                                         "b2.interest_amount, " &
                                          SelectedPeriod & " AS current_period " &
                                     "FROM " &
                                         "financial_closing_balances b1 " &
                                     "JOIN financial_closing_balances b2 ON ( " &
                                         "b1.account_number = b2.account_number " &
                                     "AND b1.current_period = " & Val(SelectedPeriod) - 1 & " " &
                                     "AND b2.current_period = " & SelectedPeriod & " " &
                                     ") " &
                                     "WHERE " &
                                        "b1.p150 = 0 " &
                                     "AND b2.p150 > 0 " &
                                     "ORDER BY b1.account_number;"


            'Dim strQuery As String = "SELECT account_number,total,current_balance,p30,p60,p90,p120,p150," &
            '                         "purchase_amount,interest_amount,current_period " &
            '                         "FROM financial_closing_balances " &
            '                         "WHERE p150 > 0  " &
            '                         "AND current_period = " & SelectedPeriod & " " &
            '                         "AND account_number IN (" &
            '                            "SELECT account_number " &
            '                            "FROM financial_closing_balances " &
            '                            "WHERE p150 = 0 " &
            '                            "AND current_period = " & Val(SelectedPeriod) - 1 & ")"

            'Dim strQuery As String = "Select " &
            '                         "* " &
            '                         "FROM bad_debt " &
            '                         "WHERE current_period = '" & SelectedPeriod & "' " &
            '                         "ORDER BY account_number"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function GetBadDebtRecoveredData(ByVal SelectedPeriod As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text

            Dim strQuery As String = "SELECT " &
                                     "b1.account_number, " &
                                     "b1.total, " &
                                     "b1.current_balance, " &
                                     "b1.p30, " &
                                     "b1.p60, " &
                                     "b1.p90, " &
                                     "b1.p120, " &
                                     "b1.p150, " &
                                     "b1.purchase_amount, " &
                                     "b1.interest_amount, " &
                                     SelectedPeriod & " AS current_period " &
                                     "FROM " &
                                        "financial_closing_balances b1 " &
                                     "JOIN financial_closing_balances b2 ON ( " &
                                        "b1.account_number = b2.account_number " &
                                        "AND b1.current_period = " & Val(SelectedPeriod) - 1 & " " &
                                        "AND b2.current_period = " & SelectedPeriod & " " &
                                     ") " &
                                     "WHERE " &
                                        "b1.p150 > 0 " &
                                     "AND b2.p150 = 0 " &
                                     "ORDER BY b1.account_number;"


            'Dim strQuery As String = "SELECT account_number,total,current_balance,p30,p60,p90,p120,p150," &
            '                         "purchase_amount,interest_amount," & SelectedPeriod & " AS current_period " &
            '                         "FROM financial_closing_balances " &
            '                         "WHERE current_period = " & Val(SelectedPeriod) - 1 & " " &
            '                         "AND account_number IN (" &
            '                            "SELECT account_number " &
            '                            "FROM financial_closing_balances " &
            '                            "WHERE p150 = 0 AND current_period = " & SelectedPeriod & " " &
            '                         "AND account_number IN " &
            '                            "(SELECT account_number " &
            '                            "FROM financial_closing_balances " &
            '                            "WHERE p150 > 0 AND current_period = " & Val(SelectedPeriod) - 1 & "))"


            'Dim strQuery As String = "Select " &
            '                         "* " &
            '                         "FROM bad_debt_recovered " &
            '                         "WHERE current_period = '" & SelectedPeriod & "' " &
            '                         "ORDER BY account_number"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function GetReductionIn150(ByVal SelectedPeriod As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text

            Dim strQuery As String = "SELECT current_period,account_number," &
                                     "prev_balance,p150 new_balance, prev_balance - p150 AS paid," &
                                     "interest_amount,prev_interest_amount,prev_interest_amount - interest_amount as paid_interest," &
                                     "purchase_amount,prev_purchase_amount,prev_purchase_amount - purchase_amount as paid_purchase " &
                                     "FROM (" &
                                     "SELECT current_period,account_number,p150,interest_amount,purchase_amount," &
                                         "  LAG ( p150 ) OVER ( PARTITION BY account_number ORDER BY current_period ) prev_balance," &
                                         "	LAG ( interest_amount ) OVER ( PARTITION BY account_number ORDER BY current_period ) prev_interest_amount," &
                                         "	LAG ( purchase_amount ) OVER ( PARTITION BY account_number ORDER BY current_period ) prev_purchase_amount " &
                                         "  FROM financial_closing_balances " &
                                         "  WHERE current_period IN ('" & Val(SelectedPeriod) - 1 & "','" & SelectedPeriod & "')) w " &
                                     "WHERE prev_balance IS NOT NULL " &
                                     "AND prev_balance > p150 " &
                                     "AND p150 > 0"
            'Dim strQuery As String = "SELECT current_period,account_number,prev_balance,p150 new_balance, prev_balance - p150 AS paid " &
            '                         "FROM (" &
            '                            "SELECT current_period,account_number,p150" &
            '                            "LAG ( p150 ) OVER ( PARTITION BY account_number ORDER BY current_period ) prev_balance " &
            '                            "FROM financial_closing_balances " &
            '                            "WHERE current_period IN ('" & Val(SelectedPeriod) - 1 & "','" & SelectedPeriod & "')) w " &
            '                         "WHERE prev_balance IS NOT NULL " &
            '                         "AND prev_balance > p150 " &
            '                         "AND p150 > 0"


            'Dim strQuery As String = "Select " &
            '                         "* " &
            '                         "FROM bad_debt_recovered " &
            '                         "WHERE current_period = '" & SelectedPeriod & "' " &
            '                         "ORDER BY account_number"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function



    Public Function GetInternalPeriods() As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT " &
                                     "internal_period ||' - '|| " &
                                     "real_month ||' ' || " &
                                     "real_year AS periods " &
                                     "FROM internal_period_to_date " &
                                     "WHERE internal_period >= 161 " &
                                     "ORDER BY internal_period"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function GetAgingSummary(ByVal MinimumAmount As String) As DataTable

        'Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "Select c || ' days' AS aging, COUNT(account_number) AS number_of_accounts, SUM(total) AS total_of_accounts " &
                                     "FROM " &
                                        "(SELECT CASE " &
                                        "WHEN p150 > 0 THEN 150 " &
                                        "WHEN p120 > 0 THEN 120 " &
                                        "WHEN p90 > 0 THEN 90 " &
                                        "WHEN p60 > 0 THEN 60 " &
                                        "WHEN p30 > 0 THEN 30 " &
                                        "END AS c, * FROM financial_balances) s " &
                                     "WHERE c IS NOT NULL " &
                                     "AND total > " & Val(MinimumAmount) & " " &
                                     "GROUP BY c ORDER BY c;"
            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData


    End Function

    Public Function GetAccountsToBeCalled(ByVal MinimumAmount As String) As DataTable

        'Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT c || ' days' AS aging, COUNT(s.account_number) AS number_of_accounts, SUM(total) AS total_of_accounts " &
                                     "FROM " &
                                     "   (SELECT CASE " &
                                     "    WHEN p150 > 0 THEN 150 " &
                                     "    WHEN p120 > 0 THEN 120 " &
                                     "    WHEN p90 > 0 THEN 90 " &
                                     "    WHEN p60 > 0 THEN 60 " &
                                     "    WHEN p30 > 0 THEN 30 " &
                                     "    END AS c, * FROM financial_balances) s" &
                                     "  INNER JOIN debtor_personal dp ON s.account_number = dp.account_number " &
                                     "WHERE s.current_contact_level = 0" &
                                     "      AND s.next_contact_time < now() " &
                                     "      AND total > " & MinimumAmount & " " &
                                     "    AND dp.status <> 'DEBT REVIEW' " &
                                     "    AND contact_investigation = False " &
                                     "      AND is_legal = False " &
                                     "      AND under_investigation = False " &
                                     "      AND c IS NOT NULL " &
                                     "  GROUP BY c " &
                                     "  ORDER BY c;"
            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function GetIncomingSMS(ByVal FromDate As String, ByVal ToDate As String) As DataTable

        'Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT time_stamp,id_number,first_name,last_name,salary,sms_result,status,credit_limit,received_from,sent_to," &
                                     "original_message,reply_sms,bureau_call," &
                                     "'--' || total_processing_time AS total_processing_time, " &
                                     "'--' || bureau_processing_time AS bureau_processing_time " &
                                     "FROM sms_incoming_sms " &
                                     "WHERE time_stamp between '" & FromDate & " 00:00:00' AND '" & ToDate & " 23:59:59' " &
                                     "ORDER BY time_stamp"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData


    End Function

    Public Function GetSalesPayments(ByVal OpenedFrom As String, ByVal OpenedTo As String,
                                     ByVal SalesFrom As String, ByVal SalesTo As String,
                                     ByVal PaymentsFrom As String, ByVal PaymentsTo As String) As DataTable

        'Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text

            Dim strQuery As String = "SELECT financial_transactions.account_number,SUM(CASE WHEN transaction_type = 'SALE' THEN 1 END) AS number_of_sales, " &
                                     "SUM(Case When transaction_type = 'PAY' THEN 1 END) AS number_of_payments, " &
                                     "SUM(CASE WHEN transaction_type = 'SALE' THEN transaction_amount END) AS sales, " &
                                     "SUM(CASE WHEN transaction_type = 'PAY' THEN transaction_amount END) AS payments, " &
                                     "MAX(itc_rating) AS itc_rating, " &
                                     "MAX(original_credit_limit) as original_credit_limit, " &
                                     "MAX(dp.branch_code) as branch_code, " &
                                     "MAX(bd.branch_name) as branch_name, " &
                                     "MAX(fcl.credit_limit) as current_credit_limit, " &
                                     "MAX(fcl.total) AS balance " &
                                     "FROM financial_transactions  " &
                                     "LEFT OUTER JOIN debtor_personal dp ON dp.account_number = financial_transactions.account_number " &
                                     "LEFT OUTER JOIN financial_balances fcl ON fcl.account_number = financial_transactions.account_number " &
                                     "LEFT OUTER JOIN branch_details bd ON dp.branch_code = bd.branch_code " &
                                     "WHERE ((financial_transactions.transaction_type = 'SALE' AND financial_transactions.sale_date " &
                                     "BETWEEN '" & SalesFrom & "' AND '" & SalesTo & "') " &
                                     "OR (financial_transactions.transaction_type = 'PAY' AND financial_transactions.sale_date " &
                                     "BETWEEN '" & PaymentsFrom & "' AND '" & PaymentsTo & "')) " &
                                     "AND financial_transactions.account_number  " &
                                     "IN (  " &
                                     "SELECT debtor_dates.account_number  " &
                                     "FROM debtor_dates  " &
                                     "INNER JOIN financial_balances ON debtor_dates.account_number = financial_balances.account_number  " &
                                     "WHERE debtor_dates.date_of_creation BETWEEN '" & OpenedFrom & "' AND '" & OpenedTo & "' AND total_spent > 0  " &
                                     "GROUP BY debtor_dates.account_number) GROUP BY financial_transactions.account_number  "

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData


    End Function

    Public Function GetLastUpdateData() As DataTable

        'Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pos_connection.Open()
            command.Connection = pos_connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "Select " &
                                    "branch_details.branch_name," &
                                    "version_numbers.branch_code," &
                                    "version_numbers.last_update_date," &
                                    "version_numbers.last_update_time," &
                                    "version_numbers.positive_shop_version," &
                                    "version_numbers.uploader_version," &
                                    "version_numbers.config_version," &
                                    "version_numbers.last_error," &
                                    "version_numbers.queries_uploaded," &
                                    "version_numbers.positive_ho_version," &
                                    "version_numbers.ip_address " &
                                    "FROM version_numbers " &
                                    "INNER JOIN branch_details On version_numbers.branch_code = branch_details.branch_code " &
                                    "ORDER BY branch_name"

            Dim reader As New NpgsqlDataAdapter(strQuery, pos_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pos_connection IsNot Nothing) Then
                pos_connection.Close()
            End If

        End Try

        Return xData


    End Function


    Public Function ReturnBureauLog(ByVal StartDate As String, ByVal EndDate As String) As DataTable

        'Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "Select " &
                        "TO_CHAR(web_service_logs.log_date, 'YYYY-MM-DD') AS logdate," &
                        "TO_CHAR(web_service_logs.log_time, 'HH24:MI:SS') AS logtime," &
                        "web_service_logs.id_number idnumber," &
                        "web_service_logs.account_number accountnumber," &
                        "web_service_logs.pre_existing preexisting," &
                        "web_service_logs.status originalstatus," &
                        "debtor_personal.status currentstatus," &
                        "debtor_personal.itc_rating itcrating," &
                        "financial_balances.credit_limit creditlimit " &
                        "FROM " &
                        "web_service_logs " &
                        "LEFT OUTER JOIN debtor_personal ON web_service_logs.account_number = debtor_personal.account_number " &
                        "LEFT OUTER JOIN financial_balances ON web_service_logs.account_number = financial_balances.account_number " &
                        "WHERE log_date BETWEEN '" & StartDate & "' AND '" & EndDate & "'"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function ReturnPTPListing(ByVal PTPDate As String) As DataTable

        'Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim MinimumCollectionAmount As Double = Val(ConfigurationManager.AppSettings("MinimumCollectionAmount"))

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text
            Dim strQuery As String = "SELECT " &
                                     "DISTINCT ON (fb.account_number) " &
                                     "fb.account_number," &
                                     "dp.first_name," &
                                     "dp.last_name," &
                                     "fb.total," &
                                     "fb.current_balance," &
                                     "fb.p30," &
                                     "fb.p60," &
                                     "fb.p90," &
                                     "fb.p120," &
                                     "fb.p150," &
                                     "dch.ptp_date," &
                                     "dch.ptp_amount," &
                                     "dch.username " &
                                     "FROM financial_balances AS fb " &
                                     "INNER JOIN debtor_personal AS dp ON fb.account_number = dp.account_number " &
                                     "INNER JOIN debtor_contact_history dch ON fb.account_number = dch.account_number " &
                                     "WHERE " &
                                     "fb.current_contact_level = 5 " &
                                     "AND result_of_action = 'PTP' " &
                                     "AND ptp_date >= '" & PTPDate & "' " &
                                     "AND p60 + p90 + p120 + p150 > " & MinimumCollectionAmount & " " &
                                     "ORDER BY " &
                                     "fb.account_number, dch.ptp_date DESC;"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function ReturnSMSReportData(ByVal SMSType As String, ByVal SendDate As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text

            Dim strQuery As String = "SELECT " &
                                     "TO_CHAR(sms_date, 'YYYY-MM-DD') AS sms_date," &
                                     "TO_CHAR(sms_time, 'HH24:MI:SS') AS sms_time," &
                                     "type_of_sms,account_number,cellphone_number,sms_message,sms_reply," &
                                     "current_balance,credit_limit,user_name,sms_sent,reason,amount_owing,amount_to_pay " &
                                     "FROM sms_sending " &
                                     "WHERE sms_date >= '" & SendDate & "'"

            If SMSType <> "All" Then
                strQuery &= " AND type_of_sms = '" & SMSType & "'"
            End If

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function ReturnPaymentsVSContactsReportData(ByVal FromDate As String, ByVal ToDate As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text

            Dim strQuery As String = "SELECT " &
                                     "DISTINCT ON (financial_transactions.account_number)  " &
                                     "financial_transactions.account_number," &
                                     "TO_CHAR(financial_transactions.sale_date, 'YYYY-MM-DD') AS sale_date," &
                                     "sale_time," &
                                     "financial_transactions.reference_number," &
                                     "financial_transactions.transaction_amount," &
                                     "debtor_contact_history.timestamp_of_contact," &
                                     "debtor_contact_history.result_of_action," &
                                     "debtor_contact_history.ptp_amount," &
                                     "debtor_contact_history.ptp_date," &
                                     "debtor_contact_history.username " &
                                     "FROM financial_transactions " &
                                     "LEFT OUTER JOIN debtor_contact_history ON financial_transactions.account_number = debtor_contact_history.account_number " &
                                     "WHERE sale_date between '" & FromDate & "' and '" & ToDate & "' " &
                                     "AND transaction_type = 'PAY' " &
                                     "ORDER BY " &
                                     "financial_transactions.account_number, debtor_contact_history.timestamp_of_contact DESC;"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function ReturnInvestigationListing(ByVal Status As String) As DataTable

        'Dim connection As Npgsql.NpgsqlConnection = Nothing

        Dim MinimumCollectionAmount As Double = Val(ConfigurationManager.AppSettings("MinimumCollectionAmount"))

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text

            Dim strQuery As String = "SELECT " &
                                     "fb.account_number," &
                                     "dp.first_name," &
                                     "dp.last_name," &
                                     "fb.total," &
                                     "fb.current_balance," &
                                     "fb.p30," &
                                     "fb.p60," &
                                     "fb.p90," &
                                     "fb.p120," &
                                     "fb.p150, " &
                                     "fb.current_contact_level " &
                                     "FROM financial_balances fb " &
                                     "INNER JOIN debtor_personal dp ON fb.account_number = dp.account_number "

            If Status = "Under Investigation" Then
                strQuery &= " WHERE under_investigation = True AND dp.contact_investigation = False AND dp.is_legal = False ORDER BY fb.account_number"
            ElseIf Status = "Contact Investigation" Then
                strQuery &= " WHERE under_investigation = False AND dp.contact_investigation = True AND dp.is_legal = False ORDER BY fb.account_number"
            ElseIf Status = "Legal" Then
                strQuery &= " WHERE under_investigation = False AND dp.contact_investigation = False AND dp.status = 'LEGAL' ORDER BY fb.account_number"
            ElseIf Status = "Debt Review" Then
                strQuery &= " WHERE under_investigation = False AND dp.contact_investigation = False AND dp.status = 'DEBT REVIEW' ORDER BY fb.account_number"
            ElseIf Status = "All Active" Then
                strQuery &= " WHERE dp.status = 'ACTIVE' ORDER BY fb.account_number"
            ElseIf Status = "Fraud" Then
                strQuery &= " WHERE dp.status = 'FRAUD' ORDER BY fb.account_number"
            ElseIf Status = "Out of Queue" Then
                strQuery &= " WHERE fb.current_contact_level = '6' ORDER BY fb.account_number"
            End If

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function


    Public Function GetLimitIncreaseReport(ByVal FromDate As String, ByVal ToDate As String) As DataTable

        Dim xData As New DataTable

        Try

            Dim command As New NpgsqlCommand()
            pcm_connection.Open()
            command.Connection = pcm_connection
            command.CommandType = CommandType.Text

            Dim strQuery As String = "select date_of_run,account_number,previous_limit,new_limit,limit_increased,auto_increase,current_balance,total_spent,income_amount,total_payments,number_sms_sent,additional_notes,sms_reply,guid from debtor_limit_increase where date_of_run between '" & FromDate & "' and '" & ToDate & "'"

            Dim reader As New NpgsqlDataAdapter(strQuery, pcm_connection)
            reader.Fill(xData)

        Catch ex As Exception
            Throw ex
        Finally
            If (pcm_connection IsNot Nothing) Then
                pcm_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function GetErrors(ByVal FromDate As String, ByVal ToDate As String) As DataTable
        Dim xData As New DataTable
        pos_connection = Me.DataBase("PostgreConnectionStringPositiveRead")

        Try
            Dim command As New NpgsqlCommand()
            pos_connection.Open()
            command.Connection = pos_connection
            command.CommandType = CommandType.Text
            command.CommandType = CommandType.Text
            Dim strQuery As String = "select error_log_id,inner_exception,message,source,stack_trace,help_link,hresult,target_site,data,created_date from error_log where created_date between '" & FromDate & "' and '" & ToDate & "' order by created_date desc"

            Dim reader As New NpgsqlDataAdapter(strQuery, pos_connection)
            reader.Fill(xData)



        Catch ex As Exception
            Throw ex
        Finally
            If (pos_connection IsNot Nothing) Then
                pos_connection.Close()
            End If

        End Try

        Return xData

    End Function

    Public Function GetCurrentTurnover(ByVal OrderBy As String) As CurrentTurnoverResponse
        Dim objDBPositive As New dlNpgSQL("PostgreConnectionStringPositiveRead")

        Dim drow As DataRow
        Dim runningTotal As Long = 0
        Dim runningTotalProfit As Long = 0
        Dim TotalStores As Long = 0
        Dim dsData As New DataSet

        dt.Columns.Add("branch")
        Dim colturnover As DataColumn = New DataColumn("turnover")
        colturnover.DataType = System.Type.GetType("System.Double")
        dt.Columns.Add(colturnover)

        Dim colprofit As DataColumn = New DataColumn("profit")
        colprofit.DataType = System.Type.GetType("System.Double")
        dt.Columns.Add(colprofit)

        dsData.Tables.Add(dt)

        tmpSQL = "SELECT * FROM current_turnover_mv "
        If OrderBy = "By Branch Name" Then
            tmpSQL &= "ORDER BY branch_name ASC"
        Else
            tmpSQL &= "ORDER BY sales DESC"
        End If

        Try
            Ds = objDBPositive.GetDataSet(tmpSQL)
            If objDBPositive.isR(Ds) Then
                For Each drBranch As DataRow In Ds.Tables(0).Rows
                    TotalStores += 1
                    drow = dt.NewRow
                    drow(0) = drBranch("branch_name")
                    drow(1) = Val(drBranch("sales") & "") - Val(drBranch("refunds") & "")
                    drow(2) = (Val(drBranch("sell_sale_ex") & "") + Val(drBranch("sell_refunds_ex") & "")) - (Val(drBranch("cost_sale_ex") & "") + Val(drBranch("cost_refunds_ex") & ""))
                    runningTotalProfit += (Val(drBranch("sell_sale_ex") & "") + Val(drBranch("sell_refunds_ex") & "")) - (Val(drBranch("cost_sale_ex") & "") + Val(drBranch("cost_refunds_ex") & ""))
                    runningTotal += Val(drBranch("sales") & "") - Val(drBranch("refunds") & "")
                    dt.Rows.Add(drow)
                Next
            End If
        Catch ex As Exception
            If (objDBPositive IsNot Nothing) Then
                objDBPositive.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDBPositive IsNot Nothing) Then
                objDBPositive.CloseConnection()
            End If
        End Try
        currentTurnoverResponse.GetDataResponse = dt
        currentTurnoverResponse.RunningTotal = runningTotal
        currentTurnoverResponse.RunningTotalProfit = runningTotalProfit
        currentTurnoverResponse.TotalStores = TotalStores
        Return currentTurnoverResponse

    End Function
    Public Function GenerateData(ByVal OrderBy As String, ByVal StartDate As String, ByVal EndDate As String) As CurrentTurnoverResponse
        Dim objDBPositive As New dlNpgSQL("PostgreConnectionStringPositiveRead")

        Dim drow As DataRow
        Dim runningTotal As Long = 0
        Dim runningTotalProfit As Long = 0
        Dim TotalStores As Long = 0
        Dim dsData As New DataSet

        dt.Columns.Add("branch")
        Dim colturnover As DataColumn = New DataColumn("turnover")
        colturnover.DataType = System.Type.GetType("System.Double")
        dt.Columns.Add(colturnover)

        Dim colprofit As DataColumn = New DataColumn("profit")
        colprofit.DataType = System.Type.GetType("System.Double")
        dt.Columns.Add(colprofit)

        dsData.Tables.Add(dt)

        tmpSQL = "SELECT " &
                 "branch_code,branch_name," &
                 "SUM(CASE when item_num=1 AND transaction_type = 'POSSALE' then transaction_total end) as sales," &
                 "SUM(CASE when item_num=1 AND transaction_type = 'POSREF' then transaction_total end) as refunds," &
                 "SUM(CASE WHEN transaction_type = 'POSSALE' THEN estimated_cost_price * quantity ELSE 0 END) AS cost_sale_ex," &
                 "SUM(CASE WHEN transaction_type = 'POSSALE' THEN selling_price * quantity ELSE 0 END) AS sell_sale_ex," &
                 "SUM(CASE WHEN transaction_type = 'POSREF' THEN estimated_cost_price * quantity ELSE 0 END) AS cost_refunds_ex," &
                 "SUM(CASE WHEN transaction_type = 'POSREF' THEN selling_price * quantity ELSE 0 END) AS sell_refunds_ex " &
                 "FROM " &
                 "(SELECT d.branch_code,p.branch_name,d.transaction_type,d.transaction_total,g.selling_price,g.estimated_cost_price,g.quantity,row_number() " &
                 "OVER (PARTITION by d.branch_code,g.link_guid) as item_num " &
                 "FROM transaction_master d " &
                 "JOIN transaction_line_items g ON d.guid = g.link_guid " &
                 "JOIN branch_details p ON d.branch_code = p.branch_code " &
                 "WHERE d.sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "')" &
                 "AS s GROUP BY branch_code,branch_name "

        If OrderBy = "By Branch Name" Then
            tmpSQL &= "ORDER BY branch_name ASC"
        Else
            tmpSQL &= "ORDER BY sales DESC"
        End If

        Try
            Ds = objDBPositive.GetDataSet(tmpSQL)
            If objDBPositive.isR(Ds) Then
                For Each drBranch As DataRow In Ds.Tables(0).Rows
                    TotalStores += 1
                    drow = dt.NewRow
                    drow(0) = drBranch("branch_name")
                    drow(1) = Val(drBranch("sales") & "") - Val(drBranch("refunds") & "")
                    drow(2) = (Val(drBranch("sell_sale_ex") & "") + Val(drBranch("sell_refunds_ex") & "")) - (Val(drBranch("cost_sale_ex") & "") + Val(drBranch("cost_refunds_ex") & ""))
                    runningTotalProfit += (Val(drBranch("sell_sale_ex") & "") + Val(drBranch("sell_refunds_ex") & "")) - (Val(drBranch("cost_sale_ex") & "") + Val(drBranch("cost_refunds_ex") & ""))
                    runningTotal += Val(drBranch("sales") & "") - Val(drBranch("refunds") & "")
                    dt.Rows.Add(drow)
                Next
            End If
        Catch ex As Exception
            If (objDBPositive IsNot Nothing) Then
                objDBPositive.CloseConnection()
            End If
            Return Nothing
        Finally
            If (objDBPositive IsNot Nothing) Then
                objDBPositive.CloseConnection()
            End If
        End Try
        currentTurnoverResponse.GetDataResponse = dt
        currentTurnoverResponse.RunningTotal = runningTotal
        currentTurnoverResponse.RunningTotalProfit = runningTotalProfit
        currentTurnoverResponse.TotalStores = TotalStores
        Return currentTurnoverResponse
    End Function

    Public Function GetTransactions(ByVal StartDate As String, ByVal EndDate As String) As GetTransactionsResponse

        tmpSQL = "SELECT  " &
                    "sale_date," &
                    "ROUND(AVG(transaction_amount),2) as avg," &
                    "ROUND(SUM(transaction_amount),2) as sum," &
                    "COUNT(account_number) as count " &
                 "FROM financial_transactions " &
                    "WHERE sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' AND transaction_type = 'SALE' " &
                 "GROUP BY sale_date"
        Try
            Ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)

            If usingObjDB.isR(Ds) Then
                dt = Ds.Tables(0)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        getTransactionsResponse.dt = dt

        Dim totalCount As Double = 0

        tmpSQL = "Select COUNT(DISTINCT account_number) as total_count  from financial_transactions
                    where sale_date between '" & StartDate & "' and  '" & EndDate & "'
                    And transaction_type = 'SALE'"

        Try
            Ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)

            If usingObjDB.isR(Ds) Then
                totalCount = Ds.Tables(0).Rows(0)("total_count")
            End If
        Catch ex As Exception
            Throw ex
        End Try

        getTransactionsResponse.TotalCount = totalCount

        Return getTransactionsResponse
    End Function

    Public Function GetCashTransactions(ByVal StartDate As String, ByVal EndDate As String) As GetTransactionsResponse

        tmpSQL = "SELECT sale_date, ROUND(AVG(sale_total), 2) as avg, COUNT(sale_total) AS count, ROUND(SUM(sale_total),2) as sum " &
                        "From cash_transactions " &
                        "WHERE sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' " &
                        "And transaction_type = 'POSSALE' AND account = 0 " &
                        "Group BY sale_date " &
                        "ORDER BY sale_date"
        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)

            If usingObjDB.isR(Ds) Then
                dt = Ds.Tables(0)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        getTransactionsResponse.dt = dt

        Dim totalCount As Double = 0

        tmpSQL = "Select COUNT(DISTINCT sale_total) as total_count  from cash_transactions where sale_date between '" & StartDate & "' and  '" & EndDate & "' And transaction_type = 'POSSALE' AND account = 0"

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)

            If usingObjDB.isR(Ds) Then
                totalCount = Ds.Tables(0).Rows(0)("total_count")
            End If
        Catch ex As Exception
            Throw ex
        End Try

        getTransactionsResponse.TotalCount = totalCount

        Return getTransactionsResponse
    End Function

    Public Function GetBranchDetails() As DataTable
        tmpSQL = "SELECT branch_code,
                  branch_name
                 FROM branch_details
                 ORDER by branch_code ASC"
        Try
            Ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)

            If usingObjDB.isR(Ds) Then
                dt = Ds.Tables(0)
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return dt
    End Function


    Public Function GetSegments(StartDate As String, EndDate As String, DateRange As String, Segment As String, AllBranches As Boolean, Optional ByVal strBranch As String = "") As GetSegmentsResponse
        Dim arrayWeekday(8) As String
        Dim arrayValue(8) As String

        Dim dsData As New DataSet
        Dim dt As New DataTable
        dt.Columns.Add("segment")
        dt.Columns.Add("value")
        dsData.Tables.Add(dt)

        Dim drow As DataRow
        Dim TotalCount As Double = 0

        tmpSQL = "SELECT " &
                 "SUM(CASE WHEN m.transaction_type = 'POSSALE' THEN m.transaction_total ELSE 0 END) as sales, " &
                 "SUM(CASE WHEN m.transaction_type = 'POSCN' THEN m.transaction_total ELSE 0 END) as cn, " &
                 "SUM(CASE WHEN m.transaction_type = 'POSREF' THEN m.transaction_total ELSE 0 END) as refunds, "

        If Segment = "Hours" Then
            tmpSQL = tmpSQL & "t.currrent_hour AS segment "
        ElseIf Segment = "Weeks" Then
            tmpSQL = tmpSQL & "t.current_week AS segment "
        ElseIf Segment = "Months" Then
            tmpSQL = tmpSQL & "t.period AS segment "
        ElseIf Segment = "Day of Week" Then
            tmpSQL = tmpSQL & "t.current_weekday AS segment "
        End If

        tmpSQL = tmpSQL & "FROM transaction_master m " &
                          "LEFT OUTER JOIN transaction_times t ON m.guid = t.guid " &
                          "WHERE m.sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' AND " &
                          "(m.transaction_type = 'POSSALE' OR m.transaction_type = 'POSCN' OR m.transaction_type = 'POSREF') "

        'Get the branches
        If Not String.IsNullOrWhiteSpace(strBranch) Then
            tmpSQL = tmpSQL & " AND (" & strBranch & ") "
        End If

        tmpSQL = tmpSQL & "GROUP BY segment " &
                          "ORDER BY segment"


        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                For Each drBranch As DataRow In Ds.Tables(0).Rows
                    TotalCount += 1
                    'Sort the weekdays
                    If Segment = "Day of Week" Then
                        If drBranch("segment") & "" = "Monday" Then
                            arrayWeekday(0) = "Monday"
                            arrayValue(0) = Val(drBranch("sales") & "") - Val(drBranch("cn") & "") - Val(drBranch("refunds") & "")
                        ElseIf drBranch("segment") & "" = "Tuesday" Then
                            arrayWeekday(1) = "Tuesday"
                            arrayValue(1) = Val(drBranch("sales") & "") - Val(drBranch("cn") & "") - Val(drBranch("refunds") & "")
                        ElseIf drBranch("segment") & "" = "Wednesday" Then
                            arrayWeekday(2) = "Wednesday"
                            arrayValue(2) = Val(drBranch("sales") & "") - Val(drBranch("cn") & "") - Val(drBranch("refunds") & "")
                        ElseIf drBranch("segment") & "" = "Thursday" Then
                            arrayWeekday(3) = "Thursday"
                            arrayValue(3) = Val(drBranch("sales") & "") - Val(drBranch("cn") & "") - Val(drBranch("refunds") & "")
                        ElseIf drBranch("segment") & "" = "Friday" Then
                            arrayWeekday(4) = "Friday"
                            arrayValue(4) = Val(drBranch("sales") & "") - Val(drBranch("cn") & "") - Val(drBranch("refunds") & "")
                        ElseIf drBranch("segment") & "" = "Saturday" Then
                            arrayWeekday(5) = "Saturday"
                            arrayValue(5) = Val(drBranch("sales") & "") - Val(drBranch("cn") & "") - Val(drBranch("refunds") & "")
                        ElseIf drBranch("segment") & "" = "Sunday" Then
                            arrayWeekday(6) = "Sunday"
                            arrayValue(6) = Val(drBranch("sales") & "") - Val(drBranch("cn") & "") - Val(drBranch("refunds") & "")
                        End If
                    Else
                        drow = dt.NewRow
                        drow(0) = drBranch("segment")
                        drow(1) = Val(drBranch("sales") & "") - Val(drBranch("cn") & "") - Val(drBranch("refunds") & "")
                        dt.Rows.Add(drow)
                    End If
                Next
            End If

            If Segment = "Day of Week" Then
                For zLoop As Integer = 0 To 6
                    drow = dt.NewRow
                    drow(0) = arrayWeekday(zLoop)
                    drow(1) = arrayValue(zLoop)
                    dt.Rows.Add(drow)
                Next
            End If

        Catch ex As Exception
            Throw ex
        End Try
        getSegmentsResponse.TotalCount = TotalCount
        getSegmentsResponse.dt = dt
        Return getSegmentsResponse
    End Function

    Public Sub InsertUserRecord(ByVal Username As String, ByVal isManager As String,
                             ByVal IPAddress As String, ByVal TypeOfAction As String,
                             ByVal ReportName As String, ByVal ReportOptions As String)


        tmpSQL = "INSERT INTO web_users_logs (user_name,is_admin,ip_address,type_of_action,report_name,report_options) VALUES " &
                 "('" & RG.Apos(Mid$(Username, 1, 30)) & "','" & isManager & "','" & RG.Apos(Mid$(IPAddress, 1, 150)) & "'," &
                 "'" & RG.Apos(Mid$(TypeOfAction, 1, 100)) & "','" & RG.Apos(Mid$(ReportName, 1, 100)) & "','" & RG.Apos(Mid$(ReportOptions, 1, 250)) & "')"

        Try
            usingObjDB.ExecuteQuery(_POSReadConnectionString, tmpSQL)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Function GetSentSMSLog(ByVal FromDate As String, ByVal ToDate As String) As DataTable
        tmpSQL = "SELECT sms_log_id,
                   user_name,
                   data,status,
                   error_message,
                   result,
                   created_date,
                   sent_message,
                   total_phone_numbers_count,
                    failed_phone_number_count
                   FROM sent_sms_log
                   ORDER by created_date desc"
        Try
            Ds = usingObjDB.GetDataSet(_PCMReadConnectionString, tmpSQL)

            If usingObjDB.isR(Ds) Then
                dt = Ds.Tables(0)
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return dt

    End Function

    Public Function GetSentEmployeeDetails() As DataTable
        tmpSQL = "SELECT
                 employee_number,
                 first_name,
                 last_name,
                 id_number, 
                 email_address,
                 cellphone, 
                 last_login 
                 FROM employee_details
                 ORDER by date_last_updated desc"
        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)

            If usingObjDB.isR(Ds) Then
                dt = Ds.Tables(0)
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return dt

    End Function

    Public Function GetBestSellersReport(ByVal StartDate As String, ByVal EndDate As String, ByVal Trim As String) As DataSet
        Dim data As DataSet

        tmpSQL = "SELECT " &
                 "SUM(CASE WHEN m.transaction_type IN ('POSSALE','POSCN','POSREF') THEN l.quantity END) AS qty, " &
                 "SUM(CASE WHEN m.transaction_type IN ('POSSALE','POSCN','POSREF') THEN (l.selling_price * l.quantity + 0.001) END) as selling," &
                 "SUM(CASE WHEN m.transaction_type IN ('POSSALE','POSCN','POSREF') THEN (l.estimated_cost_price * l.quantity) END) AS estimated, " &
                 "SUM(CASE WHEN m.transaction_type IN ('POSSALE','POSCN','POSREF') THEN (l.average_cost_price * l.quantity) END) AS average, "

        tmpSQL = tmpSQL & Trim
        tmpSQL = tmpSQL & "min(l.description) as description " &
                          "FROM transaction_master m " &
                          "INNER JOIN transaction_line_items l ON m.guid = l.link_guid " &
                          "WHERE m.sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' " &
                          "AND (m.transaction_type = 'POSSALE' OR m.transaction_type = 'POSCN' OR m.transaction_type = 'POSREF') " &
                          "GROUP BY stockcode ORDER BY qty DESC nulls LAST"


        Dim masterData As New DataTable

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                masterData = Ds.Tables(0).Copy()
            End If
        Catch ex As Exception
            Throw ex
        End Try

        masterData.PrimaryKey = New DataColumn() {masterData.Columns("stockcode")}
        masterData.TableName = "Master"


        tmpSQL = "SELECT m.sale_date,b.branch_name,m.transaction_type,"

        tmpSQL = tmpSQL & Trim

        tmpSQL &= "(l.selling_price * l.quantity) * (1 + l.tax_percentage /100) as selling_price,l.quantity " &
                 "FROM transaction_master m " &
                 "LEFT OUTER JOIN transaction_line_items l ON m.guid = l.link_guid " &
                 "INNER JOIN branch_details b ON m.branch_code = b.branch_code " &
                 "WHERE m.sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' " &
                 "AND (m.transaction_type = 'POSSALE' OR m.transaction_type = 'POSCN' OR m.transaction_type = 'POSREF') " &
                 "ORDER BY sale_date DESC nulls LAST"

        Dim detailData As New DataTable
        Dim relation As DataRelation

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                detailData = Ds.Tables(0).Copy()
            End If
        Catch ex As Exception
            Throw ex
        End Try

        detailData.PrimaryKey = New DataColumn() {detailData.Columns("guid")}
        detailData.TableName = "Detail"

        data = New DataSet()
        data.Tables.Add(masterData)
        data.Tables.Add(detailData)
        relation = New DataRelation("MasterDetail", masterData.Columns("stockcode"), detailData.Columns("stockcode"))
        data.Relations.Add(relation)


        Return data
    End Function

    Public Function GetAccountSales(StartDate As String, EndDate As String) As GetAccountSalesResponse

        Dim dsData As New DataSet
        Dim dt As New DataTable
        Dim drow As DataRow
        dt.Columns.Add("branch")
        dt.Columns.Add("turnover")
        dt.Columns.Add("payments")
        dsData.Tables.Add(dt)

        Dim runningTotal As Long = 0
        Dim runningPayments As Long = 0
        Dim TotalStores As Long = 0

        tmpSQL = "SELECT " &
             "d.branch_code,p.branch_name," &
             "SUM(CASE WHEN transaction_type = 'POSSALE' THEN d.account ELSE 0 END) AS sales," &
             "SUM(CASE WHEN transaction_type = 'POSREF' THEN d.account ELSE 0 END) AS refunds," &
             "SUM(CASE WHEN transaction_type = 'ACCPAY' THEN d.sale_total ELSE 0 END) AS payments " &
             "FROM cash_transactions d " &
             "JOIN branch_details p ON d.branch_code = p.branch_code " &
             "WHERE d.sale_date BETWEEN '" & StartDate & "' AND '" & EndDate & "' " &
             "GROUP BY d.branch_code,p.branch_name ORDER BY d.branch_code ASC"


        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                For Each drBranch As DataRow In Ds.Tables(0).Rows
                    TotalStores += 1
                    drow = dt.NewRow
                    drow(0) = drBranch("branch_name")
                    drow(1) = Val(drBranch("sales") & "") - Val(drBranch("refunds") & "")
                    drow(2) = drBranch("payments")
                    runningTotal += Val(drBranch("sales") & "") - Val(drBranch("refunds") & "")
                    runningPayments += Val(drBranch("payments"))
                    dt.Rows.Add(drow)
                Next
            End If


        Catch ex As Exception
            Throw ex
        End Try

        getAccountSalesResponse.TotalStores = TotalStores
        getAccountSalesResponse.RunningTotal = runningTotal
        getAccountSalesResponse.RunningPayments = runningPayments
        getAccountSalesResponse.dt = dt
        Return getAccountSalesResponse
    End Function

    Public Function GetMasterEmployeesPerStore(Permanent As Boolean, Casual As Boolean, All As Boolean, ByVal StartDate As String, ByVal EndDate As String) As DataSet
        Dim WhereCondition As String = String.Empty
        Dim subQuery As String = String.Empty
        Dim query As String = String.Empty
        Dim permantQuery As String = String.Empty
        Dim casualQuery As String = String.Empty

        Dim ColumnsList As String = " branch_details.branch_code,branch_name,"

        If (All = True) Then
            query = ""
            permantQuery = ""
            casualQuery = ""
        ElseIf Permanent = True Then
            query = "and is_casual=false"

            casualQuery = IIf(Permanent, " and 1=2", "")
            permantQuery = ""
        ElseIf Casual = True Then
            query = "and is_casual=true"

            casualQuery = ""
            permantQuery = IIf(Casual, " and 1=2", "")
        End If


        subQuery &= "(Select count(emptr.branch_code) from employee_transactions empTr inner join employee_details as emp on emp.employee_number = emptr.employee_number and emp.last_guid = emptr.guid where emptr.branch_code = branch_details.branch_code and 
                         is_logged_in = 't'  AND last_login BETWEEN '" & StartDate & " 00:00:00'  AND '" & EndDate & " 23:59:59' and
                      emp.is_casual=true" & casualQuery & "),
                     (Select count(emptr.branch_code) from employee_transactions empTr inner join employee_details as emp on emp.employee_number = emptr.employee_number and emp.last_guid = emptr.guid where emptr.branch_code = branch_details.branch_code and 
                     is_logged_in = 't'  AND last_login BETWEEN '" & StartDate & " 00:00:00'   AND '" & EndDate & " 23:59:59' and
                     emp.is_casual=false" & permantQuery & ") as count1"
        where = " FROM branch_details  
                     left join employee_transactions on employee_transactions.branch_code = branch_details.branch_code
                    left join employee_details on employee_details.employee_number = employee_transactions.employee_number and employee_details.last_guid = employee_transactions.guid 
                   where  is_logged_in = 't'  AND last_login BETWEEN '" & StartDate & " 00:00:00'   AND '" & EndDate & " 23:59:59' " & query & " and 
                  branch_details.is_blocked=false"

        subQuery &= where & " group by branch_details.branch_code ORDER BY count"


        Dim dt As New DataSet

        Try
            tmpSQL = "Select " & ColumnsList & " " & " " & subQuery & ""
            dt = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)

            dt.Tables.Add(GetEmployeesPerStoreDetails(StartDate, EndDate, where))
        Catch ex As Exception
            Throw ex
        End Try
        Return dt

    End Function


    Public Function GetEmployeesPerStoreDetails(StartDate As String, EndDate As String, query As String) As DataTable


        tmpSQL = "SELECT " &
                 "employee_transactions.employee_number, " &
                 "employee_details.first_name ||' '|| employee_details.last_name AS employee_name, " &
                 "employee_details.is_logged_in, " &
                 "employee_transactions.branch_code, " &
                 "employee_transactions.clocking_date_in, " &
                 "employee_transactions.clocking_hour_in, " &
                 "employee_transactions.clocking_date_out, " &
                 "employee_transactions.clocking_hour_out, " &
                 "employee_transactions.time_worked, " &
                 "employee_transactions.reason " &
                 "FROM employee_transactions " &
                 "INNER JOIN employee_details ON employee_transactions.employee_number = employee_details.employee_number " &
                 "WHERE  clocking_time_in BETWEEN '" & StartDate & " 00:00:00' " &
                 "AND '" & EndDate & " 23:59:59'"

        If query <> "" Then
            tmpSQL &= " and employee_transactions.branch_code IN (SELECT DISTINCT
		                 branch_details.branch_code " & where & ")"
        End If

        Try

            dt = usingObjDB.GetDataTable(_POSReadConnectionString, tmpSQL)

        Catch ex As Exception
            Throw ex
        End Try
        Return dt



    End Function

    Public Function GetClockingEmployees(ByVal EmployeeNumber As String) As DataTable
        tmpSQL = "SELECT employee_number,
                  first_name,
                  last_name 
                 FROM employee_details
                  WHERE employee_number " & "LIKE '" & UCase$(EmployeeNumber) & "%' ORDER BY employee_details ASC LIMIT 30"

        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)

            If usingObjDB.isR(Ds) Then
                dt = Ds.Tables(0)
            End If
        Catch ex As Exception
            Throw ex
        End Try

        Return dt

    End Function

    Public Function GetReports_Test() As DataSet
        Dim data As DataSet
        tmpSQL = "SELECT employee_number,
                  first_name,
                  last_name
                 FROM employee_details limit 50"
        Dim masterdata As DataTable
        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                masterdata = Ds.Tables(0).Copy
            End If
        Catch ex As Exception
            Throw ex
        End Try
        masterdata.PrimaryKey = New DataColumn() {masterdata.Columns("employee_number")}
        masterdata.TableName = "master"
        tmpSQL = "Select employee_number,time_worked,user_name,branch_code from employee_transactions limit 50"
        Dim detailData As New DataTable
        Dim relation As DataRelation
        Try
            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
            If usingObjDB.isR(Ds) Then
                detailData = Ds.Tables(0).Copy
            End If
        Catch ex As Exception
            Throw ex
        End Try
        detailData.PrimaryKey = New DataColumn() {detailData.Columns("guide")}
        detailData.TableName = "detail"
        data = New DataSet()
        data.Tables.Add(masterdata)
        data.Tables.Add(detailData)
        relation = New DataRelation("MasterDetail", masterdata.Columns("employee_number"), detailData.Columns("employee_number"))
        data.Relations.Add(relation)
        Return data
    End Function
    '    Public Function GetDetailReports_Test() As DataSet
    '        tmpSQL = "SELECT is_Active,date_last_updated,bank_account_number,bank_branch_code
    '                 FROM employee_details limit 100"
    '        Try
    '            Ds = usingObjDB.GetDataSet(_POSReadConnectionString, tmpSQL)
    '        Catch ex As Exception
    '            Throw ex
    '        End Try
    '        Return Ds
    '    End Function
End Class



