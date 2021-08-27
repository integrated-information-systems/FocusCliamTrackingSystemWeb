Imports System.Data.SqlClient

Partial Class Business_SupplierMaster
    Inherits System.Web.UI.Page

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then
            If SupplierGrid.SelectedIndex < 0 Then
                Dim SqlCon As SqlConnection = New SqlConnection()
                SqlCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
                SqlCon.Open()
                Dim InsertCommand As SqlCommand = New SqlCommand()
                InsertCommand.Connection = SqlCon
                'ReaderCommand.CommandText = "SELECT id  AS  ClaimId FROM ClaimHeader Where id=@Headerid AND Status!=@status  and Country=@country"
                InsertCommand.CommandText = "INSERT INTO SupplierMaster Values(@SupplierName, @ContactPerson, @ContactNumber, @Email)"
                InsertCommand.Parameters.AddWithValue("@SupplierName", txtSupplierName.Text)
                InsertCommand.Parameters.AddWithValue("@ContactPerson", txtContactPerson.Text)
                InsertCommand.Parameters.AddWithValue("@ContactNumber", txtContactNumber.Text)
                InsertCommand.Parameters.AddWithValue("@Email", txtEmail.Text)
                InsertCommand.ExecuteNonQuery()
                InsertCommand.Dispose()
                SqlCon.Close()
                Clear_Form()
                SupplierGrid.DataBind()
            Else
                Dim SqlCon As SqlConnection = New SqlConnection()
                SqlCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
                SqlCon.Open()
                Dim InsertCommand As SqlCommand = New SqlCommand()
                InsertCommand.Connection = SqlCon
                'ReaderCommand.CommandText = "SELECT id  AS  ClaimId FROM ClaimHeader Where id=@Headerid AND Status!=@status  and Country=@country"
                InsertCommand.CommandText = "UPDATE SupplierMaster SET SupplierName=@SupplierName, ContactPerson=@ContactPerson, ContactNumber=@ContactNumber, Email=@Email WHERE IdKey=@IdKey"
                InsertCommand.Parameters.AddWithValue("@SupplierName", txtSupplierName.Text)
                InsertCommand.Parameters.AddWithValue("@ContactPerson", txtContactPerson.Text)
                InsertCommand.Parameters.AddWithValue("@ContactNumber", txtContactNumber.Text)
                InsertCommand.Parameters.AddWithValue("@Email", txtEmail.Text)
                InsertCommand.Parameters.AddWithValue("@IdKey", SupplierGrid.DataKeys(SupplierGrid.SelectedRow.RowIndex).Value)
                InsertCommand.ExecuteNonQuery()
                InsertCommand.Dispose()
                SqlCon.Close()
                Clear_Form()
                SupplierGrid.SelectedIndex = -1
                btnSave.Text = "Save"
                SupplierGrid.DataBind()
            End If
        End If
    End Sub

    Protected Sub btnClear_Click(sender As Object, e As System.EventArgs) Handles btnClear.Click
        Clear_Form()
    End Sub
    Protected Sub Clear_Form()
        txtSupplierName.Text = String.Empty
        txtContactNumber.Text = String.Empty
        txtContactPerson.Text = String.Empty
        txtEmail.Text = String.Empty
    End Sub
    Protected Sub SupplierGrid_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles SupplierGrid.SelectedIndexChanged
        If SupplierGrid.SelectedRow.RowIndex >= 0 Then
            Dim SelectedIndex As Integer = SupplierGrid.SelectedRow.RowIndex

            Dim SqlCon As SqlConnection = New SqlConnection()
            SqlCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
            SqlCon.Open()
            Dim ReaderCommand As SqlCommand = New SqlCommand()
            ReaderCommand.Connection = SqlCon
            ReaderCommand.CommandText = "Select SupplierName,ContactPerson,ContactNumber,Email from SupplierMaster Where IdKey=@IdKey "
            ReaderCommand.Parameters.AddWithValue("@IdKey", SupplierGrid.DataKeys(SupplierGrid.SelectedRow.RowIndex).Value)
            Dim DataRead As SqlDataReader = ReaderCommand.ExecuteReader
            If DataRead.Read() Then
                If Not IsDBNull(DataRead("SupplierName")) Then
                    txtSupplierName.Text = DataRead("SupplierName").ToString
                End If
                If Not IsDBNull(DataRead("ContactPerson")) Then
                    txtContactPerson.Text = DataRead("ContactPerson").ToString
                End If
                If Not IsDBNull(DataRead("ContactNumber")) Then
                    txtContactNumber.Text = DataRead("ContactNumber").ToString
                End If
                If Not IsDBNull(DataRead("Email")) Then
                    txtEmail.Text = DataRead("Email").ToString
                End If
            End If
            ReaderCommand.Dispose()
            SqlCon.Close()
            btnSave.Text = "Update"
        Else
            btnSave.Text = "Save"
        End If
    End Sub
End Class
