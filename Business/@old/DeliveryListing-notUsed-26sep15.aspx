<%@ Page Title="Delivery Listings" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="DeliveryListing.aspx.vb" Inherits="Business_DeliveryListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 55%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>
                    Delivery Listings
    </h1>

    <table class="style1">
        <tr>
            <td>
                Uploaded on From:</td>
            <td>
                <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                </td>
            <td>
                To:</td>
            <td>
                 <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                 </td>
        </tr>
        <tr>
            <td>
                DO No:</td>
            <td>
                <asp:TextBox ID="txtDoNo" runat="server"></asp:TextBox></td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
               
               Uploaded By:</td>
            <td>
                <asp:TextBox ID="txtUploadedBy" runat="server"></asp:TextBox></td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="btnSearch" runat="server" Text="Search" /></td>
            <td>
                <asp:Button ID="btnClear" runat="server" Text="Clear" /></td>
            <td></td>
        </tr>
    </table>

    <asp:GridView ID="GridView1" runat="server"  Width="100%" AllowPaging="true" PageSize="10"
                        AutoGenerateColumns="False" CellPadding="4" DataKeyNames="entryId" 
                        DataSourceID="DeliveryListingSqlSource" 
        ForeColor="#333333" GridLines="None" EmptyDataText="No data found" 
       
        ShowHeaderWhenEmpty="True">
        <PagerSettings  Mode="NumericFirstLast" FirstPageText="First" PreviousPageText="Previous" NextPageText="Next" LastPageText="Last" />
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="entryId" HeaderText="Id" InsertVisible="False" 
                                ReadOnly="True" SortExpression="entryId" />
                            <asp:BoundField DataField="DoNo" HeaderText="DO No" SortExpression="DoNo" />
                            <%--<asp:ImageField HeaderText="Delivery Order"   ItemStyle-Width="50px" ControlStyle-Width="100" ControlStyle-Height = "100" DataImageUrlField="DoCopyPath">
                                 <ItemStyle Width="100px" Height="100px" />
                                 </asp:ImageField>--%>
                                  <asp:BoundField DataField="DoCopyPath" HeaderText="Delivery Order"  />
                          <%-- <asp:ImageField HeaderText="Item Image 1"   ItemStyle-Width="50px" ControlStyle-Width="100" ControlStyle-Height = "100" DataImageUrlField="ItemImage1Path">
                                 <ItemStyle Width="100px" Height="100px" />
                                 </asp:ImageField>--%>
                                 <asp:BoundField DataField="ItemImage1Path" HeaderText="Received Item1"  />
                            <%--   <asp:ImageField HeaderText="Item Image 2"   ItemStyle-Width="50px" ControlStyle-Width="100" ControlStyle-Height = "100" DataImageUrlField="ItemImage2Path">
                                 <ItemStyle Width="100px" Height="100px" />
                                 </asp:ImageField>--%>
                                  <asp:BoundField DataField="ItemImage2Path" HeaderText="Received Item2"  />
                          <%--<asp:ImageField HeaderText="Item Image  3"   ItemStyle-Width="50px" ControlStyle-Width="100" ControlStyle-Height = "100" DataImageUrlField="ItemImage3Path">
                                 <ItemStyle Width="100px" Height="100px" />
                                 </asp:ImageField>--%>
                                 <asp:BoundField DataField="ItemImage3Path" HeaderText="Received Item3"  />
                                  <asp:BoundField DataField="ItemImage4Path" HeaderText="Rejected Item1"  />
                                   <asp:BoundField DataField="ItemImage5Path" HeaderText="Rejected Item2"  />
                            <asp:BoundField DataField="Latitude" HeaderText="Latitude" 
                                SortExpression="Latitude" />
                            <asp:BoundField DataField="Longitude" HeaderText="Longitude" 
                                SortExpression="Longitude" />                          
                            <asp:BoundField DataField="Address" HeaderText="Address" 
                                SortExpression="Address" />
                            <asp:BoundField DataField="DriverId" HeaderText="Uploaded By" 
                                SortExpression="DriverId" />
                                <asp:BoundField DataField="Createdon" HeaderText="Uploaded On" 
                                SortExpression="Createdon" />
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                    <% If GridView1.Rows.Count > 0 Then%>
                    <% If GridView1.Rows(0).Cells.Count > 1 Then%>
                    <i>You are viewing page
                    <%=GridView1.PageIndex + 1%>
                    of
                    <%=GridView1.PageCount%>
                    </i>
                    <% End If%>
                    <% End If%>
                    <asp:SqlDataSource ID="DeliveryListingSqlSource" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:Custom_CRM_DB_ConnectionString %>" 
                        SelectCommand="SELECT [entryId], [DoNo], [DoCopyPath], [ItemImage1Path], [ItemImage2Path], [ItemImage3Path],[ItemImage4Path], [ItemImage5Path], [Latitude], [Longitude],[DriverId], [Createdon],[Address]  FROM [DeliveredItems] order by createdon desc ">
                    </asp:SqlDataSource>
</asp:Content>

