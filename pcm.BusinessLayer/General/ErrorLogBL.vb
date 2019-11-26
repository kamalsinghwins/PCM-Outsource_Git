Imports Entities
Imports pcm.DataLayer

Public Class ErrorLogBL
    Private _dlErrorLogging As New ErrorLogDL



    Public Sub ErrorLogging(_errorLog As Exception)

        _dlErrorLogging.ErrorLogging(_errorLog)

    End Sub

End Class
