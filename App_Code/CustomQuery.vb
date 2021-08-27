Imports Microsoft.VisualBasic
Namespace Models
    Public Class CustomQuery
        Private InputQuery As String
        Private Parameters As Dictionary(Of String, String)
        Private DB As String
        Public Property _InputQuery() As String
            Get
                Return InputQuery
            End Get
            Set(ByVal value As String)
                InputQuery = value
            End Set
        End Property
        Public Property _Parameters() As Dictionary(Of String, String)
            Get
                Return Parameters
            End Get
            Set(ByVal value As Dictionary(Of String, String))
                Parameters = value
            End Set
        End Property
        Public Property _DB() As String
            Get
                Return DB
            End Get
            Set(ByVal value As String)
                DB = value
            End Set
        End Property
    End Class
End Namespace

