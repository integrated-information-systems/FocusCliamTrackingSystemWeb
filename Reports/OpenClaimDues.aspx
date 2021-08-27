<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="OpenClaimDues.aspx.vb" Inherits="Reports_OpenClaimDues" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 139px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Open Claims for more than x days</h1>

    <table class="style1">
        <tr>
            <td class="style2" valign="top">
                Claim Open More than</td>
            <td valign="top">
                <asp:DropDownList ID="ddldays" runat="server">       
                    <asp:ListItem Value="<=7"><= 7 days</asp:ListItem>
                    <asp:ListItem Value=">7 AND <=14">>7 and <=14 days</asp:ListItem>
                    <asp:ListItem Value=">14 AND <=21">>14 and <=21 days</asp:ListItem>
                     <asp:ListItem Value=">21">>21 days</asp:ListItem>
                </asp:DropDownList>
                days</td>
        </tr>
        <tr>
            <td class="style2" valign="top">
                &nbsp;</td>
            <td valign="top">
                <asp:Button ID="btnSearch" runat="server" Text="Search" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" />
            </td>
        </tr>
    </table>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        CellPadding="4" DataSourceID="SqlPendingClaims" ForeColor="#333333" 
        GridLines="None" Width="100%" EmptyDataText="No record found." 
        ShowHeaderWhenEmpty="True">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
         <asp:BoundField DataField="id" HeaderText="Claim No" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                SortExpression="id" />
            <asp:BoundField DataField="CustomerName" HeaderText="Customer Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                SortExpression="CustomerName" />
            <asp:BoundField DataField="Personincharge" HeaderText="Person in charge" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                SortExpression="Personincharge" />           
            <asp:BoundField DataField="DocDate" HeaderText="Date" SortExpression="DocDate" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:dd-MM-yyyy}"  />            
            <asp:BoundField DataField="CountryName" HeaderText="Country Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                SortExpression="CountryName" />
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
        SelectCommand="SELECT [id], [CustomerName], [Personincharge], [EmailAddress], [DocDate],[CountryName] FROM [ClaimHeader] WHERE Status=@status AND DATEDIFF(day,CreatedOn,@DATE) =@DueDays ">
        <SelectParameters>
        <asp:Parameter Name="DATE" DefaultValue="2014-05-05" Type="DateTime" />
        <asp:Parameter Name="DueDays"  DefaultValue="-1" />
        <asp:Parameter Name="status"  DefaultValue="Inprocess" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

