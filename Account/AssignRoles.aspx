<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="AssignRoles.aspx.vb" Inherits="Account_AssignRoles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 114px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:Panel runat="server" ID="PnlMsg"  Visible="false"><center><asp:Label ID="lblMsg" runat="server" Text=""></center></asp:Label></asp:Panel>
      <h1>
                    Assign Roles</h1>
    <table class="style1">
        
        <tr>
            <td class="style2">
                Select User</td>
            <td>
                <asp:ListBox ID="LstUserList" runat="server" Height="23px" Width="173px" 
                    AutoPostBack="True" Rows="1">
                </asp:ListBox>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                Roles</td>
            <td>
                  <asp:Repeater ID="UsersRoleList" runat="server"> 
           <ItemTemplate>
           <asp:CheckBox runat="server" ID="RoleCheckBox" AutoPostBack="false"                     Text='<%# Container.DataItem %>' /><br />
           </ItemTemplate>              
              </asp:Repeater>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td>
                  <asp:Button ID="btnAssignRoles" runat="server" Text="Assign Roles" />
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>

</asp:Content>

