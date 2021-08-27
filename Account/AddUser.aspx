<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="AddUser.aspx.vb" Inherits="Account_AddUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <asp:CreateUserWizard ID="CreateUserWizard1" runat="server"  LoginCreatedUser="False"  ContinueDestinationPageUrl="~/Account/AddUser.aspx">
        <LayoutTemplate>
            <asp:PlaceHolder ID="wizardStepPlaceholder" runat="server"></asp:PlaceHolder>
            <asp:PlaceHolder ID="navigationPlaceholder" runat="server"></asp:PlaceHolder>
        </LayoutTemplate>
        <WizardSteps>
            <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
            <ContentTemplate>
                  <h2>
                 
                      Add User
                    </h2>
               
                    <span class="failureNotification">
                        <asp:Literal ID="ErrorMessage" runat="server"></asp:Literal>
                    </span>
                    <asp:ValidationSummary ID="RegisterUserValidationSummary" runat="server" CssClass="failureNotification" 
                         ValidationGroup="RegisterUserValidationGroup"/>
                         <div class="accountInfo">
                        <fieldset class="register" style="border-width:0px;padding-top:0px;">                     
                            <p>
                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" 
                                     CssClass="failureNotification" ErrorMessage="User Name is required." ToolTip="User Name is required." 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label>
                                <asp:TextBox ID="Email" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email" 
                                     CssClass="failureNotification" ErrorMessage="E-mail is required." ToolTip="E-mail is required." 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" 
                                     CssClass="failureNotification" ErrorMessage="Password is required." ToolTip="Password is required." 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">Confirm Password:</asp:Label>
                                <asp:TextBox ID="ConfirmPassword" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ControlToValidate="ConfirmPassword" CssClass="failureNotification" Display="Dynamic" 
                                     ErrorMessage="Confirm Password is required." ID="ConfirmPasswordRequired" runat="server" 
                                     ToolTip="Confirm Password is required." ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword" 
                                     CssClass="failureNotification" Display="Dynamic" ErrorMessage="The Password and Confirmation Password must match."
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:CompareValidator>
                            </p>
                            <p>
                               <asp:Label ID="lblCountry" runat="server" AssociatedControlID="LstCountry">User Name:</asp:Label>

                                <asp:ListBox ID="LstCountry" runat="server" DataSourceID="SqlDataSource1" 
                        DataTextField="country" DataValueField="id" 
                        SelectionMode="Multiple">                        
                        </asp:ListBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCountry" runat="server" ControlToValidate="LstCountry" 
                                     CssClass="failureNotification" ErrorMessage="Country is required." ToolTip="Country is required." 
                                     ValidationGroup="RegisterUserValidationGroup">*</asp:RequiredFieldValidator>
                                         <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:Custom_DB_ConnectionString %>" 
                        SelectCommand="SELECT [ID], [Country] FROM [Country]">
                    </asp:SqlDataSource>
                            </p>
                             <p>
                                <asp:Label ID="lblCompanyName" runat="server" AssociatedControlID="txtCompanyName">Company:</asp:Label>
                                <asp:TextBox ID="txtCompanyName" runat="server" CssClass="textEntry"></asp:TextBox>                              
                            </p>
                        </fieldset>
                        <p class="submitButton">
                            <asp:Button ID="CreateUserButton" runat="server" CommandName="MoveNext" Text="Create User" 
                                 ValidationGroup="RegisterUserValidationGroup"/>
                        </p>
                    </div>
            </ContentTemplate>
            <CustomNavigationTemplate></CustomNavigationTemplate>
            </asp:CreateUserWizardStep>
            <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
             <ContentTemplate>
        <table border="0" style="font-size: 100%; font-family: Verdana" id="TABLE1" >
            <tr>
                <td align="center" colspan="2" style="font-weight: bold; color: Green; height: 18px;">
                    <b>New User Added</b></td>
            </tr>
            <tr>
                <td  style="color: Green;">
                    New user account has been successfully created.<br />
                    <br />       
            </tr>
            <tr>
                <td align="right" colspan="2">
                    &nbsp;<asp:Button ID="ContinueButton" runat="server" CausesValidation="False" CommandName="Continue"
                      Text="Continue" ValidationGroup="CreateUserWizard1" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
            </asp:CompleteWizardStep>
        </WizardSteps>
    </asp:CreateUserWizard>
</asp:Content>

