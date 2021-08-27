<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Manufacturer.aspx.vb" Inherits="Business_Manufacturer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 121px;
        }
        .style3
        {
            text-align: right;
        }
    </style>
       <link href="../Styles/jquery-ui.css" 
            rel="stylesheet" type="text/css"/>       
       <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript" language="javascript" ></script>
       <script src="../Scripts/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../Scripts/JScript.js" type="text/javascript" language="javascript" ></script>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Claim Info</h1>
    <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Center"><asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Green"></asp:Label>  </asp:Panel>
    <table class="style1">        
        <tr>
            <td>
                <table class="style1">
                    <tr>
                        <td class="style2">
                            End Customer Name</td>
                        <td>
                            <asp:TextBox ID="txtEndCustomerName" runat="server" Width="457px" 
                                MaxLength="250" ReadOnly="True"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorEndCustomerName" ForeColor="Red" ControlToValidate="txtEndCustomerName" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>          
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            Person In charge</td>
                        <td>
                            <asp:TextBox ID="txtPersonincharge" runat="server" Width="457px" 
                                MaxLength="250" ReadOnly="True"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorPersonincharge" ForeColor="Red" ControlToValidate="txtPersonincharge" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>          
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            Email Address</td>
                        <td>
                            <asp:TextBox ID="txtEmailAddress" runat="server" Width="457px" MaxLength="250" 
                                ReadOnly="True"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmailAddress" ForeColor="Red" ControlToValidate="txtEmailAddress" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>          
                        </td>
                    </tr>
                    <tr>
                        <td class="style2">
                            Tel No</td>
                        <td>
                            <asp:TextBox ID="txtTelno" runat="server" Width="194px" MaxLength="20" 
                                ReadOnly="True"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorTelno" ForeColor="Red" ControlToValidate="txtTelno" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>          
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorTelno" ForeColor="Red" ControlToValidate="txtTelno" runat="server" ErrorMessage="*"   ValidationExpression="\d{1,}" ValidationGroup="SubmitForm" >*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Brand</td>
                        <td>
                            <asp:TextBox ID="txtBrand" runat="server" MaxLength="25" ReadOnly="True"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Machine Model No</td>
                        <td>
                            <asp:TextBox ID="txtMachineModelNo" runat="server" MaxLength="25" ReadOnly="True"></asp:TextBox></td>
                    </tr>  
                </table>
            </td>
            <td>
                <table class="style1">
                    <tr>
                        <td>
                            Claim ID</td>
                        <td>
                            <asp:TextBox ID="txtClaimId" runat="server" ReadOnly="True" Wrap="False" 
                                style="text-align:right" Width="70px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Date</td>
                        <td>
                            <asp:TextBox ID="txtDate" runat="server" ReadOnly="True" 
                                style="text-align:left" Width="70px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Country</td>
                        <td>
                            <asp:TextBox ID="txtCountry" runat="server" ReadOnly="True" style="text-align:left" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Status</td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server" Enabled="False">
                             
                                   <asp:ListItem Selected="True">Open</asp:ListItem>
                                <asp:ListItem>Inprocess</asp:ListItem>
                                <asp:ListItem>Complete</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr><td colspan="2" align="right">
            &nbsp;</td></tr>
        <tr>
            <td colspan="2" align="center">
                <asp:GridView ID="ClaimItemList" runat="server" CellPadding="4"  AutoGenerateColumns="false"
        EmptyDataText="No Records Added" ForeColor="#333333" GridLines="None" 
        ShowHeaderWhenEmpty="True" Width="100%" AutoGenerateSelectButton="True" 
        HorizontalAlign="Center" ViewStateMode="Enabled"  >
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField HeaderText="SNo"   >
                            <ItemTemplate>
                                <asp:TextBox runat="server" Width="25px" id="SNo" Text='<%# Bind("SNo") %>'  ReadOnly="true" >  </asp:TextBox>                        
                            </ItemTemplate>
                        </asp:TemplateField>                                          
                        <asp:TemplateField HeaderText="Invoice Date" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="InvDate" Text='<%# Bind("InvDate") %>'  >  </asp:TextBox> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorInvDate" Display="Dynamic" ForeColor="Red" ControlToValidate="InvDate" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>
                            </ItemTemplate>
                        </asp:TemplateField>          
                         <asp:TemplateField HeaderText="Invoice No" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="InvNo" Text='<%# Bind("InvNo") %>'  >  </asp:TextBox>                        
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorInvNo" ForeColor="Red" ControlToValidate="InvNo" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>
                            </ItemTemplate>
                        </asp:TemplateField>    
                         <asp:TemplateField HeaderText="Qty" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="Qty" Text='<%# Bind("Qty") %>' >  </asp:TextBox>              
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorQty" ForeColor="Red" ControlToValidate="Qty" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>          
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorQty" ForeColor="Red" ControlToValidate="Qty" runat="server" ErrorMessage="*"   ValidationExpression="\d{1,}" ValidationGroup="SubmitForm" >*</asp:RegularExpressionValidator>
                            </ItemTemplate>
                        </asp:TemplateField>    
                         <%--<asp:TemplateField HeaderText="Brand">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="Brand" Text='<%# Bind("Brand") %>' Width="95px" MaxLength="25"  >  </asp:TextBox>                        
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorBrand" ForeColor="Red" ControlToValidate="Brand" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>          
                            </ItemTemplate>
                        </asp:TemplateField>    
                         <asp:TemplateField HeaderText="Machine Model No">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="ModelNo" Text='<%# Bind("ModelNo") %>' Width="95px" MaxLength="25"  >  </asp:TextBox>             
                               <asp:RequiredFieldValidator ID="RequiredFieldValidatorModelNo" ForeColor="Red" ControlToValidate="ModelNo" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>                     
                            </ItemTemplate>
                        </asp:TemplateField>--%>    
                         <asp:TemplateField HeaderText="Machine Serial No">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="SerialNo" Text='<%# Bind("SerialNo") %>' Width="95px" MaxLength="25" >  </asp:TextBox>                        
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorSerialNo" ForeColor="Red" ControlToValidate="SerialNo" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>                     
                            </ItemTemplate>
                        </asp:TemplateField>    
                         <asp:TemplateField HeaderText="Claim Qty">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="ClaimQty" Text='<%# Bind("ClaimQty") %>' Width="30px" MaxLength="25">  </asp:TextBox>                        
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorClaimQty" ForeColor="Red" ControlToValidate="ClaimQty" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatorClaimQty" ForeColor="Red" ControlToValidate="ClaimQty" runat="server" ErrorMessage="*"   ValidationExpression="\d{1,}" ValidationGroup="SubmitForm" >*</asp:RegularExpressionValidator>
                            </ItemTemplate>
                        </asp:TemplateField>    
                         <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="Particulars" Text='<%# Bind("Particulars") %>' Width="100px" MaxLength="250" >  </asp:TextBox>    
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorParticulars" ForeColor="Red" ControlToValidate="Particulars" runat="server" ErrorMessage="*" ValidationGroup="SubmitForm">*</asp:RequiredFieldValidator>                                         
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Declare Value"  >
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="DeclareValue" Text='<%# Bind("DeclareValue") %>'  >  </asp:TextBox>                                    
                            </ItemTemplate>
                        </asp:TemplateField>   
                        <asp:TemplateField HeaderText="Declare Description"  >
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="DeclareDescription" Text='<%# Bind("DeclareDescription") %>'  >  </asp:TextBox>                                    
                            </ItemTemplate>
                        </asp:TemplateField>      
                          <asp:TemplateField HeaderText="Tpt Mode">
                            <ItemTemplate>
                                <%--<asp:TextBox runat="server" id="Claimmode" Text='<%# Bind("Claimmode") %>' Width="75px" MaxLength="25" >  </asp:TextBox>--%>
                                <asp:DropDownList ID="Claimmode" runat="server" DataSourceID="SqlDataSource1" DataValueField="ModeName" DataTextField="ModeName" AppendDataBoundItems="true">
                                <asp:ListItem Text="Select" Value=""></asp:ListItem>                                 
                                </asp:DropDownList>
                                                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:Custom_DB_ConnectionString %>" 
                                SelectCommand="SELECT [ModeName], [TransModeId] FROM [TransportMode]">
                            </asp:SqlDataSource>
                            </ItemTemplate>
                        </asp:TemplateField>   
                          <asp:TemplateField HeaderText="Completion Date">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="ClaimDate" Text='<%# Bind("ClaimDate") %>' Width="75px" MaxLength="25"  >  </asp:TextBox>                                    
                            </ItemTemplate>
                        </asp:TemplateField>   
                          <asp:TemplateField HeaderText="Claim Reference">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="ClaimReference" Text='<%# Bind("ClaimReference") %>'  Width="200px" MaxLength="25" >  </asp:TextBox>                                    
                            </ItemTemplate>
                        </asp:TemplateField>   
                          <asp:TemplateField HeaderText="Part No">
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="PartNo" Text='<%# Bind("PartNo") %>'  Width="75px" MaxLength="25"  >  </asp:TextBox>                                    
                            </ItemTemplate>
                        </asp:TemplateField>                             
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
              
            </td>
        </tr>       
    </table>

    <table class="style1">
        <tr>
            <td valign="top">
                <table class="style1">
                    <tr>
                        <td valign="top">
                            Upload Photo</td>
                        <td>
                            <asp:FileUpload ID="PhotosUpload" runat="server"    />
                            <asp:Button ID="btnUpload" runat="server"   Text="Upload" 
                                OnClientClick = "return ValidateFile()" Enabled="False" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblUploadMsg" runat="server" ></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:BulletedList ID="UploadedFiles" runat="server" BulletStyle="Numbered" DisplayMode="LinkButton"
                                ViewStateMode="Enabled" Enabled="False">
                            </asp:BulletedList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">
                            Youtube Links</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style3">
                            1</td>
                        <td>
                            <asp:TextBox ID="txtYoutubeLink1" runat="server" Width="297px" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">
                            2</td>
                        <td>
                            <asp:TextBox ID="txtYoutubeLink2" runat="server" Width="297px" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="style3">
                            3</td>
                        <td>
                            <asp:TextBox ID="txtYoutubeLink3" runat="server" Width="297px" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table class="style1" style="border: 1px solid #DDE4EC;">
                    <tr>
                        <td colspan="2">
                            <strong>For Delivery Purpose</strong></td>
                    </tr>
                    <tr>
                        <td>
                            Ship To Company</td>
                        <td>
                            <asp:TextBox ID="txtShipToCompany" runat="server" Width="457px" MaxLength="250" 
                                ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Ship to Address</td>
                        <td>
                            <asp:TextBox ID="txtShipToAddress" style="resize:none; margin-top: 0px;" 
                                runat="server" Rows="5" TextMode="MultiLine" 
                                Width="261px" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Contact Person</td>
                        <td>
                            <asp:TextBox ID="txtShipToContactPerson" runat="server" MaxLength="100" 
                                ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Tel No</td>
                        <td>
                            <asp:TextBox ID="txtShipToTelNo" runat="server" Width="184px" MaxLength="20" 
                                ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    </table>
            </td>
        </tr>
    </table>
    <asp:Button ID="btnSave" runat="server" Text="Save" 
        ValidationGroup="SubmitForm" Visible="True" />
                            <asp:Button ID="btnClear" runat="server" 
    Text="Clear" Visible="True" />
                        <asp:Button ID="btnExporttoExcel" runat="server" 
        Text="Export to Excel" />
                        </asp:Content>

