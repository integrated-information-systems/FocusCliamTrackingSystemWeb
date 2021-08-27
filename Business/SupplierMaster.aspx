<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="SupplierMaster.aspx.vb" Inherits="Business_SupplierMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 51%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<h1>Supplier Master</h1>

    <table class="style1">
        <tr>
            <td>
                Supplier Name</td>
            <td>
                <asp:TextBox ID="txtSupplierName" runat="server" MaxLength="100" Width="293px"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                    ID="txtSupplierNameRequiredFieldValidator" runat="server" ValidationGroup="HeaderValidation" ControlToValidate="txtSupplierName" ErrorMessage="*"></asp:RequiredFieldValidator>                
            </td>
        </tr>
        <tr>
            <td>
                Contact Person</td>
            <td>
                <asp:TextBox ID="txtContactPerson" runat="server" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Contact Number</td>
            <td>
                <asp:TextBox ID="txtContactNumber" runat="server" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                Email</td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server" MaxLength="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="HeaderValidation" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="SupplierGrid" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSource1" AutoGenerateSelectButton="True" DataKeyNames="IdKey"
         Width="50%"
        CellPadding="4"   
        HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Left" 
        ForeColor="#333333" GridLines="None"  
        EmptyDataText="No record Found" ShowHeaderWhenEmpty="True">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:BoundField DataField="SupplierName" HeaderText="Supplier Name" 
                    SortExpression="SupplierName" />
            <asp:BoundField DataField="ContactPerson" HeaderText="Contact Person" 
                   SortExpression="ContactPerson" />
            <asp:BoundField DataField="ContactNumber" HeaderText="Contact Number" 
                   SortExpression="ContactNumber" />
            <asp:BoundField DataField="Email" HeaderText="Email" 
                   SortExpression="Email" />
        </Columns>
        <EditRowStyle BackColor="#999999" />
    <EmptyDataRowStyle BackColor="#00CCFF" />
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" HorizontalAlign="Center" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <SortedAscendingCellStyle BackColor="#E9E7E2" />
    <SortedAscendingHeaderStyle BackColor="#506C8C" />
    <SortedDescendingCellStyle BackColor="#FFFDF8" />
    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:Custom_DB_ConnectionString %>" 
        SelectCommand="SELECT [IdKey], [SupplierName],[ContactPerson],[ContactNumber],[Email] FROM [SupplierMaster]">
    </asp:SqlDataSource>
</asp:Content>

