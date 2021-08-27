Imports System.IO
Imports System.Data.SqlClient


Partial Class Uploadfiles
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

       
        Dim DoImage As HttpPostedFile = Request.Files("DoImage")
        Dim DoItemImage1 As HttpPostedFile = Request.Files("DoImageItem1")
        Dim DoItemImage2 As HttpPostedFile = Request.Files("DoImageItem2")
        Dim DoItemImage3 As HttpPostedFile = Request.Files("DoImageItem3")
        Dim DoItemImage4 As HttpPostedFile = Request.Files("DoImageItem4")
        Dim DoItemImage5 As HttpPostedFile = Request.Files("DoImageItem5")

            Dim DoNo As String = String.Empty
            Dim Driver As String = String.Empty
        Dim DoCopyPath As String = String.Empty
        Dim ItemImage1Path As String = String.Empty
        Dim ItemImage2Path As String = String.Empty
        Dim ItemImage3Path As String = String.Empty
        Dim ItemImage4Path As String = String.Empty
        Dim ItemImage5Path As String = String.Empty

        Dim Latitude As String = String.Empty
        Dim Longitude As String = String.Empty

        'check file was submitted
        If DoImage IsNot Nothing AndAlso DoImage.ContentLength > 0 Then
            Dim fname As String = Path.GetFileName(DoImage.FileName)
                DoCopyPath = GenerateFilePath(fname)
                DoImage.SaveAs(Server.MapPath(DoCopyPath))
        End If

        'check file was submitted
        If DoItemImage1 IsNot Nothing AndAlso DoItemImage1.ContentLength > 0 Then
            Dim fname As String = Path.GetFileName(DoItemImage1.FileName)
                ItemImage1Path = GenerateFilePath(fname)
                DoItemImage1.SaveAs(Server.MapPath(ItemImage1Path))
        End If

        'check file was submitted
        If DoItemImage2 IsNot Nothing AndAlso DoItemImage2.ContentLength > 0 Then
            Dim fname As String = Path.GetFileName(DoItemImage2.FileName)
                ItemImage2Path = GenerateFilePath(fname)
                DoItemImage2.SaveAs(Server.MapPath(ItemImage2Path))
        End If

        'check file was submitted
        If DoItemImage3 IsNot Nothing AndAlso DoItemImage3.ContentLength > 0 Then
            Dim fname As String = Path.GetFileName(DoItemImage3.FileName)
                ItemImage3Path = GenerateFilePath(fname)
                DoItemImage3.SaveAs(Server.MapPath(ItemImage3Path))
        End If

            'check file was submitted
            If DoItemImage4 IsNot Nothing AndAlso DoItemImage4.ContentLength > 0 Then
                Dim fname As String = Path.GetFileName(DoItemImage4.FileName)
                ItemImage4Path = GenerateFilePath(fname)
                DoItemImage4.SaveAs(Server.MapPath(ItemImage4Path))
            End If
            'check file was submitted
            If DoItemImage5 IsNot Nothing AndAlso DoItemImage5.ContentLength > 0 Then
                Dim fname As String = Path.GetFileName(DoItemImage5.FileName)
                ItemImage5Path = GenerateFilePath(fname)
                DoItemImage5.SaveAs(Server.MapPath(ItemImage5Path))
            End If

        Dim FormValues As NameValueCollection = Request.Form
        If Not IsNothing(FormValues) Then

            If Not String.IsNullOrEmpty(FormValues("DoNo")) Then
                DoNo = FormValues("DoNo").ToString


                If Not String.IsNullOrEmpty(FormValues("Latitude")) Then
                    Latitude = FormValues("Latitude").ToString
                End If
                If Not String.IsNullOrEmpty(FormValues("Longitude")) Then
                    Longitude = FormValues("Longitude").ToString
                End If
                    If Not String.IsNullOrEmpty(FormValues("DriverId")) Then
                        Driver = FormValues("DriverId").ToString
                    End If
                Dim CustomSqlInsertConnection As New SqlConnection
                CustomSqlInsertConnection.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_CRM_DB_ConnectionString").ConnectionString
                CustomSqlInsertConnection.Open()

                Dim InsertCommand As New SqlCommand
                    InsertCommand.CommandText = "INSERT INTO DeliveredItems (DoNo, DoCopyPath,ItemImage1Path, ItemImage2Path, ItemImage3Path,ItemImage4Path,ItemImage5Path,Latitude,Longitude, DriverId, Createdon) Values(@DoNo, @DoCopyPath, @ItemImage1Path, @ItemImage2Path, @ItemImage3Path,@ItemImage4Path,@ItemImage5Path,@Latitude,@Longitude, @DriverId,  @Createdon)"
                InsertCommand.Connection = CustomSqlInsertConnection
                InsertCommand.Parameters.AddWithValue("DoNo", DoNo)
                InsertCommand.Parameters.AddWithValue("DoCopyPath", DoCopyPath)
                InsertCommand.Parameters.AddWithValue("ItemImage1Path", ItemImage1Path)
                InsertCommand.Parameters.AddWithValue("ItemImage2Path", ItemImage2Path)
                InsertCommand.Parameters.AddWithValue("ItemImage3Path", ItemImage3Path)
                    InsertCommand.Parameters.AddWithValue("ItemImage4Path", ItemImage4Path)
                    InsertCommand.Parameters.AddWithValue("ItemImage5Path", ItemImage5Path)
                InsertCommand.Parameters.AddWithValue("Latitude", Latitude)
                    InsertCommand.Parameters.AddWithValue("Longitude", Longitude)
                    InsertCommand.Parameters.AddWithValue("DriverId", Driver)
                InsertCommand.Parameters.AddWithValue("Createdon", Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss"))
                InsertCommand.ExecuteReader()
                InsertCommand.Dispose()
                CustomSqlInsertConnection.Close()
            Else
                Response.Write(" No data found")
            End If

            '    DoNo = FormValues("DoNo").ToString



            '    Dim CustomSqlInsertConnection As New SqlConnection
            '    CustomSqlInsertConnection.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_CRM_DB_ConnectionString").ConnectionString
            '    CustomSqlInsertConnection.Open()


            '    Dim InsertCommand As New SqlCommand
            '    InsertCommand.CommandText = "INSERT INTO DraftDo (DoNo,Createdon) Values(@DocNum, @Createdon)"
            '    InsertCommand.Connection = CustomSqlInsertConnection
            '    InsertCommand.Parameters.AddWithValue("DoNo", DoNo)
            '    'InsertCommand.Parameters.AddWithValue("UpdatedBy", User.Identity.Name)
            '    InsertCommand.Parameters.AddWithValue("Createdon", Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss"))
            '    InsertCommand.ExecuteReader()
            '    InsertCommand.Dispose()
            '    CustomSqlInsertConnection.Close()
        Else
            Response.Write("No Post")
        End If
        Catch ex As Exception
            Dim CustomSqlInsertConnection As New SqlConnection
            CustomSqlInsertConnection.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_CRM_DB_ConnectionString").ConnectionString
            CustomSqlInsertConnection.Open()

            Dim InsertCommand As New SqlCommand
            InsertCommand.CommandText = "INSERT INTO ErrLog (LogMsg) Values(@LogMsg)"
            InsertCommand.Connection = CustomSqlInsertConnection
            InsertCommand.Parameters.AddWithValue("LogMsg", ex.Message)
            InsertCommand.ExecuteReader()
            InsertCommand.Dispose()
            CustomSqlInsertConnection.Close()
        End Try
    End Sub
    Protected Function GenerateFilePath(ByVal FileName As String) As String
        Dim FilePath As String = (Path.Combine("~/Files/", FileName))
        While File.Exists(Server.MapPath(FilePath))
            Dim origin As New DateTime(1970, 1, 1, 0, 0, 0, 0)
            Dim Diff As New TimeSpan
            Diff = Now - origin
            Dim FileNameGenerated As String = Math.Floor(Diff.TotalSeconds)
            FilePath = (Path.Combine("~/Files/", FileNameGenerated & "_" & FileName))
        End While
        Return FilePath
    End Function    
End Class
