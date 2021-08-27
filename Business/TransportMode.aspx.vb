Imports System.Data.SqlClient

Partial Class Business_TransportMode
    Inherits System.Web.UI.Page

    Protected Sub btnClear_Click(sender As Object, e As System.EventArgs) Handles btnClear.Click
        txtTransModeName.Text = String.Empty
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As System.EventArgs) Handles btnSubmit.Click
        If Page.IsValid = True Then
            If GridView1.SelectedRow.RowIndex < 0 Then
                Dim SqlCon As SqlConnection = New SqlConnection()
                SqlCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
                SqlCon.Open()
                Dim SqlCom As SqlCommand = New SqlCommand()
                SqlCom.Connection = SqlCon
                SqlCom.CommandText = "Insert into TransportMode values(@ModeName)"
                SqlCom.Parameters.AddWithValue("@ModeName", txtTransModeName.Text)
                SqlCom.ExecuteNonQuery()
                SqlCom.Dispose()
                SqlCon.Close()
                SqlCon.Dispose()

                GridView1.DataBind()
            Else
                Dim SqlCon As SqlConnection = New SqlConnection()
                SqlCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
                SqlCon.Open()
                Dim InsertCommand As SqlCommand = New SqlCommand()
                InsertCommand.Connection = SqlCon
                'ReaderCommand.CommandText = "SELECT id  AS  ClaimId FROM ClaimHeader Where id=@Headerid AND Status!=@status  and Country=@country"
                InsertCommand.CommandText = "UPDATE TransportMode SET ModeName=@ModeName  WHERE TransModeId=@TransModeId"
                InsertCommand.Parameters.AddWithValue("@ModeName", txtTransModeName.Text)                 
                InsertCommand.Parameters.AddWithValue("@TransModeId", GridView1.DataKeys(GridView1.SelectedRow.RowIndex).Value)
                InsertCommand.ExecuteNonQuery()
                InsertCommand.Dispose()
                SqlCon.Close()
                txtTransModeName.Text = String.Empty
                GridView1.SelectedIndex = -1
                btnSubmit.Text = "Save"
                GridView1.DataBind()
            End If

        End If

    End Sub

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        If GridView1.SelectedRow.RowIndex >= 0 Then
            Dim SelectedIndex As Integer = GridView1.SelectedRow.RowIndex
            txtTransModeName.Text = GridView1.Rows(SelectedIndex).Cells(1).Text
            btnSubmit.Text = "Update"
        Else
            btnSubmit.Text = "Save"
        End If
    End Sub

    Protected Sub txtTransModeNameCustomValidator_ServerValidate(source As Object, args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles txtTransModeNameCustomValidator.ServerValidate
        'Dim SqlCon As SqlConnection = New SqlConnection()
        'SqlCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
        'SqlCon.Open()
        'Dim SqlCom As SqlCommand = New SqlCommand()
        'SqlCom.Connection = SqlCon
        'SqlCom.CommandText = "select ModeName from TransportMode where  ModeName=@ModeName"
        'SqlCom.Parameters.AddWithValue("@ModeName", txtTransModeName.Text)
        'Dim reader As SqlDataReader = SqlCom.ExecuteReader()


        'If reader.Read() Then
        '    args.IsValid = False
        'End If

        'SqlCom.Dispose()
        'SqlCon.Close()
        'SqlCon.Dispose()
    End Sub
End Class
