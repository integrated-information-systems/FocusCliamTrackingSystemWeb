Imports Models
Imports System.Data.SqlClient
Imports System.Data
Imports System.Reflection
Partial Class Account_Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString("ReturnUrl"))
        Try

            If Not IsPostBack Then

                Dim ConString As String = ConfigurationManager.ConnectionStrings("IntegPortal_ConnectionString").ToString
                Dim DB_Name As String = ConfigurationManager.AppSettings("DB_Name").ToString
                Dim ReturnResult As New DataTable
                Using SqlCon As New SqlConnection(ConString)
                    Dim Qry As String = "  Select T1.*, T2.UserName as 'ExistingUName' from Users T1 LEFT JOIN " & DB_Name & ".dbo.aspnet_Users T2 ON T1.UserName=T2.UserName "
                    SqlCon.Open()
                    Using cmd As New SqlCommand(Qry)
                        cmd.Connection = SqlCon
                        Dim SqlAdap As New SqlDataAdapter(cmd)
                        SqlAdap.Fill(ReturnResult)
                        SqlAdap.Dispose()
                    End Using

                End Using

                If Not IsNothing(ReturnResult) Then
                    For Each DRow As DataRow In ReturnResult.Rows
                        If IsDBNull(DRow.Item("ExistingUName")) Then
                            Membership.CreateUser(DRow.Item("UserName").ToString(), DRow.Item("Password").ToString(), DRow.Item("Email").ToString())
                            If Roles.RoleExists("NormalUser") Then
                                Roles.AddUserToRole(DRow.Item("UserName").ToString(), "NormalUser")
                            End If
                        Else
                            Dim MUser As MembershipUser = Membership.GetUser(DRow.Item("UserName").ToString())

                            If Not isnothing(MUser) Then
                                MUser.UnlockUser()
                                MUser.ChangePassword(MUser.ResetPassword, DRow.Item("Password").ToString())

                                MUser.Email = DRow.Item("Email").ToString()
                                ''MUser.IsApproved = True
                                Membership.UpdateUser(MUser)
                            End If

                        End If
                    Next
                End If



            End If



            If User.Identity.Name <> Nothing Then
                Response.Redirect("~")
            ElseIf Not IsNothing(Session("AccessPortal")) Then

                Dim AccessPortal As New List(Of String)

                AccessPortal = Session("AccessPortal")
                If AccessPortal.Contains("ClaimPortalLive") Then

                    If Not IsNothing(Session("Username")) Then

                        Dim UserCollect As New MembershipUserCollection
                        UserCollect = Membership.FindUsersByName(Session("Username"))
                        If UserCollect.Count <= 0 Then
                            Membership.CreateUser(Session("Username").ToString, Session("Email").ToString, Session("Password").ToString)
                            If Roles.RoleExists("NormalUser") Then
                                Roles.AddUserToRole(Session("Username").ToString, "NormalUser")
                            End If
                        End If



                        Dim Authenticated As Boolean = False

                        Authenticated = CheckCountryAssigned(Session("Username").ToString, Session("ClaimTerritoryLive").ToString)
                        If Authenticated Then
                            Session("CountryName") = GetCountryName(Session("ClaimTerritoryLive").ToString)
                            Session("Country") = Session("ClaimTerritoryLive").ToString
                            Session("ClaimId") = String.Empty
                        Else
                            Authenticated = False
                        End If

                        If Authenticated = True Then
                            Response.Cookies.Remove(FormsAuthentication.FormsCookieName)
                            FormsAuthentication.SetAuthCookie(Session("Username").ToString, False)
                            If Not IsNothing(Request.QueryString("ReturnUrl")) Then
                                Response.Redirect(Request.QueryString("ReturnUrl"))
                            Else
                                Response.Redirect("~")
                            End If
                        End If



                    End If
                End If
            End If

        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)

        End Try
    End Sub
    Function GetCountryName(ByVal Id As String) As String
        Dim ReturnResult As String = String.Empty

        If (Id <> String.Empty) Then
            Using SqlCon As New SqlConnection(ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString)


                SqlCon.Open()
                Dim Qry As String = "SELECT Country FROM Country WHERE id=@Value"

                Using cmd As New SqlCommand(Qry)
                    Dim DR As SqlDataReader
                    cmd.Connection = SqlCon
                    cmd.Parameters.AddWithValue("@Value", Id.ToLower)
                    DR = cmd.ExecuteReader()
                    If DR.Read Then
                        If Not IsDBNull(DR("Country")) Then
                            ReturnResult = DR("Country").ToString
                        End If
                    End If
                    DR.Close()
                End Using

            End Using
        End If

        Return ReturnResult
    End Function
    Function CheckCountryAssigned(ByVal UserName As String, ByVal Countryid As String) As Boolean
        Dim Authenticated As Boolean = False

        Dim UpdateProfile As ProfileCommon = Profile.GetProfile(UserName)
        Dim CountryList As String = UpdateProfile.Country()

        Dim SplittedCountry() As String = CountryList.Split("|")
        For Each i In SplittedCountry
            If i = Countryid Then
                Authenticated = True
                Exit For
            End If
        Next
        Return Authenticated
    End Function

    Protected Sub LoginUser_Authenticate(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.AuthenticateEventArgs) Handles LoginUser.Authenticate
        Dim Authenticated As Boolean = False
        Dim Country As DropDownList = LoginUser.FindControl("ddlCountry")
        Authenticated = CheckCountryAssigned(LoginUser.UserName, Country.SelectedValue)
        If Authenticated And Membership.ValidateUser(LoginUser.UserName, LoginUser.Password) Then
            Session("CountryName") = Country.SelectedItem.Text
            Session("Country") = Country.SelectedValue
            Session("ClaimId") = String.Empty
        Else
            Authenticated = False
        End If
        e.Authenticated = Authenticated
    End Sub
    Protected Sub LoginUser_LoggedIn(ByVal sender As Object, ByVal e As System.EventArgs) Handles LoginUser.LoggedIn
       
    End Sub
End Class