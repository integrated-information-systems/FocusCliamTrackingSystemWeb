<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" ValidateRequest = "false" AutoEventWireup="false" CodeFile="NewEmail.aspx.vb" Inherits="Business_NewEmail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 79px;
        }
        .style3
        {
            width: 362px;
        }
        .style4
        {
            width: 127px;
        }
    </style>
     <script src="../Scripts/tinymce/tinymce.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        tinymce.init({
            selector: "textarea.inputtext", menubar: false, plugins: "paste",
            paste_data_images: true

        });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>New Email</h1>
   <asp:Panel runat="server" HorizontalAlign="Center" ><asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Green"></asp:Label> </asp:Panel>
    <table class="style1">
        <tr>
            <td class="style2">
                Tracking ID:</td>
            <td class="style3">
             <asp:Label runat="server" ID="lblTrackingID" ></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                From:</td>
            <td class="style3">
                <asp:DropDownList ID="ddlFrom" runat="server">
                    <asp:ListItem>Customer</asp:ListItem>
                    <asp:ListItem>Manufacturer</asp:ListItem>
                    <asp:ListItem>Focus</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                To:</td>
            <td class="style3">
                <asp:DropDownList ID="ddlTo" runat="server">
                    <asp:ListItem>Customer</asp:ListItem>
                    <asp:ListItem>Manufacturer</asp:ListItem>
                    <asp:ListItem>Focus</asp:ListItem>
                </asp:DropDownList><asp:CustomValidator ID="CustomddlToValidator" runat="server" ControlToValidate="ddlTo" OnServerValidate="CheckEqual" ValidateEmptyText="true" ValidationGroup="Group1" ForeColor="Red"
                    ErrorMessage="From and To cannot be the same" Display="Dynamic">From and To cannot be the same</asp:CustomValidator>
        
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                Date:</td>
            <td class="style3">
                <asp:TextBox ID="txtDate" runat="server" ReadOnly="True" Width="99px"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    <div>
        <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" 
            CssClass="inputtext"  Rows="20" ></asp:TextBox></div>
    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red"  ErrorMessage="Cannot be empty" ValidationGroup="Group1" ControlToValidate="txtBody">Cannot be empty</asp:RequiredFieldValidator>--%>
            
    <table class="style1">
        <tr>
            <td class="style4">
               </td>
            <td>
                <asp:DropDownList ID="ddlReminder" runat="server" visible="False">
                    <asp:ListItem>0</asp:ListItem>
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style4"><br />
                <asp:Button ID="btnSave" runat="server" Text="Save" ValidationGroup="Group1" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" />
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>

</asp:Content>

