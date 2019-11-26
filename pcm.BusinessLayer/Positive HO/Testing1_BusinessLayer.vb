Imports Entities
Imports pcm.DataLayer

Public Class Testing1_BusinessLayer
    Private _testing1 As New Testing1_DataLayer

    Public Function Search(srch As Search) As DataTable
        Dim GetSpecialName As DataTable
        GetSpecialName = _testing1.Search(srch)
        Return GetSpecialName
    End Function

    Public Function AddData(_test As Testing3) As DataTable
        Dim Data As DataTable
        Data = _testing1.AddData(_test)
        Return Data
    End Function
End Class
