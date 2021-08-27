Imports Models
Imports System.Data
Imports System.IO
Imports System.Reflection
Imports System.Data.SqlClient
Imports System.Globalization
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class Business_DeliveryOrder
    Inherits System.Web.UI.Page
#Region "Custom Validation function"
    Protected Sub RowCountValidator_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles RowCountValidator.ServerValidate
        If DeliveryItemListGrid.Rows.Count > 0 Then
            args.IsValid = True
        Else
            args.IsValid = False
        End If
    End Sub
#End Region
#Region "Form Related functions"
    Protected Sub ClearForm()
        txtEndCustomerName.Text = String.Empty
        txtPersonincharge.Text = String.Empty
        txtEmailAddress.Text = String.Empty
        txtTelno.Text = String.Empty
        txtBrand.Text = String.Empty
        txtMachineModelNo.Text = String.Empty
        txtDate.Text = String.Empty
        txtCountry.Text = String.Empty
        ddlPriority.SelectedIndex = 0
        ddlStatus.SelectedIndex = 0
        txtYoutubeLink1.Text = String.Empty
        txtYoutubeLink2.Text = String.Empty
        txtYoutubeLink3.Text = String.Empty
        txtShipToCompany.Text = String.Empty
        txtShipToAddress.Text = String.Empty
        txtShipToContactPerson.Text = String.Empty
        txtShipToTelNo.Text = String.Empty
        txtreasonforrejection.Text = String.Empty
    End Sub
    Protected Sub DocStatusBasedButtonRestrictions()
        Try
            If User.IsInRole("Focus") Or User.IsInRole("admin") Then
                txtEndCustomerName.Enabled = True
                txtPersonincharge.Enabled = True
                txtEmailAddress.Enabled = True
                txtTelno.Enabled = True
                txtBrand.Enabled = True
                txtMachineModelNo.Enabled = True
                txtDate.Enabled = True
                txtCountry.Enabled = False
                ddlPriority.Enabled = True
                txtYoutubeLink1.Enabled = True
                txtYoutubeLink2.Enabled = True
                txtYoutubeLink3.Enabled = True
                txtShipToCompany.Enabled = True
                txtShipToAddress.Enabled = True
                txtShipToContactPerson.Enabled = True
                txtShipToTelNo.Enabled = True
                txtreasonforrejection.Enabled = True
            Else
                txtEndCustomerName.Enabled = False
                txtPersonincharge.Enabled = False
                txtEmailAddress.Enabled = False
                txtTelno.Enabled = False
                txtBrand.Enabled = False
                txtMachineModelNo.Enabled = False
                txtDate.Enabled = False
                txtCountry.Enabled = False
                ddlPriority.Enabled = False
                txtYoutubeLink1.Enabled = False
                txtYoutubeLink2.Enabled = False
                txtYoutubeLink3.Enabled = False
                txtShipToCompany.Enabled = False
                txtShipToAddress.Enabled = False
                txtShipToContactPerson.Enabled = False
                txtShipToTelNo.Enabled = False
                txtreasonforrejection.Enabled = False
            End If
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
    Private Sub LoadThumbnails(ByVal UploadedFilesList As List(Of String))
        If Not IsNothing(UploadedFilesList) Then

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
    Protected Sub FindClaimDocument(ByVal DocNoValue As String)
        Try
            Dim ClaimHeaderObj As New ClaimHeader
            Dim IdKey As String = DocNoValue

            If LoadClaimHeaderFromDataBase(IdKey) Then
                LoadClaimItemsFromDataBase(IdKey)
                LoadClaimUploadedFilesFromDataBase(IdKey)
                LoadClaimRemarksFromDataBase(IdKey)
                LoadItemsFromDataBase(-1)
                CopyClaimLinesToDOLines()

                DocStatusBasedButtonRestrictions()
                '    End If
            Else
                '    InitialiseForm()
                '    'DocStatusBasedButtonRestrictions()
                '    'ShowMessage("Document not found")
            End If
            txtClaimId.Text = DocNoValue
            txtDOId.Text = "New"
            ViewState("ClaimId") = DocNoValue
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
    Protected Sub FindDODocument(ByVal DocNoValue As String)
        Try
            Dim DOHeaderObj As New DOHeader
            Dim IdKey As String = DocNoValue

            If LoadHeaderFromDataBase(IdKey) Then
                txtDOId.Text = DocNoValue
                LoadItemsFromDataBase(IdKey)
                LoadDOUploadedFilesFromDataBase(IdKey)
                LoadDORemarksFromDataBase(IdKey)


                DocStatusBasedButtonRestrictions()
                '    End If
            Else
                '    InitialiseForm()
                '    'DocStatusBasedButtonRestrictions()
                '    'ShowMessage("Document not found")
            End If


        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
    Protected Sub CopyClaimLinesToDOLines()
        Dim DOLinesDataTable As DataTable = TryCast(ViewState("DOLines"), DataTable)
        Dim ClaimLinesDataTable As DataTable = TryCast(ViewState("ClaimLines"), DataTable)
        Try


            Dim Obj As New Models.ClaimLineItems
            Dim props As Type = Obj.GetType
            For Each DRow In ClaimLinesDataTable.Rows
                Dim DORow As DataRow = DOLinesDataTable.NewRow

                For Each member As PropertyInfo In props.GetProperties
                    If Not IsNothing(DRow(member.Name)) Then
                        If member.Name <> "OpenQty" Then
                            If member.Name = "SNo" Then
                                DORow("ClaimSNo") = DRow(member.Name)

                            Else
                                DORow(member.Name) = DRow(member.Name)
                            End If
                        Else
                            DORow("DOQty") = DRow(member.Name)
                        End If
                    End If
                Next
                DOLinesDataTable.Rows.Add(DORow)
            Next
            Dim i As Integer = 1
            For Each Drow As DataRow In DOLinesDataTable.Rows
                Drow("SNo") = i
                Drow.AcceptChanges()
                i = i + 1
            Next
            ViewState("DOLines") = DOLinesDataTable
            Dim GridDataTable As DataTable = TryCast(ViewState("DOLines"), DataTable)
            If Not IsNothing(GridDataTable) Then
                If GridDataTable.Rows.Count >= 0 Then
                    AppSpecificFunc.BindGridData(GridDataTable, DeliveryItemListGrid)
                End If
            End If
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
    Protected Sub LoadHeaderValues(ByVal DT As DataTable, ISDOHeader As Boolean)
        Try

            If DT.Rows.Count > 0 Then
                Dim DRow As DataRow = DT.Rows(0)
                If ISDOHeader = True Then
                    txtClaimId.Text = DRow("ClaimId").ToString
                End If
                txtEndCustomerName.Text = DRow("CustomerName").ToString
                txtPersonincharge.Text = DRow("Personincharge").ToString
                txtEmailAddress.Text = DRow("EmailAddress").ToString
                txtTelno.Text = DRow("PhoneNo").ToString
                txtBrand.Text = DRow("Brand").ToString
                txtMachineModelNo.Text = DRow("ModelNo").ToString
                txtDate.Text = DRow("DocDate").ToString
                txtCountry.Text = DRow("CountryName").ToString
                ddlPriority.SelectedIndex = ddlPriority.Items.IndexOf(ddlPriority.Items.FindByValue(DRow("Priority").ToString))
                If ISDOHeader = True Then
                    ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByValue(DRow("Status").ToString))

                    If ddlStatus.SelectedItem.Text = "Cancelled" Then
                        btnCancel.Enabled = False
                        btnSave.Enabled = False
                    End If
                    If Not IsDBNull(DRow("DocDate")) Then
                        Dim Today As Date = DRow("DocDate")
                        txtDate.Text = Today.Date.ToString("dd-MM-yyyy")
                    End If
                End If


                txtYoutubeLink1.Text = DRow("youtubelink1").ToString
                txtYoutubeLink2.Text = DRow("youtubelink2").ToString
                txtYoutubeLink3.Text = DRow("youtubelink3").ToString
                txtShipToCompany.Text = DRow("ShipToCompany").ToString
                txtShipToAddress.Text = DRow("ShipToAddress").ToString
                txtShipToContactPerson.Text = DRow("ShipToContact").ToString
                txtShipToTelNo.Text = DRow("ShipToPhone").ToString
                txtreasonforrejection.Text = DRow("reasonforrejection").ToString
            Else

            End If
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub

    Protected Function LoadClaimHeaderFromDataBase(ByVal IdKey As String) As Boolean

        Try
            Dim ReturnResult As Boolean = False
            Dim ClaimHeaderObj As New ClaimHeader
            Dim ErrMsg As String = String.Empty
            Dim SQ As New SelectQuery

            'Where Condition parameter
            ClaimHeaderObj.id = IdKey

            SQ._InputTable = ClaimHeaderObj
            SQ._DB = "Custom"
            SQ._HasInBetweenConditions = False
            SQ._HasWhereConditions = True

            'Query Conditions List
            Dim QryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

            'Query Condition Groups
            Dim ConditionsGrp1 As List(Of String) = New List(Of String)

            'Query Conditions values
            ConditionsGrp1.Add("id=@id")

            QryConditions.Add(" AND ", ConditionsGrp1)

            SQ._Conditions = QryConditions


            Dim ResultDataTable As New DataTable
            ResultDataTable = CURD.SelectAllData(SQ)



            If ResultDataTable.Rows.Count > 0 Then
                ReturnResult = True
                LoadHeaderValues(ResultDataTable, False)

            Else

                ReturnResult = False
            End If
            Return ReturnResult
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
            Return False
        End Try
    End Function
    Protected Sub LoadClaimItemsFromDataBase(ByVal IdKey As String)

        Try

            Dim ClaimLineItemsObj As New ClaimLineItems
            Dim ErrMsg As String = String.Empty
            Dim SQB As New SelectQuery

            'Where Condition parameter
            ClaimLineItemsObj.Headerid = IdKey
            ClaimLineItemsObj.OpenQty = 0

            SQB._InputTable = ClaimLineItemsObj
            SQB._DB = "Custom"
            SQB._HasInBetweenConditions = False
            SQB._HasWhereConditions = True

            'Query Conditions List
            Dim QryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

            'Query Condition Groups
            Dim ConditionsGrp1 As List(Of String) = New List(Of String)

            'Query Conditions values
            ConditionsGrp1.Add("Headerid=@Headerid")
            ConditionsGrp1.Add("OpenQty>@OpenQty")
            QryConditions.Add(" AND ", ConditionsGrp1)

            SQB._Conditions = QryConditions


            Dim ResultDataTable As New DataTable
            ResultDataTable = CURD.SelectAllData(SQB)

            If Not IsNothing(ResultDataTable) Then
                ViewState("ClaimLines") = ResultDataTable.Copy()
                'AppSpecificFunc.BindGridData(ResultDataTable, DeliveryItemListGrid)
            End If


        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
    Protected Sub LoadClaimUploadedFilesFromDataBase(ByVal IdKey As String)
        Try
            Dim FilesObj As New Files
            Dim ErrMsg As String = String.Empty
            Dim SQB As New SelectQuery

            'Where Condition parameter
            FilesObj.Headerid = IdKey

            SQB._InputTable = FilesObj
            SQB._DB = "Custom"
            SQB._HasInBetweenConditions = False
            SQB._HasWhereConditions = True

            'Query Conditions List
            Dim QryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

            'Query Condition Groups
            Dim ConditionsGrp1 As List(Of String) = New List(Of String)

            'Query Conditions values
            ConditionsGrp1.Add("Headerid=@Headerid")

            QryConditions.Add(" AND ", ConditionsGrp1)

            SQB._Conditions = QryConditions


            Dim ResultDataTable As New DataTable
            ResultDataTable = CURD.SelectAllData(SQB)
            Dim UploadedFilesList As List(Of String) = New List(Of String)

            If Not IsNothing(ResultDataTable) Then
                If ResultDataTable.Rows.Count > 0 Then
                    For Each ro As DataRow In ResultDataTable.Rows
                        UploadedFilesList.Add(ro("Path"))
                        UploadedFiles.Items.Add(System.IO.Path.GetFileName(ro("Path")) & " - Remove")
                    Next
                    LoadThumbnails(UploadedFilesList)
                    ViewState("UploadedFilesLines") = UploadedFilesList
                Else
                    ViewState("UploadedFilesLines") = New List(Of String)
                End If
            Else
                ViewState("UploadedFilesLines") = New List(Of String)
            End If

        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
    Protected Sub LoadDOUploadedFilesFromDataBase(ByVal IdKey As String)
        Try
            Dim FilesObj As New DOFiles
            Dim ErrMsg As String = String.Empty
            Dim SQB As New SelectQuery

            'Where Condition parameter
            FilesObj.Headerid = IdKey

            SQB._InputTable = FilesObj
            SQB._DB = "Custom"
            SQB._HasInBetweenConditions = False
            SQB._HasWhereConditions = True

            'Query Conditions List
            Dim QryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

            'Query Condition Groups
            Dim ConditionsGrp1 As List(Of String) = New List(Of String)

            'Query Conditions values
            ConditionsGrp1.Add("Headerid=@Headerid")

            QryConditions.Add(" AND ", ConditionsGrp1)

            SQB._Conditions = QryConditions


            Dim ResultDataTable As New DataTable
            ResultDataTable = CURD.SelectAllData(SQB)
            Dim UploadedFilesList As List(Of String) = New List(Of String)

            If Not IsNothing(ResultDataTable) Then
                If ResultDataTable.Rows.Count > 0 Then
                    For Each ro As DataRow In ResultDataTable.Rows
                        UploadedFilesList.Add(ro("Path"))
                        UploadedFiles.Items.Add(System.IO.Path.GetFileName(ro("Path")) & " - Remove")
                    Next
                    LoadThumbnails(UploadedFilesList)
                    ViewState("UploadedFilesLines") = UploadedFilesList
                Else
                    ViewState("UploadedFilesLines") = New List(Of String)
                End If
            Else
                ViewState("UploadedFilesLines") = New List(Of String)
            End If

        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
    Protected Sub LoadClaimRemarksFromDataBase(ByVal IdKey As String)

        Try

            Dim GoodsReturnLinesObj As New ClaimRemarks
            Dim ErrMsg As String = String.Empty
            Dim SQB As New SelectQuery

            'Where Condition parameter
            GoodsReturnLinesObj.ClaimId = IdKey

            SQB._InputTable = GoodsReturnLinesObj
            SQB._DB = "Custom"
            SQB._HasInBetweenConditions = False
            SQB._HasWhereConditions = True

            'Query Conditions List
            Dim QryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

            'Query Condition Groups
            Dim ConditionsGrp1 As List(Of String) = New List(Of String)

            'Query Conditions values
            ConditionsGrp1.Add("ClaimId=@ClaimId")

            QryConditions.Add(" AND ", ConditionsGrp1)

            SQB._Conditions = QryConditions


            Dim ResultDataTable As New DataTable
            ResultDataTable = CURD.SelectAllData(SQB)

            If Not IsNothing(ResultDataTable) Then
                ViewState("DORemarkLines") = ResultDataTable.Copy()
                AppSpecificFunc.BindGridData(ResultDataTable, ItemDetailsGrid)
            End If


        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
    Protected Sub LoadDORemarksFromDataBase(ByVal IdKey As String)

        Try

            Dim DORemarksObj As New DORemarks
            Dim ErrMsg As String = String.Empty
            Dim SQB As New SelectQuery

            'Where Condition parameter
            DORemarksObj.ClaimId = IdKey

            SQB._InputTable = DORemarksObj
            SQB._DB = "Custom"
            SQB._HasInBetweenConditions = False
            SQB._HasWhereConditions = True

            'Query Conditions List
            Dim QryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

            'Query Condition Groups
            Dim ConditionsGrp1 As List(Of String) = New List(Of String)

            'Query Conditions values
            ConditionsGrp1.Add("ClaimId=@ClaimId")

            QryConditions.Add(" AND ", ConditionsGrp1)

            SQB._Conditions = QryConditions


            Dim ResultDataTable As New DataTable
            ResultDataTable = CURD.SelectAllData(SQB)

            If Not IsNothing(ResultDataTable) Then
                ViewState("DORemarkLines") = ResultDataTable.Copy()
                AppSpecificFunc.BindGridData(ResultDataTable, ItemDetailsGrid)
            End If


        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
    Protected Function LoadHeaderFromDataBase(ByVal IdKey As String) As Boolean

        Try
            Dim ReturnResult As Boolean = False
            Dim DOHeaderObj As New DOHeader
            Dim ErrMsg As String = String.Empty
            Dim SQ As New SelectQuery

            'Where Condition parameter
            DOHeaderObj.id = IdKey

            SQ._InputTable = DOHeaderObj
            SQ._DB = "Custom"
            SQ._HasInBetweenConditions = False
            SQ._HasWhereConditions = True

            'Query Conditions List
            Dim QryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

            'Query Condition Groups
            Dim ConditionsGrp1 As List(Of String) = New List(Of String)

            'Query Conditions values
            ConditionsGrp1.Add("id=@id")

            QryConditions.Add(" AND ", ConditionsGrp1)

            SQ._Conditions = QryConditions


            Dim ResultDataTable As New DataTable
            ResultDataTable = CURD.SelectAllData(SQ)



            If ResultDataTable.Rows.Count > 0 Then
                ReturnResult = True
                LoadHeaderValues(ResultDataTable, True)
            Else
                ReturnResult = False
            End If
            Return ReturnResult
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
            Return False
        End Try
    End Function
    Protected Function LoadItemsFromDataBase(ByVal IdKey As String) As DataTable

        Try

            Dim DOLineItemsObj As New DOLineItems
            Dim ErrMsg As String = String.Empty
            Dim SQB As New SelectQuery

            'Where Condition parameter
            DOLineItemsObj.Headerid = IdKey

            SQB._InputTable = DOLineItemsObj
            SQB._DB = "Custom"
            SQB._HasInBetweenConditions = False
            SQB._HasWhereConditions = True

            'Query Conditions List
            Dim QryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

            'Query Condition Groups
            Dim ConditionsGrp1 As List(Of String) = New List(Of String)

            'Query Conditions values
            ConditionsGrp1.Add("Headerid=@Headerid")

            QryConditions.Add(" AND ", ConditionsGrp1)

            SQB._Conditions = QryConditions


            Dim ResultDataTable As New DataTable
            ResultDataTable = CURD.SelectAllData(SQB)

            If Not IsNothing(ResultDataTable) Then
                ViewState("DOLines") = ResultDataTable.Copy()
                AppSpecificFunc.BindGridData(ResultDataTable, DeliveryItemListGrid)
            End If

            Return ResultDataTable
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
            Return Nothing
        End Try
    End Function
    Protected Sub LoadRemarksFromDataBase(ByVal IdKey As String)

        Try

            Dim DORemarksObj As New DORemarks
            Dim ErrMsg As String = String.Empty
            Dim SQB As New SelectQuery

            'Where Condition parameter
            DORemarksObj.ClaimId = IdKey

            SQB._InputTable = DORemarksObj
            SQB._DB = "Custom"
            SQB._HasInBetweenConditions = False
            SQB._HasWhereConditions = True

            'Query Conditions List
            Dim QryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

            'Query Condition Groups
            Dim ConditionsGrp1 As List(Of String) = New List(Of String)

            'Query Conditions values
            ConditionsGrp1.Add("ClaimId=@ClaimId")

            QryConditions.Add(" AND ", ConditionsGrp1)

            SQB._Conditions = QryConditions


            Dim ResultDataTable As New DataTable
            ResultDataTable = CURD.SelectAllData(SQB)

            If Not IsNothing(ResultDataTable) Then
                ViewState("DORemarkLines") = ResultDataTable.Copy()
                AppSpecificFunc.BindGridData(ResultDataTable, ItemDetailsGrid)
            End If


        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
    Protected Sub InitialiseForm()
        Try
            ClearForm()
            LoadItemsFromDataBase(-1)
            LoadRemarksFromDataBase(-1)
            Dim UploadedFilesList As List(Of String) = New List(Of String)
            Session("FilesList") = UploadedFilesList
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
    Protected Sub GridRowToObject(ByVal FooterRow As GridViewRow, ByRef Obj As Object)
        Try


            Dim props As Type = Obj.GetType

            For Each member As PropertyInfo In props.GetProperties
                If Not IsNothing(FooterRow.FindControl(member.Name)) Then
                    Select Case (FooterRow.FindControl(member.Name).GetType())
                        Case GetType(TextBox)
                            Dim TBox As TextBox = TryCast(FooterRow.FindControl(member.Name), TextBox)
                            member.SetValue(Obj, TBox.Text, Nothing)
                        Case GetType(DropDownList)
                            Dim DDL As DropDownList = TryCast(FooterRow.FindControl(member.Name), DropDownList)
                            member.SetValue(Obj, DDL.SelectedValue, Nothing)
                        Case GetType(RadioButton)
                            Dim RDO As RadioButton = TryCast(FooterRow.FindControl(member.Name), RadioButton)
                            If RDO.Checked = True Then
                                member.SetValue(Obj, "Y", Nothing)
                            Else
                                member.SetValue(Obj, "N", Nothing)
                            End If
                        Case GetType(Label)
                            Dim Lbl As Label = TryCast(FooterRow.FindControl(member.Name), Label)
                            member.SetValue(Obj, Lbl.Text, Nothing)
                    End Select
                End If
            Next
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
    Protected Sub ObjectToDataRow(ByVal Obj As Object, ByRef DRow As DataRow)
        Try


            Dim props As Type = Obj.GetType

            For Each member As PropertyInfo In props.GetProperties
                If Not IsNothing(member.GetValue(Obj, Nothing)) Then
                    DRow(member.Name) = member.GetValue(Obj, Nothing)
                End If
            Next
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
    Protected Function GridLinesToObjectsPrepareData(Optional ByVal PrimaryKey As String = "") As Object()
        Try
            Dim PickLstLines() As Models.DOLineItems = Nothing
            Dim Exceptlist As New List(Of String)
            Exceptlist.Add("Headerid")

            Dim GridDataTable As DataTable = TryCast(ViewState("DOLines"), DataTable)
            If Not IsNothing(GridDataTable) Then
                Dim i As Integer = 0
                For Each DRow As DataRow In GridDataTable.Rows
                    'Set OpenQty as equal as Qty
                    'DRow("OpenQty") = DRow("Qty")
                    'GridDataTable.AcceptChanges()


                    ReDim Preserve PickLstLines(i)
                    PickLstLines(i) = New Models.DOLineItems
                    Dim props As Type = PickLstLines(i).GetType
                    For Each member As PropertyInfo In props.GetProperties
                        If Not Exceptlist.Contains(member.Name) Then
                            If Not IsDBNull(DRow(member.Name)) Then
                                member.SetValue(PickLstLines(i), DRow(member.Name).ToString, Nothing)
                            End If
                            If member.Name.ToLower = "sno" Then
                                member.SetValue(PickLstLines(i), (i + 1).ToString, Nothing)
                            End If
                            If member.Name.ToLower = "invdate" Then
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
            Dim PickLstLines() As Models.DOFiles = Nothing
            Dim Exceptlist As New List(Of String)
            Exceptlist.Add("Headerid")

            Dim fileslist As List(Of String) = TryCast(ViewState("UploadedFilesLines"), List(Of String))
            If Not IsNothing(fileslist) Then
                Dim i As Integer = 0
                For Each DRow In fileslist
                    'Set OpenQty as equal as Qty
                    'DRow("OpenQty") = DRow("Qty")
                    'GridDataTable.AcceptChanges()


                    ReDim Preserve PickLstLines(i)
                    PickLstLines(i) = New Models.DOFiles
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
#End Region
#Region "Form Event Handlers"
    Protected Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
        Try
            If Page.IsValid Then
                If IsNumeric(txtDOId.Text) And IsNumeric(txtClaimId.Text) Then
                    Dim LineItemsObject() As Models.DOLineItems
                    LineItemsObject = TryCast(GridLinesToObjectsPrepareData(txtDOId.Text), Models.DOLineItems())
                    Dim ProceedNext As Boolean = True
                    If ProceedNext Then
                        For Each Obj As Models.DOLineItems In LineItemsObject
                            Dim UQ As New UpdateQuery
                            Dim InputTable As New ClaimLineItems
                            Dim InputTableFieldOperators As New ClaimLineItems
                            'Input Values
                            InputTable.OpenQty = Obj.DOQty
                            InputTableFieldOperators.OpenQty = "+"

                            UQ._InputTable = InputTable
                            UQ._InputTableFieldsOperation = InputTableFieldOperators

                            'Filter Values
                            Dim FilterTable As New ClaimLineItems
                            FilterTable.Headerid = txtClaimId.Text
                            FilterTable.SNo = Obj.ClaimSNo
                            UQ._FilterTable = FilterTable
                            UQ._DB = "Custom"
                            UQ._HasInBetweenConditions = False
                            UQ._HasWhereConditions = True
                            'Query Conditions List
                            Dim QryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

                            'Query Condition Groups
                            Dim ConditionsGrp1 As List(Of String) = New List(Of String)

                            'Query Conditions values
                            ConditionsGrp1.Add("Headerid=@Filter_Headerid")
                            ConditionsGrp1.Add("SNo=@Filter_SNo")
                            QryConditions.Add(" AND ", ConditionsGrp1)
                            UQ._Conditions = QryConditions

                            Dim ItmReqPrimaryKey As String = String.Empty
                            ProceedNext = CURD.UpdateData(UQ)

                            If ProceedNext = False Then
                                Throw New System.Exception("Line Item Update error in Delivery Order Cancel query")
                            End If

                        Next
                    End If

                    If ProceedNext = True Then
                        Dim ResultDataTable As New DataTable
                        '**********************Query Builder Function *****************
                        Dim CQ As New CustomQuery
                        CQ._DB = "Custom"
                        Dim CustomQueryParameters As New Dictionary(Of String, String)
                        Dim InputQuery1 As String = String.Empty
                        InputQuery1 = "Select Sum(OpenQty) as 'OpenQtySum' from ClaimLineItems"

                        Dim Conditionlist1 As New List(Of String)

                        Conditionlist1.Add(" Headerid=@Headerid  ")
                        CustomQueryParameters.Add("@Headerid", txtClaimId.Text)


                        If Conditionlist1.Count > 0 Then
                            Dim CondiString1 As String = String.Join(" AND ", Conditionlist1)

                            InputQuery1 = InputQuery1 & " WHERE " & CondiString1
                        End If

                        '**********************Query Builder Function *****************
                        CQ._InputQuery = InputQuery1
                        CQ._Parameters = CustomQueryParameters
                        ResultDataTable = CURD.CustomQueryGetData(CQ)


                        If Not IsNothing(ResultDataTable) Then
                            If ResultDataTable.Rows.Count > 0 Then
                                If ResultDataTable.Rows(0).Item("OpenQtySum") > 0 Then

                                    Dim CH As New Models.ClaimHeader
                                    Dim UCHQ As New UpdateQuery
                                    CH.Status = "Open"
                                    UCHQ._InputTable = CH
                                    Dim CHFilterTable As New ClaimHeader
                                    CHFilterTable.id = txtClaimId.Text
                                    UCHQ._FilterTable = CHFilterTable
                                    UCHQ._DB = "Custom"
                                    UCHQ._HasInBetweenConditions = False
                                    UCHQ._HasWhereConditions = True
                                    'Query Conditions List
                                    Dim QryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

                                    'Query Condition Groups
                                    Dim ConditionsGrp1 As List(Of String) = New List(Of String)

                                    'Query Conditions values
                                    ConditionsGrp1.Add("Id=@Filter_Id")

                                    QryConditions.Add(" AND ", ConditionsGrp1)
                                    UCHQ._Conditions = QryConditions

                                    ProceedNext = CURD.UpdateData(UCHQ)
                                End If
                            End If
                        End If
                    End If
                    If ProceedNext = True Then
                        Dim DH As New DOHeader
                        DH.Status = "Cancelled"
                        Dim UQ As New UpdateQuery
                        UQ._InputTable = DH
                        Dim FilterTable As New ClaimHeader
                        FilterTable.id = txtDOId.Text
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

                        ProceedNext = CURD.UpdateData(UQ)
                    End If
                    InitialiseForm()
                    lblMsg.Text = "DO has been sucessfully cancelled."
                    lblMsg.ForeColor = Drawing.Color.Green
                End If
            End If
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
    Protected Sub BtnAddRemarks_Click(sender As Object, e As System.EventArgs) Handles BtnAddRemarks.Click
        Try
            If IsNumeric(txtDOId.Text) And Trim(txtRemarks.Text) <> String.Empty Then

                Dim CR As New DORemarks
                CR.ClaimId = txtDOId.Text
                CR.CreatedBy = User.Identity.Name
                CR.Remarks = Trim(txtRemarks.Text)
                CURD.InsertData(CR)
                lblMsg.Text = "Remark added sucessfully"
                lblMsg.ForeColor = Drawing.Color.Green
                LoadRemarksFromDataBase(CInt(txtDOId.Text))
                txtRemarks.Text = String.Empty

            Else

                If (Trim(txtRemarks.Text) <> String.Empty) Then

                Else
                    lblMsg.Text = "Please save DO form before adding Remark"
                    lblMsg.ForeColor = Drawing.Color.Red

                End If
            End If

        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
            lblMsg.Text = "There is some problem in adding your remark. Please contact your website Administrator"
            lblMsg.ForeColor = Drawing.Color.Red
        End Try
    End Sub
    Private Function IsExceedsAvailOpenQty(ClaimId As String, ClaimLineNo As String, DORequestQty As String) As Boolean
        Dim ReturnResult As Boolean = True
        Try
            Dim ClaimLineObj As New ClaimLineItems
            ClaimLineObj.Headerid = ClaimId
            ClaimLineObj.SNo = ClaimLineNo
            Dim QryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

            'Query Condition Groups
            Dim ConditionsGrp1 As List(Of String) = New List(Of String)

            'Query Conditions values
            ConditionsGrp1.Add("Headerid=@Headerid")
            ConditionsGrp1.Add("SNo=@SNo")

            QryConditions.Add(" AND ", ConditionsGrp1)

            ' INPUT FOR Query Builder
            Dim SQB As New SelectQuery
            SQB._InputTable = ClaimLineObj
            SQB._DB = "Custom"
            SQB._HasInBetweenConditions = False
            SQB._HasWhereConditions = True
            SQB._Conditions = QryConditions
            Dim ResultDataTable As New DataTable
            ResultDataTable = CURD.SelectAllData(SQB)
            If ResultDataTable.Rows.Count > 0 Then
                Dim Drow As DataRow = ResultDataTable.Rows(0)
                Dim OpQty As String = Drow("OpenQty").ToString
                If OpQty >= DORequestQty Then
                    ReturnResult = False
                End If
            End If

            Return ReturnResult
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
            Return True
        End Try
    End Function
    Private Function CheckDOIDExist(ByVal Headerid As String) As Boolean
        Dim ClaimFlag As Boolean = False
        Dim SqlCon As SqlConnection = New SqlConnection()
        SqlCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
        SqlCon.Open()
        Dim ReaderCommand As SqlCommand = New SqlCommand()
        ReaderCommand.Connection = SqlCon
        'ReaderCommand.CommandText = "SELECT id  AS  ClaimId FROM ClaimHeader Where id=@Headerid AND Status!=@status  and Country=@country"
        ReaderCommand.CommandText = "SELECT id  AS  ClaimId FROM DOHeader Where id=@Headerid   and Country=@country"
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

        DocStatusBasedButtonRestrictions()


        If Roles.IsUserInRole(User.Identity.Name, "FocusLimited") Then
            btnSave.Enabled = True
            txtreasonforrejection.ReadOnly = False
        End If



        Return ClaimFlag
    End Function
    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then
            Dim IsProcessedSuccessfully As Boolean = False
            Dim CurrentCountry As String = Session("Country")
            Dim CurrentCountryName As String = Session("CountryName")
            Dim ProceedNext As Boolean = True
            Try
                If Not IsNothing(Session("Country")) Then
                    If Not CheckDOIDExist(txtDOId.Text) Then
                        Dim IsExceedsOpQty As Boolean = False
                        Dim ExceededLineNo As String = String.Empty
                        Dim LineItemsObject() As Models.DOLineItems
                        LineItemsObject = TryCast(GridLinesToObjectsPrepareData(-1), Models.DOLineItems())
                        If Not IsNothing(LineItemsObject) Then


                            For Each obj As DOLineItems In LineItemsObject
                                Dim ClaimId As Integer = ViewState("ClaimId")
                                Dim ClaimLineNo As String = obj.ClaimSNo
                                Dim DORequestQty As Integer = obj.DOQty
                                If IsExceedsAvailOpenQty(ClaimId, ClaimLineNo, DORequestQty) Then
                                    ExceededLineNo = obj.SNo
                                    IsExceedsOpQty = True
                                    Exit For
                                End If
                            Next
                            If IsExceedsOpQty = False Then
                                Using SqlCon As New SqlConnection(CURD.GetConnectionString)

                                    SqlCon.Open()

                                    Dim SqlTrans As SqlTransaction = SqlCon.BeginTransaction()

                                    Try

                                        Dim PrimaryKey As String = String.Empty

                                        Dim DH As New Models.DOHeader
                                        DH.ClaimId = ViewState("ClaimId")
                                        DH.CustomerName = txtEndCustomerName.Text
                                        DH.Personincharge = txtPersonincharge.Text
                                        DH.EmailAddress = txtEmailAddress.Text
                                        DH.PhoneNo = txtTelno.Text
                                        DH.Brand = txtBrand.Text
                                        DH.ModelNo = txtMachineModelNo.Text
                                        DH.DocDate = Format(CDate(DateTime.Now), "yyyy-MM-dd")
                                        DH.Country = CurrentCountry
                                        DH.CountryName = CurrentCountryName
                                        DH.ShipToCompany = txtShipToCompany.Text
                                        DH.ShipToAddress = txtShipToAddress.Text
                                        DH.ShipToContact = txtShipToContactPerson.Text
                                        DH.ShipToPhone = txtShipToTelNo.Text
                                        DH.Status = ddlStatus.SelectedValue
                                        DH.Priority = ddlPriority.SelectedValue
                                        DH.youtubelink1 = txtYoutubeLink1.Text
                                        DH.youtubelink2 = txtYoutubeLink2.Text
                                        DH.youtubelink3 = txtYoutubeLink3.Text
                                        DH.reasonforrejection = txtreasonforrejection.Text
                                        DH.CreatedBy = User.Identity.Name

                                        ProceedNext = CURD.InsertDataTransaction(DH, SqlCon, SqlTrans, PrimaryKey, True)

                                        If ProceedNext Then
                                            LineItemsObject = TryCast(GridLinesToObjectsPrepareData(PrimaryKey), Models.DOLineItems())

                                            For Each Obj As Models.DOLineItems In LineItemsObject
                                                Obj.Headerid = PrimaryKey
                                                ProceedNext = CURD.InsertDataTransaction(Obj, SqlCon, SqlTrans, PrimaryKey, False)
                                                If ProceedNext = False Then
                                                    Throw New System.Exception("Line Item insertion error in DO lines page")
                                                End If
                                            Next
                                        End If
                                        Dim LineItemsFileObject() As Models.DOFiles
                                        If ProceedNext Then
                                            LineItemsFileObject = TryCast(GridLinesToFileObjectsPrepareData(PrimaryKey), Models.DOFiles())
                                            If Not IsNothing(LineItemsFileObject) Then
                                                For Each Obj As Models.DOFiles In LineItemsFileObject
                                                    Obj.Headerid = PrimaryKey
                                                    ProceedNext = CURD.InsertDataTransaction(Obj, SqlCon, SqlTrans, PrimaryKey, False)
                                                    If ProceedNext = False Then
                                                        Throw New System.Exception("Line Item files insertion error in DO lines page")
                                                    End If
                                                Next
                                            End If

                                        End If
                                        If ProceedNext Then
                                            For Each Obj As Models.DOLineItems In LineItemsObject
                                                Dim UQ As New UpdateQuery
                                                Dim InputTable As New ClaimLineItems
                                                Dim InputTableFieldOperators As New ClaimLineItems
                                                'Input Values
                                                InputTable.OpenQty = Obj.DOQty
                                                InputTableFieldOperators.OpenQty = "-"

                                                UQ._InputTable = InputTable
                                                UQ._InputTableFieldsOperation = InputTableFieldOperators

                                                'Filter Values
                                                Dim FilterTable As New ClaimLineItems
                                                FilterTable.Headerid = ViewState("ClaimId")
                                                FilterTable.SNo = Obj.ClaimSNo
                                                FilterTable.OpenQty = Obj.DOQty
                                                UQ._FilterTable = FilterTable
                                                UQ._DB = "Custom"
                                                UQ._HasInBetweenConditions = False
                                                UQ._HasWhereConditions = True
                                                'Query Conditions List
                                                Dim QryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

                                                'Query Condition Groups
                                                Dim ConditionsGrp1 As List(Of String) = New List(Of String)

                                                'Query Conditions values
                                                ConditionsGrp1.Add("Headerid=@Filter_Headerid")
                                                ConditionsGrp1.Add("SNo=@Filter_SNo")
                                                ConditionsGrp1.Add("(OpenQty-@Filter_OpenQty)>=0")
                                                QryConditions.Add(" AND ", ConditionsGrp1)
                                                UQ._Conditions = QryConditions

                                                Dim ItmReqPrimaryKey As String = String.Empty
                                                ProceedNext = CURD.UpdateDataTransaction(UQ, SqlCon, SqlTrans, ItmReqPrimaryKey, False)

                                                If ProceedNext = False Then
                                                    Throw New System.Exception("Line Item Update error in Delivery Order Insert query")
                                                End If

                                            Next
                                        End If


                                        lblMsg.Text = "DO No : " & PrimaryKey & " has been sucessfully submitted."
                                        lblMsg.ForeColor = Drawing.Color.Green




                                        SqlTrans.Commit()
                                        If ProceedNext = True Then
                                            Dim ResultDataTable As New DataTable
                                            '**********************Query Builder Function *****************
                                            Dim CQ As New CustomQuery
                                            CQ._DB = "Custom"
                                            Dim CustomQueryParameters As New Dictionary(Of String, String)
                                            Dim InputQuery1 As String = String.Empty
                                            InputQuery1 = "Select Sum(OpenQty) as 'OpenQtySum' from ClaimLineItems"

                                            Dim Conditionlist1 As New List(Of String)

                                            Conditionlist1.Add(" Headerid=@Headerid  ")
                                            CustomQueryParameters.Add("@Headerid", ViewState("ClaimId"))


                                            If Conditionlist1.Count > 0 Then
                                                Dim CondiString1 As String = String.Join(" AND ", Conditionlist1)

                                                InputQuery1 = InputQuery1 & " WHERE " & CondiString1
                                            End If

                                            '**********************Query Builder Function *****************
                                            CQ._InputQuery = InputQuery1
                                            CQ._Parameters = CustomQueryParameters
                                            ResultDataTable = CURD.CustomQueryGetData(CQ)


                                            If Not IsNothing(ResultDataTable) Then
                                                If ResultDataTable.Rows.Count > 0 Then
                                                    If ResultDataTable.Rows(0).Item("OpenQtySum") <= 0 Then

                                                        Dim CH As New Models.ClaimHeader
                                                        Dim UCHQ As New UpdateQuery
                                                        CH.Status = "Complete"
                                                        UCHQ._InputTable = CH
                                                        Dim CHFilterTable As New ClaimHeader
                                                        CHFilterTable.id = ViewState("ClaimId")
                                                        UCHQ._FilterTable = CHFilterTable
                                                        UCHQ._DB = "Custom"
                                                        UCHQ._HasInBetweenConditions = False
                                                        UCHQ._HasWhereConditions = True
                                                        'Query Conditions List
                                                        Dim QryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

                                                        'Query Condition Groups
                                                        Dim ConditionsGrp1 As List(Of String) = New List(Of String)

                                                        'Query Conditions values
                                                        ConditionsGrp1.Add("Id=@Filter_Id")

                                                        QryConditions.Add(" AND ", ConditionsGrp1)
                                                        UCHQ._Conditions = QryConditions

                                                        ProceedNext = CURD.UpdateData(UCHQ)
                                                    End If
                                                End If
                                            End If
                                        End If
                                        IsProcessedSuccessfully = True
                                    Catch ex As Exception
                                        lblMsg.Text = "There is some problem in saving your claim. Please contact your website Administrator"
                                        lblMsg.ForeColor = Drawing.Color.Red
                                        AppSpecificFunc.WriteLog(ex)
                                        SqlTrans.Rollback()
                                        IsProcessedSuccessfully = False
                                    End Try
                                End Using
                            Else
                                lblMsg.Text = "Line No " & ExceededLineNo & " DO Qty exceeds available Qty, Cannot Proceed"
                                lblMsg.ForeColor = Drawing.Color.Red
                                IsProcessedSuccessfully = False
                            End If
                        Else
                            lblMsg.Text = "There is no lines found, Cannot Proceed"
                            lblMsg.ForeColor = Drawing.Color.Red
                            IsProcessedSuccessfully = False
                        End If

                    Else



                    End If

                End If
                If IsProcessedSuccessfully = True Then
                    InitialiseForm()
                End If
            Catch ex As Exception
                AppSpecificFunc.WriteLog(ex)
                lblMsg.Text = "There is some problem in saving your claim. Please contact your website Administrator"
                lblMsg.ForeColor = Drawing.Color.Red
            End Try
        End If
    End Sub
    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        If Not IsNothing(ViewState("UploadedFilesLines")) Then

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
                    lblUploadMsg.Text = "Invalid File. Please upload a File with extension " & _
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
                UploadedFilesList = ViewState("UploadedFilesLines")
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

                        ViewState("UploadedFilesLines") = UploadedFilesList
                        LoadThumbnails(UploadedFilesList)
                    End If
                End If

            End If
        Else
            FormsAuthentication.SignOut()
        End If
    End Sub
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                InitialiseForm()
            Else
                Dim GridDataTable As DataTable = TryCast(ViewState("DOLines"), DataTable)
                If Not IsNothing(GridDataTable) Then
                    If GridDataTable.Rows.Count <= 0 Then
                        AppSpecificFunc.GridNoDataFound(DeliveryItemListGrid)
                    End If
                End If
                Dim RemarksGridDataTable As DataTable = TryCast(ViewState("DORemarkLines"), DataTable)
                If Not IsNothing(RemarksGridDataTable) Then
                    If RemarksGridDataTable.Rows.Count <= 0 Then
                        AppSpecificFunc.GridNoDataFound(ItemDetailsGrid)
                    End If
                End If
                Dim UploadedFilesListing As List(Of String) = TryCast(ViewState("UploadedFilesLines"), List(Of String))
                If Not IsNothing(UploadedFilesListing) Then
                    If UploadedFilesListing.Count > 0 Then
                        LoadThumbnails(UploadedFilesListing)
                    End If

                End If
            End If
            If Not IsNothing(Me.Context.Items.Item("ClaimId")) Then
                Dim DocNoValue As String = Me.Context.Items.Item("ClaimId").ToString
                FindClaimDocument(DocNoValue)
            ElseIf Not IsNothing(Me.Context.Items.Item("DOId")) Then
                Dim DONoValue As String = Me.Context.Items.Item("DOId").ToString
                FindDODocument(DONoValue)
            End If
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
    Protected Sub UploadedFiles_Click(sender As Object, e As System.Web.UI.WebControls.BulletedListEventArgs) Handles UploadedFiles.Click
        If Not IsNothing(ViewState("UploadedFilesLines")) Then
            If e.Index > -1 Then
                UploadedFiles.Items.RemoveAt(e.Index)
                Dim UploadedFilesList As List(Of String) = New List(Of String)
                UploadedFilesList = ViewState("UploadedFilesLines")
                Dim FileSavePath As String = String.Empty
                FileSavePath = UploadedFilesList.Item(e.Index)
                If System.IO.File.Exists(FileSavePath) Then
                    System.IO.File.Delete(FileSavePath)
                End If
                UploadedFilesList.RemoveAt(e.Index)
                ViewState("UploadedFilesLines") = UploadedFilesList
                LoadThumbnails(UploadedFilesList)
            End If
        Else
            FormsAuthentication.SignOut()
        End If
    End Sub
