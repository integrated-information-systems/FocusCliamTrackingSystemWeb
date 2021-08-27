Imports System.IO

Partial Class Business_FileHandler
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Request.QueryString("FileName")) Then
            If User.Identity.IsAuthenticated = True And File.Exists(Server.MapPath("~") & "\_tmpUploadedfiles\" & Request.QueryString("FileName")) Then
                'Response.TransmitFile(Server.MapPath("~") & "\_tmpUploadedfiles\" & Request.QueryString("FileName"))
                'Response.Write()
                Response.Clear()
                Response.AddHeader("content-disposition", "attachment; filename=" & Request.QueryString("FileName"))
                Response.WriteFile(Server.MapPath("~") & "\_tmpUploadedfiles\" & Request.QueryString("FileName"))
                Response.ContentType = ""
                Response.End()
            Else
                Response.Redirect("~/Business", True)
            End If
        Else
            Response.Write("File Not exist")
        End If
    End Sub
End Class
