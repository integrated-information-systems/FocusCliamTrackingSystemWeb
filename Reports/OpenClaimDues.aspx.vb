
Partial Class Reports_OpenClaimDues
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Response.Write(Now.Date.ToString("yyyy-MM-dd"))     
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Response.Redirect("~/Reports/OpenClaimDues.aspx", True)
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        'Response.Write(ddldays.SelectedValue)
        If ddldays.SelectedValue = "<=7" Then
            SqlPendingClaims.SelectCommand = "SELECT [id], [CustomerName], [Personincharge], [DocDate],  [CountryName] FROM [ClaimHeader] WHERE DATEDIFF(day,CreatedOn,@DATE) <=@DueDays"
            SqlPendingClaims.SelectParameters.Item("DATE").DefaultValue = Now.Date.ToString("yyyy-MM-dd")
            SqlPendingClaims.SelectParameters.Item("DueDays").DefaultValue = 7
            SqlPendingClaims.DataBind()
        ElseIf ddldays.SelectedValue = ">7 AND <=14" Then
            SqlPendingClaims.SelectCommand = "SELECT [id], [CustomerName], [Personincharge], [DocDate],  [CountryName] FROM [ClaimHeader] WHERE DATEDIFF(day,CreatedOn,@DATE) > @DueDays AND DATEDIFF(day,CreatedOn,@DATE)<=14 "
            SqlPendingClaims.SelectParameters.Item("DATE").DefaultValue = Now.Date.ToString("yyyy-MM-dd")
            SqlPendingClaims.SelectParameters.Item("DueDays").DefaultValue = 7
            SqlPendingClaims.DataBind()
        ElseIf ddldays.SelectedValue = ">14 AND <=21" Then
            SqlPendingClaims.SelectCommand = "SELECT [id], [CustomerName], [Personincharge], [DocDate],  [CountryName] FROM [ClaimHeader] WHERE DATEDIFF(day,CreatedOn,@DATE) > @DueDays AND DATEDIFF(day,CreatedOn,@DATE)<=21 "
            SqlPendingClaims.SelectParameters.Item("DATE").DefaultValue = Now.Date.ToString("yyyy-MM-dd")
            SqlPendingClaims.SelectParameters.Item("DueDays").DefaultValue = 14
            SqlPendingClaims.DataBind()
        ElseIf ddldays.SelectedValue = ">21" Then
            SqlPendingClaims.SelectCommand = "SELECT [id], [CustomerName], [Personincharge], [DocDate],  [CountryName] FROM [ClaimHeader] WHERE DATEDIFF(day,CreatedOn,@DATE) > @DueDays"
            SqlPendingClaims.SelectParameters.Item("DATE").DefaultValue = Now.Date.ToString("yyyy-MM-dd")
            SqlPendingClaims.SelectParameters.Item("DueDays").DefaultValue = 21
            SqlPendingClaims.DataBind()
        End If
        'If ddldays.SelectedValue <= 9 Then
        '    SqlPendingClaims.SelectParameters.Item("DATE").DefaultValue = Now.Date.ToString("yyyy-MM-dd")
        '    SqlPendingClaims.SelectParameters.Item("DueDays").DefaultValue = ddldays.SelectedItem.Value
        '    SqlPendingClaims.DataBind()
        'ElseIf ddldays.SelectedIndex > 9 Then
        '    SqlPendingClaims.SelectCommand = "SELECT [id], [CustomerName], [Personincharge], [Date],  [CountryName] FROM [ClaimHeader] WHERE DATEDIFF(day,CreatedOn,@DATE) >=@DueDays"
        '    SqlPendingClaims.SelectParameters.Item("DATE").DefaultValue = Now.Date.ToString("yyyy-MM-dd")
        '    SqlPendingClaims.SelectParameters.Item("DueDays").DefaultValue = ddldays.SelectedItem.Value
        '    SqlPendingClaims.DataBind()
        'End If
    End Sub
End Class
