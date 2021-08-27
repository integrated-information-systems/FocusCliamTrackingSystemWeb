Imports Microsoft.VisualBasic
Namespace Models
    Public Class SelectQuery
        Private InputTable As Object
        Private HasWhereConditions As Boolean
        Private Conditions As Dictionary(Of String, List(Of String))
        Private HasInBetweenConditions As Boolean
        Private InBetweenCondition As String
        Private OrderBy As String
        Private TopRecord As Integer
        Private DB As String
        Public Sub New()
            TopRecord = 0
        End Sub
        Public Property _InputTable() As Object
            Get
                Return InputTable
            End Get
            Set(ByVal value As Object)
                InputTable = value
            End Set
        End Property
        Public Property _HasWhereConditions() As Boolean
            Get
                Return HasWhereConditions
            End Get
            Set(ByVal value As Boolean)
                HasWhereConditions = value
            End Set
        End Property
        Public Property _Conditions() As Dictionary(Of String, List(Of String))
            Get
                Return Conditions
            End Get
            Set(ByVal value As Dictionary(Of String, List(Of String)))
                Conditions = value
            End Set
        End Property
        Public Property _HasInBetweenConditions() As Boolean
            Get
                Return HasInBetweenConditions
            End Get
            Set(ByVal value As Boolean)
                HasInBetweenConditions = value
            End Set
        End Property
        Public Property _InBetweenCondition() As String
            Get
                Return InBetweenCondition
            End Get
            Set(ByVal value As String)
                InBetweenCondition = value
            End Set
        End Property
        Public Property _OrderBy() As String
            Get
                Return OrderBy
            End Get
            Set(ByVal value As String)
                OrderBy = value
            End Set
        End Property
        Public Property _TopRecord() As Integer
            Get
                Return TopRecord
            End Get
            Set(ByVal value As Integer)
                If IsNothing(value) Then
                    TopRecord = 0
                Else
                    TopRecord = value
                End If
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

