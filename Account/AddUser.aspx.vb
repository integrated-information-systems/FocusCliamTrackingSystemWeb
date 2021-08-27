
Partial Class Account_AddUser
    Inherits System.Web.UI.Page

    Protected Sub CreateUserWizard1_CreatedUser(ByVal sender As Object, ByVal e As System.EventArgs) Handles CreateUserWizard1.CreatedUser
        Dim Username As TextBox = CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("Username")
        Dim Country As ListBox = CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("LstCountry")
        Dim Company As TextBox = CreateUserWizard1.CreateUserStep.ContentTemplateContainer.FindControl("txtCompanyName")
        Dim CountryValues As String = String.Empty
        Dim MembershipUser1 As MembershipUser = Membership.GetUser(Username.Text)
        Dim Userid As Object = MembershipUser1.ProviderUserKey
        Dim anonymousProfile As ProfileCommon = Profile.GetProfile(Username.Text)

        Dim index() As Integer = Country.GetSelectedIndices()
        For Each i In index
            CountryValues = CountryValues & Country.Items(i).Value & "|"
        Next
        anonymousProfile.CName = Company.Text
        anonymousProfile.Country = CountryValues
        anonymousProfile.Save()
    End Sub
End Class
