Namespace Models
	Public Class ClaimRemarks
		 Private _Idkey As String
        Private _ClaimId As String
        Private _Remarks As String
        Private _CreatedBy As String
        Private _CreatedOn As String
        Public Property Idkey() As String
            Get
                Return _Idkey
            End Get
            Set(ByVal value As String)
                _Idkey = value
            End Set
        End Property
        Public Property ClaimId() As String
            Get
                Return _ClaimId
            End Get
            Set(ByVal value As String)
                _ClaimId = value
            End Set
        End Property
		 Public Property Remarks() As String
			 Get 
				 Return _Remarks
			 End Get
			 Set(ByVal value As String) 
				_Remarks = value
			 End Set
        End Property
        Public Property CreatedBy() As String
            Get
                Return _CreatedBy
            End Get
            Set(ByVal value As String)
                _CreatedBy = value
            End Set
        End Property
		 Public Property CreatedOn() As String
			 Get 
				 Return _CreatedOn
			 End Get
			 Set(ByVal value As String) 
				_CreatedOn = value
			 End Set
		 End Property
	End Class
End Namespace
