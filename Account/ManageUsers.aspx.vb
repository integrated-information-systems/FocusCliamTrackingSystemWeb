Imports System.Drawing

Partial Class ManageUsers
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If Not IsPostBack Then
        BindUsers()
        'End If
    End Sub
    Protected Sub BindUsers()
        UsersList.DataSource = Membership.GetAllUsers
        UsersList.DataBind()
    End Sub
    Protected Sub UsersList_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        Dim UserName As String = UsersList.Rows(e.RowIndex).Cells(1).Text

        If UserName.ToLower = "admin" Then
            lblMsg.Text = String.Format("You Cannot Delete {0} User", UserName)
            lblMsg.ForeColor = Color.Green
            lblMsg.Visible = True
        Else
            Dim SelectedUser As MembershipUser = Membership.GetUser(UserName)
            If Not IsNothing(SelectedUser) Then
                SelectedUser.IsApproved = False
                Membership.UpdateUser(SelectedUser)
            End If
            lblMsg.Text = String.Format("User ""{0}""  deleted Successfully", UserName)
            lblMsg.ForeColor = Color.Green
            lblMsg.Visible = True
            BindUsers()
        End If
    End Sub

    Protected Sub UsersList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles UsersList.RowDataBound
        If e.Row.RowIndex > -1 Then
            Dim MUser As MembershipUser = Membership.GetUser(e.Row.Cells(1).Text)
            If MUser.IsApproved = False Then
                e.Row.BackColor = Color.Red
                e.Row.ForeColor = Color.White
                e.Row.Cells(0).Text = String.Empty
            Else
                Dim lb As LinkButton = e.Row.Cells(0).Controls(0)
                lb.OnClientClick = "return confirm('Are you certain you want to delete?');"
            End If
          
        End If
    End Sub

    Protected Sub UsersList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles UsersList.SelectedIndexChanged
        lblMsg.Visible = False
        lblMsg.Text = ""
        
        UserName.Text = UsersList.Rows(UsersList.SelectedIndex).Cells(1).Text
        Dim MUser As MembershipUser = Membership.GetUser(UserName.Text)
        Dim UpdateProfile As ProfileCommon = Profile.GetProfile(UserName.Text)
        Dim CountryList As String = UpdateProfile.Country()
        LstCountry.ClearSelection()
        Dim SplittedCountry() As String = CountryList.Split("|")
        For Each i In SplittedCountry
            Dim index As Integer = LstCountry.Items.IndexOf(LstCountry.Items.FindByValue(i))
            If index <> -1 Then
                'Territory.SelectedIndex = Territory.Items.IndexOf(Territory.Items.FindByValue(i))
                LstCountry.Items.FindByValue(i).Selected = True
            End If
            'Territory.Items.FindByValue(i).Selected = True
        Next
        Email.Text = MUser.Email
        txtCompanyName.Text = UpdateProfile.CName

    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim MUser As MembershipUser = Membership.GetUser(UserName.Text)
        If Password.Text <> String.Empty Then
            MUser.UnlockUser()
            MUser.ChangePassword(MUser.ResetPassword(), Password.Text)
        End If


        MUser.Email = Email.Text

        Membership.UpdateUser(MUser)
        Dim UpdateProfile As ProfileCommon = Profile.GetProfile(UserName.Text)
        Dim CountryValues As String = String.Empty
        Dim index() As Integer = LstCountry.GetSelectedIndices()
        For Each i In index
            CountryValues = CountryValues & LstCountry.Items(i).Value & "|"
        Next
        UpdateProfile.Country = CountryValues
        UpdateProfile.CName = txtCompanyName.Text
        UpdateProfile.Save()
        Clear()
        lblMsg.Visible = True
        lblMsg.Text = "Updated Successfully"
        lblMsg.ForeColor = Color.Green
        BindUsers()
    End Sub
    Private Sub Clear()
        UsersList.SelectedIndex = -1
        UserName.Text = String.Empty
        Email.Text = String.Empty
        Password.Text = String.Empty
        ConfirmPassword.Text = String.Empty
        LstCountry.SelectedIndex = -1
    End Sub
End Class