#End Region

#Region "Event Handlers - Grid View"

    Protected Sub DeliveryItemListGrid_RowCancelingEdit(sender As Object, e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles DeliveryItemListGrid.RowCancelingEdit
        Try
            DeliveryItemListGrid.EditIndex = -1

            Dim GridDataTable As DataTable = TryCast(ViewState("DOLines"), DataTable)
            If Not IsNothing(GridDataTable) Then
                AppSpecificFunc.BindGridData(GridDataTable, DeliveryItemListGrid)
            End If
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub

    Protected Sub DeliveryItemListGrid_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles DeliveryItemListGrid.RowDataBound
        Try
            If IsNumeric(txtDOId.Text) Then
                If e.Row.RowType = DataControlRowType.DataRow Then
                    If e.Row.Cells(e.Row.Cells.Count - 1).Controls.Count > 0 Then

                        Dim a As LinkButton = e.Row.Cells(e.Row.Cells.Count - 1).Controls(0)
                        Dim b As LinkButton = e.Row.Cells(e.Row.Cells.Count - 1).Controls(2)

                        a.Text = String.Empty
                        b.Text = String.Empty
                    End If

                End If
            End If
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
    Protected Sub DeliveryItemListGrid_RowDeleting(sender As Object, e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles DeliveryItemListGrid.RowDeleting
        Try
            DeliveryItemListGrid.EditIndex = -1
            Dim GridDataTable As DataTable = TryCast(ViewState("DOLines"), DataTable)
            If Not IsNothing(GridDataTable) Then
                GridDataTable.Rows(e.RowIndex).Delete()
                GridDataTable.AcceptChanges()
                Dim i As Integer = 1
                For Each Drow As DataRow In GridDataTable.Rows
                    Drow("SNo") = i
                    Drow.AcceptChanges()
                    i = i + 1
                Next
                AppSpecificFunc.BindGridData(GridDataTable, DeliveryItemListGrid)
            End If
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
    Protected Sub DeliveryItemListGrid_RowEditing(sender As Object, e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles DeliveryItemListGrid.RowEditing
        Try
            DeliveryItemListGrid.EditIndex = e.NewEditIndex

            Dim GridDataTable As DataTable = TryCast(ViewState("DOLines"), DataTable)
            If Not IsNothing(GridDataTable) Then
                AppSpecificFunc.BindGridData(GridDataTable, DeliveryItemListGrid)
            End If
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
    Protected Sub DeliveryItemListGrid_RowUpdating(sender As Object, e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles DeliveryItemListGrid.RowUpdating
        Try
            Dim LineItems As New DOLineItems
            DeliveryItemListGrid.EditIndex = -1
            Dim GridDataTable As DataTable = TryCast(ViewState("DOLines"), DataTable)
            If Not IsNothing(GridDataTable) Then

                Dim GridDataRow As DataRow = GridDataTable.Rows(e.RowIndex)

                GridRowToObject(DeliveryItemListGrid.Rows(e.RowIndex), LineItems)
                ObjectToDataRow(LineItems, GridDataRow)

                GridDataRow.AcceptChanges()

                AppSpecificFunc.BindGridData(GridDataTable, DeliveryItemListGrid)

            End If
        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
#End Region






    Protected Sub btnPreview_Click(sender As Object, e As System.EventArgs) Handles btnPreview.Click
        Try
            If IsNumeric(txtDOId.Text) Then
                Dim doc As New ReportDocument()
                Dim ReportFileName As String = String.Empty

                ReportFileName = Server.MapPath("~/Reports/ClaimDO.rpt")

                'Dim fileName As String = "C:\Users\USER\Desktop\PackingSlip.report.rpt"
                'reportdocument.SetDatabaseLogon("sa", "Admin123", "vivek", "PURCHASE");


                doc.Load(ReportFileName)
                doc.SetDatabaseLogon(ConfigurationManager.AppSettings("DB_Username"), ConfigurationManager.AppSettings("DB_Password"), ConfigurationManager.AppSettings("Company_Server"), ConfigurationManager.AppSettings("Company_DB"))
                doc.SetParameterValue("@id", txtDOId.Text)
                Dim exportOpts As ExportOptions = doc.ExportOptions

                exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat

                exportOpts.ExportDestinationType = ExportDestinationType.DiskFile

                exportOpts.DestinationOptions = New DiskFileDestinationOptions()

                Dim diskOpts As New DiskFileDestinationOptions()

                Dim origin As New DateTime(1970, 1, 1, 0, 0, 0, 0)
                Dim Diff As New TimeSpan
                Diff = Now - origin
                Dim FileNameGenerated As String = Math.Floor(Diff.TotalSeconds)
                FileNameGenerated = txtDOId.Text & "_" & FileNameGenerated & ".pdf"
                CType(doc.ExportOptions.DestinationOptions, DiskFileDestinationOptions).DiskFileName = Server.MapPath("~/ReportFiles/" & FileNameGenerated)
                'CType(doc.ExportOptions.DestinationOptions, DiskFileDestinationOptions).DiskFileName = "D:\ReportsSQ\" & FileNameGenerated
                'export the report to PDF rather than displaying the report in a viewer

                doc.Export()

                'force download dialog to download the PDF file at user end.

                'Set the appropriate ContentType.

                Response.ContentType = "Application/pdf"

                'Get the physical path to the file.

                'Dim FilePath As String = Server.MapPath("~/DesktopModules/OnlineForm/OnlineForm.pdf")

                ''Write the file directly to the HTTP content output stream.

                Response.WriteFile(Server.MapPath("~/ReportFiles/" & FileNameGenerated))
                'Response.WriteFile("D:\ReportsSQ\" & FileNameGenerated)
                Response.End()
            End If

        Catch ex As Exception
            AppSpecificFunc.WriteLog(ex)
        End Try
    End Sub
End Class
