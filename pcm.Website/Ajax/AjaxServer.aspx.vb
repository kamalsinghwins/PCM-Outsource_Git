Imports System.Net
Imports Entities
Imports Entities.IBT_In
Imports Entities.IBT_Out
Imports Newtonsoft.Json
Imports pcm.BusinessLayer

Public Class AjaxServer
    Inherits System.Web.UI.Page

    Private sUserId As String
    Private _SendContent As Boolean
    Private Page As String
    Private Action As String
    Private Message As String
    Private _IBT_Out As IBT_OutHOBL = New IBT_OutHOBL
    Private _IBT_In As IBT_InHOBL = New IBT_InHOBL
    Dim req As New WebClient

    Private _JsonResponse As New JsonResponse



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _JsonResponse.Success = False
        'If (Session("UserContext") == null || !HttpContext.Current.User.Identity.IsAuthenticated)

        '    Response.Clear()
        '    Response.Write(Message)
        '    Response.ContentType = "Text/Json"
        '    Response.End()
        '    _SendContent = True
        '    Exit Sub

        'End If


        sUserId = String.Empty
        _SendContent = False
        Page = String.Empty
        Action = String.Empty
        Message = String.Empty


        If String.IsNullOrEmpty(Request.QueryString("Action")) = False Then
            Action = Request.QueryString("Action").ToString()
        End If

        If Request.QueryString("Page") IsNot Nothing Then
            Page = Request.QueryString("Page").ToString()

            Dim RequsestFormData = Request.Form("formData")

            Select Case Page
                Case "IBT"
                    Select Case Action
                        Case "GetBranch"

                            Dim branchRequest = JsonConvert.DeserializeObject(Of GetBranch)(RequsestFormData)
                            Dim branchResult = _IBT_Out.SearchBranch(branchRequest.SearchType, branchRequest.SearchDetail)

                            Message = JsonConvert.SerializeObject(branchResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                        Case "GetItems"
                            Dim itemRequest = JsonConvert.DeserializeObject(Of GetItems)(RequsestFormData)
                            Dim itemResult = _IBT_Out.Searchicibtout(itemRequest.SearchText, itemRequest.SearchType, itemRequest.Master)

                            Message = JsonConvert.SerializeObject(itemResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True
                        Case "GetItemCodeDetails"
                            Dim itemCodeDetails = JsonConvert.DeserializeObject(Of ItemCode)(RequsestFormData)
                            Dim itemResult = _IBT_Out.GetGeneratedCodeInfo(itemCodeDetails.ItemCode, itemCodeDetails.BranchCode)

                            Message = JsonConvert.SerializeObject(itemResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                        Case "GetBranchDetails"
                            Dim branchDetails = JsonConvert.DeserializeObject(Of BranchDetail)(RequsestFormData)
                            Dim branchResult = _IBT_Out.GetBranchDetails(branchDetails.BranchCode)

                            Message = JsonConvert.SerializeObject(branchResult)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                        Case "Save"
                            Dim saveDetails = JsonConvert.DeserializeObject(Of Save)(RequsestFormData)
                            Dim saveResult = _IBT_Out.Save(saveDetails)

                            Message = JsonConvert.SerializeObject(saveResult)

                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"
                            _SendContent = True


                        Case "DownloadLabel"

                            Dim path As String = HttpContext.Current.Server.MapPath("~\temp\" & RequsestFormData & "")
                            Dim file As System.IO.FileInfo = New System.IO.FileInfo(path)
                            If file.Exists Then
                                Response.Clear()
                                Dim data As Byte() = req.DownloadData(path)
                                Response.BinaryWrite(data)
                                'Response.ContentType = "Text/Json"
                                Response.ContentType = "application/octet-stream"

                                Response.End()

                            Else
                                Response.Write("This file does not exist.")
                            End If



                            _SendContent = True


                        Case "Reload"
                            Dim user As String = Session("username")
                            Dim reload = _IBT_Out.Reload(user)

                            Message = JsonConvert.SerializeObject(reload)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True


                        Case "GetCodes"
                            Dim getCode = JsonConvert.DeserializeObject(Of GetCodes)(RequsestFormData)
                            Dim codes = _IBT_Out.GetCodes(getCode)

                            Message = JsonConvert.SerializeObject(codes)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True


                        Case "DeleteFile"
                            Dim deletefiles = JsonConvert.DeserializeObject(Of DeleteFiles)(RequsestFormData)
                            _IBT_Out.DeleteFile(deletefiles)

                            'Message = JsonConvert.SerializeObject(barcodes)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                    End Select

                Case "IBT_In"
                    Select Case Action
                        Case "FetchDetails"
                            Dim getDetails = JsonConvert.DeserializeObject(Of FetchDetails)(RequsestFormData)
                            Dim details = _IBT_In.FetchDetails(getDetails)

                            Message = JsonConvert.SerializeObject(details)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True


                        Case "Save"
                            Dim saveDetails = JsonConvert.DeserializeObject(Of SaveIBTIN)(RequsestFormData)
                            Dim saveResult = _IBT_In.Save(saveDetails)

                            Message = JsonConvert.SerializeObject(saveResult)

                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"
                            _SendContent = True

                        Case "DeleteFile"
                            Dim deletefiles = JsonConvert.DeserializeObject(Of DeletePrintFile)(RequsestFormData)
                            _IBT_In.DeleteFile(deletefiles)

                            'Message = JsonConvert.SerializeObject(barcodes)
                            Response.Clear()
                            Response.Write(Message)
                            Response.ContentType = "Text/Json"

                            _SendContent = True

                    End Select
            End Select
        End If


        If _SendContent = True Then
            Response.End()
        End If
    End Sub

End Class