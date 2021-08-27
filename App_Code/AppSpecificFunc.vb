Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Diagnostics
Imports System.Reflection

Namespace Models
    Public Class AppSpecificFunc
        
        Public Shared Sub BindGridData(ByVal GridDataTable As DataTable, ByRef InputGridView As GridView)
            Try


                If GridDataTable.Rows.Count > 0 Then
                    InputGridView.DataSource = GridDataTable
                    InputGridView.DataBind()
                Else

                    Dim TempDataTable As DataTable = GridDataTable.Clone
                    TempDataTable.Rows.Add(TempDataTable.NewRow())
                    InputGridView.DataSource = TempDataTable
                    InputGridView.DataBind()

                    AppSpecificFunc.GridNoDataFound(InputGridView)
                End If

            Catch ex As Exception
                AppSpecificFunc.WriteLog(ex)
            End Try
        End Sub
        Public Shared Sub GridNoDataFound(ByRef InputGridView As GridView)
            Try


                'Dim TotalColumns As Integer = InputGridView.Rows(0).Cells.Count
                'InputGridView.Rows(0).Cells.Clear()
                'InputGridView.Rows(0).Cells.Add(New TableCell())
                'InputGridView.Rows(0).Cells(0).ColumnSpan = TotalColumns
                'InputGridView.Rows(0).Cells(0).Text = "No Record Found"

                Dim TotalColumns As Integer = InputGridView.Columns.Count
                InputGridView.Rows(0).Cells.Clear()
                InputGridView.Rows(0).Cells.Add(New TableCell())
                InputGridView.Rows(0).Cells(0).ColumnSpan = TotalColumns
                InputGridView.Rows(0).Cells(0).Text = "No Record Found"
            Catch ex As Exception
                AppSpecificFunc.WriteLog(ex)
            End Try
        End Sub
       
     
        Public Shared Function WriteLog(ByRef ex As Exception) As ErrLog
            Try

           
            Dim EL As New ErrLog
            Dim St As StackTrace = New StackTrace(ex, True)
            EL.errMSg = ex.Message
            If Not IsNothing(ex.InnerException) Then
                EL.InnerException = ex.InnerException.Message
            End If
            Dim FrameCount As Integer = St.FrameCount
            For i As Integer = 0 To FrameCount - 1
                If Not IsNothing(St.GetFrame(i).GetFileName) Then
                    EL.FileName = St.GetFrame(i).GetFileName.ToString
                    EL.LineNumber = St.GetFrame(i).GetFileLineNumber.ToString
                End If

                Next
                EL.IdKey = Nothing
            EL.CreatedOn = Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss")
            CURD.InsertData(EL, True)
                Return EL
            Catch ex1 As Exception
                Return Nothing
            End Try
        End Function

        Public Shared Sub DataTableToObject(ByRef Obj As Object, ByVal DT As DataTable, Optional ByVal Index As Integer = 0)
            Try
                Dim props As Type = Obj.GetType
                If DT.Rows.Count > 0 Then
                    Dim Drow As DataRow = DT.Rows(Index)
                    For Each member As PropertyInfo In props.GetProperties
                        If DT.Columns.Contains(member.Name) Then
                            If Not IsDBNull(Drow(member.Name)) Then
                                member.SetValue(Obj, Drow(member.Name).ToString, Nothing)
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                AppSpecificFunc.WriteLog(ex)
            End Try
        End Sub
        Public Shared Sub DataRowToObject(ByRef Obj As Object, ByVal DR As DataRow)
            Try
                Dim props As Type = Obj.GetType

                Dim Drow As DataRow = DR
                For Each member As PropertyInfo In props.GetProperties

                    If Not IsDBNull(Drow(member.Name)) Then
                        member.SetValue(Obj, Drow(member.Name).ToString, Nothing)
                    End If
                Next

            Catch ex As Exception
                AppSpecificFunc.WriteLog(ex)
            End Try
        End Sub
        Public Shared Sub GridRowToObject(ByVal FooterRow As GridViewRow, ByRef Obj As Object)
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
        Public Shared Sub ObjectToDataRow(ByVal Obj As Object, ByRef DRow As DataRow)
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
        
        
        
        Public Shared Sub ObjectToPageControls(ByRef FooterRow As Control, ByVal Obj As Object)
            Try


                Dim props As Type = Obj.GetType

                For Each member As PropertyInfo In props.GetProperties
                    If Not IsNothing(FooterRow.FindControl(member.Name)) Then
                        If Not IsNothing(member.GetValue(Obj, Nothing)) Then
                            Select Case (FooterRow.FindControl(member.Name).GetType())
                                Case GetType(TextBox)
                                    Dim TBox As TextBox = TryCast(FooterRow.FindControl(member.Name), TextBox)
                                    TBox.Text = member.GetValue(Obj, Nothing).ToString
                                Case GetType(DropDownList)
                                    Dim DDL As DropDownList = TryCast(FooterRow.FindControl(member.Name), DropDownList)
                                    DDL.SelectedIndex = DDL.Items.IndexOf(DDL.Items.FindByValue(member.GetValue(Obj, Nothing).ToString))
                                Case GetType(Label)
                                    Dim Lbl As Label = TryCast(FooterRow.FindControl(member.Name), Label)
                                    Lbl.Text = member.GetValue(Obj, Nothing).ToString
                                Case GetType(RadioButtonList)
                                    Dim RdoList As RadioButtonList = TryCast(FooterRow.FindControl(member.Name), RadioButtonList)
                                    RdoList.SelectedIndex = RdoList.Items.IndexOf(RdoList.Items.FindByValue(member.GetValue(Obj, Nothing).ToString))
                            End Select
                        Else
                            Select Case (FooterRow.FindControl(member.Name).GetType())
                                Case GetType(TextBox)
                                    Dim TBox As TextBox = TryCast(FooterRow.FindControl(member.Name), TextBox)
                                    TBox.Text = String.Empty
                                Case GetType(DropDownList)
                                    Dim DDL As DropDownList = TryCast(FooterRow.FindControl(member.Name), DropDownList)
                                    DDL.SelectedIndex = 0
                                Case GetType(Label)
                                    Dim Lbl As Label = TryCast(FooterRow.FindControl(member.Name), Label)
                                    Lbl.Text = String.Empty
                                Case GetType(RadioButtonList)
                                    Dim RdoList As RadioButtonList = TryCast(FooterRow.FindControl(member.Name), RadioButtonList)
                                    RdoList.SelectedIndex = -1
                            End Select
                        End If
                    End If
                Next
            Catch ex As Exception
                AppSpecificFunc.WriteLog(ex)
            End Try
        End Sub
        Public Shared Sub PageControlsToObject(ByVal FooterRow As Control, ByRef Obj As Object)
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
                            Case GetType(Label)
                                Dim Lbl As Label = TryCast(FooterRow.FindControl(member.Name), Label)
                                member.SetValue(Obj, Lbl.Text, Nothing)
                            Case GetType(RadioButtonList)
                                Dim RdoList As RadioButtonList = TryCast(FooterRow.FindControl(member.Name), RadioButtonList)
                                If RdoList.SelectedIndex > -1 Then
                                    member.SetValue(Obj, RdoList.SelectedItem.Value, Nothing)
                                End If
                        End Select
                    End If
                Next
            Catch ex As Exception
                AppSpecificFunc.WriteLog(ex)
            End Try
        End Sub
        Public Shared Sub MakeFormReadOnly(ByRef InputForm As Control, ByVal Obj As Object, ByVal SetReadOnly As Boolean)
            Try


                Dim props As Type = Obj.GetType

                For Each member As PropertyInfo In props.GetProperties
                    If Not IsNothing(InputForm.FindControl(member.Name)) Then
                        If SetReadOnly = True Then
                            Select Case (InputForm.FindControl(member.Name).GetType())
                                Case GetType(TextBox)
                                    Dim TBox As TextBox = TryCast(InputForm.FindControl(member.Name), TextBox)
                                    TBox.Enabled = False
                                Case GetType(DropDownList)
                                    Dim DDL As DropDownList = TryCast(InputForm.FindControl(member.Name), DropDownList)
                                    DDL.Enabled = False
                                Case GetType(Label)
                                    Dim Lbl As Label = TryCast(InputForm.FindControl(member.Name), Label)
                                    Lbl.Enabled = False
                                Case GetType(RadioButtonList)
                                    Dim RdoList As RadioButtonList = TryCast(InputForm.FindControl(member.Name), RadioButtonList)
                                    RdoList.Enabled = False
                            End Select
                        Else
                            Select Case (InputForm.FindControl(member.Name).GetType())
                                Case GetType(TextBox)
                                    Dim TBox As TextBox = TryCast(InputForm.FindControl(member.Name), TextBox)
                                    TBox.Enabled = True
                                Case GetType(DropDownList)
                                    Dim DDL As DropDownList = TryCast(InputForm.FindControl(member.Name), DropDownList)
                                    DDL.Enabled = True
                                Case GetType(Label)
                                    Dim Lbl As Label = TryCast(InputForm.FindControl(member.Name), Label)
                                    Lbl.Enabled = True
                                Case GetType(RadioButtonList)
                                    Dim RdoList As RadioButtonList = TryCast(InputForm.FindControl(member.Name), RadioButtonList)
                                    RdoList.Enabled = True
                            End Select
                        End If

                    End If
                Next
            Catch ex As Exception
                AppSpecificFunc.WriteLog(ex)
            End Try
        End Sub
        Public Shared Function GetActiveSysDocumentNosAutoComplete(ByVal MasterObj As Object, ByVal prefix As String, Optional ByVal CreatedBy As String = "") As DataTable
            Try


                Dim ResultDataTable As New DataTable
                '**********************Query Builder Function *****************


                'Filter Values
                If CreatedBy <> String.Empty Then
                    MasterObj.CreatedBy = CreatedBy
                End If

                MasterObj.IdKey = prefix

                'Query Conditions List
                Dim QryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

                'Query Condition Groups
                Dim ConditionsGrp1 As List(Of String) = New List(Of String)

                'Query Conditions values

                If CreatedBy <> String.Empty Then
                    ConditionsGrp1.Add("CreatedBy=@CreatedBy")
                End If

                ConditionsGrp1.Add("IdKey LIKE '%'+ @IdKey + '%' ")

                QryConditions.Add(" AND ", ConditionsGrp1)



                ' INPUT FOR Query Builder
                Dim SQB As New SelectQuery
                SQB._InputTable = MasterObj
                SQB._DB = "Custom"
                SQB._HasInBetweenConditions = False
                SQB._HasWhereConditions = True
                SQB._Conditions = QryConditions

                ResultDataTable = CURD.SelectAllData(SQB)
                '**********************Query Builder Function *****************
                Return ResultDataTable
            Catch ex As Exception
                WriteLog(ex)
                Return New DataTable
            End Try
        End Function
        Public Shared Function GetActiveDONosAutoComplete(ByVal MasterObj As Object, ByVal prefix As String, Optional ByVal CreatedBy As String = "") As DataTable
            Try


                Dim ResultDataTable As New DataTable
                '**********************Query Builder Function *****************


                'Filter Values
                If CreatedBy <> String.Empty Then
                    MasterObj.CreatedBy = CreatedBy
                End If

                If isPropertyExist(MasterObj, "DoNo") Then
                    MasterObj.DoNo = prefix
                ElseIf isPropertyExist(MasterObj, "Idkey") Then
                    MasterObj.Idkey = prefix
                End If

                'Query Conditions List
                Dim QryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

                'Query Condition Groups
                Dim ConditionsGrp1 As List(Of String) = New List(Of String)

                'Query Conditions values

                If CreatedBy <> String.Empty Then
                    ConditionsGrp1.Add("CreatedBy=@CreatedBy")
                End If

                If isPropertyExist(MasterObj, "DoNo") Then
                    ConditionsGrp1.Add("DoNo LIKE '%'+ @DoNo + '%' ")
                ElseIf isPropertyExist(MasterObj, "Idkey") Then
                    ConditionsGrp1.Add("Idkey LIKE '%'+ @Idkey + '%' ")
                End If


                QryConditions.Add(" AND ", ConditionsGrp1)



                ' INPUT FOR Query Builder
                Dim SQB As New SelectQuery
                SQB._InputTable = MasterObj
                SQB._DB = "Custom"
                SQB._HasInBetweenConditions = False
                SQB._HasWhereConditions = True
                SQB._Conditions = QryConditions

                ResultDataTable = CURD.SelectAllData(SQB)
                '**********************Query Builder Function *****************
                Return ResultDataTable
            Catch ex As Exception
                WriteLog(ex)
                Return New DataTable
            End Try
        End Function
        Public Shared Function isPropertyExist(ByVal Obj As Object, ByVal PropName As String) As Boolean
            Try
                Dim Result As Boolean = False
                Dim Tpe As Type = Obj.GetType
                Dim Props() As PropertyInfo = Tpe.GetProperties
                For Each prop As PropertyInfo In Props
                    If prop.Name.ToLower.Equals(PropName.ToLower) Then
                        Result = True
                        Exit For
                    End If
                Next
                Return Result
            Catch ex As Exception
                WriteLog(ex)
                Return False
            End Try
        End Function
        Public Shared Function GetLocationByWarhouseCodeAutoComplete(ByVal MasterObj As Object, ByVal prefix As String, Optional ByVal WarehouseCode As String = "") As DataTable
            Try


                Dim ResultDataTable As New DataTable
                '**********************Query Builder Function *****************


                'Filter Values
                If WarehouseCode <> String.Empty Then
                    MasterObj.WarehouseCode = WarehouseCode
                End If

                MasterObj.LocationCode = prefix

                'Query Conditions List
                Dim QryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

                'Query Condition Groups
                Dim ConditionsGrp1 As List(Of String) = New List(Of String)

                'Query Conditions values

                If WarehouseCode <> String.Empty Then
                    ConditionsGrp1.Add("WarehouseCode=@WarehouseCode")
                End If

                ConditionsGrp1.Add("LocationCode LIKE '%'+ @LocationCode + '%' ")

                QryConditions.Add(" AND ", ConditionsGrp1)



                ' INPUT FOR Query Builder
                Dim SQB As New SelectQuery
                SQB._InputTable = MasterObj
                SQB._DB = "Custom"
                SQB._HasInBetweenConditions = False
                SQB._HasWhereConditions = True
                SQB._Conditions = QryConditions

                ResultDataTable = CURD.SelectAllData(SQB)
                '**********************Query Builder Function *****************
                Return ResultDataTable
            Catch ex As Exception
                WriteLog(ex)
                Return New DataTable
            End Try
        End Function
        
        Public Shared Sub GetStringListFromDataTable(ByVal InputDataTable As DataTable, ByVal ColumnName As String, ByRef ListofString As List(Of String))
            Try
                For Each Drow As DataRow In InputDataTable.Rows
                    If Not IsDBNull(Drow(ColumnName)) Then
                        ListofString.Add(Drow(ColumnName).ToString)
                    End If
                Next
            Catch ex As Exception
                AppSpecificFunc.WriteLog(ex)
            End Try
        End Sub
        Public Shared Function PrepareUpdateOpenQtyObject(ByRef UQ As UpdateQuery) As Boolean
            Try
                Dim Result As Boolean = True
                UQ._DB = "Custom"
                UQ._HasInBetweenConditions = False
                UQ._HasWhereConditions = True
                'Query Conditions List
                Dim QryConditions As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

                'Query Condition Groups
                Dim ConditionsGrp1 As List(Of String) = New List(Of String)

                'Query Conditions values
                ConditionsGrp1.Add("IdKey=@Filter_IdKey")
                ConditionsGrp1.Add("LineNum=@Filter_LineNum")
                QryConditions.Add(" AND ", ConditionsGrp1)
                UQ._Conditions = QryConditions

                Return Result
            Catch ex As Exception
                WriteLog(ex)
                Return False
            End Try
        End Function
         
        
        
        
    End Class

End Namespace

