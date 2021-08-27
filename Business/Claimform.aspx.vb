Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Globalization
Imports System.Net.Mail
Imports System.IO
Imports Models
Imports System.Reflection
Imports System.Threading

Partial Class Business_Claimform
    Inherits System.Web.UI.Page
    Dim Dtable As DataTable
    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Session("ClaimId") = String.Empty
        Response.Redirect("~/Business/Claimform.aspx", True)
    End Sub
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        'UpdateDTwithGridviewData()
        If Not IsNothing(ViewState("TempDataTable")) Then
            Dim TempDataTable As New DataTable
            TempDataTable = ViewState("TempDataTable")
            Dim DestinationRow As DataRow = TempDataTable.NewRow()
            TempDataTable.Rows.Add(DestinationRow)
            ViewState("TempDataTable") = TempDataTable
            LoadGridData()
        Else
            FormsAuthentication.SignOut()
        End If
    End Sub
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
    Private Function CheckClaimIDExist(ByVal Headerid As String) As Boolean
        Dim ClaimFlag As Boolean = False
        Dim SqlCon As SqlConnection = New SqlConnection()
        SqlCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
        SqlCon.Open()
        Dim ReaderCommand As SqlCommand = New SqlCommand()
        ReaderCommand.Connection = SqlCon
        'ReaderCommand.CommandText = "SELECT id  AS  ClaimId FROM ClaimHeader Where id=@Headerid AND Status!=@status  and Country=@country"
        ReaderCommand.CommandText = "SELECT id  AS  ClaimId FROM ClaimHeader Where id=@Headerid   and Country=@country"
        Dim ClaimId As Integer = 0
        If Integer.TryParse(Headerid, ClaimId) Then
            ReaderCommand.Parameters.AddWithValue("@Headerid", ClaimId)
        Else
            ReaderCommand.Parameters.AddWithValue("@Headerid", "-1")
        End If

        ReaderCommand.Parameters.AddWithValue("@status", "Complete")
        ReaderCommand.Parameters.AddWithValue("@country", Session("Country"))
        Dim Internalreader As String = ReaderCommand.ExecuteScalar()
        If IsNumeric(Internalreader) Then
            ClaimFlag = True
        End If
        ReaderCommand.Dispose()
        SqlCon.Close()
        If Not Roles.IsUserInRole(User.Identity.Name, "Focus") And Not Roles.IsUserInRole(User.Identity.Name, "Admin") Then
            MakeFormReadOnly()
        End If

        If Roles.IsUserInRole(User.Identity.Name, "FocusLimited") Then
            MakeFormReadOnly()
            btnSave.Enabled = True
            txtreasonforrejection.ReadOnly = False
        End If
        If Roles.IsUserInRole(User.Identity.Name, "FocusLimitedAdmin") Then
            MakeFormReadOnly()
        End If


        Return ClaimFlag
    End Function
    Private Sub ItemDetailsGrid_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles ItemDetailsGrid.PageIndexChanging
        ItemDetailsGrid.PageIndex = e.NewPageIndex
        LoadRemarks(CInt(txtClaimId.Text))
    End Sub
    Private Sub MakeFormReadOnly()
        btnAdd.Enabled = False
        btnCopy.Enabled = False
        btnDelete.Enabled = False
        btnSave.Enabled = False
        btnClear.Enabled = False
        btnUpload.Enabled = False
        txtBrand.ReadOnly = True
        txtMachineModelNo.ReadOnly = True
        txtreasonforrejection.ReadOnly = True
        UploadedFiles.Enabled = False
        txtEndCustomerName.ReadOnly = True
        txtDearerCustomerName.ReadOnly = True
        txtPersonincharge.ReadOnly = True
        txtEmailAddress.ReadOnly = True
        txtTelno.ReadOnly = True
        txtDate.ReadOnly = True
        txtCountry.ReadOnly = True
        ddlStatus.Enabled = False
        ClaimItemList.Enabled = False
        txtShipToAddress.ReadOnly = True
        txtShipToCompany.ReadOnly = True
        txtShipToContactPerson.ReadOnly = True
        txtShipToTelNo.ReadOnly = True
    End Sub
    Protected Sub btnCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        'UpdateDTwithGridviewData()
        If ClaimItemList.SelectedIndex >= 0 And Not IsNothing(ViewState("TempDataTable")) Then
            Dim TempDataTable As New DataTable
            TempDataTable = ViewState("TempDataTable")
            Dim DestinationRow As DataRow = TempDataTable.NewRow()
            Dim SourceRow As DataRow = TempDataTable.Rows(ClaimItemList.SelectedIndex)
            DestinationRow.ItemArray = SourceRow.ItemArray
            TempDataTable.Rows.InsertAt(DestinationRow, ClaimItemList.SelectedIndex + 1)
            ViewState("TempDataTable") = TempDataTable
            LoadGridData()
            ClaimItemList.SelectedIndex = -1
        Else
            FormsAuthentication.SignOut()
        End If
    End Sub
    Private Sub LoadGridData()
        If Not IsNothing(ViewState("TempDataTable")) Then
            ClaimItemList.DataSource = ViewState("TempDataTable")
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
        ViewState("FilesList") = UploadedFilesList
        ViewState("TempDataset") = TempDataset
        ViewState("TempDataTable") = TempDataTable
        ViewState("SelectedRow") = SelectedRow
        If Not IsNothing(ViewState("TempDataTable")) Then
            ViewState("TempDataTable").Rows.Clear()
        End If
        Dim SqlAdapter As _
           New SqlDataAdapter("Select [SNo],[InvDate],[InvNo],[Qty],[Brand],[ModelNo],[SerialNo],[ClaimQty],[OpenQty],[Particulars],[ClaimMode],[ClaimDate],[ClaimReference],[PartNo],[Claimed],[Status],[DeclareValue],[DeclareDescription] From ClaimLineItems", ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString)

        SqlAdapter.FillSchema(ViewState("TempDataset"), SchemaType.Source, "ClaimLineItems")
        ViewState("TempDataTable") = ViewState("TempDataset").Tables("ClaimLineItems")
        'TempDataTable = ViewState("TempDataTable")

        'TempDataTable = ViewState("TempDataTable")
        'Dim workRow As DataRow = TempDataTable.NewRow()        
        'TempDataTable.Rows.Add(workRow)

        ClaimItemList.DataSource = ViewState("TempDataTable")
        ClaimItemList.DataBind()
        SqlAdapter.Dispose()
        '    ModifyGridViewHeader()

    End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If ClaimItemList.SelectedIndex >= 0 Then
            ClaimItemList.DeleteRow(ClaimItemList.SelectedIndex)
            If Not IsNothing(ViewState("TempDataTable")) Then
                Dim TempDatatable As DataTable = New DataTable
                TempDatatable = ViewState("TempDataTable")
                TempDatatable.Rows.RemoveAt(ClaimItemList.SelectedIndex)
                ViewState("TempDataTable") = TempDatatable
                LoadGridData()
            Else
                FormsAuthentication.SignOut()
            End If
        End If
    End Sub
    Private Sub LoadExistingCliam(ByVal HeaderId As Integer)

        If Not IsNothing(ViewState("TempDataTable")) And Not IsNothing(Session("Country")) Then


            Dim SqlCon As SqlConnection = New SqlConnection()
            SqlCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
            SqlCon.Open()
            Dim SqlCom As SqlCommand = New SqlCommand()
            SqlCom.Connection = SqlCon

            If Roles.IsUserInRole(User.Identity.Name, "Focus") Or Roles.IsUserInRole(User.Identity.Name, "FocusLimitedAdmin") Then
                'SqlCom.CommandText = "Select * from ClaimHeader Where id=@HeaderId AND Status!=@status   and Country=@country"
                SqlCom.CommandText = "Select * from ClaimHeader Where id=@HeaderId and Country=@country"
            Else
                'SqlCom.CommandText = "Select * from ClaimHeader Where id=@HeaderId AND Status!=@status   and Country=@country and CreatedBy=@CreatedBy"
                SqlCom.CommandText = "Select * from ClaimHeader Where id=@HeaderId and Country=@country and CreatedBy=@CreatedBy"
                SqlCom.Parameters.AddWithValue("@CreatedBy", User.Identity.Name)

            End If

            SqlCom.Parameters.AddWithValue("@HeaderId", HeaderId)
            SqlCom.Parameters.AddWithValue("@status", "Complete")
            SqlCom.Parameters.AddWithValue("@country", Session("Country"))

            Dim HeaderReader As SqlDataReader = SqlCom.ExecuteReader

            If HeaderReader.Read() Then

                txtClaimId.Text = HeaderId
                If Not IsDBNull(HeaderReader("DealerName")) Then
                    txtDearerCustomerName.Text = HeaderReader("DealerName")
                End If

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
                    If ddlStatus.Items(1).Value = "Inprocess" Then
                        ddlStatus.Items(0).Enabled = False
                    End If
                    ddlStatus.SelectedValue = HeaderReader("Status")
                End If
                If Not IsDBNull(HeaderReader("Priority")) Then
                    ddlPriority.SelectedValue = HeaderReader("Priority")
                End If
                If Not IsDBNull(HeaderReader("CreatedBy")) Then
                    txtCreatedBy.Text = HeaderReader("CreatedBy")
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


                If Not IsDBNull(HeaderReader("reasonforrejection")) Then
                    txtreasonforrejection.Text = HeaderReader("reasonforrejection")
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
            TempDataTable = ViewState("TempDataTable")
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
                    If IsDate(Today) Then
                        TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("InvDate") = Today
                    End If
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
                    TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("ClaimQty") = 1
                End If

                If Not IsDBNull(ItemsReader("OpenQty")) Then
                    TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("OpenQty") = ItemsReader("OpenQty")
                Else
                    TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("OpenQty") = 1
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

                    If ItemsReader("ClaimReference").ToString <> String.Empty Then
                        TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("Status") = "Complete"
                    End If

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

                Dim ModelNo As String = TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("ModelNo")
                Dim SerialNo As String = TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("SerialNo")
                Dim Brand As String = TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("Brand")

                Dim SqlDuplicateCon As SqlConnection = New SqlConnection()
                SqlDuplicateCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
                SqlDuplicateCon.Open()
                Dim SqlDuplicateCom As SqlCommand = New SqlCommand()
                SqlDuplicateCom.Connection = SqlDuplicateCon
                SqlDuplicateCom.CommandText = "Select id from ClaimLineItems INNER JOIN  ClaimHeader ON  ClaimHeader.id=ClaimLineItems.HeaderId  WHERE  ClaimHeader.id!=@HeaderId   and ClaimLineItems.ModelNo=@ModelNo   and ClaimLineItems.SerialNo=@SerialNo   and ClaimLineItems.Brand=@Brand"
                SqlDuplicateCom.Parameters.AddWithValue("@ModelNo", ModelNo)
                SqlDuplicateCom.Parameters.AddWithValue("@SerialNo", SerialNo)
                SqlDuplicateCom.Parameters.AddWithValue("@Brand", Brand)
                SqlDuplicateCom.Parameters.AddWithValue("@Status", "Complete")
                SqlDuplicateCom.Parameters.AddWithValue("@HeaderId", HeaderId)
                Dim Dreader As SqlDataReader = SqlDuplicateCom.ExecuteReader
                If Dreader.Read() Then
                    TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("Claimed") = "Y"
                Else
                    TempDataTable.Rows(TempDataTable.Rows.Count - 1).Item("Claimed") = String.Empty
                End If

                Dreader.Close()
                SqlDuplicateCom.Dispose()
                SqlDuplicateCon.Close()

            End While

            ViewState("TempDataTable") = TempDataTable

            ItemsReader.Close()
            SqlItemCom.Dispose()

            Dim SqlFilesCom As SqlCommand = New SqlCommand()
            SqlFilesCom.Connection = SqlCon
            SqlFilesCom.CommandText = "Select * from Files Where Headerid=@HeaderId"
            SqlFilesCom.Parameters.AddWithValue("@HeaderId", HeaderId)

            Dim FilesReader As SqlDataReader = SqlFilesCom.ExecuteReader
            Dim UploadedFilesList As List(Of String) = New List(Of String)
            UploadedFilesList = ViewState("FilesList")
            While FilesReader.Read()

                UploadedFilesList.Add(FilesReader("Path"))
                UploadedFiles.Items.Add(System.IO.Path.GetFileName(FilesReader("Path")) & " - Remove")
            End While
            ViewState("FilesList") = UploadedFilesList
            'UploadedFiles.Enabled = False
            LoadThumbnails()
            FilesReader.Close()
            SqlFilesCom.Dispose()
            SqlCon.Close()
            LoadGridData()
        Else
            FormsAuthentication.SignOut()
        End If

    End Sub
    'Protected Sub ValidateSerialNo(ByVal source As System.Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles ModelNoValidation.ServerValidate
    '    'If args.Value.ToString() = "-" Then
    '    '    args.IsValid = False
    '    'Else
    '    '    args.IsValid = True
    '    'End If
    '    args.IsValid = True
    '    For i = 0 To ClaimItemList.Rows.Count - 1
    'Dim ModelNo As TextBox = ClaimItemList.Rows(i).FindControl("ModelNo")
    'Dim SerialNo As TextBox = ClaimItemList.Rows(i).FindControl("SerialNo")
    'Dim Brand As TextBox = ClaimItemList.Rows(i).FindControl("Brand")
    ''        Dim SqlCon As SqlConnection = New SqlConnection()
    ''        SqlCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
    ''        SqlCon.Open()
    ''        Dim SqlCom As SqlCommand = New SqlCommand()
    ''        SqlCom.Connection = SqlCon
    ''        SqlCom.CommandText = "Select id from ClaimLineItems INNER JOIN  ClaimHeader ON  ClaimHeader.id=ClaimLineItems.HeaderId  WHERE ClaimHeader.Status!=@Status   and ClaimLineItems.ModelNo=@ModelNo   and ClaimLineItems.SerialNo=@SerialNo   and ClaimLineItems.Brand=@Brand"
    ''        SqlCom.Parameters.AddWithValue("@ModelNo", ModelNo.Text)
    ''        SqlCom.Parameters.AddWithValue("@SerialNo", SerialNo.Text)
    ''        SqlCom.Parameters.AddWithValue("@Brand", Brand.Text)
    ''        SqlCom.Parameters.AddWithValue("@Status", "Complete")
    ''        Dim Dreader As SqlDataReader = SqlCom.ExecuteReader
    ''        If Dreader.Read() Then
    '            args.IsValid = False
    '            Exit For
    '        Else

    '        End If
    '        Dreader.Close()
    '        SqlCom.Dispose()
    '        SqlCon.Close()
    '        SqlCon.Dispose()
    '    Next

    'End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If User.IsInRole("Customer") Then
            lblPriority.Visible = False
            ddlPriority.Visible = False
            btnFind.Visible = False
            txtFind.Visible = False
            ddlStatus.Enabled = False
        End If
        If Not IsNothing(Session("Country")) Then


            If Not IsPostBack Then
                InitializeTempDataTable()
                initializeClaimForm()
            End If

            UpdateDTwithGridviewData()


            If Not IsNothing(Session("ClaimId")) And Not IsPostBack Then
                Dim ClaimId As Integer = 0
                If Int32.TryParse(Session("ClaimId"), ClaimId) Then
                    If CheckClaimIDExist(ClaimId) Then
                        InitializeTempDataTable()
                        LoadExistingCliam(ClaimId)
                    Else
                        lblMsg.Text = "Claim Not exist"
                        Session("ClaimId") = String.Empty
                        lblMsg.ForeColor = Drawing.Color.Red
                    End If
                Else
                    txtClaimId.Text = "New"
                End If
            End If

            If Not IsNothing(Session("ClaimId")) Then
                Dim ClaimId As Integer = 0
                If Int32.TryParse(Session("ClaimId"), ClaimId) Then
                    LoadRemarks(ClaimId)
                Else
                    LoadRemarks(-1)
                End If
            Else
                LoadRemarks(-1)
            End If
        Else
            FormsAuthentication.SignOut()
        End If


    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
        Return
    End Sub
    Private Sub initializeClaimForm()
        'txtClaimId.Text = GenerateClaimID()

        txtClaimId.Text = "New"
        txtDate.Text = Now.Date.ToString("dd-MM-yyyy")
        txtCountry.Text = Session("CountryName")
    End Sub
    Private Sub ReassignSerialNo()
        For i As Integer = 1 To ClaimItemList.Rows.Count
            Dim SNo As TextBox = ClaimItemList.Rows(i - 1).Cells(1).FindControl("SNo")
            SNo.Text = i
            'Dim InvDate As TextBox = ClaimItemList.Rows(i - 1).Cells(2).FindControl("InvDate")
            'Dim Today As Date = Now.Date

            'If Date.TryParse(InvDate.Text, Today) Then
            '    InvDate.Text = Today.Date.ToString("dd-MM-yyyy")
            'End If
        Next
    End Sub
    Protected Sub ClaimItemList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles ClaimItemList.RowDataBound
        ReassignSerialNo()
        'If User.IsInRole("Focus") Then
        '    ClaimItemList.Columns(ClaimItemList.Columns.Count - 2).Visible = True
        '    'ClaimItemList.Columns(ClaimItemList.Columns.Count - 1).ItemStyle.BackColor = Drawing.Color.Red
        'End If
        'ClaimItemList.Columns(ClaimItemList.Columns.Count - 1).Visible = True
        If e.Row.RowIndex >= 0 Then

            'Dim InvDate As TextBox = e.Row.Cells(2).FindControl("InvDate")
            'Dim Today As Date = Now.Date
            'If Date.TryParseExact(InvDate.Text, "M/d/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, Today) Then
            '    InvDate.Text = Today.ToString("dd-MM-yyyy")
            'Else
            '    response.write(InvDate.Text)
            'End If

        End If

    End Sub
    Private Sub UpdateDTwithGridviewData()
        If Not IsNothing(ViewState("TempDataTable")) Then


            Dim TempDataTable As New DataTable
            TempDataTable = ViewState("TempDataTable")
            For Each row As GridViewRow In ClaimItemList.Rows
                Dim SNo As TextBox = row.Cells(1).FindControl("SNo")
                Dim InvDate As TextBox = row.Cells(1).FindControl("InvDate")
                Dim InvNo As TextBox = row.Cells(2).FindControl("InvNo")
                Dim Qty As TextBox = row.Cells(3).FindControl("Qty")
                'Dim Brand As TextBox = row.Cells(4).FindControl("Brand")
                'Dim ModelNo As TextBox = row.Cells(5).FindControl("ModelNo")
                Dim SerialNo As TextBox = row.Cells(6).FindControl("SerialNo")
                Dim ClaimQty As TextBox = row.Cells(7).FindControl("ClaimQty")
                Dim Particulars As TextBox = row.Cells(8).FindControl("Particulars")
                Dim DeclareValue As TextBox = row.Cells(9).FindControl("DeclareValue")
                Dim DeclareDescription As TextBox = row.Cells(10).FindControl("DeclareDescription")
                'Dim ClaimMode As TextBox = row.Cells(7).FindControl("ClaimMode")
                'Dim ClaimDate As TextBox = row.Cells(7).FindControl("ClaimDate")
                'Dim ClaimReference As TextBox = row.Cells(7).FindControl("ClaimReference")
                'Dim PartNo As TextBox = row.Cells(7).FindControl("PartNo")

                If row.RowIndex <= (TempDataTable.Rows.Count - 1) Then
                    Dim Today As Date = Now.Date
                    If Date.TryParseExact(InvDate.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, Today) Then
                        TempDataTable.Rows(row.RowIndex).Item("InvDate") = Today.ToString("yyyy-MM-dd")
                    End If
                    TempDataTable.Rows(row.RowIndex).Item("SNo") = SNo.Text
                    TempDataTable.Rows(row.RowIndex).Item("InvNo") = InvNo.Text
                    TempDataTable.Rows(row.RowIndex).Item("Qty") = Qty.Text
                    'TempDataTable.Rows(row.RowIndex).Item("Brand") = Brand.Text
                    'TempDataTable.Rows(row.RowIndex).Item("ModelNo") = ModelNo.Text
                    TempDataTable.Rows(row.RowIndex).Item("Brand") = txtBrand.Text
                    TempDataTable.Rows(row.RowIndex).Item("ModelNo") = txtMachineModelNo.Text

                    TempDataTable.Rows(row.RowIndex).Item("SerialNo") = SerialNo.Text
                    TempDataTable.Rows(row.RowIndex).Item("ClaimQty") = ClaimQty.Text
                    TempDataTable.Rows(row.RowIndex).Item("Particulars") = Particulars.Text
                    TempDataTable.Rows(row.RowIndex).Item("DeclareValue") = DeclareValue.Text
                    TempDataTable.Rows(row.RowIndex).Item("DeclareDescription") = DeclareDescription.Text
                    'TempDataTable.Rows(row.RowIndex).Item("ClaimMode") = ClaimMode.Text
                    'TempDataTable.Rows(row.RowIndex).Item("ClaimDate") = ClaimDate.Text
                    'TempDataTable.Rows(row.RowIndex).Item("ClaimReference") = ClaimReference.Text
                    'TempDataTable.Rows(row.RowIndex).Item("PartNo") = PartNo.Text
                End If
            Next
            ViewState("TempDataTable") = TempDataTable
        Else
            FormsAuthentication.SignOut()
        End If
    End Sub
    Protected Sub GridViewToSessionTable()
        Try
            Dim i As Integer = 0
            Dim PickLstLines As New Models.ClaimLineItems

            For Each DRow As GridViewRow In ClaimItemList.Rows

            Next
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
    Protected Function GridLinesToObjectsPrepareData(Optional ByVal PrimaryKey As String = "") As Object()
        Try
            Dim PickLstLines() As Models.ClaimLineItems = Nothing
            Dim Exceptlist As New List(Of String)
            Exceptlist.Add("Headerid")

            Dim GridDataTable As DataTable = TryCast(ViewState("TempDataTable"), DataTable)
            If Not IsNothing(GridDataTable) Then
                Dim i As Integer = 0
                For Each DRow As DataRow In GridDataTable.Rows
                    'Set OpenQty as equal as Qty
                    'DRow("OpenQty") = DRow("Qty")
                    'GridDataTable.AcceptChanges()


                    ReDim Preserve PickLstLines(i)
                    PickLstLines(i) = New Models.ClaimLineItems
                    Dim props As Type = PickLstLines(i).GetType
                    For Each member As PropertyInfo In props.GetProperties
                        If Not Exceptlist.Contains(member.Name) Then
                            If Not IsDBNull(DRow(member.Name)) Then
                                member.SetValue(PickLstLines(i), DRow(member.Name).ToString, Nothing)
                            End If
                            If member.Name.ToLower = "sno" Then
                                member.SetValue(PickLstLines(i), (i + 1).ToString, Nothing)
                            End If
                            If member.Name.ToLower = "invdate" And Not IsDBNull(DRow(member.Name)) Then
                                Dim ValidDate As New Date
                                Dim Datevalue As String = DRow(member.Name)

                                If Date.TryParseExact(Datevalue, "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, ValidDate) Then
                                    member.SetValue(PickLstLines(i), ValidDate.ToString("yyyy-MM-dd"), Nothing)
                                End If
                            End If
                            If PrimaryKey <> String.Empty Then
                                If member.Name.ToLower = "id" Then
                                    member.SetValue(PickLstLines(i), PrimaryKey, Nothing)
                                End If
                            End If
                        End If

                    Next
                    i = i + 1
                Next
            End If
            Return PickLstLines
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
            Return Nothing
        End Try

    End Function
    Protected Function GridLinesToFileObjectsPrepareData(Optional ByVal PrimaryKey As String = "") As Object()
        Try
            Dim PickLstLines() As Models.Files = Nothing
            Dim Exceptlist As New List(Of String)
            Exceptlist.Add("Headerid")

            Dim fileslist As List(Of String) = TryCast(ViewState("FilesList"), List(Of String))
            If Not IsNothing(fileslist) Then
                Dim i As Integer = 0
                For Each DRow In fileslist
                    'Set OpenQty as equal as Qty
                    'DRow("OpenQty") = DRow("Qty")
                    'GridDataTable.AcceptChanges()


                    ReDim Preserve PickLstLines(i)
                    PickLstLines(i) = New Models.Files
                    PickLstLines(i).Headerid = PrimaryKey
                    PickLstLines(i).Path = DRow
                    i = i + 1
                Next
            End If
            Return PickLstLines
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
            Return Nothing
        End Try

    End Function
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then
            UpdateDTwithGridviewData()

            Try
                Dim CurrentCountry As String = Session("Country")
                Dim CurrentCountryName As String = Session("CountryName")
                Dim ProceedNext As Boolean = True
                If Not IsNothing(Session("Country")) Then
                    If CheckClaimIDExist(txtClaimId.Text) Then
                        Using SqlCon As New SqlConnection(CURD.GetConnectionString)

                            SqlCon.Open()

                            Dim SqlTrans As SqlTransaction = SqlCon.BeginTransaction()

                            Try

                                Dim PrimaryKey As String = txtClaimId.Text

                                Dim CH As New Models.ClaimHeader
                                CH.DealerName = txtDearerCustomerName.Text
                                CH.CustomerName = txtEndCustomerName.Text
                                CH.Personincharge = txtPersonincharge.Text
                                CH.EmailAddress = txtEmailAddress.Text
                                CH.PhoneNo = txtTelno.Text
                                CH.Brand = txtBrand.Text
                                CH.ModelNo = txtMachineModelNo.Text

                                CH.Country = CurrentCountry
                                CH.CountryName = CurrentCountryName
                                CH.ShipToCompany = txtShipToCompany.Text
                                CH.ShipToAddress = txtShipToAddress.Text
                                CH.ShipToContact = txtShipToContactPerson.Text
                                CH.ShipToPhone = txtShipToTelNo.Text
                                CH.Status = ddlStatus.SelectedValue
                                CH.Priority = ddlPriority.SelectedValue
                                CH.youtubelink1 = txtYoutubeLink1.Text
                                CH.youtubelink2 = txtYoutubeLink2.Text
                                CH.youtubelink3 = txtYoutubeLink3.Text
                                CH.reasonforrejection = txtreasonforrejection.Text
                                CH.UpdatedBy = User.Identity.Name
                                CH.Status = ddlStatus.SelectedItem.Value

                                Dim UQ As New UpdateQuery
                                UQ._InputTable = CH
                                Dim FilterTable As New ClaimHeader
                                FilterTable.id = PrimaryKey
                                UQ._FilterTable = FilterTable
                                UQ._DB = "Custom"
                                UQ._HasInBetweenConditions = False
                                UQ._HasWhereConditions = True
                                'Query Conditions List
                                Dim QryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

                                'Query Condition Groups
                                Dim ConditionsGrp1 As List(Of String) = New List(Of String)

                                'Query Conditions values
                                ConditionsGrp1.Add("Id=@Filter_Id")

                                QryConditions.Add(" AND ", ConditionsGrp1)
                                UQ._Conditions = QryConditions

                                ProceedNext = CURD.UpdateDataTransaction(UQ, SqlCon, SqlTrans, PrimaryKey, True)
                                If PrimaryKey = String.Empty Then
                                    Throw New System.Exception("Header object update error DocStatus closed in Claim update page")
                                End If

                                If ProceedNext = True Then
                                    Dim DQ As New DeleteQuery
                                    Dim DeleteFilterTable As New ClaimLineItems
                                    DeleteFilterTable.Headerid = PrimaryKey
                                    'Where Condition parameter


                                    DQ._InputTable = DeleteFilterTable
                                    DQ._DB = "Custom"
                                    DQ._HasInBetweenConditions = False
                                    DQ._HasWhereConditions = True

                                    'Query Conditions List
                                    Dim DeleteQryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

                                    'Query Condition Groups
                                    Dim DeleteConditionsGrp1 As List(Of String) = New List(Of String)

                                    'Query Conditions values
                                    DeleteConditionsGrp1.Add("Headerid=@Headerid")

                                    DeleteQryConditions.Add(" AND ", DeleteConditionsGrp1)

                                    DQ._Conditions = DeleteQryConditions


                                    ProceedNext = CURD.DeleteDataTransaction(DQ, SqlCon, SqlTrans, PrimaryKey, False)

                                Else
                                    Throw New System.Exception("Line Item deletion error in Claim update page")
                                End If

                                If ProceedNext = True Then
                                    Dim DQ As New DeleteQuery
                                    Dim DeleteFilesFilterTable As New Files
                                    DeleteFilesFilterTable.Headerid = PrimaryKey
                                    'Where Condition parameter


                                    DQ._InputTable = DeleteFilesFilterTable
                                    DQ._DB = "Custom"
                                    DQ._HasInBetweenConditions = False
                                    DQ._HasWhereConditions = True

                                    'Query Conditions List
                                    Dim DeleteFilesQryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

                                    'Query Condition Groups
                                    Dim DeleteFilesConditionsGrp1 As List(Of String) = New List(Of String)

                                    'Query Conditions values
                                    DeleteFilesConditionsGrp1.Add("Headerid=@Headerid")

                                    DeleteFilesQryConditions.Add(" AND ", DeleteFilesConditionsGrp1)

                                    DQ._Conditions = DeleteFilesQryConditions


                                    ProceedNext = CURD.DeleteDataTransaction(DQ, SqlCon, SqlTrans, PrimaryKey, False)

                                Else
                                    Throw New System.Exception("Line Item files deletion error in Claim update page")
                                End If

                                Dim LineItemsObject() As Models.ClaimLineItems
                                If ProceedNext Then
                                    LineItemsObject = TryCast(GridLinesToObjectsPrepareData(PrimaryKey), Models.ClaimLineItems())
                                    If Not IsNothing(LineItemsObject) Then
                                        For Each Obj As Models.ClaimLineItems In LineItemsObject
                                            Obj.Headerid = PrimaryKey
                                            ProceedNext = CURD.InsertDataTransaction(Obj, SqlCon, SqlTrans, PrimaryKey, False)
                                            If ProceedNext = False Then
                                                Throw New System.Exception("Line Item insertion error in Claim lines page")
                                            End If
                                        Next
                                    Else
                                        Throw New System.Exception("No Line Found Claim lines page" & PrimaryKey)
                                    End If
                                End If

                                Dim LineItemsFileObject() As Models.Files
                                If ProceedNext Then
                                    LineItemsFileObject = TryCast(GridLinesToFileObjectsPrepareData(PrimaryKey), Models.Files())
                                    If Not IsNothing(LineItemsFileObject) Then
                                        For Each Obj As Models.Files In LineItemsFileObject
                                            Obj.Headerid = PrimaryKey
                                            ProceedNext = CURD.InsertDataTransaction(Obj, SqlCon, SqlTrans, PrimaryKey, False)
                                            If ProceedNext = False Then
                                                Throw New System.Exception("Line Item files insertion error in Claim lines page")
                                            End If
                                        Next

                                    End If
                                End If


                                lblMsg.Text = "Claim Tracking No : " & PrimaryKey & " has been sucessfully updated."
                                lblMsg.ForeColor = Drawing.Color.Green
                                SqlTrans.Commit()
                                ClearFormFields()

                            Catch ex As Exception
                                lblMsg.Text = "There is some problem in saving your claim. Please contact your website Administrator"
                                lblMsg.ForeColor = Drawing.Color.Red
                                AppSpecificFunc.WriteLog(ex)
                                SqlTrans.Rollback()
                            End Try
                        End Using
                    Else

                        Using SqlCon As New SqlConnection(CURD.GetConnectionString)

                            SqlCon.Open()

                            Dim SqlTrans As SqlTransaction = SqlCon.BeginTransaction()

                            Try

                                Dim PrimaryKey As String = String.Empty

                                Dim CH As New Models.ClaimHeader
                                CH.DealerName = txtDearerCustomerName.Text
                                CH.CustomerName = txtEndCustomerName.Text
                                CH.Personincharge = txtPersonincharge.Text
                                CH.EmailAddress = txtEmailAddress.Text
                                CH.PhoneNo = txtTelno.Text
                                CH.Brand = txtBrand.Text
                                CH.ModelNo = txtMachineModelNo.Text
                                CH.DocDate = Format(CDate(DateTime.Now), "yyyy-MM-dd")
                                CH.Country = CurrentCountry
                                CH.CountryName = CurrentCountryName
                                CH.ShipToCompany = txtShipToCompany.Text
                                CH.ShipToAddress = txtShipToAddress.Text
                                CH.ShipToContact = txtShipToContactPerson.Text
                                CH.ShipToPhone = txtShipToTelNo.Text
                                CH.Status = ddlStatus.SelectedValue
                                CH.Priority = ddlPriority.SelectedValue
                                CH.youtubelink1 = txtYoutubeLink1.Text
                                CH.youtubelink2 = txtYoutubeLink2.Text
                                CH.youtubelink3 = txtYoutubeLink3.Text
                                CH.reasonforrejection = txtreasonforrejection.Text
                                CH.CreatedBy = User.Identity.Name

                                ProceedNext = CURD.InsertDataTransaction(CH, SqlCon, SqlTrans, PrimaryKey, True)
                                Dim LineItemsObject() As Models.ClaimLineItems

                                If ProceedNext Then
                                    LineItemsObject = TryCast(GridLinesToObjectsPrepareData(PrimaryKey), Models.ClaimLineItems())
                                    For Each Obj As Models.ClaimLineItems In LineItemsObject
                                        Obj.Headerid = PrimaryKey
                                        Obj.OpenQty = Obj.ClaimQty
                                        ProceedNext = CURD.InsertDataTransaction(Obj, SqlCon, SqlTrans, PrimaryKey, False)
                                        If ProceedNext = False Then
                                            Throw New System.Exception("Line Item insertion error in Claim lines page")
                                        End If
                                    Next
                                End If
                                Dim LineItemsFileObject() As Models.Files
                                If ProceedNext Then
                                    LineItemsFileObject = TryCast(GridLinesToFileObjectsPrepareData(PrimaryKey), Models.Files())
                                    If Not IsNothing(LineItemsFileObject) Then
                                        For Each Obj As Models.Files In LineItemsFileObject
                                            Obj.Headerid = PrimaryKey

                                            ProceedNext = CURD.InsertDataTransaction(Obj, SqlCon, SqlTrans, PrimaryKey, False)
                                            If ProceedNext = False Then
                                                Throw New System.Exception("Line Item files insertion error in Claim lines page")
                                            End If
                                        Next
                                    End If
                                End If

                                lblMsg.Text = "Claim Tracking No : " & PrimaryKey & " has been sucessfully submitted."
                                lblMsg.ForeColor = Drawing.Color.Green
                                SqlTrans.Commit()
                                SendMail(PrimaryKey)
                                ClearFormFields()
                            Catch ex As Exception
                                lblMsg.Text = "There is some problem in saving your claim. Please contact your website Administrator"
                                lblMsg.ForeColor = Drawing.Color.Red
                                AppSpecificFunc.WriteLog(ex)
                                SqlTrans.Rollback()
                            End Try
                        End Using

                    End If

                End If
            Catch ex As Exception
                AppSpecificFunc.WriteLog(ex)
                lblMsg.Text = "There is some problem in saving your claim. Please contact your website Administrator"
                lblMsg.ForeColor = Drawing.Color.Red
            End Try
        End If

    End Sub
    Public Sub SendMail(ByVal HeaderId As String)
        Try


            For Each CurrentUser As MembershipUser In Membership.GetAllUsers
                Dim UpdateProfile As ProfileCommon = Profile.GetProfile(CurrentUser.UserName)
                Dim CountryList As String = UpdateProfile.Country()
                Dim SplittedCountry() As String = CountryList.Split("|")
                Dim IsCountryAssigned As Boolean = False
                For Each i In SplittedCountry
                    If i = Session("Country") Then
                        IsCountryAssigned = True
                    End If
                Next
                If Roles.IsUserInRole(CurrentUser.UserName, "Focus") And IsCountryAssigned And CurrentUser.IsApproved Then
                    Dim mailMessage As New System.Net.Mail.MailMessage()
                    mailMessage.[To].Add(CurrentUser.Email)
                    mailMessage.From = New MailAddress("focusclaimsystem@gmail.com")
                    mailMessage.Subject = "New Claim Submitted"
                    mailMessage.Body = "Hello " & CurrentUser.UserName & vbLf & vbLf & "New claim has been submitted with id: " & HeaderId & " (" & txtCountry.Text & ")"
                    mailMessage.Body = mailMessage.Body & vbLf & vbLf & "Thanks & Regards" & vbLf & "Focus Garment Tech Pte Ltd"
                    'Dim smtpClient As New SmtpClient("mail.focus-gmt-tech.com")
                    'smtpClient.Send(mailMessage)
                    'Dim client As New SmtpClient("mail.focus-gmt-tech.com")


                    Dim smtpClient As New SmtpClient("smtp.gmail.com")
                    smtpClient.Port = 587
                    smtpClient.UseDefaultCredentials = False
                    smtpClient.EnableSsl = True

                    Dim nc As New System.Net.NetworkCredential("focusclaimsystem@gmail.com", "Focus25246")
                    smtpClient.Credentials = nc
                    smtpClient.Send(mailMessage)

                End If

            Next

            '' Inform logged in user with mail
            Dim LoggedInUser As MembershipUser = Membership.GetUser(User.Identity.Name)
            Dim mailMessageNew As New System.Net.Mail.MailMessage()
            mailMessageNew.[To].Add(LoggedInUser.Email)
            mailMessageNew.From = New MailAddress("sales@focus-gmt-tech.com")
            mailMessageNew.Subject = "New Claim Submitted"
            mailMessageNew.Body = "Hello " & User.Identity.Name & vbLf & vbLf & " Your new claim has been submitted with id: " & HeaderId & " (" & txtCountry.Text & ")"
            mailMessageNew.Body = mailMessageNew.Body & vbLf & vbLf & "Thanks & Regards" & vbLf & "Focus Garment Tech Pte Ltd"
            'Dim smtpClient As New SmtpClient("mail.focus-gmt-tech.com")
            'smtpClient.Send(mailMessage)
            Dim clientNew As New SmtpClient("smtp.office365.com")
            clientNew.UseDefaultCredentials = False
            clientNew.Credentials = New System.Net.NetworkCredential("sales@focus-gmt-tech.com", "VKowc566")
            clientNew.Host = "smtp.office365.com"
            clientNew.EnableSsl = True
            clientNew.TargetName = "STARTTLS/smtp.office365.com"
            clientNew.Port = 587
            clientNew.Send(mailMessageNew)
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
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
            TempHeaderDataRow("Brand") = txtBrand.Text
            TempHeaderDataRow("ModelNo") = txtMachineModelNo.Text

            If UpdateFlag = True Then
                'TempHeaderDataRow("Date") = txtDate.Text
            Else
                TempHeaderDataRow("Date") = Format(CDate(DateTime.Now), "yyyy-MM-dd")
            End If

            TempHeaderDataRow("Country") = Session("Country")
            TempHeaderDataRow("CountryName") = Session("CountryName")
            TempHeaderDataRow("ShipToCompany") = txtShipToCompany.Text
            TempHeaderDataRow("ShipToAddress") = txtShipToAddress.Text
            TempHeaderDataRow("ShipToContact") = txtShipToContactPerson.Text
            TempHeaderDataRow("ShipToPhone") = txtShipToTelNo.Text
            TempHeaderDataRow("Status") = ddlStatus.SelectedValue



            TempHeaderDataRow("Priority") = ddlPriority.SelectedValue



            TempHeaderDataRow("youtubelink1") = txtYoutubeLink1.Text
            TempHeaderDataRow("youtubelink2") = txtYoutubeLink2.Text
            TempHeaderDataRow("youtubelink3") = txtYoutubeLink3.Text
            TempHeaderDataRow("reasonforrejection") = txtreasonforrejection.Text


            Dim SCB As SqlCommandBuilder = New SqlCommandBuilder(SqlAdapterHeader)
            If UpdateFlag = False Then
                TempHeaderDataRow("CreatedBy") = User.Identity.Name
                TempHeaderDataRow("CreatedOn") = Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss")
                SqlAdapterHeader.Update(LocalTempDataset, "ClaimHeader")
            Else
                SqlAdapterHeader.Update(LocalTempHeaderDataTable)
            End If


            SqlAdapterHeader.Dispose()



            Dim HeaderId As Integer = 0
            Dim ReaderCommand As SqlCommand = New SqlCommand()
            ReaderCommand.Connection = SqlCon
            ReaderCommand.CommandText = "SELECT MAX(id) as id from ClaimHeader " &
                "WHERE ClaimHeader.CreatedBy=@LookUP"
            ReaderCommand.Parameters.AddWithValue("@LookUP", User.Identity.Name)

            Dim reader As SqlDataReader = ReaderCommand.ExecuteReader()

            If UpdateFlag = False Then
                If reader.Read() Then
                    HeaderId = reader("id").ToString()
                End If
            Else
                HeaderId = txtClaimId.Text
            End If


            reader.Close()

            ReaderCommand.Dispose()

            LocalTempDataset.Dispose()

            LocalTempHeaderDataTable.Dispose()

            SqlCon.Close()

            If UpdateFlag = False Then
                For Each CurrentUser As MembershipUser In Membership.GetAllUsers
                    Dim UpdateProfile As ProfileCommon = Profile.GetProfile(CurrentUser.UserName)
                    Dim CountryList As String = UpdateProfile.Country()
                    Dim SplittedCountry() As String = CountryList.Split("|")
                    Dim IsCountryAssigned As Boolean = False
                    For Each i In SplittedCountry
                        If i = Session("Country") Then
                            IsCountryAssigned = True
                        End If
                    Next
                    If Roles.IsUserInRole(CurrentUser.UserName, "Focus") And IsCountryAssigned And CurrentUser.IsApproved Then
                        Dim mailMessage As New System.Net.Mail.MailMessage()
                        mailMessage.[To].Add(CurrentUser.Email)
                        mailMessage.From = New MailAddress("sales@focus-gmt-tech.com")
                        mailMessage.Subject = "New Claim Submitted"
                        mailMessage.Body = "Hello " & CurrentUser.UserName & vbLf & vbLf & "New claim has been submitted with id: " & HeaderId & " (" & txtCountry.Text & ")"
                        mailMessage.Body = mailMessage.Body & vbLf & vbLf & "Thanks & Regards" & vbLf & "Focus Garment Tech Pte Ltd"
                        'Dim smtpClient As New SmtpClient("mail.focus-gmt-tech.com")
                        'smtpClient.Send(mailMessage)
                        Dim client As New SmtpClient("mail.focus-gmt-tech.com")
                        client.UseDefaultCredentials = False
                        client.Credentials = New System.Net.NetworkCredential("sales@focus-gmt-tech.com", "VKowc566")
                        client.Send(mailMessage)
                    End If

                Next

                '' Inform logged in user with mail
                Dim LoggedInUser As MembershipUser = Membership.GetUser(User.Identity.Name)
                Dim mailMessageNew As New System.Net.Mail.MailMessage()
                mailMessageNew.[To].Add(LoggedInUser.Email)
                mailMessageNew.From = New MailAddress("sales@focus-gmt-tech.com")
                mailMessageNew.Subject = "New Claim Submitted"
                mailMessageNew.Body = "Hello " & User.Identity.Name & vbLf & vbLf & " Your new claim has been submitted with id: " & HeaderId & " (" & txtCountry.Text & ")"
                mailMessageNew.Body = mailMessageNew.Body & vbLf & vbLf & "Thanks & Regards" & vbLf & "Focus Garment Tech Pte Ltd"
                'Dim smtpClient As New SmtpClient("mail.focus-gmt-tech.com")
                'smtpClient.Send(mailMessage)
                Dim clientNew As New SmtpClient("mail.focus-gmt-tech.com")
                clientNew.UseDefaultCredentials = False
                clientNew.Credentials = New System.Net.NetworkCredential("sales@focus-gmt-tech.com", "VKowc566")
                clientNew.Send(mailMessageNew)

            End If


            Return HeaderId

        Else
            Return 0
            FormsAuthentication.SignOut()
        End If

    End Function
    Protected Sub SaveClaimPhotoItems(ByVal HeaderId As Integer)




        If Not IsNothing(ViewState("FilesList")) Then

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
            UploadedFilesList = ViewState("FilesList")
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
        If Not IsNothing(ViewState("TempDataTable")) Then

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


            If ViewState("TempDataTable").Rows.Count > 0 Then

                Dim LocalTempDataTable As New DataTable
                Dim SqlAdapter As _
                   New SqlDataAdapter("Select  * From ClaimLineItems", ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString)

                SqlAdapter.FillSchema(ViewState("TempDataset"), SchemaType.Source, "ClaimLineItems")
                SqlAdapter.Fill(LocalTempDataTable)
                'LocalTempDataTable = ViewState("TempDataset").Tables("SqLineItems")
                Dim i As Integer = 0
                For Each DataRowItem As DataRow In ViewState("TempDataTable").Rows
                    Dim DR As DataRow = ViewState("TempDataset").Tables("ClaimLineItems").Rows(i)
                    DR("HeaderId") = HeaderId
                    i = i + 1
                Next
                For Each DataRowItem As DataRow In ViewState("TempDataTable").Rows
                    Dim TempDataRow As DataRow = ViewState("TempDataset").Tables("ClaimLineItems").NewRow()
                    TempDataRow = DataRowItem
                    Dim CB As SqlCommandBuilder = New SqlCommandBuilder(SqlAdapter)
                    SqlAdapter.Update(ViewState("TempDataset"), "ClaimLineItems")
                Next
                SqlAdapter.Dispose()

                ViewState("TempDataTable").Rows.Clear()
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
        If Not IsNothing(ViewState("FilesList")) Then

            If PhotosUpload.HasFile Then
                Dim validFileTypes As String() = {"bmp", "BMP", "gif", "GIF", "png", "PNG", "JPG", "jpg", "JPEG", "jpeg", "PDF", "pdf"}
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
                    lblUploadMsg.Text = "Invalid File. Please upload a File with extension " &
                                    String.Join(",", validFileTypes)
                    Exit Sub
                End If

                Dim intDocFileLength As Integer = Me.PhotosUpload.PostedFile.ContentLength
                Dim strPostedFileName As String = Me.PhotosUpload.PostedFile.FileName

                'If intDocFileLength > 2097152 Then
                '    isValidFile = False
                '    lblUploadMsg.ForeColor = System.Drawing.Color.Red
                '    lblUploadMsg.Text = "Invalid File. Max file size Limit 2MB"
                '    Exit Sub
                'Else
                '    isValidFile = True
                'End If

                Dim UploadedFilesList As List(Of String) = New List(Of String)
                UploadedFilesList = ViewState("FilesList")
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
                        'UploadedFiles.Items.Add("<img scr='data:image/png;base64," & Convert.ToBase64String(File.ReadAllBytes(FileSavePath)) & "'/>" & strPostedFileName & " - Remove")
                        'Dim img As Image = New Image
                        'img.ImageUrl = "data:image/png;base64," & Convert.ToBase64String(File.ReadAllBytes(FileSavePath))
                        'img.Width = 100
                        'img.Height = 100

                        UploadedFiles.Items.Add(strPostedFileName & " - Remove")

                        ViewState("FilesList") = UploadedFilesList
                        LoadThumbnails()
                    End If
                End If

            End If
        Else
            FormsAuthentication.SignOut()
        End If
    End Sub
    Private Sub LoadThumbnails()
        If Not IsNothing(ViewState("FilesList")) Then
            Dim UploadedFilesList As List(Of String) = New List(Of String)
            UploadedFilesList = ViewState("FilesList")
            Dim Pnl As Panel = New Panel
            If UploadedFilesList.Count > 0 Then
                For i As Integer = 0 To UploadedFilesList.Count - 1

                    Dim img As Image = New Image
                    Dim imageFileType As String() = {"bmp", "BMP", "gif", "GIF", "png", "PNG", "JPG", "jpg", "JPEG", "jpeg", "MP4", "mp4", "MTS", "mts", "wmv", "WMV"}
                    Dim ext As String = System.IO.Path.GetExtension(UploadedFilesList.Item(i))
                    Dim isImageFile As Boolean = False
                    For j As Integer = 0 To imageFileType.Length - 1
                        If ext = "." & imageFileType(j) Then
                            isImageFile = True
                            Exit For
                        End If
                    Next

                    If File.Exists(UploadedFilesList.Item(i)) And isImageFile Then
                        Dim lnk As HyperLink = New HyperLink
                        'lnk.NavigateUrl = VirtualPathUtility.ToAbsolute("~") & "/Business/FileHandler.aspx=" & System.IO.Path.GetFileName(UploadedFilesList.Item(i))
                        lnk.NavigateUrl = "FileHandler.aspx?Filename=" & System.IO.Path.GetFileName(UploadedFilesList.Item(i))
                        lnk.Text = "Download"
                        img.ImageUrl = "data:image/png;base64," & Convert.ToBase64String(File.ReadAllBytes(UploadedFilesList.Item(i)))
                        img.Width = 50
                        img.Height = 50
                        Pnl.Controls.Add(img)
                        Pnl.Controls.Add(lnk)
                    ElseIf isImageFile Then

                        img.ImageUrl = "data:image/png;base64," & Convert.ToBase64String(File.ReadAllBytes(Server.MapPath("~") & "\" & System.Configuration.ConfigurationManager.AppSettings("TempUploadPath").ToString & "\empty.png"))
                        img.Width = 50
                        img.Height = 50
                        Pnl.Controls.Add(img)

                    Else
                        Dim lnk As HyperLink = New HyperLink
                        'lnk.NavigateUrl = VirtualPathUtility.ToAbsolute("~") & "/Business/FileHandler.aspx=" & System.IO.Path.GetFileName(UploadedFilesList.Item(i))
                        lnk.NavigateUrl = "FileHandler.aspx?Filename=" & System.IO.Path.GetFileName(UploadedFilesList.Item(i))
                        lnk.Text = "Download"
                        img.ImageUrl = "data:image/png;base64," & Convert.ToBase64String(File.ReadAllBytes(Server.MapPath("~") & "\" & System.Configuration.ConfigurationManager.AppSettings("TempUploadPath").ToString & "\video.png"))
                        img.Width = 50
                        img.Height = 50
                        Pnl.Controls.Add(img)
                        Pnl.Controls.Add(lnk)
                    End If

                    UploadedThumbs.Controls.Add(Pnl)

                    isImageFile = False
                    'img.ImageUrl = "data:image/png;base64," & Convert.ToBase64String(File.ReadAllBytes(UploadedFilesList.Item(i)))

                Next
            End If
        End If
    End Sub
    Protected Sub UploadedFiles_Click(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.BulletedListEventArgs) Handles UploadedFiles.Click
        If Not IsNothing(ViewState("FilesList")) Then
            If e.Index > -1 Then
                UploadedFiles.Items.RemoveAt(e.Index)
                Dim UploadedFilesList As List(Of String) = New List(Of String)
                UploadedFilesList = ViewState("FilesList")
                Dim FileSavePath As String = String.Empty
                FileSavePath = UploadedFilesList.Item(e.Index)
                If System.IO.File.Exists(FileSavePath) Then
                    System.IO.File.Delete(FileSavePath)
                End If
                UploadedFilesList.RemoveAt(e.Index)
                ViewState("FilesList") = UploadedFilesList
                LoadThumbnails()
            End If
        Else
            FormsAuthentication.SignOut()
        End If
    End Sub
    Private Sub ClearFormFields()

        txtDearerCustomerName.Text = String.Empty
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
        txtYoutubeLink1.Text = String.Empty
        txtYoutubeLink2.Text = String.Empty
        txtYoutubeLink3.Text = String.Empty
        txtreasonforrejection.Text = String.Empty
    End Sub
    '**** old
    'Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
    '    If Page.IsValid Then
    '        Dim HeaderId As Integer = 0
    '        HeaderId = CreateClaim()
    '        If HeaderId = 0 Then
    '            FormsAuthentication.SignOut()
    '        Else
    '            SaveClaimItems(HeaderId)
    '            SaveClaimPhotoItems(HeaderId)
    '            ClearFormFields()
    '            lblMsg.Text = "Claim Tracking No : " & HeaderId & " has been sucessfully submitted."
    '        End If
    '    End If

    'End Sub

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
        td.Text = "<h3><u>Focus Garment Tech Pte Ltd</u></h3>"
        td.ColumnSpan = 10
        td.HorizontalAlign = HorizontalAlign.Center
        tr.Cells.Add(td)
        HeaderTable.Rows.Add(tr)


        Dim tr1 As TableRow = New TableRow

        Dim tdEndCustomerNamelbl As TableCell = New TableCell
        Dim tdEndCustomerNameValue As TableCell = New TableCell

        Dim tdClaimIdlbl As TableCell = New TableCell
        Dim tdClaimIdValue As TableCell = New TableCell

        'tdEndCustomerNamelbl.Text = "<b>End Customer Name</b>"
        tdEndCustomerNamelbl.Text = ""
        tdEndCustomerNamelbl.HorizontalAlign = HorizontalAlign.Left
        tdEndCustomerNameValue.Text = txtEndCustomerName.Text
        tdEndCustomerNameValue.Text = ""
        tdEndCustomerNameValue.HorizontalAlign = HorizontalAlign.Left
        tdEndCustomerNameValue.ColumnSpan = 7

        tdClaimIdlbl.Text = "<b>Claim ID</b>"
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

        'tdPersonInChargeValue.Text = txtPersonincharge.Text
        tdPersonInChargeValue.Text = "Mr Ho / Mr John Ho"
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
        'tdEmailValue.Text = txtEmailAddress.Text        
        tdEmailValue.Text = "kn.ho@focus-gmt-tech.com / john.ho@focus-gmt-tech.com"
        tdEmailValue.HorizontalAlign = HorizontalAlign.Left
        tdEmailValue.ColumnSpan = 7


        tdCountrylbl.Text = "<b>Country</b>"
        tdCountrylbl.HorizontalAlign = HorizontalAlign.Right
        tdCountryValue.Text = txtCountry.Text
        tdCountryValue.HorizontalAlign = HorizontalAlign.Left

        tr3.Cells.Add(tdEmaillbl)
        tr3.Cells.Add(tdEmailValue)
        'tr3.Cells.Add(tdCountrylbl)
        'tr3.Cells.Add(tdCountryValue)

        Dim tr4 As TableRow = New TableRow

        Dim tdTelNolbl As TableCell = New TableCell
        Dim tdTelNoValue As TableCell = New TableCell


        tdTelNolbl.Text = "<b>TelNo</b>"
        tdTelNolbl.HorizontalAlign = HorizontalAlign.Left
        'tdTelNoValue.Text = txtTelno.Text
        tdTelNoValue.Text = "65-63382256"
        tdTelNoValue.HorizontalAlign = HorizontalAlign.Left
        tdTelNoValue.ColumnSpan = 7
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


        Dim i As Integer = 0
        For Each DetailDR As TableRow In DetailsTable.Rows
            DetailDR.BorderColor = Drawing.ColorTranslator.FromHtml("#000000")
            DetailDR.BorderStyle = BorderStyle.Solid
            DetailDR.BorderWidth = 1

            Dim j As Integer = 0
            For Each DetailCL As TableCell In DetailDR.Cells
                'If j = 8 Then
                '    DetailCL.ColumnSpan = 2
                'End If
                If (j = 7) Then

                    DetailCL.HorizontalAlign = HorizontalAlign.Center
                End If

                j = j + 1


            Next
            If i = 0 Then
                DetailDR.Cells(1).Text = "PO Dated" ' Invoice Date`
                DetailDR.Cells(2).Text = "PO No"    ' Invoice No
                DetailDR.Cells(3).Text = "Machine Model" ' Qty
                DetailDR.Cells(0).Style.Add("width", "25px !important")

            Else
                DetailDR.Cells(1).Text = ""
                DetailDR.Cells(2).Text = ""
                DetailDR.Cells(3).Text = txtMachineModelNo.Text
                DetailDR.Cells(0).Style.Add("width", "25px !important")
            End If
            'DetailDR.Cells(0).ColumnSpan = 1  ' SNo
            'DetailDR.Cells(4).ColumnSpan = 2  ' Machine Serial No
            'DetailDR.Cells(5).ColumnSpan = 1  ' Claim Qty
            DetailDR.Cells(7).ColumnSpan = 3  ' Description



            DetailDR.Cells(6).Visible = False ' Open Qty
            DetailDR.Cells(8).Visible = False ' Claim mode
            DetailDR.Cells(9).Visible = False ' Date
            DetailDR.Cells(10).Visible = False ' Claim Reference
            DetailDR.Cells(11).Visible = False ' Date


            i = i + 1
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
        tdEmptycells1.ColumnSpan = 7
        tdDeliveryDetailsHeader.Text = "<h4>Delivery Details</h4>"
        tdDeliveryDetailsHeader.HorizontalAlign = HorizontalAlign.Left
        tdDeliveryDetailsHeader.ColumnSpan = 3

        trf1.Cells.Add(tdEmptycells1)
        trf1.Cells.Add(tdDeliveryDetailsHeader)

        Dim trf2 As TableRow = New TableRow

        Dim tdEmptycells2 As TableCell = New TableCell
        Dim tdShiptoCompanylbl As TableCell = New TableCell
        Dim tdShiptoCompanyValue As TableCell = New TableCell



        tdEmptycells2.Text = String.Empty
        tdEmptycells2.HorizontalAlign = HorizontalAlign.Left
        tdEmptycells2.ColumnSpan = 6
        tdEmptycells2.Text = "<b>Remarks</b>"
        tdShiptoCompanylbl.Text = "<b>Ship to company</b>"
        tdShiptoCompanylbl.HorizontalAlign = HorizontalAlign.Left

        tdShiptoCompanyValue.Text = txtShipToCompany.Text
        tdShiptoCompanyValue.HorizontalAlign = HorizontalAlign.Left
        tdShiptoCompanyValue.ColumnSpan = 3



        trf2.Cells.Add(tdEmptycells2)
        trf2.Cells.Add(tdShiptoCompanylbl)
        trf2.Cells.Add(tdShiptoCompanyValue)

        Dim trf3 As TableRow = New TableRow

        Dim tdEmptycells3 As TableCell = New TableCell
        Dim tdShiptoAddresslbl As TableCell = New TableCell
        Dim tdShiptoAddressValue As TableCell = New TableCell



        tdEmptycells3.Text = String.Empty
        tdEmptycells3.HorizontalAlign = HorizontalAlign.Left
        tdEmptycells3.ColumnSpan = 6
        'tdEmptycells3.Text = txtreasonforrejection.Text
        tdEmptycells3.Text=String.Empty
        tdShiptoAddresslbl.Text = "<b>Ship to Address</b>"
        tdShiptoAddresslbl.HorizontalAlign = HorizontalAlign.Left
        tdShiptoAddresslbl.VerticalAlign = VerticalAlign.Top
        tdShiptoAddressValue.Text = txtShipToAddress.Text.Replace(vbLf, "<br/>")
        tdShiptoAddressValue.HorizontalAlign = HorizontalAlign.Left
        tdShiptoAddressValue.ColumnSpan = 3


        trf3.Cells.Add(tdEmptycells3)
        trf3.Cells.Add(tdShiptoAddresslbl)
        trf3.Cells.Add(tdShiptoAddressValue)


        Dim trf4 As TableRow = New TableRow

        Dim tdEmptycells4 As TableCell = New TableCell
        Dim tdShiptoContactPersonlbl As TableCell = New TableCell
        Dim tdShiptoContactPersonValue As TableCell = New TableCell



        tdEmptycells4.Text = String.Empty
        tdEmptycells4.HorizontalAlign = HorizontalAlign.Left
        tdEmptycells4.ColumnSpan = 6
        tdShiptoContactPersonlbl.Text = "<b>Contact Person</b>"
        tdShiptoContactPersonlbl.HorizontalAlign = HorizontalAlign.Left
        'tdShiptoContactPersonlbl.VerticalAlign = VerticalAlign.Top
        tdShiptoContactPersonValue.Text = txtShipToContactPerson.Text
        tdShiptoContactPersonValue.HorizontalAlign = HorizontalAlign.Left
        tdShiptoContactPersonValue.ColumnSpan = 3


        trf4.Cells.Add(tdEmptycells4)
        trf4.Cells.Add(tdShiptoContactPersonlbl)
        trf4.Cells.Add(tdShiptoContactPersonValue)

        Dim trf5 As TableRow = New TableRow

        Dim tdEmptycells5 As TableCell = New TableCell
        Dim tdShiptoTelnolbl As TableCell = New TableCell
        Dim tdShiptoTelnoValue As TableCell = New TableCell



        tdEmptycells5.Text = String.Empty
        tdEmptycells5.HorizontalAlign = HorizontalAlign.Left
        tdEmptycells5.ColumnSpan = 6
        tdShiptoTelnolbl.Text = "<b>Tel No</b>"
        tdShiptoTelnolbl.HorizontalAlign = HorizontalAlign.Left
        'tdShiptoContactPersonlbl.VerticalAlign = VerticalAlign.Top
        tdShiptoTelnoValue.Text = txtShipToTelNo.Text
        tdShiptoTelnoValue.HorizontalAlign = HorizontalAlign.Left
        tdShiptoTelnoValue.ColumnSpan = 3


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
        ' CompanyTitleTable.Rows.Add(trTitle)
        'CompanyTitleTable.RenderControl(hw)

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
        For k As Integer = 1 To DgridView.HeaderRow.Cells.Count - 4

            If DgridView.HeaderRow.Cells(k).Visible = True Then
                Dim Dcell As TableCell = New TableCell
                Dcell.Text = DgridView.HeaderRow.Cells(k).Text
                Dcell.BackColor = Drawing.ColorTranslator.FromHtml("#5D7B9D")
                Drow.Cells.Add(Dcell)
            End If
        Next
        ResultTable.Rows.Add(Drow)
        For Each GRow As GridViewRow In DgridView.Rows
            Drow = New TableRow
            For j As Integer = 1 To GRow.Cells.Count - 4
                Dim Dcell As TableCell = New TableCell
                'GRow.Cells(j).Controls()
                Dcell.HorizontalAlign = HorizontalAlign.Left
                If GRow.Cells(j).Controls().Count > 0 Then
                    If GRow.Cells(j).Visible = True Then
                        For Each ctrl As Control In GRow.Cells(j).Controls()
                            If TypeOf (ctrl) Is TextBox Then
                                Dim Tbox As TextBox = ctrl
                                Dcell.Text = Tbox.Text
                            End If
                        Next
                    End If

                Else
                    If GRow.Cells(j).Visible = True Then
                        Dcell.Text = GRow.Cells(j).Text
                    End If
                End If

                Drow.Cells.Add(Dcell)
            Next
            ResultTable.Rows.Add(Drow)
        Next
        Return ResultTable
    End Function

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Response.Redirect("~/Business/Claimform.aspx", True)
    End Sub

    Protected Sub ClaimItemList_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles ClaimItemList.RowDeleting

    End Sub

    Protected Sub ClaimItemList_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ClaimItemList.SelectedIndexChanged

    End Sub


    Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click
        Session("ClaimId") = txtFind.Text
        Response.Redirect("~/Business/Claimform.aspx", True)
    End Sub
    Protected Sub RowCountValidator_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles RowCountValidator.ServerValidate
        If ClaimItemList.Rows.Count > 0 Then
            args.IsValid = True
        Else
            args.IsValid = False
        End If
    End Sub

    Protected Sub btnDuplicate_Click(sender As Object, e As System.EventArgs) Handles btnDuplicate.Click
        txtClaimId.Text = "New"
        InitializeTempDataTable()
        UploadedFiles.Items.Clear()
        txtYoutubeLink1.Text = String.Empty
        txtYoutubeLink2.Text = String.Empty
        txtYoutubeLink3.Text = String.Empty
        txtBrand.Text = String.Empty
        txtMachineModelNo.Text = String.Empty
    End Sub


    Protected Sub BtnAddRemarks_Click(sender As Object, e As System.EventArgs) Handles BtnAddRemarks.Click
        Try
            If IsNumeric(txtClaimId.Text) And Trim(txtRemarks.Text) <> String.Empty Then
                LoadThumbnails()
                Dim CR As New ClaimRemarks
                CR.ClaimId = txtClaimId.Text
                CR.CreatedBy = User.Identity.Name
                CR.Remarks = Trim(txtRemarks.Text)
                CURD.InsertData(CR)
                lblMsg.Text = "Remark added sucessfully"
                lblMsg.ForeColor = Drawing.Color.Green
                LoadRemarks(CInt(txtClaimId.Text))
                txtRemarks.Text = String.Empty

            Else
                LoadThumbnails()
                 
                    lblMsg.Text = "Please save claim form before adding Remark"
                    lblMsg.ForeColor = Drawing.Color.Red


            End If

        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
            lblMsg.Text = "There is some problem in adding your remark. Please contact your website Administrator"
            lblMsg.ForeColor = Drawing.Color.Red
        End Try
    End Sub
    Protected Function LoadRemarks(ByVal ClaimId As Integer) As DataTable
        Try

            Dim ResultDataTable As New DataTable
            Dim CR As New ClaimRemarks
            CR.ClaimId = ClaimId


            Dim SQ As New SelectQuery



            SQ._InputTable = CR
            SQ._DB = "Custom"
            SQ._HasInBetweenConditions = False
            SQ._HasWhereConditions = True

            'Query Conditions List
            Dim QryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

            'Query Condition Groups
            Dim ConditionsGrp1 As List(Of String) = New List(Of String)

            'Query Conditions values
            ConditionsGrp1.Add("ClaimId=@ClaimId")

            QryConditions.Add(" AND ", ConditionsGrp1)

            SQ._Conditions = QryConditions
            SQ._OrderBy = " CreatedOn Desc"


            ResultDataTable = CURD.SelectAllData(SQ)

            If Not IsNothing(ResultDataTable) Then
                AppSpecificFunc.BindGridData(ResultDataTable, ItemDetailsGrid)
            End If

            Return ResultDataTable
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
            Return Nothing
        End Try
    End Function

    Protected Sub btnCopyToDO_Click(sender As Object, e As System.EventArgs) Handles btnCopyToDO.Click
        Try
            If Page.IsValid Then
                Me.Context.Items.Add("ClaimId", txtClaimId.Text)
                Server.Transfer("~/Business/DeliveryOrder.aspx", True)
            End If
        Catch ex As Exception When Not TypeOf ex Is ThreadAbortException
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub


End Class
