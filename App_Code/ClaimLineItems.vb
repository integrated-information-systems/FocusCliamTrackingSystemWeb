Namespace Models
	Public Class ClaimLineItems
		 Private _Headerid As String
		 Private _SNo As String
		 Private _InvDate As String
		 Private _InvNo As String
		 Private _Qty As String
		 Private _Brand As String
		 Private _ModelNo As String
		 Private _SerialNo As String
        Private _ClaimQty As String
        Private _OpenQty As String
        Private _Particulars As String
		 Private _ClaimMode As String
		 Private _ClaimDate As String
		 Private _ClaimReference As String
		 Private _PartNo As String
		 Private _Claimed As String
		 Private _Status As String
		 Private _DeclareValue As String
		 Private _DeclareDescription As String
		 Public Property Headerid() As String
			 Get 
				 Return _Headerid
			 End Get
			 Set(ByVal value As String) 
				_Headerid = value
			 End Set
		 End Property
		 Public Property SNo() As String
			 Get 
				 Return _SNo
			 End Get
			 Set(ByVal value As String) 
				_SNo = value
			 End Set
		 End Property
		 Public Property InvDate() As String
			 Get 
				 Return _InvDate
			 End Get
			 Set(ByVal value As String) 
				_InvDate = value
			 End Set
		 End Property
		 Public Property InvNo() As String
			 Get 
				 Return _InvNo
			 End Get
			 Set(ByVal value As String) 
				_InvNo = value
			 End Set
		 End Property
		 Public Property Qty() As String
			 Get 
				 Return _Qty
			 End Get
			 Set(ByVal value As String) 
				_Qty = value
			 End Set
		 End Property
		 Public Property Brand() As String
			 Get 
				 Return _Brand
			 End Get
			 Set(ByVal value As String) 
				_Brand = value
			 End Set
		 End Property
		 Public Property ModelNo() As String
			 Get 
				 Return _ModelNo
			 End Get
			 Set(ByVal value As String) 
				_ModelNo = value
			 End Set
		 End Property
		 Public Property SerialNo() As String
			 Get 
				 Return _SerialNo
			 End Get
			 Set(ByVal value As String) 
				_SerialNo = value
			 End Set
		 End Property
		 Public Property ClaimQty() As String
			 Get 
				 Return _ClaimQty
			 End Get
			 Set(ByVal value As String) 
				_ClaimQty = value
			 End Set
        End Property
        Public Property OpenQty() As String
            Get
                Return _OpenQty
            End Get
            Set(ByVal value As String)
                _OpenQty = value
            End Set
        End Property
		 Public Property Particulars() As String
			 Get 
				 Return _Particulars
			 End Get
			 Set(ByVal value As String) 
				_Particulars = value
			 End Set
		 End Property
		 Public Property ClaimMode() As String
			 Get 
				 Return _ClaimMode
			 End Get
			 Set(ByVal value As String) 
				_ClaimMode = value
			 End Set
		 End Property
		 Public Property ClaimDate() As String
			 Get 
				 Return _ClaimDate
			 End Get
			 Set(ByVal value As String) 
				_ClaimDate = value
			 End Set
		 End Property
		 Public Property ClaimReference() As String
			 Get 
				 Return _ClaimReference
			 End Get
			 Set(ByVal value As String) 
				_ClaimReference = value
			 End Set
		 End Property
		 Public Property PartNo() As String
			 Get 
				 Return _PartNo
			 End Get
			 Set(ByVal value As String) 
				_PartNo = value
			 End Set
		 End Property
		 Public Property Claimed() As String
			 Get 
				 Return _Claimed
			 End Get
			 Set(ByVal value As String) 
				_Claimed = value
			 End Set
		 End Property
		 Public Property Status() As String
			 Get 
				 Return _Status
			 End Get
			 Set(ByVal value As String) 
				_Status = value
			 End Set
		 End Property
		 Public Property DeclareValue() As String
			 Get 
				 Return _DeclareValue
			 End Get
			 Set(ByVal value As String) 
				_DeclareValue = value
			 End Set
		 End Property
		 Public Property DeclareDescription() As String
			 Get 
				 Return _DeclareDescription
			 End Get
			 Set(ByVal value As String) 
				_DeclareDescription = value
			 End Set
		 End Property
	End Class
End Namespace
