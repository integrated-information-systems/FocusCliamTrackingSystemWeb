Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.IO
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Xml
Imports System.Globalization

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class WebService
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Function HelloWorld() As String
        Return "Hello World"
    End Function

    <WebMethod()> _
    Public Function Login(ByVal UserName As String, ByVal Password As String, ByVal Country As String) As Boolean
        Try
            Dim Authenticated As Boolean = False
            Authenticated = Membership.ValidateUser(UserName, Password)

            If Authenticated = True Then
                Dim UpdateProfile As ProfileCommon = ProfileBase.Create(UserName, True)
                Dim CountryList As String = UpdateProfile.Country()

                Dim SplittedCountry() As String = CountryList.Split("|")
                For Each i In SplittedCountry
                    If i = Country Then
                        Authenticated = True
                        Exit For
                    Else
                        Authenticated = False
                    End If
                Next
            End If

            Return Authenticated
        Catch ex As Exception
            Return False
        End Try
    End Function
    <WebMethod()> _
    Public Function ReceiveFiles() As Boolean
        Try
            Dim file As HttpPostedFile = HttpContext.Current.Request.Files(0)
            Using fileStream = New System.IO.FileStream(Server.MapPath("~") & "\_tmpUploadedfiles\" + file.FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write)
                file.InputStream.CopyTo(fileStream)
            End Using
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    '<WebMethod()> _
    'Public Function ReceiveFiles(ByVal Photo As String, ByVal FileName As String) As Boolean
    '    Try
    '        Using ms As New MemoryStream(Convert.FromBase64String(Photo))
    '            Using bm2 As New Bitmap(ms)
    '                bm2.Save(Server.MapPath("~") & "\_tmpUploadedfiles\" + FileName)
    '            End Using
    '        End Using
    '        Return True
    '    Catch ex As Exception
    '        Return False
    '    End Try
    'End Function
    <WebMethod()> _
    Public Function WriteClaimInfo(ByVal XMLInfo As String, ByVal Uploadedby As String) As Boolean
        Try


            XMLInfo = XMLInfo.Replace("&lt;", "<").Replace("&gt;", ">")



            Dim SqlCon As SqlConnection = New SqlConnection()
            SqlCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
            SqlCon.Open()
            Dim ReaderCommand As SqlCommand = New SqlCommand()
            ReaderCommand.Connection = SqlCon
            ReaderCommand.CommandText = "INSERT INTO UploadedClaims (Uploadedby,UploadedXML,UploadedOn) Values(@Uploadedby, @UploadedXML, @UploadedOn) "
            ReaderCommand.Parameters.AddWithValue("@Uploadedby", Uploadedby)
            ReaderCommand.Parameters.AddWithValue("@UploadedXML", XMLInfo)
            ReaderCommand.Parameters.AddWithValue("@UploadedOn", Format(CDate(DateTime.Now), "yyyy-MM-dd HH:mm:ss"))

            ReaderCommand.ExecuteNonQuery()


            ReaderCommand.Dispose()
            SqlCon.Close()

            Dim PersonIncharge As String
            Dim EndCustomerName As String
            Dim EmailAddress As String
            Dim HeaderTelNo As String
            Dim Brand As String
            Dim MachineModelNo As String
            Dim Country As String = String.empty
            Dim CountryName As String = "Bangladesh"
            Dim CreatedBy As String = String.empty

            Dim ShipToCompany As String
            Dim ShipToAddress As String
            Dim ContactPerson As String
            Dim FooterTelNo As String
            Dim Remarks As String

            Dim xml As New XmlDocument()
            xml.LoadXml(XMLInfo)

            Dim HeaderInfoNode As XmlNode = xml.SelectSingleNode("/ClaimInfo/HeaderInfo")
            PersonIncharge = HeaderInfoNode.SelectSingleNode("PersonIncharge").InnerText
            EndCustomerName = HeaderInfoNode.SelectSingleNode("EndCustomerName").InnerText
            EmailAddress = HeaderInfoNode.SelectSingleNode("EmailAddress").InnerText
            HeaderTelNo = HeaderInfoNode.SelectSingleNode("TelNo").InnerText
            Brand = HeaderInfoNode.SelectSingleNode("Brand").InnerText
            MachineModelNo = HeaderInfoNode.SelectSingleNode("MachineModelNo").InnerText
            Country = HeaderInfoNode.SelectSingleNode("Country").InnerText
            CreatedBy = HeaderInfoNode.SelectSingleNode("CreatedBy").InnerText


            Dim FooterInfoNode As XmlNode = xml.SelectSingleNode("/ClaimInfo/FooterInfo")
            ShipToCompany = FooterInfoNode.SelectSingleNode("ShipToCompany").InnerText
            ShipToAddress = FooterInfoNode.SelectSingleNode("ShipToAddress").InnerText
            ContactPerson = FooterInfoNode.SelectSingleNode("ContactPerson").InnerText
            FooterTelNo = FooterInfoNode.SelectSingleNode("TelNo").InnerText
            Remarks = FooterInfoNode.SelectSingleNode("Remarks").InnerText

            Dim LineItemHeader As XmlNode = xml.SelectSingleNode("/ClaimInfo/ClaimLines")
            Dim LineItemNodes As XmlNodeList = LineItemHeader.SelectNodes("LineInfo")

            Dim SqlHeaderCon As SqlConnection = New SqlConnection()
            SqlHeaderCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
            SqlHeaderCon.Open()
            Dim ReaderHeaderCommand As SqlCommand = New SqlCommand()
            ReaderHeaderCommand.Connection = SqlHeaderCon
            ReaderHeaderCommand.CommandText = "INSERT INTO ClaimHeader ([CustomerName],[Personincharge],[EmailAddress],[PhoneNo],[Date],[Country],[CountryName],[ShipToCompany],[ShipToAddress], [ShipToContact], [ShipToPhone],[reasonforrejection], [CreatedBy],[Brand], [ModelNo], [Status]) OUTPUT INSERTED.id Values(@CustomerName, @Personincharge, @EmailAddress, @PhoneNo, @Date, @Country, @CountryName, @ShipToCompany, @ShipToAddress, @ShipToContact, @ShipToPhone,@reasonforrejection,@CreatedBy, @Brand, @ModelNo, @Status ) "
            ReaderHeaderCommand.Parameters.AddWithValue("@CustomerName", EndCustomerName)
            ReaderHeaderCommand.Parameters.AddWithValue("@Personincharge", PersonIncharge)
            ReaderHeaderCommand.Parameters.AddWithValue("@EmailAddress", EmailAddress)
            ReaderHeaderCommand.Parameters.AddWithValue("@PhoneNo", HeaderTelNo)
            ReaderHeaderCommand.Parameters.AddWithValue("@Date", Format(CDate(DateTime.Now), "yyyy-MM-dd"))
            ReaderHeaderCommand.Parameters.AddWithValue("@CreatedBy", CreatedBy)
            ReaderHeaderCommand.Parameters.AddWithValue("@Country", Country)
            ReaderHeaderCommand.Parameters.AddWithValue("@CountryName", CountryName)
            ReaderHeaderCommand.Parameters.AddWithValue("@ShipToCompany", ShipToCompany)
            ReaderHeaderCommand.Parameters.AddWithValue("@ShipToAddress", ShipToAddress)
            ReaderHeaderCommand.Parameters.AddWithValue("@ShipToContact", ContactPerson)
            ReaderHeaderCommand.Parameters.AddWithValue("@ShipToPhone", FooterTelNo)
            ReaderHeaderCommand.Parameters.AddWithValue("@reasonforrejection", Remarks)
            ReaderHeaderCommand.Parameters.AddWithValue("@Brand", Brand)
            ReaderHeaderCommand.Parameters.AddWithValue("@ModelNo", MachineModelNo)
            ReaderHeaderCommand.Parameters.AddWithValue("@Status", "Open")
            Dim HeaderID As Integer = ReaderHeaderCommand.ExecuteScalar()

            ReaderHeaderCommand.Dispose()

            SqlHeaderCon.Close()


            Dim SqlLinesCon As SqlConnection = New SqlConnection()
            SqlLinesCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
            SqlLinesCon.Open()
            Dim i As Integer = 1
            For Each node As XmlNode In LineItemNodes
                Dim Lineno As String = node.Attributes.GetNamedItem("LineNo").Value
                Dim PartNo As String = node.Attributes.GetNamedItem("PartNo").Value
                Dim SerialNo As String = node.Attributes.GetNamedItem("SerialNo").Value
                Dim Qty As String = node.Attributes.GetNamedItem("Qty").Value
                Dim InvoiceNo As String = node.Attributes.GetNamedItem("InvoiceNo").Value
                Dim InvoiceDate As String = node.Attributes.GetNamedItem("InvoiceDate").Value
                Dim ValidDate As New Date

                Dim ReaderLinesCommand As SqlCommand = New SqlCommand()
                ReaderLinesCommand.Connection = SqlLinesCon
                ReaderLinesCommand.CommandText = "INSERT INTO ClaimLineItems ([HeaderId],[SNo],[InvDate],[InvNo],[Qty],[ClaimQty],[Brand],[ModelNo],[SerialNo],[Particulars])  Values(@HeaderId ,@SNo,@InvDate,@InvNo,@Qty,@ClaimQty,@Brand,@ModelNo,@SerialNo,@Particulars) "
                ReaderLinesCommand.Parameters.AddWithValue("@HeaderId", HeaderID)
                ReaderLinesCommand.Parameters.AddWithValue("@SNo", i)
                If Date.TryParseExact(InvoiceDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, ValidDate) = True Then
                    ReaderLinesCommand.Parameters.AddWithValue("@InvDate", Format(ValidDate, "yyyy-MM-dd"))
                Else
                    ReaderLinesCommand.Parameters.AddWithValue("@InvDate", Format(CDate(DateTime.Now), "yyyy-MM-dd"))
                End If

                ReaderLinesCommand.Parameters.AddWithValue("@InvNo", InvoiceNo)
                ReaderLinesCommand.Parameters.AddWithValue("@Qty", Qty)
                ReaderLinesCommand.Parameters.AddWithValue("@ClaimQty", Qty)
                ReaderLinesCommand.Parameters.AddWithValue("@Brand", Brand)
                ReaderLinesCommand.Parameters.AddWithValue("@ModelNo", MachineModelNo)
                ReaderLinesCommand.Parameters.AddWithValue("@SerialNo", SerialNo)
                ReaderLinesCommand.Parameters.AddWithValue("@Particulars", PartNo)

                ReaderLinesCommand.ExecuteNonQuery()
                ReaderLinesCommand.Dispose()
                i = i + 1
            Next
            SqlLinesCon.Close()

            Dim SqlPhotoCon As SqlConnection = New SqlConnection()
            SqlPhotoCon.ConnectionString = ConfigurationManager.ConnectionStrings("Custom_DB_ConnectionString").ConnectionString
            SqlPhotoCon.Open()
            For Each node As XmlNode In LineItemNodes

                Dim FileList As List(Of String) = New List(Of String)
                Dim strPostedFileName As String = node.Attributes.GetNamedItem("Photo").Value
                Dim strPostedFileName1 As String = node.Attributes.GetNamedItem("Photo1").Value
                Dim strPostedFileName2 As String = node.Attributes.GetNamedItem("Photo2").Value
                Dim strPostedFileName3 As String = node.Attributes.GetNamedItem("Photo3").Value

                If Not String.IsNullOrEmpty(strPostedFileName) Then
                    FileList.Add(strPostedFileName)
                End If
                If Not String.IsNullOrEmpty(strPostedFileName1) Then
                    FileList.Add(strPostedFileName1)
                End If
                If Not String.IsNullOrEmpty(strPostedFileName2) Then
                    FileList.Add(strPostedFileName2)
                End If
                If Not String.IsNullOrEmpty(strPostedFileName3) Then
                    FileList.Add(strPostedFileName3)
                End If

                For Each itm In FileList
                    Dim FileSavePath As String = String.Empty
                    FileSavePath = Server.MapPath("~") & "\" & System.Configuration.ConfigurationManager.AppSettings("TempUploadPath").ToString & "\" & itm

                    Dim ReaderPhotoCommand As SqlCommand = New SqlCommand()
                    ReaderPhotoCommand.Connection = SqlPhotoCon
                    ReaderPhotoCommand.CommandText = "INSERT INTO Files (HeaderId, Path) Values(@HeaderId, @Path) "
                    ReaderPhotoCommand.Parameters.AddWithValue("@HeaderId", HeaderID)
                    ReaderPhotoCommand.Parameters.AddWithValue("@Path", FileSavePath)
                    ReaderPhotoCommand.ExecuteNonQuery()
                    ReaderPhotoCommand.Dispose()

                Next






            Next
            SqlPhotoCon.Close()

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Class