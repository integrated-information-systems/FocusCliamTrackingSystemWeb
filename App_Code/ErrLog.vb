Imports Microsoft.VisualBasic
Namespace Models
    Public Class ErrLog
        Private _IdKey As String
        Private _errMSg As String
        Private _InnerException As String
        Private _FileName As String
        Private _LineNumber As String
        Private _CreatedOn As String
        Public Property IdKey() As String
            Get
                
                    Return _IdKey


            End Get
            Set(ByVal value As String)
                _IdKey = value
            End Set
        End Property
        Public Property errMSg() As String
            Get
                If IsNothing(_errMSg) Then
                    Return String.Empty
                Else
                    Return _errMSg
                End If

            End Get
            Set(ByVal value As String)
                _errMSg = value
            End Set
        End Property
        Public Property InnerException() As String
            Get
                If IsNothing(_InnerException) Then
                    Return String.Empty
                Else
                    Return _InnerException
                End If

            End Get
            Set(ByVal value As String)
                _InnerException = value
            End Set
        End Property
        Public Property FileName() As String
            Get
                If IsNothing(_FileName) Then
                    Return String.Empty
                Else
                    Return _FileName
                End If

            End Get
            Set(ByVal value As String)
                _FileName = value
            End Set
        End Property
        Public Property LineNumber() As String
            Get
                If IsNothing(_LineNumber) Then
                    Return String.Empty
                Else
                    Return _LineNumber
                End If

            End Get
            Set(ByVal value As String)
                _LineNumber = value
            End Set
        End Property

        Public Property CreatedOn() As String
            Get
                If IsNothing(_CreatedOn) Then
                    Return String.Empty
                Else
                    Return _CreatedOn
                End If

            End Get
            Set(ByVal value As String)
                _CreatedOn = value
            End Set
        End Property
    End Class
End Namespace

