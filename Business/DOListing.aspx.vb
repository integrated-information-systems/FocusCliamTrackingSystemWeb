Imports System.Globalization

Partial Class Business_DOListing
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Session("Country")) Then
            If Roles.IsUserInRole(User.Identity.Name, "Focus") Or Roles.IsUserInRole(User.Identity.Name, "FocusLimitedAdmin") Then
                SqlPendingClaims.SelectParameters.Item("Country").DefaultValue = Session("Country")
                SqlPendingClaims.DataBind()
            Else

                SqlPendingClaims.SelectParameters.Clear()
                SqlPendingClaims.SelectCommand = "SELECT x.CreatedOn as 'RemarksUpdatedOn' ,T1.* FROM [DOHeader] T1 OUTER APPLY(Select Top 1 CreatedOn from DORemarks T2 WHERE T1.ID=T2.ClaimId Order By T2.CreatedOn DESC) x WHERE T1.Country=@country and T1.CreatedBy=@CreatedBy"
                SqlPendingClaims.SelectParameters.Add("CreatedBy", User.Identity.Name)
                SqlPendingClaims.SelectParameters.Add("country", Session("Country"))
                SqlPendingClaims.DataBind()
            End If

        Else
            FormsAuthentication.SignOut()
        End If
    End Sub

    Protected Sub DOLists_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles DOLists.RowCommand
        If e.CommandName = "CustomEdit" Then
            Me.Context.Items.Add("DOId", e.CommandArgument)
            Server.Transfer("~/Business/DeliveryOrder.aspx", True)
        End If
    End Sub

    Protected Sub DOLists_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles DOLists.RowDataBound
        If User.IsInRole("Focus") Then
            DOLists.Columns(DOLists.Columns.Count - 1).Visible = True
        End If
    End Sub
    Protected Sub btnClear_Click(sender As Object, e As System.EventArgs) Handles btnClear.Click
        txtFromDate.Text = ""
        txtToDate.Text = ""
        ddlStatus.SelectedIndex = 0
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As System.EventArgs) Handles btnSearch.Click
        If Page.IsValid Then
            SqlPendingClaims.SelectParameters.Clear()
            SqlPendingClaims.SelectCommand = "SELECT x.CreatedOn as 'RemarksUpdatedOn' ,T1.* FROM [DOHeader] T1 OUTER APPLY(Select Top 1 CreatedOn from DORemarks T2 WHERE T1.ID=T2.ClaimId Order By T2.CreatedOn DESC) x WHERE T1.Country=@country"

            SqlPendingClaims.SelectParameters.Add("country", Session("Country"))

            If Not Roles.IsUserInRole(User.Identity.Name, "Focus") And Not Roles.IsUserInRole(User.Identity.Name, "FocusLimitedAdmin") Then
                SqlPendingClaims.SelectCommand = SqlPendingClaims.SelectCommand & "  and CreatedBy=@CreatedBy"
                SqlPendingClaims.SelectParameters.Add("CreatedBy", User.Identity.Name)
            End If


            If ddlStatus.SelectedIndex > 0 Then
                SqlPendingClaims.SelectCommand = SqlPendingClaims.SelectCommand & " AND status=@status"
                SqlPendingClaims.SelectParameters.Add("status", ddlStatus.SelectedItem.Value)
            End If
            If txtFromDate.Text <> String.Empty Then

                Dim Today As Date = Now.Date
                If Date.TryParseExact(txtFromDate.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, Today) Then
                    SqlPendingClaims.SelectCommand = SqlPendingClaims.SelectCommand & " AND Date>=@FromDate"
                    SqlPendingClaims.SelectParameters.Add("FromDate", Today.Date.ToString("yyyy-MM-dd"))
                End If

            End If
            If txtToDate.Text <> String.Empty Then
                Dim Today As Date = Now.Date
                If Date.TryParseExact(txtToDate.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, Today) Then
                    SqlPendingClaims.SelectCommand = SqlPendingClaims.SelectCommand & " AND Date<=@ToDate"
                    SqlPendingClaims.SelectParameters.Add("ToDate", Today.Date.ToString("yyyy-MM-dd"))
                End If

            End If

            'Response.write(SelectCommand)
            SqlPendingClaims.DataBind()
        End If
    End Sub
    Protected Sub FromDateValidator_ServerValidate(source As Object, args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles FromDateValidator.ServerValidate
        Dim Today As Date = Now.Date
        If txtFromDate.Text <> String.Empty Then
            If Not Date.TryParseExact(txtFromDate.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, Today) Then
                args.IsValid = False
            End If
        End If

    End Sub

    Protected Sub ToDateValidator_ServerValidate(source As Object, args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles ToDateValidator.ServerValidate
        Dim Today As Date = Now.Date
        If txtToDate.Text <> String.Empty Then
            If Not Date.TryParseExact(txtToDate.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, Today) Then
                args.IsValid = False
            End If
        End If
    End Sub
End Class
