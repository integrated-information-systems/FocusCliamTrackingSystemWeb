<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="ManageUsers.aspx.vb" Inherits="ManageUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 594px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Manage Users</h1>
    <div>
    <div style="float:left">
        <table>
        <td colspan="2">           
                <asp:ValidationSummary ID="RegisterUserValidationSummary" runat="server" 
                    CssClass="failureNotification" ValidationGroup="RegisterUserValidationGroup" />
                    <asp:Label ID="lblMsg" runat="server" Visible="False"></asp:Label>
                </td>
        <tr>
            <td>           <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                </td>
            <td class="style2">
  
                                        <asp:TextBox ID="UserName" runat="server" ReadOnly="True" Width="220px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" CssClass="failureNotification" 
                        ErrorMessage="Please Select User." ToolTip="Please Select User." ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
            </td>

        </tr>
        <tr>
            <td>           
                <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label>
                </td>
            <td class="style2">
  
                                        <asp:TextBox ID="Email" runat="server" Width="220px" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                                            ControlToValidate="Email" CssClass="failureNotification" 
                                            ErrorMessage="E-mail is required." ToolTip="E-mail is required." 
                                            ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
            </td>

        </tr>
        <tr>
            <td>
            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">
                        Password:</asp:Label></td>
            <td class="style2">
                 <asp:TextBox ID="Password" runat="server" TextMode="Password" Width="220px"></asp:TextBox></asp:RequiredFieldValidator><asp:RegularExpressionValidator    CssClass="failureNotification" Display = "static"  ControlToValidate = "Password" ID="RegularExpressionValidator3" ValidationGroup="RegisterUserValidationGroup" ValidationExpression = "^[\s\S]{6,}$" runat="server" ErrorMessage="Min 6 and characters required.">*</asp:RegularExpressionValidator></td>
        </tr>
        <tr>
            <td>
                 <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">
                        Confirm Password:</asp:Label></td>
            <td class="style2">
                  <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password" 
                      Width="220px"></asp:TextBox><asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password"
                        ControlToValidate="ConfirmPassword"    CssClass="failureNotification" Display="Dynamic" ErrorMessage="confirm password doesn't match with Password"
                        ValidationGroup="RegisterUserValidationGroup">*</asp:CompareValidator></td>
        </tr>
        <tr>
            <td>
             <asp:Label ID="CoutnryLabel" runat="server" AssociatedControlID="LstCountry">
                       Country:</asp:Label></td>
            <td class="style2">
            <asp:ListBox ID="LstCountry" runat="server" DataSourceID="SqlDataSource1" 
                        DataTextField="country" DataValueField="id" 
                        SelectionMode="Multiple" Width="225px">                        
                        </asp:ListBox>
                                <asp:RequiredFieldValidator   ID="RequiredFieldValidatorCountry" runat="server" ControlToValidate="LstCountry" 
                                     CssClass="failureNotification" ErrorMessage="Country is required."
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                                         <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:Custom_DB_ConnectionString %>" 
                        SelectCommand="SELECT [ID], [Country] FROM [Country]">
                    </asp:SqlDataSource></td>
        </tr>
         <tr>
            <td>           
                <asp:Label ID="lblCompanyName" runat="server" AssociatedControlID="txtCompanyName">Company:</asp:Label>
                </td>
            <td class="style2">  
                                        <asp:TextBox ID="txtCompanyName" runat="server" Width="220px" ></asp:TextBox>                                   
            </td>

        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td class="style2">
                <asp:Button ID="btnSubmit" runat="server" Text="Update"  ValidationGroup="RegisterUserValidationGroup" />
            </td>
        </tr>
    </table>
    </div>
    <div >
    <table class="style1" style="border-spacing: 10px;">
        <tr>
            

            <td rowspan="7" valign="top">
                <asp:GridView ID="UsersList" runat="server" AutoGenerateColumns="false" 
                    AutoGenerateDeleteButton="True" AutoGenerateSelectButton="True" CellPadding="4" 
                    ForeColor="#333333" GridLines="None" onrowdeleting="UsersList_RowDeleting">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
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
                    <Columns>
                        <asp:BoundField DataField="Username" HeaderText="Username" ReadOnly="true" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                    </Columns>
                </asp:GridView>
            </td>
        
           
        </tr>
  </table></div>

    </div>
</asp:Content>

