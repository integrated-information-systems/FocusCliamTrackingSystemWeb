<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="TransportMode.aspx.vb" Inherits="Business_TransportMode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 32%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
<h1>Transport Mode Master</h1>
    <table class="style1">
        <tr>
            <td>
                Transport Mode</td>
            <td>
                <asp:TextBox ID="txtTransModeName" runat="server" MaxLength="100"></asp:TextBox><asp:RequiredFieldValidator
                    ID="txtTransModeNameRequiredFieldValidator" runat="server" ErrorMessage="*" ControlToValidate="txtTransModeName" ValidationGroup="HeaderValidation"></asp:RequiredFieldValidator>
                <asp:CustomValidator ID="txtTransModeNameCustomValidator" runat="server" ErrorMessage="already exist" ControlToValidate="txtTransModeName" ValidationGroup="HeaderValidation"></asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" ValidationGroup="HeaderValidation"/>
                <asp:Button ID="btnClear" runat="server" Text="Clear" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" runat="server"  
        DataSourceID="SqlDataSource1" AutoGenerateSelectButton="True"
        Width="20%" DataKeyNames="TransModeId"
        CellPadding="4"   
        HeaderStyle-HorizontalAlign="Center" RowStyle-HorizontalAlign="Left" 
        ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" 
        EmptyDataText="No record Found" ShowHeaderWhenEmpty="True">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <%--<asp:BoundField DataField="TransModeId" HeaderText="S. No"  
                InsertVisible="False" ReadOnly="True" SortExpression="TransModeId" />--%>
            <asp:BoundField DataField="ModeName" HeaderText="Transport Mode" 
                SortExpression="ModeName" />
            
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
        SelectCommand="SELECT [ModeName], [TransModeId] FROM [TransportMode]">
    </asp:SqlDataSource>
</asp:Content>


