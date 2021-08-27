Imports System.Drawing
Imports System.Net.Mail
Partial Class Account_ManageRoles
    Inherits System.Web.UI.Page
    Protected Sub CheckExist(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        If Roles.RoleExists(txtRoleName.Text) Then
            e.IsValid = False
        End If
    End Sub
    Protected Sub RolesGrid_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        Dim RoleNamelbl As Label = RolesGrid.Rows(e.RowIndex).Cells(1).FindControl("lblRoleName")
        Dim RoleName As String = RoleNamelbl.Text
        If RoleName.ToLower = "admin" Then
            lblMsg.Text = String.Format("You cannot delete {0} Role", RoleName)
            lblMsg.ForeColor = Color.Green
            lblMsg.Visible = True
        Else
            If Roles.RoleExists(RoleName) Then
                Roles.DeleteRole(RoleName)
            End If
            lblMsg.Text = String.Format("Role ""{0}"" deleted Successfully", RoleName)
            lblMsg.ForeColor = Color.Green
            lblMsg.Visible = True
            LoadRoles()
        End If
    End Sub
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        If Not Roles.RoleExists(txtRoleName.Text) Then
            Roles.CreateRole(txtRoleName.Text)
            Clear()
            LoadRoles()
        End If
    End Sub
    Protected Sub LoadRoles()
        Dim rolesArray() As String
        rolesArray = Roles.GetAllRoles()
        RolesGrid.DataSource = rolesArray
        RolesGrid.DataBind()
    End Sub
    Protected Sub Clear()
        txtRoleName.Text = String.Empty
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

       

        If IsPostBack = True Then

            ''Validate()
            'If Page.IsValid = True Then
            lblMsg.Visible = True
            lblMsg.Text = "Role Successfully Added"
            lblMsg.ForeColor = Color.Green
            'Else
            '    lblMsg.Visible = False
            '    lblMsg.Text = ""
            'End If
            'Else

        End If
        ' Bind roles to GridView.

        LoadRoles()
    End Sub

    Protected Sub RolesGrid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles RolesGrid.RowDataBound
        If e.Row.RowIndex >= 0 Then
            If Not Roles.IsUserInRole(User.Identity.Name, "admin") Then
                e.Row.Cells(0).Text = String.Empty
            End If
        End If
    End Sub

    Protected Sub RolesGrid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RolesGrid.SelectedIndexChanged

    End Sub
End Class
