Imports System.Net
Imports System.Data.SqlClient
Imports System.Data
Imports System.Globalization

Partial Class Business_DeliveryListing
    Inherits System.Web.UI.Page

    Dim baseUri As String = "http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false"
    Public Function RetrieveFormatedAddress(ByVal lat As String, ByVal lng As String) As String
        Dim requestUri As String = String.Format(baseUri, lat, lng)
        Dim Finalresult As String = String.Empty
        Using wc As New WebClient()
            Dim result As String = wc.DownloadString(requestUri)
            Dim xmlElm = XElement.Parse(result)

            Dim status = (From elm In xmlElm.Descendants() Where elm.Name = "status" Select elm).FirstOrDefault()
            If status.Value.ToLower() = "ok" Then
                Dim res = (From elm In xmlElm.Descendants() Where elm.Name = "formatted_address" Select elm).ElementAtOrDefault(1)
                Finalresult = res.Value
            End If
        End Using
        Return Finalresult
    End Function
    Protected Sub UpdateAddresss()
        Dim CustomSqlSelectConnection As New SqlConnection
        CustomSqlSelectConnection.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_CRM_DB_ConnectionString").ConnectionString
        CustomSqlSelectConnection.Open()

        Dim SelectCommand As New SqlCommand
        SelectCommand.CommandText = "SELECT entryId, Latitude, Longitude FROM  DeliveredItems WHERE  Address IS NULL OR LEN(Address)=0"
        SelectCommand.Connection = CustomSqlSelectConnection
        Dim DR As SqlDataReader = SelectCommand.ExecuteReader()
        Dim DT As New DataTable
        DT.Load(DR)

        SelectCommand.Dispose()
        CustomSqlSelectConnection.Close()
        If DT.Rows.Count > 0 Then

            For Each DatRow As DataRow In DT.Rows
                Dim CustomSqlUpateConnection As New SqlConnection
                CustomSqlUpateConnection.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_CRM_DB_ConnectionString").ConnectionString
                CustomSqlUpateConnection.Open()
                Dim Address As String = String.Empty
                Dim entryId As String = String.Empty
                entryId = DatRow("entryId").ToString()
                Address = RetrieveFormatedAddress(DatRow("Latitude").ToString, DatRow("Longitude").ToString)

                Dim UpdateCommand As New SqlCommand
                UpdateCommand.CommandText = "Update  DeliveredItems SET Address=@Address  WHERE  entryId=@entryId"
                UpdateCommand.Connection = CustomSqlUpateConnection
                UpdateCommand.Parameters.AddWithValue("Address", Address)
                UpdateCommand.Parameters.AddWithValue("entryId", entryId)
                UpdateCommand.ExecuteReader()
                UpdateCommand.Dispose()
                CustomSqlUpateConnection.Close()
            Next

        End If
    End Sub

    Protected Sub GridView1_Init(sender As Object, e As System.EventArgs) Handles GridView1.Init
        UpdateAddresss()
    End Sub
    Protected Sub GridView1_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataBind()
    End Sub
    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Dim LastCell = e.Row.Cells(10)
            'LastCell.Text = RetrieveFormatedAddress(e.Row.Cells(8).Text, e.Row.Cells(9).Text)

            Dim ImageCell = e.Row.Cells(2)
            If ImageCell.Text = "&nbsp;" Then
                ImageCell.Text = "<a style='height:100px;width:100px;' class='example1' href='../Files/noimage.jpg'><img style='height:100px;width:100px;' src='../Files/noimage.jpg' /></a>"
            Else
                ImageCell.Text = "<a style='height:100px;width:100px;' class='example1' href='" + ImageCell.Text.Replace("~", "..") + "'><img style='height:100px;width:100px;' src='" + ImageCell.Text.Replace("~", "..") + "' /></a>"
            End If

            ImageCell = e.Row.Cells(3)
            If ImageCell.Text = "&nbsp;" Then
                ImageCell.Text = "<a style='height:100px;width:100px;' class='example1' href='../Files/noimage.jpg'><img style='height:100px;width:100px;' src='../Files/noimage.jpg' /></a>"
            Else
                ImageCell.Text = "<a style='height:100px;width:100px;' class='example1' href='" + ImageCell.Text.Replace("~", "..") + "'><img style='height:100px;width:100px;' src='" + ImageCell.Text.Replace("~", "..") + "' /></a>"
            End If

            ImageCell = e.Row.Cells(4)
            If ImageCell.Text = "&nbsp;" Then
                ImageCell.Text = "<a style='height:100px;width:100px;' class='example1' href='../Files/noimage.jpg'><img style='height:100px;width:100px;' src='../Files/noimage.jpg' /></a>"
            Else
                ImageCell.Text = "<a style='height:100px;width:100px;' class='example1' href='" + ImageCell.Text.Replace("~", "..") + "'><img style='height:100px;width:100px;' src='" + ImageCell.Text.Replace("~", "..") + "' /></a>"
            End If

            ImageCell = e.Row.Cells(5)
            If ImageCell.Text = "&nbsp;" Then
                ImageCell.Text = "<a style='height:100px;width:100px;' class='example1' href='../Files/noimage.jpg'><img style='height:100px;width:100px;' src='../Files/noimage.jpg' /></a>"
            Else
                ImageCell.Text = "<a style='height:100px;width:100px;' class='example1' href='" + ImageCell.Text.Replace("~", "..") + "'><img style='height:100px;width:100px;' src='" + ImageCell.Text.Replace("~", "..") + "' /></a>"
            End If

            ImageCell = e.Row.Cells(6)
            If ImageCell.Text = "&nbsp;" Then
                ImageCell.Text = "<a style='height:100px;width:100px;' class='example1' href='../Files/noimage.jpg'><img style='height:100px;width:100px;' src='../Files/noimage.jpg' /></a>"
            Else
                ImageCell.Text = "<a style='height:100px;width:100px;' class='example1' href='" + ImageCell.Text.Replace("~", "..") + "'><img style='height:100px;width:100px;' src='" + ImageCell.Text.Replace("~", "..") + "' /></a>"
            End If

            ImageCell = e.Row.Cells(7)
            If ImageCell.Text = "&nbsp;" Then
                ImageCell.Text = "<a style='height:100px;width:100px;' class='example1' href='../Files/noimage.jpg'><img style='height:100px;width:100px;' src='../Files/noimage.jpg' /></a>"
            Else
                ImageCell.Text = "<a style='height:100px;width:100px;' class='example1' href='" + ImageCell.Text.Replace("~", "..") + "'><img style='height:100px;width:100px;' src='" + ImageCell.Text.Replace("~", "..") + "' /></a>"
            End If

            For i = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).HorizontalAlign = HorizontalAlign.Left
            Next
        End If
        If e.Row.RowType = DataControlRowType.Header Then
            For i = 0 To e.Row.Cells.Count - 1
                e.Row.Cells(i).HorizontalAlign = HorizontalAlign.Left
            Next
        End If
    End Sub
    Protected Function GetSorting() As String
        Try
            If IsNothing(ViewState("Sorting")) Then
                ViewState("Sorting") = "ASC"
            Else

                If ViewState("Sorting").ToString = "DESC" Then
                    ViewState("Sorting") = "ASC"
                Else
                    ViewState("Sorting") = "DESC"
                End If
            End If
            Return ViewState("Sorting").ToString
        Catch ex As Exception
            Return "DESC"
        End Try
    End Function

    Protected Sub GridView1_Sorting(sender As Object, e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting

    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As System.EventArgs) Handles btnSearch.Click
        LoadData()
    End Sub
    Protected Sub LoadData()
        Dim ValidDate As New Date

        DeliveryListingSqlSource.SelectParameters.Clear()

        Dim Conditions As New List(Of String)
        DeliveryListingSqlSource.SelectCommand = " SELECT [entryId], [DoNo], [DoCopyPath], [ItemImage1Path], [ItemImage2Path], [ItemImage3Path],[ItemImage4Path], [ItemImage5Path], [Latitude], [Longitude],[DriverId], [Createdon],[Address]  FROM [DeliveredItems]  "

        If txtFromDate.Text <> String.Empty Then
            If Date.TryParseExact(txtFromDate.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, ValidDate) = True Then
                Conditions.Add("Createdon>=@FromDate")
                DeliveryListingSqlSource.SelectParameters.Add("FromDate", ValidDate.ToString("yyyy-MM-dd"))
            End If
        End If
        If txtToDate.Text <> String.Empty Then
            If Date.TryParseExact(txtToDate.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, ValidDate) = True Then
                Conditions.Add("Createdon<=@ToDate")
                DeliveryListingSqlSource.SelectParameters.Add("ToDate", ValidDate.ToString("yyyy-MM-dd"))
            End If
        End If

        If txtUploadedBy.Text <> String.Empty Then
            Conditions.Add("DriverId=@DriverId")
            DeliveryListingSqlSource.SelectParameters.Add("DriverId", txtUploadedBy.Text)
        End If

        If txtDoNo.Text <> String.Empty Then
            Conditions.Add("DoNo=@DoNo")
            DeliveryListingSqlSource.SelectParameters.Add("DoNo", txtDoNo.Text)
        End If

        If Conditions.Count > 0 Then
            DeliveryListingSqlSource.SelectCommand = DeliveryListingSqlSource.SelectCommand & " WHERE "
            DeliveryListingSqlSource.SelectCommand = DeliveryListingSqlSource.SelectCommand & String.Join(" AND ", Conditions.ToArray)

        End If

        DeliveryListingSqlSource.SelectCommand = DeliveryListingSqlSource.SelectCommand & " order by createdon desc"

        DeliveryListingSqlSource.DataBind()

    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        LoadData()
    End Sub

    Protected Sub btnClear_Click(sender As Object, e As System.EventArgs) Handles btnClear.Click
        txtDoNo.Text = String.Empty
        txtUploadedBy.Text = String.Empty
        txtFromDate.Text = String.Empty
        txtToDate.Text = String.Empty
    End Sub
End Class
