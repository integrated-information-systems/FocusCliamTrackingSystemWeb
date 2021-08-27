<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="ClaimList.aspx.vb" Inherits="Business_ClaimList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 46%;
        }
    </style>
    <link href="../Styles/jquery-ui.css" 
            rel="stylesheet" type="text/css"/>       
       <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript" language="javascript" ></script>
       <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../Scripts/JScript.js" type="text/javascript" language="javascript" ></script>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Claim Listing</h1>
    <table class="style1">
        <tr>
            <td>
                Date From</td>
            <td>
                <asp:TextBox ID="txtFromDate" runat="server"></asp:TextBox>
                <asp:CustomValidator ID="FromDateValidator" runat="server" 
                    ErrorMessage="Invalid" ValidationGroup="HeaderGroup"></asp:CustomValidator>
            </td>
            <td>
                To</td>
            <td>
                <asp:TextBox ID="txtToDate" runat="server"></asp:TextBox>
                <asp:CustomValidator ID="ToDateValidator" runat="server" 
                    ErrorMessage="Invalid" ValidationGroup="HeaderGroup"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td>
                Status</td>
            <td>
                <asp:DropDownList ID="ddlStatus" runat="server">
                    <asp:ListItem Text="All" Value="A"></asp:ListItem>
                    <asp:ListItem Text="Open" Value="Open"></asp:ListItem>
                    <asp:ListItem Text="Complete" Value="Complete"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="HeaderGroup" /><asp:Button ID="btnClear"
                    runat="server" Text="Clear" /></td>
            <td></td>
            <td></td>

        </tr>
    </table>
  <%--<asp:BulletedList ID="PendingClaimList" runat="server" BulletStyle="Numbered" DisplayMode="LinkButton"
                                ViewStateMode="Enabled">
                            </asp:BulletedList>--%>    
    <asp:GridView ID="DGPendingClaims" runat="server"  Width="100%"
        CellPadding="4" DataKeyNames="id" DataSourceID="SqlPendingClaims"  
        HeaderStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="Left"
        ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" 
        EmptyDataText="No record Found" ShowHeaderWhenEmpty="True">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
             <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                  <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%#Eval("id").ToString%>'
                                            CommandName="CustomEdit" Text="Edit/View" />
                                            </ItemTemplate>
            </asp:TemplateField>         
            <asp:BoundField DataField="id" HeaderText="Claim Id" 
                SortExpression="CustomerName" />
            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" 
                SortExpression="CustomerName" />
            <asp:BoundField DataField="Personincharge" HeaderText="Personin Charge" 
                SortExpression="Personincharge" />
            <asp:BoundField DataField="EmailAddress" HeaderText="Email Address" 
                SortExpression="EmailAddress" />
            <asp:BoundField DataField="PhoneNo" HeaderText="Phone No" 
                SortExpression="PhoneNo" />
            <asp:BoundField DataField="CountryName" HeaderText="Country Name" 
                SortExpression="CountryName" />
            <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:dd-MM-yyyy}" SortExpression="Date" />
	    <asp:BoundField DataField="CreatedBy" HeaderText="Created By"  Visible="false"
                SortExpression="CreatedBy" />
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
    <asp:SqlDataSource ID="SqlPendingClaims" runat="server" 
        ConnectionString="<%$ ConnectionStrings:Custom_DB_ConnectionString %>" 
        SelectCommand="SELECT [id], [CustomerName], [Personincharge], [EmailAddress], [PhoneNo], [CountryName], [Date],[CreatedBy] FROM [ClaimHeader] WHERE  Country=@country">
        <SelectParameters>         
        <asp:Parameter Name="country" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

