Imports System.Data.SqlClient
Imports System.Data

Partial Class Business_NewEmail
    Inherits System.Web.UI.Page

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsNothing(Session("ClaimId")) Then
            Dim ClaimId As Integer = 0
            If (Int32.TryParse(Session("ClaimId"), ClaimId)) Then
                lblTrackingID.Text = ClaimId
                Dim SqlCon As SqlConnection = New SqlConnection()
                SqlCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
                SqlCon.Open()
                Dim LocalTempHeaderDataTable As New DataTable
                Dim LocalTempDataset As New DataSet

                Dim SqlAdapterHeader As _
                   New SqlDataAdapter("Select * From Mails", ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString)
                Dim UpdateFlag As Boolean = False

                Dim TempHeaderDataRow As DataRow = Nothing
                SqlAdapterHeader.FillSchema(LocalTempDataset, SchemaType.Source, "Mails")
                LocalTempHeaderDataTable = LocalTempDataset.Tables("Mails")
                TempHeaderDataRow = LocalTempDataset.Tables("Mails").NewRow()
                LocalTempHeaderDataTable.Rows.Add(TempHeaderDataRow)

                TempHeaderDataRow("ClaimId") = lblTrackingID.Text
                TempHeaderDataRow("FromUser") = ddlFrom.SelectedValue
                TempHeaderDataRow("ToUser") = ddlTo.SelectedValue
                'TempHeaderDataRow("FromMail") = "Focus@iis.com"
                'TempHeaderDataRow("ToMail") = "Focus@iis.com"
                TempHeaderDataRow("Date") = Format(CDate(DateTime.Now), "yyyy-MM-dd")
                TempHeaderDataRow("Body") = txtBody.Text
                TempHeaderDataRow("remainder") = ddlReminder.SelectedValue
                TempHeaderDataRow("CreatedOn") = Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss")

                Dim SCB As SqlCommandBuilder = New SqlCommandBuilder(SqlAdapterHeader)
                SqlAdapterHeader.Update(LocalTempDataset, "Mails")
                SqlAdapterHeader.Dispose()
                lblMsg.Text = "New mail entry added successfully"
                txtBody.Text = String.Empty
            Else
                lblMsg.Text = String.Empty
                lblTrackingID.Text = String.Empty
            End If
        Else
            Response.Redirect("~/Business/ClaimList.aspx", True)
        End If
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        txtBody.Text = String.Empty
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Session("ClaimId")) Then
            Dim ClaimId As Integer = 0
            If (Int32.TryParse(Session("ClaimId"), ClaimId)) Then
                lblTrackingID.Text = ClaimId
            Else
                Response.Redirect("~/Business/ClaimList.aspx", True)
            End If
        Else
            Response.Redirect("~/Business/ClaimList.aspx", True)
        End If

        txtDate.Text = Now.Date.ToString("dd-MM-yyyy")
    End Sub


    Protected Sub CheckEqual(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustomddlToValidator.ServerValidate
        If (ddlFrom.SelectedValue = ddlTo.SelectedValue) Then
            args.IsValid = False
        Else
            args.IsValid = True
        End If
    End Sub
End Class
