Imports Microsoft.VisualBasic
Namespace Models
    Public Class DeleteQuery
        Private InputTable As Object
        Private HasWhereConditions As Boolean
        Private Conditions As Dictionary(Of String, List(Of String))
        Private HasInBetweenConditions As Boolean
        Private InBetweenCondition As String
        Private DB As String

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