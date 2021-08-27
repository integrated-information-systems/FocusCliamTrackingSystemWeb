Imports Models
Partial Class Site
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(SiteMap.CurrentNode) Then
            Page.Title = SiteMap.CurrentNode.Title
        End If
        If IsNothing(Session("Country")) Then
            FormsAuthentication.SignOut()
        End If
    End Sub

    Protected Sub NavigationMenu_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles NavigationMenu.MenuItemClick

    End Sub
    Protected Sub HeadLoginStatus_LoggingOut(ByVal sender As Object, ByVal e As LoginCancelEventArgs)
        Try
            If Not IsNothing(Session("AccessPortal")) Then
                Dim AccessPortal As New List(Of String)

                AccessPortal = Session("AccessPortal")
                Dim i As Integer = 0
                For Each Item As String In AccessPortal
                    If AccessPortal.Item(i).Equals("ClaimPortalLive") Then
                        AccessPortal.RemoveAt(i)
                        Exit For
                    End If
                    i = i + 1
                Next

            End If
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
End Class

