Namespace Models
	Public Class Files
		 Private _Headerid As String
		 Private _Path As String
		 Public Property Headerid() As String
			 Get 
				 Return _Headerid
			 End Get
			 Set(ByVal value As String) 
				_Headerid = value
			 End Set
		 End Property
		 Public Property Path() As String
			 Get 
				 Return _Path
			 End Get
			 Set(ByVal value As String) 
				_Path = value
			 End Set
		 End Property
	End Class
End Namespace
