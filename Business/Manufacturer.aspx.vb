Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Globalization
Imports System.Net.Mail
Partial Class Business_Manufacturer
    Inherits System.Web.UI.Page
    Dim Dtable As DataTable
    'Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
    '    'UpdateDTwithGridviewData()
    '    If Not IsNothing(Session("TempDataTable")) Then
    '        Dim TempDataTable As New DataTable
    '        TempDataTable = Session("TempDataTable")
    '        Dim DestinationRow As DataRow = TempDataTable.NewRow()
    '        TempDataTable.Rows.Add(DestinationRow)
    '        Session("TempDataTable") = TempDataTable
    '        LoadGridData()
    '    Else
    '        FormsAuthentication.SignOut()
    '    End If
    'End Sub
    Private Function GenerateClaimID() As Integer
        Dim ClaimId As Integer = 0
        Dim SqlCon As SqlConnection = New SqlConnection()
        SqlCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
        SqlCon.Open()
        Dim ReaderCommand As SqlCommand = New SqlCommand()
        ReaderCommand.Connection = SqlCon
        ReaderCommand.CommandText = "SELECT ISNULL(MAX(id),0)+1 AS  ClaimId FROM ClaimHeader"
        Dim Internalreader As String = ReaderCommand.ExecuteScalar()
        If IsNumeric(Internalreader) Then
            ClaimId = CInt(Internalreader)
        End If

        ReaderCommand.Dispose()
        SqlCon.Close()
        Return ClaimId
    End Function
    Private Function CheckClaimIDExist(ByVal Headerid As Integer) As Boolean
        Dim ClaimFlag As Boolean = False
        Dim SqlCon As SqlConnection = New SqlConnection()
        SqlCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
        SqlCon.Open()
        Dim ReaderCommand As SqlCommand = New SqlCommand()
        ReaderCommand.Connection = SqlCon
        ReaderCommand.CommandText = "SELECT id  AS  ClaimId FROM ClaimHeader Where id=@Headerid AND Status!=@status   and Country=@country"
        ReaderCommand.Parameters.AddWithValue("@Headerid", Headerid)
        ReaderCommand.Parameters.AddWithValue("@status", "Complete")
        ReaderCommand.Parameters.AddWithValue("@country", Session("Country"))
        Dim Internalreader As String = ReaderCommand.ExecuteScalar()
        If IsNumeric(Internalreader) Then
            ClaimFlag = True
        End If
        ReaderCommand.Dispose()
        SqlCon.Close()
        Return ClaimFlag
    End Function
    'Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click
    '    'UpdateDTwithGridviewData()
    '    If ClaimItemList.SelectedIndex >= 0 And Not IsNothing(Session("TempDataTable")) Then
    '        Dim TempDataTable As New DataTable
    '        TempDataTable = Session("TempDataTable")
    '        Dim DestinationRow As DataRow = TempDataTable.NewRow()
    '        Dim SourceRow As DataRow = TempDataTable.Rows(ClaimItemList.SelectedIndex)
    '        DestinationRow.ItemArray = SourceRow.ItemArray
    '        TempDataTable.Rows.InsertAt(DestinationRow, ClaimItemList.SelectedIndex + 1)
    '        Session("TempDataTable") = TempDataTable
    '        LoadGridData()
    '        ClaimItemList.SelectedIndex = -1
    '    Else
    '        FormsAuthentication.SignOut()
    '    End If
    'End Sub
    Private Sub LoadGridData()
        If Not IsNothing(Session("TempDataTable")) Then
            ClaimItemList.DataSource = Session("TempDataTable")
            ClaimItemList.DataBind()
        Else
            FormsAuthentication.SignOut()
        End If
    End Sub
    Protected Sub InitializeTempDataTable()
        Dim TempDataset As New DataSet
        Dim TempDataTable As New DataTable
        Dim UploadedFilesList As List(Of String) = New List(Of String)
        Dim SelectedRow As Integer = -1
        Dim HavingChildItems As Boolean = False
        Session("FilesList") = UploadedFilesList
        Session("TempDataset") = TempDataset
        Session("TempDataTable") = TempDataTable
        Session("SelectedRow") = SelectedRow
        If Not IsNothing(Session("TempDataTable")) Then
            Session("TempDataTable").Rows.Clear()
        End If
        Dim SqlAdapter As _
           New SqlDataAdapter("Select [SNo],[InvDate],[InvNo],[Qty],[Brand],[ModelNo],[SerialNo],[ClaimQty],[Particulars],[ClaimMode],[ClaimDate],[ClaimReference],[PartNo],[DeclareValue],[DeclareDescription] From ClaimLineItems", ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString)

        SqlAdapter.FillSchema(Session("TempDataset"), SchemaType.Source, "ClaimLineItems")
        Session("TempDataTable") = Session("TempDataset").Tables("ClaimLineItems")
        'TempDataTable = Session("TempDataTable")

        'TempDataTable = Session("TempDataTable")
        'Dim workRow As DataRow = TempDataTable.NewRow()        
        'TempDataTable.Rows.Add(workRow)

        ClaimItemList.DataSource = Session("TempDataTable")
        ClaimItemList.DataBind()
        SqlAdapter.Dispose()
        '    ModifyGridViewHeader()

    End Sub
    'Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
    '    If ClaimItemList.SelectedIndex >= 0 Then
    '        ClaimItemList.DeleteRow(ClaimItemList.SelectedIndex)
    '        If Not IsNothing(Session("TempDataTable")) Then
    '            Dim TempDatatable As DataTable = New DataTable
    '            TempDatatable = Session("TempDataTable")
    '            TempDatatable.Rows.RemoveAt(ClaimItemList.SelectedIndex)
    '            Session("TempDataTable") = TempDatatable
    '        Else
    '            FormsAuthentication.SignOut()
    '        End If
    '    End If
    'End Sub
    Private Sub LoadExistingCliam(ByVal HeaderId As Integer)

        Dim SqlCon As SqlConnection = New SqlConnection()
        SqlCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
        SqlCon.Open()
        Dim SqlCom As SqlCommand = New SqlCommand()
        SqlCom.Connection = SqlCon
        SqlCom.CommandText = "Select * from ClaimHeader Where id=@HeaderId  AND Status!=@status   and Country=@country"
        SqlCom.Parameters.AddWithValue("@HeaderId", HeaderId)
        SqlCom.Parameters.AddWithValue("@status", "Complete")
        SqlCom.Parameters.AddWithValue("@country", Session("Country"))

        Dim HeaderReader As SqlDataReader = SqlCom.ExecuteReader

        If HeaderReader.Read() Then

            txtClaimId.Text = HeaderId

            If Not IsDBNull(HeaderReader("CustomerName")) Then
                txtEndCustomerName.Text = HeaderReader("CustomerName")
            End If
            If Not IsDBNull(HeaderReader("Personincharge")) Then
                txtPersonincharge.Text = HeaderReader("Personincharge")
            End If
            If Not IsDBNull(HeaderReader("EmailAddress")) Then
                txtEmailAddress.Text = HeaderReader("EmailAddress")
            End If
            If Not IsDBNull(HeaderReader("PhoneNo")) Then
                txtTelno.Text = HeaderReader("PhoneNo")
            End If
            If Not IsDBNull(HeaderReader("DocDate")) Then
                Dim Today As Date = HeaderReader("DocDate")
                txtDate.Text = Today.Date.ToString("dd-MM-yyyy")
            End If
            If Not IsDBNull(HeaderReader("CountryName")) Then
                txtCountry.Text = HeaderReader("CountryName")
            End If
            If Not IsDBNull(HeaderReader("ShipToCompany")) Then
                txtShipToCompany.Text = HeaderReader("ShipToCompany")
            End If
            If Not IsDBNull(HeaderReader("ShipToAddress")) Then
                txtShipToAddress.Text = HeaderReader("ShipToAddress")
            End If
            If Not IsDBNull(HeaderReader("ShipToContact")) Then
                txtShipToContactPerson.Text = HeaderReader("ShipToContact")
            End If
            If Not IsDBNull(HeaderReader("EmailAddress")) Then
                txtShipToTelNo.Text = HeaderReader("ShipToPhone")
            End If

            If Not IsDBNull(HeaderReader("Brand")) Then
                txtBrand.Text = HeaderReader("Brand")
            End If

            If Not IsDBNull(HeaderReader("ModelNo")) Then
                txtMachineModelNo.Text = HeaderReader("ModelNo")
            End If

            If Not IsDBNull(HeaderReader("Status")) Then
                ddlStatus.SelectedValue = HeaderReader("Status")
            End If

            If Not IsDBNull(HeaderReader("youtubelink1")) Then
                txtYoutubeLink1.Text = HeaderReader("youtubelink1")
            End If

            If Not IsDBNull(HeaderReader("youtubelink2")) Then
                txtYoutubeLink2.Text = HeaderReader("youtubelink2")
            End If

            If Not IsDBNull(HeaderReader("youtubelink3")) Then
                txtYoutubeLink3.Text = HeaderReader("youtubelink3")
            End If

        End If
        HeaderReader.Close()
        SqlCom.Dispose()

        Dim SqlItemCom As SqlCommand = New SqlCommand()
        SqlItemCom.Connection = SqlCon
        SqlItemCom.CommandText = "Select * from ClaimLineItems Where Headerid=@HeaderId"
        SqlItemCom.Parameters.AddWithValue("@HeaderId", HeaderId)

        Dim ItemsReader As SqlDataReader = SqlItemCom.ExecuteReader
        Dim TempDataTable As New DataTable
        TempDataTable = Session("TempDataTable")
        While ItemsReader.Read()
            Dim DestinationRow As DataRow = TempDataTable.NewRow()
            TempDataTable.Rows.Add(DestinationRow)
            If Not IsDBNull(ItemsReader("SNo")) Then
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("SNo") = ItemsReader("SNo")
            Else
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("SNo") = TempDataTable.Rows.Count
            End If
            If Not IsDBNull(ItemsReader("InvDate")) Then
                Dim Today As Date = ItemsReader("InvDate")
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("InvDate") = Today
            End If


            If Not IsDBNull(ItemsReader("InvNo")) Then
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("InvNo") = ItemsReader("InvNo")
            Else
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("InvNo") = String.Empty
            End If

            If Not IsDBNull(ItemsReader("Qty")) Then
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("Qty") = ItemsReader("Qty")
            Else
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("Qty") = String.Empty
            End If

            If Not IsDBNull(ItemsReader("Brand")) Then
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("Brand") = ItemsReader("Brand")
            Else
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("Brand") = String.Empty
            End If

            If Not IsDBNull(ItemsReader("ModelNo")) Then
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("ModelNo") = ItemsReader("ModelNo")
            Else
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("ModelNo") = String.Empty
            End If

            If Not IsDBNull(ItemsReader("SerialNo")) Then
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("SerialNo") = ItemsReader("SerialNo")
            Else
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("SerialNo") = String.Empty
            End If

            If Not IsDBNull(ItemsReader("ClaimQty")) Then
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("ClaimQty") = ItemsReader("ClaimQty")
            Else
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("ClaimQty") = String.Empty
            End If

            If Not IsDBNull(ItemsReader("Particulars")) Then
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("Particulars") = ItemsReader("Particulars")
            Else
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("Particulars") = String.Empty
            End If

            If Not IsDBNull(ItemsReader("ClaimMode")) Then
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("ClaimMode") = ItemsReader("ClaimMode")
            Else
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("ClaimMode") = String.Empty
            End If

            If Not IsDBNull(ItemsReader("ClaimDate")) Then
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("ClaimDate") = ItemsReader("ClaimDate")
            Else
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("ClaimDate") = String.Empty
            End If

            If Not IsDBNull(ItemsReader("ClaimReference")) Then
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("ClaimReference") = ItemsReader("ClaimReference")
            Else
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("ClaimReference") = String.Empty
            End If

            If Not IsDBNull(ItemsReader("PartNo")) Then
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("PartNo") = ItemsReader("PartNo")
            Else
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("PartNo") = String.Empty
            End If


            If Not IsDBNull(ItemsReader("DeclareValue")) Then
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("DeclareValue") = ItemsReader("DeclareValue")
            Else
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("DeclareValue") = String.Empty
            End If

            If Not IsDBNull(ItemsReader("DeclareDescription")) Then
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("DeclareDescription") = ItemsReader("DeclareDescription")
            Else
                TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("DeclareDescription") = String.Empty
            End If

        End While

        Session("TempDataTable") = TempDataTable

        ItemsReader.Close()
        SqlItemCom.Dispose()

        Dim SqlFilesCom As SqlCommand = New SqlCommand()
        SqlFilesCom.Connection = SqlCon
        SqlFilesCom.CommandText = "Select * from Files Where Headerid=@HeaderId"
        SqlFilesCom.Parameters.AddWithValue("@HeaderId", HeaderId)

        Dim FilesReader As SqlDataReader = SqlFilesCom.ExecuteReader
        Dim UploadedFilesList As List(Of String) = New List(Of String)
        UploadedFilesList = Session("FilesList")
        While FilesReader.Read()
            UploadedFilesList.Add(FilesReader("Path"))
            UploadedFiles.Items.Add(System.IO.Path.GetFileName(FilesReader("Path")) & " - Remove")
        End While
        Session("FilesList") = UploadedFilesList
        'UploadedFiles.Enabled = False
        FilesReader.Close()
        SqlFilesCom.Dispose()
        SqlCon.Close()
        LoadGridData()
        

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsNothing(Session("Country")) Then

            If IsNothing(Session("ClaimId")) Then
                Response.Redirect("~/Business/ClaimList.aspx", True)
            End If

            If Not IsPostBack Then
                InitializeTempDataTable()
                initializeClaimForm()
            End If

            UpdateDTwithGridviewData()

            If Not IsNothing(Session("ClaimId")) And Not IsPostBack Then
                Dim ClaimId As Integer = 0
                If (Int32.TryParse(Session("ClaimId"), ClaimId)) Then
                    txtClaimId.Text = ClaimId
                    If CheckClaimIDExist(ClaimId) Then
                        InitializeTempDataTable()
                        LoadExistingCliam(ClaimId)
                        MakeFormReadOnly()
                    Else
                        lblMsg.Text = "Claim Not exist"
                        lblMsg.ForeColor = Drawing.Color.Red
                    End If
                    'Else
                    '    Response.Redirect("~/Business/ClaimList.aspx", True)
                End If
            Else
                'Response.Redirect("~/Business/ClaimList.aspx", True)
            End If

           
        Else
            FormsAuthentication.SignOut()
        End If

        'Dim mailMessage As New System.Net.Mail.MailMessage()
        'mailMessage.[To].Add("bharathiraja.be@gmail.com")
        'mailMessage.From = New MailAddress("braja.b@integratedinfosystem.com")
        'mailMessage.Subject = "ASP.NET e-mail test"
        'mailMessage.Body = "Hello world," & vbLf & vbLf & "This is an ASP.NET test e-mail!"
        'Dim smtpClient As New SmtpClient("mail.integratedinfosystem.com")
        'smtpClient.Send(mailMessage)
        'Response.Write("E-mail sent!")
    End Sub
    Private Sub MakeFormReadOnly()
        btnSave.Enabled = True
        btnClear.Enabled = False
        btnUpload.Enabled = False
        UploadedFiles.Enabled = False
        txtEndCustomerName.ReadOnly = True
        txtPersonincharge.ReadOnly = True
        txtEmailAddress.ReadOnly = True
        txtTelno.ReadOnly = True
        txtDate.ReadOnly = True
        txtCountry.ReadOnly = True
        ddlStatus.Enabled = False
        ClaimItemList.Enabled = True
        txtShipToAddress.ReadOnly = True
        txtShipToCompany.ReadOnly = True
        txtShipToContactPerson.ReadOnly = True
        txtShipToTelNo.ReadOnly = True
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub
    Private Sub initializeClaimForm()
        txtClaimId.Text = GenerateClaimID()
        txtDate.Text = Now.Date.ToString("dd-MM-yyyy")
        txtCountry.Text = Session("CountryName")
    End Sub
    Private Sub ReassignSerialNo()
        For i As Integer = 1 To ClaimItemList.Rows.Count
            Dim SNo As TextBox = ClaimItemList.Rows(i - 1).Cells(1).FindControl("SNo")
            SNo.Text = i
            Dim InvDate As TextBox = ClaimItemList.Rows(i - 1).Cells(2).FindControl("InvDate")
            Dim Today As Date = Now.Date
            If Date.TryParse(InvDate.Text, Today) Then
                InvDate.Text = Today.Date.ToString("dd-MM-yyyy")
            End If

        Next
    End Sub
    Protected Sub ClaimItemList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles ClaimItemList.RowDataBound
        ReassignSerialNo()
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ClaimMode As DropDownList = e.Row.Cells(7).FindControl("ClaimMode")

            If Not IsNothing(Session("TempDataTable")) Then

                Dim TempDataTable As New DataTable
                TempDataTable = Session("TempDataTable")
                If ClaimMode.Items.IndexOf(ClaimMode.Items.FindByValue(TempDataTable.Rows(e.Row.RowIndex).Item("ClaimMode"))) Then
                    ClaimMode.SelectedIndex = ClaimMode.Items.IndexOf(ClaimMode.Items.FindByValue(TempDataTable.Rows(e.Row.RowIndex).Item("ClaimMode")))
                End If

            End If


        End If

    End Sub
    Private Sub UpdateDTwithGridviewData()
        If Not IsNothing(Session("TempDataTable")) Then


            Dim TempDataTable As New DataTable
            TempDataTable = Session("TempDataTable")
            For Each row As GridViewRow In ClaimItemList.Rows
                Dim SNo As TextBox = row.Cells(1).FindControl("SNo")              
                Dim Brand As TextBox = row.Cells(4).FindControl("Brand")
                Dim ModelNo As TextBox = row.Cells(5).FindControl("ModelNo")
                Dim SerialNo As TextBox = row.Cells(6).FindControl("SerialNo")
                Dim ClaimQty As TextBox = row.Cells(7).FindControl("ClaimQty")
                Dim Particulars As TextBox = row.Cells(8).FindControl("Particulars")
                Dim DeclareValue As TextBox = row.Cells(9).FindControl("DeclareValue")
                Dim DeclareDescription As TextBox = row.Cells(10).FindControl("DeclareDescription")
                Dim ClaimMode As DropDownList = row.Cells(11).FindControl("ClaimMode")
                Dim ClaimDate As TextBox = row.Cells(12).FindControl("ClaimDate")
                Dim ClaimReference As TextBox = row.Cells(13).FindControl("ClaimReference")


                'Dim Today As Date = Now.Date
                'If Date.TryParseExact(InvDate.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, Today) Then
                '    TempDataTable.Rows(row.RowIndex).Item("InvDate") = Today.Date.ToString("dd-MM-yyyy")
                'End If
                TempDataTable.Rows(row.RowIndex).Item("SNo") = SNo.Text
                'TempDataTable.Rows(row.RowIndex).Item("InvNo") = InvNo.Text
                'TempDataTable.Rows(row.RowIndex).Item("Qty") = Qty.Text
                'TempDataTable.Rows(row.RowIndex).Item("Brand") = Brand.Text
                'TempDataTable.Rows(row.RowIndex).Item("ModelNo") = ModelNo.Text

                TempDataTable.Rows(row.RowIndex).Item("Brand") = txtBrand.Text
                TempDataTable.Rows(row.RowIndex).Item("ModelNo") = txtMachineModelNo.Text

                TempDataTable.Rows(row.RowIndex).Item("SerialNo") = SerialNo.Text
                TempDataTable.Rows(row.RowIndex).Item("ClaimQty") = ClaimQty.Text
                TempDataTable.Rows(row.RowIndex).Item("Particulars") = Particulars.Text
                'TempDataTable.Rows(row.RowIndex).Item("ClaimMode") = ClaimMode.Text
                TempDataTable.Rows(row.RowIndex).Item("ClaimMode") = ClaimMode.SelectedItem.Value
                TempDataTable.Rows(row.RowIndex).Item("ClaimDate") = ClaimDate.Text
                TempDataTable.Rows(row.RowIndex).Item("ClaimReference") = ClaimReference.Text

                TempDataTable.Rows(row.RowIndex).Item("DeclareValue") = DeclareValue.Text
                TempDataTable.Rows(row.RowIndex).Item("DeclareDescription") = DeclareDescription.Text
            Next
            Session("TempDataTable") = TempDataTable
        Else
            FormsAuthentication.SignOut()
        End If
    End Sub
    Private Function CreateClaim() As Integer
        If Not IsNothing(Session("Country")) Then

            Dim SqlCon As SqlConnection = New SqlConnection()
            SqlCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
            SqlCon.Open()
            Dim LocalTempHeaderDataTable As New DataTable
            Dim LocalTempDataset As New DataSet

            Dim SqlAdapterHeader As _
               New SqlDataAdapter("Select * From ClaimHeader", ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString)
            Dim UpdateFlag As Boolean = False

            Dim TempHeaderDataRow As DataRow = Nothing
            If CheckClaimIDExist(txtClaimId.Text) Then
                'SqlAdapterHeader.FillSchema(LocalTempDataset, SchemaType.Source, "ClaimHeader")
                SqlAdapterHeader.MissingSchemaAction = MissingSchemaAction.AddWithKey
                SqlAdapterHeader.Fill(LocalTempHeaderDataTable)
                TempHeaderDataRow = LocalTempHeaderDataTable.Rows.Find(txtClaimId.Text)
                UpdateFlag = True
            Else
                SqlAdapterHeader.FillSchema(LocalTempDataset, SchemaType.Source, "ClaimHeader")
                LocalTempHeaderDataTable = LocalTempDataset.Tables("ClaimHeader")
                TempHeaderDataRow = LocalTempDataset.Tables("ClaimHeader").NewRow()
                LocalTempHeaderDataTable.Rows.Add(TempHeaderDataRow)
            End If

            TempHeaderDataRow("CustomerName") = txtEndCustomerName.Text
            TempHeaderDataRow("Personincharge") = txtPersonincharge.Text
            TempHeaderDataRow("EmailAddress") = txtEmailAddress.Text
            TempHeaderDataRow("PhoneNo") = txtTelno.Text
            If UpdateFlag = True Then
                Dim Today As Date = Now.Date
                If Date.TryParseExact(txtDate.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, Today) Then
                    TempHeaderDataRow("DocDate") = Today.Date.ToString("yyyy-MM-dd")
                End If
                'TempHeaderDataRow("Date") = Format(CDate(txtDate.Text), "yyyy-MM-dd")
            Else
                TempHeaderDataRow("DocDate") = Format(CDate(DateTime.Now), "yyyy-MM-dd")
            End If

            TempHeaderDataRow("Country") = Session("Country")
            TempHeaderDataRow("CountryName") = Session("CountryName")
            TempHeaderDataRow("ShipToCompany") = txtShipToCompany.Text
            TempHeaderDataRow("ShipToAddress") = txtShipToAddress.Text
            TempHeaderDataRow("ShipToContact") = txtShipToContactPerson.Text
            TempHeaderDataRow("ShipToPhone") = txtShipToTelNo.Text





            Dim SCB As SqlCommandBuilder = New SqlCommandBuilder(SqlAdapterHeader)
            If UpdateFlag = False Then
                TempHeaderDataRow("CreatedOn") = Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss")
                TempHeaderDataRow("CreatedBy") = User.Identity.Name
                SqlAdapterHeader.Update(LocalTempDataset, "ClaimHeader")
            Else
                SqlAdapterHeader.Update(LocalTempHeaderDataTable)
            End If


            SqlAdapterHeader.Dispose()



            Dim HeaderId As Integer = 0
            If UpdateFlag = False Then
                Dim ReaderCommand As SqlCommand = New SqlCommand()
                ReaderCommand.Connection = SqlCon
                ReaderCommand.CommandText = "SELECT MAX(id) as id from ClaimHeader " & _
                    "WHERE ClaimHeader.CreatedBy=@LookUP"
                ReaderCommand.Parameters.AddWithValue("@LookUP", User.Identity.Name)

                Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()

                If reader.Read() Then
                    HeaderId = reader("id").ToString()
                End If

                reader.Close()

                ReaderCommand.Dispose()

                LocalTempDataset.Dispose()

                LocalTempHeaderDataTable.Dispose()

                SqlCon.Close()
            Else
                HeaderId = txtClaimId.Text
            End If

            Return HeaderId

        Else
            Return 0
        End If

    End Function
    Protected Sub SaveClaimPhotoItems(ByVal HeaderId As Integer)




        If Not IsNothing(Session("FilesList")) Then

            Dim SqlCon As SqlConnection = New SqlConnection()
            SqlCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
            SqlCon.Open()
            Dim LocalTempItemDataTable As New DataTable
            Dim SqlAdapterItems As _
                    New SqlDataAdapter("Select * From Files", SqlCon)
            Dim SqlCommand As New SqlCommand("DELETE FROM Files  Where HeaderId=@HeaderID", SqlCon)
            SqlAdapterItems.DeleteCommand = SqlCommand
            SqlAdapterItems.DeleteCommand.Parameters.AddWithValue("@HeaderID", HeaderId)
            SqlAdapterItems.Fill(LocalTempItemDataTable)
            SqlAdapterItems.DeleteCommand.ExecuteNonQuery()
            SqlAdapterItems.Dispose()
            SqlCommand.Dispose()
            SqlCon.Close()




            Dim UploadedFilesList As List(Of String) = New List(Of String)
            UploadedFilesList = Session("FilesList")
            If UploadedFilesList.Count > 0 Then
                'Dim SqlCon As SqlConnection = New SqlConnection()
                'SqlCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
                'SqlCon.Open()
                Dim LocalTempDataTable As New DataTable
                Dim LocalTempDataSet As New DataSet
                Dim SqlAdapter As _
                   New SqlDataAdapter("Select  * From Files", ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString)

                SqlAdapter.FillSchema(LocalTempDataSet, SchemaType.Source, "Files")
                LocalTempDataTable = LocalTempDataSet.Tables("Files")

                For i As Integer = 0 To UploadedFilesList.Count - 1
                    Dim TempDataRow As DataRow = LocalTempDataSet.Tables("Files").NewRow()
                    TempDataRow("HeaderId") = HeaderId
                    TempDataRow("Path") = UploadedFilesList.Item(i)
                    LocalTempDataTable.Rows.Add(TempDataRow)
                    Dim CB As SqlCommandBuilder = New SqlCommandBuilder(SqlAdapter)
                    SqlAdapter.Update(LocalTempDataSet, "Files")
                Next
                SqlAdapter.Dispose()


                LocalTempDataTable.Dispose()
                LocalTempDataSet.Dispose()
                'SqlCon.Close()
            End If
        Else
            FormsAuthentication.SignOut()
        End If


    End Sub
    Protected Sub SaveClaimItems(ByVal HeaderId As Integer)
        If Not IsNothing(Session("TempDataTable")) Then

            Dim SqlCon As SqlConnection = New SqlConnection()
            SqlCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
            SqlCon.Open()
            Dim LocalTempItemDataTable As New DataTable
            Dim SqlAdapterItems As _
                    New SqlDataAdapter("Select * From ClaimLineItems", SqlCon)
            Dim SqlCommand As New SqlCommand("DELETE FROM ClaimLineItems  Where HeaderId=@HeaderID", SqlCon)
            SqlAdapterItems.DeleteCommand = SqlCommand
            SqlAdapterItems.DeleteCommand.Parameters.AddWithValue("@HeaderID", HeaderId)
            SqlAdapterItems.Fill(LocalTempItemDataTable)
            SqlAdapterItems.DeleteCommand.ExecuteNonQuery()
            SqlAdapterItems.Dispose()
            SqlCommand.Dispose()
            SqlCon.Close()


            If Session("TempDataTable").Rows.Count > 0 Then

                Dim LocalTempDataTable As New DataTable
                Dim SqlAdapter As _
                   New SqlDataAdapter("Select  * From ClaimLineItems", ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString)

                SqlAdapter.FillSchema(Session("TempDataset"), SchemaType.Source, "ClaimLineItems")
                SqlAdapter.Fill(LocalTempDataTable)
                'LocalTempDataTable = Session("TempDataset").Tables("SqLineItems")
                Dim i As Integer = 0
                For Each DataRowItem As DataRow In Session("TempDataTable").Rows
                    Dim DR As DataRow = Session("TempDataset").Tables("ClaimLineItems").Rows(i)
                    DR("HeaderId") = HeaderId
                    i = i + 1
                Next
                For Each DataRowItem As DataRow In Session("TempDataTable").Rows
                    Dim TempDataRow As DataRow = Session("TempDataset").Tables("ClaimLineItems").NewRow()
                    TempDataRow = DataRowItem
                    Dim CB As SqlCommandBuilder = New SqlCommandBuilder(SqlAdapter)
                    SqlAdapter.Update(Session("TempDataset"), "ClaimLineItems")
                Next
                SqlAdapter.Dispose()

                Session("TempDataTable").Rows.Clear()
                LoadGridData()
                LocalTempDataTable.Dispose()
            Else
                'Response.Redirect("~/Business/SQ.aspx?id=Session Expired.Please Try again&status=0", False)
            End If
        Else
            FormsAuthentication.SignOut()
        End If


    End Sub
    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        If Not IsNothing(Session("FilesList")) Then

            If PhotosUpload.HasFile Then
                Dim validFileTypes As String() = {"bmp", "gif", "png", "jpg", "jpeg", "doc", "xls"}
                Dim ext As String = System.IO.Path.GetExtension(PhotosUpload.PostedFile.FileName)
                Dim isValidFile As Boolean = False
                For i As Integer = 0 To validFileTypes.Length - 1
                    If ext = "." & validFileTypes(i) Then
                        isValidFile = True
                        Exit For
                    End If
                Next
                If Not isValidFile Then
                    lblUploadMsg.ForeColor = System.Drawing.Color.Red
                    lblUploadMsg.Text = "Invalid File. Please upload a File with extension " & _
                                    String.Join(",", validFileTypes)
                    Exit Sub
                End If

                Dim intDocFileLength As Integer = Me.PhotosUpload.PostedFile.ContentLength
                Dim strPostedFileName As String = Me.PhotosUpload.PostedFile.FileName

                If intDocFileLength > 2097152 Then
                    isValidFile = False
                    lblUploadMsg.ForeColor = System.Drawing.Color.Red
                    lblUploadMsg.Text = "Invalid File. Max file size Limit 2MB"
                    Exit Sub
                Else
                    isValidFile = True
                End If

                Dim UploadedFilesList As List(Of String) = New List(Of String)
                UploadedFilesList = Session("FilesList")
                If isValidFile = True Then
                    Dim FileSavePath As String = String.Empty
                    FileSavePath = Server.MapPath("~") & "\" & System.Configuration.ConfigurationManager.AppSettings("TempUploadPath").ToString & "\" & strPostedFileName
                    While System.IO.File.Exists(FileSavePath)
                        FileSavePath = Server.MapPath("~") & "\" & System.Configuration.ConfigurationManager.AppSettings("TempUploadPath").ToString & "\" & DateAndTime.Now.Ticks & "_" & strPostedFileName
                    End While
                    If Not System.IO.File.Exists(FileSavePath) Then
                        UploadedFilesList.Add(FileSavePath)
                        PhotosUpload.PostedFile.SaveAs(FileSavePath)
                        lblUploadMsg.ForeColor = System.Drawing.Color.Green
                        lblUploadMsg.Text = "File uploaded successfully."
                        UploadedFiles.Items.Add(strPostedFileName & " - Remove")
                        Session("FilesList") = UploadedFilesList
                    End If
                End If

            End If
        Else
            FormsAuthentication.SignOut()
        End If
    End Sub

    Protected Sub UploadedFiles_Click(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.BulletedListEventArgs) Handles UploadedFiles.Click
        If Not IsNothing(Session("FilesList")) Then
            If e.Index > -1 Then
                UploadedFiles.Items.RemoveAt(e.Index)
                Dim UploadedFilesList As List(Of String) = New List(Of String)
                UploadedFilesList = Session("FilesList")
                Dim FileSavePath As String = String.Empty
                FileSavePath = UploadedFilesList.Item(e.Index)
                If System.IO.File.Exists(FileSavePath) Then
                    System.IO.File.Delete(FileSavePath)
                End If
                UploadedFilesList.RemoveAt(e.Index)
                Session("FilesList") = UploadedFilesList
            End If
        Else
            FormsAuthentication.SignOut()
        End If
    End Sub
    Private Sub ClearFormFields()

        txtEndCustomerName.Text = String.Empty
        txtPersonincharge.Text = String.Empty
        txtEmailAddress.Text = String.Empty
        txtTelno.Text = String.Empty
        txtShipToAddress.Text = String.Empty
        txtShipToCompany.Text = String.Empty
        txtShipToContactPerson.Text = String.Empty
        txtShipToTelNo.Text = String.Empty

        txtBrand.Text = String.Empty
        txtMachineModelNo.Text = String.Empty

        initializeClaimForm()
        InitializeTempDataTable()
        UploadedFiles.Items.Clear()
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsNothing(Session("TempDataTable")) Then
            'Dim dt As DataTable = Session("TempDataTable")
            'If dt.Rows.Count > 0 Then
            '    'Response.Write(dt.Rows(0).Item("InvNo").ToString)
            'End If
            Dim HeaderId As Integer = 0
            HeaderId = CreateClaim()
            If HeaderId = 0 Then
                FormsAuthentication.SignOut()
            Else
                SaveClaimItems(HeaderId)
                SaveClaimPhotoItems(HeaderId)
                ClearFormFields()
                lblMsg.Text = "Claim Tracking No : " & HeaderId & " has been sucessfully submitted."
            End If
        Else
            FormsAuthentication.SignOut()
        End If
    End Sub

    Protected Sub btnExporttoExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExporttoExcel.Click
        Dim dt As DataTable = New DataTable
        Dim filename As String = "ClaimNo-" & txtClaimId.Text & ".xls"
        Dim excelHeader As String = "Quiz Report"
        Dim tw As System.IO.StringWriter = New System.IO.StringWriter
        Dim hw As System.Web.UI.HtmlTextWriter = New System.Web.UI.HtmlTextWriter(tw)
        'hw.WriteLine("<b><u><font size=’3′> " + excelHeader + " </font></u></b>")
        'hw.WriteLine("<b><u><font size=’3′> " + excelHeader + " </font></u></b>")

        Dim HeaderTable As Table = New Table
        Dim tr As TableRow = New TableRow
        Dim td As TableCell = New TableCell
        HeaderTable.BorderColor = Drawing.ColorTranslator.FromHtml("#000000")
        HeaderTable.BorderStyle = BorderStyle.Solid
        HeaderTable.BorderWidth = 1
        td.Text = "<h3><u>Claim Details</u></h3>"
        td.ColumnSpan = 10
        td.HorizontalAlign = HorizontalAlign.Center
        tr.Cells.Add(td)
        HeaderTable.Rows.Add(tr)


        Dim tr1 As TableRow = New TableRow

        Dim tdEndCustomerNamelbl As TableCell = New TableCell
        Dim tdEndCustomerNameValue As TableCell = New TableCell

        Dim tdClaimIdlbl As TableCell = New TableCell
        Dim tdClaimIdValue As TableCell = New TableCell

        tdEndCustomerNamelbl.Text = "<b>End Customer Name</b>"
        tdEndCustomerNamelbl.HorizontalAlign = HorizontalAlign.Left
        tdEndCustomerNameValue.Text = txtEndCustomerName.Text
        tdEndCustomerNameValue.HorizontalAlign = HorizontalAlign.Left
        tdEndCustomerNameValue.ColumnSpan = 7

        tdClaimIdlbl.Text = "<b>Claim Id</b>"
        tdClaimIdlbl.HorizontalAlign = HorizontalAlign.Right
        tdClaimIdValue.Text = txtClaimId.Text
        tdClaimIdValue.HorizontalAlign = HorizontalAlign.Left

        tr1.Cells.Add(tdEndCustomerNamelbl)
        tr1.Cells.Add(tdEndCustomerNameValue)

        tr1.Cells.Add(tdClaimIdlbl)
        tr1.Cells.Add(tdClaimIdValue)



        Dim tr2 As TableRow = New TableRow

        Dim tdPersonInChargelbl As TableCell = New TableCell
        Dim tdPersonInChargeValue As TableCell = New TableCell

        Dim tdDatelbl As TableCell = New TableCell
        Dim tdDateValue As TableCell = New TableCell


        tdPersonInChargelbl.Text = "<b>Person in charge</b>"
        tdPersonInChargelbl.HorizontalAlign = HorizontalAlign.Left

        tdPersonInChargeValue.Text = txtPersonincharge.Text
        tdPersonInChargeValue.HorizontalAlign = HorizontalAlign.Left

        tdPersonInChargeValue.ColumnSpan = 7


        tdDatelbl.Text = "<b>Date</b>"
        tdDatelbl.HorizontalAlign = HorizontalAlign.Right
        tdDateValue.Text = txtDate.Text
        tdDateValue.HorizontalAlign = HorizontalAlign.Left

        tr2.Cells.Add(tdPersonInChargelbl)
        tr2.Cells.Add(tdPersonInChargeValue)

        tr2.Cells.Add(tdDatelbl)
        tr2.Cells.Add(tdDateValue)

        Dim tr3 As TableRow = New TableRow

        Dim tdEmaillbl As TableCell = New TableCell
        Dim tdEmailValue As TableCell = New TableCell

        Dim tdCountrylbl As TableCell = New TableCell
        Dim tdCountryValue As TableCell = New TableCell


        tdEmaillbl.Text = "<b>Email</b>"
        tdEmaillbl.HorizontalAlign = HorizontalAlign.Left
        tdEmailValue.Text = txtEmailAddress.Text
        tdEmailValue.HorizontalAlign = HorizontalAlign.Left
        tdEmailValue.ColumnSpan = 7


        tdCountrylbl.Text = "<b>Country</b>"
        tdCountrylbl.HorizontalAlign = HorizontalAlign.Right
        tdCountryValue.Text = txtCountry.Text
        tdCountryValue.HorizontalAlign = HorizontalAlign.Left

        tr3.Cells.Add(tdEmaillbl)
        tr3.Cells.Add(tdEmailValue)
        tr3.Cells.Add(tdCountrylbl)
        tr3.Cells.Add(tdCountryValue)

        Dim tr4 As TableRow = New TableRow

        Dim tdTelNolbl As TableCell = New TableCell
        Dim tdTelNoValue As TableCell = New TableCell


        tdTelNolbl.Text = "<b>TelNo</b>"
        tdTelNolbl.HorizontalAlign = HorizontalAlign.Left
        tdTelNoValue.Text = txtTelno.Text
        tdTelNoValue.HorizontalAlign = HorizontalAlign.Left
        tdTelNoValue.ColumnSpan = 8
        tr4.Cells.Add(tdTelNolbl)
        tr4.Cells.Add(tdTelNoValue)


        HeaderTable.Rows.Add(tr1)
        HeaderTable.Rows.Add(tr2)
        HeaderTable.Rows.Add(tr3)
        HeaderTable.Rows.Add(tr4)

        HeaderTable.RenderControl(hw)

        Dim DetailsTable As Table = GridviewToTable(ClaimItemList)
        DetailsTable.BorderColor = Drawing.ColorTranslator.FromHtml("#000000")
        DetailsTable.BorderStyle = BorderStyle.Solid
        DetailsTable.BorderWidth = 1
        For Each DetailDR As TableRow In DetailsTable.Rows
            DetailDR.BorderColor = Drawing.ColorTranslator.FromHtml("#000000")
            DetailDR.BorderStyle = BorderStyle.Solid
            DetailDR.BorderWidth = 1
        Next
        DetailsTable.RenderControl(hw)

        'ClaimItemList.RenderControl(hw)

        Dim FooterTable As Table = New Table
        FooterTable.BorderColor = Drawing.ColorTranslator.FromHtml("#000000")
        FooterTable.BorderStyle = BorderStyle.Solid
        FooterTable.BorderWidth = 1
        Dim trf1 As TableRow = New TableRow

        Dim tdEmptycells1 As TableCell = New TableCell
        Dim tdDeliveryDetailsHeader As TableCell = New TableCell



        tdEmptycells1.Text = String.Empty
        tdEmptycells1.HorizontalAlign = HorizontalAlign.Left
        tdEmptycells1.ColumnSpan = 8
        tdDeliveryDetailsHeader.Text = "<h4>Delivery Details</h4>"
        tdDeliveryDetailsHeader.HorizontalAlign = HorizontalAlign.Left
        tdDeliveryDetailsHeader.ColumnSpan = 2

        trf1.Cells.Add(tdEmptycells1)
        trf1.Cells.Add(tdDeliveryDetailsHeader)

        Dim trf2 As TableRow = New TableRow

        Dim tdEmptycells2 As TableCell = New TableCell
        Dim tdShiptoCompanylbl As TableCell = New TableCell
        Dim tdShiptoCompanyValue As TableCell = New TableCell



        tdEmptycells2.Text = String.Empty
        tdEmptycells2.HorizontalAlign = HorizontalAlign.Left
        tdEmptycells2.ColumnSpan = 7
        tdShiptoCompanylbl.Text = "<b>Ship to company</b>"
        tdShiptoCompanylbl.HorizontalAlign = HorizontalAlign.Left

        tdShiptoCompanyValue.Text = txtShipToCompany.Text
        tdShiptoCompanyValue.HorizontalAlign = HorizontalAlign.Left
        tdShiptoCompanyValue.ColumnSpan = 2



        trf2.Cells.Add(tdEmptycells2)
        trf2.Cells.Add(tdShiptoCompanylbl)
        trf2.Cells.Add(tdShiptoCompanyValue)

        Dim trf3 As TableRow = New TableRow

        Dim tdEmptycells3 As TableCell = New TableCell
        Dim tdShiptoAddresslbl As TableCell = New TableCell
        Dim tdShiptoAddressValue As TableCell = New TableCell



        tdEmptycells3.Text = String.Empty
        tdEmptycells3.HorizontalAlign = HorizontalAlign.Left
        tdEmptycells3.ColumnSpan = 7
        tdShiptoAddresslbl.Text = "<b>Ship to Address</b>"
        tdShiptoAddresslbl.HorizontalAlign = HorizontalAlign.Left
        tdShiptoAddresslbl.VerticalAlign = VerticalAlign.Top
        tdShiptoAddressValue.Text = txtShipToAddress.Text.Replace(vbLf, "<br/>")
        tdShiptoAddressValue.HorizontalAlign = HorizontalAlign.Left
        tdShiptoAddressValue.ColumnSpan = 2


        trf3.Cells.Add(tdEmptycells3)
        trf3.Cells.Add(tdShiptoAddresslbl)
        trf3.Cells.Add(tdShiptoAddressValue)


        Dim trf4 As TableRow = New TableRow

        Dim tdEmptycells4 As TableCell = New TableCell
        Dim tdShiptoContactPersonlbl As TableCell = New TableCell
        Dim tdShiptoContactPersonValue As TableCell = New TableCell



        tdEmptycells4.Text = String.Empty
        tdEmptycells4.HorizontalAlign = HorizontalAlign.Left
        tdEmptycells4.ColumnSpan = 7
        tdShiptoContactPersonlbl.Text = "<b>Contact Person</b>"
        tdShiptoContactPersonlbl.HorizontalAlign = HorizontalAlign.Left
        'tdShiptoContactPersonlbl.VerticalAlign = VerticalAlign.Top
        tdShiptoContactPersonValue.Text = txtShipToContactPerson.Text
        tdShiptoContactPersonValue.HorizontalAlign = HorizontalAlign.Left
        tdShiptoContactPersonValue.ColumnSpan = 2


        trf4.Cells.Add(tdEmptycells4)
        trf4.Cells.Add(tdShiptoContactPersonlbl)
        trf4.Cells.Add(tdShiptoContactPersonValue)

        Dim trf5 As TableRow = New TableRow

        Dim tdEmptycells5 As TableCell = New TableCell
        Dim tdShiptoTelnolbl As TableCell = New TableCell
        Dim tdShiptoTelnoValue As TableCell = New TableCell



        tdEmptycells5.Text = String.Empty
        tdEmptycells5.HorizontalAlign = HorizontalAlign.Left
        tdEmptycells5.ColumnSpan = 7
        tdShiptoTelnolbl.Text = "<b>Tel No</b>"
        tdShiptoTelnolbl.HorizontalAlign = HorizontalAlign.Left
        'tdShiptoContactPersonlbl.VerticalAlign = VerticalAlign.Top
        tdShiptoTelnoValue.Text = txtShipToTelNo.Text
        tdShiptoTelnoValue.HorizontalAlign = HorizontalAlign.Left
        tdShiptoTelnoValue.ColumnSpan = 2


        trf5.Cells.Add(tdEmptycells5)
        trf5.Cells.Add(tdShiptoTelnolbl)
        trf5.Cells.Add(tdShiptoTelnoValue)


        FooterTable.Rows.Add(trf1)
        FooterTable.Rows.Add(trf2)
        FooterTable.Rows.Add(trf3)
        FooterTable.Rows.Add(trf4)
        FooterTable.Rows.Add(trf5)

        FooterTable.RenderControl(hw)


        Dim CompanyTitleTable As Table = New Table
        Dim trTitle As TableRow = New TableRow
        Dim tdTitle As TableCell = New TableCell
        CompanyTitleTable.BorderColor = Drawing.ColorTranslator.FromHtml("#000000")
        CompanyTitleTable.BorderStyle = BorderStyle.Solid
        CompanyTitleTable.BorderWidth = 1
        tdTitle.Text = "Focus Garment Tech Pte Ltd, Singapore."
        tdTitle.ColumnSpan = 10
        tdTitle.HorizontalAlign = HorizontalAlign.Center
        trTitle.Cells.Add(tdTitle)
        CompanyTitleTable.Rows.Add(trTitle)
        CompanyTitleTable.RenderControl(hw)

        Response.ContentType = "application/vnd.ms-excel"

        Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "")

        Response.Write(tw.ToString())
        Response.End()

    End Sub
    Private Function GridviewToTable(ByVal DgridView As GridView) As Table
        Dim ResultTable As Table = New Table
        ResultTable.BorderColor = Drawing.ColorTranslator.FromHtml("#5D7B9D")
        ResultTable.BorderStyle = BorderStyle.Solid
        ResultTable.BorderWidth = 1
        Dim Drow As TableRow = New TableRow
        Dim i As Integer = 0
        For k As Integer = 1 To DgridView.HeaderRow.Cells.Count - 1
            If k = 2 Or k = 3 Or k = 4 Then
            Else
                Dim Dcell As TableCell = New TableCell
                Dcell.Text = DgridView.HeaderRow.Cells(k).Text
                Dcell.BackColor = Drawing.ColorTranslator.FromHtml("#5D7B9D")
                Drow.Cells.Add(Dcell)

            End If
        Next
        ResultTable.Rows.Add(Drow)
        For Each GRow As GridViewRow In DgridView.Rows
            Drow = New TableRow
            For j As Integer = 1 To GRow.Cells.Count - 1
                Dim Dcell As TableCell = New TableCell
                'GRow.Cells(j).Controls()
                Dcell.HorizontalAlign = HorizontalAlign.Left
                If GRow.Cells(j).Controls().Count > 0 Then
                    For Each ctrl As Control In GRow.Cells(j).Controls()
                        If TypeOf (ctrl) Is TextBox Then
                            Dim Tbox As TextBox = ctrl
                            Dcell.Text = Tbox.Text
                        End If
                    Next
                Else
                    Dcell.Text = GRow.Cells(j).Text
                End If
                If j = 2 Or j = 3 Or j = 4 Then
                Else
                    Drow.Cells.Add(Dcell)
                End If

            Next
            ResultTable.Rows.Add(Drow)
        Next
        Return ResultTable
    End Function

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        If Not IsNothing(Session("TempDataTable")) Then
            'Dim dt As DataTable = Session("TempDataTable")
            'If dt.Rows.Count > 0 Then
            '    'Response.Write(dt.Rows(0).Item("InvNo").ToString)
            'End If
        Else
            FormsAuthentication.SignOut()
        End If
    End Sub

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Response.Redirect("~/Business/Manufacturer.aspx", True)
    End Sub
End Class
