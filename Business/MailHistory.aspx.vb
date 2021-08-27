Imports System.Data.SqlClient

Partial Class Business_MailHistory
    Inherits System.Web.UI.Page

    Private Sub LoadMailHistory()
        If Not IsNothing(Session("Country")) Then
            Dim ContentHolder As Literal = New Literal
            ContentHolder = BubbleContent.FindControl("ContentHolder")

            Dim SqlCon As SqlConnection = New SqlConnection()
            SqlCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
            SqlCon.Open()
            Dim ReaderCommand As SqlCommand = New SqlCommand()
            ReaderCommand.Connection = SqlCon
            ReaderCommand.CommandText = "SELECT * FROM Mails INNER JOIN ClaimHeader ON ClaimHeader.ID=Mails.ClaimId Where ClaimId=@ClaimId  and  ClaimHeader.Country=@country ORDER BY Mails.CreatedOn DESC"
            ReaderCommand.Parameters.AddWithValue("@ClaimId", lblTrackingId.Text)
            ReaderCommand.Parameters.AddWithValue("@country", Session("Country"))
            Dim Internalreader As SqlDataReader = ReaderCommand.ExecuteReader()

            Dim MsgThread As String = String.Empty
            Dim PreviousUser As String = String.Empty
            Dim PreviousDirection As String = String.Empty
            While (Internalreader.Read)
                'If i Mod 2 = 0 Then
                '    MsgThread = MsgThread & "<blockquote class='example-left-odd'>"
                'Else
                '    MsgThread = MsgThread & "<blockquote class='example-right-odd'>"
                'End If
                'Dim pattern As String = "width=\""[0-9]*\"""
                'Dim rgx As New Regex(pattern)
                'Dim result As String = rgx.Replace(Internalreader("Body"), "width=""700""")            
                If PreviousUser = String.Empty Then
                    PreviousUser = Internalreader("FromUser").ToString
                End If
                If PreviousDirection = String.Empty Then
                    PreviousDirection = "left"
                End If

                If Not User.IsInRole("FocusLimited") Then

                    If Internalreader("FromUser").ToString = "Manufacturer" Then
                        Dim SameUser As Boolean = False
                        If PreviousUser <> Internalreader("FromUser").ToString Then
                            SameUser = False
                        Else
                            SameUser = True
                        End If

                        If SameUser = True Then
                            MsgThread = MsgThread & "<blockquote class='manufacturer-" & PreviousDirection & "'>"
                        Else
                            If PreviousDirection = "left" Then
                                PreviousDirection = "right"
                            Else
                                PreviousDirection = "left"
                            End If
                            MsgThread = MsgThread & "<blockquote class='manufacturer-" & PreviousDirection & "'>"
                        End If
                        PreviousUser = Internalreader("FromUser")
                    End If

                End If

                If User.IsInRole("FocusLimited") And (Internalreader("ToUser").ToString = "Manufacturer") Then

                Else
                    If Internalreader("FromUser").ToString = "Customer" Then
                        Dim SameUser As Boolean = False
                        If PreviousUser <> Internalreader("FromUser").ToString Then
                            SameUser = False
                        Else
                            SameUser = True
                        End If

                        If SameUser = True Then
                            MsgThread = MsgThread & "<blockquote class='customer-" & PreviousDirection & "'>"
                        Else
                            If PreviousDirection = "left" Then
                                PreviousDirection = "right"
                            Else
                                PreviousDirection = "left"
                            End If
                            MsgThread = MsgThread & "<blockquote class='customer-" & PreviousDirection & "'>"
                        End If
                        PreviousUser = Internalreader("FromUser")

                    End If
                End If


                If User.IsInRole("FocusLimited") And (Internalreader("ToUser").ToString = "Manufacturer") Then

                Else

                    If Internalreader("FromUser").ToString = "Focus" Then

                        Dim SameUser As Boolean = False
                        If PreviousUser <> Internalreader("FromUser").ToString Then
                            SameUser = False
                        Else
                            SameUser = True
                        End If

                        If SameUser = True Then
                            MsgThread = MsgThread & "<blockquote class='focus-" & PreviousDirection & "'>"
                        Else
                            If PreviousDirection = "left" Then
                                PreviousDirection = "right"
                            Else
                                PreviousDirection = "left"
                            End If
                            MsgThread = MsgThread & "<blockquote class='focus-" & PreviousDirection & "'>"
                        End If
                        PreviousUser = Internalreader("FromUser")

                    End If

                End If

                'MsgThread = MsgThread & result
                If User.IsInRole("FocusLimited") And (Internalreader("ToUser").ToString = "Manufacturer" Or Internalreader("FromUser").ToString = "Manufacturer") Then

                Else
                    MsgThread = MsgThread & Internalreader("Body")
                    MsgThread = MsgThread & "</blockquote>"
                    MsgThread = MsgThread & "<p>" & Internalreader("FromUser") & ", " & CDate(Internalreader("CreatedOn")).ToString("d MMM yyyy") & "</p>"
                End If

            End While
            ReaderCommand.Dispose()
            SqlCon.Close()
            ContentHolder.Text = MsgThread
        Else
            FormsAuthentication.SignOut()
        End If

        '        ContentHolder.Text = "<blockquote class='example-right'>     <p>Dear Mr. Williams:</p>" _
        '& "<p>Thank you for inquiring about our new email marketing enterprise application. A team member will contact you tomorrow with a detailed explanation of the product that fits your business need.</p>" _
        '& "<p>Thanks again for your inquiry.</p>" _
        '& "<p>Sincerely,</p>" _
        '& "<p>James Burton</p>" _
        '      & "</blockquote>" _
        '      & "<p>Ivan Chermayeff</p>"

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Session("Country")) Then
            If Not IsNothing(Session("ClaimId")) Then
                Dim ClaimId As Integer = 0
                If (Int32.TryParse(Session("ClaimId"), ClaimId)) Then
                    lblTrackingId.Text = ClaimId
                    LoadMailHistory()
                Else
                    Response.Redirect("~/Business/ClaimList.aspx", True)
                End If
            Else
                Response.Redirect("~/Business/ClaimList.aspx", True)
            End If
        Else
            FormsAuthentication.SignOut()
        End If
    End Sub
End Class
